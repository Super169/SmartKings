
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


    public class GameAccount : InfoBase, IComparable<GameAccount>
    {

        public enum AccountStatus { Online, Offline, Unknown }

        private static class KEY
        {
            public const string account = "account";
            public const string enabled = "enabled";
            public const string sid = "sid";
            public const string status = "status";
            public const string timeAdjust = "timeAdjust";
            public const string server = "server";
            public const string serverTitle = "serverTitle";
            public const string nickName = "nickName";
            public const string corpsName = "corpsName";
            public const string level = "level";
            public const string vipLevel = "vipLevel";
            public const string connectionInfo = "connectionInfo";
            public const string currHeader = "currHeader";
            public const string lastUpdateDTM = "lastUpdateDTM";
            public const string heros = "heros";
            public const string decreeHeros = "decreeHeros";

            /*
                        public const string BossWarHeros = "BossWarHeros";
                        public const string BossWarChiefIdx = "BossWarChiefIdx";
                        public const string BossWarBody = "BossWarBody";
                        public const string BossWarCount = "BossWarCount";
            */
            public const string autoTasks = "autoTasks";

        }

        public bool ready { get; set; } = false;
        public string account { get; set; }
        public bool enabled { get; set; }
        public string sid { get; set; }
        public AccountStatus status { get; set; }
        public string server { get; set; }
        public string serverTitle { get; set; }
        public int pubGameServerId { get; set; }
        public string serverCode { get; set; }
        public int timeAdjust { get; set; }
        public string nickName { get; set; }
        public string corpsName { get; set; }
        public int level { get; set; }
        public int vipLevel { get; set; }
        public ConnectionInfo connectionInfo { get; set; }
        public DateTime lastUpdateDTM { get; set; }
        public List<HeroInfo> heros;
        public List<DecreeInfo> decreeHeros;
        public List<Scheduler.AutoTask> autoTasks;
        public List<WarInfo> warInfos;

        // Boss War Data
        public string bwBody { get; set; }
        public bool bwStarted { get; set; }
        public bool bwEnded { get; set; }
        public int bwFailCnt { get; set; }
        public DateTime bwLastSend { get; set; }

        public string displayName { get { return this.serverCode + " " + this.nickName; } }

        public override void initObject()
        {
            this.ready = false;
            this.account = null;
            this.enabled = false;
            this.sid = null;
            this.status = AccountStatus.Unknown;
            this.server = null;
            this.serverTitle = null;
            this.serverCode = null;
            this.timeAdjust = 0;
            this.nickName = null;
            this.corpsName = null;
            this.level = 0;
            this.vipLevel = 0;
            this.connectionInfo = null;
            this.lastUpdateDTM = DateTime.Now;
            this.heros = new List<HeroInfo>();
            this.decreeHeros = new List<DecreeInfo>();
            this.autoTasks = new List<Scheduler.AutoTask>();
            this.warInfos = new List<WarInfo>();

            this.bwBody = null;
            this.bwStarted = false;
            this.bwEnded = false;
            this.bwFailCnt = 0;
            // Just set a value for LastSend as required, will be ignored as bwStarted is fales
            this.bwLastSend = DateTime.Now;

        }

        public GameAccount(AccountInfo li, ConnectionInfo ci)
        {
            initObject();
            if (li.ready)
            {
                this.account = li.account;
                this.enabled = false;
                this.updateSession(li, ci);
                this.rebuildAutoTasks();
            }
        }

        public void updateSession(AccountInfo li, ConnectionInfo ci)
        {
            this.sid = li.sid;
            this.status = AccountStatus.Online;
            // timeAjust will be set later using system.ping
            this.timeAdjust = 0;
            this.server = li.server;
            this.serverTitle = li.serverTitle;
            this.nickName = li.nickName;
            this.corpsName = li.CORPS_NAME;
            try
            {
                this.level = int.Parse(li.LEVEL);

            }
            catch { this.level = 0; }

            try
            {
                this.vipLevel = int.Parse(li.VIP_LEVEL);

            }
            catch { this.vipLevel = 0; }

            this.connectionInfo = ci;

            this.lastUpdateDTM = DateTime.Now;
            refreshAccount();
            this.ready = true;
        }

        public GameAccount(dynamic json)
        {
            fromJson(json);
            this.sortHeros();
        }

        public override bool fromJson(dynamic json)
        {
            initObject();
            if (json == null) return false;
            this.account = JSON.getString(json, KEY.account, "");
            this.enabled = JSON.getBool(json, KEY.enabled);
            this.sid = JSON.getString(json, KEY.sid, "");
            this.status = AccountStatus.Unknown;
            this.timeAdjust = JSON.getInt(json, KEY.timeAdjust, 0);
            this.server = JSON.getString(json, KEY.server, "");
            this.serverTitle = JSON.getString(json, KEY.serverTitle, "");
            this.nickName = JSON.getString(json, KEY.nickName, "");
            this.corpsName = JSON.getString(json, KEY.corpsName, "");
            this.level = JSON.getInt(json, KEY.level, 0);
            this.vipLevel = JSON.getInt(json, KEY.vipLevel, 0);

            dynamic ci = json[KEY.connectionInfo];
            this.connectionInfo = new ConnectionInfo(ci);
            conv.Json2List(ref this.heros, json[KEY.heros]);
            conv.Json2List(ref this.decreeHeros, json[KEY.decreeHeros]);

            this.autoTasks = new List<Scheduler.AutoTask>();
            if (JSON.exists(json, KEY.autoTasks, typeof(DynamicJsonArray)))
            {
                DynamicJsonArray tasks = (DynamicJsonArray)json[KEY.autoTasks];
                foreach (dynamic t in tasks)
                {
                    autoTasks.Add(new Scheduler.AutoTask(t));
                }
            }
            rebuildAutoTasks();
            // Do not restore warInfo here, warInfo will be save/restore from otherway
            this.ready = true;
            return true;
        }

        public override dynamic toJson()
        {
            if (!this.ready) return null;
            dynamic json = JSON.Empty;
            json[KEY.account] = this.account;
            json[KEY.enabled] = this.enabled;
            json[KEY.sid] = this.sid;
            json[KEY.status] = this.status;
            json[KEY.timeAdjust] = this.timeAdjust;
            json[KEY.server] = this.server;
            json[KEY.serverTitle] = this.serverTitle;
            json[KEY.nickName] = this.nickName;
            json[KEY.corpsName] = this.corpsName;
            json[KEY.level] = this.level;
            json[KEY.vipLevel] = this.vipLevel;
            json[KEY.connectionInfo] = this.connectionInfo.toJson();
            json[KEY.heros] = util.infoBaseListToJsonArray(this.heros.ToArray());
            json[KEY.decreeHeros] = util.infoBaseListToJsonArray(this.decreeHeros.ToArray());
            json[KEY.autoTasks] = util.infoBaseListToJsonArray(this.autoTasks.ToArray());
            // Do not save warInfo here, warInfo will be save/restore from otherway

            return json;
        }

        public void rebuildAutoTasks()
        {
            foreach (Scheduler.KingsTask t in Scheduler.autoTaskList)
            {
                rebuildAutoTask(t.id);
            }
        }

        public Scheduler.AutoTask rebuildAutoTask(string taskId)
        {
            Scheduler.AutoTask oAT = this.autoTasks.Find(x => x.taskId == taskId);
            if (oAT == null)
            {
                oAT = new Scheduler.AutoTask(taskId, true, null, null);
                this.autoTasks.Add(oAT);
            }
            if (oAT.schedule == null)
            {
                oAT.schedule = Scheduler.defaultSchedule(taskId);
            }
            return oAT;
        }


        public static bool find(List<GameAccount> gameAccounts, GameAccount oGA, ref GameAccount oFind)
        {
            return find(gameAccounts, oGA.account, ref oFind);
        }

        public static bool find(List<GameAccount> gameAccounts, string account, ref GameAccount oFind)
        {
            if ((account == null) || (account.Trim() == "")) return false;
            oFind = gameAccounts.Find(x => x.account == account);
            if (oFind == null) return false;
            return true;
        }

        public AccountStatus checkStatus(bool forceCheck = false)
        {
            if ((!forceCheck) && (this.status == AccountStatus.Offline)) return AccountStatus.Offline;
            if (connectionInfo == null)
            {
                this.status = AccountStatus.Offline;
            }
            else
            {
                int timeAdj = 0;
                this.status = action.account.checkStatus(connectionInfo, sid, ref timeAdj);
                if (this.status == AccountStatus.Online) this.timeAdjust = timeAdj;
            }
            return this.status;
        }

        public bool refreshHeros()
        {
            if (this.connectionInfo == null) return false;
            if (this.status != AccountStatus.Online) return false;

            this.heros = action.hero.getInfo(connectionInfo, sid);
            sortHeros();
            this.decreeHeros = action.decree.getInfo(connectionInfo, sid, this.heros);

            return true;
        }

        public void sortHeros()
        {
            this.heros.Sort(delegate (HeroInfo a, HeroInfo b)
            {
                return b.power.CompareTo(a.power);
            });
        }

        public string getHeroName(int heroIdx)
        {
            HeroInfo hi = heros.Find(x => x.idx == heroIdx);
            if (hi == null) return "[無名]";
            return hi.nm;
        }


        public void refreshRecord()
        {
            // For pubgame accounts:
            string[] parts = serverTitle.Split(' ');
            if ((parts[0].Length < 2) || (parts[0].Length > 4)) serverCode = parts[0];
            try
            {
                if (this.account.Contains("_pubgame"))
                {
                    int serverId = Convert.ToInt32(parts[0].Substring(1));
                    this.pubGameServerId = (serverId > 9 ? serverId - 9 : serverId);
                    this.serverCode = parts[0].Substring(0, 1) + pubGameServerId.ToString();
                }
                else
                {
                    this.pubGameServerId = -1;
                    this.serverCode = parts[0];
                }
            }
            catch
            {
                this.pubGameServerId = -1;
                this.serverCode = parts[0];
            }
        }

        public bool refreshAccount()
        {
            this.refreshRecord();
            if (this.checkStatus(true) != AccountStatus.Online) return false;
            return this.refreshHeros();
        }

        public int CompareTo(GameAccount compareGA)
        {
            if (compareGA == null)
                return 1;
            else
                return this.serverTitle.CompareTo(compareGA.serverTitle);

        }

        public bool IsOnline()
        {
            return (this.status == AccountStatus.Online);
        }

        public Scheduler.AutoTask findAutoTask(string taskId)
        {
            Scheduler.AutoTask autoTask = autoTasks.Find(x => x.taskId == taskId);
            return autoTask;
        }

        public string getTaskParameter(string taskId)
        {
            Scheduler.AutoTask autoTask = findAutoTask(taskId);
            if (autoTask == null) return null;
            return autoTask.parameter;
        }

        public dynamic getTaskParmObject(string taskId)
        {
            Scheduler.AutoTask autoTask = findAutoTask(taskId);
            if (autoTask == null) return null;
            return autoTask.parmObject;
        }

        public WarInfo getWarInfo(string taskId, int idx)
        {
            WarInfo wi = warInfos.Find(x => ((x.account == this.account) && (x.taskId == taskId) && (x.idx == idx)));
            return wi;
        }

        public bool executeTask(string taskId, action.DelegateUpdateInfo updateInfo, bool debug)
        {
            Scheduler.AutoTask myTask = findAutoTask(taskId);
            if (myTask == null) return false;

            Scheduler.KingsTask sysTask = Scheduler.autoTaskList.Find(x => x.id == taskId);
            if (sysTask == null) return false;

            return sysTask.executeTask(this, updateInfo, debug);
        }

        public DateTime goAutoTask(action.DelegateUpdateInfo updateInfo, bool debug)
        {
            // No auto task during Boss War
            if (Scheduler.bossTime()) return goBossWar(updateInfo, debug);

            DateTime nextTime = DateTime.Now.AddMinutes(5);
            // foreach (Scheduler.AutoTask myTask in this.autoTasks)
            foreach (Scheduler.KingsTask sysTask in Scheduler.autoTaskList)
            {
                if (!sysTask.isEnabled) continue;
                Scheduler.AutoTask myTask = findAutoTask(sysTask.id);
                if (myTask == null)
                {
                    rebuildAutoTask(sysTask.id);
                    myTask = findAutoTask(sysTask.id);
                    // In case myTask still cannot rebuild
                    if (myTask == null)
                    {
                        LOG.E(string.Format("{0}: Unable to rebuild autoTask {1}", this.displayName, sysTask.id));
                        continue;
                    }
                }

                string taskName = Scheduler.getTaskName(myTask.taskId);
                if (myTask.schedule.readyToGo())
                {
                    if (debug) action.showDebugMsg(updateInfo, this.displayName, taskName, "開始");
                    myTask.schedule.setNextTime(this.executeTask(myTask.taskId, updateInfo, debug));
                    if (debug) action.showDebugMsg(updateInfo, this.displayName, taskName, "結束");
                }

                if (debug) action.showDebugMsg(updateInfo, this.displayName, taskName, string.Format("下次執行時間: {0:yyyy-MM-dd HH:mm:ss}", myTask.schedule.nextExecutionTime.GetValueOrDefault()));
                DateTime? myNext = myTask.schedule.nextExecutionTime;
                if (myNext != null)
                {
                    DateTime realNext = myNext.GetValueOrDefault();
                    if (realNext < nextTime) nextTime = realNext;
                }
            }
            return nextTime;
        }

        private DateTime goBossWar(action.DelegateUpdateInfo updateInfo, bool debug)
        {
            // Shoudl BossWar be a special case under GameAccount?
            // There are many setting to be updated in GameAccount, and it will not use the normal scheduling

            // action.task.goBassWar(this, updateInfo, debug);

            // For safety, read WarInfo all the time
            WarInfo wi = getWarInfo(Scheduler.TaskId.BossWar, 0);
            if ((wi == null) || (wi.body == null) || (wi.body == ""))
            {
                bwBody = null;
                updateInfo(this.displayName, "BossWar", "尚未完成設定, 二分鐘後重試");
                return DateTime.Now.AddMinutes(2);
            }
            else
            {
                bwBody = wi.body;
            }


            // BossWar here
            if (bwStarted && (DateTime.Now < bwLastSend.AddSeconds(30)))
            {
                return bwLastSend.AddSeconds(30);
            }

            if (bwEnded) return DateTime.Now.AddDays(1);

            bool troopsSent = false;

            ConnectionInfo ci = this.connectionInfo;
            string sid = this.sid;

            // ...
            // Send Troops here, update troopsSent & bwEnded (fail for 3 times) here
            troopsSent = action.task.goBassWar(this, updateInfo, debug);

            // .... 
            if (bwEnded) return DateTime.Now.AddDays(1);


            // If failed, retry in 2 seconds later
            if (!troopsSent) return DateTime.Now.AddSeconds(2);
            // Send troops here

            this.bwStarted = true;
            this.bwLastSend = DateTime.Now;
            return DateTime.Now.AddSeconds(31);
     
        }

    }
}
