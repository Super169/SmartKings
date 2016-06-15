using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.scheduler
{
    public class KingsTask
    {
        public delegate bool DelegateExecuteTask(GameAccount oGA, action.DelegateUpdateInfo updateInfo, bool debug);

        public class TaskInfo
        {
            public string id;
            public bool isEnabled;
            public DelegateExecuteTask executeTask;
        }

        public static List<TaskInfo> systemTasks;

        public static void intiSystemTasks()
        {
            systemTasks = new List<TaskInfo>();
            systemTasks.Add(new TaskInfo() { id = TaskId.Harvest, isEnabled = true, executeTask = action.task.goTaskHarvest });
        }

        public static class TaskId
        {
            public const string SignIn = "SignIn";
            public const string Travel = "Travel";
            public const string Reload = "Reload";
            public const string Harvest = "Harvest";
        }

        public enum TaskType
        {
            System, User, Unknown
        }

        public static class KEY
        {
            public const string id = "id";
            public const string type = "type";
            public const string isEnabled = "isEnabled";
            public const string parameters = "parameters";
        }

        /*
                public class TaskInfo : data.InfoBase
                {
                    public string id;
                    public TaskType type;
                    public bool isEnabled;
                    public dynamic parameters;

                    public override void initObject()
                    {
                        this.id = null;
                        this.type = TaskType.Unknown;
                        this.isEnabled = false;
                        this.parameters = null;
                    }

                    public override bool fromJson(dynamic json)
                    {
                        this.initObject();
                        this.id = JSON.getString(json[KEY.id]);
                        if ((this.id == null) || (this.id == "")) return false;
                        this.type = JSON.getString(json[KEY.type]);
                        this.isEnabled = JSON.getBool(json[KEY.isEnabled]);
                        this.parameters = json[KEY.parameters];
                        return true;
                    }

                    public override dynamic toJson()
                    {
                        dynamic json = JSON.Empty;
                        json[KEY.id] = this.id;
                        json[KEY.type] = this.type;
                        json[KEY.isEnabled] = this.isEnabled;
                        json[KEY.parameters] = this.parameters;
                        return json;
                    }

                    public TaskInfo()
                    {
                        this.initObject();
                    }

                    public TaskInfo(string jsonString)
                    {
                        this.initObject();
                        this.fromJsonString(jsonString);
                    }

                }
        */

    }
}
