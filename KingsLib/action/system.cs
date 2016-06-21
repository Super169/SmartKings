
using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class action
    {
        public static class system
        {
            public static long getTime(ConnectionInfo ci, string sid)
            {
                long currTime = 0;
                RequestReturnObject rro = request.System.ping(ci, sid);
                if (rro.SuccessWithJson(RRO.System.serverTime))
                {
                    currTime = rro.getLong(RRO.System.serverTime);
                }
                else
                {
                    TimeSpan t = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
                    currTime = (long)(t.TotalMilliseconds + 0.5);
                }
                return currTime;
            }
        }
    }
}
