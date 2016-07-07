
using KingsLib.scheduler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib.data
{

    public class WarInfo : InfoBase
    {

        private static class KEY
        {
            public const string account = "account";
            public const string taskId = "taskId";
            public const string idx = "idx";
            public const string body = "body";
            public const string warSetup = "warSetup";
        }

        public string account { get; set; }
        public string taskId { get; set; }
        public int idx { get; set; }
        public string body { get; set; }
        public dynamic warSetup { get; set; }

        public WarInfo()
        {
            // this.initObject();
            // this.warSetup = null;
        }

        public WarInfo(dynamic json)
        {
            this.fromJson(json);
        }

        public override bool fromJson(dynamic json)
        {
            this.initObject();
            this.account = JSON.getString(json, KEY.account, null);
            this.taskId = JSON.getString(json, KEY.taskId, null);
            this.idx = JSON.getInt(json, KEY.idx, 0);
            this.body = JSON.getString(json, KEY.body, null);
            this.warSetup = JSON.recode(json[KEY.warSetup]);
            return true;
        }

        public override void initObject()
        {
            this.account = null;
            this.taskId = null;
            this.idx = 0;
            this.body = null;
            this.warSetup = JSON.Empty;
        }

        public override dynamic toJson()
        {
            dynamic json = JSON.Empty;
            json[KEY.account] = this.account;
            json[KEY.taskId] = this.taskId;
            json[KEY.idx] = this.idx;
            json[KEY.body] = this.body;
            json[KEY.warSetup] = this.warSetup;
            return json;
        }

        public static bool validBody(string jsonString, bool reqChief = true, int minHeros = 1, int maxHeros = 5)
        {
            int[,] warPos = { { -5, -1 }, { -3, -1 }, { -6, 0 }, { -4, 0 }, { -2, 0 }, { -5, 1 }, { -3, 1 } };
            dynamic json = JSON.decode(jsonString);
            if (json == null) return false;
            if (reqChief && !JSON.exists(json, "chief")) return false;
            if (!reqChief && JSON.exists(json, "chief")) return false;
            if (!JSON.exists(json, "heros", typeof(DynamicJsonArray))) return false;
            DynamicJsonArray heros = json["heros"];
            if ((heros.Length < minHeros) || (heros.Length > maxHeros)) return false;

            foreach (dynamic o in heros)
            {
                int pos = -1;
                for (int idx = 0; idx < 7; idx++)
                {
                    if ((warPos[idx, 0] == JSON.getInt(o,"x",-999)) && (warPos[idx, 1] == JSON.getInt(o, "y", -999)))
                    {
                        pos = idx;
                        break;
                    }
                }
                if (pos == -1) return false;
            }
            return true;
        }
    }
}
