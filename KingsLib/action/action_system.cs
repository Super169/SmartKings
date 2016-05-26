using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib
{
    public static partial class action
    {
        public static GameAccount.AccountStatus goCheckAccountStatus(HTTPRequestHeaders oH, string sid)
        {
            RequestReturnObject rro = request.System.ping(oH, sid);
            if (!rro.success) return GameAccount.AccountStatus.Unknown;
            if (rro.prompt == PROMPT_NOT_ENOUGH) return GameAccount.AccountStatus.Offline;
            return GameAccount.AccountStatus.Online;
        }
    }
}
