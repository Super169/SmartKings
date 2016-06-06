
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Shop
    {
        private const string CMD_availableShops = "Shop.availableShops";
        private const string CMD_buyCycleShopItem = "Shop.buyCycleShopItem";
        private const string CMD_buyShopItem = "Shop.buyShopItem";
        private const string CMD_buyTravelShopItem = "Shop.buyTravelShopItem";
        private const string CMD_getCycleShopInfo = "Shop.getCycleShopInfo";
        private const string CMD_getResourceTradeInfo = "Shop.getResourceTradeInfo";
        private const string CMD_getShopInfo = "Shop.getShopInfo";
        private const string CMD_getShuangShiyiShopInfo = "Shop.getShuangShiyiShopInfo";
        private const string CMD_getTravelShopInfo = "Shop.getTravelShopInfo";
        private const string CMD_otherShopsRefreshTime = "Shop.otherShopsRefreshTime";
        private const string CMD_refreshShop = "Shop.refreshShop";
        private const string CMD_shopNextRefreshTime = "Shop.shopNextRefreshTime";
        private const string CMD_tradeResource = "Shop.tradeResource";


        public static RequestReturnObject buyCycleShopItem(ConnectionInfo ci, string sid, int pos)
        {
            string body = "{\"pos\":" + pos.ToString() + "}";
            return com.SendGenericRequest(ci, sid, CMD_buyCycleShopItem, true, body);
        }


        public static RequestReturnObject buyTravelShopItem(ConnectionInfo ci, string sid, int idx)
        {
            string body = "{\"idx\":" + idx.ToString() + ", \"type\":\"TRAVEL\"}";
            return com.SendGenericRequest(ci, sid, CMD_buyTravelShopItem, true, body);
        }


        public static RequestReturnObject getCycleShopInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getCycleShopInfo);
        }

        public static RequestReturnObject getResourceTradeInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getResourceTradeInfo);
        }


        public static RequestReturnObject getShuangShiyiShopInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getShuangShiyiShopInfo);
        }

        public static RequestReturnObject getTravelShopInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getTravelShopInfo);
        }

        public static RequestReturnObject otherShopsRefreshTime(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_otherShopsRefreshTime);
        }


        public static RequestReturnObject shopNextRefreshTime(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_shopNextRefreshTime);
        }

        public static RequestReturnObject tradeResource(ConnectionInfo ci, string sid, string resource)
        {
            string body = string.Format("{{\"resouce\":\"{0}\"}}", resource);
            return com.SendGenericRequest(ci, sid, CMD_tradeResource, true, body);
        }

    }
}
