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
        public static partial class task
        {
            public static bool goHuarongRoad(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                // return true;

                string taskId = Scheduler.TaskId.HuarongRoad;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                WarInfo wi = oGA.getWarInfo(taskId, 0);
                if (wi == null)
                {
                    updateInfo(oGA.displayName, taskName, "尚未完成設定");
                    return false;
                }

                if (!WarInfo.validBody(wi.body))
                {
                    updateInfo(oGA.displayName, taskName, "設定有問題");
                    LOG.E(oGA.displayName, taskName, string.Format("設定有問題: {0}", wi.body));
                    return false;
                }

                dynamic parmObject = oGA.getTaskParmObject(taskId);
                int skipPos = 1;
                if (parmObject != null)
                {
                    skipPos = JSON.getInt(parmObject, Scheduler.Parm.HuarongRoad.skipPos, 1);
                    if ((skipPos < 1) || (skipPos > 6)) skipPos = 1;
                }


                bool goFight = true;
                while (goFight)
                {

                    rro = request.Circle.getHuarongRoadInfo(oGA.connectionInfo, oGA.sid);
                    if (!rro.success) return false;
                    if (!(rro.exists(RRO.Circle.left) && rro.exists(RRO.Circle.reset) && rro.exists(RRO.Circle.refresh) && rro.exists(RRO.Circle.rewarded)))
                    {
                        LOG.E(string.Format("{0} : {1} : Unexpected result: {2}", oGA.displayName, "Circle.getHuarongRoadInfo", rro.responseText));
                        return false;
                    }

                    dynamic stepInfo = rro.responseJson[RRO.Circle.step];
                    int step = JSON.getInt(stepInfo, RRO.Circle.step);
                    string status = JSON.getString(stepInfo, RRO.Circle.status,"");
                    bool rewarded = rro.getBool(RRO.Circle.rewarded, true);
                    int reset = rro.getInt(RRO.Circle.reset);

                    if ((step < 0) || (step > 7)) return false;

                    if ((step == 7) && (status == RRO.Circle.status_REWARED) && !rewarded)
                    {
                        rro = request.Circle.drawPassRewards(ci, sid);
                        continue;
                    }

                    if ((step == 7) && rewarded && (reset > 0))
                    {
                        return true;
                    }

                    if ((step == 7) && rro.getBool(RRO.Circle.rewarded) && (rro.getInt(RRO.Circle.reset) < 1))
                    {
                        updateInfo(oGA.displayName, taskName, "購買額外次數.");
                        rro = request.Circle.restartHuarongRoad(ci, sid);
                        if (!rro.SuccessWithJson(RRO.Circle.step)) return false;
                        stepInfo = rro.responseJson[RRO.Circle.step];
                        step = JSON.getInt(stepInfo, RRO.Circle.step);
                    }

                    campaign.quitCampaign(ci, sid, 0);
                    FightResult fr = goOneHuarongRoadFight(oGA, updateInfo, debug, wi.body, skipPos);
                    if ((fr == FightResult.error) || (fr == FightResult.failed))
                    {
                        updateInfo(oGA.displayName, taskName, string.Format("挑戰第 {0} 關失敗.", step));
                        return false;
                    }
                    updateInfo(oGA.displayName, taskName, string.Format("挑戰第 {0} 關成功.", step));
                    if (fr == FightResult.alreadyEnd) return true;

                }

                campaign.quitCampaign(ci, sid, 0);
                return true;

            }

            private enum FightResult
            {
                success, failed, alreadyEnd, error
            }

            private static FightResult goOneHuarongRoadFight(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug, string body, int skipPos)
            {
                string taskId = Scheduler.TaskId.HuarongRoad;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;


                rro = request.Circle.getHuarongRoadInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.Circle.step)) return FightResult.error;
                dynamic stepInfo = rro.responseJson[RRO.Circle.step];
                int step = JSON.getInt(stepInfo, RRO.Circle.step);

                if ((step == 7) && rro.getBool(RRO.Circle.rewarded) && (rro.getInt(RRO.Circle.reset) > 0))
                {
                    return FightResult.alreadyEnd;
                }


                for (int idx = 1; idx <= 6; idx++)
                {
                    if (idx != skipPos)
                    {
                        rro = request.Circle.turnOverHeroCard(ci, sid, idx);
                        if (!rro.success) LOG.E(oGA.displayName, taskName, rro.responseText);
                    }

                    // Ignore turn card error
                }

                rro = request.Circle.challenge(ci, sid);
                if (!rro.success) return FightResult.error;
                if (rro.style == STYLE.ERROR) return FightResult.error;
                if (!((rro.exists(RRO.Circle.specInfo) &&
                       rro.exists(RRO.Circle.specInfo, RRO.Circle.heroStg))))
                {
                    LOG.E(oGA.displayName, taskName, string.Format("Circle.challenge 出錯: {0}", rro.responseText));
                    return FightResult.error;
                }
                dynamic heroStg = rro.responseJson[RRO.Circle.specInfo][RRO.Circle.heroStg];
                if (!JSON.exists(heroStg, RRO.Circle.targets, typeof(DynamicJsonArray)))
                {
                    LOG.E(oGA.displayName, taskName, string.Format("Circle.challenge 出錯: {0}", rro.responseText));
                    return FightResult.error;
                }

                DynamicJsonArray targets = heroStg[RRO.Circle.targets];
                List<int> skipList = new List<int>();
                foreach (dynamic o in targets)
                {
                    string heroName = (string)o;
                    HeroInfo hi = oGA.heros.Find(x => x.nm == heroName);
                    if (hi != null) skipList.Add(hi.idx);
                }

                List<int> goHero = new List<int>();
                dynamic fightHeros = JSON.decode(body);
                int chief = JSON.getInt(fightHeros, "chief", -1);
                DynamicJsonArray dja = fightHeros["heros"];
                foreach (dynamic o in dja)
                {
                    goHero.Add(JSON.getInt(o, "index", -1));
                }
                foreach (dynamic o in dja)
                {
                    int heroIdx = JSON.getInt(o, "index", -1);
                    if (skipList.Exists(x => x == heroIdx))
                    {
                        foreach (HeroInfo hi in oGA.heros)
                        {
                            if (!skipList.Exists(x => x == hi.idx) && !goHero.Exists(x => x == hi.idx))
                            {
                                if (heroIdx == chief)
                                {
                                    fightHeros["chief"] = hi.idx;
                                }
                                o["index"] = hi.idx;
                                goHero.Add(heroIdx);
                            }
                        }
                    }
                }
                string fightBody = JSON.encode(fightHeros);


                rro = request.Campaign.getAttFormation(ci, sid, RRO.Campaign.march_HUARONG_ROAD);
                if (!rro.SuccessWithJson(RRO.Campaign.heros, typeof(DynamicJsonArray)))
                {
                    campaign.quitCampaign(ci, sid, -1);
                    return FightResult.failed;
                }
                Thread.Sleep(500);

                rro = request.Campaign.nextEnemies(ci, sid);
                if (!rro.SuccessWithJson(RRO.Campaign.enemies, typeof(DynamicJsonArray)))
                {
                    campaign.quitCampaign(ci, sid, -1);
                    return FightResult.failed;
                }
                Thread.Sleep(500);

                rro = request.Campaign.saveFormation(ci, sid, fightBody);
                if (!rro.SuccessWithJson(RRO.Campaign.power))
                {
                    campaign.quitCampaign(ci, sid, -1);
                    return FightResult.failed;
                }
                Thread.Sleep(500);


                rro = request.Campaign.fightNext(ci, sid);
                if (rro.ok != 1)
                {
                    campaign.quitCampaign(ci, sid, -1);
                    return FightResult.failed;
                }
                Thread.Sleep(500);

                rro = request.Circle.getHuarongRoadInfo(ci, sid);
                campaign.quitCampaign(ci, sid, 0);

                return FightResult.success;
            }

        }
    }
}
