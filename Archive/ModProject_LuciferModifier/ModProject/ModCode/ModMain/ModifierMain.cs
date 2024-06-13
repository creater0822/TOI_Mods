using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace LuciferModifier
{
    public class ModifierMain : MelonMod
    {
        public static string BanBenHao = "Restart version 1.1";
        public static Dictionary<string, string> _data = new Dictionary<string, string>();
        private static bool isChange;

        public static bool IsChange
        {
            get
            {
                return isChange;
            }
            set
            {
                if (value)
                {
                    LuciferSystem.MainWindows();
                }
                else
                {
                    Object.Destroy(LuciferSystem.root);
                }
                isChange = value;
            }
        }

        public override void OnApplicationLateStart()
        {
            base.OnApplicationLateStart();
            FileUtils.GuiguInit();
            _data = FileUtils.readFile();
            MelonLogger.Msg("Welcome to Lucifer's built-in tools MOD" + BanBenHao + "！");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if ((Input.GetKeyDown(_data["config1"]) || Input.GetKeyDown(_data["config2"])) && g.world.isIntoWorld)
            {
                IsChange = !IsChange;
            }
        }

        public static void Log(string str)
        {
            MelonLogger.Msg("[Lucifer built-in]" + str);
        }
    }

}
