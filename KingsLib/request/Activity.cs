using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Activity
    {
        private const string CMD_drawCompanyAnniversaryLoginReward = "Activity.drawCompanyAnniversaryLoginReward";
        private const string CMD_drawCompanyAnniversaryRechargeReward = "Activity.drawCompanyAnniversaryRechargeReward";
        private const string CMD_drawExchangeHoliday = "Activity.drawExchangeHoliday";
        private const string CMD_drawStrategicFundInfo = "Activity.drawStrategicFundInfo";
        private const string CMD_getActivityList = "Activity.getActivityList";
        private const string CMD_getAnnouncement = "Activity.getAnnouncement";
        private const string CMD_getBookHeroInfo = "Activity.getBookHeroInfo";
        private const string CMD_getCloudSellerInfo = "Activity.getCloudSellerInfo";
        private const string CMD_getPlayerGoBackActivityInfo = "Activity.getPlayerGoBackActivityInfo";
        private const string CMD_getRankInfo = "Activity.getRankInfo";
        private const string CMD_getRationActivity = "Activity.getRationActivity";
        private const string CMD_getRedPointActivityInfo = "Activity.getRedPointActivityInfo";
        private const string CMD_getShuangShiyiActivityInfo = "Activity.getShuangShiyiActivityInfo";
        private const string CMD_getShuangShiyiActivityReward = "Activity.getShuangShiyiActivityReward";
        private const string CMD_getTuanGouInfo = "Activity.getTuanGouInfo";
        private const string CMD_getTuanGouOpenInfo = "Activity.getTuanGouOpenInfo";
        private const string CMD_serverOpenTime = "Activity.serverOpenTime";
        private const string CMD_showIconForServerOpenActivity = "Activity.showIconForServerOpenActivity";
        private const string CMD_tuanGouReward = "Activity.tuanGouReward";

        public static RequestReturnObject drawCompanyAnniversaryLoginReward(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_drawCompanyAnniversaryLoginReward);
        }

        public static RequestReturnObject drawCompanyAnniversaryRechargeReward(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_drawCompanyAnniversaryRechargeReward);
        }

        public static RequestReturnObject drawExchangeHoliday(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_drawExchangeHoliday);
        }

        public static RequestReturnObject drawStrategicFundInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_drawStrategicFundInfo);
        }

        public static RequestReturnObject getActivityList(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getActivityList);
        }

        public static RequestReturnObject getAnnouncement(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getAnnouncement);
        }

        public static RequestReturnObject getBookHeroInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getBookHeroInfo);
        }

        public static RequestReturnObject getCloudSellerInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getCloudSellerInfo);
        }

        public static RequestReturnObject getPlayerGoBackActivityInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getPlayerGoBackActivityInfo);
        }

        public static RequestReturnObject getRankInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getRankInfo);
        }

        public static RequestReturnObject getRationActivity(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getRationActivity);
        }


        public static RequestReturnObject getShuangShiyiActivityInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getShuangShiyiActivityInfo);
        }

        public static RequestReturnObject getShuangShiyiActivityReward(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getShuangShiyiActivityReward);
        }

        public static RequestReturnObject getTuanGouInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getTuanGouInfo);
        }

        public static RequestReturnObject getTuanGouOpenInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getTuanGouOpenInfo);
        }

        public static RequestReturnObject serverOpenTime(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_serverOpenTime);
        }

        public static RequestReturnObject showIconForServerOpenActivity(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_showIconForServerOpenActivity);
        }

        public static RequestReturnObject tuanGouReward(ConnectionInfo ci, string sid, int bagId)
        {
            // string body = "{\"bagId\":" + bagId.ToString() + "}";
            string body = string.Format("{{\"bagId\":{0}}}", bagId);
            return com.SendGenericRequest(ci, sid, CMD_tuanGouReward, true, body);
        }



    }
}
