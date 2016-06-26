using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KingsLib.data;
using MyUtil;

using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Data;
using System.Web.Helpers;
using KingsLib.monitor;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        List<GameAccount> gameAccounts = new List<GameAccount>();
        Object gameAccountsLocker = new Object();
        const string KEY_GA = "gameAccounts";
        const string jazFileName = "gameAccounts.jaz";

        private void bindAccounts()
        {
            restoreAccounts();
            lvAccounts.ItemsSource = gameAccounts;
        }

        private void saveAccounts()
        {
            dynamic jsonData = JSON.Empty;
            List<dynamic> acList = new List<dynamic>();
            foreach (GameAccount oGA in gameAccounts)
            {
                acList.Add(oGA.toJson());
            }
            jsonData[KEY_GA] = acList;
            JSON.toFile(jsonData, jazFileName);
        }

        private void restoreAccounts()
        {

            dynamic json = JSON.Empty;
            if (!JSON.fromFile(ref json, jazFileName)) return;
            if ((json[KEY_GA] == null) || (json[KEY_GA].GetType() != typeof(DynamicJsonArray))) return;
            DynamicJsonArray dja = json[KEY_GA];

            int currSelectedIndex = lvAccounts.SelectedIndex;
            lock (gameAccountsLocker)
            {
                gameAccounts.Clear();
                foreach (dynamic o in dja)
                {
                    GameAccount oGA = new GameAccount(o);
                    gameAccounts.Add(oGA);
                    KingsMonitor.addAccount(oGA.account, oGA.sid);
                    oGA.refreshAccount();
                }
            }

            gameAccounts.Sort();
            lvAccounts.SelectedIndex = (currSelectedIndex == -1 ? 0 : currSelectedIndex);
        }

        void refreshAccountList()
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => refreshAccountList()));
                return;
            }

            ICollectionView view = CollectionViewSource.GetDefaultView(lvAccounts.ItemsSource);
            view.Refresh();
        }

        void UpdateAccountList(AccountInfo li, ConnectionInfo ci)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateAccountList(li, ci)));
                return;
            }

            lock (gameAccountsLocker)
            {
                GameAccount oExists = gameAccounts.SingleOrDefault(x => x.account == li.account);
                if (oExists == null)
                {
                    GameAccount oGA = new GameAccount(li, ci);
                    gameAccounts.Add(oGA);
                    if (lvAccounts.SelectedIndex == -1) lvAccounts.SelectedIndex = 0;

                    UpdateStatus(String.Format("加入 {0}: {1} - {2} [{3}]", li.account, li.serverTitle, li.nickName, li.sid));
                }
                else
                {
                    oExists.updateSession(li, ci);
                    UpdateStatus(String.Format("更新 {0}: {1} - {2} [{3}]", li.account, li.serverTitle, li.nickName, li.sid));
                }
                refreshAccountList();
            }
        }

        void UpdateAccountList(GameAccount oGA)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateAccountList(oGA)));
                return;
            }

            lock (gameAccountsLocker)
            {
                GameAccount oExists = gameAccounts.SingleOrDefault(x => x.account == oGA.account);
                if (oExists != null) gameAccounts.Remove(oExists);
                gameAccounts.Add(oGA);
            }
            if (lvAccounts.SelectedIndex == -1) lvAccounts.SelectedIndex = 0;

        }

        private GameAccount GetSelectedAccount(bool activeOnly = true)
        {
            if (gameAccounts.Count == 0)
            {
                MessageBox.Show("尚未偵測到大皇帝帳戶, 請先登入遊戲.");
                return null;
            }

            GameAccount oGA = (GameAccount)lvAccounts.SelectedItem;
            if (oGA == null)
            {
                MessageBox.Show("請先選擇帳戶.");
                return null;
            }

            oGA.checkStatus();
            if (activeOnly && !oGA.IsOnline())
            {
                MessageBox.Show("帳戶已在其他地方登入了, 請重新登入一次.");
                return null;
            }
            return oGA;
        }

    }
}
