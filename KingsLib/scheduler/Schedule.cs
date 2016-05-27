using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib.scheduler
{
    public class Schedule
    {

        public const int DAY_START_HOUR = 5;
        public const int DAY_START_ADJ_MIN = 5;

        public enum FreqType
        {
            DAILY, ELAPSE, WEEKLY
        }

        public class ScheduleInfo : data.InfoBase
        {
            public static int getGameDOW()
            {
                return getGameDOW(DateTime.Now);
            }

            public static int getGameDOW(DateTime baseTime)
            {
                DateTime gameDate = getRefTime(baseTime, new TimeSpan(12, 00, 00));
                return (int)gameDate.DayOfWeek;
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

            private static class SI_KEY
            {
                public const string dow = "dow";
                public const string startTime = "startTime";
                public const string endTime = "endTime";
                public const string elapseMin = "elapseMin";
                public const string executionTimes = "executionTimes";
                public const string lastExecutionTime = "lastExecutionTime";
                public const string lastExecutionFailCount = "lastExecutionFailCount";
                public const string nextExecutionTime = "nextExecutionTime";
            }


            public List<int> dow { get; set; }
            public TimeSpan? startTime { get; set; }
            public TimeSpan? endTime { get; set; }
            public int elapseMin { get; set; }
            public List<TimeSpan> executionTimes { get; set; }
            public DateTime? lastExecutionTime;
            public int lastExecutionFailCount { get; set; }
            public DateTime? nextExecutionTime;

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
                lastExecutionFailCount = 0;
                nextExecutionTime = null;
            }

            public override bool fromJson(dynamic json)
            {
                this.initObject();
                dow = JSON.getIntList(json, SI_KEY.dow);
                startTime = JSON.getTimeSpanX(json, SI_KEY.startTime);
                endTime = JSON.getTimeSpanX(json, SI_KEY.endTime);
                elapseMin = JSON.getInt(json, SI_KEY.elapseMin);
                executionTimes = JSON.getTimeSpanList(json, SI_KEY.executionTimes);
                lastExecutionTime = JSON.getDateTimeX(json, SI_KEY.lastExecutionTime);
                lastExecutionFailCount = JSON.getInt(json, SI_KEY.lastExecutionFailCount);
                nextExecutionTime = JSON.getDateTimeX(json, SI_KEY.nextExecutionTime);
                return true;
            }

            public override dynamic toJson()
            {
                dynamic json = JSON.Empty();
                json[SI_KEY.dow] = dow;
                json[SI_KEY.startTime] = startTime;
                json[SI_KEY.endTime] = endTime;
                json[SI_KEY.elapseMin] = elapseMin;
                json[SI_KEY.executionTimes] = executionTimes;
                json[SI_KEY.lastExecutionTime] = lastExecutionTime;
                json[SI_KEY.lastExecutionFailCount] = lastExecutionFailCount;
                json[SI_KEY.nextExecutionTime] = nextExecutionTime;
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
                int currDow = (int)getStartTime(chkTime).DayOfWeek;
                return (dow.FindIndex(x => x == currDow) >= 0);
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

            public DateTime getNextByElapseTime(DateTime baseTime)
            {
                DateTime nextTime = baseTime.AddMinutes(this.elapseMin);
                if (!endTimeOK(baseTime, nextTime) || !matchDOW(nextTime))
                {
                    nextTime = getStartTime(baseTime).AddDays(1);
                    while (!matchDOW(nextTime))
                    {
                        nextTime = nextTime.AddDays(1);
                    }
                }
                return nextTime;
            }

            public DateTime getNextByFixStartTime(DateTime baseTime)
            {
                bool findOnBase = false;
                bool findMinTime = false;

                // Dummy initialize to due with the checking for usage before assignment
                DateTime nextTime = baseTime;
                DateTime minTime = baseTime;

                foreach (TimeSpan ts in executionTimes)
                {
                    DateTime newTime = getRefTime(baseTime, ts);
                    if (validActionTime(newTime) && (newTime > baseTime))
                    {
                        if (findOnBase)
                        {
                            if (newTime < nextTime) nextTime = newTime;
                        }
                        else
                        {
                            findOnBase = true;
                            nextTime = newTime;
                        }
                    }
                    if (matchTime(newTime))
                    {
                        if (!findMinTime || (newTime < minTime))
                        {
                            findMinTime = true;
                            minTime = newTime;
                        }
                    }
                }
                if (!findOnBase)
                {
                    nextTime = minTime.AddDays(1);
                    while (!matchDOW(nextTime))
                    {
                        nextTime = nextTime.AddDays(1);
                    }
                }
                return nextTime;
            }

            public void setNextExecutionTime()
            {
                DateTime now = DateTime.Now;

                if (lastExecutionTime == null)
                {
                    // set lastExecuteTime on today if it cannot be execute on today,
                    // so that it will search for next time as normal
                    if (!matchDOW(now) || !endTimeOK(now)) lastExecutionTime = now;
                }
                if (lastExecutionTime == null)
                {
                    if (this.elapseMin > 0)
                    {
                        // Set to execute immedicately
                        this.nextExecutionTime = now;
                    }
                    else if (executionTimes.Count() > 0)
                    {
                        DateTime dayStart = getStartTime(now);
                        this.nextExecutionTime = getNextByFixStartTime(dayStart);
                    }
                    else
                    {
                        // Daily, so set to execute immedicately
                        this.nextExecutionTime = now;
                    }
                }
                else
                {
                    if (this.elapseMin > 0)
                    {
                        this.nextExecutionTime = getNextByElapseTime(this.lastExecutionTime.GetValueOrDefault());
                    }
                    else if (executionTimes.Count() > 0)
                    {
                        this.nextExecutionTime = getNextByFixStartTime(this.lastExecutionTime.GetValueOrDefault());
                    }
                    else
                    {
                        DateTime nextTime = getStartTime(this.lastExecutionTime.GetValueOrDefault()).AddDays(1);
                        while (!matchDOW(nextTime)) nextTime = nextTime.AddDays(1);
                        this.nextExecutionTime = nextTime;
                    }
                }
            }

        }




    }
}
