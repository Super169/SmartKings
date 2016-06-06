using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class CrossCamp
    {
        private const string CMD_getInfo = "CrossCamp.getInfo";
        private const string CMD_getJackpotInfo = "CrossCamp.getJackpotInfo";

        public static RequestReturnObject getInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getInfo);
        }

        public static RequestReturnObject getJackpotInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getJackpotInfo);
        }


    }
}
