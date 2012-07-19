using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LutExplorer.Helpers.DatabaseEntities;

namespace LutExplorer.Helpers
{
    /// <summary>
    /// Cookie manager takes care of all the cookie related control
    /// such as reading from the cookies and storing there
    /// </summary>
    public sealed class CookieManager
    {
        private static volatile CookieManager instance;
        private static object syncRoot = new object();

        // The cookie related information
        private string cookieName = "LUTExplorerCookie";
        private string playerType = "PlayerType";
        private string playerId = "PlayerId";

        private int expirationDays = 2;

        private CookieManager() { }

        /// <summary>
        /// The Instance function is a static method for accessing
        /// the instantiated cookie manager in a thread-safe way
        /// </summary>
        public static CookieManager Instance
        {
            get
            {
                // If instance does not exist
                if (instance == null)
                {
                    // Lock
                    lock (syncRoot)
                    {
                        // If instance is still null
                        if (instance == null)
                        {
                            // Create the instance
                            instance = new CookieManager();
                        }
                    }
                }
                // Return the new instance
                return instance;
            }
        }

        #region Functions

        /// <summary>
        /// Creates the cookie to the player in question
        /// </summary>
        /// <param name="userType">The type of user that is playing</param>
        /// <param name="id">The id of the player in question</param>
        /// <param name="response">The HttpResponseBase used to send something to browser</param>
        /// <param name="request">The HttpRequestBase used to query for the cookie in the browser</param>
        public void CreateCookie(PlayerEntity.UserType userType, int id, HttpResponseBase response, HttpRequestBase request)
        {

            // First, check if the cookie exists
            if (CheckIfCookieExists(request) == true)
            {
                // Just leave
                return;
            }

            // Otherwise, create the cookie
            HttpCookie lutExplorerCookie = new HttpCookie(cookieName);

            // Set the cookie values
            lutExplorerCookie[playerType] = userType.ToString();
            lutExplorerCookie[playerId] = id.ToString();

            // Set the expiration date
            lutExplorerCookie.Expires = DateTime.Now.AddDays(expirationDays);

            // And finally, save the cookie to the browser
            response.Cookies.Add(lutExplorerCookie);



        }

        /// <summary>
        /// Checks if the given cookie exists
        /// </summary>
        /// <param name="request">The HttpRequest Base for asking the browser</param>
        /// <returns>
        /// True if the cookie is found
        /// False if no cookie found
        /// </returns>
        public bool CheckIfCookieExists(HttpRequestBase request)
        {
            // Get the cookie
            if (request.Cookies[cookieName] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the game specific cookie from the browser. 
        /// Checks always first for the existence of the cookie
        /// </summary>
        /// <param name="request">The HttpRequestBase used to query the player browser</param>
        /// <returns>
        /// The cookie if it exists
        /// null if not
        /// </returns>
        public HttpCookie GetCookie(HttpRequestBase request)
        {
            if (CheckIfCookieExists(request) == true)
            {
                return request.Cookies[cookieName];
            }

            // Otherwise, return nothing
            return null;

        }

        #endregion Functions
    }
}