using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
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
            // Thread oThread = new Thread(new ThreadStart(goTester));
            // oThread.Start();
            int a = 1;
            
            GameAccount oGA1 = gameAccounts.ElementAt(0);
            string js = oGA1.toJsonString();
            dynamic json = JSON.decode(js);
            GameAccount oGA2 = new GameAccount(json);

            a = a + 1;

            List<Scheduler.AutoTask> ats = oGA1.autoTasks;
            Scheduler.AutoTask at1 = ats.ElementAt(a);
            js = at1.toJsonString();
            Scheduler.AutoTask at2 = new Scheduler.AutoTask(js);

            a = a + 1;

            Scheduler.ScheduleInfo si1 = at1.schedule;
            js = si1.toJsonString();
            Scheduler.ScheduleInfo si2 = new Scheduler.ScheduleInfo(js);

            a = a + 1;
            
            /*
            a = a + 1;
            DateTime d1 = DateTime.Now;
            dynamic j1 = Json.Decode("{}");
            j1["now"] = d1;
            string js1 = Json.Encode(j1);

            dynamic j2 = Json.Decode(js1);
            string js2 = Json.Encode(j2);
            DateTime d2 = j2["now"];

            a = a + 1;

            TimeZone tz = TimeZone.CurrentTimeZone;
            DateTime d3 = TimeZoneInfo.ConvertTimeFromUtc(d2, TimeZoneInfo.Local);
            a = a + 1;
            */

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
            msg = string.Format("{0:yyyy-MM-dd HH:mm:ss} : {1:yyyy-MM-dd HH:mm:ss} - {2:yyyy-MM-dd HH:mm:ss}", dt, startDtm, endDtm);
            Console.WriteLine(msg);


            si = new Schedule.ScheduleInfo();
            si.startTime = new TimeSpan(11, 0, 0);
            si.endTime = new TimeSpan(19, 0, 0);
            si.setNextTime(true);
            dt = si.nextExecutionTime;
            startDtm = si.getStartTime(DateTime.Now);
            endDtm = si.getEndTime(DateTime.Now);
            msg = string.Format("{0:yyyy-MM-dd HH:mm:ss} : {1:yyyy-MM-dd HH:mm:ss} - {2:yyyy-MM-dd HH:mm:ss}", dt, startDtm, endDtm);
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
            msg = string.Format("{0:yyyy-MM-dd HH:mm:ss} : {1:yyyy-MM-dd HH:mm:ss} - {2:yyyy-MM-dd HH:mm:ss}", dt, startDtm, endDtm);
            Console.WriteLine(msg);
            */
            /*
                        List<KingsTask.TaskInfo> t = KingsLib.scheduler.KingsTask.systemTasks;
                        foreach (KingsTask.TaskInfo task in t)
                        {
                            if (task.isEnabled)
                            {
                                Console.WriteLine(task.id);
                            }
                        }
            */

            LOG.D("Not so bad");
            LOG.E("Too Bad");

        }


    }
}
