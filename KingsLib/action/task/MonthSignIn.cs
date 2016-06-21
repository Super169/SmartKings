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

            public static bool goMonthSignIn(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string actionName = "簽到領獎";
                RequestReturnObject rro = request.MonthSignIn.getInfo(oGA.connectionInfo, oGA.sid);
                if (!rro.SuccessWithJson(RRO.MonthSignIn.today)) return false;
                int today = rro.getInt(RRO.MonthSignIn.today);
                if (today <= 0) return false;
                updateInfo(oGA.displayName, actionName, string.Format("完成第{0}天簽到", today), true, false);
                return true;
            }
        }
    }
}
