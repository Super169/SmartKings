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
        private static class KEY
        {
            public const string taskId = "taskId";
            public const string schedule = "schedule";
            public const string task = "task";
        }

        public Schedule.ScheduleInfo schedule  { get; set; }
        public Task.TaskInfo task { get; set; }
        public string taskId { get { return task.id;  } }

        public Scheduler()
        {
            initObject();
        }

        public Scheduler(string jsonString)
        {
            initObject();
            this.fromJsonString(jsonString);
        }

        public Scheduler(GFR.GenericFileRecord gfr)
        {
            this.initObject();
            if (gfr == null) return;
            if ((gfr.key == null) || (gfr.key == "")) return;
            this.schedule = new Schedule.ScheduleInfo(JSON.getString(gfr.getObject(KEY.schedule)));
            this.task = new Task.TaskInfo(JSON.getString(gfr.getObject(KEY.task)));
        }

        public Scheduler(dynamic json)
        {
            initObject();
            this.fromJson(json);
        }

        public override void initObject()
        {
            this.schedule = new Schedule.ScheduleInfo();
            this.task = new Task.TaskInfo();
        }

        public override bool fromJson(dynamic json)
        {
            if (json == null) return false;
            try
            {
                this.schedule = new Schedule.ScheduleInfo(JSON.getString(json, KEY.schedule, null));
                this.task = new Task.TaskInfo(JSON.getString(json, KEY.task, null));

            }
            catch {
                this.initObject();
                return false;
            }
            return true;
        }

        public override dynamic toJson()
        {
            dynamic json = JSON.Empty;
            try
            {
                json["schedule"] = this.schedule.toJsonString();
            } catch
            {
                json = JSON.Empty;
            }
            return json;
        }

        public GFR.GenericFileRecord ToGFR()
        {
            GFR.GenericFileRecord gfr = new GFR.GenericFileRecord(this.taskId);
            gfr.saveObject(KEY.taskId, this.taskId);
            gfr.saveObject(KEY.schedule, this.schedule.toJsonString());
            gfr.saveObject(KEY.task, this.task.toJsonString());
            return gfr;
        }


        public void verifySystemTask(ref List<Scheduler> systemTasks)
        {
            string[] validId = { Task.TaskId.SignIn, Task.TaskId.Harvest, Task.TaskId.Reload};
            foreach (Scheduler oS in systemTasks)
            {
                string id = validId.FirstOrDefault(x => x == oS.taskId);
                if (id == null) systemTasks.Remove(oS);
            }

            foreach (string id in validId)
            {
                addTask(ref systemTasks, id);
            }


        }

        public void addTask(ref List<Scheduler> list, string id) 
        {
            if ((id == null) || (id == "")) return;
            if (list.FindIndex(x => x.taskId == id) >= 0) return;

            Scheduler oS = new Scheduler();
            oS.task.id = id;

            switch (id)
            {
                case Task.TaskId.SignIn:
                    oS.schedule.executionTimes.Add(new TimeSpan(06, 05, 00));
                    oS.schedule.executionTimes.Add(new TimeSpan(10, 05, 00));
                    oS.schedule.executionTimes.Add(new TimeSpan(13, 05, 00));
                    break;
                case Task.TaskId.Harvest:
                    oS.schedule.elapseMin = 60;
                    break;
            }

        }
    }
}
