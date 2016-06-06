using KingsLib.data;
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
        private void btnTeter_Click(object sender, RoutedEventArgs e)
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;
            RequestReturnObject rro = KingsLib.request.System.ping(oGA.connectionInfo, oGA.sid);
            UpdateInfo(oGA.serverCode, "測試", rro.requestText);
            Console.WriteLine("Done");
        }




    }
}
