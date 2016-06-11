using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    class DianJiangTai
    {

        private const string CMD_beforeStart = "DianJiangTai.beforeStart";

        public static RequestReturnObject beforeStart(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_beforeStart);
        }

    }
}
