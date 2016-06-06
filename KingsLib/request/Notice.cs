using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Notice
    {
        private const string CMD_queryAllMarqueeMessage = "Notice.queryAllMarqueeMessage";

        public static RequestReturnObject queryAllMarqueeMessage(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_queryAllMarqueeMessage);
        }


    }
}
