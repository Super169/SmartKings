
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class BossWar
    {
        private const string CMD_bossInfo = "BossWar.bossInfo ";
        private const string CMD_bossLineup = "BossWar.bossLineup ";
        private const string CMD_enterWar = "BossWar.enterWar ";
        private const string CMD_keyKillInfo = "BossWar.keyKillInfo ";
        private const string CMD_leaveWar = "BossWar.leaveWar ";
        private const string CMD_openInfo = "BossWar.openInfo ";
        private const string CMD_pk = "BossWar.pk ";
        private const string CMD_rankInfo = "BossWar.rankInfo ";
        private const string CMD_sendTroop = "BossWar.sendTroop ";

        public static RequestReturnObject bossInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_bossInfo);
        }

        public static RequestReturnObject bossLineup(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_bossLineup);
        }

        public static RequestReturnObject enterWar(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_enterWar);
        }

        public static RequestReturnObject keyKillInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_keyKillInfo);
        }

        public static RequestReturnObject leaveWar(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_leaveWar);
        }

        public static RequestReturnObject openInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_openInfo);
        }

        public static RequestReturnObject pk(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_pk);
        }

        public static RequestReturnObject rankInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_rankInfo);
        }

        public static RequestReturnObject sendTroop(ConnectionInfo ci, string sid, string body)
        {
            return com.SendGenericRequest(ci, sid, CMD_sendTroop, true, body);
        }


    }
}
