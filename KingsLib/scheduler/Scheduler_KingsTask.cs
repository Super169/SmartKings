using KingsLib.data;
using MyUtil;
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
            public const string ArenaReward = "ArenaReward";
            public const string ArenasReward = "ArenasReward";
            public const string BossWar = "BossWar";
            public const string CleanUpBag = "CleanUpBag";
            public const string CorpsCityReward = "CorpsCityReward";
            public const string CycleShop = "CycleShop";
            public const string EliteFight = "EliteFight";
            public const string EliteBuyTime = "EliteBuyTime";
            public const string FinishTask = "FinishTask";
            public const string GrassArrow = "GrassArrow";
            public const string Harvest = "Harvest";
            public const string IndustryShop = "IndustryShop";
            public const string LuckyCycle = "LuckyCycle";
            public const string Lottery = "Lottery";
            public const string Market = "Market";
            public const string OneYearSignIn = "OneYearSignIn";
            public const string Patrol = "Patrol";
            public const string ReadAllEmail = "ReadAllEmail";
            public const string SignIn = "SignIn";
            public const string SLShop = "SLShop";
            public const string StarryFight = "StarryFight";
            public const string StarryReward = "StarryReward";
            public const string TrainHero = "TrainHero";
            public const string TrialsBuyTimes = "TrialsBuyTimes";
            public const string TuanGo = "TuanGo";

        }

        public static string getTaskName(string id)
        {
            string taskName = "";
            switch (id)
            {
                case TaskId.ArenaReward:
                    taskName = "天下比武獎勵";
                    break;
                case TaskId.ArenasReward:
                    taskName = "三軍演武獎勵";
                    break;
                case TaskId.BossWar:
                    taskName = "神將無雙";
                    break;
                case TaskId.CleanUpBag:
                    taskName = "清理背包";
                    break;
                case TaskId.CycleShop:
                    taskName = "東瀛寶船";
                    break;
                case TaskId.EliteBuyTime:
                    taskName = "購買討伐次數";
                    break;
                case TaskId.FinishTask:
                    taskName = "任務報酬";
                    break;
                case TaskId.GrassArrow:
                    taskName = "草船借箭";
                    break;
                case TaskId.Harvest:
                    taskName = "封地收獲";
                    break;
                case TaskId.LuckyCycle:
                    taskName = "幸運轉盤";
                    break;
                case TaskId.Lottery:
                    taskName = "轉盤抽獎";
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
                case TaskId.OneYearSignIn:
                    taskName = "嘉年華會";
                    break;
                case TaskId.SLShop:
                    taskName = "勢力商店";
                    break;
                case TaskId.IndustryShop:
                    taskName = "勢力市集";
                    break;
                case TaskId.TuanGo:
                    taskName = "團購寶箱";
                    break;

                case TaskId.TrainHero:
                    taskName = "校場訓練";
                    break;
                case TaskId.TrialsBuyTimes:
                    taskName = "購買試練次數";
                    break;

                default:
                    taskName = string.Format("[{0}]", id);
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

            private static class KEY
            {
                public const string id = "id";
                public const string info = "info";
                public const string isEnabled = "isEnabled";
                public const string customSchedule = "customSchedule";
                public const string executeTask = "executeTask";
            }

            public KingsTask()
            {
                initObject();
            }

            public KingsTask(dynamic json)
            {
                fromJson(json);
            }

            private void initObject()
            {
                this.id = "";
                this.info = "";
                this.isEnabled = false;
                this.customSchedule = false;
                this.executeTask = null;
            }
            public dynamic toJson()
            {
                dynamic json = JSON.Empty;
                json[KEY.id] = this.id;
                json[KEY.isEnabled] = this.isEnabled;
                return json;
            }

            public void fromJson(dynamic json)
            {
                initObject();
                this.id = JSON.getString(json, KEY.id, "");
                this.isEnabled = JSON.getBool(json, KEY.isEnabled);
            }

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
                id = TaskId.GrassArrow,
                info = "進行草船借箭並領取換領獎勵",
                isEnabled = true,
                customSchedule = false,
                executeTask = action.task.goGrassArrow
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

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.IndustryShop,
                info = "在勢力市集中購買糧食/銀子",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goIndustryShop
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.TrainHero,
                info = "訓練英雄, 獲取升級經驗",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goTrainHero
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.LuckyCycle,
                info = "轉動幸運轉盤並領取獎勵",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goLuckyCycle
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.Lottery,
                info = "自動完成轉盤抽獎",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goLottery
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.ArenasReward,
                info = "領取三軍演武的獎勵",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goAreansReward
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.EliteBuyTime,
                info = "購買額外的英雄討伐次數",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goEliteBuyTime
            });

            autoTaskList.Add(new KingsTask()
            {
                id = TaskId.TrialsBuyTimes,
                info = "購買額外的英雄試煉次數",
                isEnabled = true,
                customSchedule = true,
                executeTask = action.task.goTrialBuyTime
            });

        }

        public static ScheduleInfo defaultSchedule(string taskId)
        {
            ScheduleInfo si = new ScheduleInfo();
            switch (taskId)
            {
                case TaskId.CleanUpBag:
                case TaskId.CorpsCityReward:
                case TaskId.FinishTask:
                case TaskId.Harvest:
                case TaskId.Patrol:
                case TaskId.ReadAllEmail:
                    si.elapseMin = 60;
                    si.maxRetry = 3;
                    si.retryFreqMin = 1;
                    break;
                case TaskId.CycleShop:
                    si.dow = new List<int>();
                    si.dow.Add(0);
                    si.dow.Add(3);
                    si.executionTimes = new List<TimeSpan>();
                    si.executionTimes.Add(new TimeSpan(5, 35, 0));
                    si.executionTimes.Add(new TimeSpan(13, 35, 0));
                    si.executionTimes.Add(new TimeSpan(3, 15, 0));
                    break;
                case TaskId.EliteFight:
                    si.dow = new List<int>();
                    si.dow.Add(0);
                    si.dow.Add(3);
                    si.executionTimes = new List<TimeSpan>();
                    si.executionTimes.Add(new TimeSpan(19, 15, 0));
                    si.executionTimes.Add(new TimeSpan(3, 15, 0));
                    break;
                case TaskId.Market:
                case TaskId.SignIn:
                    si.executionTimes = new List<TimeSpan>();
                    si.executionTimes.Add(new TimeSpan(10, 35, 0));
                    si.executionTimes.Add(new TimeSpan(13, 35, 0));
                    si.executionTimes.Add(new TimeSpan(3, 15, 0));
                    break;
                case TaskId.StarryFight:
                case TaskId.StarryReward:
                    si.executionTimes = new List<TimeSpan>();
                    si.executionTimes.Add(new TimeSpan(10, 35, 0));
                    si.executionTimes.Add(new TimeSpan(13, 35, 0));
                    si.executionTimes.Add(new TimeSpan(3, 15, 0));
                    break;
                case TaskId.IndustryShop:
                    si.executionTimes = new List<TimeSpan>();
                    si.executionTimes.Add(new TimeSpan(9, 35, 0));
                    si.executionTimes.Add(new TimeSpan(12, 35, 0));
                    si.executionTimes.Add(new TimeSpan(18, 35, 0));
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
