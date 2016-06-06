
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class com
    {

        private static bool isStarted { get; set; } = false;

        public static void start(string appName)
        {
            isStarted = true;
        }


        public static void ShutDown()
        {
            isStarted = false;
        }


    }
}
