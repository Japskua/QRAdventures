using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LutExplorer.Helpers.DatabaseEntities;

namespace LutExplorer.Helpers
{
    /// <summary>
    /// The GameManager class contains all the functions for checking
    /// player advancement in the game
    /// </summary>
    public class GameManager
    {

        public GameManager() { }

        /// <summary>
        /// Checks if the treasure found is the correct one the player is looking for
        /// </summary>
        /// <param name="currentSearchedTreasure">The current treasure number searched by the player</param>
        /// <param name="foundTreasure">The found treasure number</param>
        /// <returns>
        /// True if the numbers match
        /// False, if the treasure was a wrong one
        /// </returns>
        public bool CheckIfCorrectTreasure(int currentSearchedTreasure, int foundTreasure)
        {
            // If the treasure matches with the target
            if (currentSearchedTreasure == foundTreasure)
            {
                // Return true
                return true;
            }

            // Else, return false
            return false;

        }

        /// <summary>
        /// Gets the treasure defined and stores it to the player treasure chest
        /// </summary>
        /// <param name="player">The player entity in question</param>
        /// <param name="treasure">The treasure that was found</param>
        /// <param name="nextTreasure">The next treasure to be attained</param>
        public void GetTreasure(PlayerEntity player, int treasure, int nextTreasure)
        {
            // If the treasure chest does not exist, create it (just to be sure)
            if (player.TreasureChest == null)
            {
                player.TreasureChest = new Dictionary<int, DateTime>();
            }

            // Check if the player does not have the treasure yet
            if (!player.TreasureChest.ContainsKey(treasure))
            {
                // Add the treasure to the treasure chest
                player.TreasureChest.Add(treasure, DateTime.Now);
                // Set the next treasure to be the current one that is searched
                player.CurrentSearchedTreasure = nextTreasure;
                // Save the changes
                DatabaseManager.Instance.SavePlayer(player);
            }

        }

        /// <summary>
        /// Gets the current searched treasure
        /// </summary>
        /// <param name="player">The player in question</param>
        /// <returns>The number of the treasure currently looked for</returns>
        public int GetCurrentSearchedTreasure(PlayerEntity player)
        {
            // Return the value
            return player.CurrentSearchedTreasure;
        }

        /// <summary>
        /// Gets the achievement in question if the player is allowed to attain
        /// achievements and saves the information immediately to the database
        /// </summary>
        /// <param name="player">The player in question</param>
        /// <param name="achievement">The achievement gained</param>
        public void GetAchievement(PlayerEntity player, string achievement)
        {
            // Check if the player is achievement player or not
            if (player.PartitionKey.Equals(PlayerEntity.UserType.Regular.ToString()))
            {
                // Regular player, just return from here
                return;
            }

            // If the achievements list does not exist, create it (just to be sure)
            if (player.Achievements == null)
            {
                player.Achievements = new Dictionary<string,DateTime>();
            }

            // Now, save the achivement to the player information
            player.Achievements.Add(achievement, DateTime.Now);

            // And save the values to the database
            DatabaseManager.Instance.SavePlayer(player);


        }

    }
}