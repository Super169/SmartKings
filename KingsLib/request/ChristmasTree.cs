
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class ChristmasTree
    {
        private const string CMD_time = "ChristmasTree.time";

        public static RequestReturnObject time(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_time);
        }


    }
}
