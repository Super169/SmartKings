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

            public static bool goTuanGou(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.TuanGo);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                RequestReturnObject rro = request.Activity.getTuanGouInfo(ci, sid);
                if (!rro.SuccessWithJson(RRO.Activity.bagInfos, typeof(DynamicJsonArray))) return false;
                if (!rro.exists(RRO.Activity.rewardBags, typeof(DynamicJsonArray))) return false;

                DynamicJsonArray bagInfos = rro.responseJson[RRO.Activity.bagInfos];
                DynamicJsonArray rewardBags = rro.responseJson[RRO.Activity.rewardBags];

                int sumSize = 0;
                foreach (dynamic o in bagInfos)
                {
                    sumSize += JSON.getInt(o, RRO.Activity.sumSize, 0);
                }

                foreach (dynamic o in rewardBags)
                {
                    int needSize = JSON.getInt(o, RRO.Activity.needSize, 0);
                    bool isReward = JSON.getBool(o, RRO.Activity.isReward, true);
                    if ((needSize <= sumSize) && !isReward)
                    {
                        int bagId = JSON.getInt(o, RRO.Activity.bagId);
                        rro = request.Activity.tuanGouReward(ci, sid, bagId);
                        if (rro.ok == 1)
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("開啟寶箱 {0} ", bagId), true, false);
                        }
                    }
                }
                return true;
            }

        }
    }
}
