using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class System
    {
        private const string CMD_ping = "System.ping";

        public static RequestReturnObject ping(ConnectionInfo ci, string sid)
        {
            TimeSpan t = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
            Int64 jsTime = (Int64)(t.TotalMilliseconds + 0.5);
            string body = "{\"clientTime\":\"" + jsTime.ToString() + " \"}";
            return com.SendGenericRequest(ci, sid, CMD_ping, true, body);
        }

    }
}
