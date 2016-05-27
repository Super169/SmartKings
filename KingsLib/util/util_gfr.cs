using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using KingsLib.data;

namespace KingsLib
{
    public partial class util
    {
        public static void Gfr2List<T>(ref List<T> data, GFR.GenericFileRecord gfr, string key) where T : IInfoObject, new()
        {
            string jsonString = JSON.getString(gfr.getObject(key));
            dynamic json = Json.Decode(jsonString);
            if ((json["data"] != null) && (json["data"].GetType() == typeof(DynamicJsonArray)))
            {
                conv.Json2List(ref data, json["data"]);
            }
        }
    }
}
