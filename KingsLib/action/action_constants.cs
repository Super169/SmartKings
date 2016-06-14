using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class action
    {
        public static class STYLE
        {
            public const string ALERT = "ALERT";
        }

        public static class PROMPT
        {
            public const string ERR_NOT_ENOUGH = "ERR_NOT_ENOUGH";
            public const string ERR_COMMON_NOT_SUPPORT = "ERR_COMMON_NOT_SUPPORTED";
            public const string ERR_COMMON_RELOGIN = "ERR_COMMON_RELOGIN";

            public const string ACTIVITY_NOT_OPEN = "ACTIVITY_IS_NOT_OPEN";
            public const string GOT_REWARD_ALREADY = "GOT_REWARD_ALREADY";

            public const string RESOURCE_FULL = "RESOURCE_FULL";

            public const string ACTIVITY_CAN_NOT_PARTICIPATE = "ACTIVITY_CAN_NOT_PARTICIPATE";

        }

        public static class RRO
        {

            public static class Bag
            {
                public const string items = "items";
                public const string nm = "nm";
                public const string n = "n";

                public const string nm_ticket = "嘉年华入场券";
            }

            public static class Campaign
            {
                public const string elite = "elite";
                public const string eliteBuyTimes = "eliteBuyTimes";
                public const string eliteCanBuyTimes = "eliteCanBuyTimes";

                public const string times = "times";
                public const string buyTimes = "buyTimes";
                public const string weekday = "weekday";
            }

            public static class DianJiangTai
            {
                public const string leftTimes = "leftTimes";
            }

            public static class Hero
            {
                public const string afi = "afi";
                public const string dsp = "dsp";
                public const string heros = "heros";
                public const string matchTimes = "matchTimes";
                public const string nm = "nm";
                public const string sta = "sta";

                public const string sta_NULL = "NULL";
                public const string sta_MAKE = "MAKE";
                public const string sta_IN_ACT = "IN_ACT";
                public const string sta_STR = "STR";
            }

            public static class KingRoad
            {
                public const string enemy = "enemy";
                public const string remainChallenge = "remainChallenge";
                public const string star = "star";
                public const string seasonType = "seasonType";
                public const string seasonType_TO_KEEP = "TO_KEEP";
                public const string seasonType_GIFT = "GIFT";

            }

            public static class LongMarch
            {
                public const string leftTimes = "leftTimes";
                public const string curStation = "curStation";
            }

            public static class Naval
            {
                public const string alives = "alives";
                public const string deads = "deads";
                public const string deadhero = "deadhero";
            }

            public static class OneYear
            {
                public const string activityStatus = "activityStatus";
                public const string remainCount = "remainCount";
            }

            public static class System
            {
                public const string clientTime = "clientTime";
                public const string serverTime = "serverTime";
            }

            public static class TeamDuplicate
            {
                public const string times = "times";

            }

            public static class Travel
            {
                public const string diceNum = "diceNum";

            }

        }

    }

}
