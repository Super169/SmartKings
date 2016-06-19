
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
    public static partial class action
    {
        public static StarryInfo getStarryInfo(ConnectionInfo ci, string sid)
        {
            StarryInfo si = new StarryInfo();
            RequestReturnObject rro;

            rro = request.Starry.info(ci, sid);
            if (!rro.success) return null;

            si = StarryInfo.fromJaon(rro.responseJson);
            return si;
        }

        public static StarryInfo.ChapterInfo getChapterInfo(ConnectionInfo ci, string sid, int chapterId)
        {
            StarryInfo.ChapterInfo chapterInfo = new StarryInfo.ChapterInfo();
            RequestReturnObject rro;
            rro = request.Starry.chapterInfo(ci, sid, chapterId);
            if (!rro.success) return null;



            return chapterInfo;
        }

    }
}
