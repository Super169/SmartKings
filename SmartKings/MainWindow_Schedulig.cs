using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        System.Timers.Timer autoTimer = new System.Timers.Timer(1000);
        bool normalMode = true;

        List<Scheduler> systemTasks = new List<Scheduler>();
        Object systemTasksLocker = new object();

        private void initTimer()
        {
            autoTimer.Elapsed += new System.Timers.ElapsedEventHandler(autoTimerElapsedEventHandler);
            autoTimer.Enabled = false;
        }

        private void goAutoKings()
        {
            normalMode = !normalMode;
            SetUI();

            if (normalMode)
            {
                autoTimer.Enabled = false;
                UpdateStatus("自動大皇帝 - 停止");
            } else
            {
                UpdateStatus("自動大皇帝 - 啟動");
                autoTimer.Interval = 1000;
                autoTimer.Enabled = true;
            }
        }

        void autoTimerElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            autoTimer.Enabled = false;
            DateTime nextActionTime = DateTime.Now.AddMinutes(1);
            UpdateTextBox(txtLastExecution, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), false);
            UpdateTextBox(txtNextExecution, "等待中。。。。。。", false);

            if (Monitor.TryEnter(AppSettings.actionLocker, 5000))
            {
                try
                {
                    UpdateTextBox(txtNextExecution, "執行中。。。。。。", false);
                    UpdateProgress("自動大皇帝 - 進行中......");
                    if (AppSettings.DEBUG) UpdateInfo("***", "排程", "自動大皇帝 - 開始執行");

                    // Thread.Sleep(3000);

                    foreach (GameAccount oGA in gameAccounts)
                    {
                        if (oGA.IsOnline())
                        {
                            DateTime nextTime = oGA.goAutoTask(UpdateInfo, AppSettings.DEBUG);
                            if (nextTime < nextActionTime) nextActionTime = nextTime;
                        }
                    }

                    /*
                    DateTime minNext;
                    DateTime nextActionTime;

                    minNext = DateTime.Now;
                    int currMinSlot =  ( (int) (minNext.Minute / AppSettings.elapseMin + 1)) * AppSettings.elapseMin;
                    nextActionTime = new DateTime(minNext.Year, minNext.Month, minNext.Day, minNext.Hour, 00, 00).AddMinutes(currMinSlot + AppSettings.extraStartMin);
                    */

                    if (AppSettings.DEBUG) UpdateInfo("***", "排程", "自動大皇帝 - 執行完畢");
                    UpdateProgress();

                }
                finally
                {
                    Monitor.Exit(AppSettings.actionLocker);
                }
            } else
            {
                UpdateInfo("***", "自動大皇帝", string.Format("等待超時, 未能進行.  將於 {0:HH:mm:ss} 重試", nextActionTime));
            }

            double waitMS = (nextActionTime - DateTime.Now).TotalSeconds * 1000;
            if (waitMS < 0) waitMS = 1000;
            autoTimer.Interval = waitMS;
            UpdateTextBox(txtNextExecution, nextActionTime.ToString("yyyy-MM-dd HH:mm:ss"));
            autoTimer.Enabled = true;

        }

    }
}
