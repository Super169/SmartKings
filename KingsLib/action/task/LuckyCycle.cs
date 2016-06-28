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
            public static bool goLuckyCycle(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.LuckyCycle);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.LuckyCycle.info(ci, sid);
                if (!rro.SuccessWithJson(RRO.LuckyCycle.remainCount)) return false;
                int remainCount = rro.getInt(RRO.LuckyCycle.remainCount);
                while (remainCount > 0)
                {
                    rro = request.LuckyCycle.draw(ci, sid);
                    if (!rro.success) return false;
                    if (!(rro.exists(RRO.LuckyCycle.id) && rro.exists(RRO.LuckyCycle.progress) && rro.exists(RRO.LuckyCycle.needRefresh)))
                    {
                        if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "不能確定結果");
                        LOG.E(string.Format("{0} - {1} : unexpected return {2}", oGA.displayName, taskName, rro.responseText));
                        return false;
                    }
                    updateInfo(oGA.displayName, taskName, "轉動一次輪盤");
                    remainCount--;
                }
                return true;
            }
        }
    }
}