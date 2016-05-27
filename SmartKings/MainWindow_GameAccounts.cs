using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KingsLib.data;
using MyUtil;
using Fiddler;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Data;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        List<GameAccount> gameAccounts = new List<GameAccount>();
        Object gameAccountsLocker = new Object();
        string gaFileName = "SmartKings.GFR";

        private void blindingAccounts()
        {
            restoreAccounts();
            lvAccounts.ItemsSource = gameAccounts;
        }

        private void saveAccounts()
        {
            List<GFR.GenericFileRecord> gfrs = new List<GFR.GenericFileRecord>();

            foreach (GameAccount oGA in gameAccounts)
            {
                gfrs.Add(oGA.ToGFR());
            }
            GFR.saveGFR(gaFileName, gfrs);
        }

        private void restoreAccounts()
        {
            List<GFR.GenericFileRecord> gfrs = null;
            if (GFR.restoreGFR(gaFileName, ref gfrs))
            {
                lock (gameAccountsLocker)
                {
                    gameAccounts.Clear();

                    int currSelectedIndex = lvAccounts.SelectedIndex;

                    lock (gameAccountsLocker)
                    {
                        gameAccounts.Clear();
                        foreach (GFR.GenericFileRecord gfr in gfrs)
                        {
                            gameAccounts.Add(new GameAccount(gfr));
                        }
                    }
                    lvAccounts.SelectedIndex = (currSelectedIndex == -1 ? 0 : currSelectedIndex);
                    goTaskCheckStatus(true);

                    /*
                    goCheckAccountStatus(true);
                    foreach (GameAccount oGA in gameAccounts)
                    {
                        KingsMonitor.addAccount(oGA.account, oGA.sid);
                    }
                    refreshAccountList();
                    */
                }
            }
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

        void UpdateAccountList(LoginInfo li, HTTPRequestHeaders oH)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateAccountList(li, oH)));
                return;
            }

            lock (gameAccountsLocker)
            {
                GameAccount oExists = gameAccounts.SingleOrDefault(x => x.account == li.account);
                if (oExists == null)
                {
                    GameAccount oGA = new GameAccount(li, oH);
                    gameAccounts.Add(oGA);
                    if (lvAccounts.SelectedIndex == -1) lvAccounts.SelectedIndex = 0;

                    UpdateStatus(String.Format("加入 {0}: {1} - {2} [{3}]", li.account, li.serverTitle, li.nickName, li.sid));
                }
                else
                {
                    oExists.status = GameAccount.AccountStatus.Online;
                    oExists.sid = li.sid;
                    oExists.serverTitle = li.serverTitle;
                    oExists.nickName = li.nickName;
                    oExists.currHeader = oH;
                    oExists.lastUpdateDTM = DateTime.Now;
                    oExists.refreshRecord();

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

    }
}
