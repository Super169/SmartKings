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
            public const string BossWar = "BossWar";
            public const string CleanUpBag = "CleanUpBag";
            public const string CycleShop = "CycleShop";
            public const string FinishTask = "FinishTask";
            public const string Harvest = "Harvest";
            public const string Market = "Market";
            public const string SignIn = "SignIn";
            public const string ReadAllEmail = "ReadAllEmail";
            public const string Starry = "Starry";

        }

        public static string getTaskName(string id)
        {
            string taskName = "";
            switch (id)
            {
                case TaskId.BossWar:
                    taskName = "神將無雙";
                    break;
                case TaskId.CleanUpBag:
                    taskName = "清理背包";
                    break;
                case TaskId.CycleShop:
                    taskName = "東瀛寶船";
                    break;
                case TaskId.FinishTask:
                    taskName = "任務報酬";
                    break;
                case TaskId.Harvest:
                    taskName = "封地收獲";
                    break;
                case TaskId.Market:
                    taskName = "糧草先行";
                    break;
                case TaskId.SignIn:
                    taskName = "簽到領獎";
                    break;
                case TaskId.Starry:
                    taskName = "攬星壇";
                    break;
                case TaskId.ReadAllEmail:
                    taskName = "開啟郵件";
                    break;
                default:
                    taskName = string.Format("[{id}]", id);
                    break;
            }
            return taskName;
        }

        public class KingsTask
        {
            public string id;
            public bool isEnabled { get; set; }
            public bool customSchedule;
            public DelegateExecuteTask executeTask;
            public string taskName { get { return getTaskName(id); } }


        }



        public static void initAutoTasks()
        {
            autoTaskList = new List<KingsTask>();
            autoTaskList.Add(new KingsTask() { id = TaskId.CleanUpBag, isEnabled = true, customSchedule = false, executeTask = action.task.goCleanupBag });
            autoTaskList.Add(new KingsTask() { id = TaskId.CycleShop, isEnabled = true, customSchedule = false, executeTask = action.task.goCycleShop });
            autoTaskList.Add(new KingsTask() { id = TaskId.FinishTask, isEnabled = true, customSchedule = false, executeTask = action.task.goFinishAllTask });
            autoTaskList.Add(new KingsTask() { id = TaskId.Harvest, isEnabled = true, customSchedule = false, executeTask = action.task.goHarvest });
            autoTaskList.Add(new KingsTask() { id = TaskId.Market, isEnabled = true, customSchedule = false, executeTask = action.task.goMarket });
            autoTaskList.Add(new KingsTask() { id = TaskId.SignIn, isEnabled = true, customSchedule = false, executeTask = action.task.goSignIn });
            autoTaskList.Add(new KingsTask() { id = TaskId.ReadAllEmail, isEnabled = true, customSchedule = false, executeTask = action.task.goReadAllEmail });
            autoTaskList.Add(new KingsTask() { id = TaskId.Starry, isEnabled = true, customSchedule = true, executeTask = action.task.goCheckStarry });
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
