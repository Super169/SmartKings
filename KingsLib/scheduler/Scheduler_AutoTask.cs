using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib.scheduler
{
    public partial class Scheduler
    {
        public class AutoTask : InfoBase
        {

            private static class KEY
            {
                public const string taskId = "TaskId";
                public const string isEnabled = "isEnabled";
                public const string parameter = "parameter";
                public const string parmObject = "parmObject";
                public const string schedule = "schedule";
            }

            public string taskId { get; set; }
            public bool isEnabled { get; set; }
            public string parameter { get; set; }
            public dynamic parmObject;
            public ScheduleInfo schedule;

            public AutoTask()
            {
            }

            public AutoTask(string taskId, bool isEnabled, string parameter, ScheduleInfo schedule)
            {
                this.taskId = taskId;
                this.isEnabled = isEnabled;
                this.parameter = parameter;
                this.parmObject = null;
                if (schedule == null) this.schedule = defaultSchedule(taskId);
                else this.schedule = schedule;
                this.schedule.initNextTime();
            }

            public AutoTask(string jsonString)
            {
                fromJson(JSON.decode(jsonString));
            }

            public AutoTask(dynamic json)
            {
                fromJson(json);
            }

            public override void initObject()
            {
                this.taskId = null;
                this.isEnabled = true;
                this.parameter = null;
                this.parmObject = null;
            }

            public override bool fromJson(dynamic json)
            {
                this.taskId = JSON.getString(json, KEY.taskId, null);
                this.isEnabled = JSON.getBool(json, KEY.isEnabled, true);
                this.parameter = JSON.getString(json, KEY.parameter, null);
                string js = JSON.getString(json, KEY.parmObject, null);
                this.parmObject = JSON.decode(js);
                
                js = JSON.getString(json, KEY.schedule, null);
                if (js != null)
                {
                    DynamicJsonObject jSchedule = JSON.decode(js);
                    if (jSchedule.GetDynamicMemberNames().Count() > 0)
                    {
                        this.schedule = new ScheduleInfo(js);
                    }
                }

                return true;
            }

            public override dynamic toJson()
            {
                dynamic json = JSON.Empty;
                json[KEY.taskId] = taskId;
                json[KEY.isEnabled] = isEnabled;
                json[KEY.parameter] = parameter;
                json[KEY.parmObject] = JSON.encode(parmObject);
                json[KEY.schedule] = JSON.encode(schedule.toJson());
                return json;
            }
        }
    }
}
