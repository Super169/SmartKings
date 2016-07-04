using KingsLib.data;
using KingsLib.scheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class action
    {
        public static partial class task
        {

            public static bool goReload(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.Reload;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                if (oGA.pubGameServerId <= 0)
                {
                    updateInfo(oGA.displayName, taskName, "找不到相關的 pubGame 設定, 暫時只支持 pubGame 帳戶重啟");
                    return true;
                }
                string url = "http://www.pubgame.tw/play.do?gc=king&gsc=" + oGA.pubGameServerId.ToString();
                Process.Start("chrome.exe", url);
                int waitCnt = 0;
                // Wait a maximum of 1 minute
                while (waitCnt < 12)
                {
                    Thread.Sleep(5000);
                    if (sid != oGA.sid) waitCnt = 100;
                }
                // Wait extra 5s for safety, as some system prcess may still running
                Thread.Sleep(5000);
                return true;
            }
        }
    }
}
