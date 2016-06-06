﻿
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Valentine
    {
        private const string CMD_getActivityInfo = "Valentine.getActivityInfo";

        public static RequestReturnObject getActivityInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getActivityInfo);
        }


    }
}
