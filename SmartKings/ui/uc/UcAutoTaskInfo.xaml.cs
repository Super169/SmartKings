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
                } else
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
                        remark = at.schedule.getScheduleInfo()
                    };
                }
                atis.Add(ati);
            }
            lvAutoTaskInfo.ItemsSource = atis;
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
    }
}
