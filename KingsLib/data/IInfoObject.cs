using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.data
{
    public interface IInfoObject
    {
        bool fromJson(dynamic json);
        bool fromJsonString(string jsonString);
        dynamic toJson();
        string toJsonString();
    }
}
