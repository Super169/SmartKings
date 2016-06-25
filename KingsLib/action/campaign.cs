
using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib
{
    public static partial class action
    {
        public static class campaign
        {
            // Special function for quit Campaign at any time with expected return value
            public static int quitCampaign(ConnectionInfo ci, string sid, int returnValue)
            {
                request.Campaign.quitCampaign(ci, sid);
                return returnValue;
            }

            public static int eliteFight(ConnectionInfo ci, string sid, string difficult, int stage, int chapter, string fightHeros)
            {
                RequestReturnObject rro;

                // For safety, always do Campaign.quitCampaign before any war (maybe before any action later if possible).
                campaign.quitCampaign(ci, sid, 0);

                rro = request.Campaign.getLeftTimes(ci, sid);
                if (!rro.SuccessWithJson(RRO.Campaign.elite)) return campaign.quitCampaign(ci, sid, -1);

                rro = request.Campaign.eliteFight(ci, sid, difficult, stage, chapter);
                if (!rro.SuccessWithJson(RRO.Campaign.data, RRO.Campaign.state)) return campaign.quitCampaign(ci, sid, -1);
                string state = JSON.getString(rro.responseJson[RRO.Campaign.data], RRO.Campaign.state, "");
                if (state != RRO.Campaign.state_PREPARE) return campaign.quitCampaign(ci, sid, -1);
                // TODO: add check for no more fight


                rro = request.Campaign.getAttFormation(ci, sid, "ELITE");
                if (!rro.SuccessWithJson(RRO.Campaign.heros, typeof(DynamicJsonArray))) return campaign.quitCampaign(ci, sid, -1);

                string body;
                if ((fightHeros == null) || (fightHeros == ""))
                {
                    if (rro.responseJson[RRO.Campaign.heros].Length < 5) return campaign.quitCampaign(ci, sid, 1);
                    dynamic json = JSON.Empty;
                    json[RRO.Campaign.heros] = rro.responseJson[RRO.Campaign.heros];
                    json[RRO.Campaign.chief] = rro.getInt(RRO.Campaign.chief);
                    body = JSON.encode(json);
                }
                else
                {
                    body = fightHeros;
                }

                rro = request.Campaign.nextEnemies(ci, sid);
                if (!rro.SuccessWithJson(RRO.Campaign.enemies, typeof(DynamicJsonArray))) return campaign.quitCampaign(ci, sid, -1);
                // Thread.Sleep(500);

                rro = request.Campaign.saveFormation(ci, sid, body);
                if (!rro.SuccessWithJson(RRO.Campaign.power)) return campaign.quitCampaign(ci, sid, -1);
                // Thread.Sleep(500);

                rro = request.Campaign.fightNext(ci, sid);
                if (rro.ok != 1) return campaign.quitCampaign(ci, sid, -1);

                rro = request.Campaign.nextEnemies(ci, sid);
                if (!rro.SuccessWithJson(RRO.Campaign.enemies, typeof(DynamicJsonArray))) return campaign.quitCampaign(ci, sid, -1);
                // Thread.Sleep(500);

                rro = request.Campaign.fightNext(ci, sid);
                if (rro.ok != 1) return campaign.quitCampaign(ci, sid, -1);

                return campaign.quitCampaign(ci, sid, 0);
            }


        }

    }
}
