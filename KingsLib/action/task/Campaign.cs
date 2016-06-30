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
            public static bool goEliteFight(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.EliteFight;
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.EliteFight);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Campaign.getLeftTimes(ci, sid);
                if (!rro.SuccessWithJson(RRO.Campaign.elite)) return false;
                int leftCount = rro.getInt(RRO.Campaign.elite);
                if (leftCount <= 0) return true;

                // S35 : 10-1
                // S37 :  9-2
                dynamic parmObject = oGA.getTaskParmObject(taskId);
                int targetChapter = JSON.getInt(parmObject, Scheduler.Parm.EliteFight.targetChapter);
                int targetStage = JSON.getInt(parmObject, Scheduler.Parm.EliteFight.targetStage);

                if (!((targetChapter > 0) && (targetStage > 0)))
                {
                    updateInfo(oGA.displayName, taskName, "尚未設定目標");
                    return false;
                }

                string[] targetReward = { "金旗鼓号", "金旗鼓號", "金鞋", "金枪", "金马", "金槍", "金馬" };

                updateInfo(oGA.displayName, taskName, string.Format("餘下 {0} 次, 是次出戰: {1} - {2}",
                                                                      leftCount,
                                                                      util.getEliteChapterName(targetChapter),
                                                                      util.getEliteHeroName(targetChapter, targetStage)));

                WarInfo wi = oGA.getWarInfo(Scheduler.TaskId.EliteFight, 0);
                string fightHeros = null;
                if (wi != null) fightHeros = wi.body;
                int fightResult = action.campaign.eliteFight(ci, sid, RRO.Campaign.difficult_normal, targetStage, targetChapter, ref fightHeros);
                if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("作戰陣形 {0} ", fightHeros));

                if (fightResult == -1)
                {
                    updateInfo(oGA.displayName, taskName, "出戰失敗");
                    return false;
                }
                else if (fightResult == 1)
                {
                    updateInfo(oGA.displayName, taskName, "尚未完成佈陣");
                    return false;
                }

                // Before get reward; just do as what the frontend does, no handling on return
                rro = request.Campaign.getLeftTimes(ci, sid);
                rro = request.Campaign.eliteGetCampaignInfo(ci, sid, RRO.Campaign.difficult_normal, targetChapter);
                rro = request.Campaign.getLeftTimes(ci, sid);
                rro = request.Hero.getFeastInfo(ci, sid);

                // Go get reward
                int targetCnt = 0;
                string itemList = "";
                rro = request.TurnCardReward.getTurnCardRewards(ci, sid);
                if (!rro.SuccessWithJson(RRO.TurnCardReward.rewards, typeof(DynamicJsonArray))) return true;

                targetCnt = checkReward(rro, targetReward, ref itemList);
                if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("並有 {0} 個目標物: {1}", targetCnt, itemList));

                rro = request.TurnCardReward.turnCard(ci, sid, RRO.TurnCardReward.turnCardMode_ONE);
                targetCnt -= checkReward(rro, targetReward, ref itemList);
                if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("第一次翻牌後, 取得 {1}, 餘下 {0} 個目標物", targetCnt, itemList));

                rro = request.TurnCardReward.turnCard(ci, sid, RRO.TurnCardReward.turnCardMode_ONE);
                targetCnt -= checkReward(rro, targetReward, ref itemList);
                if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("第二次翻牌後, 取得 {1}, 餘下 {0} 個目標物", targetCnt, itemList));

                // Pay 5 gold only if more than 2 target remained
                if (targetCnt >= 2)
                {
                    rro = request.TurnCardReward.turnCard(ci, sid, RRO.TurnCardReward.turnCardMode_ONE);
                    if (debug)
                    {
                        targetCnt -= checkReward(rro, targetReward, ref itemList);
                        showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("付費翻牌一次後, 取得 {1}, 餘下 {0} 個目標物", targetCnt, itemList));
                    }
                }

                return true;
            }

            private static int checkReward(RequestReturnObject rro, string[] target, ref string itemList)
            {
                itemList = "";
                if (!rro.SuccessWithJson(RRO.TurnCardReward.rewards, typeof(DynamicJsonArray))) return 0;
                int rewardCnt = 0;
                DynamicJsonArray rewards;
                rewards = rro.responseJson[RRO.TurnCardReward.rewards];
                foreach (dynamic reward in rewards)
                {
                    if (JSON.exists(reward, RRO.TurnCardReward.itemDefs, typeof(DynamicJsonArray)))
                    {
                        DynamicJsonArray itemdefs = reward[RRO.TurnCardReward.itemDefs];
                        // Just make it simple, check the first item only
                        string name = JSON.getString(itemdefs.ElementAt(0), RRO.TurnCardReward.name, "-");
                        int num = JSON.getInt(itemdefs.ElementAt(0), RRO.TurnCardReward.num, 0);
                        if (target.Contains(name)) rewardCnt++;
                        if (itemList != "") itemList += ", ";
                        itemList += string.Format("{0} x {1}", name, num);
                    }
                    else if (JSON.exists(reward, RRO.TurnCardReward.resources))
                    {
                        DynamicJsonObject resources = reward[RRO.TurnCardReward.resources];
                        string resource = resources.GetDynamicMemberNames().ElementAt(0);
                        int num = JSON.getInt(resources, resource);
                        if (num > 0)
                        {
                            if (itemList != "") itemList += ", ";
                            itemList += string.Format("{0} x {1}", resource, num);
                        }
                    }
                }
                if (itemList.Trim() == "")
                {
                    LOG.E("Missing items: " + rro.responseText);
                }
                return rewardCnt;
            }

            public static bool goEliteBuyTime(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.EliteBuyTime;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Campaign.getLeftTimes(ci, sid);
                if (!rro.SuccessWithJson(RRO.Campaign.eliteBuyTimes)) return false;
                int eliteBuyTimes = rro.getInt(RRO.Campaign.eliteBuyTimes);
                int eliteCanBuyTimes = rro.getInt(RRO.Campaign.eliteCanBuyTimes);
                if ((eliteBuyTimes >= 1) || (eliteBuyTimes >= eliteCanBuyTimes)) return true;

                rro = request.Campaign.eliteBuyTime(ci, sid);
                if (!rro.SuccessWithJson(RRO.Campaign.eliteBuyTimes)) return false;
                int eliteBuyTimesAfter = rro.getInt(RRO.Campaign.eliteBuyTimes);
                if (eliteBuyTimesAfter <= eliteBuyTimes) return false;
                updateInfo(oGA.displayName, taskName, "購買一次額外次數");
                return true;
            }

            public static bool goTrialBuyTime(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.TrialsBuyTimes;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Campaign.getTrialsInfo(ci, sid);
                if (!(rro.SuccessWithJson(RRO.Campaign.weekday) && rro.exists(RRO.Campaign.buyTimes, typeof(DynamicJsonObject)))) return false;
                int weekday = rro.getInt(RRO.Campaign.weekday);
                dynamic buyTimes = rro.responseJson["buyTimes"];

                string[] trialType = { "", "WZLJ", "WJDD", "WHSJ" };
                int buyCnt = 0;
                for (int idx = 1; idx <= 3; idx++)
                {
                    // Interesting, weekday of Sunday is 7 instead of 0, for sefety, check both 0 & 7.
                    if ((weekday == 0) || (weekday == 7) || (weekday == idx) || (weekday == idx + 3))
                    {
                        int buyTime = JSON.getInt(buyTimes, trialType[idx]);
                        if (buyTime == 0)
                        {
                            rro = request.Campaign.trialsBuyTimes(ci, sid, trialType[idx]);
                            if (rro.ok == 1) buyCnt++;
                        }
                    }
                }
                if (buyCnt > 0) updateInfo(oGA.displayName, taskName, string.Format("購買 {0} 次額外次數", buyCnt));
                return true;
            }



        }
    }
}
