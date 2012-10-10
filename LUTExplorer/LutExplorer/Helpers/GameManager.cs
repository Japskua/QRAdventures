﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        /// Checks if the treasure found is the correct one the player is looking for BUT doesn't require the parameters as previous, just the PlayerEntity
        /// </summary>
        /// <param name="player">PlayerEntity, the player in question</param>
        /// <returns>Boolean</returns>
        public bool CheckIfCorrectTreasure(PlayerEntity player, int found)
        {
            return CheckIfCorrectTreasure(player.CurrentSearchedTreasure, found); 

        }

        /// <summary>
        /// Gets the treasure defined and stores it to the player treasure chest
        /// </summary>
        /// <param name="player">The player entity in question</param>
        /// <param name="treasure">The treasure that was found</param>
        /// <param name="nextTreasure">The next treasure to be attained</param>
        public void GetTreasure(PlayerEntity player, int treasure)
        {
            int nextTreasure = RouteManager.getNext(player.CurrentRoute, treasure);
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

                DatabaseManager.Instance.SaveTreasure(treasure.ToString(), player);
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
        public string GetAchievement(PlayerEntity player, string achievement)
        {
            if (achievement == null) return "";
            
            if (player.Achievements == null)
            {
                player.Achievements = new Dictionary<string, DateTime>();
            }

            // special case for the explorer and time based achievement(s)

            if (achievement == "explorer" && player.Achievements.ContainsKey(achievement))
            {
                return GetAchievement(player, "superExplorer");
            }

            if (achievement == "speedy" && player.Achievements.ContainsKey(achievement))
            {
                if (!player.Achievements.ContainsKey("superSpeedy"))
                    return GetAchievement(player, "superSpeedy");
                else return GetAchievement(player, "hyperSpeedy");
            }

            // now regular achievements

            // Check if the player does not have the treasure yet
            if (!player.Achievements.ContainsKey(achievement))
            {
                // Add the treasure to the treasure chest
                player.Achievements.Add(achievement, DateTime.Now);
                                                
                // Save the changes
                DatabaseManager.Instance.SavePlayer(player);

                DatabaseManager.Instance.SaveAchievement(achievement, player);

                return "Achievement unlocked: " + achievement + "<br />" + "<img src=\"../../Content/achievements/" +achievement + ".jpg\"> <br />" + GetAchievementDesc(achievement) ;
            }

            return "";
        }


        /// <summary>
        ///  Contains the logic to work out the actual page content
        /// </summary>
        /// <param name="treasure">Number of the treasure (QR-code)/checkpoint number</param>
        /// <returns>page content</returns>
        public string getPageContext(int treasure)
        {

                switch (treasure)
                {
                    case 1:
                        return "<iframe width=420 height=315 src=http://www.youtube.com/embed/SHj153yFDg4 frameborder=0 allowfullscreen></iframe>"
                            + "";
                    case 2:
                        return "Kari Smolander, Professor in Software engineering, is also known for being the guitarist of Alice in Wasteland. <br /><iframe width=\"420\" height=\"315\" src=\"http://www.youtube.com/embed/ZFWNhwNR30g\" frameborder=\"0\" allowfullscreen></iframe>";
                        //return "Ohjelmistotekniikan professori Kari Smolander tunnetaan myös Alice in Wasteland -yhtyeen kitaristina. <br /><iframe width=\"420\" height=\"315\" src=\"http://www.youtube.com/embed/ZFWNhwNR30g\" frameborder=\"0\" allowfullscreen></iframe>";
                    case 3:
                        return "";
                    case 4:
                        return "Department of Information Technology moved from this building in 2011 but the sign remains in place.";
                        //return "Tietotekniikan osasto muutti pois 6-vaiheen yläkerroksista jo keväällä 2011, mutta kyltti jäi.";
                    case 5:
                        return "";
                    case 6:
                        return "Kukaan ei tarkalleen tiedä, mitä nuo puukoristeet seinällä mahtavat esittää.";
                    case 7:
                        return "For as long as the developers of LUT Explorer have been around LUT there has never been anything on display in this glass cabinet";
                    case 8:
                        return "";
                    case 9:
                        return "With the telescope you can check out the green windmill up close.";
                        //return "Kaukoputkesta voi tarkastella vaikkapa LUTin vihreää tuulimyllyä lähemmin!";
                    case 10:
                        return "You could make a Skype call anywhere in the world using that screen mounted web camera.";
                    case 11:
                        return "Many business students claim this is the best view of the whole campus area. It's hard to disagree, isn't it.";
                    case 12:
                        return "Student union building's internet kiosk, taken into use April 12th 2008";
                    case 13:
                        return "";
                    case 14:
                        return "";
                    case 15:
                        return "";
                    case 16:
                        return "";
                    case 17:
                        return "";
                    case 18:
                        return "";
                    case 19:
                        return "";
                    case 20:
                        return "";

                        
                    default:
                        return "0";
                }
            
            

                        
        }

        /// <summary>
        /// Shit method for passing relevant clues to next check points
        /// Possibly this could be automated slightly, ie in the future to read the content from a text file etc
        /// </summary>
        /// <param name="number">Number of the page that loaded</param>
        /// <returns>string Clue to the next checkpoint</returns>
        public string getPageClue(int number)
        {
            switch (number)
            { 

                case 0:
                    return "Congratulations, you made it to the finish! <ul><li>For a new round <a href=\"http://lutexplorer.cloudapp.net/?p=1021959v\">Click here</a></li></ul>";
                case 1:
                    return "";
                    
                case 2:
                    return "<h2>Your next checkpoint is here:</h2><script>function tip(){document.getElementById(\"tip\").innerHTML=\"The next QR Code is at the IT department\";}"
                        +"</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><a onclick=\"tip()\"><p class=\"hint\">Give me a hint</p></a><br /><p id=\"tip\"></p>"; 
                    
                case 3:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 4:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 5:
                    return "<h2>Your next checkpoint is here:</h2><script>function tip(){document.getElementById(\"tip\").innerHTML=\"<img src = ../../Content/pics/"+ number+"/h.jpg>\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><a onclick=\"tip()\"><p class=\"hint\">Give me a hint</p></a><br /><p id=\"tip\"></p>"; 
                case 6:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 7:
                    return "<h2>Your next checkpoint is here:</h2><script>function tip(){document.getElementById(\"tip\").innerHTML=\"<img src = ../../Content/pics/" + number + "/h.jpg>\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><a onclick=\"tip()\"><p class=\"hint\">Give me a hint</p></a><br /><p id=\"tip\"></p>";
                case 8:
                    return "<h2>Your next checkpoint is here:</h2><script>function tip(){document.getElementById(\"tip\").innerHTML=\"<img src = ../../Content/pics/" + number + "/h.jpg>\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><a onclick=\"tip()\"><p class=\"hint\">Give me a hint</p></a><br /><p id=\"tip\"></p>";
                case 9:
                    return "<h2>Your next checkpoint is here:</h2><script>function tip(){document.getElementById(\"tip\").innerHTML=\"This QR code is somewhere in Phase 1.\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><a onclick=\"tip()\"><p class=\"hint\">Give me a hint</p></a><br /><p id=\"tip\"></p>";
                case 10:
                    return "<h2>Your next checkpoint is here:</h2><script>function tip(){document.getElementById(\"tip\").innerHTML=\"This QR-code is near a lift in phase 7.\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><a onclick=\"tip()\"><p class=\"hint\">Give me a hint</p></a><br /><p id=\"tip\"></p>";
                case 11:
                    return "<h2>Your next checkpoint is here:</h2><script>function tip(){document.getElementById(\"tip\").innerHTML=\"<img src = ../../Content/pics/" + number + "/h.jpg width=40%>\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=tip()><p class=\"hint\">Give me a hint</p></a></p>";
                case 12:
                    return "<h2>Your next checkpoint is here:</h2><script>function tip(){document.getElementById(\"tip\").innerHTML=\"You're heading for the student union building.\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><a onclick=\"tip()\"><p class=\"hint\">Give me a hint</p></a><br /><p id=\"tip\"></p>";
                case 13:
                    return "<h2>Your next checkpoint is here:</h2><script>function tip(){document.getElementById(\"tip\").innerHTML=\"Point your nose towards LUT Language Centre..\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><a onclick=\"tip()\"><p class=\"hint\">Give me a hint</p></a><br /><p id=\"tip\"></p>";
                case 14:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 15:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 16:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 17:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 18:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 19:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 20:
                    return "<h2>Your next checkpoint is here:</h2><img src= ../../Content/pics/" + number + "/p.jpg width=40% />";

                default:
                    return ""; 
            }

        }

        /// <summary>
        /// A method to work out what achievement to give based on page number
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <returns> NULL if no achievement, achievement name otherwise </returns>

        public string GetAchievementFromNumber(int pageNumber, PlayerEntity player)
        {

            if (player.CurrentRoute == 1)
            {
                switch (pageNumber)
                {

                    case 1:
                        return null;
                    case 2:
                        if (RouteManager.CheckTimeAchievements(2, 1, 16, player))
                            return "speedy";
                        return null;
                    case 3:
                        return "halfway";
                    case 4:
                        if (RouteManager.CheckTimeAchievements(4, 1, 23, player))
                            return "speedy";
                        return null;
                    case 5:
                        return null;
                    case 6:
                        return null;
                    case 7:
                        return null;
                    case 8:
                        return null;
                    case 9:
                        return "telescope";
                    case 10:
                        if (RouteManager.CheckTimeAchievements(10, 1, 9, player))
                            return "speedy";
                        return null;
                    case 13:
                        if (RouteManager.CheckTimeAchievements(13, 1, 5, player))
                            return "speedy";
                        return null;
                }
                return null;
            }
            else return null;
        }

        public string GetAchievementDesc(string name)
        {
            switch (name)
            {
                case "speedy":
                    return "You are quick to think on your feet! You beat the target time to reach this checkpoint!";
                case "superSpeedy":
                    return "Wow, you're as fast as the wind!";
                case "hyperSpeedy":
                    return "Faster than lighning dude!";
            }

            return "";
        }
        
        /// <summary>
        /// The only method that needs to be called outside this class itself
        /// Works out what content needs to be on the page and returns it in a tuple.
        /// Calls all other necessary methods that return the actual raw-html as a string.
        /// </summary>
        /// <param name="player">Player entity</param>
        /// <param name="pageNumber">Number of the page that loaded</param>
        /// <returns>All of the necessary page content in a string, string, string, string -type Tuple</returns>
        public Tuple<string,string,string,string> getPageContent(PlayerEntity player, int pageNumber)
        {
            // TODO: handlers for page reload and restardation
            // if pageNumber==XX restart game = delete cookie from browser
            // restart done in the controller, so sue me. Fuck me silly IDGAF!
            
            // making the "are you sure to restart"-feature

            if (pageNumber == 924)
            {
                return new Tuple<string, string, string, string>("Are you sure to restart?", "For a new round <a href=\"http://lutexplorer.cloudapp.net/?p=1021959v\" >Click here</a>", "", "");
            }

            // error handling in case shit hits fan and also for debugging

            if (pageNumber == 999) return new Tuple<string, string, string, string>("", "", "", "");

            // player is at the right checkpoint

            // player has completed the game
            if (player.CurrentSearchedTreasure == 0)
            {
                //player.CurrentSearchedTreasure = 1;
                //DatabaseManager.Instance.SavePlayer(player);
                return new Tuple<string, string, string, string>("You've finished the game! For a new round <a href=\"http://lutexplorer.cloudapp.net/?p=1021959v\" >Click here</a>", "", "", "");
            }

            if (player.CurrentSearchedTreasure == pageNumber)
            {
                
                // you can has treasure
                // my precious
                GetTreasure(player, pageNumber);
                
                
                //what is the player's type?
                switch (player.PartitionKey)
                {
                    case "Regular":
                        if (pageNumber == 1) return new Tuple<string, string, string, string>("Welcome to the LUT Explorer game.", "LUT Explorer is a pervasive scavenger hunt game.<br />You must now find the next checkpoint. Good hunting!", "", getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
                        return new Tuple<string, string, string, string>("You found the checkpoint!", "", "",  getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)) );
                        
                    case "Achievements":
                        // GET achievement here
                        
                        //return content
                        if (pageNumber == 1) return new Tuple<string, string, string, string>("Welcome to the LUT Explorer game.", "LUT Explorer is a pervasive scavenger hunt game.<br />You must now find the next checkpoint. Good hunting!", GetAchievement(player, GetAchievementFromNumber(pageNumber, player)), getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
                        return new Tuple<string, string, string, string>("You found the checkpoint!", "", GetAchievement(player, GetAchievementFromNumber(pageNumber, player)), getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
                    
                    case "Context":
                        if (pageNumber == 1) return new Tuple<string, string, string, string>("Welcome to the LUT Explorer game.", "LUT Explorer is a pervasive scavenger hunt game.<br />You must now find the next checkpoint. Good hunting!", "", getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
                        return new Tuple<string, string, string, string>("You found the checkpoint!", getPageContext(pageNumber), "", getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
                    
                    case "ContextAchievements":
                        // GET achievement here


                        if (pageNumber == 1) return new Tuple<string, string, string, string>("Welcome to the LUT Explorer game.", "LUT Explorer is a pervasive scavenger hunt game.<br />You must now find the next checkpoint. Good hunting!", GetAchievement(player, GetAchievementFromNumber(pageNumber, player)), getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
                        return new Tuple<string, string, string, string>("You found the checkpoint!", getPageContext(pageNumber), GetAchievement(player, GetAchievementFromNumber(pageNumber, player)), getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
                    default:
                        return new Tuple<string, string, string, string>(" ", " ", " ", " " + " ");
                }
            }
            
                //if this is just a page reload
            else if (pageNumber == RouteManager.getPrevious(player.CurrentRoute, player.CurrentSearchedTreasure) ) {

                // if player gets achievements, get reload achievement 

                return new Tuple<string, string, string, string>(" ", " ", " ", " " + getPageClue(player.CurrentSearchedTreasure));
            }

             // player is at the wrong checkpoint
            else
            {
                if (pageNumber == 1) // does the player just want to start over?
                {
                    // TODO: add a link to reload page
                    return new Tuple<string, string, string, string>("Back in the main hall. Would you like to start again?", " ", " ", " " + getPageClue(player.CurrentSearchedTreasure));
                }


                // otherwise Get explorer achievement
                if (player.PartitionKey == PlayerEntity.UserType.Achievements.ToString() || player.PartitionKey== PlayerEntity.UserType.ContextAchievements.ToString() )
                {
                    if (player.CurrentSearchedTreasure == 1)
                        return new Tuple<string, string, string, string>("Welcome to LUT Explorer - The Game!", "The Game starts from the Main Lobby of the University", GetAchievement(player, "explorer"), " " + getPageClue(player.CurrentSearchedTreasure));
                    else
                        return new Tuple<string, string, string, string>("You're at the wrong checkpoint", " ", GetAchievement(player, "explorer"), " " + getPageClue(player.CurrentSearchedTreasure));
                }
                else
                {
                    return new Tuple<string, string, string, string>("You're at the wrong checkpoint", " ", " ", " " + getPageClue(player.CurrentSearchedTreasure));
                }
            }
        }


        
    }
}