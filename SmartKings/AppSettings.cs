using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartKings
{
    public static class AppSettings
    {
        private const string jazFileName = "AppSetting.jaz";

        private static class DEFAULT
        {
            public const bool AutoRun = true;
            public const bool DEBUG = false;
            public const int elapseMin = 2;
            public const int extraStartMin = 0;
        }

        private static class KEY
        {
            public const string AutoRun = "AutoRun";
            public const string DEBUG = "DEBUG";
            public const string elapseMin = "elapseMin";
            public const string extraStartMin = "extraStartMin";
        }

        public static bool AutoRun = DEFAULT.AutoRun;
        public static bool DEBUG = DEFAULT.DEBUG;
        public static int elapseMin = DEFAULT.elapseMin;
        public static int extraStartMin = DEFAULT.extraStartMin;

        private static dynamic toJson()
        {
            dynamic json = JSON.Empty;
            json[KEY.AutoRun] = AutoRun;
            json[KEY.DEBUG] = DEBUG;
            json[KEY.elapseMin] = elapseMin;
            json[KEY.extraStartMin] = extraStartMin;
            return json;
        }

        private static void fromJson(dynamic json)
        {
            if (json == null) return;
            AutoRun = JSON.getBool(json, KEY.AutoRun, DEFAULT.AutoRun);
            DEBUG = JSON.getBool(json, KEY.DEBUG, DEFAULT.DEBUG);
            elapseMin = JSON.getInt(json, KEY.elapseMin, DEFAULT.elapseMin);
            extraStartMin = JSON.getInt(json, KEY.extraStartMin, DEFAULT.extraStartMin);
        }

        public static bool saveSettings()
        {
            dynamic json = toJson();
            return JSON.toFile(json, jazFileName);
        }

        public static bool restoreSettings()
        {
            dynamic json = JSON.Empty;
            if (!JSON.fromFile(ref json, jazFileName)) return false;
            fromJson(json);
            return true;
        }

    }
}
