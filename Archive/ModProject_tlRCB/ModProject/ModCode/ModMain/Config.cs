using System.IO;
using Newtonsoft.Json;

namespace MOD_tlRCB
{
    public class Config
    {
        private static string configPath = Path.Combine(g.mod.GetModPathRoot("tlRCB"), "ModAssets", "Config.json");

        public static Config Instance { get; private set; } = new Config();


        [JsonProperty("生育设置")]
        public BirthSettings BirthSettings { get; internal set; } = new BirthSettings();


        [JsonProperty("性奴设置")]
        public SexSlaveSettings SexSlaveSettings { get; internal set; } = new SexSlaveSettings();
        [JsonProperty("NPC交互")]
        public NpcSettings NpcSettings { get; internal set; } = new NpcSettings();


        [JsonProperty("青楼设置")]
        public BrothelSettings BrothelSettings { get; internal set; } = new BrothelSettings();


        public static void Init()
        {
            if (File.Exists(configPath))
            {
                string text = File.ReadAllText(configPath);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    Instance = JsonConvert.DeserializeObject<Config>(text);
                    Log.Debug("Read configuration file：" + text);
                }
            }
        }

        public bool WillPrematureBirth(int duration)
        {
            if (!BirthSettings.PrematureBirth || BirthSettings.PrematureBirthPercent <= 0)
            {
                return false;
            }
            if (BirthSettings.PregnancyDuration <= 1)
            {
                return false;
            }
            if (duration < BirthSettings.PregnancyDuration && duration <= 3)
            {
                return CommonTool.Random(0, 100) <= BirthSettings.PrematureBirthPercent;
            }
            return false;
        }

        public bool WillAbortion(int duration)
        {
            if (!BirthSettings.Abortion || BirthSettings.AbortionPercent <= 0)
            {
                return false;
            }
            if (BirthSettings.PregnancyDuration <= 1)
            {
                return false;
            }
            if (BirthSettings.PregnancyDuration <= 4 && BirthSettings.PrematureBirth)
            {
                return false;
            }
            if (duration < BirthSettings.PregnancyDuration && duration >= BirthSettings.PregnancyDuration - 2)
            {
                return CommonTool.Random(0, 100) <= BirthSettings.AbortionPercent;
            }
            return false;
        }

        public void Save()
        {
            string contents = JsonConvert.SerializeObject(this);
            File.WriteAllText(configPath, contents);
        }
    }
}
