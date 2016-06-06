using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class KingRoad
    {
        private const string CMD_afterSeasonEnemy = "KingRoad.afterSeasonEnemy";
        private const string CMD_kingroadEnd = "KingRoad.kingroadEnd";
        private const string CMD_kingroadState = "KingRoad.kingroadState";
        private const string CMD_seasonChallenge = "KingRoad.seasonChallenge";

        public static RequestReturnObject afterSeasonEnemy(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_afterSeasonEnemy);
        }

        public static RequestReturnObject kingroadEnd(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_kingroadEnd);
        }

        public static RequestReturnObject kingroadState(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_kingroadState);
        }


    }
}
