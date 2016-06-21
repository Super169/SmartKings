using KingsLib.data;
using KingsLib.scheduler;
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
            public static bool goCheckStarry(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = "攬星壇";
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                StarryInfo si = action.starry.getInfo(ci, sid);

                if (si == null) return false;

                int currChapterId = si.chapterList.Last().chapterId;
                int currBarrierId = si.chapterList.Last().barrierList.Last().barrierId;
                int lastBarrierId = -1;


                string fightHeros = oGA.getTaskParameter(Scheduler.TaskId.Starry);

                updateInfo(oGA.displayName, taskName, string.Format("餘下 {0} 次, Chapter: {1}:{2}, Barrier {3}", si.leftAllCount, currChapterId, si.chapterList.Last().barrierList.Count, currBarrierId));

                while (si.leftAllCount > 0)
                {

                    // Just follow the step to call chapterInfo, but information alreay collected
                    rro = request.Starry.chapterInfo(ci, sid, currChapterId);

                    StarryInfo.ChapterInfo currChapter = action.starry.getChapterInfo(ci, sid, currChapterId);
                    if (currChapter == null) return false;

                    currChapterId = currChapter.chapterId;
                    currBarrierId = currChapter.barrierList.Last().barrierId;

                    updateInfo(oGA.displayName, taskName, string.Format("備戰: ChapterId: {0}, Barrier {1}", currChapterId, currBarrierId));

                    int fightResult = action.starry.fight(ci, sid, currBarrierId, fightHeros);

                    if (fightResult == -1)
                    {
                        updateInfo(oGA.displayName, taskName, "出戰失敗");
                        return false;
                    }
                    else if (fightResult == 1)
                    {
                        updateInfo(oGA.displayName, taskName, "尚未完成佈陣");
                        return false;
                    }
                    updateInfo(oGA.displayName, taskName, string.Format("完成出戰: ChapterId: {0}, Barrier {1}", currChapterId, currBarrierId));

                    lastBarrierId = currBarrierId;

                    si = action.starry.getInfo(ci, sid);

                    currChapterId = si.chapterList.Last().chapterId;
                    currBarrierId = si.chapterList.Last().barrierList.Last().barrierId;

                    if (lastBarrierId == currBarrierId)
                    {
                        updateInfo(oGA.displayName, taskName, string.Format("餘下 {0} 次, Chapter: {1}:{2}, Barrier {3} - 作戰失敗", si.leftAllCount, currChapterId, si.chapterList.Last().barrierList.Count, currBarrierId));
                        // Need some action for fail?
                        return false;
                    } else
                    {
                        updateInfo(oGA.displayName, taskName, string.Format("餘下 {0} 次, Chapter: {1}:{2}, Barrier {3}", si.leftAllCount, currChapterId, si.chapterList.Last().barrierList.Count, currBarrierId));
                    }
                }
                return true;
            }
        }
    }

}