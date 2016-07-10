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
            public const string ERROR = "ERROR";
        }

        public static class PROMPT
        {
            public const string ERR_DEBUG = "ERR_DEBUG";
            public const string ERR_NOT_ENOUGH = "ERR_NOT_ENOUGH";
            public const string ERR_COMMON_NOT_SUPPORT = "ERR_COMMON_NOT_SUPPORTED";
            public const string ERR_COMMON_RELOGIN = "ERR_COMMON_RELOGIN";

            public const string ACTIVITY_IS_NOT_OPEN = "ACTIVITY_IS_NOT_OPEN";
            public const string GOT_REWARD_ALREADY = "GOT_REWARD_ALREADY";
            public const string NOT_IN_ACTIVITY_TIME = "NOT_IN_ACTIVITY_TIME";

            public const string RESOURCE_FULL = "RESOURCE_FULL";

            public const string ACTIVITY_CAN_NOT_PARTICIPATE = "ACTIVITY_CAN_NOT_PARTICIPATE";

        }

        public static class RRO
        {
            public static class Activity
            {
                public const string bagInfos = "bagInfos";
                public const string rewardBags = "rewardBags";

                public const string sumSize = "sumSize";
                public const string needSize = "needSize";
                public const string isReward = "isReward";
                public const string bagId = "bagId";

            }

            public static class Arenas
            {
                public const string rank = "rank";
                public const string highest = "highest";
                public const string rwdRank = "rwdRank";
                public const string coin = "coin";
                public const string leftTimes = "leftTimes";
            }
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

            public static class BossWar
            {
                public const string troops = "troops";
                public const string sendCount = "sendCount";

                public const string bossInfo = "bossInfo";
                public const string hpp = "hpp";
                public const string nm = "nm";
                public const string lv = "lv";
            }


            public static class Campaign
            {
                public const string march_PATROL_NPC = "PATROL_NPC";
                public const string march_GRASS_ARROW = "GRASS_ARROW";
                public const string march_HUARONG_ROAD = "HUARONG_ROAD";
                public const string march_TRIALS = "TRIALS";

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

                public const string data = "data";
                public const string state = "state";
                public const string state_PREPARE = "PREPARE";

                public const string _type = "_type";
                public const string _rs = "_rs";

                public const string difficult_normal = "普通";

                // - code for error
                public const string args = "args";
                public const string args_NOT_ACTIVITY = "本日没有开放";
                public const string args_LEVEL_TOO_LOW = "等级不足，无法进入";




            }

            public static class Circle
            {
                public const string left = "left";
                public const string step = "step";
                public const string reset = "reset";
                public const string refresh = "refresh";
                public const string rewarded = "rewarded";
                public const string status = "status";
                public const string status_INIT = "INIT";
                public const string status_REWARED = "REWARED";

                public const string specInfo = "specInfo";
                public const string heroStg = "heroStg";
                public const string targets = "targets";
            }


            public static class City
            {
                public const string items = "items";
                public const string config = "config";
                public const string discount = "discount";
                public const string sold = "sold";
            }

            public static class Corps
            {
                public const string corpsCityReward = "corpsCityReward";
                public const string cityNum = "cityNum";
                public const string corpsFund = "corpsFund";
                public const string chestMap = "chestMap";
                public const string personalReward = "personalReward";
                public const string items = "items";
                public const string city = "city";
                public const string industryId = "industryId";
                public const string type = "type";
                public const string reward = "reward";
                public const string resource = "resource";
                public const string amount = "amount";
                public const string tips = "tips";



            }

            public static class DianJiangTai
            {
                public const string leftTimes = "leftTimes";
            }

            public static class EightTrigrams
            {
                public const string isIn = "isIn";
                public const string leftTimes = "leftTimes";
                public const string resetGold = "resetGold";
            }


            public static class Email
            {
                public const string emails = "emails";

                public const string status = "status";
                public const string status_NR = "NR";
                public const string id = "id";
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
                public const string items = "items";
                public const string id = "id";
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

                public const string commonLeftSeconds = "commonLeftSeconds";
                public const string goldLeftSeconds = "goldLeftSeconds";
                public const string discountTimes = "discountTimes";
                public const string goldOneTms = "goldOneTms";
                public const string type_COMMON_FIVE = "COMMON_FIVE";
                public const string type_GOLD_ONE = "GOLD_ONE";
                public const string discount = "discount";

                public const string nxtTm = "nxtTm";
                public const string wineConsume = "wineConsume";
                public const string wineTms = "wineTms";
                public const string items = "items";
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

            public static class Login
            {
                public const string account = "account";
                public const string serverTitle = "serverTitle";
                public const string nickName = "nickName";
            }


            public static class Lottery
            {
                public const string freeLotteryTimes = "freeLotteryTimes";
            }


            public static class LuckyCycle
            {
                public const string remainCount = "remainCount";
                public const string id = "id";
                public const string progress = "progress";
                public const string needRefresh = "needRefresh";
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

                public const string type_OFFICE = "OFFICE"; // 官邸
                public const string type_YK = "YK"; // 銀庫
                public const string type_LC = "LC"; // 糧倉
                public const string type_JTK = "JTK"; // 精鐵庫

                public const string type_GJF = "GJF"; // 工匠坊

                public const string type_MH = "MH"; // 民戶
                public const string type_SP = "SP"; // 商鋪

                public const string type_NT = "NT"; // 農田
                public const string type_MC = "MC"; // 牧場

                public const string type_LTC = "LTC"; // 鍊鐵廠

                public const string type_JC = "JC";  // 校場

                public const string trainMinute = "trainMinute";
                public const string gainExp = "gainExp";
                public const string totalTimes = "totalTimes";
                public const string usedTimes = "usedTimes";

                public const string times = "times";


            }

            public static class MonthSignIn
            {
                public const string today = "today";
                public const string msItems = "msItems";
                public const string day = "day";
                public const string st = "st";
            }


            public static class Naval
            {
                public const string alives = "alives";
                public const string deads = "deads";
                public const string deadhero = "deadhero";

                public const string disp = "disp";
                public const string troops = "troops";

            }

            public static class OneYear
            {
                public const string activityStatus = "activityStatus";
                public const string remainCount = "remainCount";
                public const string startTime = "startTime";
                public const string endTime = "endTime";
            }

            public static class OneYearEntry
            {
                public const string entryLists = "entryLists";
                public const string type = "type";
                public const string KEY_SignInType = "登陆";

                public const string isDraw = "isDraw";
                public const string reward = "reward";
                public const string itemDefs = "itemDefs";
                public const string name = "name";
                public const string num = "num";
            }

            public static class Patrol
            {
                public const string jf = "jf";
                public const string rewarded = "rewarded";
                public const string events = "events";
                public const string nextTm = "nextTm";
                public const string type = "type";
                public const string id = "id";
                public const string idx = "idx";
                public const string city = "city";
                public const string sec = "sec";
                public const string target = "target";
                public const string data = "data";
                public const string title = "title";
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

            public static class SevenDaysPoints
            {
                public const string needRefresh = "needRefresh";
            }

            public static class Shop
            {
                public const string items = "items";
                public const string pos = "pos";
                public const string res = "res";
                public const string nm = "nm";
                public const string amount = "amount";
                public const string sold = "sold";
            }

            public static class Shop2
            {
                public const string TYPE_SL_SHOP = "SL_SHOP";
                public const string remainBuyCount = "remainBuyCount";
                public const string pos = "pos";
                public const string res = "res";
                public const string nm = "nm";
                public const string amount = "amount";
                public const string sold = "sold";
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

            public static class Task
            {
                public const string tasks = "tasks";
                public const string id = "id";
                public const string taskId = "taskId";
                public const string status = "status";
                public const string status_ACC = "ACC";
                public const string status_FIN = "FIN";
            }


            public static class TeamDuplicate
            {
                public const string times = "times";
                public const string refreshTimes = "refreshTimes";
                public const string items = "items";
                public const string duplicateId = "duplicateId";
                public const string completed = "completed";
                public const string doubleReward = "doubleReward";
                public const string teamId = "teamId";
                public const string status = "status";
            }

            public static class Travel
            {

                public const string isIn = "isIn";
                public const string canPlayTimes = "canPlayTimes";
                public const string simpleMap = "simpleMap";
                public const string playerLevel = "playerLevel";
                public const string currStep = "currStep";
                public const string remainSteps = "remainSteps";
                public const string diceNum = "diceNum";
                public const string nextDicePrice = "nextDicePrice";
                public const string chipsNum = "chipsNum";
                public const string avaBuyDice = "avaBuyDice";
                public const string total = "total";
                public const string num1 = "num1";
                public const string num2 = "num2";
                public const string nextStep = "nextStep";
                public const string step = "step";
                public const string stepType = "stepType";
                public const string reward = "reward";
                public const string itemDefs = "itemDefs";
                public const string name = "name";

                public const string MAP_ZHANDOU = "ZHANDOU";
                public const string MAP_KONG = "KONG";
                public const string MAP_BAOXIANG = "BAOXIANG";
                public const string MAP_FANPAI = "FANPAI";
                public const string MAP_SHANGDIAN = "SHANGDIAN";
            }

            public static class TurnCardReward
            {
                public const string rewards = "rewards";
                public const string itemDefs = "itemDefs";
                public const string resources = "resources";
                public const string name = "name";
                public const string num = "num";
                public const string turnCardMode_ONE = "ONE";
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

            public static class Yiling
            {
                public const string canBattleTimes = "canBattleTimes";
                public const string boughtTimes = "boughtTimes";
                public const string isInMap = "isInMap";
                public const string level = "level";
                public const string step = "step";
                public const string rewardStep = "rewardStep";
                public const string resetFailureTimes = "resetFailureTimes";
                public const string deadHers = "deadHers";

            }

        }

    }

}
