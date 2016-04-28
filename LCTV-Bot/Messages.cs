using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCTV_Bot
{
    class Messages
    {
        private Messages(string message) { Value = message; }

        public string Value { get; set; }

        public static Messages NoRights  { get { return new Messages("Sorry, you do not have access to this command!"); } }
        public static Messages Disabled  { get { return new Messages("Sorry, this command is disabled!"); } }
        public static Messages HostOnly  { get { return new Messages(" [HOST ONLY]"); } }
        public static Messages RandomMsg { get { return new Messages(""); } }
    }
}
