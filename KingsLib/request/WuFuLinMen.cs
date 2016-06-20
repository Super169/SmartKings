
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class WuFuLinMen
    {
        private const string CMD_getGameInfo = "WuFuLinMen.getGameInfo";

        public static RequestReturnObject getGameInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getGameInfo);
        }


    }
}
