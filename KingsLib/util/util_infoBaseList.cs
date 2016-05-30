using KingsLib.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib
{
    public partial class util
    {
        public static DynamicJsonArray infoBaseListToJsonArray(IInfoObject[] data)
        {
            DynamicJsonArray retJson;
            try
            {
                dynamic json = JSON.Empty;
                List<dynamic> jsonArray = new List<dynamic>();
                foreach (IInfoObject ibo in data)
                {
                    jsonArray.Add(ibo.toJson());
                }
                retJson = new DynamicJsonArray(jsonArray.ToArray());
            }
            catch
            {
                retJson = null;
            }
            return retJson;
        }

        public static string infoBaseListToJsonString(IInfoObject[] data)
        {
            string retJson = JSON.EmptyString;
            try
            {
                DynamicJsonArray dja = infoBaseListToJsonArray(data);
                if (dja != null)
                {
                    dynamic json = JSON.Empty;
                    json["data"] = dja;
                    retJson = Json.Encode(json);
                }
            }
            catch
            {
                retJson = JSON.EmptyString; 
            }
            return retJson;
        }
    }
}
