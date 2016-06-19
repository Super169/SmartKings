using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class action
    {
        public static partial class task
        {
            public static bool goTaskStarry(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = "攬星壇";
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                StarryInfo si = action.getStarryInfo(ci, sid);

                if (si == null) return false;

                int chapterCnt = si.chapterList.Count;
                StarryInfo.ChapterInfo currChapter = si.chapterList.ElementAt(chapterCnt - 1);
                int currChapterId = currChapter.chapterId;

                int barrierCnt = currChapter.barrierList.Count;
                StarryInfo.ChapterInfo.BarrierInfo currBarrier = currChapter.barrierList.ElementAt(barrierCnt - 1);
                int currBarrierId = currBarrier.barrierId;

                updateInfo(oGA.displayName, taskName, string.Format("餘下 {0} 次, Chapter: {1}:{2}, Barrier {3}", si.leftAllCount, currChapterId, barrierCnt, currBarrierId));

                if (si.leftAllCount > 0)
                {
                    rro = request.Starry.chapterInfo(ci, sid, currChapterId);

                }


                return true;
            }
        }
    }

}