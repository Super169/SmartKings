﻿using Fiddler;
using KingsLib.data;
using KingsLib.request;
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
        public static List<HeroInfo> getHerosInfo(HTTPRequestHeaders oH, string sid)
        {
            List<HeroInfo> heroList = new List<HeroInfo>();
            try
            {
                RequestReturnObject rro = Hero.getPlayerHeroList(oH, sid);
                if (rro.SuccessWithJson("heros", typeof(DynamicJsonArray)))
                {
                    DynamicJsonArray heros = rro.responseJson.heros;

                    foreach (dynamic hero in heros)
                    {
                        HeroInfo hi = new HeroInfo(hero);
                        if (hi.idx > 0) heroList.Add(hi);
                    }
                }
            }
            catch (Exception)
            {
                heroList = null;
            }

            return heroList;
        }



    }
}
