using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib.data
{
    public abstract class InfoBase : IInfoObject
    {
        public abstract void initObject();

        public abstract bool fromJson(dynamic json);

        public bool fromJsonString(string jsonString)
        {
            if ((jsonString == null) || (jsonString == "")) return false;
            try
            {
                dynamic json = Json.Decode(jsonString);
                return this.fromJson(json);
            }
            catch
            {
                return false;
            }
        }

        public abstract dynamic toJson();

        public string toJsonString()
        {
            string jsonString = JSON.EmptyString;
            try
            {
                jsonString = Json.Encode(this.toJson());
            }
            catch
            {
                jsonString = JSON.EmptyString;
            }
            return jsonString;
        }
    }
}
