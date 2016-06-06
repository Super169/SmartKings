
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class NorthMarch
    {
        private const string CMD_enterWar = "NorthMarch.enterWar";
        private const string CMD_inMissionHeros = "NorthMarch.inMissionHeros";
        private const string CMD_leaveWar = "NorthMarch.leaveWar";
        private const string CMD_northCitySituation = "NorthMarch.northCitySituation";
        private const string CMD_retreatAllTroops = "NorthMarch.retreatAllTroops";
        private const string CMD_sendTroops = "NorthMarch.sendTroops";

        public static RequestReturnObject enterWar(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_enterWar);
        }

        public static RequestReturnObject inMissionHeros(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_inMissionHeros);
        }

        public static RequestReturnObject leaveWar(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_leaveWar);
        }

        public static RequestReturnObject northCitySituation(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_northCitySituation);
        }

        public static RequestReturnObject retreatAllTroops(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_retreatAllTroops);
        }


    }
}
