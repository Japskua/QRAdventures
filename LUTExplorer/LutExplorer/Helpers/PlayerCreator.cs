﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LutExplorer.Helpers.DatabaseEntities;

namespace LutExplorer.Helpers
{
    /// <summary>
    /// The PlayerCreator class handles creating new players
    /// and deciding whether the player is achievement or regular one
    /// </summary>
    public class PlayerCreator
    {

        // The likelihood of having achievement player
        //private int achievementLikelihood = 50;

        public PlayerEntity NewPlayerEntity;

        /// <summary>
        /// The constructor for the player creator
        /// </summary>
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
            int number = random.Next(1, 100);
            
            // Dividing players into two groups
            // If the value is bigger than achievement likelihood value
            //if (random.Next(1, 100) > achievementLikelihood)
            //{
            //    // Return achievement type play
            //    return PlayerEntity.UserType.Achievements;
            //}
            //else
            //{
            //    // Otherwise, return regular play
            //    return PlayerEntity.UserType.Regular;
            //}


            // dividing players into four groups
            if (number < 26) return PlayerEntity.UserType.Regular;
            else if (number < 51) return PlayerEntity.UserType.Achievements;
            else if (number < 76) return PlayerEntity.UserType.Context;
            else return PlayerEntity.UserType.ContextAchievements;
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

        /// <summary>
        /// Tries to create the database entry with the player in question
        /// </summary>
        /// <param name="playerId">The id to be inserted</param>
        /// <param name="playerType">The type of the player</param>
        /// <returns>True if succesful, false if not</returns>
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

        /// <summary>
        /// Tries to create the player into the database in question
        /// </summary>
        /// <returns>
        /// True if succeeded in adding the player to the database
        /// False if not
        /// </returns>
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