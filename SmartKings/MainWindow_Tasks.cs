using KingsLib.data;
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
        private void goTaskCheckStatus(bool forceCheck = false)
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                oGA.checkStatus(forceCheck);
            }
        }
    }
}
