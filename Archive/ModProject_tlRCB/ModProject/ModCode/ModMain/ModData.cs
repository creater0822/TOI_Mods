using Newtonsoft.Json;

namespace MOD_tlRCB
{
    public class ModData
    {
        [JsonProperty("mods_version")]
        public string Version { get; set; }

        [JsonProperty("mods_updateTime")]
        public string UpdateTime { get; set; }
    }
}
