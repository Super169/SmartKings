
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    class OperateActivity
    {
        private const string CMD_getUpgradeActivityInfo = "OperateActivity.getUpgradeActivityInfo";
        private const string CMD_upgradeActivityReward = "OperateActivity.upgradeActivityReward";

        public static RequestReturnObject getUpgradeActivityInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getUpgradeActivityInfo);
        }

        public static RequestReturnObject upgradeActivityReward(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_upgradeActivityReward);
        }

    }
}
