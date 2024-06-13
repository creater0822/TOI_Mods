using Newtonsoft.Json;

namespace MOD_tlRCB
{
    public class NpcSettings
    {
        [JsonProperty("开启")]
        public bool Enable { get; internal set; } = true;


        [JsonProperty("NPC不会强奸玩家亲友")]
        public bool NpcCannotFuckPlayersFriend { get; internal set; } = true;


        [JsonProperty("NPC不会强奸女玩家")]
        public bool NpcCannotFuckFemalePlayer { get; internal set; }

        [JsonProperty("NPC战胜后可强上时强上机率")]
        public int RapePercent { get; internal set; } = 100;


        [JsonProperty("NPC不会拐卖女玩家")]
        public bool NpcCannotSellFemalePlayer { get; internal set; }

        [JsonProperty("NPC战胜败者后高仇恨时拐卖机率")]
        public int SellPercent { get; internal set; } = 70;


        [JsonProperty("绑架要求最小仇恨")]
        public int KidnapMinHate { get; internal set; } = 180;

    }
}
