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
            /*
                    Empty:
                    		"field": 20,
		                    "type": "JC",
		                    "level": 13,
		                    "levelSeconds": 0,
		                    "heroIndex": -1,
		                    "leftSeconds": 0,
		                    "products": 0,
		                    "produceSeconds": 0

                    Training:
		                    "field": 20,
		                    "type": "JC",
		                    "level": 13,
		                    "levelSeconds": 0,
		                    "heroIndex": 10,
		                    "leftSeconds": 463,
		                    "products": 0,
		                    "produceSeconds": 0

                    Finished:
                    		"field": 20,
		                    "type": "JC",
		                    "level": 13,
		                    "levelSeconds": 0,
		                    "heroIndex": 10,
		                    "leftSeconds": 0,
		                    "products": 0,
		                    "produceSeconds": 0
            */


            private class TrainHero
            {
                public int idx { get; set; }
                public int level { get; set; }
                public int power { get; set; }
                public int exp { get; set; }
            }

            private class TrainField
            {
                public int field { get; set; }
                public int level { get; set; }
                public bool used { get; set; }
            }

            public static bool goTrainHero(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.TrainHero;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                /*
                dynamic json = oGA.getTaskParmObject(taskId);
                if (json == null)
                {
                    updateInfo(oGA.displayName, taskName, "設定尚未完成");
                }
                */

                List<int> busyHeros = new List<int>();
                List<TrainField> trainFields = new List<TrainField>();

                // Finish all trining first
                List<ManorInfo> mis = action.Manor.getManorInfo(ci, sid);
                foreach (ManorInfo mi in mis)
                {

                    // Check hero in 工匹坊
                    if (mi.type == RRO.Manor.type_GJF)
                    {
                        if (mi.heroIndex > 0)
                        {
                            if (mi.leftSeconds == 0)
                            {
                                rro = request.Manor.harvestActivity(ci, sid, mi.field);
                                if (!rro.SuccessWithJson(RRO.Manor.times)) busyHeros.Add(mi.heroIndex);
                            }
                            else
                            {
                                busyHeros.Add(mi.heroIndex);
                            }
                        }
                        continue;
                    }

                    if (mi.type != RRO.Manor.type_JC) continue;

                    bool emptyField = false;
                    if (mi.heroIndex == -1)
                    {
                        emptyField = true;
                    }
                    else if (mi.leftSeconds == 0)
                    {
                        // Finish
                        rro = request.Manor.harvestActivity(ci, sid, mi.field);
                        emptyField = rro.SuccessWithJson(RRO.Manor.times);
                    }
                    else
                    {
                        busyHeros.Add(mi.heroIndex);
                    }
                    if (emptyField)
                    {
                        rro = request.Manor.trainHeroInfo(ci, sid, mi.field);
                        if (rro.SuccessWithJson(RRO.Manor.totalTimes) && rro.exists(RRO.Manor.usedTimes))
                        {
                            if (rro.getInt(RRO.Manor.totalTimes) > rro.getInt(RRO.Manor.usedTimes))
                            {
                                trainFields.Add(new TrainField() { field = mi.field, level = mi.level, used = false });
                            }
                        }
                    }
                    else
                    {
                        busyHeros.Add(mi.heroIndex);
                    }
                }

                if (trainFields.Count == 0)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "沒有可供訓練的教場.");
                    return true;
                }

                oGA.refreshHeros();

                List<TrainHero> trainHeros = new List<TrainHero>();
                int heroCnt = 0;

                dynamic parmObject = oGA.getTaskParmObject(taskId);
                if (JSON.exists(parmObject, Scheduler.Parm.TrainHero.targetHeros, typeof(DynamicJsonArray)))
                {
                    bool trainSameLevel = JSON.getBool(parmObject, Scheduler.Parm.TrainHero.trainSameLevel, true);
                    DynamicJsonArray dja = parmObject[Scheduler.Parm.TrainHero.targetHeros];
                    foreach (dynamic o in dja)
                    {
                        int heroIdx = JSON.getInt(o, -1);
                        if (heroIdx > 0)
                        {
                            heroCnt++;
                            HeroInfo hi = oGA.heros.Find(x => x.idx == heroIdx);
                            if ((trainSameLevel || (hi.lv < oGA.level)) &&
                                (!busyHeros.Exists(x => x == heroIdx)) &&
                                (util.getLevelUpExp(hi.lv) > hi.exp)
                               )
                            {
                                trainHeros.Add(new TrainHero() { idx = hi.idx, level = hi.lv, exp = hi.exp, power = hi.power });
                            }
                        }
                    }
                }

                if (trainHeros.Count == 0)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "沒有可供訓練的英雄.");
                    return true;
                }

                // Sort heros by power, train the stronger first
                trainHeros.Sort(delegate (TrainHero a, TrainHero b)
                                {
                                    return b.power.CompareTo(a.power);
                                });

                foreach (TrainHero th in trainHeros)
                {
                    if (!trainFields.Exists(x => !x.used)) break;
                    int levelUpExp = util.getLevelUpExp(th.level);
                    int remainExp = levelUpExp - th.exp;

                    foreach (TrainField tf in trainFields)
                    {
                        if (tf.used) continue;

                        int trainExp = util.getTrainExp(tf.level);
                        if (remainExp > (trainExp * 3))
                        {
                            rro = request.Manor.doHeroActivity(ci, sid, th.idx, tf.field);
                            if (rro.ok == 1)
                            {
                                string nm = oGA.getHeroName(th.idx);
                                updateInfo(oGA.displayName, taskName, string.Format("對 {0} 進行訓練: {1}/{2} : {3}", nm, th.exp, levelUpExp, trainExp));
                                tf.used = true;
                            }
                            // go to other hero no matter success or not
                            break;
                        }
                    }
                }

                return true;
            }

        }
    }
}
