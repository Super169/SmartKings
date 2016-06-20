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

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        List<EventLog> eventLogs = new List<EventLog>();

        private void bindEventLogs()
        {
            lvEventLog.ItemsSource = eventLogs;
        }

        private void refreshEventLog()
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => refreshAccountList()));
                return;
            }

            ICollectionView view = CollectionViewSource.GetDefaultView(lvEventLog.ItemsSource);
            view.Refresh();
        }

        private void SetUI()
        {
            
            lblAutoRunning.Content = (normalMode ? "自動大皇帝 準備中" : "自動大皇帝 已啟動");
            lblAutoRunning.Background = (normalMode ? Brushes.Gray : Brushes.LawnGreen);
            lblAutoRunning.Foreground = (normalMode ? Brushes.Black : Brushes.Red);


            btnAuto.IsEnabled = true;
            btnAuto.Content = (normalMode ? "啟動" : "停止") + " 自動大皇帝";
            btnAuto.Background = (normalMode ? Brushes.LawnGreen : Brushes.Red);

            if (normalMode) txtNextExecution.Text = "";
        }

        private string SystemTime()
        {
            return DateTime.Now.ToString("MM-dd HH:mm:ss");
        }

        private void UpdateTextBox(TextBox tb, string content, bool async = true)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                if (async)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      (Action)(() => UpdateTextBox(tb, content, async)));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      (Action)(() => UpdateTextBox(tb, content, async)));
                }
                return;
            }
            tb.Text = content;
            tb.ScrollToEnd();
            // if (!async) tb.InvalidateVisual();
        }

        private void userNotification(TextBox tb, string info, bool addTime = true, bool resetText = false, bool newLine = true, bool async = true)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                // Time must be added here, otherwise, there will have longer delay
                if (addTime) info = SystemTime() + " | " + info;

                if (async)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      (Action)(() => userNotification(tb, info, false, resetText, newLine)));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      (Action)(() => userNotification(tb, info, false, resetText, newLine)));

                }
                return;
            }
            if (resetText) tb.Text = "";
            if (addTime) tb.Text += SystemTime() + " | ";
            tb.Text += info + (newLine ? "\n" : "");
            tb.ScrollToEnd();
        }


        private void UpdateEventLog(DateTime eventTime, string account, string action, string msg, bool async = false)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                if (async)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      (Action)(() => UpdateEventLog(eventTime, account, action, msg, async)));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(
                      System.Windows.Threading.DispatcherPriority.Normal,
                      (Action)(() => UpdateEventLog(eventTime, account, action, msg, async)));
                }
                return;
            }
            eventLogs.Add(new EventLog() { eventTime = eventTime, account = account, action = action, msg = msg });
            refreshEventLog();
        }


        private void UpdateStatus(string status, bool addTime = true, bool resetText = false)
        {
            userNotification(txtStatus, status, addTime, resetText);
        }

        private void UpdateInfo(string account, string action, string msg, bool addTime = true, bool async = true)
        {
            UpdateEventLog(DateTime.Now, (account == null ? "***" : account), action, msg, async);
        }

        private void DebugLog(string action, string msg, string account = null)
        {
            UpdateEventLog(DateTime.Now, (account == null ? "***" : account), action,  "**** " + msg, false);
        }

        private void UpdateProgress(string progress = "")
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.Invoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    (Action)(() => UpdateProgress(progress)));
                return;
            }
            txtProgress.Text = progress;
            txtProgress.Background = (progress == "" ? Brushes.Transparent : Brushes.LawnGreen);
        }
    }
}
