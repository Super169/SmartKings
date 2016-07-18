﻿
using KingsLib.data;
using KingsLib.scheduler;
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
// 皇榜
// 五福臨門
// 草船借箭
// 奇門八卦
// 懸賞任務
// 鏢行四海
// 登入好禮

namespace KingsLib
{
    public static partial class action
    {
        public static bool checkAllOutstandingTasks(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug = false)
        {
            string action = "遺漏任務";

            if (debug) showDebugMsg(updateInfo, oGA.displayName, action, "開始");

            // 征戰
            goCheckOutstandTasks(action, "征戰", checkOutstandingCampaign, oGA, updateInfo, debug);

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

            // 皇榜
            goCheckOutstandTasks(action, "皇榜", checkOutstandingTeamDuplicate, oGA, updateInfo, debug);

            // 五福臨門
            goCheckOutstandTasks(action, "五福臨門", checkOutstandingWuFuLinMen, oGA, updateInfo, debug);

            // 草船借箭
            goCheckOutstandTasks(action, "草船借箭", checkOutstandingGrassArrow, oGA, updateInfo, debug);

            // 夷陵之戰
            goCheckOutstandTasks(action, "夷陵之戰", checkOutstandingYiling, oGA, updateInfo, debug);

            // 奇門八卦
            goCheckOutstandTasks(action, "奇門八卦", checkOutstandingEightTrigrams, oGA, updateInfo, debug);

            // 懸賞任務
            goCheckOutstandTasks(action, "懸賞任務", checkOutstandingSevenDaysPoints, oGA, updateInfo, debug);

            // 華容道
            goCheckOutstandTasks(action, "華容道", checkOutstandingHuarongRoad, oGA, updateInfo, debug);

            // 鏢行四海
            goCheckOutstandTasks(action, "鏢行四海", checkOutstandingEscort, oGA, updateInfo, debug);

            // 
            // 登入好禮
            goCheckOutstandTasks(action, "登入好禮", checkOutstandingOperateActivity, oGA, updateInfo, debug);

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

        public static bool checkOutstandingCampaign(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro = request.Campaign.getLeftTimes(oGA.connectionInfo, oGA.sid);
            if (!(rro.SuccessWithJson(RRO.Campaign.elite) &&
                  rro.exists(RRO.Campaign.eliteBuyTimes) &&
                  rro.exists(RRO.Campaign.eliteCanBuyTimes) &&
                  rro.exists(RRO.Campaign.arena) &&
                  rro.exists(RRO.Campaign.lmarch) &&
                  rro.exists(RRO.Campaign.arenas) &&
                  rro.exists(RRO.Campaign.starryLeftCount)
                  )) return false;

            int elite = rro.getInt(RRO.Campaign.elite, -1);
            int eliteBuyTimes = rro.getInt(RRO.Campaign.eliteBuyTimes, -1);
            int eliteCanBuyTimes = rro.getInt(RRO.Campaign.eliteCanBuyTimes, -1);
            string msg;
            if ((elite > 0) || ((eliteCanBuyTimes > 0) && (eliteBuyTimes == 0)))
            {
                msg = "討伐群雄: ";
                if (elite > 0) msg += string.Format("尚餘 {0} 次 ", elite);
                if ((eliteCanBuyTimes > 0) && (eliteBuyTimes == 0)) msg += " 尚未購買額外次數";
                updateInfo(oGA.displayName, actionName, msg, true, false);
            }
            checkRemainCount(oGA, updateInfo, "天下比武", rro.getInt(RRO.Campaign.arena, -1));
            checkRemainCount(oGA, updateInfo, "攬星壇", rro.getInt(RRO.Campaign.starryLeftCount, -1));
            if (oGA.level < 65)
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, string.Format("{0}: 主公只有 {1} 等的, 尚未達到三軍演武的最低要求", module, oGA.level));
            }
            else
            {
                checkRemainCount(oGA, updateInfo, "三軍演武", rro.getInt(RRO.Campaign.arenas, -1));
            }

            return true;
        }

