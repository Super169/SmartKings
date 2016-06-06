
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
        private const string CMD_teamDuplicateFreeTimes = "TeamDuplicate.teamDuplicateFreeTimes";

        public static RequestReturnObject teamDuplicateFreeTimes(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_teamDuplicateFreeTimes);
        }


    }
}
