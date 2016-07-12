using KingsLib.data;
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

namespace SmartKings.ui.win
{
    /// <summary>
    /// Interaction logic for WinAccTaskConfig.xaml
    /// </summary>
    public partial class WinAccTaskConfig : Window
    {

        GameAccount oGA;

        public WinAccTaskConfig(GameAccount oGA)
        {
            InitializeComponent();
            Setup(oGA);
        }

        public void Setup(GameAccount oGA)
        {
            this.oGA = oGA;
            this.Title = string.Format("{0} 掛機設定", oGA.displayName);
            lvAutoTaskInfo.Setup(oGA);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            lvAutoTaskInfo.SaveSettings();
        }

        private void btnClearRecord_Click(object sender, RoutedEventArgs e)
        {
            lvAutoTaskInfo.clearRecord();
        }

        private void btnResetSchedule_Click(object sender, RoutedEventArgs e)
        {
            lvAutoTaskInfo.resetSchedule();
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            lvAutoTaskInfo.updateSelection(true);
        }

        private void btnSelectNone_Click(object sender, RoutedEventArgs e)
        {
            lvAutoTaskInfo.updateSelection(false);
        }

        private void btnSelectSafe_Click(object sender, RoutedEventArgs e)
        {
            lvAutoTaskInfo.selectSafe();
        }

        private void btnSelectSuggested_Click(object sender, RoutedEventArgs e)
        {
            lvAutoTaskInfo.selectSuggested();
        }

        private void btTaskSetup_Click(object sender, RoutedEventArgs e)
        {
            lvAutoTaskInfo.taskSetup();
        }

        private void btTaskWar0_Click(object sender, RoutedEventArgs e)
        {
            lvAutoTaskInfo.warSetup(0);
        }

        private void btTaskWar1_Click(object sender, RoutedEventArgs e)
        {
            lvAutoTaskInfo.warSetup(1);
        }


    }
}
