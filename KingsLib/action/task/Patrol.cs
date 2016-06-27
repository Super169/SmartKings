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
            public static bool goPatrol(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.Patrol);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                WarInfo wi = oGA.getWarInfo(Scheduler.TaskId.Patrol, 0);
                string fightHeros = null;
                if (wi != null) fightHeros = wi.body;

                 wi = oGA.getWarInfo(Scheduler.TaskId.Patrol, 1);
                string fightHerosBackup = null;
                if (wi != null) fightHerosBackup = wi.body;

                rro = request.Patrol.getPatrolInfo(ci, sid);
                if (!rro.exists(RRO.Patrol.events, typeof(DynamicJsonArray))) return false;
                DynamicJsonArray events = rro.responseJson[RRO.Patrol.events];

                foreach (dynamic patrolEvent in events)
                {
                    int cityId = JSON.getInt(patrolEvent, RRO.Patrol.city);
                    if (cityId > 0)
                    {
                        string title = "";
                        bool useBackup = false;
                        int fightResult = goPatrolCampaign(ci, sid, cityId, ref fightHeros, ref title, true);
                        if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("作戰陣形 {0} ", fightHeros));
                        if ((fightResult == 999) && (fightHerosBackup != null))
                        {
                            useBackup = true;
                            updateInfo(oGA.displayName, taskName, string.Format("{0} 第一部隊作戰失敗, 出動第二部隊", title));
                            fightResult = goPatrolCampaign(ci, sid, cityId, ref fightHerosBackup, ref title, true);
                            if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("作戰陣形 {0} ", fightHerosBackup));
                            LOG.I(string.Format("{0} 民生民情部隊太弱, 未能完成任務", oGA.displayName));
                        }


                        switch (fightResult)
                        {
                            case -2:
                                updateInfo(oGA.displayName, taskName, string.Format("{0} 未能檢查作戰結果", title));
                                break;
                            case -1:
                                updateInfo(oGA.displayName, taskName, string.Format("{0} 出戰失敗", title));
                                break;
                            case 1:
                                updateInfo(oGA.displayName, taskName, string.Format("{0} 尚未完成佈陣", title));
                                break;
                            case 0:
                                updateInfo(oGA.displayName, taskName, string.Format("{0} {1}完成任務", title, (useBackup ? "第二部隊" : "第一部隊")));
                                break;
                            case 999:
                                updateInfo(oGA.displayName, taskName, string.Format("{0} {1}未能完成任務", title, (useBackup ? "派出第一, 二部隊均" : "")));
                                break;
                        }
                    }
                }

                return true;
            }
        }

        // -2 : fail to check result
        // -1 : fail
        // 0  : Win
        // 1  : setup problem
        // 999 : Fail
        private static int goPatrolCampaign(ConnectionInfo ci, string sid, int cityId, ref string fightHeros, ref string title, bool useLastConfig = false)
        {
            RequestReturnObject rro;

            campaign.quitCampaign(ci, sid, 0);

            rro = request.Patrol.dealPatroledEvent(ci, sid, cityId);
            if (!rro.SuccessWithJson(RRO.Patrol.data)) return -1;
            title = rro.getString(RRO.Patrol.title);

            rro = request.Campaign.getAttFormation(ci, sid, RRO.Campaign.march_PATROL_NPC);
            if (!rro.SuccessWithJson(RRO.Campaign.heros, typeof(DynamicJsonArray))) return campaign.quitCampaign(ci, sid, -1);

            if ((fightHeros == null) || (fightHeros == ""))
            {
                if (!useLastConfig) return campaign.quitCampaign(ci, sid, 1);
                if (rro.responseJson[RRO.Campaign.heros].Length < 1) return campaign.quitCampaign(ci, sid, 1);
                dynamic json = JSON.Empty;
                json[RRO.Campaign.heros] = rro.responseJson[RRO.Campaign.heros];
                json[RRO.Campaign.chief] = rro.getInt(RRO.Campaign.chief);
                fightHeros = JSON.encode(json);
            }

            rro = request.Campaign.nextEnemies(ci, sid);
            if (!rro.SuccessWithJson(RRO.Campaign.enemies, typeof(DynamicJsonArray))) return campaign.quitCampaign(ci, sid, -1);
            // Thread.Sleep(500);

            rro = request.Campaign.saveFormation(ci, sid, fightHeros);
            if (!rro.SuccessWithJson(RRO.Campaign.power)) return campaign.quitCampaign(ci, sid, -1);
            // Thread.Sleep(500);

            rro = request.Campaign.fightNext(ci, sid);
            if (rro.ok != 1) return campaign.quitCampaign(ci, sid, -1);

            campaign.quitCampaign(ci, sid, 0);

            // Check for draw
            rro = request.TurnCardReward.getTurnCardRewards(ci, sid);
            if (rro.SuccessWithJson(RRO.TurnCardReward.rewards)) {
                request.TurnCardReward.turnCard(ci, sid, RRO.TurnCardReward.turnCardMode_ONE);
                request.TurnCardReward.turnCard(ci, sid, RRO.TurnCardReward.turnCardMode_ONE);
                // Must win if there has TurnCardReward, so no need to check
                return 0;
            }

            rro = request.Patrol.getPatrolInfo(ci, sid);
            if (!rro.exists(RRO.Patrol.events, typeof(DynamicJsonArray)))
            {
                return -2;
            }
            DynamicJsonArray afterEvents = rro.responseJson[RRO.Patrol.events];
            bool win = true;
            foreach (dynamic o in afterEvents)
            {
                if (JSON.getInt(o, RRO.Patrol.city) == cityId) win = false;
            }
            return (win ? 0 : 999);
        }
    }
}
