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

                rro = request.Patrol.getPatrolInfo(ci, sid);
                if (!rro.exists(RRO.Patrol.events, typeof(DynamicJsonArray))) return false;
                DynamicJsonArray events = rro.responseJson[RRO.Patrol.events];

                foreach (dynamic patrolEvent in events)
                {
                    int cityId = JSON.getInt(patrolEvent, RRO.Patrol.city);
                    if (cityId > 0)
                    {
                        string title = "";
                        int fightResult = goPatrolCampaign(ci, sid, cityId, ref fightHeros, ref title, true);
                        if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("作戰陣形 {0} ", fightHeros));

                        if (fightResult == -1)
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("{0} 出戰失敗", title));
                        }
                        else if (fightResult == 1)
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("{0} 尚未完成佈陣", title));
                        } else
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("{0} 完成任務", title));
                        }
                    }
                }

                return true;
            }
        }

        // -1 : fail
        // 0  : ok
        // 1  : setup problem
        private static int goPatrolCampaign(ConnectionInfo ci, string sid, int cityId, ref string fightHeros, ref string title, bool useLastConfig = false)
        {
            RequestReturnObject rro;

            campaign.quitCampaign(ci, sid, 0);

            rro = request.Patrol.dealPatroledEvent(ci, sid, cityId);
            if (!rro.SuccessWithJson(RRO.Patrol.data)) return -1;
            title = rro.getString(RRO.Patrol.title);

            rro = request.Campaign.getAttFormation(ci, sid, "PATROL_NPC");
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
            if (!rro.SuccessWithJson(RRO.TurnCardReward.rewards)) return 0;

            request.TurnCardReward.turnCard(ci, sid, RRO.TurnCardReward.turnCardMode_ONE);
            request.TurnCardReward.turnCard(ci, sid, RRO.TurnCardReward.turnCardMode_ONE);

            return 0;
        }
    }
}
