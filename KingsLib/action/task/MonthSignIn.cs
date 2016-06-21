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
        public static partial class task
        {

            public static bool goSignIn(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string actionName = "簽到領獎";
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                RequestReturnObject rro = request.MonthSignIn.getInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.MonthSignIn.today)) return false;
                int today = rro.getInt(RRO.MonthSignIn.today);
                if (today <= 0) return false;
                updateInfo(oGA.displayName, actionName, string.Format("完成第{0}天簽到", today), true, false);

                return true;
            }

            public static bool goOneYearSignIn(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string actionName = "嘉年華";
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                int eventCount = action.OneYear.eventCount(ci, sid);
                if (eventCount < 0) return false;
                if (eventCount == 0)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, "今天沒有嘉年華活動");
                    return true;
                }

                int ticket = action.OneYear.drawTicket(ci, sid);
                if (ticket < 0)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, "登陸領取張嘉年華入場劵失敗");
                }
                else             if (ticket > 0)
                {
                    updateInfo(oGA.displayName, actionName, string.Format("登陸並領取了 {0} 張嘉年華入場劵", ticket), true, false);
                }
                return true;
            }
        }
    }
}
