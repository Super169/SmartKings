using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.scheduler
{
    public partial class Scheduler
    {
        public static List<KingsTask> autoTaskList;

        public delegate bool DelegateExecuteTask(GameAccount oGA, action.DelegateUpdateInfo updateInfo, bool debug);

        public static class TaskId
        {
            public const string Harvest = "Harvest";
            public const string CleanUpBag = "CleanUpBag";
            public const string Starry = "Starry";

        }

        public class KingsTask
        {
            public string id;
            public bool isEnabled;
            public bool customSchedule;
            public DelegateExecuteTask executeTask;
        }

        public static void initAutoTasks()
        {
            autoTaskList = new List<KingsTask>();
            autoTaskList.Add(new KingsTask() { id = TaskId.Harvest, isEnabled = true, customSchedule = false, executeTask = action.task.goTaskHarvest });
            autoTaskList.Add(new KingsTask() { id = TaskId.CleanUpBag, isEnabled = true, customSchedule = false, executeTask = action.task.goTaskCleanupBag });
            autoTaskList.Add(new KingsTask() { id = TaskId.Starry, isEnabled = true, customSchedule = true, executeTask = action.task.goTaskStarry });

        }

        public static ScheduleInfo defaultSchedule(string taskId)
        {
            ScheduleInfo si = new ScheduleInfo();
            switch (taskId)
            {
                case TaskId.Harvest:
                case TaskId.CleanUpBag:
                    si.elapseMin = 60;
                    si.maxRetry = 3;
                    si.retryFreqMin = 1;
                    break;
                default:
                    // Default excuted at the following timeslot
                    // - 05:15 (day start)
                    // - 12:15 (Special start after maintenance)
                    si.executionTimes = new List<TimeSpan>();
                    si.executionTimes.Add(new TimeSpan(5, 15, 0));
                    si.executionTimes.Add(new TimeSpan(12, 15, 0));
                    break;
            }

            return si;
        }


    }
}
