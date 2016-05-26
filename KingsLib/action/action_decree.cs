using Fiddler;
using KingsLib.data;
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
        public static List<DecreeInfo> getDecreeInfo(HTTPRequestHeaders oH, string sid, List<HeroInfo> heroList)
        {
            List<DecreeInfo> decreeInfo = new List<DecreeInfo>();
            try
            {
                RequestReturnObject rro = request.Manor.decreeInfo(oH, sid);
                if (rro.SuccessWithJson("decHeros", typeof(DynamicJsonArray)))
                {
                    DynamicJsonArray decHeros = rro.responseJson["decHeros"];

                    foreach (dynamic decree in decHeros)
                    {

                        DecreeInfo decInfo = new DecreeInfo() { decId = decree.decId };
                        DynamicJsonArray heros = decree.heros;
                        foreach (dynamic hero in heros)
                        {
                            int heroIdx = (hero.open ? hero.heroIdx : -1);
                            decInfo.heroIdx[hero.pos - 1] = heroIdx;
                            if (heroIdx > 0)
                            {
                                HeroInfo hi = null;
                                if (heroList.Count > 0) hi = heroList.SingleOrDefault(x => x.idx == heroIdx);
                                decInfo.heroName[hero.pos - 1] = (hi == null ? "????" : hi.nm);
                            }
                            else
                            {
                                decInfo.heroName[hero.pos - 1] = (heroIdx == 0 ? "+" : "-");
                            }
                        }
                        decreeInfo.Add(decInfo);
                    }
                }

            }
            catch (Exception) { }
            return decreeInfo;
        }
    }
}