        private static void checkRemainCount(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, int remainCount)
        {
            if (remainCount > 0)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚餘 {1} 次", actionName, remainCount), true, false);
            }
        }

        public static bool checkOutstandingVisitHero(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro = request.Hero.getVisitHeroInfo(oGA.connectionInfo, oGA.sid, "徐晃");
            if (!rro.SuccessWithJson(RRO.Hero.matchTimes)) return false;
            int matchTimes = rro.getInt(RRO.Hero.matchTimes, -1);
            if (matchTimes > 0)
            {
                string msg = string.Format("{0}: 尚有 {1} 次未完成", module, matchTimes);
                updateInfo(oGA.displayName, actionName, msg, true, false);
            }
            return true;
        }

        public static bool checkOutstandingCampainTrials(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {

            RequestReturnObject rro = request.Campaign.getTrialsInfo(oGA.connectionInfo, oGA.sid);
            if (!(rro.SuccessWithJson(RRO.Campaign.weekday) && rro.exists(RRO.Campaign.times) && rro.exists(RRO.Campaign.buyTimes))) return false;
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
                updateInfo(oGA.displayName, actionName, msg, true, false);
            }
            return true;
        }

        public static bool checkOutstandingAfterKingRoad(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            int gameDOW = Scheduler.getGameDOW();
            if (!((gameDOW == 3) || (gameDOW == 6)))
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, "今天沒有王者獎勵/保級賽");
                return true;
            }
            if (DateTime.Now > Scheduler.getRefTime(DateTime.Now, new TimeSpan(21, 0, 0)))
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, "王者獎勵/保級賽 已經結束");
                return true;
            }
            RequestReturnObject rro = request.KingRoad.afterSeasonEnemy(oGA.connectionInfo, oGA.sid);
            if (!(rro.SuccessWithJson(RRO.KingRoad.remainChallenge) &&
                  rro.exists(RRO.KingRoad.seasonType) &&
                  rro.exists(RRO.KingRoad.enemy, typeof(DynamicJsonArray))))
            {
                return false;
            }

            int remainChallenge = rro.getInt(RRO.KingRoad.remainChallenge);
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
                    string seasonType = rro.getString(RRO.KingRoad.seasonType, "UNKNOWN");
                    switch (seasonType)
                    {
                        case RRO.KingRoad.seasonType_TO_KEEP:
                            seasonType = "保級";
                            break;
                        case RRO.KingRoad.seasonType_GIFT:
                            seasonType = "獎勵";
                            break;

                    }
                    updateInfo(oGA.displayName, actionName, string.Format("王者{0}賽: 餘下 {1} 次挑戰, 尚有 {2} 個未達三星", seasonType, remainChallenge, failCnt), true, false);
                }
            }
            return true;
        }

        public static bool checkOutstandingTravel(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro = request.Travel.getMapInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (rro.prompt == PROMPT.ACTIVITY_CAN_NOT_PARTICIPATE) return true;
            if (!(rro.exists(RRO.Travel.diceNum))) return false;
            int diceNum = rro.getInt(RRO.Travel.diceNum);
            if (diceNum > 0)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 還有 {1} 步可行", module, diceNum), true, false);
            }
            else
            {
                updateInfo(oGA.displayName, actionName, module + ": 尚未挑戰精英", true, false);
            }

            return true;
        }

        public static bool checkOutstandingLongMarch(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro = request.LongMarch.getMyStatus(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!(rro.exists(RRO.LongMarch.leftTimes) && rro.exists(RRO.LongMarch.curStation))) return false;
            int leftTimes = rro.getInt(RRO.LongMarch.leftTimes);
            if (leftTimes > 0)
            {
                updateInfo(oGA.displayName, actionName, module + ": 今日尚未開始", true, false);
                return true;
            }
            int curStation = rro.getInt(RRO.LongMarch.curStation);
            if (curStation < 15)
            {
                updateInfo(oGA.displayName, actionName, module + ": 尚未到達 匈奴", true, false);
                return true;
            }

            return true;
        }

        public static bool checkOutstandingOneYear(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            ConnectionInfo ci = oGA.connectionInfo;
            string sid = oGA.sid;

            int eventCount = action.OneYear.eventCount(ci, sid);
            if (eventCount < 0) return false;
            if (eventCount == 0) return true;

            int ticket = action.OneYear.checkTicket(ci, sid);
            if (ticket > 0)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚有 {1} 張嘉年華入場卷", module, ticket), true, false);
            }

            int leftTimes = action.OneYear.checkDianJiangTai(ci, sid);
            if (leftTimes > 0)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 點將台尚有 {1} 次", module, leftTimes), true, false);
            }

            int remainCount = action.OneYear.checkRemain(ci, sid);
            if (remainCount > 0)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 仙鶴雲居尚有 {1} 次", module, remainCount), true, false);
            }

            return true;


            /*

            RequestReturnObject rro = request.OneYear.cityStatus(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!rro.exists(RRO.OneYear.activityStatus, typeof(DynamicJsonArray))) return false;
            long currTime = system.getTime(oGA.connectionInfo, oGA.sid) / 1000;
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
                    updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚有 {1} 張嘉年華入場卷", module, ticket), true, false);
                }

            }

            // 點將台
            rro = request.DianJiangTai.beforeStart(oGA.connectionInfo, oGA.sid);
            if (rro.SuccessWithJson(RRO.DianJiangTai.leftTimes))
            {
                int leftTimes = rro.getInt(RRO.DianJiangTai.leftTimes);
                if (leftTimes > 0)
                {
                    updateInfo(oGA.displayName, actionName, string.Format("{0}: 點將台尚有 {1} 次", module, leftTimes), true, false);
                }
            }

            // 仙鶴雲居
            rro = request.OneYear.info(oGA.connectionInfo, oGA.sid);
            if (rro.SuccessWithJson(RRO.OneYear.remainCount))
            {
                int remainCount = rro.getInt(RRO.OneYear.remainCount);
                if (remainCount > 0)
                {
                    updateInfo(oGA.displayName, actionName, string.Format("{0}: 仙鶴雲居尚有 {1} 次", module, remainCount), true, false);
                    return true;

                }
            }


            return true;
            */
        }


        public static bool checkOutstandingHeroAfi(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            if (oGA.level < 80)
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, string.Format("{0}: 主公只有 {1} 等的, 尚未達到打造神器的最低要求", module, oGA.level));
                return true;
            }

            RequestReturnObject rro = request.Hero.getPlayerHeroList(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!rro.exists(RRO.Hero.heros, typeof(DynamicJsonArray))) return false;
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
                if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, string.Format("{0}: 正在為 {1} 打造神器", module, heroMAKE));
            }
            else
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 沒有打造中的神器", module), true, false);
            }
            return true;
        }

        private static void showDebugNoActivity(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module)
        {
            showDebugMsg(updateInfo, oGA.displayName, actionName, string.Format("今天沒有 {0} 活動", module));
        }

        public static bool checkOutstandingNaval(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            int gameDOW = Scheduler.getGameDOW();
            DateTime now = DateTime.Now;
            if ((gameDOW != 1) && (gameDOW != 2))
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, string.Format("今天沒有{0}", module));
                return true;
            }

            if ((now.Hour < 9) || (now.Hour >= 21)) return true;

            int cityId = (gameDOW == 1 ? 1 : 3);

            RequestReturnObject rro;
            rro = request.Naval.inMissionHeros(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!(rro.exists(RRO.Naval.alives, typeof(DynamicJsonArray)) &&
                  rro.exists(RRO.Naval.deads, typeof(DynamicJsonArray)) &&
                  rro.exists(RRO.Naval.deadhero, typeof(DynamicJsonArray))))
                return false;

            DynamicJsonArray oAlives, oDeads, oDeadHero;
            oAlives = rro.responseJson["alives"];
            oDeads = rro.responseJson["deads"];
            oDeadHero = rro.responseJson["deadhero"];
            // The task should be executed before start, so may only need to check alives
            // If any of them contain data, that means troops already sent, no action required.
            if ((oAlives.Length + oDeads.Length + oDeadHero.Length) > 0) return true;

            updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚未出兵", module), true, false);
            return true;
        }

        public static bool checkOutstandingTeamDuplicate(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro;
            rro = request.TeamDuplicate.teamDuplicateFreeTimes(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!rro.exists(RRO.TeamDuplicate.times)) return false;

            int times = rro.getInt(RRO.TeamDuplicate.times);
            if (times < 3)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 只做了 {1} 次, 尚未完成", module, times), true, false);
            }
            return true;
        }

        public static bool checkOutstandingWuFuLinMen(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro;
            rro = request.WuFuLinMen.getGameInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (rro.prompt == PROMPT.ACTIVITY_IS_NOT_OPEN) return true;
            if (!rro.exists(RRO.WuFuLinMen.drawDatas)) return false;

            dynamic drawDatas = rro.responseJson[RRO.WuFuLinMen.drawDatas];
            int stage = JSON.getInt(drawDatas, RRO.WuFuLinMen.stage);
            if (stage < 1) return false;

            switch (stage)
            {
                case 1:
                    updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚未開始", module), true, false);
                    break;
                case 2:
                    updateInfo(oGA.displayName, actionName, string.Format("{0}: 只選了道具, 尚未抽取獎勵數量", module), true, false);
                    break;
                case 3:
                    if (!JSON.exists(drawDatas, RRO.WuFuLinMen.drawStatus, typeof(DynamicJsonArray))) return false;
                    DynamicJsonArray drawStatus = drawDatas[RRO.WuFuLinMen.drawStatus];
                    bool getEverydayLogin = false;
                    foreach (dynamic draw in drawStatus)
                    {
                        if (JSON.getString(draw, RRO.WuFuLinMen.type, null) == RRO.WuFuLinMen.type_EVERYDAY_LOGIN)
                        {
                            getEverydayLogin = true;
                            break;
                        }
                    }
                    if (!getEverydayLogin)
                    {
                        updateInfo(oGA.displayName, actionName, string.Format("{0}: 己抽取獎勵, 尚未決定領取每日登陛獎勵", module), true, false);
                    }
                    break;

                default:
                    return false;
            }
            return true;
        }


        public static bool checkOutstandingGrassArrow(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            int gameDOW = Scheduler.getGameDOW();
            if (!(gameDOW == 1))
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, string.Format("今天沒有{0}", module));
                return true;
            }

            RequestReturnObject rro;
            rro = request.GrassArrow.acquireGrassArrowInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!(rro.exists(RRO.GrassArrow.arrowNum) &&
                  rro.exists(RRO.GrassArrow.fightCount) &&
                  rro.exists(RRO.GrassArrow.totalNum) &&
                  rro.exists(RRO.GrassArrow.rewards, typeof(DynamicJsonArray))
                  )) return false;

            int fightCount = rro.getInt(RRO.GrassArrow.fightCount);
            if (fightCount > 0)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚未有 {1} 次未完成", module, fightCount), true, false);
            }
            else
            {
                int arrowNum = rro.getInt(RRO.GrassArrow.arrowNum);
                if (arrowNum >= 160)
                {
                    updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚有 {1} 支箭可換領獎品", module, arrowNum), true, false);
                }
                int totalNum = rro.getInt(RRO.GrassArrow.totalNum);
                DynamicJsonArray rewards = rro.responseJson[RRO.GrassArrow.rewards];
                int notYetGot = 0;
                foreach (dynamic reward in rewards)
                {
                    int num = JSON.getInt(reward, RRO.GrassArrow.num, 999999);
                    bool got = JSON.getBool(reward, RRO.GrassArrow.got, true);
                    if ((totalNum >= num) && (!got)) notYetGot++;
                }
                if (notYetGot > 0)
                {
                    updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚有 {1} 個獎勵未領取", module, notYetGot), true, false);
                }
            }

            return true;
        }

        public static bool checkOutstandingYiling(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro;
            rro = request.Yiling.getStatus(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (rro.prompt == PROMPT.NOT_IN_ACTIVITY_TIME) return true;
            if (rro.responseText == "") return true;  // Not met the minimum requirement
            if (!(rro.exists(RRO.Yiling.canBattleTimes) &&
                  rro.exists(RRO.Yiling.boughtTimes) &&
                  rro.exists(RRO.Yiling.isInMap)
                  )) return false;

            int canBattleTimes = rro.getInt(RRO.Yiling.canBattleTimes);
            bool isInMap = rro.getBool(RRO.Yiling.isInMap);
            int boughtTimes = rro.getInt(RRO.Yiling.boughtTimes);
            if (isInMap) updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚未完成", module), true, false);
            else if (canBattleTimes > 0) updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚有 {1} 次未開始", module, canBattleTimes), true, false);
            else if (boughtTimes == 0) updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚有未購買額外次數", module), true, false);
            return true;
        }

        public static bool checkOutstandingEightTrigrams(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            int gameDOW = Scheduler.getGameDOW();
            if (!(gameDOW == 5))
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, string.Format("今天沒有{0}", module));
                return true;
            }

            RequestReturnObject rro;
            rro = request.EightTrigrams.open(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!(rro.exists(RRO.EightTrigrams.isIn) &&
                  rro.exists(RRO.EightTrigrams.leftTimes)
                  )) return false;

            bool isIn = rro.getBool(RRO.EightTrigrams.isIn);
            int leftTimes = rro.getInt(RRO.EightTrigrams.leftTimes);
            if (isIn) updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚未完成", module), true, false);
            else if (leftTimes > 0) updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚可購買 {1} 次額外次數", module, leftTimes), true, false);
            return true;
        }

        public static bool checkOutstandingSevenDaysPoints(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro;
            rro = request.SevenDaysPoints.getOldInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (rro.style == STYLE.ERROR) return true;  // Already end
            if (!rro.SuccessWithJson(RRO.SevenDaysPoints.needRefresh))
            {
                LOG.E(string.Format("{0} : {1} : Unexpected result: {2}", oGA.displayName, "SevenDaysPoints.getOldInfo", rro.responseText));
                return false;
            }
            if (rro.getBool(RRO.SevenDaysPoints.needRefresh))
            {
                rro = request.SevenDaysPoints.getActInfo(oGA.connectionInfo, oGA.sid);
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 獲取任務", module), true, false);
            }
            return true;
        }

        public static bool checkOutstandingHuarongRoad(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            int gameDOW = Scheduler.getGameDOW();
            if (!(gameDOW == 4))
            {
                if (debug) showDebugMsg(updateInfo, oGA.displayName, actionName, string.Format("今天沒有{0}", module));
                return true;
            }

            RequestReturnObject rro;

            rro = request.Circle.getHuarongRoadInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!(rro.exists(RRO.Circle.left) && rro.exists(RRO.Circle.reset) && rro.exists(RRO.Circle.refresh) && rro.exists(RRO.Circle.rewarded)))
            {
                LOG.E(string.Format("{0} : {1} : Unexpected result: {2}", oGA.displayName, "Circle.getHuarongRoadInfo", rro.responseText));
                return false;
            }
            // Step is saved in step of step
            dynamic stepInfo = rro.responseJson[RRO.Circle.step];
            int step = JSON.getInt(stepInfo, RRO.Circle.step);
            if (step < 7)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚未完成, 當前位置: {1}", module, step), true, false);
                return true;
            }
            if (!rro.getBool(RRO.Circle.rewarded))
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚未提取獎勵", module), true, false);
                return true;
            }
            if (rro.getInt(RRO.Circle.reset) < 1)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚未花費 50金 再次挑戰", module), true, false);
                return true;
            }


            rro = request.SevenDaysPoints.getOldInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (rro.style == STYLE.ERROR) return true;  // Already end
            if (!rro.SuccessWithJson(RRO.SevenDaysPoints.needRefresh))
            {
                LOG.E(string.Format("{0} : {1} : Unexpected result: {2}", oGA.displayName, "SevenDaysPoints.getOldInfo", rro.responseText));
                return false;
            }
            if (rro.getBool(RRO.SevenDaysPoints.needRefresh))
            {
                rro = request.SevenDaysPoints.getActInfo(oGA.connectionInfo, oGA.sid);
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 獲取任務", module), true, false);
            }
            return true;
        }



        public static bool checkOutstandingEscort(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro;

            rro = request.Escort.worldInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!rro.exists(RRO.Escort.remainLootTime)) return false;
            int remainLootTime = rro.getInt(RRO.Escort.remainLootTime);
            if (remainLootTime > 0)
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚有 {1} 次鏢可以去劫", module, remainLootTime), true, false);
            }
            return true;
        }

        public static bool checkOutstandingOperateActivity(GameAccount oGA, DelegateUpdateInfo updateInfo, string actionName, string module, bool debug)
        {
            RequestReturnObject rro;

            rro = request.OperateActivity.getUpgradeActivityInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (!rro.exists(RRO.OperateActivity.isGot)) return false;
            if (!rro.getBool(RRO.OperateActivity.isGot))
            {
                updateInfo(oGA.displayName, actionName, string.Format("{0}: 尚未領取", module), true, false);
            }
            return true;
        }

    }
}
