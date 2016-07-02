using KingsLib;
using KingsLib.data;
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

namespace SmartKings.ui.uc
{
    /// <summary>
    /// Interaction logic for UcEliteHero.xaml
    /// </summary>
    /// 

    public partial class UcEliteHero : UserControl
    {
        List<EliteFightInfo> efis = new List<EliteFightInfo>();
        EliteFightInfo lastEfi = null;

        public UcEliteHero()
        {
            InitializeComponent();

            for (int i = 1; i <= util.maxChapter(); i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    efis.Add(new EliteFightInfo(i, j));
                }
            }
            lvHeroList.ItemsSource = efis;
        }

        public bool setSelection(int targetChapter, int targetStage)
        {
            EliteFightInfo efi = efis.Find(x => ((x.chapter == targetChapter) && (x.stage == targetStage)));
            if (efi == null) return false;
            lastEfi = efi;
            efi.selected = true;
            lvHeroList.Items.Refresh();
            txtEliteHero.Text = string.Format("{0} :  {1} - {2}", lastEfi.chapterName, lastEfi.heroName, lastEfi.reward);
            return true;
        }

        public EliteFightInfo getSelection()
        {
            return lastEfi;
        }

        private void HeroUnselected(object sender, RoutedEventArgs e)
        {
            lastEfi = null;
            txtEliteHero.Text = "";
        }

        private void HeroSelected(object sender, RoutedEventArgs e)
        {
            if (lastEfi != null) lastEfi.selected = false;
            Object x = ((System.Windows.FrameworkElement)e.OriginalSource).DataContext;
            if (x.GetType() == typeof(EliteFightInfo))
            {
                lastEfi = (EliteFightInfo)x;
            } else
            {
                foreach (EliteFightInfo o in efis)
                {
                    if (o.selected) lastEfi = o;
                }
            }

            lvHeroList.Items.Refresh();
            txtEliteHero.Text = string.Format("{0} :  {1} - {2}", lastEfi.chapterName, lastEfi.heroName, lastEfi.reward);
        }

}
}
 