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
    /// Interaction logic for UcAppSettings.xaml
    /// </summary>
    public partial class UcAppSettings : UserControl
    {

        public event EventHandler<string> UpdateStatus;

        public UcAppSettings()
        {
            InitializeComponent();
        }

        public void loadAppSettings()
        {
            txtElapseMin.Text = AppSettings.elapseMin.ToString();
            txtExtraStartMin.Text = AppSettings.extraStartMin.ToString();
        }

        private bool IsNumeric(string data, int max)
        {
            if (data == "") return true;
            if (System.Text.RegularExpressions.Regex.IsMatch(data, "[^0-9]")) return false;
            int value = int.Parse(data);
            if (value > max) return false;
            return true;
        }

        private bool checkNumberInput(TextBox txtInput, int max)
        {
            if (IsNumeric(txtInput.Text, max))
            {
                txtInput.Tag = txtInput.Text;
                return true;
            }
            else
            {
                txtInput.Text = (string) txtInput.Tag;
                txtInput.SelectionStart = 999;
                return false;
            }
        }

        private void cbxAutoRun_Changed(object sender, RoutedEventArgs e)
        {
        }

        private void cbxDebug_Changed(object sender, RoutedEventArgs e)
        {
        }

        private void txtElapseMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkNumberInput(txtElapseMin, 99);
        }

        private void txtExtraStartMin_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkNumberInput(txtExtraStartMin, 99);
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            if (txtElapseMin.Text == "")
            {
                MessageBox.Show("頻率必須填寫");
                return;
            }
            int elapseMin = int.Parse(txtElapseMin.Text);
            if (elapseMin == 0)
            {
                MessageBox.Show("頻率不可為 0");
                return;
            }
            AppSettings.elapseMin = elapseMin;
            AppSettings.extraStartMin = (txtExtraStartMin.Text == "" ? 0 : int.Parse(txtExtraStartMin.Text));
            AppSettings.saveSettings();
            if (this.UpdateStatus != null) this.UpdateStatus(new object(), "設定儲存完成");
        }
    }
}
