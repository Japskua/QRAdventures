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
        /// Creates the cookie randomly, according to the predefined ways
        /// </summary>
        /// <param name="response"></param>
        /// <param name="request"></param>
        public void CreateCookie(HttpResponseBase response, HttpRequestBase request)
        {

            // First, check if the cookie exists
            if (CheckIfCookieExists(request) == true)
            {
                // Just leave

                return;
            }

            // Otherwise, create the cookie
            HttpCookie lutExplorerCookie = new HttpCookie(cookieName);

            // Create the player creator
            PlayerCreator playerCreator = new PlayerCreator();

            // Next, try to create the new value to the database
            if (playerCreator.TryCreateDatabasePlayerEntry() == false)
            {
                // Do nothing for now
                throw NotImplementedException();
            }

            // Else, keep on going

            // Set the cookie values
            lutExplorerCookie[playerType] = playerCreator.NewPlayerEntity.PartitionKey;
            lutExplorerCookie[playerId] = playerCreator.NewPlayerEntity.RowKey;

            // Set the expiration date
            lutExplorerCookie.Expires = DateTime.Now.AddDays(expirationDays);

            // And finally, save the cookie to the browser
            response.Cookies.Add(lutExplorerCookie);
        }

        /// <summary>
        ///  The error for not implemented exception
        /// </summary>
        /// <returns></returns>
        private Exception NotImplementedException()
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Gets the player from the database based on the cookie information
        /// </summary>
        /// <param name="cookie">The cookie which contains the information</param>
        /// <returns>The player entity from the database if found</returns>
        public PlayerEntity GetPlayerFromCookie(HttpCookie cookie)
        {
            // Try to find the player entity
            PlayerEntity playerEntity = DatabaseManager.Instance.FindPlayerEntity(cookie[playerType], cookie[playerId]);

            return playerEntity;
        }

        /// <summary>
        /// Gets the player automatically from the database, based on the cookie in the
        /// player browser.
        /// </summary>
        /// <param name="request">The HttpRequestBase that is used to get requests from the browser</param>
        /// <returns>The player entity from the database if found</returns>
        public PlayerEntity GetPlayerAutomatically(HttpRequestBase request)
        {
            return GetPlayerFromCookie(GetCookie(request));
        }


        public bool DeleteCookie(HttpRequestBase request)
        {
            request.Cookies.Remove(cookieName);
            
            return true;
        }

        #endregion Functions
    }
}