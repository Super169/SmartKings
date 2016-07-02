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
                    // 討伐群雄
                    json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
                    json[Scheduler.Parm.EliteFight.targetChapter] = 10;
                    json[Scheduler.Parm.EliteFight.targetStage] = 5;
                    break;
                case "S36 無名無姓":
                    break;

                case "S37 怕死的水子遠":
                    // 討伐群雄
                    json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
                    json[Scheduler.Parm.EliteFight.targetChapter] = 9;
                    json[Scheduler.Parm.EliteFight.targetStage] = 4;
                    break;
                case "S43 自由人":

                    // 討伐群雄
                    json = oGA.getTaskParmObject(Scheduler.TaskId.EliteFight);
                    json[Scheduler.Parm.EliteFight.targetChapter] = 6;
                    json[Scheduler.Parm.EliteFight.targetStage] = 3;

                    break;
            }
        }


    }
}
