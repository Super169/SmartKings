
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
        public static LoginInfo getAccountInfo(ConnectionInfo ci, string sid)
        {
            LoginInfo info = new LoginInfo() { ready = false, sid = sid };
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
