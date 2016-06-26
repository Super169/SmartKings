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
        public static class Shop
        {
            public static List<CycleShopInfo> getCycleShopInfo(ConnectionInfo ci, string sid)
            {


                List<CycleShopInfo> csis = new List<CycleShopInfo>();
                RequestReturnObject rro = request.Shop.getCycleShopInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.Shop.items, typeof(DynamicJsonArray))) return csis;
                try
                {
                    DynamicJsonArray items = rro.responseJson[RRO.Shop.items];
                    foreach (dynamic item in items)
                    {
                        CycleShopInfo csi = new CycleShopInfo();
                        csi.pos = JSON.getInt(item, RRO.Shop.pos);
                        csi.res = JSON.getString(item, RRO.Shop.res, "");
                        csi.nm = JSON.getString(item, RRO.Shop.nm, "");
                        csi.amount = JSON.getInt(item, RRO.Shop.amount);
                        csi.sold = JSON.getBool(item, RRO.Shop.sold);
                        csis.Add(csi);
                    }
                }
                catch (Exception) { }

                return csis;
            }

            public static bool goCycleShopBuyItem(ConnectionInfo ci, string sid, int pos)
            {
                RequestReturnObject rro = request.Shop.buyCycleShopItem(ci, sid, pos);
                if (!rro.SuccessWithJson(RRO.Shop.pos) || (rro.style == STYLE.ERROR)) return false;
                int retPos = rro.getInt(RRO.Shop.pos, -1);
                return (retPos == pos);
            }

            public static int goSLShopBuyFood(ConnectionInfo ci, string sid)
            {
                PlayerProperties pp = action.player.getProperties(ci, sid);
                if (!pp.ready) return -1;
                if (pp.EXPLOIT < 93) return 0;

                RequestReturnObject rro = request.Shop2.shop2Info(ci, sid, RRO.Shop2.TYPE_SL_SHOP);
                if (!rro.SuccessWithJson(RRO.Shop2.remainBuyCount)) return -1;

                int coins = pp.EXPLOIT;
                int remainCount = rro.getInt(RRO.Shop2.remainBuyCount);
                int buyCount = 0;
                bool error = false;

                while ((!error) && (coins >= 93) && (remainCount > 0))
                {
                    rro = request.Shop2.buyItem(ci, sid, 1, 1, RRO.Shop2.TYPE_SL_SHOP);
                    error = (rro.ok != 1);
                    if (!error)
                    {
                        buyCount++;
                        remainCount--;
                        coins -= 93;
                    }
                }
                return buyCount;
            }

            public static int goIndustryBuyAll(ConnectionInfo ci, string sid, bool buyFood, bool buySilver)
            {
                PlayerProperties pp = action.player.getProperties(ci, sid);
                if (!pp.ready) return -1;
                int exploit = pp.EXPLOIT;
                int buyCnt = 0;

                List<Industry> industrys = getIndustgryList(ci, sid, "");
                foreach (Industry industry in industrys)
                {
                    if (industry.isMarket())
                    {
                        List<IndustryInfo> iis = getIndustryInfo(ci, sid, industry.industryId);
                        // Need to buy by position, so cnannot use foreach here
                        for (int idx = 0; idx < iis.Count; idx++)
                        {
                            IndustryInfo ii = iis.ElementAt(idx);
                            int itemCost = ii.ItemCost(exploit, buyFood, buySilver);
                            if (itemCost > 0)
                            {
                                RequestReturnObject rro = request.City.buyProduct(ci, sid, industry.industryId, idx);
                                if (rro.ok == 1)
                                {
                                    buyCnt++;
                                    exploit -= itemCost;
                                }
                            }
                        }
                    }
                }
                return buyCnt;
            }

            private static List<Industry> getIndustgryList(ConnectionInfo ci, string sid, string industryId)
            {
                List<Industry> industrys = new List<Industry>();
                RequestReturnObject rro = request.Corps.personIndustryList(ci, sid, industryId);
                if (rro.SuccessWithJson(RRO.Corps.items, typeof(DynamicJsonArray)))
                {
                    DynamicJsonArray items = rro.responseJson[RRO.Corps.items];
                    foreach (dynamic item in items)
                    {
                        Industry industry = new Industry();
                        industry.industryId = JSON.getInt(item, RRO.Corps.industryId);
                        industry.city = JSON.getString(item, RRO.Corps.city, "");
                        industry.type = JSON.getString(item, RRO.Corps.type, "");
                        industrys.Add(industry);
                    }
                }
                return industrys;
            }

            private static List<IndustryInfo> getIndustryInfo(ConnectionInfo ci, string sid, int industryId)
            {
                List<IndustryInfo> iis = new List<IndustryInfo>();
                RequestReturnObject rro = request.City.getIndustryInfo(ci, sid, industryId);
                if (!rro.SuccessWithJson(RRO.City.items, typeof(DynamicJsonArray))) return iis;

                DynamicJsonArray items = rro.responseJson[RRO.City.items];
                foreach (dynamic item in items)
                {
                    IndustryInfo ii = new IndustryInfo();
                    ii.config = JSON.getInt(item, RRO.City.config);
                    ii.discount = JSON.getInt(item, RRO.City.discount);
                    ii.sold = JSON.getBool(item, RRO.City.sold);
                    iis.Add(ii);
                }
                return iis;
            }
        }
    }
}