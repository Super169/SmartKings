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
        public static string infoBaseListToJsonString(IInfoObject[] data)
        {
            string retJson = "{}";
            try
            {
                dynamic json = JSON.Empty;
                List<object> jsonArray = new List<dynamic>();
                foreach (IInfoObject ibo in data)
                {
                    jsonArray.Add(ibo.toJson());
                }
                json["data"] = new DynamicJsonArray(jsonArray.ToArray());
                retJson = Json.Encode(json);
            }
            catch
            {
                retJson = JSON.Empty; 
            }
            return retJson;
        }
    }
}
