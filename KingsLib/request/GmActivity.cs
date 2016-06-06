using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class GmActivity
    {
        private const string CMD_superPackageInfo = "GmActivity.superPackageInfo";

        public static RequestReturnObject superPackageInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_superPackageInfo);
        }

    }
}
