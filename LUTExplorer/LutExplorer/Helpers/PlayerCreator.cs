using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LutExplorer.Helpers.DatabaseEntities;

namespace LutExplorer.Helpers
{
    public class PlayerCreator
    {

        private int achievementLikelihood = 50;

        public PlayerEntity NewPlayerEntity;

        public PlayerCreator() 
        {
            NewPlayerEntity = new PlayerEntity(CreateUserType(), CreatePlayerId());
        }

        /// <summary>
        /// Creates the user type according to the likelihood
        /// </summary>
        /// <returns>The type of game designated for the player in question</returns>
        public PlayerEntity.UserType CreateUserType()
        {
            // Create a random value
            Random random = new Random(DateTime.Now.GetHashCode());

            // If the value is bigger than achievement likelihood value
            if (random.Next(1, 100) > achievementLikelihood)
            {
                // Return achievement type play
                return PlayerEntity.UserType.Achievements;
            }
            else
            {
                // Otherwise, return regular play
                return PlayerEntity.UserType.Regular;
            }


        }

        /// <summary>
        /// Creates the player ID for the player randomly, 
        /// based on a hash integer created from current datetime value
        /// </summary>
        /// <returns>A pseudo-random integer value</returns>
        public int CreatePlayerId()
        {
            int playerId = 0;

            playerId += DateTime.Now.GetHashCode();

            return playerId;
        }

        public bool TryCreateDatabasePlayerEntry(int playerId, PlayerEntity.UserType playerType)
        {
            // First, try to find if the player entity exists
            PlayerEntity playerEntity = DatabaseManager.Instance.FindPlayerEntity(playerType.ToString(), playerId.ToString()); 
            // If the value is not null (== already exists)
            if (playerEntity != null)
            {
                // Return false, as this cannot be created
                return false;
            }

            // Otherwise, keep on going
            // First, create the new player entity to be stored
            playerEntity = new PlayerEntity(playerType, playerId);

            // Then, store it to the database
            DatabaseManager.Instance.SavePlayer(playerEntity);

            // And finally return true as a mark of success
            return true;

        }

        public bool TryCreateDatabasePlayerEntry()
        {
            // First, try to find if the player entity exists
            PlayerEntity playerEntity = DatabaseManager.Instance.FindPlayerEntity(NewPlayerEntity);
            // If the value is not null (== already exists)
            if (playerEntity != null)
            {
                // Return false, as this cannot be created
                return false;
            }

            // Otherwise, keep on going

            // Store the new entity to the database
            DatabaseManager.Instance.SavePlayer(NewPlayerEntity);

            // And finally return true as a mark of success
            return true;

        }
    }
}