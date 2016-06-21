﻿
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Task
    {
        private const string CMD_finishTask = "Task.finishTask";
        private const string CMD_getAchievementInfo = "Task.getAchievementInfo";
        private const string CMD_getTaskTraceInfo = "Task.getTaskTraceInfo";


        public static RequestReturnObject finishTask(ConnectionInfo ci, string sid, int taskId)
        {
            string body = string.Format("{{\"taskId\":{0}}}", taskId);
            return com.SendGenericRequest(ci, sid, CMD_finishTask, true, body);
        }

        public static RequestReturnObject getAchievementInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getAchievementInfo);
        }

        public static RequestReturnObject getTaskTraceInfo(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_getTaskTraceInfo);
        }


    }
}
