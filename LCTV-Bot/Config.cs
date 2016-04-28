using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCTV_Bot
{
    class Config
    {
        private static string _user = "username";
        private static string _password = "password";

        public static string Username
        {
            get { return _user.ToLower(); }
            set { _user = value; }
        }

        public static string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
