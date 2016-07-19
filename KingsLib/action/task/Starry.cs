using KingsLib.data;
using KingsLib.scheduler;
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
        public static partial class task
        {
            private static int[,] bookTarget = { { 10, 5 }, { 22, 5 } };

            public static bool goStarryBook(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.StarryBook;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                StarryInfo si = action.starry.getInfo(ci, sid);
                if (si.leftAllCount <= 0) return true;

                int leftCount = si.leftAllCount;
                int i = 0;
                while ((leftCount > 0) && (i < 2))
                {
                    int chapterId = bookTarget[i, 0];
                    int barrierStep = bookTarget[i, 1];
                    i++;
                    StarryInfo.ChapterInfo currChapter = action.starry.getChapterInfo(ci, sid, chapterId);
                    if (currChapter == null) continue;
                    StarryInfo.ChapterInfo.BarrierInfo bi = currChapter.barrierList.Find(x => x.barrierStep == barrierStep);
                    if (bi == null) continue;
                    if (bi.leftCount <= 0) continue;
                    int fightCount = (bi.leftCount < 3 ? bi.leftCount : 3);
                    fightCount = (leftCount < fightCount ? leftCount : fightCount);
                    rro = request.Starry.batchStarry(ci, sid, bi.barrierId, fightCount);
                    if (!rro.SuccessWithJson(RRO.Starry.rewards, typeof(DynamicJsonArray)))
                    {
                        LOG.E(oGA.displayName, taskName, string.Format("Starry.batchStarry: {0} \n-ERR->\n{1}", rro.requestText, rro.requestText));
                        continue;
                    }
                    updateInfo(oGA.displayName, taskName, string.Format("掃蕩 {0} 次:  {1} - {2}", fightCount, chapterId, barrierStep));
                    leftCount -= fightCount;
                }
                return true;
            }

            public static bool goStarryFight(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskId = Scheduler.TaskId.StarryFight;
                string taskName = Scheduler.getTaskName(taskId);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                if (debug) action.showDebugMsg(updateInfo, oGA.displayName, taskName, "通關任務 - 開始");

                StarryInfo si = action.starry.getInfo(ci, sid);

                if (si == null) return false;

                int currChapterId = si.chapterList.Last().chapterId;
                int currBarrierId = si.chapterList.Last().barrierList.Last().barrierId;
                int lastBarrierId = -1;

                WarInfo wi = oGA.getWarInfo(Scheduler.TaskId.StarryFight, 0);
                string fightHeros = null;
                if (wi != null) fightHeros = wi.body;

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

                    int fightResult = action.starry.fight(ci, sid, currBarrierId, ref fightHeros);
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("作戰陣形 {0} ", fightHeros));

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
                    }
                    else
                    {
                        updateInfo(oGA.displayName, taskName, string.Format("餘下 {0} 次, Chapter: {1}:{2}, Barrier {3}", si.leftAllCount, currChapterId, si.chapterList.Last().barrierList.Count, currBarrierId));
                    }
                }
                if (debug) action.showDebugMsg(updateInfo, oGA.displayName, taskName, "通關任務 - 結束");

                return true;
            }

            public static bool goStarryReward(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.StarryReward);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                if (debug) action.showDebugMsg(updateInfo, oGA.displayName, taskName, "領取獎勵 - 開始");

                StarryInfo si = action.starry.getInfo(ci, sid);
                if (si == null) return false;

                foreach (StarryInfo.ChapterInfo chapter in si.chapterList)
                {
                    if (chapter.chapterReward == 1)
                    {
                        rro = request.Starry.chapterReward(ci, sid, chapter.chapterId);
                        if (rro.ok == 1)
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("領取 Chapter {0} 的獎勵", chapter.chapterId));
                        }
                    }
                    if (chapter.starReward < 7)
                    {
                        int starCnt = 0;
                        foreach (StarryInfo.ChapterInfo.BarrierInfo bi in chapter.barrierList)
                        {
                            starCnt += bi.star;
                        }
                        for (int step = 1; step <= 3; step++)
                        {
                            if ((starCnt >= (5 * step)) && ((chapter.starReward & (1 << (step - 1))) == 0))
                            {
                                rro = request.Starry.starReward(ci, sid, chapter.chapterId, step);
                                if (rro.ok == 1)
                                {
                                    updateInfo(oGA.displayName, taskName, string.Format("領取 Chapter {0}, step {1} 的獎勵", chapter.chapterId, step));
                                }

                            }
                        }
                    }
                }

                if (debug) action.showDebugMsg(updateInfo, oGA.displayName, taskName, "領取獎勵 - 結束");
                return true;
            }
        }
    }

}