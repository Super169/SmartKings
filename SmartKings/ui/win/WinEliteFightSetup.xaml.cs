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

namespace SmartKings.ui.win
{
    /// <summary>
    /// Interaction logic for WinEliteFightSetup.xaml
    /// </summary>
    public partial class WinEliteFightSetup : Window
    {
        GameAccount oGA;

        public WinEliteFightSetup(GameAccount oGA)
        {
            InitializeComponent();
            this.oGA = oGA;
            dynamic json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
            if (json != null)
            {
                int targetChapter = JSON.getInt(json, Scheduler.Parm.EliteFight.targetChapter);
                int targetStage = JSON.getInt(json, Scheduler.Parm.EliteFight.targetStage);
                if ((targetChapter > 0) && (targetStage > 0))
                {
                    ucEliteHero.setSelection(targetChapter, targetStage);
                }

            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            EliteFightInfo efi = ucEliteHero.getSelection();
            dynamic json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
            if (efi == null)
            {
                json[Scheduler.Parm.EliteFight.targetChapter] = null;
                json[Scheduler.Parm.EliteFight.targetStage] = null;
            } else
            {
                json[Scheduler.Parm.EliteFight.targetChapter] = efi.chapter;
                json[Scheduler.Parm.EliteFight.targetStage] = efi.stage;
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
