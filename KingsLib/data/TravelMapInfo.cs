using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using static KingsLib.action;

namespace KingsLib.data
{
    public class TravelMapInfo
    {
        public bool ready;
        public int playerLevel { get; set; }
        public int currStep { get; set; }
        public int remainSteps { get; set; }
        public int diceNum { get; set; }
        public int nextDicePrice { get; set; }
        public int chipsNum { get; set; }
        public int avaBuyDice { get; set; }
        public int total { get; set; }
        public string[] simpleMap { get; set; }
        public int[] boxInfo { get; set; }
        public int mapSize { get; set; }

        public TravelMapInfo()
        {
            this.ready = false;
        }

        public TravelMapInfo(dynamic json)
        {
            this.ready = false;
            if (json == null) return;
            if ((json[RRO.Travel.simpleMap] == null) || (json[RRO.Travel.simpleMap].GetType() != typeof(DynamicJsonObject))) return;

            this.playerLevel = JSON.getInt(json, RRO.Travel.playerLevel);
            this.currStep = JSON.getInt(json, RRO.Travel.currStep);
            this.remainSteps = JSON.getInt(json, RRO.Travel.remainSteps);
            this.diceNum = JSON.getInt(json, RRO.Travel.diceNum);
            this.nextDicePrice = JSON.getInt(json, RRO.Travel.nextDicePrice);
            this.chipsNum = JSON.getInt(json, RRO.Travel.chipsNum);
            this.avaBuyDice = JSON.getInt(json, RRO.Travel.avaBuyDice);
            this.total = JSON.getInt(json, RRO.Travel.total);

            DynamicJsonObject djo = json[RRO.Travel.simpleMap];
            this.mapSize = djo.GetDynamicMemberNames().Count() - 1;
            this.simpleMap = new String[mapSize + 1];
            for (int i = 0; i <= mapSize; i++)
            {
                this.simpleMap[i] = JSON.getString(json[RRO.Travel.simpleMap], i.ToString(), "");
            }
            boxInfo = new int[mapSize + 1];
            this.ready = true;
        }
    }
}
