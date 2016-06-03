﻿using System;
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
using System.Web.Helpers;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        List<GameAccount> gameAccounts = new List<GameAccount>();
        Object gameAccountsLocker = new Object();
        string gaFileName = "SmartKings.GFR";

        const string KEY_GA = "gameAccounts";
        const string jazFileName = "gameAccounts.jaz";

        private void blindingAccounts()
        {
            restoreAccounts();
            lvAccounts.ItemsSource = gameAccounts;
        }

        private void saveAccounts_old()
        {
            List<GFR.GenericFileRecord> gfrs = new List<GFR.GenericFileRecord>();

            foreach (GameAccount oGA in gameAccounts)
            {
                gfrs.Add(oGA.ToGFR());
            }
            GFR.saveGFR(gaFileName, gfrs);
        }

        private void saveAccounts()
        {
            dynamic jsonData = JSON.Empty;
            jsonData[KEY_GA] = gameAccounts;
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
                }

            }
            lvAccounts.SelectedIndex = (currSelectedIndex == -1 ? 0 : currSelectedIndex);
            goTaskCheckStatus(true);
        }


        private void restoreAccounts_old()
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
                    const string KEY_GA = "gameAccounts";
                    const string fileName = "gameAccounts.jaz";

                    dynamic jsonData = JSON.Empty;
                    jsonData[KEY_GA] = gameAccounts;
                    JSON.toFile(jsonData, fileName);

                    dynamic json = JSON.Empty;
                    JSON.fromFile(ref json, fileName);

                    if (json[KEY_GA] == null) return;

                    if (json[KEY_GA].GetType() != typeof(DynamicJsonArray)) return;

                    Console.WriteLine("Type match");

                    DynamicJsonArray dja = json[KEY_GA];

                    List<GameAccount> ga = new List<GameAccount>();

                    foreach (dynamic o in dja)
                    {
                        GameAccount oGA = new GameAccount(o);
                        ga.Add(oGA);
                    }

                    /*  // Testing code only
                    List<GameAccount> gs = new List<GameAccount>();
                    DynamicJsonArray json = KingsLib.util.infoBaseListToJsonArray(gameAccounts.ToArray());
                    if (json.GetType() == typeof(DynamicJsonArray))
                    {
                        foreach (dynamic o in json)
                        {
                            GameAccount ga = new GameAccount(o);
                            if (ga.ready) gs.Add(ga);
                        }
                    }
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
