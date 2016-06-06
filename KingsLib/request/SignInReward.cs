
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class SignInReward
    {
        private const string CMD_getInfo = "SignInReward.getInfo";
        private const string CMD_signIn = "SignInReward.signIn";
        private const string CMD_signInMultiple = "SignInReward.signInMultiple";

        public static RequestReturnObject getInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getInfo);
        }

        public static RequestReturnObject signIn(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_signIn);
        }

    }
}
