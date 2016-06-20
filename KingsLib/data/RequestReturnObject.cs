
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.data
{
    public struct RequestReturnObject
    {
        public bool success;
        public int ok;
        public string style;
        public string prompt;
        public int returnCode;
        public string msg;
        public string requestText;
        public string responseText;
        public dynamic responseJson;

        public bool SuccessWithJson(string key1 = null)
        {
            return SuccessWithJson(key1, null, null);
        }

        public bool SuccessWithJson(string key1, string key2)
        {
            return SuccessWithJson(key1, key2, null);
        }

        public bool SuccessWithJson(string key1, Type targetType)
        {
            return SuccessWithJson(key1, null, targetType);
        }

        public bool SuccessWithJson(string key1, string key2, Type targetType)
        {
            if (!success) return false;
            if (responseJson == null) return false;
            if ((key1 == null) || (key1 == "")) return true;
            if (responseJson[key1] == null) return false;
            if ((key2 == null) && (targetType != null) && (responseJson[key1].GetType() != targetType)) return false;
            if ((key2 == null) || (key2 == "")) return true;
            if (responseJson[key1][key2] == null) return false;
            if ((targetType != null) && (responseJson[key1][key2].GetType() != targetType)) return false;
            return true;
        }

        public bool exists(string key, Type targetType = null)
        {
            if (responseJson == null) return false;
            if ((key == null) || (key == "")) return true;
            if (responseJson[key] == null) return false;
            if (targetType == null) return true;
            if (responseJson[key].GetType() != targetType) return false;
            return true;
        }

        public byte getByte(string key, byte defValue = 0)
        {
            return JSON.getByte(this.responseJson, key, defValue);
        }

        public bool getBool(string key, bool defValue = false)
        {
            return JSON.getBool(this.responseJson, key, defValue);
        }

        public int getInt(string key, int defValue = -1)
        {
            return JSON.getInt(this.responseJson, key, defValue);
        }

        public long getLong(string key, long defValue = -1)
        {
            return JSON.getLong(this.responseJson, key, defValue);
        }


        public double getDouble(string key, double defValue = -1.0)
        {
            return JSON.getDouble(this.responseJson, key, defValue);
        }

        public string getString(string key, string defValue = null)
        {
            return JSON.getString(this.responseJson, key, defValue);
        }

    }
}
