
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class City
    {
        private const string CMD_buyProduct = "City.buyProduct";
        private const string CMD_getIndustryInfo = "City.getIndustryInfo";
        private const string CMD_getPlayerCityInfo = "City.getPlayerCityInfo";

        public static RequestReturnObject buyProduct(ConnectionInfo ci, string sid, int industryId, int index)
        {
            string body = string.Format("{{\"industryId\":{0}, \"index\":{1}}}", industryId, index);
            return com.SendGenericRequest(ci, sid, CMD_buyProduct, true, body);
        }

        public static RequestReturnObject getIndustryInfo(ConnectionInfo ci, string sid, int industryId)
        {
            string body = string.Format("{{\"industryId\":{0}}}", industryId);
            return com.SendGenericRequest(ci, sid, CMD_getIndustryInfo, true, body);
        }

        public static RequestReturnObject getPlayerCityInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getPlayerCityInfo);
        }


    }
}
