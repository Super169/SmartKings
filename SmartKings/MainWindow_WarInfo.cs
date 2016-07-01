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
using MyUtil;
using KingsLib.scheduler;
using System.Web.Helpers;
using KingsLib.data;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        const string KEY_WARINFOS = "warInfos";
        const string jazWarInfos = "warInfos.jaz";

        private void saveWarInfos()
        {
            // Special handling, warInfos will not be clear until the file is removed
            dynamic json = JSON.Empty;
            List<WarInfo> saveInfos = new List<WarInfo>();
            if (JSON.fromFile(ref json, jazWarInfos) && JSON.exists(json, KEY_WARINFOS, typeof(DynamicJsonArray)))
            {
                DynamicJsonArray dja = json[KEY_WARINFOS];
                foreach (dynamic o in dja)
                {
                    WarInfo wi = new WarInfo(o);
                    if ((wi.account != null) && (wi.taskId != null))
                    {
                        saveInfos.Add(wi);
                    }
                }
            }

            lock (gameAccountsLocker)
            {
                foreach (GameAccount oGA in gameAccounts)
                {
                    foreach (WarInfo wi in oGA.warInfos)
                    {
                        WarInfo w = saveInfos.Find(x => ((x.account == wi.account) && (x.taskId == wi.taskId) && (x.idx == wi.idx)));
                        if (w != null) saveInfos.Remove(w);
                        saveInfos.Add(wi);
                    }
                }
            }

            json = JSON.Empty;
            List<dynamic> warInfoList = new List<dynamic>();
            foreach (WarInfo wi in saveInfos)
            {
                warInfoList.Add(wi.toJson());
            }
            json[KEY_WARINFOS] = warInfoList;
            JSON.saveConfig(json, jazWarInfos);
        }

        private void restoreWarInfos()
        {

            dynamic json = JSON.Empty;
            if (!JSON.restoreConfig(ref json, jazWarInfos)) return;
            if (!JSON.exists(json, KEY_WARINFOS, typeof(DynamicJsonArray))) return;

            lock (gameAccountsLocker)
            {
                foreach (GameAccount oGA in gameAccounts)
                {
                    oGA.warInfos = new List<WarInfo>();
                }
                DynamicJsonArray wis = json[KEY_WARINFOS];
                foreach (dynamic o in wis)
                {
                    WarInfo wi = new WarInfo(o);
                    if ((wi.account != null) && (wi.taskId != null))
                    {
                        GameAccount oGA = gameAccounts.Find(x => x.account == wi.account);
                        if (oGA != null)
                        {
                            oGA.warInfos.Add(wi);
                        }
                    }
                }
            }
        }

        private void restoreWarInfos(string account)
        {
            lock (gameAccounts)
            {
                GameAccount oGA = gameAccounts.Find(x => x.account == account);
                if (oGA == null) return;
                dynamic json = JSON.Empty;
                if (!JSON.fromFile(ref json, jazWarInfos)) return;
                if (!JSON.exists(json, KEY_WARINFOS, typeof(DynamicJsonArray))) return;
                DynamicJsonArray wis = json[KEY_WARINFOS];
                foreach (dynamic o in wis)
                {
                    WarInfo wi = new WarInfo(o);
                    if (wi.account == account) oGA.warInfos.Add(wi);
                }
            }
        }

    }
}
