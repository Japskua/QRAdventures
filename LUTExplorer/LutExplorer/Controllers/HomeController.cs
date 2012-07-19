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
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            // Create the query
            PlayerEntity query = new PlayerEntity(PlayerEntity.UserType.Regular, 1);

            // Try to find the player entity
            PlayerEntity playerEntity = DatabaseManager.Instance.FindPlayerEntity(query);

            // Bad the info for displaying on the browser
            ViewBag.Message = "Player " + playerEntity.RowKey + "Playing as " + playerEntity.PartitionKey + " player." +
                               "\nCurrently searching for treasure number " + playerEntity.CurrentSearchedTreasure.ToString();



            return View();
            
        }
    }
}
