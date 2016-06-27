using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtil
{
    public class LOG
    {

        public static string errLog = "error.log";
        public static string debugLog = "debug.log";
        public static string infoLog = "info.log";

        public static bool D(string msg)
        {
            return W(msg, debugLog);
        }

        public static bool E(string msg)
        {
            return W(msg, errLog);
        }

        public static bool I(string msg)
        {
            return W(msg, infoLog);
        }

        public static bool W(string msg, string fileName)
        {
            return FILE.saveToFile(string.Format("{0:yyyy-MM-dd HH:mm:ss} - {1}", DateTime.Now, msg), fileName);
        }

    }
}
