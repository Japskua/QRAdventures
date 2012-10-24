using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LutExplorer.Helpers;
using LutExplorer.Helpers.DatabaseEntities;

namespace LutExplorer.Controllers
{
    public class HomeController : Controller
    {
        private string cookieName = "LUTExplorerCookie";
        private string playerType = "PlayerType";
        private string playerId = "PlayerId";


        //public ActionResult Index()
        //{
        //    ViewBag.Message = "Welcome to ASP.NET MVC!";

        //    ViewBag.Message = Request.Url;

        //    // Create the cookie for the user in question
        //    CookieManager.Instance.CreateCookie(Response, Request);

        //    int pageNumber = RouteManager.getPageNumberFromRequest(Request);
            
        //    ViewBag.Context = pageNumber.ToString();

        //    return View();
        //}


        /// <summary>
        /// Actual index page
        /// Or at least it should be.
        /// </summary>
        /// <returns>the view</returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            ViewBag.header = Request.UrlReferrer;

            // get page number from redirect
            int pageNumber = RouteManager.getPageNumberFromRequest(Request);
            // helper
            int i;

            // game restart
            if (pageNumber == 998)
            {
                CookieManager.Instance.DeleteCookie(Request);

                CookieManager.Instance.CreateCookie(Response, Request);

                PlayerEntity pe = CookieManager.Instance.GetPlayerAutomatically(Request);
                GameManager gm = new GameManager();
                ViewBag.Achievement = gm.GetAchievement(pe, "restart");
                
                ViewBag.Message = "Round starts at the main lobby!";
                ViewBag.Context = "";
                
                ViewBag.Clue = "";

                return View();
            }
            
            // Create the cookie for the user in question
            CookieManager.Instance.CreateCookie(Response, Request);

            // Get player stats from db
            PlayerEntity playerEntity = CookieManager.Instance.GetPlayerAutomatically(Request);
            // Start a game manageer instance
            GameManager gameManager = new GameManager();

            // handling route selection

            if (pageNumber == 901)
            {
                playerEntity.CurrentRoute = 1;
                DatabaseManager.Instance.SavePlayer(playerEntity);
                pageNumber = 1;
            }
            if (pageNumber == 902)
            {
                playerEntity.CurrentRoute = 2;
                DatabaseManager.Instance.SavePlayer(playerEntity);
                pageNumber = 1;
            }

            //int pageNumber = playerEntity.CurrentSearchedTreasure;

            //  Get page content and bag it for view
            Tuple<string, string, string, string> tuple = gameManager.getPageContent(playerEntity, pageNumber);
            ViewBag.Message = tuple.Item1;
            ViewBag.Context = tuple.Item2;
            ViewBag.Achievement = tuple.Item3;
            ViewBag.Clue = tuple.Item4;


            

            if (playerEntity.Achievements != null && playerEntity.Achievements.Count() > 0)
            {

                ViewBag.Badges += "<h3>Your achievement badges:</h3><br />";
                ViewBag.Badges += "<table><tr><td>";
                i = 0;
                foreach (KeyValuePair<string, DateTime> n in playerEntity.Achievements)
                {

                    ViewBag.Badges += "<table><tr><td>";
                    ViewBag.Badges += RouteManager.GetBadge(n.Key);
                    ViewBag.Badges += "</td></tr><tr><td>" + n.Key + "</td></tr></table>";

                    if (i % 2 == 0 && i != 0)
                    {
                        ViewBag.Badges += "</td></tr><tr><td>";
                        i = 0;
                    }

                    else
                    {
                        ViewBag.Badges += "</td><td>";
                    }

                    i++;
                }

            }
            ViewBag.Badges += "</td></tr></table>";
            // return the view and gtfo
            return View();
        }

        /// <summary>
        /// Action result for the TestView page
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult TestView()
        {
         
            // Get player from db
            PlayerEntity playerEntity = CookieManager.Instance.GetPlayerAutomatically(Request);
            GameManager gameManager = new GameManager();

            // get page number from redirect

            //int pageNumber = RouteManager.getPageNumberFromRequest(Request);

            // Get the page content...
            // and bag the info for displaying on the browser
            
            // page number for debugginh
            int pageNumber = playerEntity.CurrentSearchedTreasure;

            Tuple<string,string,string,string> tuple = gameManager.getPageContent(playerEntity, pageNumber);
            ViewBag.Message = tuple.Item1;
            ViewBag.Context = tuple.Item2;
            ViewBag.Achievement = tuple.Item3;
            ViewBag.clue = tuple.Item4;
            

            // earlier debugging shit: 
            //// get the context info for correct checkpoint or error for wrong one
            //ViewBag.ContextInfo = gameManager.getPageContent(playerEntity, pageNumber);
            //// check if player is at the correct checkpoint
            //if(gameManager.CheckIfCorrectTreasure(playerEntity, pageNumber) ) {
            //    // get the treasure, update player 
            //    gameManager.GetTreasure(playerEntity, pageNumber);
            //}         
            // show the clue to the next checkpoint
            // becuase GetTreasure updates player status, the next checkpoint will be the next checkpoint
            //ViewBag.PageCLue = gameManager.getPageClue(playerEntity);

            // Modifying player info
            //DatabaseManager.Instance.SaveNextTreasure(playerEntity, 3);
            //gameManager.GetAchievement(playerEntity, "supermies");
            //debugging progress, so incremnt page numbr
            
            
            // Return the view

            return View();
        }

        public ActionResult About()
        {
            // initialize the id and user type values
            string id = "";
            string userType = "";

            // Retrieve the cookie from the user
            HttpCookie cookie = CookieManager.Instance.GetCookie(Request);

            // Check if the cookie exists
            if (cookie == null)
            {
                return View();
            }

            // Otherwise continue 
            // Get the ID and user type
            id = cookie[playerId];
            userType = cookie[playerType];

            // Save the info to viewbag contents
            ViewBag.CookieContents = "Playertype is: " + cookie[playerType] + " and player id is: " + cookie[playerId];

            // Create the query <<--- Not Needed anymore
            //PlayerEntity query = new PlayerEntity(PlayerEntity.UserType.Regular, 1);

            // Search the database for player entity
            // Try to find the player entity
            PlayerEntity playerEntity = DatabaseManager.Instance.FindPlayerEntity(userType, id);

            // Modifying player info
            //DatabaseManager.Instance.SaveNextTreasure(playerEntity, 3);

            // Save the info
            // Throw the whole player entity and the manager will handle the rest
            
            //DatabaseManager.Instance.SavePlayer(playerEntity);

            // Bag the info for displaying on the browser
            ViewBag.Message = "Player " + playerEntity.RowKey + " Playing as " + playerEntity.PartitionKey + " player." +
                               "\nCurrently searching for treasure number " + playerEntity.CurrentSearchedTreasure.ToString();

            ViewBag.Achievements = "";
            if (playerEntity.Achievements != null )
            {
                foreach (KeyValuePair<string, DateTime> ach in playerEntity.Achievements)
                {
                    ViewBag.Achievements = ViewBag.Achievements + ", " + ach.Key;
                }
            }
            if (playerEntity.TreasureChest != null)
            {
                ViewBag.Treasures = "";
                foreach (KeyValuePair<int, DateTime> ach in playerEntity.TreasureChest)
                {
                    ViewBag.Treasures = ViewBag.Treasures + ", " + ach.Key.ToString();
                }
            }



            return View();

        }
    }
}
