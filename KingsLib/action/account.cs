
using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib
{
    public static partial class action
    {
        public static class account
        {
            public static GameAccount.AccountStatus checkStatus(ConnectionInfo ci, string sid, ref int timeAdjust)
            {
                if ((sid == null) || (sid == "")) return GameAccount.AccountStatus.Offline;
                RequestReturnObject rro = request.System.ping(ci, sid);
                if (!rro.success) return GameAccount.AccountStatus.Unknown;
                if (rro.prompt == PROMPT.ERR_COMMON_RELOGIN) return GameAccount.AccountStatus.Offline;
                if (rro.style == STYLE.ALERT) return GameAccount.AccountStatus.Offline;
                if (!rro.SuccessWithJson(RRO.System.clientTime) || !rro.SuccessWithJson(RRO.System.serverTime)) return GameAccount.AccountStatus.Offline;
                long clientTime = rro.getLong(RRO.System.clientTime, 0);
                long serverTime = rro.getLong(RRO.System.serverTime, 0);
                timeAdjust = (int)(serverTime - clientTime);
                return GameAccount.AccountStatus.Online;
            }

            public static AccountInfo getInfo(ConnectionInfo ci, string sid)
            {
                AccountInfo info = new AccountInfo() { ready = false, sid = sid };
                RequestReturnObject rro = request.Login.login(ci, sid);
                if (!rro.SuccessWithJson(RRO.Login.account)) return info;
                info.account = rro.getString(RRO.Login.account, "");
                info.serverTitle = rro.getString(RRO.Login.serverTitle, "");
                info.nickName = rro.getString(RRO.Login.nickName, "");

                rro = request.Player.getProperties(ci, sid);
                if (!rro.SuccessWithJson(RRO.Player.pvs, typeof(DynamicJsonArray))) return info;
                // only assign the sid here if all data is ready. or should it use other field like isReady?
                DynamicJsonArray pvs = (DynamicJsonArray)rro.responseJson.pvs;
                foreach (dynamic pv in pvs)
                {
                    if (JSON.getString(pv, RRO.Player.p, "") == RRO.Player.CORPS_NAME) info.CORPS_NAME = JSON.getString(pv, RRO.Player.v, "");
                    else if (JSON.getString(pv, RRO.Player.p, "") == RRO.Player.LEVEL) info.LEVEL = JSON.getString(pv, RRO.Player.v, "");
                    else if (JSON.getString(pv, RRO.Player.p, "") == RRO.Player.VIP_LEVEL) info.VIP_LEVEL = JSON.getString(pv, RRO.Player.v, "");
                }
                info.ready = true;
                return info;
            }
        }
    }
}
