
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Arenas
    {
        private const string CMD_changeEnemies = "Arenas.changeEnemies";
        private const string CMD_drawTimeReward = "Arenas.drawTimeReward";
        private const string CMD_getDefFormation = "Arenas.getDefFormation";
        private const string CMD_myArenasStatus = "Arenas.myArenasStatus";

        public static RequestReturnObject changeEnemies(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_changeEnemies);
        }

        public static RequestReturnObject drawTimeReward(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_drawTimeReward);
        }

        public static RequestReturnObject getDefFormation(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getDefFormation);
        }

        public static RequestReturnObject myArenasStatus(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_myArenasStatus);
        }


    }
}
