using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;
using LutExplorer.Helpers.DatabaseEntities;
using System.Diagnostics;

namespace LutExplorer.Helpers
{
    /// <summary>
    /// Database Manager takes cares of all the actions that are happening with the databases
    /// </summary>
    public sealed class DatabaseManager
    {
        private static volatile DatabaseManager instance;
        private static object syncRoot = new object();


        // The table name
        private string tableName = "lutexplorer";
        // The required classes for connecting to the service
        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private TableServiceContext serviceContext;

        private DatabaseManager() 
        {
            // Retrieve the storage account from the connection string
            storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client
            tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist
            tableClient.CreateTableIfNotExist(tableName);

            // Get the data service context
            serviceContext = tableClient.GetDataServiceContext();
        
        }

        /// <summary>
        /// This is the thread-safe Instantiator for the singleton
        /// database manager
        /// </summary>
        public static DatabaseManager Instance
        {
            get
            {
                // If instance does not exist
                if (instance == null)
                {
                    // Lock
                    lock (syncRoot)
                    {
                        // If still null
                        if (instance == null)
                        {
                            // Return the manager
                            instance = new DatabaseManager();
                        }
                    }
                }
                // Return the instance
                return instance;
            }
        }


        #region Functions

        /// <summary>
        /// Saves the player entity to the storage database
        /// </summary>
        /// <param name="playerEntity">The player entity to be saved</param>
        public void SavePlayer(PlayerEntity playerEntity)
        {
            // First, check if the entity exists in the database or not
            PlayerEntity checker = FindPlayerEntity(playerEntity);
            if (checker != null)
            {
                // Just update the value
                checker.CurrentSearchedTreasure = playerEntity.CurrentSearchedTreasure;
                checker.Achievements = playerEntity.Achievements;
                checker.TreasureChest = playerEntity.TreasureChest;
            }
            else
            {
                // Otherwise, save the entity
                serviceContext.AddObject(tableName, playerEntity);
            }

            // And finally, save the changes
            serviceContext.SaveChangesWithRetries();
        }


        /// <summary>
        /// Searches the table storage for the given player entity
        /// </summary>
        /// <param name="playerEntity">The player entity to be searched for</param>
        /// <returns>Found player entity</returns>
        public PlayerEntity FindPlayerEntity(PlayerEntity playerEntity)
        {
            try
            {
                // Get the entity with given values
                IQueryable<PlayerEntity> listEntities = (from e in serviceContext.CreateQuery<PlayerEntity>(tableName)
                                                        // where e.PartitionKey == playerEntity.PartitionKey && e.RowKey == playerEntity.RowKey
                                                         where e.PartitionKey == playerEntity.PartitionKey
                                                         select e);

                if (listEntities.ToList().Count > 0)
                {
                    return listEntities.FirstOrDefault();
                }

                // Otherwise return null
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }



        public void TestAddPlayer()
        {
            // Create the player
            PlayerEntity player = new PlayerEntity(PlayerEntity.UserType.Regular, 1);

            // And save it to the storage
            serviceContext.AddObject(tableName, player);

            // Submit the operation to the table service
            serviceContext.SaveChangesWithRetries();

            Trace.WriteLine("Saved the information!");

        }

        public PlayerEntity TestRetrievePlayer()
        {

            CloudTableQuery<PlayerEntity> playerQuery = (from e in serviceContext.CreateQuery<PlayerEntity>(tableName)
                                                         where e.PartitionKey == PlayerEntity.UserType.Regular.ToString()
                                                         && e.RowKey == "1"
                                                         select e).AsTableServiceQuery<PlayerEntity>();

            try
            {

                // Loop through the results, displaying information about the entity
                foreach (PlayerEntity player in playerQuery)
                {
                    Trace.WriteLine(player.PartitionKey + " " + player.RowKey + "-" +
                                    ": Current Treasure: " + player.CurrentSearchedTreasure);

                }
            }
            catch
            {
                // ignore
            }


            return playerQuery.FirstOrDefault();
        }

        #endregion Fuctions

    }
}