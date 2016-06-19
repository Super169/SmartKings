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
        public static partial class task
        {
            public static bool goTaskCleanupBag(GameAccount oGA, DelegateUpdateInfo updateInfo, bool debug)
            {
                string taskName = "清理背包";
                ConnectionInfo ci = oGA.connectionInfo;
                string sid = oGA.sid;

                List<BagInfo> bis = action.getBagInfo(ci, sid);
                if (bis == null) return false;

                foreach (BagInfo bi in bis)
                {
                    if (bi.AutoUseItem())
                    {
                        RequestReturnObject rro = request.Bag.useItem(ci, sid, bi.n, bi.idx);
                        if (rro.SuccessWithJson(RRO.Bag.deleted))
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("倉庫中 {0} 個 {1} 全部使用了", bi.n, bi.nm));
                        } else if (rro.SuccessWithJson(RRO.Bag.updated))
                        {
                            updateInfo(oGA.displayName, taskName, string.Format("倉庫中使用了 {0} 個 {1}", bi.n, bi.nm));
                        }
                    }
                }
                return true;
            }
        }
    }
}
