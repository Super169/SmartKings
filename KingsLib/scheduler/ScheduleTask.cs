using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.scheduler
{
    public static class ScheduleTask
    {
        public static List<Scheduler> scheduler;

        public static void initScheduler()
        {
            scheduler = new List<Scheduler>();

            /*
            List<TimeSpan> listTS;
            scheduler.Add(new Scheduler(new KingsTask.TaskInfo()
            {
                id = KingsTask.TaskId.Harvest,
                isEnabled = false
            }, new Schedule.ScheduleInfo()
            {
                dow = null,
                startTime = null,
                endTime = null,
                elapseMin = 60,
                executionTimes = null,
                maxRetry = 3,
                retryFreqMin = 1
            }));

            listTS = new List<TimeSpan>();
            listTS.Add(new TimeSpan(5, 15, 0));
            listTS.Add(new TimeSpan(9, 15, 0));
            listTS.Add(new TimeSpan(11, 35, 0));
            listTS.Add(new TimeSpan(12, 35, 0));
            listTS.Add(new TimeSpan(18, 15, 0));

            scheduler.Add(new Scheduler(new KingsTask.TaskInfo()
            {
                id = KingsTask.TaskId.Reload,
                isEnabled = false,
            }, new Schedule.ScheduleInfo()
            {
                dow = null,
                startTime = null,
                endTime = null,
                elapseMin = 0,
                executionTimes = listTS,
                maxRetry = 0,
                retryFreqMin = 0
            }));
            */
        }

        public static Schedule.ScheduleInfo defaultSchedule(string taskId)
        {
            Schedule.ScheduleInfo si = null;
            switch (taskId)
            {
                case KingsTask.TaskId.Harvest:
                    // Harvest for every hour
                    si = new Schedule.ScheduleInfo()
                    {
                        dow = null,
                        startTime = null,
                        endTime = null,
                        elapseMin = 60,
                        executionTimes = null,
                        maxRetry = 3,
                        retryFreqMin = 1
                    };
                    break;

                default:
                    // Default excuted at the following timeslot
                    // - 05:15 (day start)
                    // - 12:15 (Special start after maintenance)
                    List<TimeSpan> ts = new List<TimeSpan>();
                    ts.Add(new TimeSpan(5, 15, 0));
                    ts.Add(new TimeSpan(12, 15, 0));
                    si = new Schedule.ScheduleInfo()
                    {
                        dow = null,
                        startTime = null,
                        endTime = null,
                        elapseMin = 0,
                        executionTimes = ts,
                        maxRetry = 0,
                        retryFreqMin = 0
                    };
                    break;
            }


            return si;
        }

    }
}
