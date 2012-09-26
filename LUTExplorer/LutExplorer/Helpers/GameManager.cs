using System;
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
        public void GetAchievement(PlayerEntity player, string achievement)
        {
            if (achievement == null) return;
            
            if (player.Achievements == null)
            {
                player.Achievements = new Dictionary<string, DateTime>();
            }

            // Check if the player does not have the treasure yet
            if (!player.Achievements.ContainsKey(achievement))
            {
                // Add the treasure to the treasure chest
                player.Achievements.Add(achievement, DateTime.Now);
                                                
                // Save the changes
                DatabaseManager.Instance.SavePlayer(player);

                DatabaseManager.Instance.SaveAchievement(achievement, player);
            }

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
                        return "Ohjelmistotekniikan professori Kari Smolander tunnetaan myös Alice in Wasteland -yhtyeen kitaristina. <br /><iframe width=\"420\" height=\"315\" src=\"http://www.youtube.com/embed/ZFWNhwNR30g\" frameborder=\"0\" allowfullscreen></iframe>";
                    case 3:
                        return "";
                    case 4:
                        return "Tietotekniikan osasto muutti pois 6-vaiheen yläkerroksista jo keväällä 2011, mutta kyltti jäi.";
                    case 5:
                        return "";
                    case 6:
                        return "Kukaan ei tarkalleen tiedä, mitä nuo puukoristeet seinällä mahtavat esittää.";
                    case 7:
                        return "Tässä vitriinissä ei ole koskaan ollut mitään sisällä.";
                    case 8:
                        return "";
                    case 9:
                        return "Kaukoputkesta voi tarkastella vaikkapa LUTin vihreää tuulimyllyä lähemmin!";
                    case 10:
                        return "Näytöön kiinnitetyn web-kameran avulla voit vaikkapa skypettää Kiinaan!";
                    case 11:
                        return "Monen kauppatieteilijän mielestä tämä on paras näköalapaikka.";
                    case 12:
                        return "Ylioppilastalo Oy:n veppikioski on otettu käyttöön 12.4.2008";
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
                    return "Onneksi olkoon, pääsit maaliin!";
                case 1:
                    return "Lähtöpaikka on pääaulassa";
                    
                case 2:
                    return "<script>function tip(){document.getElementById(\"tip\").innerHTML=\"Etsimäsi rasti sijaitsee Tietotekniikan osastolla\";}"
                        +"</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=\"tip()\"><h2>anna vihje</h2></a></p>"; 
                    
                case 3:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 4:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 5:
                    return "<script>function tip(){document.getElementById(\"tip\").innerHTML=\"<img src = ../../Content/pics/"+ number+"/h.jpg>\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=\"tip()\"><h2>anna vihje</h2></a></p>"; 
                case 6:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 7:
                    return "<script>function tip(){document.getElementById(\"tip\").innerHTML=\"<img src = ../../Content/pics/" + number + "/h.jpg>\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=\"tip()\"><h2>anna vihje</h2></a></p>";
                case 8:
                    return "<script>function tip(){document.getElementById(\"tip\").innerHTML=\"Rasti sijaitsee 1-vaiheessa.\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=\"tip()\"><h2>anna vihje</h2></a></p>";
                case 9:
                    return "<script>function tip(){document.getElementById(\"tip\").innerHTML=\"Rasti sijaitsee 1-vaiheessa.\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=\"tip()\"><h2>anna vihje</h2></a></p>";
                case 10:
                    return "<script>function tip(){document.getElementById(\"tip\").innerHTML=\"Rasti sijaitsee yhden 7-vaiheen hissin liepeillä.\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=\"tip()\"><h2>anna vihje</h2></a></p>";
                case 11:
                    return "<script>function tip(){document.getElementById(\"tip\").innerHTML=\"<img src = ../../Content/pics/" + number + "/h.jpg width=40%>\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=tip()><h2>anna vihje</h2></a></p>";
                case 12:
                    return "<script>function tip(){document.getElementById(\"tip\").innerHTML=\"Olet matkalla ylioppilastalolle.\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=\"tip()\"><h2>anna vihje</h2></a></p>";
                case 13:
                    return "<script>function tip(){document.getElementById(\"tip\").innerHTML=\"Suuntaa kohti kielikeskusta..\";}"
                        + "</script> <img src= ../../Content/pics/" + number + "/p.jpg width=40% /> <br /><p id=\"tip\"><a onclick=\"tip()\"><h2>anna vihje</h2></a></p>";
                case 14:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 15:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 16:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 17:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 18:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 19:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";
                case 20:
                    return "<img src= ../../Content/pics/" + number + "/p.jpg width=40% />";

                default:
                    return ""; 
            }

        }

        /// <summary>
        /// A method to work out what achievement to give based on page number
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <returns> NULL if no achievement, achievement name otherwise </returns>

        public string GetAchievementFromNumber(int pageNumber)
        {

            switch (pageNumber)
            {

                case 1:
                    return "ykkönen";
                case 2:
                    return "kakkonen";
                case 3:
                    return null;
                case 4:
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
                    return null;
                case 10:
                    return null;
            }
            return null;
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
            // if pageNumber==998 restart game

            // player is at the right checkpoint

            // player has completed the game
            if (player.CurrentSearchedTreasure == 0)
            {
                player.CurrentSearchedTreasure = 1;
                DatabaseManager.Instance.SavePlayer(player);
                return new Tuple<string, string, string, string>("Olet jo päässyt pelin läpi! <br>Aloita uusi kierros pääaulasta.", "", "", "");
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
                        return new Tuple<string, string, string, string>("Löysit rastin!", "", "",  getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)) );
                        
                    case "Achievements":
                        // GET achievement here
                        GetAchievement(player, GetAchievementFromNumber(pageNumber));
                        //return content
                        return new Tuple<string, string, string, string>("Löysit rastin!", "", "", getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
                    
                    case "Context":
                        return new Tuple<string, string, string, string>("Löysit rastin!", getPageContext(pageNumber), "", getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
                    
                    case "ContextAchievements":
                        // GET achievement here
                        GetAchievement(player, GetAchievementFromNumber(pageNumber));

                        return new Tuple<string, string, string, string>("Löysit rastin!", getPageContext(pageNumber), "", getPageClue(RouteManager.getNext(player.CurrentRoute, pageNumber)));
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
                    return new Tuple<string, string, string, string>("Olet väärällä rastilla. Vai aloitetaanko uusi kierros?", " ", " ", " " + getPageClue(player.CurrentSearchedTreasure));
                }

                // Get explorer achievement

                return new Tuple<string, string, string, string>("Olet väärällä rastilla", " ", " ", " " + getPageClue(player.CurrentSearchedTreasure) );
            }
        }


        
    }
}