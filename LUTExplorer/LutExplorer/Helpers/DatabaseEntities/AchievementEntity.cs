using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace LutExplorer.Helpers.DatabaseEntities
{
    /// <summary>
    /// User entity class portrays the saved entry in the database
    /// </summary>
    public class AchievementEntity : TableServiceEntity
    {


        /// <summary>
        /// The user entity constructor
        /// </summary>
        /// <param name="userType">The type which the user is</param>
        /// <param name="userId">The Id of the user in question</param>
        public AchievementEntity(string userId, string achievementName)
        {
            // Set the partition and row keys to match the user type and id
            this.PartitionKey = userId;
            this.RowKey = achievementName;


        }

        public AchievementEntity() { }

    }
}