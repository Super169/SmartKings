using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using static KingsLib.action;

namespace KingsLib.data
{
    public class StarryInfo
    {
        public class ChapterInfo
        {
            public class BarrierInfo
            {
                public int star { get; set; }
                public int barrierStep { get; set; }
                public int barrierId { get; set; }
                public int leftCount { get; set; }
                public int gold { get; set; }
                public int boughtCount { get; set; }

                public static BarrierInfo fromJson(dynamic json)
                {
                    if (!(JSON.exists(json, RRO.Starry.star) &&
                          JSON.exists(json, RRO.Starry.barrierStep) &&
                          JSON.exists(json, RRO.Starry.barrierId) &&
                          JSON.exists(json, RRO.Starry.leftCount) &&
                          JSON.exists(json, RRO.Starry.gold) &&
                          JSON.exists(json, RRO.Starry.boughtCount))) return null;
                    StarryInfo.ChapterInfo.BarrierInfo barrier = new StarryInfo.ChapterInfo.BarrierInfo();
                    barrier.star = JSON.getInt(json, RRO.Starry.star);
                    barrier.barrierStep = JSON.getInt(json, RRO.Starry.barrierStep);
                    barrier.barrierId = JSON.getInt(json, RRO.Starry.barrierId);
                    barrier.leftCount = JSON.getInt(json, RRO.Starry.leftCount);
                    barrier.gold = JSON.getInt(json, RRO.Starry.gold);
                    barrier.boughtCount = JSON.getInt(json, RRO.Starry.boughtCount);
                    return barrier;
                }

            }

            public static ChapterInfo fromJson(dynamic json)
            {
                if (!(JSON.exists(json, RRO.Starry.barrierList, typeof(DynamicJsonArray)))) return null;
                if (!(JSON.exists(json, RRO.Starry.chapterId) &&
                      JSON.exists(json, RRO.Starry.chapterReward) &&
                      JSON.exists(json, RRO.Starry.starReward))) return null;
                StarryInfo.ChapterInfo chapter = new StarryInfo.ChapterInfo();
                chapter.chapterId = JSON.getInt(json, RRO.Starry.chapterId);
                chapter.chapterReward = JSON.getInt(json, RRO.Starry.chapterReward);
                chapter.starReward = JSON.getInt(json, RRO.Starry.starReward);

                chapter.barrierList = new List<StarryInfo.ChapterInfo.BarrierInfo>();
                DynamicJsonArray barrierList = json[RRO.Starry.barrierList];

                foreach (dynamic oBarrier in barrierList)
                {
                    BarrierInfo barrier = BarrierInfo.fromJson(oBarrier);
                    if (barrier == null) return null;
                    chapter.barrierList.Add(barrier);
                }
                return chapter;
            }

            public int chapterId { get; set; }
            public int chapterReward { get; set; }
            public int starReward { get; set; }
            public List<BarrierInfo> barrierList;
        }
        public int allCountBuyGold { get; set; }
        public int leftAllCount { get; set; }
        public int buyCount { get; set; }
        public List<ChapterInfo> chapterList;

        public static StarryInfo fromJaon(dynamic json)
        {
            StarryInfo si = new StarryInfo();
            if (!(JSON.exists(json, RRO.Starry.list, typeof(DynamicJsonArray)))) return null;

            if (!(JSON.exists(json, RRO.Starry.allCountBuyGold) && JSON.exists(json, RRO.Starry.leftAllCount) && JSON.exists(json, RRO.Starry.buyCount))) return null;
            si.allCountBuyGold = JSON.getInt(json, RRO.Starry.allCountBuyGold);
            si.leftAllCount = JSON.getInt(json, RRO.Starry.leftAllCount);
            si.buyCount = JSON.getInt(json, RRO.Starry.buyCount);

            si.chapterList = new List<StarryInfo.ChapterInfo>();
            DynamicJsonArray chapterList = json[RRO.Starry.list];
            foreach (dynamic oChapter in chapterList)
            {
                StarryInfo.ChapterInfo chapter = StarryInfo.ChapterInfo.fromJson(oChapter);
                if (chapter == null) return null;
                si.chapterList.Add(chapter);
            }
            return si;
        }

    }
}
