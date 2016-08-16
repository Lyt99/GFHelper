using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFHelper.Models
{
    class SimpleInfo
    {
        public static Platform platform;
        public static string host;
        public static string uid;
        public static string sign;
        public static bool isServerLoaded = false;
        public static int reqid = 0;
        public static int timeoffset = 0;
    }
}
