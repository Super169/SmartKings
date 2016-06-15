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
        public KingsTask.TaskInfo task { get; set; }
        public string taskId { get { return task.id; } }

        public Scheduler()
        {
            initObject();
        }

        public Scheduler(String taskId, KingsTask.TaskType taskType, string taskParameter,
                          List<int> dow, TimeSpan? startTime, TimeSpan? endTime,
                          int elapseMin, List<TimeSpan> executionTimes,
                          int maxRetry, int retryFreqMin)
        {
            this.task = new KingsTask.TaskInfo()
            {
                id = taskId
            };

            this.schedule = new Schedule.ScheduleInfo()
            {
                dow = dow,
                startTime = startTime,
                endTime = endTime,
                elapseMin = elapseMin,
                executionTimes = executionTimes,
                maxRetry = maxRetry,
                retryFreqMin = retryFreqMin
            };

        }

        public Scheduler(KingsTask.TaskInfo taskInfo, Schedule.ScheduleInfo scheduleInfo)
        {
            this.task = taskInfo;
            this.schedule = scheduleInfo;
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
            this.task = new KingsTask.TaskInfo();
        }

        public override bool fromJson(dynamic json)
        {
            if (json == null) return false;
            try
            {
                this.schedule = new Schedule.ScheduleInfo(JSON.getString(json, KEY.schedule, null));
                // this.task = new KingsTask.TaskInfo(JSON.getString(json, KEY.task, null));

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

        public void verifySystemTask(ref List<Scheduler> systemTasks)
        {
            string[] validId = { KingsTask.TaskId.SignIn, KingsTask.TaskId.Harvest, KingsTask.TaskId.Reload};
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
                case KingsTask.TaskId.SignIn:
                    oS.schedule.executionTimes.Add(new TimeSpan(06, 05, 00));
                    oS.schedule.executionTimes.Add(new TimeSpan(10, 05, 00));
                    oS.schedule.executionTimes.Add(new TimeSpan(13, 05, 00));
                    break;
                case KingsTask.TaskId.Harvest:
                    oS.schedule.elapseMin = 60;
                    break;
            }

        }
    }
}
