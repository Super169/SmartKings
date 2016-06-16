
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Bag
    {
        private const string CMD_getBagInfo = "Bag.getBagInfo";
        private const string CMD_useItem = "Bag.useItem";

        public static RequestReturnObject getBagInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getBagInfo);
        }

        public static RequestReturnObject useItem(ConnectionInfo ci, string sid, int num, int index)
        {
            string body = string.Format("{{\"parmList\":[\"-1\"], \"num\":{0}, \"index\":{1}}}", num, index);
            return com.SendGenericRequest(ci, sid, CMD_useItem, true, body);
        }


    }
}
