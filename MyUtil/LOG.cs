using System;
using System.Collections.Generic;
using System.IO;
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

        public static bool D(string account, string action, string msg)
        {
            return W(account, action, msg, debugLog);
        }

        public static bool E(string msg)
        {
            return W(msg, errLog);
        }

        public static bool E(string account, string action, string msg)
        {
            return W(account, action, msg, errLog);
        }

        public static bool I(string msg)
        {
            return W(msg, infoLog);
        }

        public static bool I(string account, string action, string msg)
        {
            return W(account, action, msg, infoLog);
        }

        public static bool W(string account, string action, string msg, string fileName)
        {
            return W(string.Format("{0} : {1} : {2}", account, action, msg), fileName);
        }


        public static bool W(string msg, string fileName)
        {
            string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Log");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                if (!Directory.Exists(filePath)) filePath = Directory.GetCurrentDirectory();
            }
            string fullName = System.IO.Path.Combine(filePath, fileName);

            return FILE.saveToFile(string.Format("{0:yyyy-MM-dd HH:mm:ss} - {1}", DateTime.Now, msg), fullName);
        }

    }
}
