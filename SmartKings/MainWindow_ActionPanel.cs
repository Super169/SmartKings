﻿using System;
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
                case "btnQuickSetup":
                    QuickSetup();
                    break;
                case "btnAutoTaskSetting":
                    ui.WinAutoTaskConfig winConfig = new ui.WinAutoTaskConfig();
                    winConfig.Owner = this;
                    winConfig.ShowDialog();
                    saveAutoTasksSettings();
                    break;
                case "btnCheckStatus":
                    goCheckStatus();
                    break;
                case "btnOutstanding":
                    goAction("檢查遺漏", allPlayers, action.checkAllOutstandingTasks);
                    break;
                case "btnHarvest":
                    goTask(Scheduler.TaskId.Harvest, allPlayers);
                    break;
                case "btnMonthSignIn":
                    goTask(Scheduler.TaskId.SignIn, allPlayers);
                    break;
                case "btnCleanBag":
                    goTask(Scheduler.TaskId.CleanUpBag, allPlayers);
                    break;
                case "btnStarrySetup":
                    // goStarrySetup();
                    goWarSetup(Scheduler.TaskId.StarryFight, 0, 1, 5, true, -1, null);
                    break;
                case "btnStarryFight":
                    goTask(Scheduler.TaskId.StarryFight, allPlayers);
                    break;
                case "btnStarryReward":
                    goTask(Scheduler.TaskId.StarryReward, allPlayers);
                    break;
                case "btnMarket":
                    goTask(Scheduler.TaskId.Market, allPlayers);
                    break;
                case "btnCycleShop":
                    goTask(Scheduler.TaskId.CycleShop, allPlayers);
                    break;
                case "btnReadEmail":
                    goTask(Scheduler.TaskId.ReadAllEmail, allPlayers);
                    break;
                case "btnFinishTask":
                    goTask(Scheduler.TaskId.FinishTask, allPlayers);
                    break;
                case "btnEliteFightSetup":
                    // goEliteFightSetup();
                    goWarSetup(Scheduler.TaskId.EliteFight, 0, 1, 5, true, -1, null);
                    break;
                case "btnEliteFight":
                    /*
                    MessageBoxResult dialogResult = MessageBox.Show("討伐群雄的對象及目標是否已設定正確", "請再三確認", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        goTask(Scheduler.TaskId.EliteFight, false);
                    }
                    */
                    goTask(Scheduler.TaskId.EliteFight, false);
                    break;
                case "btnPatrolSetup":
                    goWarSetup(Scheduler.TaskId.Patrol, 0, 1, 5, true, 3, "預留");
                    break;
                case "btnPatrolSetup2":
                    goWarSetup(Scheduler.TaskId.Patrol, 1, 1, 5, true, 3, "預留");
                    break;
                case "btnPatrol":
                    goTask(Scheduler.TaskId.Patrol, allPlayers);
                    break;
                case "btnCorpsCityReward":
                    goTask(Scheduler.TaskId.CorpsCityReward, allPlayers);
                    break;
                case "btnIndustryShop":
                    goTask(Scheduler.TaskId.IndustryShop, allPlayers);
                    break;
                case "btnTrainHero":
                    goTask(Scheduler.TaskId.TrainHero, allPlayers);
                    break;
                case "btnGrassArrowSetup":
                    goWarSetup(Scheduler.TaskId.GrassArrow, 0, 1, 3, true, 2, "諸葛亮");
                    break;
                case "btnGrassArrow":
                    goTask(Scheduler.TaskId.GrassArrow, allPlayers);
                    break;
            }
        }


        private void QuickSetup()
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;

            dynamic json;
            switch (oGA.displayName)
            {
                case "S35 超級一六九":
                    json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
                    json[Scheduler.Parm.EliteFight.targetChapter] = 10;
                    json[Scheduler.Parm.EliteFight.targetStage] = 1;
                    break;
                case "S36 無名無姓":
                    break;
                case "S37 怕死的水子遠":
                    json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
                    json[Scheduler.Parm.EliteFight.targetChapter] = 9;
                    json[Scheduler.Parm.EliteFight.targetStage] = 2;
                    break;
                case "S43 自由人":

                    json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
                    json[Scheduler.Parm.EliteFight.targetChapter] = 6;
                    json[Scheduler.Parm.EliteFight.targetStage] = 1;

                    dynamic heros = JSON.Empty;
                    List<int> hs = new List<int>();
                    hs.Add(5);
                    hs.Add(10);
                    hs.Add(18);
                    hs.Add(3);
                    hs.Add(4);
                    heros[Scheduler.Parm.TrainHero.targetHeros] = hs;
                    heros = JSON.recode(heros);

                    json = oGA.getTaskParmObject(Scheduler.TaskId.TrainHero);
                    json[Scheduler.Parm.TrainHero.targetHeros] = heros[Scheduler.Parm.TrainHero.targetHeros];
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

        /*
        private void goStarrySetup()
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            WarSetup.goSetup(oGA, Scheduler.TaskId.StarryFight, 0, 1, 5, true, -1, null, this);
        }
        */

        private void goTask(string taskId, bool allPlayers)
        {
            if (allPlayers)
            {
                Thread thread = new Thread(() => goTaskThread(null, taskId));
                thread.Start();
            }
            else
            {
                // Must get account in UI thread then pass to background thread
                GameAccount oGA = GetSelectedAccount();
                if (oGA == null) return;
                if (oGA.IsOnline())
                {
                    Thread thread = new Thread(() => goTaskThread(oGA, taskId));
                    thread.Start();
                }
                else
                {
                    System.Windows.MessageBox.Show("帳戶已在其他地方登入");
                }
            }
        }

        private void goTaskThread(GameAccount oGA, string taskId)
        {
            string actionName = Scheduler.getTaskName(taskId);
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
                            gameAccount.executeTask(taskId, UpdateInfo, AppSettings.DEBUG);
                        }
                    }
                    else
                    {
                        oGA.checkStatus(true);
                        oGA.executeTask(taskId, UpdateInfo, AppSettings.DEBUG);
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
                    System.Windows.MessageBox.Show("帳戶已在其他地方登入");
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
                System.Windows.MessageBox.Show("尚未偵測到大皇帝帳戶, 請先登入遊戲.");
                return null;
            }

            GameAccount oGA = (GameAccount)lvAccounts.SelectedItem;
            if (oGA == null) System.Windows.MessageBox.Show("請先選擇帳戶");
            return oGA;
        }

        /*
        private void goEliteFightSetup()
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            WarSetup.goSetup(oGA, Scheduler.TaskId.EliteFight, 0, 1, 5, true, -1, null, this);
        }
        */

        private void goWarSetup(string taskId, int idx, int minHeros, int maxHeros, bool reqChief, int fixHero, string fixHeroName)
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            WarSetup.goSetup(oGA, taskId, idx, minHeros, maxHeros, reqChief, fixHero, fixHeroName, this);
            saveWarInfos();
        }

    }
}
