using Newtonsoft.Json;

namespace MOD_tlRCB
{
    public class BirthSettings
    {
        [JsonProperty("怀孕时长")]
        public int PregnancyDuration { get; internal set; } = 9;


        [JsonProperty("怀孕机率")]
        public int PregnancyPercent { get; internal set; } = 10;


        [JsonProperty("是否流产")]
        public bool Abortion { get; internal set; } = true;


        [JsonProperty("流产机率")]
        public int AbortionPercent { get; internal set; } = 5;


        [JsonProperty("是否早产")]
        public bool PrematureBirth { get; internal set; } = true;


        [JsonProperty("早产机率")]
        public int PrematureBirthPercent { get; internal set; } = 5;


        [JsonProperty("玩家必生女儿")]
        public bool ChildMustBeWomen { get; internal set; } = true;


        [JsonProperty("新生儿男性比例")]
        public int ChildIsMalePercent { get; internal set; } = 50;


        [JsonProperty("出生后为16岁")]
        public bool NewbornGrowUp16 { get; internal set; } = true;


        [JsonProperty("长相继承百分比")]
        public int FaceInheritPercent { get; internal set; } = 50;


        [JsonProperty("流产几次后不孕")]
        public int AbortionCount { get; internal set; } = 3;

    }
}
