using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Data;
using MyUtil;
using KingsLib.scheduler;
using System.Web.Helpers;
using KingsLib.data;

namespace SmartKings
{
    public partial class MainWindow : Window
    {

        private void QuickSetup()
        {
            GameAccount oGA = GetSelectedAccount();
            if (oGA == null) return;

            dynamic json;
            dynamic heros;
            List<int> hs = new List<int>();
            switch (oGA.displayName)
            {
                case "S35 超級一六九":
                    json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
                    json[Scheduler.Parm.EliteFight.targetChapter] = 10;
                    json[Scheduler.Parm.EliteFight.targetStage] = 1;

                    // 18 - 吕布; 5 - 吕绮玲; 9 - 周瑜; 42 - 马超; 46 - 孙策
                    heros = JSON.Empty;
                    hs = new List<int>();
                    hs.Add(18);
                    hs.Add(5);
                    hs.Add(9);
                    hs.Add(42);
                    hs.Add(46);
                    heros[Scheduler.Parm.TrainHero.targetHeros] = hs;
                    heros = JSON.recode(heros);
                    json = oGA.getTaskParmObject(Scheduler.TaskId.TrainHero);
                    json[Scheduler.Parm.TrainHero.targetHeros] = heros[Scheduler.Parm.TrainHero.targetHeros];
                    json[Scheduler.Parm.TrainHero.trainSameLevel] = true;
                    break;
                case "S36 無名無姓":
                    break;
                case "S37 怕死的水子遠":
                    json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
                    json[Scheduler.Parm.EliteFight.targetChapter] = 9;
                    json[Scheduler.Parm.EliteFight.targetStage] = 2;
                    break;
                case "S43 自由人":

                    json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
                    json[Scheduler.Parm.EliteFight.targetChapter] = 6;
                    json[Scheduler.Parm.EliteFight.targetStage] = 1;

                    heros = JSON.Empty;
                    hs = new List<int>();
                    hs.Add(5);
                    hs.Add(10);
                    hs.Add(18);
                    hs.Add(3);
                    hs.Add(4);
                    heros[Scheduler.Parm.TrainHero.targetHeros] = hs;
                    heros = JSON.recode(heros);
                    json = oGA.getTaskParmObject(Scheduler.TaskId.TrainHero);
                    json[Scheduler.Parm.TrainHero.targetHeros] = heros[Scheduler.Parm.TrainHero.targetHeros];
                    json[Scheduler.Parm.TrainHero.trainSameLevel] = true;
                    break;
            }
        }


    }
}
