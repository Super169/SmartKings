
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
        public static class GrassArrow
        {
            // -1 : fail
            // 0  : Win
            // 1  : setup problem
            public static int goGrassArrowFight(ConnectionInfo ci, string sid, string fightHeros)
            {
                RequestReturnObject rro;

                campaign.quitCampaign(ci, sid, 0);

                if ((fightHeros == null) || (fightHeros.Trim() == "")) return 1;

                rro = request.GrassArrow.doGrassArrowFight(ci, sid);
                if (!rro.SuccessWithJson(RRO.Campaign.data, RRO.Campaign.state)) return campaign.quitCampaign(ci, sid, -1);
                string state = JSON.getString(rro.responseJson[RRO.Campaign.data], RRO.Campaign.state, "");
                if (state != RRO.Campaign.state_PREPARE) return campaign.quitCampaign(ci, sid, -1);

                rro = request.Campaign.getAttFormation(ci, sid, RRO.Campaign.march_GRASS_ARROW);
                if (!rro.SuccessWithJson(RRO.Campaign.heros, typeof(DynamicJsonArray))) return campaign.quitCampaign(ci, sid, -1);

                rro = request.Campaign.nextEnemies(ci, sid);
                if (!rro.SuccessWithJson(RRO.Campaign.enemies, typeof(DynamicJsonArray))) return campaign.quitCampaign(ci, sid, -1);

                rro = request.Campaign.saveFormation(ci, sid, fightHeros);
                if (!rro.SuccessWithJson(RRO.Campaign.power)) return campaign.quitCampaign(ci, sid, -1);

                rro = request.Campaign.fightNext(ci, sid);
                if (rro.ok != 1) return campaign.quitCampaign(ci, sid, -1);

                campaign.quitCampaign(ci, sid, 0);
                return 0;
            }

        }
    }
}
