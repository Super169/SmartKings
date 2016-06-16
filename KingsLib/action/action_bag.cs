
using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib
{
    public static partial class action
    {
        public static List<BagInfo> getBagInfo(ConnectionInfo ci, string sid)
        {
            List<BagInfo> bis = new List<BagInfo>();
            RequestReturnObject rro = request.Bag.getBagInfo(ci, sid);
            if (!rro.SuccessWithJson(RRO.Bag.items, typeof(DynamicJsonArray))) return null;
            DynamicJsonArray items = rro.responseJson[RRO.Bag.items];

            foreach(dynamic item in items)
            {
                BagInfo bi = new BagInfo();
                bi.idx = JSON.getInt(item, RRO.Bag.idx);
                if (bi.idx >= 0)
                {
                    bi.nm = JSON.getString(item, RRO.Bag.nm, "");
                    bi.n = JSON.getInt(item, RRO.Bag.n);
                    bi.us = JSON.getBool(item, RRO.Bag.us);
                    bis.Add(bi);
                }
            }
            return bis;

        }

    }
}
