
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
            this.initObject();
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
    }
}
