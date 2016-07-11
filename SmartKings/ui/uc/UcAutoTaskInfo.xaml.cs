using KingsLib;
using KingsLib.data;
using KingsLib.scheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartKings.ui.uc
{
    /// <summary>
    /// Interaction logic for UcAutoTaskInfo.xaml
    /// </summary>
    public partial class UcAutoTaskInfo : UserControl
    {
        GameAccount oGA;

        private class AutoTaskInfo
        {
            public bool sysEnabled { get; set; }
            public bool accEnabled { get; set; }
            public string id { get; set; }
            public string taskName { get; set; }
            public string info { get; set; }
            public string remark { get; set; }
            public string lastExecution { get; set; }
            public string nextExecution { get; set; }
        }

        List<AutoTaskInfo> atis = new List<AutoTaskInfo>();

        public UcAutoTaskInfo()
        {
            InitializeComponent();
        }

        public void Setup(GameAccount oGA)
        {
            this.oGA = oGA;
            refreshAll();
        }

        private void refreshAll()
        {

            atis = new List<AutoTaskInfo>();
            foreach (Scheduler.KingsTask kt in Scheduler.autoTaskList)
            {
                string taskId = kt.id;
                Scheduler.AutoTask at = oGA.findAutoTask(taskId);
                AutoTaskInfo ati;
                if (at == null)
                {
                    ati = new AutoTaskInfo()
                    {
                        sysEnabled = kt.isEnabled,
                        accEnabled = false,
                        id = kt.id,
                        taskName = kt.taskName,
                        info = kt.info,
                        lastExecution = "--",
                        nextExecution = "--",
                        remark = "帳戶沒有該項工作"
                    };
                }
                else
                {
                    ati = new AutoTaskInfo()
                    {
                        sysEnabled = kt.isEnabled,
                        accEnabled = at.isEnabled,
                        id = kt.id,
                        taskName = kt.taskName,
                        info = kt.info,
                        lastExecution = (at.schedule.lastExecutionTime == null ? "--" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", at.schedule.lastExecutionTime)),
                        nextExecution = (at.schedule.nextExecutionTime == null ? "--" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", at.schedule.nextExecutionTime)),
                        remark = at.schedule.getScheduleInfo(kt.getNextTime == null)
                    };
                }
                atis.Add(ati);
            }
            lvAutoTaskInfo.ItemsSource = atis;

            ICollectionView view = CollectionViewSource.GetDefaultView(lvAutoTaskInfo.ItemsSource);
            view.Refresh();
        }

        public void SaveSettings()
        {
            foreach (AutoTaskInfo ati in atis)
            {
                Scheduler.AutoTask myTask = oGA.findAutoTask(ati.id);
                if (myTask != null) myTask.isEnabled = ati.accEnabled;
            }
        }

        private AutoTaskInfo getSelectedTask()
        {

            if (atis.Count == 0) return null;

            AutoTaskInfo ati  = (AutoTaskInfo) lvAutoTaskInfo.SelectedItem;

            return ati;
        }


        public void resetSchedule()
        {
            AutoTaskInfo ati = getSelectedTask();

            if (ati == null)
            {
                MessageBox.Show("請先選擇排程項目");
                return;
            }

            Scheduler.AutoTask at = oGA.findAutoTask(ati.id);
            if (at == null)
            {
                MessageBox.Show("系統錯誤, 找不到相關排程.");
                return;
            }

            at.schedule.lastExecutionTime = null;
            at.schedule.initNextTime();

            ati.lastExecution = (at.schedule.lastExecutionTime == null ? "--" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", at.schedule.lastExecutionTime));
            ati.nextExecution = (at.schedule.nextExecutionTime == null ? "--" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", at.schedule.nextExecutionTime));

            ICollectionView view = CollectionViewSource.GetDefaultView(lvAutoTaskInfo.ItemsSource);
            view.Refresh();

        }

        public void updateSelection(bool isEnabled)
        {
            foreach (Scheduler.AutoTask at in oGA.autoTasks)
            {
                at.isEnabled = isEnabled;
            }
            refreshAll();
        }

        public void selectSafe()
        {
            foreach (Scheduler.AutoTask at in oGA.autoTasks)
            {
                Scheduler.KingsTask kt = Scheduler.autoTaskList.Find(x => x.id == at.taskId);
                if (kt ==null)
                {
                    at.isEnabled = false;
                } else
                {
                    at.isEnabled = (kt.suggestion == 1);
                }
            }
            refreshAll();
        }

        public void selectSuggested()
        {
            foreach (Scheduler.AutoTask at in oGA.autoTasks)
            {
                Scheduler.KingsTask kt = Scheduler.autoTaskList.Find(x => x.id == at.taskId);
                if (kt == null)
                {
                    at.isEnabled = false;
                }
                else
                {
                    at.isEnabled = ((kt.suggestion == 1) || (kt.suggestion == 2));
                }
            }
            refreshAll();
        }

        public void taskSetup()
        {
        }

        public void warSetup(int idx)
        {
            AutoTaskInfo ati = getSelectedTask();

            if (ati == null)
            {
                MessageBox.Show("請先選擇排程項目");
                return;
            }

            Scheduler.AutoTask at = oGA.findAutoTask(ati.id);
            if (at == null)
            {
                MessageBox.Show("系統錯誤, 找不到相關排程.");
                return;
            }

            string taskId = at.taskId;
            string taskName = Scheduler.getTaskName(taskId);

            switch (taskId)
            {
                case Scheduler.TaskId.StarryFight:
                    goWarSetup(Scheduler.TaskId.StarryFight, idx, 1, 5, true, -1, null);
                    break;
                case Scheduler.TaskId.NavalWar:
                    goWarSetup(Scheduler.TaskId.NavalWar, idx, 1, 5, true, -1, null);
                    break;
                case Scheduler.TaskId.EliteFight:
                    goWarSetup(Scheduler.TaskId.EliteFight, idx, 1, 5, true, -1, null);
                    break;
                case Scheduler.TaskId.Patrol:
                    goWarSetup(Scheduler.TaskId.Patrol, idx, 1, 5, true, 3, "預留");
                    break;
                case Scheduler.TaskId.GrassArrow:
                    goWarSetup(Scheduler.TaskId.GrassArrow, idx, 1, 3, true, 2, "諸葛亮");
                    break;
                case Scheduler.TaskId.BossWar:
                    if (idx == 0)
                    {
                        goWarSetup(Scheduler.TaskId.BossWar, 0, 1, 5, true, -1, null);
                    } else
                    {
                        MessageBox.Show(taskName + " 不支援後備部隊設定");
                    }
                    break;
                default:
                    MessageBox.Show(taskName + " 不支援作戰部隊設定");
                    break;
            }
        }

        private void goWarSetup(string taskId, int idx, int minHeros, int maxHeros, bool reqChief, int fixHero, string fixHeroName)
        {
            if (oGA == null) return;
            WarSetup.goSetup(oGA, taskId, idx, minHeros, maxHeros, reqChief, fixHero, fixHeroName, Window.GetWindow(this));
        }



    }
}
