
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class TeamDuplicate
    {
        private const string CMD_duplicateList = "TeamDuplicate.duplicateList";
        private const string CMD_refreshDuplicate = "TeamDuplicate.refreshDuplicate";
        private const string CMD_teamDuplicateFreeTimes = "TeamDuplicate.teamDuplicateFreeTimes";

        private const string CMD_createTeamDuplicate = "TeamDuplicate.createTeamDuplicate";
        private const string CMD_heroInBattle = "TeamDuplicate.heroInBattle";
        private const string CMD_battleStart = "TeamDuplicate.battleStart";
        private const string CMD_exitTeam = "TeamDuplicate.exitTeam";


        public static RequestReturnObject battleStart(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_battleStart);
        }

        public static RequestReturnObject createTeamDuplicate(ConnectionInfo ci, string sid, int duplicateId)
        {
            string body = string.Format("{{\"duplicateId\":{0}}}", duplicateId);
            return com.SendGenericRequest(ci, sid, CMD_createTeamDuplicate, true, body);
        }

        public static RequestReturnObject duplicateList(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_duplicateList);
        }

        public static RequestReturnObject exitTeam(ConnectionInfo ci, string sid, int teamId)
        {
            string body = string.Format("{{\"teamId\":{0}}}", teamId);
            return com.SendGenericRequest(ci, sid, CMD_exitTeam, true, body);
        }

        public static RequestReturnObject heroInBattle(ConnectionInfo ci, string sid, string heroIdx, int teamId)
        {
            string body = string.Format("{{\"heroIdx\":{0}, \"teamId\":{1}}}", heroIdx, teamId);
            return com.SendGenericRequest(ci, sid, CMD_heroInBattle, true, body);
        }

        public static RequestReturnObject refreshDuplicate(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_refreshDuplicate);
        }

        public static RequestReturnObject teamDuplicateFreeTimes(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_teamDuplicateFreeTimes);
        }

    }
}
