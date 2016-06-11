using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class action
    {
        public const string PROMPT_NOT_ENOUGH = "ERR_NOT_ENOUGH";
        public const string PROMPT_NOT_SUPPORT = "ERR_COMMON_NOT_SUPPORTED";
        public const string PROMPT_RELOGIN = "ERR_COMMON_RELOGIN";

        public const string PROMPT_ACTIVITY_NOT_OPEN = "ACTIVITY_IS_NOT_OPEN";
        public const string PROMPT_GOT_REWARD_ALREADY = "GOT_REWARD_ALREADY";

        public const string PROMPT_RESOURCE_FULL = "RESOURCE_FULL";

        public static class RRO
        {
            public const string elite = "elite";
            public const string eliteBuyTimes = "eliteBuyTimes";
            public const string eliteCanBuyTimes = "eliteCanBuyTimes";

            public const string matchTimes = "matchTimes";

            public const string times = "times";
            public const string buyTimes = "buyTimes";
            public const string weekday = "weekday";
        }

    }

}
