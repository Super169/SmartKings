using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class CorpsWar
    {
        private const string CMD_getInfo = "CorpsWar.getInfo";
        public static RequestReturnObject getInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getInfo);
        }


    }
}
