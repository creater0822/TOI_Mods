using Newtonsoft.Json;

namespace MOD_tlRCB
{
    public class SexSlaveSettings
    {
        [JsonProperty("性奴生的女儿也是性奴")]
        public bool SexSlaveInherit { get; internal set; } = true;


        [JsonProperty("性奴生的女儿夫君为玩家")]
        public bool SexSlaveChildMarryFather { get; internal set; } = true;

    }
}
