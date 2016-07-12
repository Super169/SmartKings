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
        const string KEY_GAMEACCOUNTS = "gameAccounts";
        const string jazGameAccounts = "gameAccounts.jaz";
        const string jazNewtonGameAccounts = "gameAccounts.newton.jaz";

        private void bindAccounts()
        {
            lvAccounts.ItemsSource = gameAccounts;
        }

        private void saveAccounts()
        {
            DateTime startTime = DateTime.Now;

            dynamic jsonData = JSON.Empty;
            List<dynamic> acList = new List<dynamic>();
            foreach (GameAccount oGA in gameAccounts)
            {
                acList.Add(oGA.toJson());
            }
            jsonData[KEY_GAMEACCOUNTS] = acList;
            JSON.saveConfig(jsonData, jazGameAccounts);

            
            string js = Newtonsoft.Json.JsonConvert.SerializeObject(gameAccounts);
            JSON.saveConfig(js, jazNewtonGameAccounts);
            
            DateTime endTime = DateTime.Now;
            TimeSpan ts = endTime - startTime;
            if (AppSettings.DEBUG) LOG.D(string.Format("It takes {0}ms to save account data", ts.TotalMilliseconds));
        }

        private void restoreAccounts()
        {
            int currSelectedIndex = lvAccounts.SelectedIndex;

            dynamic json = JSON.Empty;
            if (!JSON.restoreConfig(ref json, jazGameAccounts)) return;
            if ((json[KEY_GAMEACCOUNTS] == null) || (json[KEY_GAMEACCOUNTS].GetType() != typeof(DynamicJsonArray))) return;
            DynamicJsonArray dja = json[KEY_GAMEACCOUNTS];

            lock (gameAccountsLocker)
            {
                gameAccounts.Clear();
                foreach (dynamic o in dja)
                {
                    GameAccount oGA = new GameAccount(o);
                    gameAccounts.Add(oGA);
                    KingsMonitor.addAccount(oGA.account, oGA.sid);

                    ConnectionInfo ci = oGA.connectionInfo;

                    if (ci.headers.Exists(x => x.Key == "Connection")) ci.headers.Remove(ci.headers.Find(x => x.Key == "Connection"));
                    if (ci.headers.Exists(x => x.Key == "Proxy-Connection")) ci.headers.Remove(ci.headers.Find(x => x.Key == "Proxy-Connection"));
                    if (ci.headers.Exists(x => x.Key == "Proxy-Authorization")) ci.headers.Remove(ci.headers.Find(x => x.Key == "Proxy-Authorization"));
                    if (AppSettings.UseProxy)
                    {
                        ci.headers.Add(new KeyValuePair<string, string>("Proxy-Connection", "keep-alive"));
                        ci.headers.Add(new KeyValuePair<string, string>("Proxy-Authorization", AppSettings.Proxy));

                    } else
                    {
                        ci.headers.Add(new KeyValuePair<string, string>("Connection", "keep-alive"));
                    }


                    DateTime t1 = DateTime.Now;
                    oGA.refreshAccount();
                    DateTime t2 = DateTime.Now;
                    TimeSpan ts = t2 - t1;
//                    MessageBox.Show(string.Format("{0} - {1}", oGA.displayName, ts));
                }
            }

            /*
            List<GameAccount> ga;

            string js = "";
            if (!JSON.restoreConfig(ref js, jazNewtonGameAccounts)) return;
            ga = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GameAccount>>(js);

            foreach (GameAccount o in ga)
            {
                o.refreshAccount();
            }
            
            // gameAccounts = ga;  // Dummy for testing

            */
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
                    restoreWarInfos(oGA.account);
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
