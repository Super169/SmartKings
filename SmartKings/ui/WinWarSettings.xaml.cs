﻿using KingsLib.data;
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
        public delegate void DelSaveSettingHandler(GameAccount oGA, dynamic json);
        public event DelSaveSettingHandler saveSettingHandler;

        GameAccount oGA;
        public WinWarSettings()
        {
            InitializeComponent();
        }

        public WinWarSettings(string title, GameAccount oGA, dynamic json, int minHeros, int maxHeros, bool reqChief)
        {
            InitializeComponent();
            this.init(title, oGA, json, minHeros,  maxHeros,  reqChief);
        }

        public void init(string title, GameAccount oGA, dynamic json, int minHeros, int maxHeros, bool reqChief)
        {
            this.Title = title;
            this.oGA = oGA;
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
            if (saveSettingHandler != null) saveSettingHandler(oGA, (dynamic)e);

            this.Close();
        }
    }
}
