using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartKings.ui.setup
{
    /// <summary>
    /// Interaction logic for WinPickHeros.xaml
    /// </summary>
    public partial class WinPickHeros : Window
    {
        GameAccount oGA;
        string taskId;
        string parmName;
        int maxHeros;


        public WinPickHeros()
        {
            InitializeComponent();
        }

        public WinPickHeros(GameAccount oGA, string taskId, string parmName, int maxHeros)
        {
            InitializeComponent();
            this.init(oGA, taskId, parmName, maxHeros);
        }

        public void init(GameAccount oGA, string taskId, string parmName, int maxHeros)
        {
            this.oGA = oGA;
            this.taskId = taskId;
            this.parmName = parmName;
            this.maxHeros = maxHeros;
            this.Title = Scheduler.getTaskName(this.taskId) + " 設定";
            List<int> heroList = new List<int>();
            dynamic parmObject = oGA.getTaskParmObject(taskId);
            if (JSON.exists(parmObject, parmName, typeof(DynamicJsonArray)))
            {
                DynamicJsonArray dja = parmObject[parmName];
                foreach (dynamic o in dja)
                {
                    int heroIdx = JSON.getInt(o, -1);
                    if (heroIdx > 0) heroList.Add(heroIdx);
                }
            }

            ucHeroList.loadHeros(oGA.heros, heroList);
        }

        private void ucHeroList_SelectionChanged(object sender, EventArgs e)
        {
            this.txtHeroList.Text = ucHeroList.getSelectedName();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            List<int> heroList = ucHeroList.getSelectedIdx();
            if (heroList.Count == 0)
            {
                MessageBox.Show("請先點選要出戰皇榜的英雄");
                return;
            }
            if (heroList.Count > maxHeros)
            {
                MessageBox.Show(string.Format("選了{0}名英雄, 最多只可點選{1}名英雄", heroList.Count, maxHeros));
                return;
            }

            dynamic json = JSON.Empty;
            json[parmName] = heroList;
            json = JSON.recode(json);
            dynamic parmObject = oGA.getTaskParmObject(taskId);
            parmObject[parmName] = json[parmName];

            this.Close();
        }
    }
}
