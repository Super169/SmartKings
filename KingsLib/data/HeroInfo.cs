using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib.data
{
    public class HeroInfo : IInfoObject
    {

        private static class KEY
        {
            public const string idx = "idx";
            public const string nm = "nm";
            public const string dsp = "nm";
            public const string army = "army";
            public const string lv = "lv";
            public const string exp = "exp";
            public const string power = "power";
            public const string cfd = "cfd";
            public const string intl = "intl";
            public const string strg = "strg";
            public const string chrm = "chrm";
            public const string attk = "attk";
            public const string dfnc = "dfnc";
            public const string spd = "spd";
            public const string amftLvs = "amftLvs";
        }


        public int idx { get; set; }
        public string nm { get; set; }
        public string dsp { get; set; }
        public string army { get; set; }
        public int lv { get; set; }
        public int exp { get; set; }
        public int power { get; set; }
        public int cfd { get; set; }
        public int intl { get; set; }
        public int strg { get; set; }
        public int chrm { get; set; }
        public int attk { get; set; }
        public int dfnc { get; set; }
        public int spd { get; set; }
        public int[] amftLvs { get; set; } = new int[5];

        public HeroInfo()
        {
            idx = 0;
        }

        public HeroInfo(string jsonString)
        {
            this.fromJsonString(jsonString);
        }

        public HeroInfo(dynamic json)
        {
            this.fromJson(json);
        }

        public bool fromJson(dynamic json)
        {
            bool success = false;
            try
            {
                this.idx = JSON.getInt(json, KEY.idx, 0);
                this.nm = JSON.getString(json, KEY.nm, "");
                this.army = JSON.getString(json, KEY.army, "");
                this.lv = JSON.getInt(json, KEY.lv);
                this.power = JSON.getInt(json, KEY.power);
                this.cfd = JSON.getInt(json, KEY.cfd);
                this.intl = JSON.getInt(json, KEY.intl);
                this.strg = JSON.getInt(json, KEY.strg);
                this.chrm = JSON.getInt(json, KEY.chrm);
                this.attk = JSON.getInt(json, KEY.attk);
                this.dfnc = JSON.getInt(json, KEY.dfnc);
                this.spd = JSON.getInt(json, KEY.spd);

                if (json.amftLvs is DynamicJsonArray)
                {
                    // DynamicJsonArray s = (DynamicJsonArray)json.amftLvs;
                    // for (int i = 0; i < 5; i++) this.amftLvs[i] = (int) s.ElementAt(i);
                    this.amftLvs = JSON.getIntArray(json, KEY.amftLvs);
                }
                success = true;
            }
            catch
            {
                idx = 0;
            }
            return success;
        }

        public bool fromJsonString(string jsonString)
        {
            return fromJson(JSON.decode(jsonString));
        }

        public dynamic toJson()
        {
            dynamic json = JSON.Empty;
            try
            {
                json[KEY.idx] = this.idx;
                json[KEY.nm] = this.nm;
                json[KEY.army] = this.army;
                json[KEY.lv] = this.lv;
                json[KEY.power] = this.power;
                json[KEY.cfd] = this.cfd;
                json[KEY.intl] = this.intl;
                json[KEY.strg] = this.strg;
                json[KEY.chrm] = this.chrm;
                json[KEY.attk] = this.attk;
                json[KEY.dfnc] = this.dfnc;
                json[KEY.spd] = this.spd;
                json[KEY.amftLvs] = this.amftLvs;
            }
            catch (Exception) { }
            return json;
        }

        public string toJsonString()
        {
            return JSON.encode(this.toJson());
        }
    }
}
