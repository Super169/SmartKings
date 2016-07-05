﻿using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.scheduler
{
    public partial class Scheduler
    {
        public const int DAY_START_HOUR = 5;
        public const int DAY_START_ADJ_MIN = 5;

        public static DateTime getRefTime(TimeSpan refTS)
        {
            return getRefTime(DateTime.Now, refTS);
        }

        public static DateTime getRefTime(DateTime baseTime, TimeSpan refTS)
        {
            DateTime refTime = new DateTime(baseTime.Year, baseTime.Month, baseTime.Day) + refTS;
            if (baseTime.Hour < DAY_START_HOUR)
            {
                if (refTS.Hours >= DAY_START_HOUR) refTime = refTime.AddDays(-1);
            }
            else
            {
                if (refTS.Hours < DAY_START_HOUR) refTime = refTime.AddDays(1);
            }
            return refTime;
        }

        public static int getGameDOW()
        {
            return getGameDOW(DateTime.Now);
        }

        public static int getGameDOW(DateTime baseTime)
        {
            DateTime gameDate = getRefTime(baseTime, new TimeSpan(12, 00, 00));
            return (int)gameDate.DayOfWeek;
        }


        public static bool bossTime()
        {
            // For quick checking, just use PC close, no need to convert refTime
            DateTime now = DateTime.Now;
            int dow = (int) now.DayOfWeek;
            if ((dow != 5) && (dow != 0)) return false;
            if (now.AddMinutes(1).Hour < 20) return false;
            if (now.AddMinutes(-30).Hour >= 20) return false;
            return true;
        }

        public class ScheduleInfo : data.InfoBase
        {
 
            private static class KEY
            {
                public const string dow = "dow";
                public const string startTime = "startTime";
                public const string endTime = "endTime";
                public const string elapseMin = "elapseMin";
                public const string executionTimes = "executionTimes";
                public const string maxRetry = "maxRetry";
                public const string retryFreqMin = "retryFreqMin";
                public const string lastExecutionTime = "lastExecutionTime";
                public const string retryCnt = "retryCnt";
                public const string nextExecutionTime = "nextExecutionTime";
            }

            public List<int> dow { get; set; }
            public TimeSpan? startTime { get; set; }
            public TimeSpan? endTime { get; set; }
            public int elapseMin { get; set; }
            public List<TimeSpan> executionTimes { get; set; }
            public int maxRetry { get; set; }
            public int retryFreqMin { get; set; }
            public DateTime? lastExecutionTime;
            public int retryCnt { get; set; }
            public DateTime? nextExecutionTime;

            public string getScheduleInfo()
            {
                StringBuilder sb = new StringBuilder();
                if (dow.Count == 0)
                {
                    sb.Append("每天:");
                } else
                {
                    sb.Append("星期 ");
                    foreach (int i in dow)
                    {
                        string[] dName = { "日","一","二","三","四","五","六", "日" };
                        sb.Append(dName[i]);
                    }
                    sb.Append(":");
                }
                if (this.startTime != null)
                {
                    if (this.endTime != null)
                    {
                        sb.Append(string.Format(" {0:HH:mm} 至 {1:HH:mm}", this.startTime, this.endTime));
                    } else
                    {
                        sb.Append(string.Format(" {0:HH:mm} 之後", this.startTime));
                    }
                } else if (this.endTime != null)
                {
                    sb.Append(string.Format(" {0:HH:mm} 之前", this.startTime));
                }
                if (this.elapseMin > 0)
                {
                    sb.Append(string.Format(" 每 {0} 分鐘一次", this.elapseMin));
                } else if (executionTimes.Count > 0)
                {
                    sb.Append(" 在");
                    foreach (TimeSpan ts in this.executionTimes)
                    {
                        sb.Append(string.Format(" [{0:hh\\:mm}]", ts));
                    }
                    sb.Append(" 執行");
                } else
                {
                    sb.Append(" 執行一次");
                }
                return sb.ToString();
            }

            public ScheduleInfo()
            {
                this.initObject();
            }

            public ScheduleInfo(string jsonString)
            {
                this.initObject();
                this.fromJsonString(jsonString);
            }

            public ScheduleInfo(dynamic json)
            {
                this.initObject();
                this.fromJson(json);
            }

            public override void initObject()
            {
                dow = new List<int>();
                startTime = null;
                endTime = null;
                elapseMin = -1;
                executionTimes = new List<TimeSpan>();
                lastExecutionTime = null;
                retryCnt = 0;
                nextExecutionTime = null;
            }

            public override bool fromJson(dynamic json)
            {
                this.initObject();
                dow = JSON.getIntList(json, KEY.dow);
                startTime = JSON.getTimeSpanX(json, KEY.startTime);
                endTime = JSON.getTimeSpanX(json, KEY.endTime);
                elapseMin = JSON.getInt(json, KEY.elapseMin);
                executionTimes = JSON.getTimeSpanList(json, KEY.executionTimes);
                maxRetry = JSON.getInt(json, KEY.maxRetry);
                retryFreqMin = JSON.getInt(json, KEY.retryFreqMin);
                lastExecutionTime = JSON.getDateTimeX(json, KEY.lastExecutionTime);
                retryCnt = JSON.getInt(json, KEY.retryCnt);
                nextExecutionTime = JSON.getDateTimeX(json, KEY.nextExecutionTime);
                return true;
            }

            public override dynamic toJson()
            {
                dynamic json = JSON.Empty;
                json[KEY.dow] = dow;
                json[KEY.startTime] = startTime;
                json[KEY.endTime] = endTime;
                json[KEY.elapseMin] = elapseMin;
                json[KEY.executionTimes] = executionTimes;
                json[KEY.maxRetry] = maxRetry;
                json[KEY.retryFreqMin] = retryFreqMin;
                json[KEY.lastExecutionTime] = lastExecutionTime;
                json[KEY.retryCnt] = retryCnt;
                json[KEY.nextExecutionTime] = nextExecutionTime;
                return json;
            }

            public DateTime getStartTime(DateTime baseTime)
            {
                TimeSpan refTS = (startTime == null ? new TimeSpan(DAY_START_HOUR, DAY_START_ADJ_MIN, 0) : startTime.GetValueOrDefault());
                return getRefTime(baseTime, refTS);
            }

            public DateTime getEndTime(DateTime baseTime)
            {
                TimeSpan refTS = (endTime == null ? new TimeSpan(DAY_START_HOUR, 0, 0).Subtract(new TimeSpan(0, DAY_START_ADJ_MIN, 0)) : endTime.GetValueOrDefault());
                return getRefTime(baseTime, refTS);
            }

            public bool startTimeOK(DateTime chkTime)
            {
                return (startTimeOK(chkTime, chkTime));
            }

            public bool startTimeOK(DateTime baseTime, DateTime chkTime)
            {
                return (chkTime > getStartTime(baseTime));
            }

            public bool endTimeOK(DateTime chkTime)
            {
                return (endTimeOK(chkTime, chkTime));
            }

            public bool endTimeOK(DateTime baseTime, DateTime chkTime)
            {
                return (chkTime < getEndTime(baseTime));
            }

            public bool matchDOW(DateTime chkTime)
            {
                if (dow.Count == 0) return true;
                int gameDow = getGameDOW(chkTime);
                return (dow.FindIndex(x => x == gameDow) >= 0);
            }

            public bool matchTime(DateTime chkTime)
            {
                return (startTimeOK(chkTime) && endTimeOK(chkTime));
            }

            public bool validActionTime(DateTime chkTime)
            {
                return (matchDOW(chkTime) && matchTime(chkTime));
            }

            public bool readyToGo()
            {
                DateTime now = DateTime.Now;
                if (now < this.nextExecutionTime) return false;
                return (validActionTime(now));
            }

            private void sortExecutionTimes()
            {
                if (this.executionTimes.Count < 2) return;
                this.executionTimes.Sort();
            }

            public void initNextTime()
            {
                this.nextExecutionTime = getNextTime();
            }

            public void setNextTime(bool success)
            {
                setNextTime(success, DateTime.Now);
            }

            public void setNextTime(bool success, DateTime executionTime)
            {
                this.lastExecutionTime = executionTime;
                if (!success)
                {
                    if (this.retryCnt < this.maxRetry)
                    {
                        this.retryCnt++;
                        DateTime nextTime = executionTime.AddMinutes(retryFreqMin);
                        if (nextTime <= getEndTime(executionTime))
                        {
                            this.nextExecutionTime = nextTime;
                            return;
                        }
                    }
                    else
                    {
                        // may prompt message here for fail to retry
                    }
                }
                // Either success, or this execution cannot be retried.
                this.nextExecutionTime = getNextTime();
            }

            // Get the next time for new start or after a successful run
            public DateTime getNextTime()
            {
                DateTime nextTime = DateTime.Now;
                DateTime chkTime;

                bool lastOnToday = false;
                if (this.lastExecutionTime != null)
                {
                    DateTime refTimeLast = getRefTime(this.lastExecutionTime.GetValueOrDefault(), new TimeSpan(12, 0, 0));
                    DateTime refTimeNow = getRefTime(nextTime, new TimeSpan(12, 0, 0));
                    lastOnToday = (refTimeLast == refTimeNow);
                }
                bool sameDay = true;
                bool searchNextDate = true;
                while (searchNextDate)
                {
                    if (matchDOW(nextTime) &&
                       (!sameDay || (endTime == null) || (nextTime < getRefTime(nextTime, this.endTime.GetValueOrDefault()))))
                    {
                        if (this.elapseMin > 0)
                        {
                            if (sameDay && lastOnToday)
                            {
                                chkTime = lastExecutionTime.GetValueOrDefault().AddMinutes(this.elapseMin);
                                if (chkTime < getEndTime(nextTime))
                                {
                                    nextTime = chkTime;
                                    searchNextDate = false;
                                }
                            }
                            else
                            {
                                nextTime = getStartTime(nextTime);
                                searchNextDate = false;
                            }
                            // by elapseMin
                        }
                        else if (this.executionTimes.Count > 0)
                        {
                            // by fixed time slot
                            if (sameDay)
                            {
                                for (int idx = 0; idx < executionTimes.Count; idx++)
                                {
                                    DateTime slotTime = getRefTime(nextTime, executionTimes.ElementAt(idx));
                                    if ((this.lastExecutionTime == null) || (this.lastExecutionTime.GetValueOrDefault() < slotTime))
                                    {
                                        if (slotTime > nextTime)
                                        {
                                            nextTime = slotTime;
                                        }
                                        searchNextDate = false;
                                        break;
                                    }

                                }
                            }
                            else
                            {
                                nextTime = getRefTime(nextTime, executionTimes.ElementAt(0));
                                searchNextDate = false;
                            }

                        }
                        else
                        {
                            // Once a day
                            if (sameDay)
                            {
                                if (!lastOnToday)
                                {
                                    DateTime nextStart = getStartTime(nextTime);
                                    nextTime = (nextTime < nextStart ? nextStart : nextTime);
                                    searchNextDate = false;
                                }
                            }
                            else
                            {
                                nextTime = getStartTime(nextTime);
                                searchNextDate = false;
                            }
                        }
                    }
                    if (searchNextDate)
                    {
                        nextTime = getStartTime(nextTime).AddDays(1);
                        sameDay = false;
                    }
                }
                return nextTime;
            }

        }

    }
}
