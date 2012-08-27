using System;
using System.Collections.Generic;
using System.Linq;


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

    }
}
