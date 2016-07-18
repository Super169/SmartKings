using KingsLib.scheduler;
using System;
using System.Collections.Generic;
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

namespace SmartKings.ui
{
    /// <summary>
    /// Interaction logic for UcActionPanel.xaml
    /// </summary>
    public partial class UcActionPanel : UserControl
    {
        public delegate void DelegateActionEventHandler(bool allPlayers, string btnClicked);
        public DelegateActionEventHandler goAction;

        public class SysTaskEntry
        {
            public string TaskId { get; set; }
            public string TaskName { get; set; }
        }

        List<SysTaskEntry> safeList;
        List<SysTaskEntry> suggestedList;
        List<SysTaskEntry> othersList;

        public UcActionPanel()
        {
            InitializeComponent();
        }

        public void initTaskList()
        {
            safeList = new List<SysTaskEntry>();
            suggestedList = new List<SysTaskEntry>();
            othersList = new List<SysTaskEntry>();
            foreach (Scheduler.KingsTask kt in Scheduler.autoTaskList)
            {
                switch (kt.suggestion)
                {
                    case 1:
                        safeList.Add(new SysTaskEntry() { TaskId = kt.id, TaskName = Scheduler.getTaskName(kt.id) });
                        break;
                    case 2:
                        suggestedList.Add(new SysTaskEntry() { TaskId = kt.id, TaskName = Scheduler.getTaskName(kt.id) });
                        break;
                    default:
                        othersList.Add(new SysTaskEntry() { TaskId = kt.id, TaskName = Scheduler.getTaskName(kt.id) });
                        break;
                }
            }
            cboSafe.ItemsSource = safeList;
            cboSuggested.ItemsSource = suggestedList;
            cboOthers.ItemsSource = othersList;

        }

        public void setActionHandler(DelegateActionEventHandler actionHandler)
        {
            goAction = actionHandler;
        }

        private void playAction(string action)
        {
            if (goAction != null)
            {
                goAction((cbxAllPlayers.IsChecked == true), action);
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            playAction(btn.Name);
        }

        private void btnGoSafe_Click(object sender, RoutedEventArgs e)
        {
            SysTaskEntry ste = (SysTaskEntry)cboSafe.SelectedItem;
            if (ste == null) return;
            playAction("btn" + ste.TaskId);
        }

        private void btnGoSuggested_Click(object sender, RoutedEventArgs e)
        {
            SysTaskEntry ste = (SysTaskEntry)cboSuggested.SelectedItem;
            if (ste == null) return;
            playAction("btn" + ste.TaskId);
        }
        private void btnGoOthers_Click(object sender, RoutedEventArgs e)
        {
            SysTaskEntry ste = (SysTaskEntry)cboOthers.SelectedItem;
            if (ste == null) return;
            playAction(ste.TaskId);
        }

    }
}
