using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib.data
{
    public class ConnectionInfo : InfoBase
    {
        private static class KEY
        {
            public const string uri = "uri";
            public const string fullPath = "fullPath";
            public const string cookies = "cookies";
            public const string headers = "headers";
        }

        public string uri { get; set; }
        public string fullPath { get; set; }
        public List<KeyValuePair<string, string>> cookies { get; set; }
        public List<KeyValuePair<string, string>> headers { get; set; }

        public ConnectionInfo()
        {
            this.initObject();
        }

        public ConnectionInfo(dynamic json)
        {
            fromJson(json);
        }

        public ConnectionInfo(string jsonString)
        {
            fromJsonString(jsonString);
        }

        public override void initObject()
        {
            uri = null;
            fullPath = null;
            cookies = new List<KeyValuePair<string, string>>();
            headers = new List<KeyValuePair<string, string>>();
        }   
            
        public void addCookie(string key, string value)
        {
            this.cookies.Add(new KeyValuePair<string, string>(key, value));
        }

        public void addHeader(string key, string value)
        {
            this.headers.Add(new KeyValuePair<string, string>(key, value));
        }

        public string getHeader(string key)
        {
            if (!this.headers.Exists(x => x.Key == key)) return null;
            return this.headers.Find(x => x.Key == key).Value;
        }

        public override bool fromJson(dynamic json)
        {
            this.initObject();
            this.uri = JSON.getString(json, KEY.uri, null);
            this.fullPath = JSON.getString(json, KEY.fullPath, null);
            this.cookies = JSON.getKeyVlauePairList(json, KEY.cookies);
            this.headers = JSON.getKeyVlauePairList(json, KEY.headers);
            return true;
        }

        public override dynamic toJson()
        {
            dynamic json = JSON.Empty;
            json[KEY.uri] = this.uri;
            json[KEY.fullPath] = this.fullPath;
            json[KEY.cookies] = JSON.fromKeyVlauePairList(this.cookies);
            json[KEY.headers] = JSON.fromKeyVlauePairList(this.headers);
            return json;
        }

        public void fromTcpPacketData(string tpcPacketData)
        {
            this.initObject();
            string[] headerStr = tpcPacketData.Split('|');
            foreach (string s in headerStr)
            {
                if ((s.Trim() != "") && !s.StartsWith("POST ") && !s.StartsWith("{") && s.Contains(":"))
                {
                    string[] pair = s.Split(':');
                    string key = pair[0].Trim();
                    string value = pair[1].Trim();
                    if ((key != "") && (value != "") && (key != "Content-Length") && (key != "Content-Type"))
                    {
                        this.addHeader(key, value);
                    }
                }
            }

            this.uri = "http://" + this.getHeader("Host");
            this.fullPath = this.uri + "/m.do";

            string cookiesStr = this.getHeader("Cookie");
            if (cookiesStr != null)
            {
                string[] cookies = cookiesStr.Split(';');
                foreach (string cookie in cookies)
                {
                    string[] c = cookie.Split('=');
                    if (c.Length == 2)
                    {
                        this.addCookie(c[0], c[1]);
                    }
                }
            }

            // Check for abnormal cookies
            if (this.cookies.Count > 1)
            {
                string cookieString = this.getHeader("Cookie");
                LOG.D("Abnormal cookies: " + cookieString);
                // TODO: may need to remove abormal cookies, need further study
            }

        }

    }
}
