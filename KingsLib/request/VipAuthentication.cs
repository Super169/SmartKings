using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class VipAuthentication
    {
        private const string CMD_isGotMobileGift = "VipAuthentication.isGotMobileGift";

        public static RequestReturnObject isGotMobileGift(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_isGotMobileGift);
        }


    }
}
