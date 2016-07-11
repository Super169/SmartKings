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

            public static bool goMonthSignIn(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                bool retValue = goSignInToday(oGA, updateInfo, debug);

                goAddUpRewards(oGA, updateInfo, debug);

                // Just indicate the result for SignInToday
                return retValue;
                /*
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.MonthSignIn);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                RequestReturnObject rro = request.MonthSignIn.getInfo(ci, sid);
                if (!(rro.SuccessWithJson(RRO.MonthSignIn.today) && rro.exists(RRO.MonthSignIn.msItems, typeof(DynamicJsonArray)))) return false;
                int today = rro.getInt(RRO.MonthSignIn.today);
                if (today <= 0)
                {
                    LOG.E(oGA.displayName, taskName, string.Format("MonthSignIn.getInfo 出錯： {0}", rro.responseText));
                    return false;
                }
                bool alreadySign = false;
                DynamicJsonArray msItems = rro.responseJson[RRO.MonthSignIn.msItems];
                foreach(dynamic msItem in msItems)
                {
                    int day = JSON.getInt(msItem, RRO.MonthSignIn.day, -1);
                    if (day == today)
                    {
                        int st = JSON.getInt(msItem, RRO.MonthSignIn.st, 0);
                        alreadySign = ((st == 1) || (st == 3));
                    }
                }
                if (alreadySign)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("已經進行第 {0} 天簽到", today));
                    return true;
                }

                rro = request.MonthSignIn.signInToday(ci, sid);
                if (rro.ok != 1)
                {
                    updateInfo(oGA.displayName, taskName, string.Format("進行第{0}天簽到失敗", today), true, false);
                    return false;
                }

                updateInfo(oGA.displayName, taskName, string.Format("完成第{0}天簽到", today), true, false);
                return true;
                */
            }

            public static bool goSignInToday(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.MonthSignIn);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                RequestReturnObject rro = request.MonthSignIn.getInfo(ci, sid);
                if (!(rro.SuccessWithJson(RRO.MonthSignIn.today) && rro.exists(RRO.MonthSignIn.msItems, typeof(DynamicJsonArray)))) return false;
                int today = rro.getInt(RRO.MonthSignIn.today);
                if (today <= 0)
                {
                    LOG.E(oGA.displayName, taskName, string.Format("MonthSignIn.getInfo 出錯： {0}", rro.responseText));
                    return false;
                }
                bool alreadySign = false;
                DynamicJsonArray msItems = rro.responseJson[RRO.MonthSignIn.msItems];
                foreach (dynamic msItem in msItems)
                {
                    int day = JSON.getInt(msItem, RRO.MonthSignIn.day, -1);
                    if (day == today)
                    {
                        int st = JSON.getInt(msItem, RRO.MonthSignIn.st, 0);
                        alreadySign = ((st == 1) || (st == 3));
                    }
                }
                if (alreadySign)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("已經進行第 {0} 天簽到", today));
                    return true;
                }

                rro = request.MonthSignIn.signInToday(ci, sid);
                if (rro.ok != 1)
                {
                    updateInfo(oGA.displayName, taskName, string.Format("進行第{0}天簽到失敗", today), true, false);
                    return false;
                }

                updateInfo(oGA.displayName, taskName, string.Format("完成第{0}天簽到", today), true, false);
                return true;
            }

            public static bool goAddUpRewards(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.MonthSignIn);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                RequestReturnObject rro = request.MonthSignIn.getInfo(ci, sid);
                if (!(rro.SuccessWithJson(RRO.MonthSignIn.today) && rro.exists(RRO.MonthSignIn.auItems, typeof(DynamicJsonArray)))) return false;
                int today = rro.getInt(RRO.MonthSignIn.today);

                DynamicJsonArray auItems = rro.responseJson[RRO.MonthSignIn.auItems];
                foreach (dynamic auItem in auItems)
                {
                    int days = JSON.getInt(auItem, RRO.MonthSignIn.days);
                    bool drawed = JSON.getBool(auItem, RRO.MonthSignIn.drawed, true);
                    if ((days <= today) && !drawed)
                    {
                        rro = request.MonthSignIn.drawAddUpRwds(ci, sid, days);
                        if (rro.ok == 1)
                        {
                            rro = request.Activity.drawCompanyAnniversaryRechargeReward(ci, sid);
                            updateInfo(oGA.displayName, taskName, string.Format("領取 {0} 天登入獎勵", days));
                        } 
                    }
                }
                return true;
            }

            public static bool goOneYearSignIn(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.OneYearSignIn);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                int eventCount = action.OneYear.eventCount(ci, sid);
                if (eventCount < 0) return false;
                if (eventCount == 0)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "今天沒有嘉年華活動");
                    return true;
                }

                int ticket = action.OneYear.drawTicket(ci, sid);
                if (ticket < 0)
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "登陸領取張嘉年華入場劵失敗");
                }
                else if (ticket > 0)
                {
                    updateInfo(oGA.displayName, taskName, string.Format("登陸並領取了 {0} 張嘉年華入場劵", ticket), true, false);
                }
                return true;
            }
        }
    }
}
