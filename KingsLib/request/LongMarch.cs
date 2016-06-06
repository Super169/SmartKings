
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class LongMarch
    {
        private const string CMD_getBuyResetTimes = "LongMarch.getBuyResetTimes";
        private const string CMD_getFinishedReward = "LongMarch.getFinishedReward";
        private const string CMD_getHeroStatus = "LongMarch.getHeroStatus";
        private const string CMD_getMyStatus = "LongMarch.getMyStatus";
        private const string CMD_getUnpassBuff = "LongMarch.getUnpassBuff";
        private const string CMD_restart = "LongMarch.restart";

        public static RequestReturnObject getBuyResetTimes(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getBuyResetTimes);
        }

        public static RequestReturnObject getFinishedReward(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getFinishedReward);
        }

        public static RequestReturnObject getHeroStatus(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getHeroStatus);
        }

        public static RequestReturnObject getMyStatus(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getMyStatus);
        }

        public static RequestReturnObject getUnpassBuff(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getUnpassBuff);
        }

        public static RequestReturnObject restart(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_restart);
        }


    }
}
