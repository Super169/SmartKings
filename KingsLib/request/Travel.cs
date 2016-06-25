
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Travel
    {
        private const string CMD_arriveStep = "Travel.arriveStep";
        private const string CMD_attack = "Travel.attack";
        private const string CMD_checkOut = "Travel.checkOut";
        private const string CMD_controlDice = "Travel.controlDice";
        private const string CMD_dice = "Travel.dice";
        private const string CMD_escape = "Travel.escape";
        private const string CMD_getMapInfo = "Travel.getMapInfo";
        private const string CMD_getStatus = "Travel.getStatus";
        private const string CMD_restartTravel = "Travel.restartTravel";
        private const string CMD_viewStep = "Travel.viewStep";


        public static RequestReturnObject arriveStep(ConnectionInfo ci, string sid, int step)
        {
            string body = string.Format("{{\"step\":{0}}}", step);
            return com.SendGenericRequest(ci, sid, CMD_arriveStep, true, body);
        }


        public static RequestReturnObject attack(ConnectionInfo ci, string sid, int step)
        {
            string body = string.Format("{{\"step\":{0}}}", step);
            return com.SendGenericRequest(ci, sid, CMD_attack, true, body);
        }

        public static RequestReturnObject checkOut(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_checkOut);
        }

        public static RequestReturnObject controlDice(ConnectionInfo ci, string sid, int num)
        {
            string body = string.Format("{{\"num\":{0}}}", num);
            return com.SendGenericRequest(ci, sid, CMD_controlDice, true, body);
        }


        public static RequestReturnObject dice(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_dice);
        }

        public static RequestReturnObject escape(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_escape);
        }

        public static RequestReturnObject getMapInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getMapInfo);
        }

        public static RequestReturnObject getStatus(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getStatus);
        }

        public static RequestReturnObject restartTravel(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_restartTravel);
        }


        public static RequestReturnObject viewStep(ConnectionInfo ci, string sid, int step)
        {
            string body = string.Format("{{\"step\":{0}}}", step);
            return com.SendGenericRequest(ci, sid, CMD_viewStep, true, body);
        }


    }
}
