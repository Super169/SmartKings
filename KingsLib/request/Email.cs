
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Email
    {
        private const string CMD_getAttachment = "Email.getAttachment";
        private const string CMD_openInBox = "Email.openInBox";
        private const string CMD_read = "Email.read";

        public static RequestReturnObject getAttachment(ConnectionInfo ci, string sid, int emailId)
        {
            string body = string.Format("{{\"emailId\":{0}}}", emailId);
            return com.SendGenericRequest(ci, sid, CMD_getAttachment, true, body);
        }

        public static RequestReturnObject openInBox(ConnectionInfo ci, string sid)
        {
            return com.SendGenericRequest(ci, sid, CMD_openInBox);
        }

        public static RequestReturnObject read(ConnectionInfo ci, string sid, int emailId)
        {
            string body = string.Format("{{\"emailId\":{0}}}", emailId);
            return com.SendGenericRequest(ci, sid, CMD_read, true, body);
        }



    }
}
