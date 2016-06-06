using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Drama
    {
        private const string CMD_getDramaInfo = "Drama.getDramaInfo";
        public static RequestReturnObject getDramaInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getDramaInfo);
        }


    }
}
