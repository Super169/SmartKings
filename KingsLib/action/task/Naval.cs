using KingsLib.data;
using KingsLib.scheduler;
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
            public static bool goNavalWar(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.NavalWar;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                int dow = scheduler.Scheduler.getGameDOW();
                if ((dow != 1) && (dow != 2)) return true;

                DateTime startTime = Scheduler.getRefTime(new TimeSpan(9, 5, 0));
                if (DateTime.Now < startTime) return false;

                DateTime endTime = Scheduler.getRefTime(new TimeSpan(20, 15, 0));
                if (DateTime.Now > endTime) return true;

                int cityId = (dow == 1 ? 1 : 3);


                string[] fightHeros = new string[3];

                for (int idx = 0; idx < 3; idx++)
                {
                    WarInfo wi = oGA.getWarInfo(taskId, idx);
                    if (wi == null) fightHeros[idx] = null;
                    else fightHeros[idx] = wi.body;
                }

                if (fightHeros[0] == null)
                {
                    updateInfo(oGA.displayName, taskName, "尚未完成佈陣");
                    return false;
                }

                rro = request.Naval.inMissionHeros(oGA.connectionInfo, oGA.sid);
                if (!(rro.success &&
                      rro.exists(RRO.Naval.alives, typeof(DynamicJsonArray)) &&
                      rro.exists(RRO.Naval.deads, typeof(DynamicJsonArray)) &&
                      rro.exists(RRO.Naval.deadhero, typeof(DynamicJsonArray))))
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "Naval.inMissionHeros 出錯.");
                    return false;
                }

                DynamicJsonArray oAlives, oDeads, oDeadHero;
                oAlives = rro.responseJson["alives"];
                oDeads = rro.responseJson["deads"];
                oDeadHero = rro.responseJson["deadhero"];

                // The task should be executed before start, so may only need to check alives
                // If any of them contain data, that means troops already sent, no action required.
                if ((oAlives.Length + oDeads.Length + oDeadHero.Length) > 0)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "早前已派遣部隊參與");
                    return true;
                }

                rro = request.Naval.enterWar(ci, sid, cityId);
                if (!rro.SuccessWithJson(RRO.Naval.disp))
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "Naval.enterWar 出錯.");
                    return false;
                }

                rro = request.Naval.inMissionHeros(ci, sid);
                if (!rro.success)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "第二次執行 Naval.inMissionHeros 出錯.");
                    return false;
                }

                int maxIdx = (cityId == 1 ? 2 : 3);
                for (int idx = 0; idx < maxIdx; idx++)
                {
                    if (fightHeros[idx] != null)
                    {
                        rro = request.Naval.sendTroops(ci, sid, fightHeros[idx], cityId + idx);
                        if (rro.SuccessWithJson(RRO.Naval.troops))
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("向戰場 {0} 出兵: {1}", cityId + idx, fightHeros[idx]));
                        } else
                        {
                            if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("向戰場 {0} 出兵: {1} 時出了問題", cityId + idx, fightHeros[idx]));
                            return false;
                        }
                    }
                }
                return true;
            }

        }
    }
}
