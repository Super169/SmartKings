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

        public static void goSetup(GameAccount oGA, string taskId, int minHeros, int maxHeros, bool reqChief, Window owner)
        {
            if (oGA == null) return;

            dynamic parmObj = oGA.getTaskParmObject(taskId);
            dynamic warSetup = null;
            if (JSON.exists(parmObj, parmObjEleName))
            {
                warSetup = parmObj[parmObjEleName];
            }
            ui.WinWarSettings winStarry = new ui.WinWarSettings(oGA, taskId, warSetup, minHeros, maxHeros, reqChief);
            winStarry.saveSettingHandler += new ui.WinWarSettings.DelSaveSettingHandler(saveWarSetup);
            winStarry.Owner = owner;
            bool? dialogResult = winStarry.ShowDialog();
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
