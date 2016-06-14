
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

        public static RequestReturnObject duplicateList(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_duplicateList);
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
