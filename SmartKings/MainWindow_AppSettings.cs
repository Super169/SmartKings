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

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        private void loadAppSettings()
        {
            AppSettings.restoreSettings();
            ucAppSwitches.UpdateStatus += new EventHandler<string>(ucAppSetting_UpdateStatus);
            ucAppSwitches.loadAppSwitches();
        }

        private void ucAppSetting_UpdateStatus(object sender, string status)
        {
            UpdateInfo("***", "系統設定", status);
        }

        const string KEY_AUTOTASKS = "autoTasks";
        const string jazAutoTasks = "autoTasks.jaz";

        private void  restoreAutoTasksSettings()
        {
            // Don't load directly from file, load to temporary location then update the static variable
            List<Scheduler.KingsTask> aTasks = new List<Scheduler.KingsTask>();
            dynamic json = JSON.Empty;
            if (!JSON.fromFile(ref json, jazAutoTasks)) return;
            if (!JSON.exists(json, KEY_AUTOTASKS, typeof(DynamicJsonArray))) return;
            DynamicJsonArray dja = json[KEY_AUTOTASKS];
            foreach(dynamic j in dja)
            {
                Scheduler.KingsTask t = new Scheduler.KingsTask(j);
                Scheduler.KingsTask at = Scheduler.autoTaskList.Find(x => x.id == t.id);
                if (at != null)
                {
                    at.isEnabled = t.isEnabled;
                }
            }
        }

        private void saveAutoTasksSettings()
        {
            dynamic json = JSON.Empty;
            List<dynamic> taskList = new List<dynamic>();
            foreach (Scheduler.KingsTask t in Scheduler.autoTaskList)
            {
                taskList.Add(t.toJson());
            }
            json[KEY_AUTOTASKS] = taskList;
            JSON.toFile(json, jazAutoTasks);
        }


    }
}
