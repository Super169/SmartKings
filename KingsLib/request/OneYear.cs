
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class OneYear
    {
        private const string CMD_cityStatus = "OneYear.cityStatus";
        private const string CMD_draw = "OneYear.draw";
        private const string CMD_info = "OneYear.info";

        public static RequestReturnObject cityStatus(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_cityStatus);
        }

        public static RequestReturnObject draw(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_draw);
        }

        public static RequestReturnObject info(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_info);
        }


    }
}
