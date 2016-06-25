
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Corps
    {
        private const string CMD_corpsCityReward = "Corps.corpsCityReward";
        private const string CMD_getCityRewardInfo = "Corps.getCityRewardInfo";
        private const string CMD_getCorpsJoinedPlayers = "Corps.getCorpsJoinedPlayers";
        private const string CMD_getCorpsMessageNum = "Corps.getCorpsMessageNum";
        private const string CMD_getJoinedCorps = "Corps.getJoinedCorps";
        private const string CMD_getNationalRank = "Corps.getNationalRank";
        private const string CMD_personIndustryList = "Corps.personIndustryList";
        private const string CMD_personIndustryRefresh = "Corps.personIndustryRefresh";
        private const string CMD_playerTech = "Corps.playerTech";
        private const string CMD_takeZhanjiStep = "Corps.takeZhanjiStep";
        private const string CMD_upgradePlayerTech = "Corps.upgradePlayerTech";

        public static RequestReturnObject corpsCityReward(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_corpsCityReward);
        }

        public static RequestReturnObject getCityRewardInfo(ConnectionInfo ci, string sid, int step)
        {
            string body = string.Format("{{\"step\":{0}}}", step);
            return com.SendGenericRequest(ci, sid, CMD_getCityRewardInfo, true, body);
        }

        public static RequestReturnObject getCorpsMessageNum(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getCorpsMessageNum);
        }

        public static RequestReturnObject getJoinedCorps(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getJoinedCorps);
        }

        public static RequestReturnObject getNationalRank(ConnectionInfo ci, string sid, int page)
        {
            string body = string.Format("{{\"page\":{0}}}", page);
            return com.SendGenericRequest(ci, sid, CMD_getNationalRank, true, body);
        }


        public static RequestReturnObject personIndustryRefresh(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_personIndustryRefresh);
        }

        public static RequestReturnObject playerTech(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_playerTech);
        }

        public static RequestReturnObject takeZhanjiStep(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_takeZhanjiStep);
        }




    }
}
