using KingsLib.data;
using KingsLib.request;
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
        public static List<ManorInfo> getManorInfo(ConnectionInfo ci, string sid)
        {
            List<ManorInfo> manorInfo = new List<ManorInfo>();
            RequestReturnObject rro = request.Manor.getManorInfo(ci, sid);
            if (!rro.SuccessWithJson(RRO.Manor.buildings, typeof(DynamicJsonArray))) return manorInfo;
            DynamicJsonArray buildings = rro.responseJson[RRO.Manor.buildings];
            foreach (dynamic building in buildings)
            {
                ManorInfo mi = new ManorInfo();
                mi.field = JSON.getString(building, RRO.Manor.field);
                mi.heroIndex = JSON.getString(building, RRO.Manor.heroIndex);
                mi.leftSeconds = JSON.getString(building, RRO.Manor.leftSeconds);
                mi.level = JSON.getString(building, RRO.Manor.level);
                mi.levelSeconds = JSON.getString(building, RRO.Manor.levelSeconds);
                mi.produceSeconds = JSON.getString(building, RRO.Manor.produceSeconds);
                mi.products = JSON.getString(building, RRO.Manor.products);
                mi.type = JSON.getString(building, RRO.Manor.type);
                manorInfo.Add(mi);
            }
            return manorInfo;
        }

        public static int goHarvestField(ConnectionInfo ci, string sid, int field)
        {
            // No special handle required
            RequestReturnObject rro = request.Manor.harvestProduct(ci, sid, field);
            if (!rro.SuccessWithJson(RRO.Manor.product_out)) return -1;
            int outProduct = JSON.getInt(rro.responseJson, RRO.Manor.product_out, -1);
            return outProduct;
        }

    }
}
