using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LutExplorer.Helpers.DatabaseEntities;


namespace LutExplorer.Helpers
{
    public static class RouteManager
    {
        private static List<int> route1 = new List<int>(new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13});
        
        private static List<int> route2 = new List<int>(new int[] { 1, 3, 5, 7, 9 });

        
        

        static RouteManager()
        {
            
        }
        public static int getNext(int route, int current)
        {
          
                switch (route)
                {
                    case 1:
                        if (route1.Count != route1.FindIndex(i => i == current) + 1)
                            return route1[route1.FindIndex(i => i == current) + 1];
                        else return 0;
                    case 2:
                        if (route1.Count != route1.FindIndex(i => i == current) + 1)
                            return route2[route2.FindIndex(i => i == current) + 1];
                        else return 0;
                    default:
                        return 0;
                }
            
           
        }

        public static int getPrevious(int route, int current)
        {
            switch (route)
            {
                case 1:
                    try
                    {
                        return route1[route1.FindIndex(i => i == current) - 1];
                    }
                    catch (Exception)
                    {
                        return 0;
                    }
                case 2:
                    try
                    {
                        return route2[route2.FindIndex(i => i == current) - 1];
                    }
                    catch (Exception) { return 0; }
                default:
                    return 0;
            }
        }

        public static int getPageNumberFromRequest(HttpRequestBase request)
        {

            string url = request.Url.ToString();

            if (url.Contains("p="))
            {
                return (Convert.ToInt32( url.Split('=')[1].Split('v')[0] ) -7 ) /1024;

            }

            //if (url.Contains("p=10")) return 10;
            //if (url.Contains("p=11")) return 11;
            //if (url.Contains("p=12")) return 12;
            //if (url.Contains("p=13")) return 13;
            //if (url.Contains("p=14")) return 14;
            //if (url.Contains("p=15")) return 15;
            //if (url.Contains("p=16")) return 16;
            //if (url.Contains("p=17")) return 17;
            //if (url.Contains("p=18")) return 18;
            //if (url.Contains("p=19")) return 19;
            //if (url.Contains("p=20")) return 20;
            //if (url.Contains("p=1")) return 1;
            //if (url.Contains("p=2")) return 2;
            //if (url.Contains("p=3")) return 3;
            //if (url.Contains("p=4")) return 4;
            //if (url.Contains("p=5")) return 5;
            //if (url.Contains("p=6")) return 6;
            //if (url.Contains("p=7")) return 7;
            //if (url.Contains("p=8")) return 8;
            //if (url.Contains("p=9")) return 9;

            return 999;

    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetBadge(string name)
        {

            return "<img src=\"../../Content/achievements/"+ name +  ".jpg\" />";

        }
        /// <summary>
        /// Datetime comparison for time achievements.
        /// takes first chekpoint, 2nd checkpoint, allowed difference in minutes and player entity
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static bool CheckTimeAchievements(int t1, int t2,int diff,  PlayerEntity player)
        {
            if (player.TreasureChest.ContainsKey(t1) && player.TreasureChest.ContainsKey(t2))
            {
                TimeSpan dt = player.TreasureChest[t2].Subtract(player.TreasureChest[t1]);
                if (dt.TotalMinutes < diff) return true;
            }
            return false;
        }

        public static bool CheckIfIntervalCheckpoint(int pageNumber)
        {
            return false;
        }

        //end class
    }

    
}
