using DreamScape.Data.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data.Utility
{
    class Session
    {
        // User session for retrieving logged in user
        public User? User;
        private static Session _instance;

        public static Session Instance
        {
            get
            {
                // If session is null and called, create a new session and return it
                if (_instance == null)
                {
                    _instance = new Session();
                }
                return _instance;
            }
        }

        /// <summary>
        /// This method sets the Session's user to Null, and sends the user back to the login screen.
        /// </summary>
        /// <param name="frame">This is needed to navigate to the login screen.</param>
        /// <param name="xamlRoot">This is needed to display a message about logging out.</param>
        public void Logout(Frame frame, XamlRoot xamlRoot)
        {
            if (_instance != null)
            {
                User = null;
                frame.Navigate(typeof(LoginPage));
                Helper.ShowPopup("Success", "You've been logged out.\nHave a nice day!", xamlRoot);
            }
        }
    }
}
