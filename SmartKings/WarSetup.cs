using KingsLib.data;
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartKings
{
    public static class WarSetup
    {
        private const string parmObjEleName = "WarSetup";

        public static void goSetup(GameAccount oGA, string taskId, int minHeros, int maxHeros, bool reqChief, int fixHero, string fixHeroName, Window owner)
        {
            if (oGA == null) return;

            dynamic parmObj = oGA.getTaskParmObject(taskId);
            dynamic warSetup = null;
            if (JSON.exists(parmObj, parmObjEleName))
            {
                warSetup = parmObj[parmObjEleName];
            }
            ui.WinWarSettings winWarSetup = new ui.WinWarSettings(oGA, taskId, warSetup, minHeros, maxHeros, reqChief);
            winWarSetup.saveSettingHandler += new ui.WinWarSettings.DelSaveSettingHandler(saveWarSetup);
            winWarSetup.Owner = owner;
            if (fixHero >= 0) winWarSetup.setFixedHero(fixHero, fixHeroName);
            bool? dialogResult = winWarSetup.ShowDialog();
        }

        private static  void saveWarSetup(GameAccount oGA, string taskId, dynamic json)
        {
            Scheduler.AutoTask autoTask = oGA.findAutoTask(taskId);
            if (autoTask != null)
            {
                if (autoTask.parmObject == null) autoTask.parmObject = JSON.Empty;
                autoTask.parmObject[parmObjEleName] = json;
                autoTask.parameter = (json == null ? null : JSON.encode(json));
            }
        }


    }
}
