using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SmartKings
{
    public partial class MainWindow : Window
    {

        private string SystemTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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

                }
                return;
            }
            if (resetText) tb.Text = "";
            if (addTime) tb.Text += SystemTime() + " | ";
            tb.Text += info + (newLine ? "\n" : "");
            tb.ScrollToEnd();
        }

        private void UpdateStatus(string status, bool addTime = true, bool resetText = false)
        {
            userNotification(txtStatus, status, addTime, resetText);
        }

        private void UpdateInfo(string account, string action, string msg, bool addTime = true, bool async = true)
        {
            string infoMsg = account + "|" + action + "|" + msg;
            userNotification(txtInfo, infoMsg, addTime, false, async);
        }
    }
}
