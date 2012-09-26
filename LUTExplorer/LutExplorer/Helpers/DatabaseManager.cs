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
        private string achievementTable = "achievements";
        private string treasureTable = "treasures";
        
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
            tableClient.CreateTableIfNotExist(achievementTable);
            tableClient.CreateTableIfNotExist(treasureTable);
            
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

            // TL;DR some bodging has been done. 

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
                // data here is for debugging, no real reason to store it
                DataServiceResponse data = serviceContext.SaveChangesWithRetries();
        }

        /// <summary>
        /// A very untested method to store achivements into a seperate table.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="player"></param>
        public void SaveAchievement(string name, PlayerEntity player)
        {
            serviceContext.AddObject(achievementTable, new AchievementEntity(player.RowKey, name));
            serviceContext.SaveChangesWithRetries();
        }

        public void SaveTreasure(string name, PlayerEntity player)
        {
            serviceContext.AddObject(treasureTable, new TreasureEntity(player.RowKey, name)); // rowkey = ucid
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
        /// THIS IS the one for some reason we're using to do player searches
        /// Dunno why. Dillygaf.
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


                    if (listEntities.ToList().Count() > 0)
                    {
                        //return listEntities.FirstOrDefault();

                        PlayerEntity player = listEntities.FirstOrDefault();

                        // query the treasure database too
                        //IQueryable<TreasureEntity> 
                        IQueryable<TreasureEntity> treasureEntities = (from e in serviceContext.CreateQuery<TreasureEntity>(treasureTable)
                                                                       where e.PartitionKey == player.RowKey // partition key for treasure table == plid
                                                                       select e);
                        // and append the treasure dictionary accordingly
                        //
                        //right, because this is the gayest thing ever I'm going to need to just nest these try-catch blocks here
                        // since the db query doesn't return null or anything, instead it goes tits up.

                        try
                        {
                            foreach (TreasureEntity t in treasureEntities)
                            {
                                if (player.TreasureChest == null)
                                {
                                    player.TreasureChest = new Dictionary<int, DateTime>();
                                }

                                if (!player.TreasureChest.ContainsKey(Convert.ToInt32(t.RowKey)))
                                    player.TreasureChest.Add(Convert.ToInt32(t.RowKey), Convert.ToDateTime(t.Timestamp));

                            }
                        }
                        catch (Exception)
                        { 
                            // if you are wondering why this is already the third nested try-catch block within this method...
                            // it is because the IQueryable object doesn't have a method in it to tell if it contains anything or not...
                            // and it throws exception if you try and convert it to a list :|
                        }

                        // Then we try and query the achievement table
                        // god help us all

                        IQueryable<AchievementEntity> achievementEntities = (from e in serviceContext.CreateQuery<AchievementEntity>(achievementTable)
                                                                       where e.PartitionKey == player.RowKey // partition key for achievement table == plid
                                                                       select e);
                        try
                        {
                            foreach (AchievementEntity t in achievementEntities)
                            {
                                if (player.Achievements == null)
                                {
                                    player.Achievements = new Dictionary<string, DateTime>();
                                }

                                if (!player.Achievements.ContainsKey(Convert.ToString(t.RowKey)))
                                    player.Achievements.Add(Convert.ToString(t.RowKey), Convert.ToDateTime(t.Timestamp));

                            }
                        }
                        catch (Exception)
                        {
                         
                        }
 


                        return player;
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