
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class OneYearEntry
    {

        private const string CMD_draw = "OneYearEntry.draw";
        private const string CMD_getOneYearEntryInfo = "OneYearEntry.getOneYearEntryInfo";

        public static RequestReturnObject draw(ConnectionInfo ci, string sid, string type)
        {
            string body = string.Format("{{\"type\":{0}}}", type);
            return com.SendGenericRequest(ci, sid, CMD_draw, true, body);
        }

        public static RequestReturnObject getOneYearEntryInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getOneYearEntryInfo);
        }

    }
}
