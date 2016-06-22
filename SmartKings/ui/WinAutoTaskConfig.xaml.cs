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
using System.Windows.Shapes;

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
    }
}
