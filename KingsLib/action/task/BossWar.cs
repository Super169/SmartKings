﻿using KingsLib.data;
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
            public static bool goBassWar(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.BossWar;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                action.campaign.quitCampaign(ci, sid, 0);

                rro = request.BossWar.enterWar(ci, sid);
                if (!rro.success) return false;
                if ((rro.style == STYLE.ERROR) && (rro.prompt == PROMPT.ERR_COMMON_NOT_SUPPORT))
                {
                    if (oGA.bwStarted)
                    {
                        oGA.bwEnded = true;
                        updateInfo(oGA.displayName, taskName, "已經完結");
                        return true;
                    }
                    return false;
                }
                int beforeCnt = rro.getInt(RRO.BossWar.sendCount);
                int bossHP = -1;
                if (rro.SuccessWithJson(RRO.BossWar.bossInfo, RRO.BossWar.hpp))
                {
                    bossHP = JSON.getInt(rro.responseJson[RRO.BossWar.bossInfo], RRO.BossWar.hpp, -1);
                }

                rro = request.BossWar.sendTroop(ci, sid, oGA.bwBody);
                if (rro.ok != 1) return false;

                oGA.bwStarted = true;
                oGA.bwEnded = false;
                oGA.bwLastSend = DateTime.Now;
                if (beforeCnt == 0) oGA.bwFailCnt = 0;
                rro = request.BossWar.leaveWar(ci, sid);

                updateInfo(oGA.displayName, taskName, string.Format("神將HP: {0}, 第 {1} 次出兵", bossHP, beforeCnt + 1));
                return true;
            }


        }
    }
}
