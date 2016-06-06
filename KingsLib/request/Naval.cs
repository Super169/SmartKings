
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Naval
    {
        private const string CMD_enterWar = "Naval.enterWar";
        private const string CMD_getInfo = "Naval.getInfo";
        private const string CMD_getVersusCount = "Naval.getVersusCount";
        private const string CMD_inMissionHeros = "Naval.inMissionHeros";
        private const string CMD_killRank = "Naval.killRank";
        private const string CMD_leaveWar = "Naval.leaveWar";
        private const string CMD_retreatAllTroops = "Naval.retreatAllTroops";
        private const string CMD_rewardCfg = "Naval.rewardCfg";
        private const string CMD_sendTroops = "Naval.sendTroops";

        public static RequestReturnObject getInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getInfo);
        }

        public static RequestReturnObject inMissionHeros(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_inMissionHeros);
        }

        public static RequestReturnObject killRank(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_killRank);
        }

        public static RequestReturnObject leaveWar(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_leaveWar);
        }

        public static RequestReturnObject rewardCfg(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_rewardCfg);
        }


    }
}
