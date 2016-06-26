
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class SevenDaysPoints
    {

        private const string CMD_getActInfo = "SevenDaysPoints.getActInfo";
        private const string CMD_getOldInfo = "SevenDaysPoints.getOldInfo";

        public static RequestReturnObject getActInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getActInfo);
        }

        public static RequestReturnObject getOldInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getOldInfo);
        }

    }
}
