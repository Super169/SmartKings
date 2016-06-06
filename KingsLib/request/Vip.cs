
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Vip
    {
        private const string CMD_firstChargeInfo = "Vip.firstChargeInfo";
        private const string CMD_monthCard = "Vip.monthCard";
        private const string CMD_vipPrivilegeLevel = "Vip.vipPrivilegeLevel";

        public static RequestReturnObject firstChargeInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_firstChargeInfo);
        }

        public static RequestReturnObject monthCard(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_monthCard);
        }


    }
}
