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


                List<int> targetHeros = new List<int>();

                dynamic parmObject = oGA.getTaskParmObject(taskId);
                if (JSON.exists(parmObject, Scheduler.Parm.TrainHero.targetHeros, typeof(DynamicJsonArray)))
                {
                    bool trainSameLevel = JSON.getBool(parmObject, Scheduler.Parm.TrainHero.trainSameLevel);
                    DynamicJsonArray dja = parmObject[Scheduler.Parm.TrainHero.targetHeros];
                    foreach (dynamic o in dja)
                    {
                        int heroIdx = JSON.getInt(o, -1);
                        if (heroIdx > 0)
                        {
                            HeroInfo hi = oGA.heros.Find(x => x.idx == heroIdx);
                            if (trainSameLevel || (hi.lv < oGA.level))
                            {
                                targetHeros.Add(heroIdx);
                            }
                        }
                    }
                }

                if (targetHeros.Count == 0)
                {
                    updateInfo(oGA.displayName, taskName, "設定尚未完成");
                    return false;
                }


                List<int> jcFields = new List<int>();

                List<ManorInfo> mis = action.Manor.getManorInfo(ci, sid);
                foreach (ManorInfo mi in mis)
                {
                    if (mi.type != RRO.Manor.type_JC) continue;
                    // Unknown error
                    if (mi.heroIndex == 0) continue;
                    bool checkCount = false;
                    if (mi.heroIndex == -1)
                    {
                        checkCount = true;
                    }
                    else if (mi.leftSeconds == 0)
                    {
                        // Finish
                        rro = request.Manor.harvestActivity(ci, sid, mi.field);
                        checkCount = rro.SuccessWithJson(RRO.Manor.times);
                    }
                    else
                    {
                        if (targetHeros.Contains(mi.heroIndex))  targetHeros.Remove(mi.heroIndex);
                    }

                    if (checkCount)
                    {
                        rro = request.Manor.trainHeroInfo(ci, sid, mi.field);
                        if (rro.SuccessWithJson(RRO.Manor.totalTimes) && rro.exists(RRO.Manor.usedTimes))
                        {
                            if (rro.getInt(RRO.Manor.totalTimes) > rro.getInt(RRO.Manor.usedTimes)) jcFields.Add(mi.field);
                        }
                    }
                }

                if (targetHeros.Count == 0) return false;
                int heroPos = 0;
                int fieldPos = 0;
                while (fieldPos < jcFields.Count)
                {
                    int field = jcFields.ElementAt(fieldPos);
                    rro = request.Manor.doHeroActivity(ci, sid, targetHeros.ElementAt(heroPos), field);
                    if (rro.ok == 1)
                    {
                        string nm = oGA.getHeroName(targetHeros.ElementAt(heroPos));
                        updateInfo(oGA.displayName, taskName, string.Format("對 {0} 進行訓練", nm));
                        fieldPos++;
                    }
                    if (++heroPos >= targetHeros.Count) break;
                }
                
                return true;
            }

        }
    }
}
