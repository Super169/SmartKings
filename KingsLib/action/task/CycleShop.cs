using KingsLib.data;
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
            public static bool goCycleShop(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                
                int dow = scheduler.Scheduler.getGameDOW();
                if ((dow != 0) && (dow != 3)) return true;

                DateTime now = DateTime.Now;
                string actionName = "東瀛寶船";

                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                List<CycleShopInfo> csis = action.Shop.getCycleShopInfo(ci, sid);
                foreach (CycleShopInfo csi in csis)
                {
                    // No way, can only check using this string, or hard code the position.  Text has been converted to TradChinese
                    if ((csi.pos < 3) && (!csi.sold) && ((csi.res == "銀兩") || (csi.res == "银两")))
                    {
                        // string info = string.Format("用 {0} 銀買 {1} : ", csi.amount, csi.nm);
                        string info = string.Format("用銀買 {0} : ", csi.nm);
                        if (action.Shop.goCycleShopBuyItem(ci, sid, csi.pos))
                        {
                            info += "成功";
                        }
                        else
                        {
                            info += "失敗";
                        }
                        updateInfo(oGA.displayName, actionName, info , true, false);
                    }
                }
                return true;
            }
        }
    }
}
