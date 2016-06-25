
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Patrol
    {
        private const string CMD_dealPatroledEvent = "Patrol.dealPatroledEvent";
        private const string CMD_getPatrolInfo = "Patrol.getPatrolInfo";

        public static RequestReturnObject getPatrolInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getPatrolInfo);
        }

        public static RequestReturnObject dealPatroledEvent(ConnectionInfo ci, string sid, int cityId)
        {
            string body = string.Format("{{\"cityId\":{0}}}", cityId);
            return com.SendGenericRequest(ci, sid, CMD_dealPatroledEvent, true, body);
        }
    }
}
