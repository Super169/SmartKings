
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

        public static class starry
        {
            public static StarryInfo getInfo(ConnectionInfo ci, string sid)
            {
                StarryInfo si = new StarryInfo();
                RequestReturnObject rro;

                rro = request.Starry.info(ci, sid);
                if (!rro.success) return null;

                si = StarryInfo.fromJaon(rro.responseJson);
                return si;
            }

            public static StarryInfo.ChapterInfo getChapterInfo(ConnectionInfo ci, string sid, int chapterId)
            {
                RequestReturnObject rro;
                rro = request.Starry.chapterInfo(ci, sid, chapterId);
                if (!rro.success) return null;

                StarryInfo.ChapterInfo chapterInfo = StarryInfo.ChapterInfo.fromJson(rro.responseJson);
                return chapterInfo;
            }

            public static int fight(ConnectionInfo ci, string sid, int barrierId, string fightHeros)
            {
                RequestReturnObject rro;

                // For safety, always do Campaign.quitCampaign before any war (maybe before any action later if possible).
                campaign.quitCampaign(ci, sid, 0);

                rro = request.Starry.fight(ci, sid, barrierId);
                if (!rro.SuccessWithJson(RRO.Starry.data)) return campaign.quitCampaign(ci, sid, -1);
                if (!(rro.exists(RRO.Starry._type) && rro.exists(RRO.Starry._rs))) return campaign.quitCampaign(ci, sid, -1);
                string fight_type = rro.getString(RRO.Starry._type, null);
                int fight_rs = rro.getInt(RRO.Starry._rs);
                if (!((fight_type == RRO.Starry._type_SCEnterCampaign) && (fight_rs == 1))) return campaign.quitCampaign(ci, sid, -1);

                rro = request.Campaign.getAttFormation(ci, sid, "STARRY");
                if (!rro.SuccessWithJson(RRO.Campaign.heros, typeof(DynamicJsonArray))) return campaign.quitCampaign(ci, sid, -1);
                // Thread.Sleep(500);

                string body;
                if ((fightHeros == null) || (fightHeros == ""))
                {
                    if (rro.responseJson[RRO.Campaign.heros].Length < 5) return campaign.quitCampaign(ci, sid, 1);
                    dynamic json = JSON.Empty;
                    json[RRO.Campaign.heros] = rro.responseJson[RRO.Campaign.heros];
                    json[RRO.Campaign.chief] = rro.getInt(RRO.Campaign.chief);
                    body = JSON.encode(json);
                } else
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

                return campaign.quitCampaign(ci, sid, 0);
            }
        }

    }
}
