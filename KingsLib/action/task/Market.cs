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
            public static bool goMarket(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                string actionName = "糧草先行";


                RequestReturnObject rro;
                rro = request.Task.getTaskTraceInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.Task.tasks, typeof(DynamicJsonArray))) return false;
                DynamicJsonArray tasks = (DynamicJsonArray)rro.responseJson["tasks"];
                dynamic o = tasks.FirstOrDefault(x => JSON.getInt(x, "id") == 8);
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
        }
    }
}