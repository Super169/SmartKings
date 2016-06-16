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
                    goAction("封地收獲", allPlayers, action.task.goTaskHarvest);
                    break;
                case "btnCleanBag":
                    goAction("清理背包", allPlayers, action.task.goTaskCleanupBag);
                    break;
            }

        }

        private void goCheckStatus()
        {
            if (AppSettings.DEBUG) DebugLog("帳戶狀況", "開始");
            goTaskCheckStatus();
            refreshAccountList();
            if (AppSettings.DEBUG) DebugLog("帳戶狀況", "結束");
        }


        private void goAction(string actionName, bool allPlayers, DelegateActionHandler actionHandler)
        {
            if (allPlayers)
            {
                Thread thread = new Thread(() => goActionThread(actionName, actionHandler));
                thread.Start();
            }
            else
            {
                // Must get account in UI thread then pass to background thread
                GameAccount oGA = GetSelectedActiveAccount();
                if (oGA == null) return;
                if (oGA.IsOnline())
                {
                    Thread thread = new Thread(() => goActionThread(actionName, oGA, actionHandler));
                    thread.Start();
                } else
                {
                    MessageBox.Show("帳戶已在其他地方登入");
                }
            }
        }

        private void goActionThread(string actionName, DelegateActionHandler actionHandler)
        {
            if (AppSettings.DEBUG) DebugLog(actionName, "開始");
            foreach (GameAccount oGA in gameAccounts)
            {
                oGA.checkStatus(true);
                if (oGA.IsOnline())
                {
                    actionHandler(oGA, UpdateInfo, AppSettings.DEBUG);
                }
            }
            if (AppSettings.DEBUG) DebugLog(actionName, "結束");
        }

        private void goActionThread(string actionName, GameAccount oGA, DelegateActionHandler actionHandler)
        {
            if (AppSettings.DEBUG) DebugLog(actionName, "開始");
            if ((oGA != null) && oGA.IsOnline())
            {
                actionHandler(oGA, UpdateInfo, AppSettings.DEBUG);
            }
            if (AppSettings.DEBUG) DebugLog(actionName, "結束");
        }

        private GameAccount GetSelectedActiveAccount()
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
