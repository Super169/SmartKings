
using KingsLib.data;
using KingsLib.request;
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
        public static class OneYear
        {
            public static int eventCount(ConnectionInfo ci, string sid)
            {
                RequestReturnObject rro = request.OneYear.cityStatus(ci, sid);
                if (!rro.success) return -1;
                if (!rro.exists(RRO.OneYear.activityStatus, typeof(DynamicJsonArray))) return -1;
                long currTime = system.getTime(ci, sid) / 1000;
                DynamicJsonArray activityStatus = rro.responseJson[RRO.OneYear.activityStatus];
                int eventCount = 0;
                foreach (dynamic o in activityStatus)
                {
                    if (JSON.exists(o, RRO.OneYear.startTime) && JSON.exists(o, RRO.OneYear.endTime))
                    {
                        long startTime = JSON.getLong(o, RRO.OneYear.startTime);
                        long endTime = JSON.getLong(o, RRO.OneYear.endTime);
                        if ((startTime < currTime) && (currTime < endTime))
                        {
                            eventCount++;
                        }
                    }
                }
                return eventCount;
            }

            // 嘉年华入场券
            public static int checkTicket(ConnectionInfo ci, string sid)
            {
                RequestReturnObject rro;
                rro = request.Bag.getBagInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.Bag.items, typeof(DynamicJsonArray))) return -1;
                int ticket = 0;
                DynamicJsonArray dja = rro.responseJson[RRO.Bag.items];
                foreach (dynamic o in dja)
                {
                    if (JSON.getString(o, RRO.Bag.nm, "") == RRO.Bag.nm_ticket)
                    {
                        ticket += JSON.getInt(o, RRO.Bag.n, 0);
                    }
                }
                return ticket;
            }

            public static int drawTicket(ConnectionInfo ci, string sid)
            {
                RequestReturnObject rro;
                rro = request.OneYearEntry.getOneYearEntryInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.OneYearEntry.entryLists, typeof(DynamicJsonArray))) return 0;

                DynamicJsonArray dja = (DynamicJsonArray) rro.responseJson[RRO.OneYearEntry.entryLists];

                int totalCount = 0;
                foreach (dynamic o in dja)
                {
                    int thisCount = 0;
                    if (JSON.getString(o, RRO.OneYearEntry.type, "") == RRO.OneYearEntry.KEY_SignInType)
                    {
                        bool isDraw = JSON.getBool(o, RRO.OneYearEntry.isDraw, true);
                        if (!isDraw)
                        {
                            string rewardList = "";
                            if ((JSON.exists(o, RRO.OneYearEntry.reward)) && 
                                 JSON.exists(o[RRO.OneYearEntry.reward][RRO.OneYearEntry.itemDefs], typeof(DynamicJsonArray)))
                            {
                                DynamicJsonArray itemDefs = o[RRO.OneYearEntry.reward][RRO.OneYearEntry.itemDefs];
                                foreach (dynamic i in itemDefs)
                                {
                                    if (JSON.exists(i, RRO.OneYearEntry.name) && JSON.exists(i, RRO.OneYearEntry.num))
                                    {
                                        string name = JSON.getString(i, RRO.OneYearEntry.name, null);
                                        int num = JSON.getInt(i, RRO.OneYearEntry.num);
                                        rewardList += (rewardList == "" ? "" : ", ") + name + " x " + num.ToString();
                                        thisCount += num;
                                    }
                                }
                            }
                            rro = request.OneYearEntry.draw(ci, sid, RRO.OneYearEntry.KEY_SignInType);
                            if (rro.ok == 1) totalCount += thisCount;
                        }
                    }
                }
                // TODO: Just assume all of them are tickets (may need to fine tune this part)
                return totalCount;
            }


            // 點將台
            public static int checkDianJiangTai(ConnectionInfo ci, string sid)
            {
                RequestReturnObject rro;
                rro = request.DianJiangTai.beforeStart(ci, sid);
                if (!rro.SuccessWithJson(RRO.DianJiangTai.leftTimes)) return -1;
                int leftTimes = rro.getInt(RRO.DianJiangTai.leftTimes);
                return leftTimes;
            }

            // 仙鶴雲居
            public static int checkRemain(ConnectionInfo ci, string sid)
            {
                RequestReturnObject rro;
                rro = request.OneYear.info(ci, sid);
                if (!rro.SuccessWithJson(RRO.OneYear.remainCount)) return -1;
                int remainCount = rro.getInt(RRO.OneYear.remainCount);
                return remainCount;
            }
        }

    }
}
