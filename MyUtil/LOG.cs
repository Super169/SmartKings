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

        public static bool D(string errMsg)
        {
            return D(errMsg, debugLog);
        }

        public static bool D(string errMsg, string fileName)
        {
            return FILE.saveToFile(string.Format("{0:yyyy-MM-dd HH:mm:ss} - {1}", DateTime.Now, errMsg), fileName);
        }

        public static bool E(string errMsg)
        {
            return E(errMsg, errLog);
        }

        public static bool E(string errMsg, string fileName)
        {
            return FILE.saveToFile(string.Format("{0:yyyy-MM-dd HH:mm:ss} - {1}", DateTime.Now, errMsg), fileName);
        }
    }
}
