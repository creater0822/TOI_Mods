using Newtonsoft.Json;

namespace MOD_tlRCB
{
    public class BrothelSettings
    {
        [JsonProperty("允许NPC自赎身")]
        public bool NpcCanRedemptionSelf { get; internal set; } = true;


        [JsonProperty("青楼最短工作时长")]
        public int CanRedemptionMonth { get; internal set; } = 6;


        [JsonProperty("女档过月自动接客")]
        public bool FemalePlayerAutoFuck { get; internal set; }

        [JsonProperty("每座城青楼最多妓女数量")]
        public int BitchCountForCity { get; internal set; } = 100;

    }
}
