using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


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
                            return route2[route1.FindIndex(i => i == current) + 1];
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
                    return route1[route1.FindIndex(i => i == current) - 1];
                case 2:
                    return route2[route1.FindIndex(i => i == current) + 1];
                default:
                    return 0;
            }
        }

        public static int getPageNumberFromRequest(HttpRequestBase request)
        {

            string url = request.UrlReferrer.ToString();

            if (url.Contains("1.php")) return 1;
            if (url.Contains("2.php")) return 2;
            if (url.Contains("3.php")) return 3;
            if (url.Contains("4.php")) return 4;
            if (url.Contains("5.php")) return 5;
            if (url.Contains("6.php")) return 6;
            if (url.Contains("7.php")) return 7;
            if (url.Contains("8.php")) return 8;
            if (url.Contains("9.php")) return 9;
            if (url.Contains("10.php")) return 10;
            if (url.Contains("11.php")) return 11;
            if (url.Contains("12.php")) return 12;
            if (url.Contains("13.php")) return 13;
            if (url.Contains("14.php")) return 14;
            if (url.Contains("15.php")) return 15;
            if (url.Contains("16.php")) return 16;
            if (url.Contains("17.php")) return 17;
            if (url.Contains("18.php")) return 18;
            if (url.Contains("19.php")) return 19;
            if (url.Contains("20.php")) return 20;


            return 999;

    
        }
    }
}
