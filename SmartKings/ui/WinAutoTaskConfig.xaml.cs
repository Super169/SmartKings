using KingsLib.scheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SmartKings.ui
{
    /// <summary>
    /// Interaction logic for WinAutoTaskConfig.xaml
    /// </summary>
    public partial class WinAutoTaskConfig : Window
    {
        public WinAutoTaskConfig()
        {
            InitializeComponent();
            lvTask.ItemsSource = Scheduler.autoTaskList;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelection(true);
        }

        private void btnSelectNone_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelection(false);
        }

        private void UpdateSelection(bool isEnabled)
        {
            foreach (Scheduler.KingsTask t in Scheduler.autoTaskList)
            {
                t.isEnabled = isEnabled;
                lvTask.ItemsSource = Scheduler.autoTaskList;
            }
            RefreshList();
        }

        void RefreshList()
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => RefreshList()));
                return;
            }

            ICollectionView view = CollectionViewSource.GetDefaultView(lvTask.ItemsSource);
            view.Refresh();
        }

    }
}
