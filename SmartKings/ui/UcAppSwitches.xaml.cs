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
    /// Interaction logic for UcAppSwitches.xaml
    /// </summary>
    public partial class UcAppSwitches : UserControl
    {

        // public event EventHandler<string> UpdateStatus;

        public UcAppSwitches()
        {
            InitializeComponent();
        }

        public void loadAppSwitches()
        {
            cbxAutoRun.IsChecked = AppSettings.AutoRun;
            cbxDebug.IsChecked = AppSettings.DEBUG;
        }

        private void saveAppSwitches()
        {
            AppSettings.saveSettings();
        }

        private void cbxAutoRun_Changed(object sender, RoutedEventArgs e)
        {
            AppSettings.AutoRun = (cbxAutoRun.IsChecked == true);
            saveAppSwitches();
        }

        private void cbxDebug_Changed(object sender, RoutedEventArgs e)
        {
            AppSettings.DEBUG = (cbxDebug.IsChecked == true);
            saveAppSwitches();
        }

    }
}
