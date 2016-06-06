
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class EightTrigrams
    {
        private const string CMD_attack = "EightTrigrams.attack";
        private const string CMD_getInfo = "EightTrigrams.getInfo";
        private const string CMD_open = "EightTrigrams.open";
        private const string CMD_openBox = "EightTrigrams.openBox";

        public static RequestReturnObject getInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getInfo);
        }

        public static RequestReturnObject open(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_open);
        }

        public static RequestReturnObject openBox(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_openBox);
        }

    }
}
