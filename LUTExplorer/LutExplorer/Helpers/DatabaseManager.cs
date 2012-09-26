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

        public void SaveNextTreasure(PlayerEntity playerEntity, int nextTreasure)
        {

            // Find the player entity  - !! what the fuck for?!?
            //PlayerEntity pe = FindPlayerEntity(playerEntity);  
            
            if (playerEntity != null)
            {
                // Update the value
                playerEntity.CurrentSearchedTreasure = nextTreasure;
                serviceContext.UpdateObject(playerEntity);
                serviceContext.SaveChangesWithRetries();
            }
        }


        /// <summary>
        /// Saves the player entity to the storage database
        /// </summary>
        /// <param name="playerEntity">The player entity to be saved</param>
        public void SavePlayer(PlayerEntity playerEntity)
        {
            // Basically, calling the query later on will b0rk the service context
            // therefore we should update it before
            // but that can't be done before the player is saved to the db in the first place.

            // TL;DR DO NOT USE FOR UPDATING ANYTHING, IT WILL NOT WORK!

            //serviceContext.UpdateObject(playerEntity);
            //serviceContext.SaveChangesWithRetries();

            // now this is the worst idea I've ever had. So fuck me.
            // Salvage all the important data from the entity because otherwise the query will destroy it.
            int currentSearchedTreasure = playerEntity.CurrentSearchedTreasure;
            int route = playerEntity.CurrentRoute;
            Dictionary<int, DateTime> treasureChest = playerEntity.TreasureChest;
            Dictionary<string, DateTime> achievements = playerEntity.Achievements;

            // then do the query.
                // First, check if the entity exists in the database or not
                //PlayerEntity checker = FindPlayerEntity(playerEntity);
                PlayerEntity checker = FindPlayerEntity(playerEntity.PartitionKey, playerEntity.RowKey);
                if (checker != null)
                {
                    // Just update the value
                    checker.CurrentSearchedTreasure = currentSearchedTreasure;
                    checker.CurrentRoute = route;
                    checker.Achievements = achievements;
                    checker.TreasureChest = treasureChest;
                    serviceContext.UpdateObject(checker);
                }
                else
                {
                    // Otherwise, save the entity
                    serviceContext.AddObject(tableName, playerEntity);
                }

                // And finally, save the changes
                DataServiceResponse data = serviceContext.SaveChangesWithRetries();
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
                                                         where e.PartitionKey == playerEntity.PartitionKey && e.RowKey == playerEntity.RowKey
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

        /// <summary>
        /// Finds the player entity by the given player type and id
        /// which are given as strings (usually this is the case when
        /// retrieving the information from the cookies)
        /// </summary>
        /// <param name="playerType">The type fo the player</param>
        /// <param name="id">The player id</param>
        /// <returns>PlayerEntity if found</returns>
        public PlayerEntity FindPlayerEntity(string playerType, string id)
        {
            try
            {
                // Get the entity with given values
                IQueryable<PlayerEntity> listEntities = (from e in serviceContext.CreateQuery<PlayerEntity>(tableName)
                                                         where e.PartitionKey == playerType && e.RowKey == id
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