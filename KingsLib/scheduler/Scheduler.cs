using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib.scheduler
{
    public class Scheduler : data.InfoBase
    {
        private static class SI_KEY
        {
            public const string schedule = "schedule";
        }

        public Schedule.ScheduleInfo schedule  { get; set; }

        public Scheduler()
        {
            initObject();
        }

        public Scheduler(string jsonString)
        {
            initObject();
            this.fromJsonString(jsonString);
        }

        public Scheduler(dynamic json)
        {
            initObject();
            this.fromJson(json);
        }

        public override void initObject()
        {
            this.schedule = new Schedule.ScheduleInfo();
        }

        public override bool fromJson(dynamic json)
        {
            if (json == null) return false;
            try
            {
                this.schedule = new Schedule.ScheduleInfo(JSON.getString(json, SI_KEY.schedule, null));
            }
            catch (Exception ex) {
                this.initObject();
                return false;
            }
            return true;
        }

        public override dynamic toJson()
        {
            dynamic json = JSON.Empty();
            try
            {
                json["schedule"] = this.schedule.toJsonString();
            } catch
            {
                json = JSON.Empty();
            }
            return json;
        }

    }
}
