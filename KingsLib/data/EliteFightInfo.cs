using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.data
{
    public class EliteFightInfo
    {
        public bool selected { get; set; }
        public int chapter { get; set; }
        public int stage { get; set; }
        public int chapterColorCode { get { return (this.chapter % 2); } }
        public string chapterName { get { return util.getEliteChapterName(this.chapter); } }
        public string heroName { get { return util.getEliteHeroName(this.chapter, this.stage); } }
        public string reward { get { return util.getEliteReward(this.chapter, this.stage); } }

        public EliteFightInfo(int chapter, int stage)
        {
            this.selected = false;
            this.chapter = chapter;
            this.stage = stage;
        }
    }
}
