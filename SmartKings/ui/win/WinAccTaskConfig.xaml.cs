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
    }
}
