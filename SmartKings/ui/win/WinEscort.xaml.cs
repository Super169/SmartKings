﻿using KingsLib;
using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using static KingsLib.action;

namespace SmartKings.ui.win
{
    /// <summary>
    /// Interaction logic for WinEscort.xaml
    /// </summary>
    public partial class WinEscort : Window
    {
        private class Escort
        {
            public int id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int loot { get; set; }
            public string defend { get; set; }
        }

        private GameAccount oGA;
        private List<Escort> escorts = new List<Escort>();

        public WinEscort(GameAccount oGA)
        {
            InitializeComponent();
            this.oGA = oGA;
            this.Title = oGA.displayName + " : 鏢行四海";
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RequestReturnObject rro;

            rro = KingsLib.request.Escort.worldInfo(oGA.connectionInfo, oGA.sid);
            if (!rro.success) return;
            if (!rro.SuccessWithJson(RRO.Escort.list, typeof(DynamicJsonArray))) return;
            DynamicJsonArray list = rro.responseJson[RRO.Escort.list];
            escorts.Clear();
            foreach (dynamic o in list)
            {
                int id = JSON.getInt(o, RRO.Escort.id);
                string name = JSON.getString(o, RRO.Escort.a,"");
                int loot = JSON.getInt(o, RRO.Escort.c);
                long xe = JSON.getLong(o, RRO.Escort.e);
                long xf = JSON.getLong(o, RRO.Escort.f);
                int diff = JSON.getInt(o, RRO.Escort.f) - JSON.getInt(o, RRO.Escort.e);
                string type = "";
                switch (diff)
                {
                    case 600:
                        type = "金";
                        break;
                    case 900:
                        type = "紫";
                        break;
                    case 1200:
                        type = "藍";
                        break;
                    case 1800:
                        type = "綠";
                        break;
                    default:
                        type = "??";
                        break;
                }

                string heros = "";
                rro = KingsLib.request.Escort.getDefFormation(oGA.connectionInfo, oGA.sid, id);
                if (rro.SuccessWithJson(RRO.Escort.displayHero, typeof(DynamicJsonArray)))
                {
                    DynamicJsonArray displayHero = rro.responseJson[RRO.Escort.displayHero];
                    bool first = true;
                    foreach (dynamic hero in displayHero)
                    {
                        if (!first) heros += ", ";
                        heros += string.Format("{0}{1}[{2}]", 
                                               (JSON.getString(hero, RRO.Escort.sta,"") == "STR" ? "神" : ""), 
                                               JSON.getString(hero, RRO.Escort.nm,"??"), JSON.getInt(hero, RRO.Escort.lv));
                        first = false;
                    }
                    escorts.Add(new Escort() { id = id, name = name, type = type, loot = loot, defend = heros });
                }
                else
                {
                    LOG.E(oGA.displayName, "鏢行四海", xf.ToString());
                }
                Thread.Sleep(100);
            }

            lvEscort.ItemsSource = escorts;
        }

        private void btnLoot_Click(object sender, RoutedEventArgs e)
        {
            Escort es = (Escort)lvEscort.SelectedItem;
            if (es == null) return;
            WarInfo wi = oGA.getWarInfo(Scheduler.TaskId.BossWar, 0);
            if (wi == null) return;
            if ((wi.body == null) || (wi.body == "")) return;
            action.Escort.goLoot(oGA, es.id, wi.body);
        }
    }
}
