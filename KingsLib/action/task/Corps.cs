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
            public static bool goCorpsCityReward(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.CorpsCityReward);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                bool goNext = true;
                while (goNext)
                {
                    rro = request.Corps.getNationalRank(ci, sid, 1);
                    if (!rro.SuccessWithJson(RRO.Corps.corpsCityReward)) return false;
                    int step = rro.getInt(RRO.Corps.corpsCityReward);
                    if (step < 0) break;

                    rro = request.Corps.getCityRewardInfo(ci, sid, step);
                    int cityNum = rro.getInt(RRO.Corps.cityNum);
                    int corpsFund = rro.getInt(RRO.Corps.corpsFund);
                    if (!rro.exists(RRO.Corps.personalReward)) break;

                    rro = request.Corps.corpsCityReward(ci, sid);
                    cityNum = rro.getInt(RRO.Corps.cityNum);
                    goNext = (rro.exists(RRO.Corps.personalReward));
                    if (goNext)
                    {
                        updateInfo(oGA.displayName, taskName, string.Format("{0}:00 - 獲取 {1} 城池產出", 10 + step, cityNum));
                    }
                }
                return true;
            }
        }
    }
}
