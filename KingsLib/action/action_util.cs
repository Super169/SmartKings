
using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

// Checking included:
// Check 討伐群雄
// Check 英雄切磋
// 英雄試練
// 王者獎勵/保級賽
// 周遊天下
// 遠征西域
// 嘉年華活動
// 神器打造
// 誇服入侵
// 皇榜
// 五福臨門
// 草船借箭
// 奇門八卦

namespace KingsLib
{
    public static partial class action
    {
        public delegate bool DelegateCheckOutstandingTask(GameAccount oGA, DelegateUpdateInfo updateInfo, string action, string module, bool debug);

        public static void showDebugMsg(DelegateUpdateInfo updateInfo, string account, string action, string msg)
        {
            updateInfo(account, action, "**** " + msg, true, false);
            LOG.D(string.Format("{0} : {1} : {2}", account, action, msg));
        }
    }
}
