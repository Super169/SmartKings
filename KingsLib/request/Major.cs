using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Major
    {
        private const string CMD_getMyMajorInfo = "Major.getMyMajorInfo";

        public static RequestReturnObject getMyMajorInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getMyMajorInfo);
        }


    }
}
