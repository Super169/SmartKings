
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
using KingsLib.data;
using System.ComponentModel;

namespace SmartKings.ui
{
    /// <summary>
    /// Interaction logic for UcHeroList.xaml
    /// </summary>
    public partial class UcHeroList : UserControl
    {

        public event EventHandler SelectionChanged;

        List<HeroInfo> heros;

        public UcHeroList()
        {
            InitializeComponent();
        }

        public void loadHeros(List<HeroInfo> heros, List<int> heroList)
        {
            foreach (HeroInfo hi in heros)
            {
                hi.selected = heroList.Exists(x => x == hi.idx);
            }
            this.heros = heros;
            lvHeros.ItemsSource = this.heros;
            SelectionChanged(this, new EventArgs());
        }

        public string getSelectedName()
        {
            string heroList = "";
            foreach (HeroInfo hi in heros)
            {
                if (hi.selected)
                {
                    heroList += (heroList == "" ? "" : ", ") + hi.nm;
                }
            }
            return heroList;
        }

        public List<int> getSelectedIdx()
        {
            List<int> heroList = new List<int>();
            foreach (HeroInfo hi in heros)
            {
                if (hi.selected) heroList.Add(hi.idx);
            }
            return heroList;
        }

    }
}
