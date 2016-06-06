﻿
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
            public const string scheduledTask = "scheduledTask";

            public const string BossWarHeros = "BossWarHeros";
            public const string BossWarChiefIdx = "BossWarChiefIdx";
            public const string BossWarBody = "BossWarBody";
            public const string BossWarCount = "BossWarCount";
            public const string AutoTasks = "AutoTasks";
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
        public string level { get; set; }
        public string vipLevel { get; set; }
        public ConnectionInfo connectionInfo { get; set; }
        public DateTime lastUpdateDTM { get; set; }
        public List<HeroInfo> heros;
        public List<DecreeInfo> decreeHeros;
        public List<Scheduler> scheduledTasks;

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
            this.level = null;
            this.vipLevel = null;
            this.connectionInfo = null;
            this.lastUpdateDTM = DateTime.Now;
            this.heros = new List<HeroInfo>();
            this.decreeHeros = new List<DecreeInfo>();
            this.scheduledTasks = new List<Scheduler>();
        }

        public GameAccount(LoginInfo li, ConnectionInfo ci)
        {
            initObject();
            if (li.ready)
            {
                this.account = li.account;
                this.enabled = false;
                this.updateSession(li, ci);
                /*
                this.sid = li.sid;
                this.status = AccountStatus.Online;
                // timeAjust will be set later using system.ping
                this.timeAdjust = 0;
                this.server = li.server;
                this.serverTitle = li.serverTitle;
                this.nickName = li.nickName;
                this.corpsName = li.CORPS_NAME;
                this.level = li.LEVEL;
                this.vipLevel = li.VIP_LEVEL;

                this.currHeader = oH;
                buildConnectionHeader();

                refreshAccount();
                this.ready = true;
                */
            }
        }

        public void updateSession(LoginInfo li, ConnectionInfo ci)
        {
            this.sid = li.sid;
            this.status = AccountStatus.Online;
            // timeAjust will be set later using system.ping
            this.timeAdjust = 0;
            this.server = li.server;
            this.serverTitle = li.serverTitle;
            this.nickName = li.nickName;
            this.corpsName = li.CORPS_NAME;
            this.level = li.LEVEL;
            this.vipLevel = li.VIP_LEVEL;

            this.connectionInfo = ci;

            this.lastUpdateDTM = DateTime.Now;
            refreshAccount();
            this.ready = true;
        }

        public GameAccount(dynamic json)
        {
            fromJson(json);
        }

        public override bool fromJson(dynamic json)
        {
            initObject();
            if (json == null) return false;
            this.account = JSON.getString(json[KEY.account], "");
            this.enabled = JSON.getBool(json[KEY.enabled]);
            this.sid = JSON.getString(json[KEY.sid], "");
            this.status = AccountStatus.Unknown;
            this.timeAdjust = JSON.getInt(json[KEY.timeAdjust], 0);
            this.server = JSON.getString(json[KEY.server], "");
            this.serverTitle = JSON.getString(json[KEY.serverTitle], "");
            this.nickName = JSON.getString(json[KEY.nickName], "");
            this.corpsName = JSON.getString(json[KEY.corpsName], "");
            this.level = JSON.getString(json[KEY.level], "");
            this.vipLevel = JSON.getString(json[KEY.vipLevel], "");

            dynamic ci = json[KEY.connectionInfo];
            this.connectionInfo = new ConnectionInfo(ci);
            conv.Json2List(ref this.heros, json[KEY.heros]);
            conv.Json2List(ref this.decreeHeros, json[KEY.decreeHeros]);

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
            json[KEY.scheduledTask] = util.infoBaseListToJsonArray(this.scheduledTasks.ToArray());
            return json;
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
                this.status = action.goCheckAccountStatus(connectionInfo, sid, ref timeAdj);
                if (this.status == AccountStatus.Online) this.timeAdjust = timeAdj;
            }
            return this.status;
        }

        public bool refreshHeros()
        {
            if (this.connectionInfo == null) return false;
            if (this.status != AccountStatus.Online) return false;

            this.heros = action.getHerosInfo(connectionInfo, sid);
            this.decreeHeros = action.getDecreeInfo(connectionInfo, sid, this.heros);

            return true;
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

    }
}
