
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class MonthSignIn
    {
        private const string CMD_getInfo = "MonthSignIn.getInfo";
        private const string CMD_getOpenInfo = "MonthSignIn.getOpenInfo";
        private const string CMD_signInToday = "MonthSignIn.signInToday";

        public static RequestReturnObject getInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getInfo);
        }

        public static RequestReturnObject getOpenInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getOpenInfo);
        }

        public static RequestReturnObject signInToday(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_signInToday);
        }

    }
}
