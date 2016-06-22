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
        int minHeros = 1;
        int maxHeros = 5;
        bool reqChief = true;

        private static int[,] warPos = { { -5, -1 }, { -3, -1 }, { -6, 0 }, { -4, 0 }, { -2, 0 }, { -5, 1 }, { -3, 1 } };


        public event EventHandler<dynamic> Save;
        public event EventHandler Cancel;

        UcWarHero[] warHeros = new UcWarHero[7];
        UcWarHero selectedWH = null;

        public void init(GameAccount oGA, dynamic json, int minHeros, int maxHeros, bool reqChief)
        {
            this.oGA = oGA;
            lvHero.ItemsSource = oGA.heros;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvHero.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("power", ListSortDirection.Descending));
            this.minHeros = minHeros;
            this.maxHeros = maxHeros;
            this.reqChief = reqChief;
            btnChief.IsEnabled = reqChief;

            for (int i = 0; i < 7; i++)
            {
                warHeros[i].Reset();
            }

            if (json == null) return;
            if (!JSON.exists(json, "heros", typeof(DynamicJsonArray))) return;
            int chief = 0;
            if (reqChief) chief = JSON.getInt(json, "chief");
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
                        if (reqChief && (heroIdx == chief)) warHeros[pos].SetChief(true);
                    }

                }
            }
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

            if ((heroCnt >= maxHeros) && (selectedWH.heroIdx == 0))
            {
                MessageBox.Show(string.Format("最多只可派 {0} 名英雄出戰", maxHeros));
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

            if (heroCnt < minHeros)
            {
                MessageBox.Show(string.Format("請派遣最少 {0} 名英雄出戰", minHeros));
                return;
            }

            if (reqChief && (chiefIdx == -1))
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
            if (reqChief) json["chief"] = warHeros[chiefIdx].heroIdx;
            json["heros"] = heros;
            // A little bit tricky way to synchronize the type of object after restore.
            // Just convert to string then back to json.
            // It may waste time, but it's safe.
            string body = JSON.encode(json);
            json = JSON.decode(body);
            if (Save != null) Save(this, json);

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Cancel != null) Cancel(this, EventArgs.Empty);
        }

    }
}
