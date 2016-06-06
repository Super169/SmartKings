
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class LordGodUp
    {
        private const string CMD_getDispInfo = "LordGodUp.getDispInfo";

        public static RequestReturnObject getDispInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getDispInfo);
        }


    }
}
