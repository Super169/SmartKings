using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
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

namespace SmartKings.ui
{
    /// <summary>
    /// Interaction logic for WinWarSettings.xaml
    /// </summary>
    public partial class WinWarSettings : Window
    {
        public delegate void DelSaveSettingHandler(GameAccount oGA, string taskId, int idx, dynamic json);
        public event DelSaveSettingHandler saveSettingHandler;

        GameAccount oGA;
        string taskId;
        int idx;
        public WinWarSettings()
        {
            InitializeComponent();
        }

        public WinWarSettings(GameAccount oGA, string taskId, int idx, dynamic json, int minHeros, int maxHeros, bool reqChief)
        {
            InitializeComponent();
            this.init(oGA, taskId, idx, json, minHeros,  maxHeros,  reqChief);
        }

        public void init(GameAccount oGA, string taskId, int idx, dynamic json, int minHeros, int maxHeros, bool reqChief)
        {
            this.oGA = oGA;
            this.taskId = taskId;
            this.idx = idx;
            this.Title = Scheduler.getTaskName(taskId) + " 佈陣設定";
            warSettings.init(oGA, json, minHeros, maxHeros, reqChief);

        }

        public void setFixedHero(int pos, string nm)
        {
            warSettings.setFixedHero(pos, nm);
        }

        private void CancelSettingsHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveSettingHandler(object sender, object e)
        {
            if (saveSettingHandler != null) saveSettingHandler(oGA, taskId, idx, (dynamic)e);

            this.Close();
        }
    }
}
