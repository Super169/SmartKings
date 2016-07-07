using KingsLib.data;
using KingsLib.scheduler;
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
            public static bool goTravel(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.Travel;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                WarInfo wi = oGA.getWarInfo(taskId, 0);
                string fightHeros = null;
                if (wi != null) fightHeros = wi.body;

                if ((fightHeros == null) || (fightHeros == ""))
                {
                    updateInfo(oGA.displayName, taskName, "尚未完成佈陣");
                        return false;
                }

                DateTime now = DateTime.Now;
                int mode = 0;
                int hour = now.Hour;
                if (hour < 5) mode = 0;
                else if (hour < 9) mode = 1;
                else if (hour < 12) mode = 2;
                else if (hour < 18) mode = 3;
                else if (hour < 19) mode = 4;
                else mode = 0;

                rro = request.Travel.getStatus(ci, sid);
                if (!(rro.SuccessWithJson(RRO.Travel.isIn) && rro.exists(RRO.Travel.canPlayTimes))) {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "Travel.getStatus 出錯");
                    return false;
                }
                if (!rro.getBool(RRO.Travel.isIn) && (rro.getInt(RRO.Travel.canPlayTimes, -1) == 0))
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "今天的周遊已完結");
                    return false;
                }


                TravelMapInfo mapInfo = action.Travel.getTravelMapInfo(ci, sid);
                if (!mapInfo.ready)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "Travel.getTravelMapInfo 出錯");
                    return false;
                }
                if (mapInfo.simpleMap.Length == 0)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "mapInfo.simpleMap 空白");
                    return false;
                }


                int[] boxInfo = new int[mapInfo.mapSize + 1];

                // Read BAOXIANG
                for (int i = 1; i <= mapInfo.mapSize; i++)
                {
                    if (mapInfo.simpleMap[i] == RRO.Travel.MAP_BAOXIANG)
                    {
                        rro = request.Travel.viewStep(ci, sid, i);
                        if (rro.SuccessWithJson(RRO.Travel.step, RRO.Travel.stepType) && 
                            (JSON.getString(rro.responseJson[RRO.Travel.step], RRO.Travel.stepType, "") == RRO.Travel.MAP_BAOXIANG) && 
                            rro.exists(RRO.Travel.step, RRO.Travel.reward))
                        {
                            dynamic reward = rro.responseJson[RRO.Travel.step][RRO.Travel.reward];
                            if (JSON.exists(reward, RRO.Travel.itemDefs, typeof(DynamicJsonArray)))
                            {
                                DynamicJsonArray itemDefs = reward[RRO.Travel.itemDefs];
                                foreach (dynamic o in itemDefs)
                                {
                                    string name = JSON.getString(o, RRO.Travel.name, "");
                                    if (name.EndsWith("紅色寶物包") || name.EndsWith("红色宝物包"))
                                    {
                                        boxInfo[i] = 2;
                                    }
                                    else if (name.EndsWith("金色寶物包") || name.EndsWith("金色宝物包"))
                                    {
                                        boxInfo[i] = 1;
                                    }
                                    else if (name.EndsWith("色寶物包") || name.EndsWith("色宝物包"))
                                    {
                                        boxInfo[i] = 0;
                                    }

                                }
                            }

                        }
                    }
                }

                showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("執行模式: {0}", mode));

                int nextStep = 0;
                int errCount = 0;
                if (mode == 0)
                {
                    // Play mode
                    while (mapInfo.diceNum > 0)
                    {
                        nextStep = action.Travel.goOneStep(0, oGA, ref mapInfo, ref boxInfo, fightHeros, false, updateInfo);

                        if (nextStep == -1)
                        {
                            errCount++;
                            if (errCount > 0)
                            {
                                updateInfo(oGA.displayName, taskName, "出現多次錯誤離開");
                                LOG.E(string.Format("{0} : {1} : 出現多次錯誤離開", oGA.displayName, taskName));
                                break;
                            }
                        }
                        mapInfo = action.Travel.getTravelMapInfo(ci, sid);
                    }
                }
                else
                {
                    // Go shopping only
                    if (mapInfo.simpleMap[mapInfo.currStep] == RRO.Travel.MAP_SHANGDIAN)
                    {
                        action.Travel.goHandleMove(oGA, ref mapInfo, ref boxInfo, mapInfo.currStep, updateInfo);
                    }
                    else
                    {
                        int tryCnt = 0;
                        while ((mapInfo.simpleMap[mapInfo.currStep] != RRO.Travel.MAP_SHANGDIAN) && (mapInfo.diceNum > 0))
                        {
                            // need to move to shop

                            nextStep = action.Travel.goOneStep(mode, oGA, ref mapInfo, ref boxInfo, fightHeros, false, updateInfo);
                            if (nextStep == -1)
                            {
                                errCount++;
                                if (errCount > 0)
                                {
                                    updateInfo(oGA.displayName, taskName, "出現多次錯誤, 暫時放棄");
                                    LOG.E(string.Format("{0} : {1} : 出現多次錯誤, 暫時放棄", oGA.displayName, taskName));
                                    break;
                                }
                            }
                            else
                            {
                                if (mapInfo.simpleMap[nextStep] != RRO.Travel.MAP_SHANGDIAN)
                                {
                                    tryCnt++;
                                    if ((mapInfo.diceNum < 25) && (tryCnt > 5))
                                    {
                                        updateInfo(oGA.displayName, taskName, string.Format("未能到達商店, 暫時放棄, 餘下 {0} 次", mapInfo.diceNum));
                                        break;
                                    }
                                }
                            }
                            mapInfo = action.Travel.getTravelMapInfo(ci, sid);
                        }
                    }
                }

                return true;

            }
        }
    }
}

