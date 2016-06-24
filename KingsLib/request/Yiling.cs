
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Yiling
    {
        private const string CMD_attack = "Yiling.attack";
        private const string CMD_getStatus = "Yiling.getStatus";
        private const string CMD_mapStatuts = "Yiling.mapStatuts";
        private const string CMD_viewEnemies = "Yiling.viewEnemies";

        public static RequestReturnObject attack(ConnectionInfo ci, string sid, int step)
        {
            string body = string.Format("{{\"step\":{0}}}", step);
            return com.SendGenericRequest(ci, sid, CMD_attack, true, body);
        }
        public static RequestReturnObject getStatus(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getStatus);
        }

        public static RequestReturnObject mapStatuts(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_mapStatuts);
        }

        public static RequestReturnObject viewEnemies(ConnectionInfo ci, string sid, int step)
        {
            string body = string.Format("{{\"step\":{0}}}", step);
            return com.SendGenericRequest(ci, sid, CMD_viewEnemies, true, body);
        }

    }
}
