using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib.data
{
    public class DecreeInfo : IInfoObject
    {

        private static class KEY
        {
            public const string decId = "decId";
            public const string heroIdx = "heroIdx";
            public const string heroName = "heroName";
        }

        static string[] DECREE_NAME = { "", "騎馳", "戍卒", "口賦", "軍屯", "射禦", "戰陣", "列校", "女騎", "兵戶", "????", "冶煉", "鱗甲" };
        public string name()
        {
            return DECREE_NAME[decId];
        }

        public int decId { get; set; }
        public int[] heroIdx { get; set; } = new int[5];
        public string[] heroName { get; set; } = new string[5];

        public DecreeInfo()
        {
        }

        public DecreeInfo(dynamic json)
        {
            fromJson(json);
        }

        public dynamic toJson()
        {
            dynamic json = JSON.Empty;
            try
            {
                json[KEY.decId] = this.decId;
                json[KEY.heroIdx] = this.heroIdx;
                json[KEY.heroName] = this.heroName;
            }
            catch (Exception) { }
            return json;
        }

        public bool fromJson(dynamic json)
        {
            try
            {
                decId = JSON.getInt(json, KEY.decId);
                heroIdx = JSON.getIntArray(json, KEY.heroIdx);
                heroName = JSON.getStringArray(json, KEY.heroName);
                return true;
            }
            catch { }
            return false;
        }

        public bool fromJsonString(string jsonString)
        {
            return fromJson(JSON.decode(jsonString));
        }

        public string toJsonString()
        {
            return JSON.encode(this.toJson());
        }
    }
}
