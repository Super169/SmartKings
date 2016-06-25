
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Campaign
    {
        private const string CMD_eliteBuyTime = "Campaign.eliteBuyTime";
        private const string CMD_eliteFight = "Campaign.eliteFight";
        private const string CMD_eliteGetAllInfos = "Campaign.eliteGetAllInfos";
        private const string CMD_eliteGetCampaignInfo = "Campaign.eliteGetCampaignInfo";
        private const string CMD_evalMarchFormation = "Campaign.evalMarchFormation";
        private const string CMD_fightNext = "Campaign.fightNext";
        private const string CMD_getAttFormation = "Campaign.getAttFormation";
        private const string CMD_getLeftTimes = "Campaign.getLeftTimes";
        private const string CMD_getTrialsInfo = "Campaign.getTrialsInfo";
        private const string CMD_nextEnemies = "Campaign.nextEnemies";
        private const string CMD_quitCampaign = "Campaign.quitCampaign";
        private const string CMD_saveFormation = "Campaign.saveFormation";
        private const string CMD_trialsBuyTimes = "Campaign.trialsBuyTimes";

        public static RequestReturnObject eliteBuyTime(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_eliteBuyTime);
        }

        public static RequestReturnObject eliteFight(ConnectionInfo ci, string sid, string difficult, int stage, int chapter)
        {
            string body = string.Format("{{\"difficult\":\"{0}\", \"stage\":{1}, \"chapter\":{2}}}", difficult, stage, chapter);
            return com.SendGenericRequest(ci, sid, CMD_eliteFight, true, body);
        }

        public static RequestReturnObject eliteGetCampaignInfo(ConnectionInfo ci, string sid, string difficult, int chapter)
        {
            string body = string.Format("{{\"difficult\":\"{0}\", \"chapter\":{1}}}", difficult, chapter);
            return com.SendGenericRequest(ci, sid, CMD_eliteGetCampaignInfo, true, body);
        }

        public static RequestReturnObject eliteGetAllInfos(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_eliteGetAllInfos);
        }

        public static RequestReturnObject fightNext(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_fightNext);
        }

        public static RequestReturnObject getAttFormation(ConnectionInfo ci, string sid, string march)
        {
            string body = string.Format("{{\"march\":\"{0}\"}}", march);
            return com.SendGenericRequest(ci, sid, CMD_getAttFormation, true, body);
        }
        
        public static RequestReturnObject getLeftTimes(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getLeftTimes);
        }

        public static RequestReturnObject getTrialsInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getTrialsInfo);
        }

        public static RequestReturnObject nextEnemies(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_nextEnemies);
        }

        public static RequestReturnObject saveFormation(ConnectionInfo ci, string sid, string body)
        {
            return com.SendGenericRequest(ci, sid, CMD_saveFormation, true, body);
        }

        public static RequestReturnObject quitCampaign(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_quitCampaign);
        }

        public static RequestReturnObject trialsBuyTimes(ConnectionInfo ci, string sid, string type)
        {
            string body = string.Format("{{\"type\":\"{0}\"}}", type);
            return com.SendGenericRequest(ci, sid, CMD_trialsBuyTimes, true, body);
        }

    }
}
