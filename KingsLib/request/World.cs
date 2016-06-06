﻿
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class World
    {
        private const string CMD_citySituationDetail = "World.citySituationDetail";
        private const string CMD_getAllOpenedCities = "World.getAllOpenedCities";
        private const string CMD_getAllTransportingUnits = "World.getAllTransportingUnits";
        private const string CMD_getCityChapterBlueprint = "World.getCityChapterBlueprint";
        private const string CMD_getCityRewardInfo = "World.getCityRewardInfo";
        private const string CMD_getExploredWorldArea = "World.getExploredWorldArea";
        private const string CMD_go = "World.go";
        private const string CMD_worldSituation = "World.worldSituation";

        public static RequestReturnObject getAllOpenedCities(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getAllOpenedCities);
        }

        public static RequestReturnObject getAllTransportingUnits(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getAllTransportingUnits);
        }

        public static RequestReturnObject getCityChapterBlueprint(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getCityChapterBlueprint);
        }

        public static RequestReturnObject getCityRewardInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getCityRewardInfo);
        }

        public static RequestReturnObject getExploredWorldArea(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getExploredWorldArea);
        }


    }
}
