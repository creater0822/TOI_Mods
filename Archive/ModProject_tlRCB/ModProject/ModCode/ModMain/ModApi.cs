using System.Net.Http;
using Newtonsoft.Json;

namespace MOD_tlRCB
{
    public static class ModApi
    {
        private class Result
        {
            [JsonProperty("code")]
            public int Code { get; set; }

            [JsonProperty("msg")]
            public string Message { get; set; }
        }

        private const int ModId = 189612;

        private static HttpClient Client { get; }

        public static MOD_tlRCB.ModData LastDat { get; private set; }

        static ModApi()
        {
            Client = new HttpClient();
            LastDat = new MOD_tlRCB.ModData
            {
                Version = "读取中",
                UpdateTime = "读取中"
            };
            Init();
        }

        public static async void Init()
        { // Not a good idea... Cause yea... We don't want to update into a non-English ver xD
            LastDat = JsonConvert.DeserializeObject<MOD_tlRCB.ModData>(await (await Client.GetAsync("https://mod.3dmgame.com/mod/API/189612")).Content.ReadAsStringAsync());
        }
    }
}
