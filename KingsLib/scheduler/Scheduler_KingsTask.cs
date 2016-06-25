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
            public const string StarryFight = "StarryFight";
            public const string StarryReward = "StarryReward";
            public const string EliteFight = "EliteFight";
            public const string Patrol = "Patrol";
            public const string CorpsCityReward = "CorpsCityReward";
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
                case TaskId.StarryFight:
                    taskName = "攬星通關";
                    break;
                case TaskId.StarryReward:
                    taskName = "攬星獎勵";
                    break;
                case TaskId.ReadAllEmail:
                    taskName = "開啟郵件";
                    break;
                case TaskId.EliteFight:
                    taskName = "討伐群雄";
                    break;
                case TaskId.Patrol:
                    taskName = "民生民惰";
                    break;
                case TaskId.CorpsCityReward:
                    taskName = "城池產出";
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
            public string info { get; set;  }
            public bool isEnabled { get; set; }
            public bool customSchedule;
            public DelegateExecuteTask executeTask;
            public string taskName { get { return getTaskName(id); } }
        }



        public static void initAutoTasks()
        {
            autoTaskList = new List<KingsTask>();

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.CleanUpBag,
                info = "使用背包中的消耗品 - 喇叭, 地圖, 寶箱",
                isEnabled = true,
                customSchedule = false,
                executeTask = action.task.goCleanupBag
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.CycleShop,
                info = "在東瀛寶船以銀子購物",
                isEnabled = true,
                customSchedule = false,
                executeTask = action.task.goCycleShop
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.FinishTask,
                info = "領取已完成的任務的獎勵",
                isEnabled = true,
                customSchedule = false,
                executeTask = action.task.goFinishAllTask
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.Harvest,
                info = "收取封地上生產的 銀子, 糧食 及 精鐵",
                isEnabled = true,
                customSchedule = false,
                executeTask = action.task.goHarvest
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.Market,
                info = "到 雜貨店 以銀子購買 糧食 及 精鐵",
                isEnabled = true,
                customSchedule = false,
                executeTask = action.task.goMarket
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.SignIn,
                info = "自動進行每天登入",
                isEnabled = true,
                customSchedule = false,
                executeTask = action.task.goSignIn
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.ReadAllEmail,
                info = "打開郵件並取出附件的物品",
                isEnabled = true,
                customSchedule = false,
                executeTask = action.task.goReadAllEmail
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.StarryFight,
                info = "在 攬星壇 作戰以取得兵書所需",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goStarryFight
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.StarryReward,
                info = "檢查並領取 攬星壇 中已完成任務的獎勵",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goStarryReward
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.EliteFight,
                info = "進行討伐群雄",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goEliteFight
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.Patrol,
                info = "執行民生民情任務",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goPatrol
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.CorpsCityReward,
                info = "收取城池產出獎勵",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goCorpsCityReward
            });

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
