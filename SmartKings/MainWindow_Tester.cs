using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        private void btnTeter_Click(object sender, RoutedEventArgs e)
        {
            /*
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            RequestReturnObject rro = KingsLib.request.System.ping(oGA.connectionInfo, oGA.sid);
            UpdateInfo(oGA.serverCode, "測試", rro.requestText);
            Console.WriteLine("Done");
            */
            Thread oThread = new Thread(new ThreadStart(goTester));
            oThread.Start();
        }

        private void goTester()
        {
            /*
            DateTime? dt;
            DateTime startDtm;
            DateTime endDtm;
            string msg;

            Schedule.ScheduleInfo si;

            si = new Schedule.ScheduleInfo();
            si.startTime = new TimeSpan(11, 0, 0);
            si.endTime = new TimeSpan(19, 0, 0);
            si.initNextTime();
            dt = si.nextExecutionTime;
            startDtm = si.getStartTime(DateTime.Now);
            endDtm = si.getEndTime(DateTime.Now);
            msg = string.Format("{0:yyyy-MM-dd HH:mm:ss} : {1:yyyy-MM-dd hh:mm:ss} - {2:yyyy-MM-dd hh:mm:ss}", dt, startDtm, endDtm);
            Console.WriteLine(msg);


            si = new Schedule.ScheduleInfo();
            si.startTime = new TimeSpan(11, 0, 0);
            si.endTime = new TimeSpan(19, 0, 0);
            si.setNextTime(true);
            dt = si.nextExecutionTime;
            startDtm = si.getStartTime(DateTime.Now);
            endDtm = si.getEndTime(DateTime.Now);
            msg = string.Format("{0:yyyy-MM-dd HH:mm:ss} : {1:yyyy-MM-dd hh:mm:ss} - {2:yyyy-MM-dd hh:mm:ss}", dt, startDtm, endDtm);
            Console.WriteLine(msg);

            si = new Schedule.ScheduleInfo();
            si.startTime = new TimeSpan(11, 0, 0);
            si.endTime = new TimeSpan(19, 0, 0);
            si.maxRetry = 1;
            si.retryFreqMin = 12;
            si.setNextTime(false);
            dt = si.nextExecutionTime;
            startDtm = si.getStartTime(DateTime.Now);
            endDtm = si.getEndTime(DateTime.Now);
            msg = string.Format("{0:yyyy-MM-dd HH:mm:ss} : {1:yyyy-MM-dd hh:mm:ss} - {2:yyyy-MM-dd hh:mm:ss}", dt, startDtm, endDtm);
            Console.WriteLine(msg);
            */

            List<KingsTask.TaskInfo> t = KingsLib.scheduler.KingsTask.systemTasks;
            foreach (KingsTask.TaskInfo task in t)
            {
                if (task.isEnabled)
                {
                    Console.WriteLine(task.id);
                }
            }


            UpdateEventLog(DateTime.Now, "***", "手動測試", "開始", true);
            goTaskCheckStatus();
            refreshAccountList();
            goTaskCheckOutstanding();
            UpdateEventLog(DateTime.Now, "***", "手動測試", "結束", true);
            
        }


    }
}
