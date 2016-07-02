using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib
{
    public partial class util
    {

        private static string[] eliteChapter = { "天下無雙", "西涼錦騎", "四世三公", "混世魔王", "荊襄義士", "江東霸王", "力挽漢室", "亂世梟雄", "西蜀忠良", "帝王之志", "南彊惡虎" };
        private static string[,] eliteHeros =
       {{"張遼","貂蟬","呂綺紅","陳宮","呂布"},
            {"馬岱","龐德","馬雲祿","馬超","馬騰"},
            {"高覽","顏良","文醜","張郃","袁紹"},
            {"李儒","華雄","張遼","呂布","董卓"},
            {"龐統","徐庶","魏延","黃忠","劉表"},
            {"甘寧","太史慈","孫權","孫策","孫堅"},
            {"姜維","趙雲","張飛","關羽","劉備"},
            {"夏候惇","夏候淵","訐褚","典韋","曹操"},
            {"張松","孟達","嚴顏","張任","劉璋"},
            {"雷薄","張勳","橋蕤","紀靈","袁術"},
            {"木鹿大王","帶來洞主","兀突骨","祝融","孟獲"}
        };
        private static string[,] eliteRewards =
        {{"綠旗鼓號x200, 綠盾x200","綠刀劍x200, 綠鞋x200","綠槍x200, 綠馬x200","綠錘x200, 綠輕甲x200","綠弓弩x200, 綠重甲x200"},
            {"綠旗鼓號x300, 綠盾x300","綠刀劍x300, 綠鞋x300","綠槍x300, 綠馬x300","綠錘x300, 綠輕甲x300","綠弓弩x300, 綠重甲x300"},
            {"藍旗鼓號x150, 藍盾x150","藍刀劍x150, 藍鞋x150","藍槍x150, 藍馬x150","藍錘x150, 藍輕甲x150","藍弓弩x150, 藍重甲x150"},
            {"藍旗鼓號x250, 藍盾x250","藍刀劍x250, 藍鞋x250","藍槍x250, 藍馬x250","藍錘x250, 藍輕甲x250","藍弓弩x250, 藍重甲x250"},
            {"紫旗鼓號x150, 紫盾x150","紫刀劍x150, 紫鞋x150","紫槍x150, 紫馬x150","紫錘x150, 紫輕甲x150","紫弓弩x150, 紫重甲x150"},
            {"紫旗鼓號x250, 紫盾x250","紫刀劍x250, 紫鞋x250","紫槍x250, 紫馬x250","紫錘x250, 紫輕甲x250","紫弓弩x250, 紫重甲x250"},
            {"紫旗鼓號x300, 紫盾x300","紫刀劍x300, 紫鞋x300","紫槍x300, 紫馬x300","紫錘x300, 紫輕甲x300","紫弓弩x300, 紫重甲x300"},
            {"金旗鼓號x200, 金盾x200","金刀劍x200, 金鞋x200","金槍x200, 金馬x200","金錘x200, 金輕甲x200","金弓弩x200, 金重甲x200"},
            {"金旗鼓號x300, 金盾x300","金刀劍x300, 金鞋x300","金槍x300, 金馬x300","金錘x300, 金輕甲x300","金弓弩x300, 金重甲x300"},
            {"金旗鼓號x400, 金盾x400","金刀劍x400, 金鞋x400","金槍x400, 金馬x400","金錘x400, 金輕甲x400","金弓弩x400, 金重甲x400"},
            {"紅旗鼓號x300, 紅盾x300","紅刀劍x300, 紅鞋x300","紅槍x300, 紅馬x300","紅錘x300, 紅輕甲x300","紅弓弩x300, 紅重甲x300"}
        };

        public static int maxChapter()
        {
            return eliteChapter.Length;
        }

        public static string getEliteChapterName(int chapter)
        {
            string chapterName = "";
            try
            {
                chapterName = eliteChapter[chapter - 1];
            }
            catch
            {
                chapterName = "??????";
            }
            return chapterName;
        }


        public static string getEliteHeroName(int chapter, int stage)
        {
            string heroName;
            try
            {
                heroName = eliteHeros[chapter - 1, stage - 1];
            }
            catch
            {
                heroName = "??????";
            }
            return heroName;
        }

        public static string getEliteReward(int chapter, int stage)
        {
            string elterReward;
            try
            {
                elterReward = eliteRewards[chapter - 1, stage - 1];
            }
            catch
            {
                elterReward = "??????";
            }
            return elterReward;
        }



    }
}


