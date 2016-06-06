using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class NationalWar
    {
        private const string CMD_acquireCityCommandInfo = "NationalWar.acquireCityCommandInfo";
        private const string CMD_acquireNationCardPanelInfo = "NationalWar.acquireNationCardPanelInfo";
        private const string CMD_cityDeclareInfo = "NationalWar.cityDeclareInfo";
        private const string CMD_convertNationCardReward = "NationalWar.convertNationCardReward";
        private const string CMD_getMyTroops = "NationalWar.getMyTroops";


        public static RequestReturnObject acquireCityCommandInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_acquireCityCommandInfo);
        }

        public static RequestReturnObject acquireNationCardPanelInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_acquireNationCardPanelInfo);
        }

        public static RequestReturnObject getMyTroops(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getMyTroops);
        }


    }
}
