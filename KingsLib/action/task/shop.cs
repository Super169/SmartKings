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

            public static bool goMarket(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                string actionName = "糧草先行";

                RequestReturnObject rro;
                rro = request.Task.getTaskTraceInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.Task.tasks, typeof(DynamicJsonArray))) return false;
                DynamicJsonArray tasks = (DynamicJsonArray)rro.responseJson["tasks"];
                dynamic o = tasks.FirstOrDefault(x => JSON.getInt(x, RRO.Task.id) == 8);
                if (o == null) return false;
                if (JSON.getString(o, RRO.Task.status, "") != RRO.Task.status_ACC) return false;

                string buyType = "IRON";

                rro = request.Shop.tradeResource(ci, sid, buyType);
                if ((!rro.SuccessWithJson("resource")) ||
                    (JSON.getString(rro.responseJson, "resource", "") != buyType))
                {
                    // Try to buy food if IRON is not available
                    buyType = "FOOD";
                    rro = request.Shop.tradeResource(ci, sid, buyType);
                    if ((!rro.SuccessWithJson("resource")) ||
                        (JSON.getString(rro.responseJson, "resource", "") != buyType))
                    {
                        return false;
                    }
                }
                updateInfo(oGA.displayName, actionName, string.Format("市集: 為任務購買 {0}", buyType), true, false);

                return true;
            }

            public static bool goSLShop(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                string actionName = "勢力商店";

                int buyCount = action.Shop.goSLShopBuyFood(ci, sid);
                if (buyCount < 0)
                {
                    return false;
                } else if (buyCount > 0)
                {
                    updateInfo(oGA.displayName, actionName, string.Format("購買了 {0} 次糧食", buyCount), true, false);
                }
                return true;
            }

            public static bool goIndustryShop(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                string actionName = "勢力市集";

                return true;
            }

        }
    }
}
