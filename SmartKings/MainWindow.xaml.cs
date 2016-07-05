using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KingsLib.monitor;
using static KingsLib.monitor.KingsMonitor;
using KingsLib;
using KingsLib.scheduler;

namespace SmartKings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var version = Assembly.GetEntryAssembly().GetName().Version;
            var buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
            TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
            TimeSpan.TicksPerSecond * 2 * version.Revision)); /* seconds since midnight,
                                                     (multiply by 2 to get original) */
                                                              // this.Title = ((App)Application.Current).winTitle + string.Format("   [{0:yyyy-MM-dd HH:mm}]", buildDateTime);

            this.Title = ((App)Application.Current).winTitle;
            mainInitialization();
        }

    }
}
