using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class action
    {

        public delegate void DelegateUpdateInfo(string account, string action, string msg, bool addTime = true, bool async = true);

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
                public const string deleted = "deleted";
                public const string updated = "updated";

                public const string idx = "idx";
                public const string items = "items";
                public const string nm = "nm";
                public const string n = "n";
                public const string us = "us";

                public const string nm_ticket = "嘉年华入场券";
            }

            public static class Campaign
            {
                public const string elite = "elite";
                public const string eliteBuyTimes = "eliteBuyTimes";
                public const string eliteCanBuyTimes = "eliteCanBuyTimes";

                public const string arena = "arena";
                public const string lmarch = "lmarch";
                public const string arenas = "arenas";
                public const string starryLeftCount = "starryLeftCount";


                public const string times = "times";
                public const string buyTimes = "buyTimes";
                public const string weekday = "weekday";

                public const string heros = "heros";
                public const string index = "index";
                public const string x = "x";
                public const string y = "y";
                public const string chief = "chief";
                public const string power = "power";
                public const string costFd = "costFd";

                public const string enemies = "enemies";

            }

            public static class DianJiangTai
            {
                public const string leftTimes = "leftTimes";
            }

            public static class GrassArrow
            {
                public const string fightGold = "fightGold";
                public const string arrowNum = "arrowNum";
                public const string fightCount = "fightCount";
                public const string totalNum = "totalNum";
                public const string rewards = "rewards";
                public const string got = "got";
                public const string num = "num";
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

            public static class Manor
            {
                public const string buildings = "buildings";
                public const string field = "field";
                public const string heroIndex = "heroIndex";
                public const string leftSeconds = "leftSeconds";
                public const string level = "level";
                public const string levelSeconds = "levelSeconds";
                public const string product_out = "out";
                public const string produceSeconds = "produceSeconds";
                public const string products = "products";
                public const string type = "type";

                public const string MH = "MH";
                public const string SP = "SP";

                public const string NT = "NT";
                public const string MC = "MC";

                public const string LTC = "LTC";

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

            public static class Player
            {
                public const string ARENA_COIN = "ARENA_COIN";
                public const string CONTRIBUTION = "CONTRIBUTION";
                public const string CORPS_NAME = "CORPS_NAME";
                public const string CSKING_COIN = "CSKING_COIN";
                public const string EXP = "EXP";
                public const string EXPLOIT = "EXPLOIT";
                public const string FIGHTING_SPIRIT = "FIGHTING_SPIRIT";
                public const string FOOD = "FOOD";
                public const string GOLD = "GOLD";
                public const string GOLD_TICKET = "GOLD_TICKET";
                public const string ICON = "ICON";
                public const string IRON = "IRON";
                public const string LEVEL = "LEVEL";
                public const string LEVEL_UP_EXP = "LEVEL_UP_EXP";
                public const string LONGMARCH_COIN = "LONGMARCH_COIN";
                public const string MAX_FOOD = "MAX_FOOD";
                public const string MAX_IRON = "MAX_IRON";
                public const string MAX_SILVER = "MAX_SILVER";
                public const string PLATFORM_MARK = "PLATFORM_MARK";
                public const string p = "p";
                public const string pvs = "pvs";
                public const string SILVER = "SILVER";
                public const string UNDERGO_EXP = "UNDERGO_EXP";
                public const string v = "v";
                public const string VIP_LEVEL = "VIP_LEVEL";
                public const string XIYU_COIN = "XIYU_COIN";
            }

            public static class Starry
            {
                public const string allCountBuyGold = "allCountBuyGold";
                public const string leftAllCount = "leftAllCount";
                public const string buyCount = "buyCount";

                public const string list = "list";
                public const string chapterId = "chapterId";
                public const string chapterReward = "chapterReward";
                public const string starReward = "starReward";
                public const string barrierList = "barrierList";

                public const string star = "star";
                public const string barrierStep = "barrierStep";
                public const string barrierId = "barrierId";
                public const string leftCount = "leftCount";
                public const string gold = "gold";
                public const string boughtCount = "boughtCount";

                public const string data = "data";
                public const string _type = "_type";
                public const string _rs = "_rs";

                public const string _type_SCEnterCampaign = "SCEnterCampaign";

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


            public static class WuFuLinMen
            {
                public const string drawDatas = "drawDatas";
                public const string stage = "stage";
                public const string rewards = "rewards";
                public const string reward = "reward";
                public const string resources = "resources";
                public const string quality = "quality";
                public const string drawStatus = "drawStatus";
                public const string type = "type";
                public const string index = "index";
                public const string type_EVERYDAY_LOGIN = "EVERYDAY_LOGIN";
                public const string todayCharge = "todayCharge";

            }
        }

    }

}
