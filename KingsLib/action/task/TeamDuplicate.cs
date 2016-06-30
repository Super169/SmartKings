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
            public static bool goTeamDuplicate(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.TeamDuplicate;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                dynamic parmObject = oGA.getTaskParmObject(taskId);
                if (!JSON.exists(parmObject, Scheduler.Parm.TeamDuplicate.heroIdx, typeof(DynamicJsonArray))) {
                    updateInfo(oGA.displayName, taskName, "設定尚未完成");
                    return false;
                }
                DynamicJsonArray dja = parmObject[Scheduler.Parm.TeamDuplicate.heroIdx];
                string heroIdx = "";
                foreach (dynamic o in dja)
                {
                    int idx = JSON.getInt(o, -1);
                    if (idx > 0) heroIdx += (heroIdx == "" ? "" : ", ") + idx.ToString();
                }
                heroIdx = "[" + heroIdx + "]";

                campaign.quitCampaign(ci, sid, 0);

                rro = request.TeamDuplicate.teamDuplicateFreeTimes(ci, sid);
                if (!rro.SuccessWithJson(RRO.TeamDuplicate.times)) return false;
                int times = rro.getInt(RRO.TeamDuplicate.times);
                if (times == 3) return true;

                rro = request.TeamDuplicate.duplicateList(ci, sid);
                if (!rro.SuccessWithJson(RRO.TeamDuplicate.items, typeof(DynamicJsonArray))) return false;
                DynamicJsonArray items = rro.responseJson[RRO.TeamDuplicate.items];
                foreach(dynamic item in items)
                {
                    bool completed = JSON.getBool(item, RRO.TeamDuplicate.completed, true);
                    int duplicateId = JSON.getInt(item, RRO.TeamDuplicate.duplicateId);

                    if (!completed && (duplicateId > 0)) 
                    {
                        bool battleStarted = duplicateBattle(ci, sid, duplicateId, heroIdx);
                        campaign.quitCampaign(ci, sid, 0);
                        if (battleStarted)
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("進行皇榜 {0}", duplicateId));
                            times++;
                            if (times == 3) break;
                        }
                    }
                }
                // TODO: Need to read the count again to check for success, and also refresh if needed.
                return true;
            }


            private static bool duplicateBattle(ConnectionInfo ci, string sid, int duplicateId, string heroIdx)
            {
                RequestReturnObject rro;
                rro = request.TeamDuplicate.createTeamDuplicate(ci, sid, duplicateId);
                if (!rro.SuccessWithJson(RRO.TeamDuplicate.teamId)) return false;
                int teamId = rro.getInt(RRO.TeamDuplicate.teamId);

                rro = request.TeamDuplicate.heroInBattle(ci, sid, heroIdx, teamId);
                if (rro.ok != 1) return false;

                rro = request.TeamDuplicate.battleStart(ci, sid);
                if (rro.ok != 1) return false;

                rro = request.TeamDuplicate.exitTeam(ci, sid, teamId);
                // Already strated, no need to check for fail anymore

                return true;
            }

        }
    }
}
