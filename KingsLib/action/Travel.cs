using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib
{
    public static partial class action
    {
        public static class Travel
        {
            public static TravelMapInfo getTravelMapInfo(ConnectionInfo ci, string sid)
            {
                RequestReturnObject rro = request.Travel.getMapInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.Travel.simpleMap)) return new TravelMapInfo();
                return new TravelMapInfo(rro.responseJson);
            }

            public static bool goDice(int mode, GameAccount oGA, ref TravelMapInfo mapInfo, ref int[] boxInfo, ref int actStep, ref int nextStep, DelegateUpdateInfo updateInfo)
            {
                string taskId = Scheduler.TaskId.Travel;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                int goStep = -1;
                int checkPos = mapInfo.currStep;
                for (int i = 1; i <= 6; i++)
                {
                    checkPos++;
                    if (checkPos > mapInfo.mapSize) checkPos -= mapInfo.mapSize;
                    if (mode == 0)
                    {
                        if ((boxInfo[checkPos] == 3) ||
                            ((mapInfo.diceNum <= 25) && (boxInfo[checkPos] == 2)) ||
                            ((mapInfo.diceNum <= 20) && (boxInfo[checkPos] == 1)))
                        {
                            goStep = i;
                            break;
                        }

                    }
                    else
                    {
                        if (mapInfo.simpleMap[checkPos] == RRO.Travel.MAP_SHANGDIAN)
                        {
                            goStep = i;
                            break;
                        }
                    }
                }

                bool controlStepOK = false;
                if ((goStep >= 1) && (goStep <= 6))
                {
                    rro = request.Travel.controlDice(ci, sid, goStep);
                    if (rro.ok == 1)
                    {
                        nextStep = mapInfo.currStep + goStep;
                        if (nextStep > mapInfo.mapSize) nextStep -= mapInfo.mapSize;
                        updateInfo(oGA.displayName, taskName, string.Format("餘下{0} 次, 指定擲出 {1}, 將會前進到 {2}", mapInfo.diceNum, goStep, nextStep));
                        actStep = nextStep;
                    }
                    else
                    {
                        updateInfo(oGA.displayName, taskName, string.Format("餘下{0} 次, 操控骰子失敗", mapInfo.diceNum));
                    }
                }
                if (!controlStepOK)
                {
                    rro = request.Travel.dice(ci, sid);
                    if (!rro.SuccessWithJson()) return false;
                    mapInfo.diceNum--;
                    int num1 = rro.getInt(RRO.Travel.num1);
                    int num2 = rro.getInt(RRO.Travel.num2);
                    actStep = mapInfo.currStep + num1 + num2;
                    if (actStep > mapInfo.mapSize) actStep -= mapInfo.mapSize;
                    nextStep = JSON.getInt(rro.responseJson, RRO.Travel.nextStep);
                    updateInfo(oGA.displayName, taskName, string.Format("餘下{0} 次, 擲出 {1} {2}, 將會前進到 {3} - {4}", mapInfo.diceNum, num1, num2, nextStep, actStep));
                    if (nextStep < 0)
                    {
                        // Invalid dice (i.e. not allow die at this moment, e.g. dice before battle completed
                        return false;
                    }
                    if (nextStep > mapInfo.mapSize) return false;
                }
                return true;

            }

            private enum AttackResult
            {
                error, failEscape, ready
            }

            public static int goOneStep(int mode, GameAccount oGA, ref TravelMapInfo mapInfo, ref int[] boxInfo, string heros, bool escapeBattle, DelegateUpdateInfo updateInfo)
            {
                string taskId = Scheduler.TaskId.Travel;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                int nextStep = mapInfo.currStep;
                int actStep = nextStep + mapInfo.remainSteps;
                if (actStep > mapInfo.mapSize) actStep -= mapInfo.mapSize;

                string stepType;
                stepType = mapInfo.simpleMap[nextStep];

                AttackResult attackResult = goPassAttack(oGA, ref mapInfo, ref boxInfo, heros, updateInfo);

                if (attackResult == AttackResult.error) return -1;

                // Try even failEscape

                if (!goDice(mode, oGA, ref mapInfo, ref boxInfo, ref actStep, ref nextStep, updateInfo)) return -1;
                stepType = mapInfo.simpleMap[nextStep];

                if (stepType == "ZHANDOU")
                {
                    mapInfo.currStep = nextStep;
                    attackResult = goPassAttack(oGA, ref mapInfo, ref boxInfo, heros, updateInfo);
                    if (attackResult == AttackResult.error) return -1;
                }
                else
                {
                    goHandleMove(oGA, ref mapInfo, ref boxInfo, nextStep, updateInfo);
                }
                return nextStep;
            }
            private static AttackResult goPassAttack(GameAccount oGA, ref TravelMapInfo mapInfo, ref int[] boxInfo, string heros, DelegateUpdateInfo updateInfo)
            {
                string taskId = Scheduler.TaskId.Travel;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                mapInfo = getTravelMapInfo(ci, sid);
                if (!mapInfo.ready) return AttackResult.error;

                int nextStep = mapInfo.currStep;
                if (mapInfo.simpleMap[nextStep] != RRO.Travel.MAP_ZHANDOU) return AttackResult.ready;

                updateInfo(oGA.displayName, taskName, string.Format("在 {0} 進行戰鬥", nextStep));

                bool goBattle = true;
                int failCount = 0;
                while (goBattle)
                {
                    Thread.Sleep(1000);
                    rro = request.Travel.viewStep(ci, sid, nextStep);
                    Thread.Sleep(1000);
                    rro = request.Travel.attack(ci, sid, nextStep);
                    rro = request.Campaign.getAttFormation(ci, sid, "TRAVEL");
                    Thread.Sleep(1000);
                    rro = request.Campaign.nextEnemies(ci, sid);
                    Thread.Sleep(1000);
                    rro = request.Campaign.saveFormation(ci, sid, heros);
                    Thread.Sleep(1000);
                    rro = request.Campaign.fightNext(ci, sid);
                    Thread.Sleep(1000);
                    // rro = Hero.getScoreHero(ci, sid, heroBody);
                    rro = request.Campaign.quitCampaign(ci, sid);
                    rro = request.Travel.getMapInfo(ci, sid);
                    rro = request.Travel.arriveStep(ci, sid, nextStep);
                    if ((!rro.success) || (rro.prompt == PROMPT.ERR_COMMON_NOT_SUPPORT))
                    {
                        // Battle failed
                        failCount++;
                        if (failCount >= 3)
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("逃避 {0} 的戰鬥", nextStep));
                            rro = request.Travel.escape(ci, sid);
                            // unexpected fail, nothing can do if it cannot escape
                            if (rro.ok != 1) return AttackResult.failEscape;
                            goBattle = false;
                        }
                    }
                    else
                    {
                        updateInfo(oGA.displayName, taskName, string.Format("在 {0} 的戰鬥中獲勝", nextStep));
                        mapInfo.simpleMap[nextStep] = RRO.Travel.MAP_KONG;
                        goBattle = false;
                    }
                }
                // Battle win or escaped
                if (mapInfo.remainSteps > 0)
                {
                    nextStep = nextStep + mapInfo.remainSteps;
                    if (nextStep > mapInfo.mapSize) nextStep -= mapInfo.mapSize;
                    // TODO: go handle next step
                    goHandleMove(oGA, ref mapInfo, ref boxInfo, nextStep, updateInfo);
                    mapInfo.currStep = nextStep;
                    mapInfo.remainSteps = 0;
                }
                return AttackResult.ready;
            }


            public static bool goHandleMove(GameAccount oGA, ref TravelMapInfo mapInfo, ref int[] boxInfo, int nextStep, DelegateUpdateInfo updateInfo)
            {
                string taskId = Scheduler.TaskId.Travel;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                string stepType = mapInfo.simpleMap[nextStep];

                updateInfo(oGA.displayName, taskName, string.Format("到達 {0} - {1}", nextStep, stepType));
                switch (stepType)
                {
                    case RRO.Travel.MAP_KONG:
                        break;
                    case RRO.Travel.MAP_BAOXIANG:
                        Thread.Sleep(1000);
                        rro = request.Travel.arriveStep(ci, sid, nextStep);
                        if (!rro.SuccessWithJson("step", "stepType")) return false;
                        if (rro.responseJson["step"]["stepType"] == "BAOXIANG")
                        {
                            mapInfo.simpleMap[nextStep] = "KONG";
                            boxInfo[nextStep] = 0;
                        }
                        break;
                    case RRO.Travel.MAP_FANPAI:
                        Thread.Sleep(1000);
                        rro = request.Travel.arriveStep(ci, sid, nextStep);
                        Thread.Sleep(1000);
                        rro = request.TurnCardReward.getTurnCardRewards(ci, sid);
                        Thread.Sleep(1000);
                        rro = request.TurnCardReward.getTurnCardRewards(ci, sid);
                        break;
                    case RRO.Travel.MAP_SHANGDIAN:
                        Thread.Sleep(1000);
                        int buyCnt = 0;
                        rro = request.Shop.getTravelShopInfo(ci, sid);
                        if (rro.SuccessWithJson("items", typeof(DynamicJsonArray)))
                        {
                            DynamicJsonArray items = rro.responseJson["items"];
                            for (int idx = 0; idx < 3; idx++)
                            {
                                dynamic o = items.ElementAt(idx);
                                if (!JSON.getBool(o, "sold"))
                                {
                                    int config = JSON.getInt(o, "config");
                                    if (isBuyConfig(config))
                                    {
                                        buyCnt++;
                                        rro = request.Shop.buyTravelShopItem(ci, sid, idx);
                                    }
                                }
                            }
                        }
                        if (buyCnt > 0) updateInfo(oGA.displayName, taskName, string.Format("在 {0} 買了 {1} 件物品", nextStep, buyCnt));
                        break;
                }
                mapInfo.currStep = nextStep;
                return true;
            }

            public static bool isBuyConfig(int config)
            {
                // Known poor item: 
                // 1 - 15:  經驗書; 16-30: (烈酒, 拜帖, 影子虎符, 單挑戰令, 廢棄); 31-46: 糧草; 47-67: 精鐵; 68-70 虎符
                // 70 - 140: gold item of above 70
                // ???? unknown area
                // 291 - 310 - 白, 綠, 藍, 紫, 金 items
                if (config < 30) return false;
                if (config < 68) return true;
                if (config <= 140) return false;
                if (config < 291) return false;
                if (config > 310) return false;  // seems gold item are in this range, need further testing
                return true;
            }


        }
    }
}
