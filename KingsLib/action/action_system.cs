
using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class action
    {
        public static GameAccount.AccountStatus goCheckAccountStatus(ConnectionInfo ci, string sid, ref int timeAdjust)
        {
            if ((sid == null) || (sid == "")) return GameAccount.AccountStatus.Offline;
            RequestReturnObject rro = request.System.ping(ci, sid);
            if (!rro.success) return GameAccount.AccountStatus.Unknown;
            if (rro.prompt == PROMPT.ERR_COMMON_RELOGIN) return GameAccount.AccountStatus.Offline;
            if (rro.style == "ALERT")  return GameAccount.AccountStatus.Offline;
            if (!rro.SuccessWithJson("clientTime") || !rro.SuccessWithJson("serverTime")) return GameAccount.AccountStatus.Offline;
            long clientTime = JSON.getLong(rro.responseJson, "clientTime", 0);
            long serverTime = JSON.getLong(rro.responseJson, "serverTime", 0);
            timeAdjust = (int) (serverTime - clientTime);
            return GameAccount.AccountStatus.Online;
        }

        public static long getSystemTime(ConnectionInfo ci, string sid)
        {
            long currTime = 0;
            RequestReturnObject rro = request.System.ping(ci, sid);
            if (rro.SuccessWithJson(RRO.serverTime))
            {
                currTime = JSON.getLong(rro.responseJson, RRO.serverTime);
            }
            else
            {
                TimeSpan t = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
                currTime = (long)(t.TotalMilliseconds + 0.5);
            }
            return currTime;
        }


    }
}
