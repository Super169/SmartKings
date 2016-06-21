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
    public static partial class action
    {
        public static partial class task
        {
            public static bool goReadAllEmail(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string actionName = "讀取郵件";
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;
                RequestReturnObject rro;
                int readCnt = 0;

                rro = request.Email.openInBox(ci, sid);
                if (!rro.SuccessWithJson(RRO.Email.emails)) return false;

                DynamicJsonArray emails = rro.responseJson[RRO.Email.emails];
                foreach (dynamic email in emails)
                {
                    if (JSON.getString(email, RRO.Email.status, "") == RRO.Email.status_NR)
                    {
                        int emailId = JSON.getInt(email, RRO.Email.id);
                        if (emailId > 0)
                        {
                            rro = request.Email.read(ci, sid, emailId);
                            // May have different count for open email and get attachment : check if [record][resources] exists
                            // But it may be ok to check getAttachment with OK for all unread email.
                            rro = request.Email.getAttachment(ci, sid, emailId);
                            if (rro.ok == 1) readCnt++;
                        }
                    }
                }
                if (readCnt > 0) updateInfo(oGA.displayName, actionName, string.Format("開啟 {0} 封郵件", readCnt));

                return true;
            }

        }
    }
}
