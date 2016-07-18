using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    class Escort
    {
        private const string CMD_getDefFormation = "Escort.getDefFormation";
        private const string CMD_loot = "Escort.loot";
        private const string CMD_worldInfo = "Escort.worldInfo";

        public static RequestReturnObject getDefFormation(ConnectionInfo ci, string sid, int id)
        {
            string body = string.Format("{{\"id\":{0}}}", id);
            return com.SendGenericRequest(ci, sid, CMD_getDefFormation, true, body);
        }

        public static RequestReturnObject loot(ConnectionInfo ci, string sid, int id)
        {
            string body = string.Format("{{\"id\":{0}}}", id);
            return com.SendGenericRequest(ci, sid, CMD_loot, true, body);
        }

        public static RequestReturnObject worldInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_worldInfo);
        }


    }
}
