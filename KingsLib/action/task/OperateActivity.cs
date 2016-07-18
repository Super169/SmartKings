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
            public static bool goOperateActivity(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.OperateActivity;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.OperateActivity.getUpgradeActivityInfo(ci, sid);
                if (!rro.success) return false;
                if (!rro.exists(RRO.OperateActivity.isGot))
                {
                    LOG.E(oGA.displayName, taskName, string.Format("{0} - Unexpected return: {1}", "OperateActivity.getUpgradeActivityInfo", rro.responseText));
                    return false;
                }
                if (rro.getBool(RRO.OperateActivity.isGot))
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "今天已經領取了登入好禮");
                    return true;
                }

                rro = request.OperateActivity.upgradeActivityReward(ci, sid);
                if (rro.ok != 1) return false;

                updateInfo(oGA.displayName, taskName, "領取登入好禮");
                return true;
            }
        }
    }
}
