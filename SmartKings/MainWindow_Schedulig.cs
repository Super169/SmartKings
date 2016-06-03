using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartKings
{
    public partial class MainWindow : Window
    {
        List<Scheduler> systemTasks = new List<Scheduler>();
        Object systemTasksLocker = new object();

    }
}
