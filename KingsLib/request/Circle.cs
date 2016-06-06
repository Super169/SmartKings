﻿using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Circle
    {
        private const string CMD_challenge = "Circle.challenge";
        private const string CMD_drawPassRewards = "Circle.drawPassRewards";
        private const string CMD_getHuarongRoadInfo = "Circle.getHuarongRoadInfo";
        private const string CMD_restartHuarongRoad = "Circle.restartHuarongRoad";
        private const string CMD_turnOverHeroCard = "Circle.turnOverHeroCard";

        public static RequestReturnObject challenge(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_challenge);
        }

        public static RequestReturnObject drawPassRewards(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_drawPassRewards);
        }

        public static RequestReturnObject getHuarongRoadInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getHuarongRoadInfo);
        }

        public static RequestReturnObject restartHuarongRoad(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_restartHuarongRoad);
        }


    }
}
