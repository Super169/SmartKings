
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
        public delegate void DelegateUpdateInfo(string account, string action, string msg, bool addTime = true, bool async = true);

        public delegate bool DelegateCheckOutstandingTask(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, bool debug);

        private static void showDebugMsg(DelegateUpdateInfo updateInfo, string account, string action, string msg)
        {
            updateInfo(account, action, "**** " + msg, true, false);
        }

        public static bool checkAllOutstandingTasks(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug = false)
        {
            string action = "遺漏任務";

            if (debug) showDebugMsg(updateInfo, oGA.displayName, action, "開始");

            // Check 討伐群雄
            goCheckOutstandTasks(action, "討伐群雄", checkOutstandingCampaignElite, oGA, updateInfo, debug);

            // Check 英雄切磋
            goCheckOutstandTasks(action, "英雄切磋", checkOutstandingVisitHero, oGA, updateInfo, debug);

            // 英雄試練
            goCheckOutstandTasks(action, "英雄試練", checkOutstandingCampainTrials, oGA, updateInfo, debug);

            // 王者獎勵/保級賽
            goCheckOutstandTasks(action, "王者獎勵", checkOutstandingAfterKingRoad, oGA, updateInfo, debug);

            // 周遊天下
            goCheckOutstandTasks(action, "周遊天下", checkOutstandingTravel, oGA, updateInfo, debug);

            if (debug) showDebugMsg(updateInfo, oGA.displayName, action, "結束");

            return true;
        }

        private static void goCheckOutstandTasks(string action, string module, DelegateCheckOutstandingTask checkTask, GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
        {
            if (debug) showDebugMsg(updateInfo, oGA.displayName, action, string.Format("{0}: {1}", "開始檢查", module));
            if (!checkTask(oGA, updateInfo, action, debug))
            {
                updateInfo(oGA.displayName, action, module + ": 檢查失敗", true, false);
            }
            if (debug) showDebugMsg(updateInfo, oGA.displayName, action, string.Format("{0}: {1}", "結束檢查", module));
        }

        public static bool checkOutstandingCampaignElite(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, bool debug)
        {
            RequestReturnObject rro = request.Campaign.getLeftTimes(oGA.connectionInfo, oGA.sid);
            if (!(rro.SuccessWithJson(RRO.elite) && rro.Exists(RRO.eliteBuyTimes) && rro.Exists(RRO.eliteCanBuyTimes))) return false;

            int elite = JSON.getInt(rro.responseJson, RRO.elite, -1);
            int eliteBuyTimes = JSON.getInt(rro.responseJson, RRO.eliteBuyTimes, -1);
            int eliteCanBuyTimes = JSON.getInt(rro.responseJson, RRO.eliteCanBuyTimes, -1);
            if ((elite > 0) || ((eliteCanBuyTimes > 0) && (eliteBuyTimes == 0)))
            {
                string msg = "討伐群雄: ";
                if (elite > 0) msg += string.Format("尚餘 {0} 次 ", elite);
                if ((eliteCanBuyTimes > 0) && (eliteBuyTimes == 0)) msg += "尚未購買額外次數";
                updateInfo(oGA.displayName, action, msg, true, false);
            }
            return true;
        }

        public static bool checkOutstandingVisitHero(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, bool debug)
        {
            RequestReturnObject rro = request.Hero.getVisitHeroInfo(oGA.connectionInfo, oGA.sid, "徐晃");
            if (!rro.SuccessWithJson(RRO.matchTimes)) return false;
            int matchTimes = JSON.getInt(rro.responseJson, RRO.matchTimes, -1);
            if (matchTimes > 0)
            {
                string msg = string.Format("英雄切磋: 尚有 {0} 次未完成", matchTimes);
                updateInfo(oGA.displayName, action, msg, true, false);
            }
            return true;
        }

        public static bool checkOutstandingCampainTrials(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, bool debug)
        {

            RequestReturnObject rro = request.Campaign.getTrialsInfo(oGA.connectionInfo, oGA.sid);
            if (!(rro.SuccessWithJson(RRO.weekday) && rro.Exists(RRO.times) && rro.Exists(RRO.buyTimes))) return false;
            int weekday = JSON.getInt(rro.responseJson[RRO.weekday]);
            dynamic times = rro.responseJson[RRO.times];
            dynamic buyTimes = rro.responseJson[RRO.buyTimes];
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
                string msg = "英雄試練: ";
                if (remain) msg += "尚餘 " + msgRemain + "未完成; ";
                if (notBuy) msg += "尚未購買 " + msgNotBuy;
                updateInfo(oGA.displayName, action, msg, true, false);
            }
            return true;
        }

        public static bool checkOutstandingAfterKingRoad(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, bool debug)
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
            if (!(rro.SuccessWithJson(RRO.remainChallenge) &&
                  rro.Exists(RRO.seasonType) &&
                  rro.Exists(RRO.enemy, typeof(DynamicJsonArray))))
            {
                return false;
            }

            int remainChallenge = JSON.getInt(rro.responseJson, RRO.remainChallenge);
            if (remainChallenge > 0)
            {
                int failCnt = 0;
                DynamicJsonArray dja = rro.responseJson[RRO.enemy];
                foreach (dynamic o in dja)
                {
                    int star = JSON.getInt(o, RRO.star, 0);
                    if (star < 3) failCnt++;
                }
                if (failCnt > 0)
                {
                    string seasonType = JSON.getString(rro.responseJson, RRO.seasonType, "UNKNOWN");
                    switch (seasonType)
                    {
                        case RRO.TO_KEEP:
                            seasonType = "保級";
                            break;
                        case RRO.GIFT:
                            seasonType = "獎勵";
                            break;

                    }
                    updateInfo(oGA.displayName, action, string.Format("王者{0}賽: 餘下 {1} 次挑戰, 尚有 {2} 個未達三星", seasonType, remainChallenge, failCnt), true, false);
                }
            }
            return true;
        }

        public static bool checkOutstandingTravel(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, bool debug)
        {
            RequestReturnObject rro = request.Travel.getMapInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return false;
            if (rro.prompt == PROMPT.ACTIVITY_CAN_NOT_PARTICIPATE) return true;
            if (!(rro.Exists(RRO.diceNum))) return false;
            int diceNum = JSON.getInt(rro.responseJson, RRO.diceNum);
            if (diceNum > 0)
            {
                updateInfo(oGA.displayName, action, string.Format("周遊天下: 還有{0}可行", diceNum), true, false);
            } else
            {
                updateInfo(oGA.displayName, action, "周遊天下: 尚未挑戰精英", true, false);
            }

            return true;
        }


    }
}
