
using KingsLib.data;
using KingsLib.request;
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
        public static PlayerProperties getPlayerProperties(ConnectionInfo ci, string sid)
        {
            PlayerProperties pp = new PlayerProperties() { ready = false };
            RequestReturnObject rro = request.Player.getProperties(ci, sid);
            if (!rro.SuccessWithJson(RRO.Player.pvs, typeof(DynamicJsonArray))) return pp;
            DynamicJsonArray pvs = rro.responseJson(RRO.Player.pvs);
            foreach (dynamic pv in pvs)
            {
                string p = JSON.getString(pv, RRO.Player.p);
                switch (p)
                {
                    case RRO.Player.EXP:
                        pp.EXP = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.UNDERGO_EXP:
                        pp.UNDERGO_EXP = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.LEVEL_UP_EXP:
                        pp.LEVEL_UP_EXP = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.LEVEL:
                        pp.LEVEL = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.VIP_LEVEL:
                        pp.VIP_LEVEL = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.GOLD:
                        pp.GOLD = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.SILVER:
                        pp.SILVER = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.FOOD:
                        pp.FOOD = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.EXPLOIT:
                        pp.EXPLOIT = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.ARENA_COIN:
                        pp.ARENA_COIN = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.XIYU_COIN:
                        pp.XIYU_COIN = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.MAX_FOOD:
                        pp.MAX_FOOD = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.MAX_SILVER:
                        pp.MAX_SILVER = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.MAX_IRON:
                        pp.MAX_IRON = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.CORPS_NAME:
                        pp.CORPS_NAME = JSON.getString(pv, RRO.Player.v);
                        break;
                    case RRO.Player.IRON:
                        pp.IRON = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.ICON:
                        pp.ICON = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.PLATFORM_MARK:
                        pp.PLATFORM_MARK = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.LONGMARCH_COIN:
                        pp.LONGMARCH_COIN = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.CSKING_COIN:
                        pp.CSKING_COIN = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.FIGHTING_SPIRIT:
                        pp.FIGHTING_SPIRIT = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.CONTRIBUTION:
                        pp.CONTRIBUTION = JSON.getInt(pv, RRO.Player.v);
                        break;
                    case RRO.Player.GOLD_TICKET:
                        pp.GOLD_TICKET = JSON.getInt(pv, RRO.Player.v);
                        break;
                }
            }
            pp.ready = true;
            return pp;

        }
    }
}
