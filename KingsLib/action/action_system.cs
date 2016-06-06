using Fiddler;
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
            if (rro.prompt == PROMPT_RELOGIN) return GameAccount.AccountStatus.Offline;
            if (rro.style == "ALERT")  return GameAccount.AccountStatus.Offline;
            if (!rro.SuccessWithJson("clientTime") || !rro.SuccessWithJson("serverTime")) return GameAccount.AccountStatus.Offline;
            long clientTime = JSON.getLong(rro.responseJson, "clientTime", 0);
            long serverTime = JSON.getLong(rro.responseJson, "serverTime", 0);
            timeAdjust = (int) (serverTime - clientTime);
            return GameAccount.AccountStatus.Online;
        }


    }
}
