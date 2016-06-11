using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Data;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        private void loadAppSettings()
        {
            AppSettings.restoreSettings();
            ucAppSettings.UpdateStatus += new EventHandler<string>(ucAppSetting_UpdateStatus);
            ucAppSettings.loadAppSettings();
        }

        private void ucAppSetting_UpdateStatus(object sender, string status)
        {
            UpdateInfo("***", "系統設定", status);
        }
    }
}
