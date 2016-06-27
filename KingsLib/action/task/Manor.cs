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

                dynamic json = JSON.Empty;
                List<int> hs = new List<int>();
                hs.Add(10);
                hs.Add(12);
                json[Scheduler.Parm.TrainHero.targetHeros] = hs;
                json = JSON.recode(json);

                List<int> targetHeros = new List<int>();
                DynamicJsonArray dja = json[Scheduler.Parm.TrainHero.targetHeros];
                foreach (dynamic o in dja)
                {
                    int heroIndex = JSON.getInt(o, -1);
                    if (heroIndex > 0) targetHeros.Add(heroIndex);
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

                foreach (int field in jcFields)
                {

                }

                return true;
            }

        }
    }
}
