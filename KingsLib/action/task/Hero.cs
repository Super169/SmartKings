using KingsLib.data;
using KingsLib.scheduler;
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
            public static bool goFeastHero(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.FeastHero);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Hero.getFeastInfo(ci, sid);
                if (!rro.success) return false;
                if (!(rro.exists(RRO.Hero.commonLeftSeconds) && rro.exists(RRO.Hero.goldLeftSeconds)))
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("Hero.getFeastInfo: 回傳出錯: {0}", rro.responseText));
                    return false;
                }
                int commonLeftSeconds = rro.getInt(RRO.Hero.commonLeftSeconds);
                int goldLeftSeconds = rro.getInt(RRO.Hero.goldLeftSeconds);
                bool retStatus = true;
                if (commonLeftSeconds == 0)
                {
                    rro = request.Hero.feastHero(ci, sid, true, RRO.Hero.type_COMMON_FIVE);
                    if (rro.SuccessWithJson(RRO.Hero.discount))
                    {
                        updateInfo(oGA.displayName, taskName, "進行一次免費酒宴");
                    }
                    else
                    {
                        updateInfo(oGA.displayName, taskName, "進行免費酒宴失敗");
                        retStatus = false;
                    }
                }
                if (goldLeftSeconds == 0)
                {
                    rro = request.Hero.feastHero(ci, sid, true, RRO.Hero.type_GOLD_ONE);
                    if (rro.SuccessWithJson(RRO.Hero.discount))
                    {
                        updateInfo(oGA.displayName, taskName, "進行一次免費盛宴");
                    }
                    else
                    {
                        updateInfo(oGA.displayName, taskName, "進行免費盛宴失敗");
                        retStatus = false;
                    }
                }
                return retStatus;
            }

            public static DateTime? getFeastHeroNextTime(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.FeastHero);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Hero.getFeastInfo(ci, sid);
                if (!rro.success) return null;
                if (!(rro.exists(RRO.Hero.commonLeftSeconds) && rro.exists(RRO.Hero.goldLeftSeconds)))
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("Hero.getFeastInfo: 回傳出錯: {0}", rro.responseText));
                    return null;
                }
                int commonLeftSeconds = rro.getInt(RRO.Hero.commonLeftSeconds);
                int goldLeftSeconds = rro.getInt(RRO.Hero.goldLeftSeconds);
                return DateTime.Now.AddSeconds(Math.Min(commonLeftSeconds, goldLeftSeconds));
            }


            public static bool goWineHero(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.WineHero);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Hero.getWineInfo(ci, sid);
                if (!rro.success) return false;
                if (!(rro.exists(RRO.Hero.nxtTm) && rro.exists(RRO.Hero.wineConsume) && rro.exists(RRO.Hero.wineTms)))
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("Hero.getWineInfo: 回傳出錯: {0}", rro.responseText));
                    return false;
                }
                int nxtTm = rro.getInt(RRO.Hero.nxtTm);
                int wineConsume = rro.getInt(RRO.Hero.wineConsume);
                int wineTms = rro.getInt(RRO.Hero.wineTms);
                if ((wineConsume == 0) && (wineTms == 0))
                {
                    rro = request.Hero.wineHero(ci, sid);
                    if (!rro.success) return false;
                    if (!rro.exists(RRO.Hero.items, typeof(DynamicJsonArray)))
                    {
                        updateInfo(oGA.displayName, taskName, string.Format("Hero.wineHero: 回傳出錯: {0}", rro.responseText));
                        return false;
                    }
                    else
                    {
                        updateInfo(oGA.displayName, taskName, "成功進行一次 煮酒論英雄");
                        return true;
                    }
                }

                updateInfo(oGA.displayName, taskName, "進行中, 等待宴席結束");
                return true;

            }

            public static DateTime? getWineHeroNextTime(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.WineHero);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Hero.getWineInfo(ci, sid);
                if (!rro.success) return null;
                if (!(rro.exists(RRO.Hero.nxtTm) && rro.exists(RRO.Hero.wineConsume) && rro.exists(RRO.Hero.wineTms)))
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("Hero.getWineInfo: 回傳出錯: {0}", rro.responseText));
                    return null;
                }
                int nxtTm = rro.getInt(RRO.Hero.nxtTm);
                int wineConsume = rro.getInt(RRO.Hero.wineConsume);
                int wineTms = rro.getInt(RRO.Hero.wineTms);
                if ((wineConsume == 0) && (wineTms == 0)) { return DateTime.Now.AddSeconds(1); }

                return DateTime.Now.AddSeconds(nxtTm);

            }

        }
    }
}
