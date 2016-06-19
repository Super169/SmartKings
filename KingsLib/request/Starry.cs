using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Starry
    {
        private const string CMD_info = "Starry.info";
        private const string CMD_chapterInfo = "Starry.chapterInfo";
        private const string CMD_fight = "Starry.fight";

        public static RequestReturnObject info(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_info);
        }

        public static RequestReturnObject chapterInfo(ConnectionInfo ci, string sid, int chapterId)
        {
            string body = string.Format("{{\"chapterId\":{0}}}", chapterId);
            return com.SendGenericRequest(ci, sid, CMD_chapterInfo, true, body);
        }

        public static RequestReturnObject fight(ConnectionInfo ci, string sid, int barrierId)
        {
            int campaignId = 111000 + barrierId;
            string body = string.Format("{{\"campaignId\":{0}}}", campaignId);
            return com.SendGenericRequest(ci, sid, CMD_fight, true, body);
        }
    }
}
