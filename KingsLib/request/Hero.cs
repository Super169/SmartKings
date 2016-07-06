
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Hero
    {
        private const string CMD_assessScore = "Hero.assessScore";
        private const string CMD_feastHero = "Hero.feastHero";
        private const string CMD_getConvenientFormations = "Hero.getConvenientFormations";
        private const string CMD_getCurrRecommendActivityInfo = "Hero.getCurrRecommendActivityInfo";
        private const string CMD_getFeastInfo = "Hero.getFeastInfo";
        private const string CMD_getHeroIconInfo = "Hero.getHeroIconInfo";
        private const string CMD_getPlayerHeroList = "Hero.getPlayerHeroList";
        private const string CMD_getScoreHero = "Hero.getScoreHero";
        private const string CMD_getVisitHeroInfo = "Hero.getVisitHeroInfo";
        private const string CMD_getWineInfo = "Hero.getWineInfo";
        private const string CMD_matchHero = "Hero.matchHero";


        public static RequestReturnObject assessScore(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_assessScore);
        }

        public static RequestReturnObject feastHero(ConnectionInfo ci, string sid, bool free, string type)
        {
            string body = string.Format("{{\"free\":{0}, \"type\":\"{1}\"}}", free, type);
            return com.SendGenericRequest(ci, sid, CMD_feastHero, true, body);
        }

        public static RequestReturnObject getConvenientFormations(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getConvenientFormations);
        }

        public static RequestReturnObject getCurrRecommendActivityInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getCurrRecommendActivityInfo);
        }

        public static RequestReturnObject getFeastInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getFeastInfo);
        }

        public static RequestReturnObject getHeroIconInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getHeroIconInfo);
        }

        public static RequestReturnObject getPlayerHeroList(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getPlayerHeroList);
        }

        /*
        public static RequestReturnObject getScoreHero(ConnectionInfo ci, string sid, string body)
        {
            return com.SendGenericRequest(ci, sid, CMD_getScoreHero, true, body);
        }
        */
        public static RequestReturnObject getVisitHeroInfo(ConnectionInfo ci, string sid, string heroName)
        {
            string body = string.Format("{{\"heroName\":\"{0}\"}}", heroName);
            return com.SendGenericRequest(ci, sid, CMD_getVisitHeroInfo, true, body);
        }


        public static RequestReturnObject getWineInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getWineInfo);
        }




    }
}
