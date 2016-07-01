
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        // **** MainWindow_Closing has been added in App.xaml
        
        public void WindowPreClose()
        {
            saveAccounts();
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

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("真的要離開?", "請確定", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ((App)Application.Current).ExitApplication();
            }
        }

        private void btnAuto_Click(object sender, RoutedEventArgs e)
        {
            goAutoKings();
        }

        private void btnSaveEventLog_Click(object sender, RoutedEventArgs e)
        {
            string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Log");
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
            MessageBox.Show(string.Format("Result saved to {0}", fullName));
        }

        private void btnClearEventLog_Click(object sender, RoutedEventArgs e)
        {
            eventLogs.Clear();
            refreshEventLog();
        }
    }
}
