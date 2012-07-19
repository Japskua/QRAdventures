using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LutExplorer.Helpers;
using LutExplorer.Helpers.DatabaseEntities;
using LutExplorer.Helpers;

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

        public ActionResult About()
        {

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

            id = cookie[playerId];
            userType = cookie[playerType];

            ViewBag.CookieContents = "Playertype is: " + cookie[playerType] + " and player id is: " + cookie[playerId];


            // Create the query
            PlayerEntity query = new PlayerEntity(PlayerEntity.UserType.Regular, 1);

            // Try to find the player entity
            //PlayerEntity playerEntity = DatabaseManager.Instance.FindPlayerEntity(query);

            PlayerEntity playerEntity = DatabaseManager.Instance.FindPlayerEntity(userType, id);

            // Bad the info for displaying on the browser
            ViewBag.Message = "Player " + playerEntity.RowKey + "Playing as " + playerEntity.PartitionKey + " player." +
                               "\nCurrently searching for treasure number " + playerEntity.CurrentSearchedTreasure.ToString();



            return View();

        }
    }
}
