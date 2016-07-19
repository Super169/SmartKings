
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        // **** MainWindow_Closing has been added in App.xaml

        public bool readyClose()
        {
            if (autoRunning)
            {
                MessageBox.Show("自動排程進行中, 暫時不能離開.  請在排程執行結束後再試.", "請稍候");
                return false;
            }
            return true;
        }

        public void WindowPreClose()
        {
            Thread exitThread = new Thread(closeMe);
            exitThread.Start();

            /*
            autoTimer.Enabled = false;
            saveEventLog();
            saveAccounts();
            */
        }

        private void closeMe()
        {
            // Must sure no other action is runing by using acitonLocker
            // May need to improve this part, and Environment.Exit(0) may not be a good choice.
            if (Monitor.TryEnter(AppSettings.actionLocker, 5000))
            {
                try
                {
//                    normalMode = true;
//                    SetUI();
//                    UpdateStatus("自動大皇帝 - 停止");
                    autoTimer.Enabled = false;
                    saveEventLog();
                    saveAccounts();
                    ((App)Application.Current).preClose();
                    closeWindow();
                }
                finally
                {
                    Monitor.Exit(AppSettings.actionLocker);
                }
                // Environment.Exit(0);
            }
            else
            {
                UpdateInfo("***", "自動大皇帝", "等待超時, 暫時未能離開.");
            }
            AppSettings.stopAllActiion = false;

        }



        private void closeWindow()
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => closeWindow()));
                return;
            }
            this.Close();
        }

        private void OnNotificationHandler(string info)
        {
            // UpdateInfo(info);
        }

        private void OnNewSidHandler(AccountInfo li, ConnectionInfo ci)
        {

            // UpdateStatus(string.Format("Server: {0}, SID: {1}, {2} - {3}", li.server, li.sid, li.serverTitle, li.nickName));
            UpdateAccountList(li, ci);
        }

        private void btnCheckStatus_Click(object sender, RoutedEventArgs e)
        {
            UpdateEventLog(DateTime.Now, "***", "檢查帳戶", "開始", true);
            goTaskCheckStatus();
            refreshAccountList();
            UpdateEventLog(DateTime.Now, "***", "檢查帳戶", "結束", true);
        }

        private void btnTaskSetting_Click(object sender, RoutedEventArgs e)
        {
            ui.WinAutoTaskConfig winConfig = new ui.WinAutoTaskConfig();
            winConfig.Owner = this;
            winConfig.ShowDialog();
            saveAutoTasksSettings();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.stopAllActiion = true;
            if (MessageBox.Show("真的要離開?", "請確定", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ((App)Application.Current).ExitApplication();
            }
            else
            {
                AppSettings.stopAllActiion = false;
            }
        }

        private void btnAuto_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.stopAllActiion) return;
            toggleAutoKings();
        }

        private void btnSaveEventLog_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.stopAllActiion) return;
            saveEventLog();
            MessageBox.Show(string.Format("Result saved to {0}", saveEventLog()));
        }

        private string saveEventLog()
        {
            string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Log");
            filePath = System.IO.Path.Combine(filePath, "EventLog");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                if (!Directory.Exists(filePath)) filePath = Directory.GetCurrentDirectory();
            }

            string fileName = string.Format("{0:yyyyMMddHHmm}.Log", DateTime.Now);
            string fullName = System.IO.Path.Combine(filePath, fileName);

            StringBuilder sb = new StringBuilder();
            foreach (EventLog log in eventLogs)
            {
                sb.Append(string.Format("{0:yyyy-MM-dd HH:mm:ss}: {1} : {2} : {3}\n", log.eventTime, log.account, log.action, log.msg));
            }
            File.WriteAllText(fullName, sb.ToString());
            return fullName;
        }

        private void btnClearEventLog_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.stopAllActiion) return;
            eventLogs.Clear();
            refreshEventLog();
        }

        private void btnAccSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            Thread saveThread = new Thread(goAccSaveSetting);
            saveThread.Start();
        }

        private void goAccSaveSetting()
        {

            AppSettings.stopAllActiion = true;
            if (Monitor.TryEnter(AppSettings.actionLocker, 5000))
            {
                try
                {
                    UpdateProgress("儲存帳戶資料......");
                    saveAccounts();
                    UpdateProgress("");
                }
                finally
                {
                    Monitor.Exit(AppSettings.actionLocker);
                }
                // Environment.Exit(0);
            }
            else
            {
                UpdateInfo("***", "儲存帳戶資料", "等待超時, 暫時未能離開.");
            }
            AppSettings.stopAllActiion = false;

        }
    }
}
