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
            // After lottery, it need to reload to display the updated iron quantity 

            public static bool goLottery(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.Lottery);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Lottery.refreshLottery(ci, sid);
                if (!rro.SuccessWithJson(RRO.Lottery.freeLotteryTimes)) return false;

                int freeLotteryTimes = rro.getInt(RRO.Lottery.freeLotteryTimes);
                int rollCnt = 0;
                while (freeLotteryTimes > 0)
                {
                    rro = request.Lottery.drawLottery(ci, sid);
                    if (rro.ok != 1)
                    {
                        if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "轉動輪盤失敗");
                        LOG.E(string.Format("{0} - {1} : unexpected return {2}", oGA.displayName, taskName, rro.responseText));
                        break;
                    }
                    rollCnt++;

                    rro = request.Lottery.refreshLottery(ci, sid);
                    if (!rro.SuccessWithJson(RRO.Lottery.freeLotteryTimes)) break;
                    freeLotteryTimes = rro.getInt(RRO.Lottery.freeLotteryTimes);
                }

                updateInfo(oGA.displayName, taskName, string.Format("轉動輪盤 {0} 次", rollCnt));

                return true;
            }
        }
    }
}
