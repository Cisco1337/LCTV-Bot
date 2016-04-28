using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCTV_Bot
{
    class Helpers
    {
        public static string HandleError(Exception x)
        {
            int error;
            string result;
            result = "UnknownException";
            error = (x.HResult / (-2));
            switch (error)
            {
                case 1073116543:
                    result = "IndexOutOfRangeException";
                    break;
            }
            return error + " (" + result + ")";
        }
    }
}
