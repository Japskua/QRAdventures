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
                       
            PlayerEntity playerEntity = CookieManager.Instance.GetPlayerAutomatically(Request);

            // Bag the info for displaying on the browser
            ViewBag.Message = "hai ";
            // Modifying player info
            DatabaseManager.Instance.SaveNextTreasure(playerEntity, 3);
                        

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



            return View();

        }
    }
}
