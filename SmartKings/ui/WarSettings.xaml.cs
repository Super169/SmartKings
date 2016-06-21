﻿using KingsLib.data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartKings.ui
{
    /// <summary>
    /// Interaction logic for WarSettings.xaml
    /// </summary>
    public partial class WarSettings : UserControl
    {
        GameAccount oGA;

        WarHero[] warHeros = new WarHero[7];
        WarHero selectedWH = null;

        public void setData(GameAccount oGA)
        {
/*
            this.oGA = oGA;
            lvHero.ItemsSource = oGA.Heros;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvHero.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("power", ListSortDirection.Descending));
            for (int i = 0; i < 7; i++)
            {
                if (oGA.BossWarHeros[i] == 0)
                {
                    warHeros[i].Reset();
                }
                else
                {
                    HeroInfo hi = oGA.Heros.SingleOrDefault(x => x.idx == oGA.BossWarHeros[i]);
                    if (hi != null)
                    {
                        warHeros[i].SetHero(oGA.BossWarHeros[i], hi.nm, hi.lv, hi.power, hi.cfd, hi.spd);
                    }
                }
                if (oGA.BossWarChiefIdx != -1) warHeros[oGA.BossWarChiefIdx].SetChief(true);
            }
*/
        }

        public WarSettings()
        {
            InitializeComponent();
            warHeros[0] = wh00;
            warHeros[1] = wh01;
            warHeros[2] = wh02;
            warHeros[3] = wh03;
            warHeros[4] = wh04;
            warHeros[5] = wh05;
            warHeros[6] = wh06;
            for (int i = 0; i < 7; i++)
            {
                warHeros[i].id = i;
                warHeros[i].Reset();
            }
        }

        private void warHero_Click(object sender, EventArgs e)
        {
            WarHero wh = (WarHero)sender;
            bool selection = !wh.selected;
            if (selectedWH != null) selectedWH.SetSelected(false);
            wh.SetSelected(selection);
            lvHero.UnselectAll();
            if (selection) selectedWH = wh;
            else selectedWH = null;
        }

        private void setStatus()
        {
            btnChief.IsEnabled = ((selectedWH != null) && (selectedWH.heroIdx > 0));
            btnClear.IsEnabled = (selectedWH != null);
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
/*
            this.DialogResult = false;
            this.Close();
*/
        }

        private void lvHero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedWH == null) return;
            HeroInfo hi = (HeroInfo)lvHero.SelectedItem;
            if (hi == null) return;

            int heroCnt = 0;

            foreach (WarHero wh in warHeros)
            {
                if (wh.heroIdx == hi.idx) wh.Reset();
                if (wh.heroIdx > 0) heroCnt++;
            }

            if ((heroCnt == 5) && (selectedWH.heroIdx == 0))
            {
                MessageBox.Show("最多只可派5名英雄出戰");
            }
            else
            {
                selectedWH.SetHero(hi.idx, hi.nm, hi.lv, hi.power, hi.cfd, hi.spd);
            }
        }

        private void btnChief_Click(object sender, RoutedEventArgs e)
        {
            if ((selectedWH == null) || (selectedWH.chief) || (selectedWH.IsEmpty())) return;
            foreach (WarHero wh in warHeros) wh.SetChief(false);
            selectedWH.SetChief(true);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if ((selectedWH == null) || (selectedWH.IsEmpty())) return;
            selectedWH.Reset();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            /*

            int heroCnt = 0;
            int chiefIdx = -1;
            for (int i = 0; i < 7; i++)
            {
                if (warHeros[i].heroIdx > 0)
                {
                    heroCnt++;
                    if (warHeros[i].chief) chiefIdx = i;
                }
            }

            if (heroCnt == 0)
            {
                MessageBox.Show("請選擇要作戰的英雄");
                return;
            }

            if (chiefIdx == -1)
            {
                MessageBox.Show("請設定一名英雄為主帥");
                return;
            }

            for (int i = 0; i < 7; i++)
            {
                oGA.BossWarHeros[i] = warHeros[i].heroIdx;
            }
            oGA.BossWarChiefIdx = chiefIdx;

            // The JSON to build the body string
            dynamic json = JSON.Empty;
            List<dynamic> heros = new List<dynamic>();

            int[,] warPos = { { -5, -1 }, { -3, -1 }, { -6, 0 }, { -4, 0 }, { -2, 0 }, { -5, 1 }, { -3, 1 } };
            for (int i = 0; i < 7; i++)
            {
                if (warHeros[i].heroIdx > 0)
                {
                    dynamic o = JSON.Empty;
                    o.x = warPos[i, 0];
                    o.y = warPos[i, 1];
                    o.index = warHeros[i].heroIdx;
                    heros.Add(o);
                }
            }
            json["chief"] = warHeros[chiefIdx].heroIdx;
            json["heros"] = heros;
            string body = Json.Encode(json);

            oGA.BossWarBody = body;
            this.DialogResult = true;
            this.Close();

            */
        }


    }
}
