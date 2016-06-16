
using KingsLib.data;
using KingsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace KingsLib.monitor
{
    public static class KingsMonitor
    {
        static List<KingsSocketMonitor> monitorList = new List<KingsSocketMonitor>();

        static List<KingsPacketKey> accounts = new List<KingsPacketKey>();
        static Object accountsLocker = new Object();

        static string prePacket = null;
        static string preServer = null;

        // Propagate to parent handler when new sid is detected  
        public delegate void NewSidEventHandler(LoginInfo li, ConnectionInfo ci);
        public static event NewSidEventHandler newSidEventHandler;

        public delegate void NotificationEventHandler(string info);
        public static event NotificationEventHandler notificationEventHandler;

        public static void addAccount(string account, string sid)
        {
            accounts.Add(new KingsPacketKey() { account = account, sid = sid });
            return;
        }

        public static bool Start()
        {
            monitorList.Clear();
            IPAddress[] hosts = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (hosts == null || hosts.Length == 0)
            {

#if CONSOLE_DEBUG
                    Console.WriteLine("No hosts detected, please check your network!");
#endif
                return false;
            }
            for (int i = 0; i < hosts.Length; i++)
            {
                KingsSocketMonitor monitor = new KingsSocketMonitor(hosts[i]);
                monitor.newPacketEventHandler += new KingsSocketMonitor.NewPacketEventHandler(onNewPacketHandler);
                monitorList.Add(monitor);
                // UpdateUI("Monitor added on " + hosts[i].ToString());
            }

            int monitorCnt = 0;

            foreach (KingsSocketMonitor monitor in monitorList)
            {
                if (monitor.Start()) monitorCnt++;
            }

            return (monitorCnt > 0);
        }


        public static bool Stop()
        {
            foreach (KingsSocketMonitor monitor in monitorList)
            {
                monitor.Stop();
            }
            return true;
        }

        private static void onNewPacketHandler(KingsSocketMonitor monitor, KingsPacket p)
        {
            Thread T = new Thread(() => CheckNewPacket(p));
            T.Start();
        }

        private static void CheckNewPacket(KingsPacket p)
        {
            string server = "";
            string sid = "";
            string fullPacket = "";

            string rx = "(kings[0-9]+)\\.icantw.com.+\"sid\":\"([a-z0-9]+)\"";
            Match match = Regex.Match(p.data, rx);
            if (match.Success)
            {
                preServer = null;
                prePacket = null;
                server = match.Groups[1].Value;
                sid = match.Groups[2].Value;
                fullPacket = p.data;
            }
            else
            {
                rx = "(kings[0-9]+)\\.icantw.com";
                match = Regex.Match(p.data, rx);
                if (match.Success)
                {
                    preServer = match.Groups[1].Value;
                    prePacket = p.data;
                    return;
                }
                else
                {
                    if (preServer == null) return;

                    rx = "\"sid\":\"([a-z0-9]+)\"";
                    match = Regex.Match(p.data, rx);
                    if (match.Success)
                    {
                        server = preServer;
                        sid = match.Groups[1].Value;
                        fullPacket = prePacket + p.data;
                    } else
                    {
                        preServer = null;
                        prePacket = null;
                        return;
                    }
                }
            }
            UpdateUI(string.Format("Server: {0} ; sid: {1}", server, sid));

            KingsPacketKey oKPK = null;
            lock (accountsLocker)
            {
                if (!accounts.Exists(x => x.sid == sid))
                {
                    oKPK = new KingsPacketKey() { sid = sid };
                    accounts.Add(oKPK);
                }
            }
            if (oKPK == null) return;

            UpdateUI("*** Find new sid: " + sid);

            // Build Connection Info here
            ConnectionInfo ci = new ConnectionInfo();
            // ci.fromTcpPacketData(p.data);
            ci.fromTcpPacketData(fullPacket);

            data.LoginInfo li = action.getAccountInfo(ci, sid);
            if (!li.ready) return;
            li.server = server;
            UpdateUI(string.Format("{0} | {1} - {2}", li.account, li.serverTitle, li.nickName));

            NewSid(li, ci);


        }


        private static void NewSid(LoginInfo li, ConnectionInfo ci)
        {
            if (newSidEventHandler != null)
            {
                newSidEventHandler(li, ci);
            }
        }

        private static void UpdateUI(string info)
        {
            if (notificationEventHandler != null)
            {
                notificationEventHandler(info);
            }
        }


    }

}
