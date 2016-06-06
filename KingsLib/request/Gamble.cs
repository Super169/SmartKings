
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Gamble
    {
        private const string CMD_chouqianOpenInfo = "Gamble.chouqianOpenInfo";

        public static RequestReturnObject chouqianOpenInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_chouqianOpenInfo);
        }


    }
}
