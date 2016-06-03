using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsLib.data
{
    public static class conv
    {
        public static void Json2List<T>(ref List<T> data, dynamic json) where T : IInfoObject, new()
        {
            if (json == null) return;
            if (json.GetType() != typeof(DynamicJsonArray)) return;
            data = new List<T>();
            DynamicJsonArray dja = json;
            foreach (dynamic o in dja)
            {
                IInfoObject x = new T() as IInfoObject;
                x.fromJson(o);
                data.Add((T)x);
            }
        }

        public static dynamic List2Json<T>(List<T> data) where T : IInfoObject
        {
            List<dynamic> json = new List<dynamic>();
            foreach (IInfoObject o in data)
            {
                json.Add(o.toJson());
            }
            return json;
        }
    }
}
