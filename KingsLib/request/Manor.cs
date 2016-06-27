
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Manor
    {
        private const string CMD_appointHero = "Manor.appointHero";
        private const string CMD_appointInfo = "Manor.appointInfo";
        private const string CMD_armsTechnology = "Manor.armsTechnology";
        private const string CMD_autoAppoint = "Manor.autoAppoint";
        private const string CMD_currIncEffect = "Manor.currIncEffect";
        private const string CMD_decreeInfo = "Manor.decreeInfo";
        private const string CMD_doHeroActivity = "Manor.doHeroActivity";
        private const string CMD_getManorInfo = "Manor.getManorInfo";
        private const string CMD_harvestActivity = "Manor.harvestActivity";
        private const string CMD_harvestProduct = "Manor.harvestProduct";
        private const string CMD_heroScoreTips = "Manor.heroScoreTips";
        private const string CMD_moveBuilding = "Manor.moveBuilding";
        private const string CMD_produceArmamentInfo = "Manor.produceArmamentInfo";
        private const string CMD_refreshManor = "Manor.refreshManor";
        private const string CMD_resHourOutput = "Manor.resHourOutput";
        private const string CMD_retireAll = "Manor.retireAll";
        private const string CMD_trainHeroInfo = "Manor.trainHeroInfo";
        private const string CMD_upgradeArmsTechnology = "Manor.upgradeArmsTechnology";

        public static RequestReturnObject armsTechnology(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_armsTechnology);
        }

        public static RequestReturnObject decreeInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_decreeInfo);
        }

        public static RequestReturnObject getManorInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getManorInfo);
        }

        public static RequestReturnObject harvestActivity(ConnectionInfo ci, string sid, int field)
        {
            string body = string.Format("{{\"field\":{0}}}", field);
            return com.SendGenericRequest(ci, sid, CMD_harvestActivity, true, body);
        }

        public static RequestReturnObject harvestProduct(ConnectionInfo ci, string sid, int field)
        {
            string body = string.Format("{{\"field\":{0}}}", field);
            return com.SendGenericRequest(ci, sid, CMD_harvestProduct, true, body);
        }

        public static RequestReturnObject refreshManor(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_refreshManor);
        }

        public static RequestReturnObject resHourOutput(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_resHourOutput);
        }

        public static RequestReturnObject trainHeroInfo(ConnectionInfo ci, string sid, int field)
        {
            string body = string.Format("{{\"field\":{0}}}", field);
            return com.SendGenericRequest(ci, sid, CMD_trainHeroInfo, true, body);
        }



    }
}
