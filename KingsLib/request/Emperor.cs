
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Emperor
    {
        private const string CMD_collect = "Emperor.collect";
        private const string CMD_collected = "Emperor.collected";
        private const string CMD_draw = "Emperor.draw";
        private const string CMD_getBuyInfo = "Emperor.getBuyInfo";
        private const string CMD_getGameInfo = "Emperor.getGameInfo";
        private const string CMD_isFloat = "Emperor.isFloat";

        public static RequestReturnObject collected(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_collected);
        }

        public static RequestReturnObject draw(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_draw);
        }

        public static RequestReturnObject getBuyInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getBuyInfo);
        }

        public static RequestReturnObject getGameInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getGameInfo);
        }

        public static RequestReturnObject isFloat(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_isFloat);
        }



    }
}
