﻿
using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

// Checking included:
// Check 討伐群雄
// Check 英雄切磋
// 英雄試練
// 王者獎勵/保級賽
// 周遊天下
// 遠征西域
// 嘉年華活動
// 神器打造
// 誇服入侵

namespace KingsLib
{
    public static partial class action
    {
        public delegate void DelegateUpdateInfo(string account, string action, string msg, bool addTime = true, bool async = true);

        public delegate bool DelegateCheckOutstandingTask(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug);

        private static void showDebugMsg(DelegateUpdateInfo updateInfo, string account, string action, string msg)
        {
            updateInfo(account, action, "**** " + msg, true, false);
        }

        public static bool checkAllOutstandingTasks(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug = false)
        {
            string action = "遺漏任務";

            if (debug) showDebugMsg(updateInfo, oGA.displayName, action, "開始");

            // 討伐群雄
            goCheckOutstandTasks(action, "討伐群雄", checkOutstandingCampaignElite, oGA, updateInfo, debug);

            // 英雄切磋
            goCheckOutstandTasks(action, "英雄切磋", checkOutstandingVisitHero, oGA, updateInfo, debug);

            // 英雄試練
            goCheckOutstandTasks(action, "英雄試練", checkOutstandingCampainTrials, oGA, updateInfo, debug);

            // 王者獎勵/保級賽
            goCheckOutstandTasks(action, "王者獎勵", checkOutstandingAfterKingRoad, oGA, updateInfo, debug);

            // 周遊天下
            goCheckOutstandTasks(action, "周遊天下", checkOutstandingTravel, oGA, updateInfo, debug);

            // 遠征西域
            goCheckOutstandTasks(action, "遠征西域", checkOutstandingLongMarch, oGA, updateInfo, debug);

            // 嘉年華活動
            goCheckOutstandTasks(action, "嘉年華活動", checkOutstandingOneYear, oGA, updateInfo, debug);

            // 神器打造
            goCheckOutstandTasks(action, "神器打造", checkOutstandingHeroAfi, oGA, updateInfo, debug);

            // 誇服入侵
            goCheckOutstandTasks(action, "誇服入侵", checkOutstandingNaval, oGA, updateInfo, debug);

            if (debug) showDebugMsg(updateInfo, oGA.displayName, action, "結束");

            return true;
        }

