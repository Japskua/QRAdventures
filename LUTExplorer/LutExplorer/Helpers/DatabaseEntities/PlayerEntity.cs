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
    public class PlayerEntity : TableServiceEntity
    {

        /// <summary>
        /// The different user types that can exist
        /// Ahicevements - Plays with achievements
        /// Regular - Plays without achievements
        /// </summary>
        public enum UserType
        {
            Achievements,
            Regular,
        };

        /// <summary>
        /// The user entity constructor
        /// </summary>
        /// <param name="userType">The type which the user is</param>
        /// <param name="userId">The Id of the user in question</param>
        public PlayerEntity(UserType userType, int userId)
        {
            // Set the partition and row keys to match the user type and id
            this.PartitionKey = userType.ToString();
            this.RowKey = userId.ToString();

            // Create the basic values
            TreasureChest = new Dictionary<int, DateTime>();
            Achievements = new Dictionary<string, DateTime>();
            CurrentSearchedTreasure = 1;


        }

        public PlayerEntity() { }

        /// <summary>
        /// The current treasure the player is supposed to be
        /// searching for
        /// </summary>
        public int CurrentSearchedTreasure { get; set; }

        /// <summary>
        /// The Treasure Chest holds all the treasures the player
        /// has achieved and the times when those have been gained
        /// </summary>
        public Dictionary<int, DateTime> TreasureChest { get; set; }

        /// <summary>
        /// The Achievements holds the information of what player
        /// has gained and when
        /// </summary>
        public Dictionary<string, DateTime> Achievements { get; set; }
    }
}