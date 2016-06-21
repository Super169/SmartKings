
using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib
{
    public static partial class action
    {
        public static class campaign
        {
            // Special function for quit Campaign at any time with expected return value
            public static int quitCampaign(ConnectionInfo ci, string sid, int returnValue)
            {
                request.Campaign.quitCampaign(ci, sid);
                return returnValue;
            }
        }

    }
}