        private static void goCheckOutstandTasks(string action, string module, DelegateCheckOutstandingTask checkTask, GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
        {
            if (debug) showDebugMsg(updateInfo, oGA.displayName, action, string.Format("{0}: {1}", "開始檢查", module));
            if (!checkTask(oGA, updateInfo, action, module, debug))
            {
                updateInfo(oGA.displayName, action, module + ": 檢查失敗", true, false);
            }
            if (debug) showDebugMsg(updateInfo, oGA.displayName, action, string.Format("{0}: {1}", "結束檢查", module));
        }

        public static bool checkOutstandingCampaignElite(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug)
        {
            RequestReturnObject rro = request.Campaign.getLeftTimes(oGA.connectionInfo, oGA.sid);
            if (!(rro.SuccessWithJson(RRO.Campaign.elite) && rro.Exists(RRO.Campaign.eliteBuyTimes) && rro.Exists(RRO.Campaign.eliteCanBuyTimes))) return false;

            int elite = JSON.getInt(rro.responseJson, RRO.Campaign.elite, -1);
            int eliteBuyTimes = JSON.getInt(rro.responseJson, RRO.Campaign.eliteBuyTimes, -1);
            int eliteCanBuyTimes = JSON.getInt(rro.responseJson, RRO.Campaign.eliteCanBuyTimes, -1);
            if ((elite > 0) || ((eliteCanBuyTimes > 0) && (eliteBuyTimes == 0)))
            {
                string msg = module + ": ";
                if (elite > 0) msg += string.Format("尚餘 {0} 次 ", elite);
                if ((eliteCanBuyTimes > 0) && (eliteBuyTimes == 0)) msg += "尚未購買額外次數";
                updateInfo(oGA.displayName, action, msg, true, false);
            }
            return true;
        }

        public static bool checkOutstandingVisitHero(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug)
        {
            RequestReturnObject rro = request.Hero.getVisitHeroInfo(oGA.connectionInfo, oGA.sid, "徐晃");
            if (!rro.SuccessWithJson(RRO.Hero.matchTimes)) return false;
            int matchTimes = JSON.getInt(rro.responseJson, RRO.Hero.matchTimes, -1);
            if (matchTimes > 0)
            {
                string msg = string.Format("{0}: 尚有 {1} 次未完成", module, matchTimes);
                updateInfo(oGA.displayName, action, msg, true, false);
            }
            return true;
        }

        public static bool checkOutstandingCampainTrials(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug)
        {

            RequestReturnObject rro = request.Campaign.getTrialsInfo(oGA.connectionInfo, oGA.sid);
            if (!(rro.SuccessWithJson(RRO.Campaign.weekday) && rro.Exists(RRO.Campaign.times) && rro.Exists(RRO.Campaign.buyTimes))) return false;
            int weekday = JSON.getInt(rro.responseJson[RRO.Campaign.weekday]);
            dynamic times = rro.responseJson[RRO.Campaign.times];
            dynamic buyTimes = rro.responseJson[RRO.Campaign.buyTimes];
            string[] trialType = { "", "WZLJ", "WJDD", "WHSJ" };
            string[] trialName = { "", "五子良將", "五俊都督", "五虎上將" };
            string msgRemain = "";
            bool remain = false;
            string msgNotBuy = "";
            bool notBuy = false;
            for (int idx = 1; idx <= 3; idx++)
            {

                if ((weekday == 0) || (weekday == 7) || (weekday == idx) || (weekday == idx + 3))
                {
                    int remainTimes = JSON.getInt(times, trialType[idx]);
                    if (remainTimes > 0)
                    {
                        remain = true;
                        msgRemain += string.Format("{0} - {1} 次 ", trialName[idx], remainTimes);
                    }

                    int BuyTimes = JSON.getInt(buyTimes, trialName[idx]);
                    if ((oGA.vipLevel > (idx - 1)) && (BuyTimes == 0))
                    {
                        notBuy = true;
                        msgNotBuy += trialName[idx] + " ";
                    }

                }

            }
            if (remain || notBuy)
            {
                string msg = module + ": ";
                if (remain) msg += "尚餘 " + msgRemain + "未完成; ";
                if (notBuy) msg += "尚未購買 " + msgNotBuy;
                updateInfo(oGA.displayName, action, msg, true, false);
            }
            return true;
        }

        public static bool checkOutstandingAfterKingRoad(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug)
        {
            int gameDOW = scheduler.Schedule.ScheduleInfo.getGameDOW();
            if (!((gameDOW == 3) || (gameDOW == 6)))
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, action, "今天沒有王者獎勵/保級賽");
                return true;
            }
            if (DateTime.Now > scheduler.Schedule.ScheduleInfo.getRefTime(DateTime.Now, new TimeSpan(21, 0, 0)))
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, action, "王者獎勵/保級賽 已經結束");
                return true;
            }
            RequestReturnObject rro = request.KingRoad.afterSeasonEnemy(oGA.connectionInfo, oGA.sid);
            if (!(rro.SuccessWithJson(RRO.KingRoad.remainChallenge) &&
                  rro.Exists(RRO.KingRoad.seasonType) &&
                  rro.Exists(RRO.KingRoad.enemy, typeof(DynamicJsonArray))))
            {
                return false;
            }

            int remainChallenge = JSON.getInt(rro.responseJson, RRO.KingRoad.remainChallenge);
            if (remainChallenge > 0)
            {
                int failCnt = 0;
                DynamicJsonArray dja = rro.responseJson[RRO.KingRoad.enemy];
                foreach (dynamic o in dja)
                {
                    int star = JSON.getInt(o, RRO.KingRoad.star, 0);
                    if (star < 3) failCnt++;
                }
                if (failCnt > 0)
                {
                    string seasonType = JSON.getString(rro.responseJson, RRO.KingRoad.seasonType, "UNKNOWN");
                    switch (seasonType)
                    {
                        case RRO.KingRoad.seasonType_TO_KEEP:
                            seasonType = "保級";
                            break;
                        case RRO.KingRoad.seasonType_GIFT:
                            seasonType = "獎勵";
                            break;

                    }
                    updateInfo(oGA.displayName, action, string.Format("王者{0}賽: 餘下 {1} 次挑戰, 尚有 {2} 個未達三星", seasonType, remainChallenge, failCnt), true, false);
                }
            }
            return true;
        }

        public static bool checkOutstandingTravel(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug)
        {
            RequestReturnObject rro = request.Travel.getMapInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (rro.prompt == PROMPT.ACTIVITY_CAN_NOT_PARTICIPATE) return true;
            if (!(rro.Exists(RRO.Travel.diceNum))) return false;
            int diceNum = JSON.getInt(rro.responseJson, RRO.Travel.diceNum);
            if (diceNum > 0)
            {
                updateInfo(oGA.displayName, action, string.Format("{0}: 還有 {1} 步可行", module, diceNum), true, false);
            }
            else
            {
                updateInfo(oGA.displayName, action, module + ": 尚未挑戰精英", true, false);
            }

            return true;
        }

        public static bool checkOutstandingLongMarch(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug)
        {
            RequestReturnObject rro = request.LongMarch.getMyStatus(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!(rro.Exists(RRO.LongMarch.leftTimes) && rro.Exists(RRO.LongMarch.curStation))) return false;
            int leftTimes = JSON.getInt(rro.responseJson, RRO.LongMarch.leftTimes);
            if (leftTimes > 0)
            {
                updateInfo(oGA.displayName, action, module + ": 今日尚未開始", true, false);
                return true;
            }
            int curStation = JSON.getInt(rro.responseJson, RRO.LongMarch.curStation);
            if (curStation < 15)
            {
                updateInfo(oGA.displayName, action, module + ": 尚未到達 匈奴", true, false);
                return true;
            }

            return true;
        }

        public static bool checkOutstandingOneYear(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug)
        {
            RequestReturnObject rro = request.OneYear.cityStatus(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!rro.Exists(RRO.OneYear.activityStatus, typeof(DynamicJsonArray))) return false;
            long currTime = getSystemTime(oGA.connectionInfo, oGA.sid) / 1000;
            bool oneYearActivity = false;
            DynamicJsonArray activityStatus = rro.responseJson[RRO.OneYear.activityStatus];
            foreach (dynamic o in activityStatus)
            {
                if (JSON.exists(o, "startTime") && JSON.exists(o, "endTime"))
                {
                    long startTime = JSON.getLong(o, "startTime");
                    long endTime = JSON.getLong(o, "endTime");
                    if ((startTime < currTime) && (currTime < endTime))
                    {
                        oneYearActivity = true;
                        break;
                    }
                }
            }
            if (!oneYearActivity) return true;

            // 嘉年华入场券
            rro = request.Bag.getBagInfo(oGA.connectionInfo, oGA.sid);
            if (rro.SuccessWithJson(RRO.Bag.items, typeof(DynamicJsonArray)))
            {
                DynamicJsonArray dja = rro.responseJson[RRO.Bag.items];
                int ticket = 0;
                foreach (dynamic o in dja)
                {
                    if (JSON.getString(o, RRO.Bag.nm, "") == RRO.Bag.nm_ticket)
                    {
                        ticket += JSON.getInt(o, RRO.Bag.n, 0);
                    }
                }
                if (ticket > 0)
                {
                    updateInfo(oGA.displayName, action, string.Format("{0}: 尚有 {1} 張嘉年華入場卷", module, ticket), true, false);
                }

            }

            // 點將台
            rro = request.DianJiangTai.beforeStart(oGA.connectionInfo, oGA.sid);
            if (rro.SuccessWithJson(RRO.DianJiangTai.leftTimes))
            {
                int leftTimes = JSON.getInt(rro.responseJson, RRO.DianJiangTai.leftTimes);
                if (leftTimes > 0)
                {
                    updateInfo(oGA.displayName, action, string.Format("{0}: 點將台尚有 {1} 次", module, leftTimes), true, false);
                }
            }

            // 仙鶴雲居
            rro = request.OneYear.info(oGA.connectionInfo, oGA.sid);
            if (rro.SuccessWithJson(RRO.OneYear.remainCount))
            {
                int remainCount = JSON.getInt(rro.responseJson, RRO.OneYear.remainCount);
                if (remainCount > 0)
                {
                    updateInfo(oGA.displayName, action, string.Format("{0}: 仙鶴雲居尚有 {1} 次", module, remainCount), true, false);
                    return true;

                }
            }


            return true;
        }


        public static bool checkOutstandingHeroAfi(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug)
        {
            if (oGA.level < 80)
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, action, string.Format("{0}: 主公只有 {1} 等的, 尚未達到打造神器的最低要求", module, oGA.level));
                return true;
            }

            RequestReturnObject rro = request.Hero.getPlayerHeroList(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!rro.Exists(RRO.Hero.heros, typeof(DynamicJsonArray))) return false;
            DynamicJsonArray dja = rro.responseJson[RRO.Hero.heros];
            bool findMAKE = false;
            string heroMAKE = "";
            foreach (dynamic o in dja)
            {
                dynamic afi = o[RRO.Hero.afi];
                if (JSON.getString(afi, RRO.Hero.sta, "") == RRO.Hero.sta_MAKE)
                {
                    findMAKE = true;
                    heroMAKE = JSON.getString(o, RRO.Hero.dsp, "");
                    break;
                }
            }

            if (findMAKE)
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, action, string.Format("{0}: 正在為 {1} 打造神器", module, heroMAKE));
            }
            else
            {
                updateInfo(oGA.displayName, action, string.Format("{0}: 沒有打造中的神器", module), true, false);
            }
            return true;
        }

        public static bool checkOutstandingNaval(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug)
        {
            int gameDOW = scheduler.Schedule.ScheduleInfo.getGameDOW();
            DateTime now = DateTime.Now;
            if ((gameDOW != 1) && (gameDOW != 2)) return true;
            if ((now.Hour < 9) || (now.Hour > 21)) return true;

            int cityId = (gameDOW == 1 ? 1 : 3);

            RequestReturnObject rro;
            rro = request.Naval.inMissionHeros(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!(rro.Exists(RRO.Naval.alives, typeof(DynamicJsonArray)) &&
                  rro.Exists(RRO.Naval.deads, typeof(DynamicJsonArray)) &&
                  rro.Exists(RRO.Naval.deadhero, typeof(DynamicJsonArray))))
                return false;

            DynamicJsonArray oAlives, oDeads, oDeadHero;
            oAlives = rro.responseJson["alives"];
            oDeads = rro.responseJson["deads"];
            oDeadHero = rro.responseJson["deadhero"];
            // The task should be executed before start, so may only need to check alives
            // If any of them contain data, that means troops already sent, no action required.
            if ((oAlives.Length + oDeads.Length + oDeadHero.Length) > 0) return true;

            updateInfo(oGA.displayName, action, string.Format("{0}: 尚未出兵", module), true, false);
            return true;
        }


    }
}
