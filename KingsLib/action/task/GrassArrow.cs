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
            public static bool goGrassArrow(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = Scheduler.getTaskName(Scheduler.TaskId.GrassArrow);
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;

                rro = request.GrassArrow.acquireGrassArrowInfo(ci, sid);
                if (!(rro.SuccessWithJson(RRO.GrassArrow.items, typeof(DynamicJsonArray)) &&
                      rro.SuccessWithJson(RRO.GrassArrow.rewards, typeof(DynamicJsonArray))))
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "GrassArrow.acquireGrassArrowInfo 出錯");
                    return false;
                }
                int fightCount = rro.getInt(RRO.GrassArrow.fightCount);
                int arrowNum = rro.getInt(RRO.GrassArrow.arrowNum);
                int totalNum = rro.getInt(RRO.GrassArrow.totalNum);
                if (fightCount > 0)
                {
                    WarInfo wi = oGA.getWarInfo(Scheduler.TaskId.GrassArrow, 0);
                    string fightHeros = null;
                    if (wi != null) fightHeros = wi.body;

                    while (fightCount > 0)
                    {
                        int fightResult = action.GrassArrow.goGrassArrowFight(ci, sid, fightHeros);
                        if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("作戰陣形 {0} ", fightHeros));
                        switch (fightResult)
                        {
                            case -1:
                                updateInfo(oGA.displayName, taskName, "出戰失敗");
                                fightCount = -1;
                                break;
                            case 1:
                                updateInfo(oGA.displayName, taskName, "尚未完成佈陣");
                                fightCount = -1;
                                break;
                            case 0:
                                fightCount--;
                                break;
                        }
                    }
                    rro = request.GrassArrow.acquireGrassArrowInfo(ci, sid);
                    if (!(rro.SuccessWithJson(RRO.GrassArrow.items, typeof(DynamicJsonArray)) &&
                          rro.SuccessWithJson(RRO.GrassArrow.rewards, typeof(DynamicJsonArray))))
                    {
                        if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, "GrassArrow.acquireGrassArrowInfo 出錯");
                        return false;
                    }
                    fightCount = rro.getInt(RRO.GrassArrow.fightCount);
                    arrowNum = rro.getInt(RRO.GrassArrow.arrowNum);
                    totalNum = rro.getInt(RRO.GrassArrow.totalNum);
                    updateInfo(oGA.displayName, taskName, string.Format("出戰後, 獲得 {0} 支箭, 現餘下 {1} 支箭", totalNum, arrowNum));
                }
                else
                {
                    if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("早前已經完成了, 獲得 {0} 支箭, 現餘下 {1} 支箭", totalNum, arrowNum));
                }

                DynamicJsonArray items = rro.responseJson[RRO.GrassArrow.items];
                DynamicJsonArray rewards = rro.responseJson[RRO.GrassArrow.rewards];

                foreach (dynamic reward in rewards)
                {
                    bool got = JSON.getBool(reward, RRO.GrassArrow.got, true);
                    if (!got)
                    {
                        int num = JSON.getInt(reward, RRO.GrassArrow.num, 99999);
                        if (totalNum >= num)
                        {
                            int id = JSON.getInt(reward, RRO.GrassArrow.id);
                            if (id > 0)
                            {
                                rro = request.GrassArrow.drawStageReward(ci, sid, id);
                                updateInfo(oGA.displayName, taskName, string.Format("領取 {0} 號寶箱獎勵{1}", id, (rro.ok == 1 ? "" : "失敗")));
                            }
                        }
                    }
                }

                int minItemCode = 999;
                foreach (dynamic item in items )
                {
                    int itemCode = JSON.getInt(item, RRO.GrassArrow.id, 999);
                    if (itemCode < minItemCode) minItemCode = itemCode;
                }

                if ((minItemCode < 7) || (minItemCode > 10))
                {
                    LOG.I(string.Format("{0}: 草船不會換領獎品: {1}", oGA.displayName, JSON.encode(items)));
                    return true;
                }

                // now, only handle for higher level only, need to check if it can get the cost of items
                switch (minItemCode)
                {
                    case 7:
                        goExchange(oGA, updateInfo, debug, taskName, ref arrowNum, 8, 200, "銀兩", 10000);
                        goExchange(oGA, updateInfo, debug, taskName, ref arrowNum, 7, 160, "高級經驗書", 1);
                        break;
                    case 10:
                        goExchange(oGA, updateInfo, debug, taskName, ref arrowNum, 11, 400, "銀兩", 20000);
                        goExchange(oGA, updateInfo, debug, taskName, ref arrowNum, 10, 160, "高級經驗書", 1);
                        break;
                }
                if (debug) showDebugMsg(updateInfo, oGA.displayName, taskName, string.Format("換領完畢, 餘下 {0} 支箭", arrowNum));
                return true;
            }


            private static void goExchange(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug, string taskName, ref int arrowNum, int itemId, int itemCost, string itemName, int num)
            {
                RequestReturnObject rro;
                int failCnt = 0;
                while (arrowNum >= itemCost)
                {
                    rro = request.GrassArrow.exchangeGrassArrow(oGA.connectionInfo, oGA.sid, itemId);
                    if (rro.ok == 1)
                    {
                        arrowNum -= itemCost;
                        updateInfo(oGA.displayName, taskName, string.Format("用 {0} 箭換領 {1} x {2}, 餘下 {3} 箭", itemCost, itemName, num, arrowNum));
                    }
                    else
                    {
                        failCnt++;
                        updateInfo(oGA.displayName, taskName, string.Format("換領 {0} 失敗 {1} 次, 餘下 {2} 箭", itemName, failCnt, arrowNum));
                        if (failCnt >= 3) break;
                    }
                }
            }
        }


    }
}
