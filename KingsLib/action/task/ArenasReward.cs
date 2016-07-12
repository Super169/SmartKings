using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class action
    {
        public static partial class task
        {
            public static bool goArenaDefFormation(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.ArenaDefFormation;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                DateTime defStart = scheduler.Scheduler.getRefTime(new TimeSpan(18, 0, 0));
                DateTime defEnd = scheduler.Scheduler.getRefTime(new TimeSpan(21,0,0));
                int warIdx = (((DateTime.Now > defStart) && (DateTime.Now < defEnd)) ? 0 : 1);
                WarInfo wi = oGA.getWarInfo(taskId, warIdx);
                if ((wi == null) || (wi.body == null) || (wi.body == ""))
                {
                    updateInfo(oGA.displayName, taskName, string.Format("尚未設定第 {0} 部隊", warIdx));
                    return false;
                }
                rro = request.Arena.saveDefFormation(ci, sid, wi.body);
                if (!rro.SuccessWithJson(RRO.Arena.food)) {
                    LOG.E(oGA.displayName, taskName, string.Format("Arena.saveDefFormation: {0} - {1}", rro.success, rro.responseText));
                    return false;
                }

                updateInfo(oGA.displayName, taskName, string.Format("設定防守為第 {0} 部隊", warIdx));
                return true;
            }

            public static bool goArenasReward(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.ArenasReward;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Arenas.getDefFormation(ci, sid);

                rro = request.Arenas.myArenasStatus(ci, sid);
                if (!rro.SuccessWithJson(RRO.Arenas.rwdRank)) return false;
                int rwdRank = rro.getInt(RRO.Arenas.rwdRank);
                if (rwdRank == 0) return true;

                rro = request.Arenas.drawTimeReward(ci, sid);
                if (!rro.SuccessWithJson(RRO.Arenas.rwdRank))
                {
                    updateInfo(oGA.displayName, taskName, string.Format("領取第 {0} 名的獎勵 失敗", rwdRank));
                    return false;
                }

                updateInfo(oGA.displayName, taskName, string.Format("領取第 {0} 名的獎勵", rwdRank));

                return true;
            }
        }
    }
}
