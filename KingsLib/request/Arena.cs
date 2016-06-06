
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Arena
    {
        private const string CMD_acceptRankReward = "Arena.acceptRankReward";
        private const string CMD_changeEnemies = "Arena.changeEnemies";
        private const string CMD_getDefFormation = "Arena.getDefFormation";
        private const string CMD_myArenaStatus = "Arena.myArenaStatus";

        public static RequestReturnObject acceptRankReward(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_acceptRankReward);
        }

        public static RequestReturnObject changeEnemies(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_changeEnemies);
        }

        public static RequestReturnObject getDefFormation(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getDefFormation);
        }

        public static RequestReturnObject myArenaStatus(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_myArenaStatus);
        }

    }
}
