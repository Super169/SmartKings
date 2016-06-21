using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartKings.ui
{
    /// <summary>
    /// Interaction logic for UcWarSettings.xaml
    /// </summary>
    public partial class UcWarSettings : UserControl
    {

        GameAccount oGA;
        int minHero = 1;
        int maxHero = 5;
        bool reqChief = true;

        private static int[,] warPos = { { -5, -1 }, { -3, -1 }, { -6, 0 }, { -4, 0 }, { -2, 0 }, { -5, 1 }, { -3, 1 } };


        public event EventHandler<dynamic> Save;
        public event EventHandler Cancel;

        UcWarHero[] warHeros = new UcWarHero[7];
        UcWarHero selectedWH = null;

        public void init(GameAccount oGA, dynamic json)
        {
            this.oGA = oGA;
            lvHero.ItemsSource = oGA.heros;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvHero.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("power", ListSortDirection.Descending));

            for (int i = 0; i < 7; i++)
            {
                warHeros[i].Reset();
            }

            if (json == null) return;
            if (!JSON.exists(json, "heros", typeof(DynamicJsonArray))) return;
            int chief = JSON.getInt(json, "chief");
            DynamicJsonArray heros = json["heros"];

            foreach (dynamic hero in heros)
            {
                int pos = getPos(JSON.getInt(hero, "x"), JSON.getInt(hero, "y"));
                int heroIdx = JSON.getInt(hero, "index");
                if ((heroIdx > 0) && (pos >= 0))
                {
                    HeroInfo hi = oGA.heros.SingleOrDefault(x => x.idx == heroIdx);
                    if (hi != null)
                    {
                        warHeros[pos].SetHero(heroIdx, hi.nm, hi.lv, hi.power, hi.cfd, hi.spd);
                        if (heroIdx == chief) warHeros[pos].SetChief(true);
                    }

                }
            }


            /*
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

        private int getPos(int x, int y)
        {
            int pos = -1;
            for (int idx = 0; idx < 7; idx++)
            {
                if ((warPos[idx, 0] == x) && (warPos[idx, 1] == y))
                {
                    pos = idx;
                    break;
                }
            }
            return pos;
        }

        public UcWarSettings()
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
            UcWarHero wh = (UcWarHero)sender;
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

        private void lvHero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedWH == null) return;
            HeroInfo hi = (HeroInfo)lvHero.SelectedItem;
            if (hi == null) return;

            int heroCnt = 0;

            foreach (UcWarHero wh in warHeros)
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
            foreach (UcWarHero wh in warHeros) wh.SetChief(false);
            selectedWH.SetChief(true);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if ((selectedWH == null) || (selectedWH.IsEmpty())) return;
            selectedWH.Reset();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
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

            // The JSON to build the body string
            dynamic json = JSON.Empty;
            List<dynamic> heros = new List<dynamic>();

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
            string body = JSON.encode(json);
            if (Save != null) Save(this, json);

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel != null) Cancel(this, EventArgs.Empty);
        }

    }
}
