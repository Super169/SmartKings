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
            public static bool goFinishAllTask(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.FinishTask);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;
                int finCnt = 0;

                rro = request.Task.getTaskTraceInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.Task.tasks, typeof(DynamicJsonArray))) return false;
                DynamicJsonArray tasks = rro.responseJson[RRO.Task.tasks];

                foreach (dynamic t in tasks)
                {
                    if (JSON.getString(t, RRO.Task.status,"") == RRO.Task.status_FIN)
                    {
                        int taskId = JSON.getInt(t, RRO.Task.id);
                        if (taskId > 0)
                        {
                            rro = request.Task.finishTask(ci, sid, taskId);
                            if (rro.SuccessWithJson(RRO.Task.taskId) && (rro.getInt(RRO.Task.taskId) == taskId)) finCnt++;
                        }
                    }
                }
                if (finCnt > 0) updateInfo(oGA.displayName, taskName, string.Format("領取 {0} 項任務報酬", finCnt));


                return true;
            }


        }
    }
}
