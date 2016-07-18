using KingsLib;
using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
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
            public int basicSetup { get; set; }
            public int war0Setup { get; set; }
            public int war1Setup { get; set; }
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
                        remark = "帳戶沒有該項工作",
                        basicSetup = -1,
                        war0Setup = -1,
                        war1Setup = -1

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
                    ati.basicSetup = getBasicSetupStatus(kt.id);
                    ati.war0Setup = getWarSetupStatus(kt.id, 0);
                    ati.war1Setup = getWarSetupStatus(kt.id, 1);
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

            AutoTaskInfo ati = (AutoTaskInfo)lvAutoTaskInfo.SelectedItem;

            return ati;
        }


        public void clearRecord()
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

            at.schedule = Scheduler.defaultSchedule(at.taskId);
            at.schedule.initNextTime();

            ati.lastExecution = (at.schedule.lastExecutionTime == null ? "--" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", at.schedule.lastExecutionTime));
            ati.nextExecution = (at.schedule.nextExecutionTime == null ? "--" : string.Format("{0:yyyy-MM-dd HH:mm:ss}", at.schedule.nextExecutionTime));

            Scheduler.KingsTask kt = Scheduler.autoTaskList.Find(x => x.id == at.taskId);
            if (kt == null)
            {
                ati.remark = "";
            }
            else
            {
                ati.remark = at.schedule.getScheduleInfo(kt.getNextTime == null);
            }

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
                if (kt == null)
                {
                    at.isEnabled = false;
                }
                else
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


        private int getWarSetupStatus(string taskId, int idx)
        {
            bool checkSetup = false;
            switch (taskId)
            {
                case Scheduler.TaskId.ArenaDefFormation:
                case Scheduler.TaskId.StarryFight:
                case Scheduler.TaskId.NavalWar:
                case Scheduler.TaskId.EliteFight:
                case Scheduler.TaskId.Patrol:
                case Scheduler.TaskId.Travel:
                    checkSetup = true;
                    break;
                case Scheduler.TaskId.GrassArrow:
                case Scheduler.TaskId.BossWar:
                    checkSetup = (idx == 0);
                    break;
            }
            if (!checkSetup) return -1;

            WarInfo wi = oGA.getWarInfo(taskId, idx);
            if (wi == null) return 0;
            return 1;
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
                case Scheduler.TaskId.ArenaDefFormation:
                case Scheduler.TaskId.StarryFight:
                case Scheduler.TaskId.NavalWar:
                case Scheduler.TaskId.Travel:
                case Scheduler.TaskId.EliteFight:
                    goWarSetup(taskId, idx, 1, 5, true, -1, null);
                    break;
                case Scheduler.TaskId.Patrol:
                    goWarSetup(taskId, idx, 1, 5, true, 3, "預留");
                    break;
                case Scheduler.TaskId.GrassArrow:
                    if (idx == 0)
                    {
                        goWarSetup(taskId, idx, 1, 3, true, 2, "諸葛亮");
                    }
                    else
                    {
                        MessageBox.Show(taskName + " 不支援後備部隊設定");
                    }
                    break;
                case Scheduler.TaskId.BossWar:
                    if (idx == 0)
                    {
                        goWarSetup(taskId, 0, 1, 5, true, -1, null);
                    }
                    else
                    {
                        MessageBox.Show(taskName + " 不支援後備部隊設定");
                    }
                    break;
                default:
                    MessageBox.Show(taskName + " 不支援作戰部隊設定");
                    break;
            }
            refreshAll();
        }

        private void goWarSetup(string taskId, int idx, int minHeros, int maxHeros, bool reqChief, int fixHero, string fixHeroName)
        {
            if (oGA == null) return;
            WarSetup.goSetup(oGA, taskId, idx, minHeros, maxHeros, reqChief, fixHero, fixHeroName, Window.GetWindow(this));
        }


        private int getBasicSetupStatus(string taskId)
        {
            switch (taskId)
            {
                case Scheduler.TaskId.TeamDuplicate:
                case Scheduler.TaskId.TrainHero:
                case Scheduler.TaskId.EliteFight:
                    DynamicJsonObject json = oGA.getTaskParmObject(taskId);
                    if (json == null) return 0;
                    if (json.GetDynamicMemberNames().Count() == 0) return 0;
                    return 1;
            }
            return -1;
        }

        public void taskSetup()
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
                case Scheduler.TaskId.TeamDuplicate:
                    goSetupTeamDuplicate();
                    break;
                case Scheduler.TaskId.TrainHero:
                    goSetupTrainHero();
                    break;
                case Scheduler.TaskId.EliteFight:
                    goSetupElite();
                    break;
                case Scheduler.TaskId.StarryFight:
                case Scheduler.TaskId.NavalWar:
                case Scheduler.TaskId.Patrol:
                case Scheduler.TaskId.GrassArrow:
                case Scheduler.TaskId.BossWar:
                default:
                    MessageBox.Show(taskName + " 沒有提供設定項目");
                    break;
            }
        }


        private void goSetupTeamDuplicate()
        {
            if (oGA == null) return;

            ui.setup.WinPickHeros winSetup = new ui.setup.WinPickHeros(oGA, Scheduler.TaskId.TeamDuplicate, Scheduler.Parm.TeamDuplicate.heroIdx, 4);
            // ui.WinSetupTeamDuplicate winSetup = new ui.WinSetupTeamDuplicate();
            winSetup.Owner = Window.GetWindow(this);
            winSetup.ShowDialog();
        }

        private void goSetupTrainHero()
        {
            if (oGA == null) return;

            ui.setup.WinPickHeros winSetup = new ui.setup.WinPickHeros(oGA, Scheduler.TaskId.TrainHero, Scheduler.Parm.TrainHero.targetHeros, 999);
            winSetup.Owner = Window.GetWindow(this);
            winSetup.ShowDialog();
        }

        private void goSetupElite()
        {
            if (oGA == null) return;
            ui.win.WinEliteFightSetup winEFS = new ui.win.WinEliteFightSetup(oGA);
            winEFS.Owner = Window.GetWindow(this);
            winEFS.ShowDialog();
        }
    }
}
