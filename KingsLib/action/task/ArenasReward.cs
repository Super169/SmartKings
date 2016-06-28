using KingsLib.data;
using KingsLib.scheduler;
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
            public static bool goAreansReward(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.ArenasReward);
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
