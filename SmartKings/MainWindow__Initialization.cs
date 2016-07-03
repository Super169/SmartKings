using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KingsLib.monitor;
using KingsLib;
using KingsLib.scheduler;
using KingsLib.data;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        private void mainInitialization()
        {
            UpdateStatus(this.Title + " 啟動");
            com.start("SmartKings");

            // Setup static variables
            Scheduler.initAutoTasks();

            // Load settings
            loadAppSettings();
            restoreAutoTasksSettings();
            restoreAccounts();
            restoreWarInfos();

            // UI Settings                
            bindAccounts();
            bindEventLogs();
            initActionPanel();

            // Monitor
            KingsMonitor.notificationEventHandler += new KingsMonitor.NotificationEventHandler(this.OnNotificationHandler);
            KingsMonitor.newSidEventHandler += new KingsMonitor.NewSidEventHandler(this.OnNewSidHandler);
            if (!KingsMonitor.Start())
            {
                MessageBox.Show("啟動監察器失敗");
                this.Close();
            }


            // AUtoTask settings
            initTimer();
            if (AppSettings.AutoRun) toggleAutoKings();

        }



    }
}
