using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Data;
using KingsLib.data;
using KingsLib;
using KingsLib.scheduler;
using MyUtil;

namespace SmartKings
{
    public partial class MainWindow : Window
    {

        public delegate bool DelegateActionHandler(GameAccount oGA, action.DelegateUpdateInfo updateInfo, bool debug);

        private void initActionPanel()
        {
            ucActionPanel.setActionHandler(actionPanelHandler);
        }

        private void actionPanelHandler(bool allPlayers, string btnClicked)
        {
            switch (btnClicked)
            {
                case "btnCheckStatus":
                    goCheckStatus();
                    break;
                case "btnOutstanding":
                    goAction("檢查遺漏", allPlayers, action.checkAllOutstandingTasks);
                    break;
                case "btnHarvest":
                    goAction("封地收獲", allPlayers, action.task.goHarvest);
                    break;
                case "btnMonthSignIn":
                    goAction("簽到領獎", allPlayers, action.task.goSignIn);
                    break;
                case "btnCleanBag":
                    goAction("清理背包", allPlayers, action.task.goCleanupBag);
                    break;
                case "btnStarrySetup":
                    goStarrySetup();
                    break;
                case "btnStarry":
                    goAction("攬星壇", allPlayers, action.task.goCheckStarry);
                    break;
                case "btnMarket":
                    goAction("糧草先行", allPlayers, action.task.goMarket);
                    break;
                case "btnCycleShop":
                    goAction("東瀛寶船", allPlayers, action.task.goCycleShop);
                    break;
                case "btnReadEmail":
                    goAction("開啟郵件", allPlayers, action.task.goReadAllEmail);
                    break;
                case "btnFinishTask":
                    goAction("任務報酬", allPlayers, action.task.goFinishAllTask);
                    break;

            }

        }

        private void goCheckStatus()
        {
            UpdateProgress("檢查帳戶狀況 進行中......");
            if (AppSettings.DEBUG) DebugLog("帳戶狀況", "開始");
            goTaskCheckStatus();
            refreshAccountList();
            if (AppSettings.DEBUG) DebugLog("帳戶狀況", "結束");
            UpdateProgress();
        }

        private void goStarrySetup()
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;

            ui.WinWarSettings winStarry = new ui.WinWarSettings("攬星壇佈陣設定", oGA, oGA.getTaskParmObject(Scheduler.TaskId.Starry),1,5,true);
            winStarry.saveSettingHandler += new ui.WinWarSettings.DelSaveSettingHandler(this.saveStarrySertting);
            winStarry.Owner = this;
            // winStarry.init("攬星壇佈陣設定", oGA, oGA.getTaskParmObject(Scheduler.TaskId.Starry));
            bool? dialogResult = winStarry.ShowDialog();
        }

        private void saveStarrySertting(GameAccount oGA, dynamic json)
        {
            Scheduler.AutoTask autoTask = oGA.findAutoTask(Scheduler.TaskId.Starry);
            if (autoTask != null)
            {
                autoTask.parmObject = json;
                autoTask.parameter = ( json == null ? null : JSON.encode(json));
            }
        }

        private void goAction(string actionName, bool allPlayers, DelegateActionHandler actionHandler)
        {
            if (allPlayers)
            {
                Thread thread = new Thread(() => goActionThread(actionName, actionHandler, null));
                thread.Start();
            }
            else
            {
                // Must get account in UI thread then pass to background thread
                GameAccount oGA = GetSelectedAccount();
                if (oGA == null) return;
                if (oGA.IsOnline())
                {
                    Thread thread = new Thread(() => goActionThread(actionName, actionHandler, oGA));
                    thread.Start();
                }
                else
                {
                    MessageBox.Show("帳戶已在其他地方登入");
                }
            }
        }

        private void goActionThread(string actionName, DelegateActionHandler actionHandler, GameAccount oGA)
        {
            if (Monitor.TryEnter(AppSettings.actionLocker, 5000))
            {
                try
                {
                    UpdateProgress(actionName + " 進行中......");
                    if (AppSettings.DEBUG) DebugLog(actionName, "開始");
                    if (oGA == null)
                    {
                        foreach (GameAccount gameAccount in gameAccounts)
                        {
                            gameAccount.checkStatus(true);
                            if (gameAccount.IsOnline()) actionHandler(gameAccount, UpdateInfo, AppSettings.DEBUG);
                        }
                    }
                    else
                    {
                        oGA.checkStatus();
                        if (oGA.IsOnline()) actionHandler(oGA, UpdateInfo, AppSettings.DEBUG);
                    }

                    if (AppSettings.DEBUG) DebugLog(actionName, "結束");
                    UpdateProgress();
                }
                finally
                {
                    Monitor.Exit(AppSettings.actionLocker);
                }
            }
            else
            {
                UpdateInfo((oGA == null ? null : oGA.displayName), actionName, "等待超時, 未能進行");
            }
        }

        private GameAccount GetSelectedAccount()
        {
            if (gameAccounts.Count == 0)
            {
                MessageBox.Show("尚未偵測到大皇帝帳戶, 請先登入遊戲.");
                return null;
            }

            GameAccount oGA = (GameAccount)lvAccounts.SelectedItem;
            if (oGA == null) MessageBox.Show("請先選擇帳戶");
            return oGA;
        }

    }
}
