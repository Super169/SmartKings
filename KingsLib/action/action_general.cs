
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
        public delegate void DelegateUpdateInfo(string account, string action, string msg, bool addTime = true, bool async = true);

        public static bool checkOutstandingTask(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug = false)
        {
            RequestReturnObject rro;
            ConnectionInfo ci = oGA.connectionInfo;
            string sid = oGA.sid;
            string action = "遺漏任務";

            if (debug) updateInfo(oGA.displayName, action, "開始", true, false);

            // Check 討伐群雄
            rro = request.Campaign.getLeftTimes(ci, sid);
            if (rro.SuccessWithJson(RRO.elite) && rro.Exists(RRO.eliteBuyTimes) && rro.Exists(RRO.eliteCanBuyTimes))
            {
                int elite = JSON.getInt(rro.responseJson, RRO.elite, -1);
                int eliteBuyTimes = JSON.getInt(rro.responseJson, RRO.eliteBuyTimes, -1);
                int eliteCanBuyTimes = JSON.getInt(rro.responseJson, RRO.eliteCanBuyTimes, -1);
                if ((elite > 0) || ((eliteCanBuyTimes > 0) && (eliteBuyTimes == 0)))
                {
                    string msg = "討伐群雄: ";
                    if (elite > 0) msg += string.Format("尚餘 {0} 次 ", elite);
                    if ((eliteCanBuyTimes > 0) && (eliteBuyTimes == 0)) msg += "尚未購買額外次數";
                    updateInfo(oGA.displayName, action, msg, true, false);
                }
            } else
            {
                updateInfo(oGA.displayName, action, "討伐群雄: 檢查失敗", true, false);
            }


            // Check 切磋 - just use a heroName for chekcing
            rro = request.Hero.getVisitHeroInfo(ci, sid, "徐晃");
            if (rro.SuccessWithJson(RRO.matchTimes))
            {
                int matchTimes = JSON.getInt(rro.responseJson, RRO.matchTimes, -1);
                if (matchTimes > 0)
                {
                    string msg = string.Format("英雄切磋: 尚有 {0} 次未完成", matchTimes);
                    updateInfo(oGA.displayName, action, msg, true, false);
                }
            } else
            {
                updateInfo(oGA.displayName, action, "英雄切磋: 檢查失敗", true, false);
            }

            // 英雄試練
            rro = request.Campaign.getTrialsInfo(ci, sid);
            if (rro.SuccessWithJson(RRO.weekday) && rro.Exists(RRO.times) && rro.Exists(RRO.buyTimes))
            {
                int weekday = JSON.getInt(rro.responseJson[RRO.weekday]);
                dynamic times = rro.responseJson[RRO.times];
                dynamic buyTimes = rro.responseJson[RRO.buyTimes];
                string[] trialType = { "", "WZLJ", "WJDD", "WHSJ" };
                string[] trialName = { "", "五子良將", "五俊都督", "五虎上將" };
                string msgRemain = "";
                bool remain = false;
                string msgNotBuy = "";
                bool notBuy = false;
                for (int idx = 1; idx <= 3; idx++)
                {

                    if ((weekday == 0) || (weekday == 7) || (weekday == idx) || (weekday == idx + 3))
                    {
                        int remainTimes = JSON.getInt(times, trialType[idx]);
                        if (remainTimes > 0)
                        {
                            remain = true;
                            msgRemain += string.Format("{0} - {1} 次 ", trialName[idx], remainTimes);
                        }

                        int BuyTimes = JSON.getInt(buyTimes, trialName[idx]);
                        if ((oGA.vipLevel > (idx - 1)) && (BuyTimes == 0))
                        {
                            notBuy = true;
                            msgNotBuy += trialName[idx] + " ";
                        }

                    }

                }
                if (remain || notBuy)
                {
                    string msg = "英雄試練: ";
                    if (remain) msg += "尚餘 " + msgRemain + "未完成; ";
                    if (notBuy) msg += "尚未購買 " + msgNotBuy;
                    updateInfo(oGA.displayName, action, msg, true, false);
                }
            }
            else
            {
                updateInfo(oGA.displayName, action, "英雄試練: 檢查失敗", true, false);
            }






            if (debug) updateInfo(oGA.displayName, action, "結束", true, false);

            return true;
        }



    }
}
