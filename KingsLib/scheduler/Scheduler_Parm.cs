using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.scheduler
{
    public partial class Scheduler
    {
        public static class Parm
        {
            public static class EliteFight
            {
                public const string targetChapter = "targetChapter";
                public const string targetStage = "targetStage";
            }

            public static class IndustryShop
            {
                public const string buyFood = "buyFood";
                public const string buySilver = "buySilver";
            }

            public static class TrainHero
            {
                public const string targetHeros = "targetHeros";
                public const string trainSameLevel = "trainSameLevel";
            }

            public static class TeamDuplicate
            {
                public const string heroIdx = "heroIdx";
            }

        }
    }
}
