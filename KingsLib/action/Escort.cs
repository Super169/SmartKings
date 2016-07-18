
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
        public static class Escort
        {
            public static bool goLoot(GameAccount oGA, int id, string body)
            {
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.Escort.getDefFormation(ci, sid, id);
                if (!rro.SuccessWithJson(RRO.Escort.displayHero, typeof(DynamicJsonArray))) return false;

                rro = request.Escort.loot(ci, sid, id);
                if (!rro.SuccessWithJson(RRO.Escort.data)) return false;

                rro = request.Campaign.getAttFormation(ci, sid, "ESCORT");
                if (!rro.success) return (campaign.quitCampaign(ci, sid, 1) == 0);
                Thread.Sleep(100);

                rro = request.Campaign.nextEnemies(ci, sid);
                if (!rro.success) return (campaign.quitCampaign(ci, sid, 1) == 0);
                Thread.Sleep(100);

                rro = request.Campaign.saveFormation(ci, sid, body);
                if (!rro.success) return (campaign.quitCampaign(ci, sid, 1) == 0);
                Thread.Sleep(100);

                rro = request.Campaign.fightNext(ci, sid);
                if (!rro.success) return (campaign.quitCampaign(ci, sid, 1) == 0);
                Thread.Sleep(100);

                campaign.quitCampaign(ci, sid, 0);

                return true;
            }
        }
    }
}
