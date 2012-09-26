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
        
        
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            
            // Create the cookie for the user in question
            CookieManager.Instance.CreateCookie(Response, Request);

            return View();
        }

        /// <summary>
        /// Action result for the TestView page
        /// </summary>
        /// <returns>The view</returns>
        public ActionResult TestView()
        {
            int pageNumber = 1;

            PlayerEntity playerEntity = CookieManager.Instance.GetPlayerAutomatically(Request);
            GameManager gameManager = new GameManager();

            // Get the page content...
            // and bag the info for displaying on the browser
            
            Tuple<string,string,string,string> tuple = gameManager.getPageContent(playerEntity, playerEntity.CurrentSearchedTreasure);
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
