using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Country
    {
        private const string CMD_corpsCountry = "Country.corpsCountry";
        private const string CMD_viewCountry = "Country.viewCountry";


        public static RequestReturnObject corpsCountry(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_corpsCountry);
        }

        public static RequestReturnObject viewCountry(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_viewCountry);
        }

    }
}
