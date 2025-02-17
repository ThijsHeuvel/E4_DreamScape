using DreamScape.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data
{
    class Session
    {
        // User session for retrieving logged in user
        public User User;
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
    }
}
