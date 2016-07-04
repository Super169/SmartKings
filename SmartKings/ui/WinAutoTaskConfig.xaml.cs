using KingsLib.scheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            txtElapseMin.Text = AppSettings.elapseMin.ToString();
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
            }
            lvTask.ItemsSource = Scheduler.autoTaskList;
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

        private void txtElapseMin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            int elapseMin = 0;
            bool ready = false;
            try
            {
                elapseMin = Int32.Parse(txtElapseMin.Text);
                ready = (elapseMin > 0);
            }
            catch { }
            if (!ready)
            {
                MessageBox.Show("執行間距不正確, 必須為 1-99 的數值.");
                e.Cancel = true;
            }
            AppSettings.elapseMin = elapseMin;
            AppSettings.saveSettings();
        }

        private void btnSelectSafe_Click(object sender, RoutedEventArgs e)
        {
            foreach (Scheduler.KingsTask t in Scheduler.autoTaskList)
            {
                t.isEnabled = (t.suggestion == 1);
            }
            lvTask.ItemsSource = Scheduler.autoTaskList;
            RefreshList();
        }

        private void btnSelectSuggested_Click(object sender, RoutedEventArgs e)
        {
            foreach (Scheduler.KingsTask t in Scheduler.autoTaskList)
            {
                t.isEnabled = ((t.suggestion == 1) || (t.suggestion == 2));
            }
            lvTask.ItemsSource = Scheduler.autoTaskList;
            RefreshList();
        }
    }
}
