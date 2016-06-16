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
            private const int MIN_HARVEST = 100;

            public static bool goTaskHarvest(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string actionName = "封地收獲";
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                PlayerProperties pp = getPlayerProperties(ci, sid);
                if (!pp.ready) return false;
                if ((pp.FOOD >= pp.MAX_FOOD) && (pp.SILVER >= pp.MAX_SILVER) && (pp.IRON >= pp.MAX_IRON))
                {
                    updateInfo(oGA.displayName, actionName, "各項資源都滿了", true, false);
                    return true;
                }

                List<ManorInfo> mis = getManorInfo(ci, sid);
                int getSILVER = 0, getFOOD = 0, getIRON = 0;

                foreach (ManorInfo mi in mis)
                {
                    int outProducts = 0;
                    switch (mi.type)
                    {
                        case "MH":
                        case "SP":
                            if ((mi.products > MIN_HARVEST) && (pp.SILVER < pp.MAX_SILVER))
                            {
                                outProducts = goHarvestField(ci, sid, mi.field);
                                if (outProducts > 0)
                                {
                                    // updateInfo(string.Format("收取 {0} 的銀 {1}", mi.field, mi.products));
                                    getSILVER += outProducts;
                                    pp.SILVER += outProducts;
                                }
                            }
                            break;
                        case "NT":
                        case "MC":
                            if ((mi.products > MIN_HARVEST) && (pp.FOOD < pp.MAX_FOOD))
                            {
                                outProducts = goHarvestField(ci, sid, mi.field);
                                if (outProducts > 0)
                                {
                                    // updateInfo(string.Format("收取 {0} 的糧 {1}", mi.field, mi.products));
                                    getFOOD += outProducts;
                                    pp.FOOD += outProducts;
                                }
                            }
                            break;
                        case "LTC":
                            if ((mi.products > MIN_HARVEST) && (pp.IRON < pp.MAX_IRON))
                            {
                                outProducts = goHarvestField(ci, sid, mi.field);
                                if (outProducts > 0)
                                {
                                    // updateInfo(string.Format("收取 {0} 的鐵 {1}", mi.field, mi.products));
                                    getIRON += outProducts;
                                    pp.IRON += outProducts;
                                }
                            }
                            break;
                    }
                }
                if (getSILVER + getFOOD + getIRON > 0)
                {
                    updateInfo(oGA.displayName, actionName, string.Format("收取 {0} 銀, {1} 糧, {2} 鐵", getSILVER, getFOOD, getIRON), true, false);
                }
                return true;
            }
        }
    }
}
