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
    /// Interaction logic for UcActionPanel.xaml
    /// </summary>
    public partial class UcActionPanel : UserControl
    {
        public delegate void DelegateActionEventHandler(bool allPlayers, string btnClicked);
        public DelegateActionEventHandler goAction;

        public UcActionPanel()
        {
            InitializeComponent();
        }

        public void setActionHandler(DelegateActionEventHandler actionHandler)
        {
            goAction = actionHandler;
        }

        private void playAction(string action)
        {
            if (goAction != null)
            {
                goAction((cbxAllPlayers.IsChecked == true), action);
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            playAction(btn.Name);
        }
    }
}
