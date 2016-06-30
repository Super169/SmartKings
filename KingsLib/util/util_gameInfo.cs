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
            {{"","","","",""},
             {"","","","",""},
             {"","","","",""},
             {"","","","",""},
             {"","","","",""},
             {"","","","",""},
             {"","","","",""},
             {"","","","",""},
             {"金旗鼓號x300, 金盾x300","金刀劍x300, 金鞋x300","金槍x300, 金馬x300","金錘x300, 金輕甲x300","金弓弩x300, 金重甲x300"},
             {"金旗鼓號x400, 金盾x400","金刀劍x400, 金鞋x400","金槍x400, 金馬x400","金錘x400, 金輕甲x400","金弓弩x400, 金重甲x400"},
             {"紅旗鼓號x300, 紅盾x300","","","",""}
        };

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

    }
}


