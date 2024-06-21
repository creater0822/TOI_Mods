using System;
using HarmonyLib;

namespace GGBH_MOD
{
    [HarmonyPatch(typeof(EventsMgr), "Init")]
    public class Patch_EventsMgr_Init
    {
        private static Il2CppSystem.Action<ETypeData> onGameCMDMelonLoaderCall = (Action<ETypeData>)OnGameCMDMelonLoader;
        private static int lastEventHashCode;

        [HarmonyPostfix]
        private static void Postfix()
        {
            if (lastEventHashCode != g.events.GetHashCode())
            {
                lastEventHashCode = g.events.GetHashCode();
                g.events.Off("GameCMDMelonLoader", onGameCMDMelonLoaderCall);
                g.events.On("GameCMDMelonLoader", onGameCMDMelonLoaderCall, 0);
            }
        }

        private static void OnGameCMDMelonLoader(ETypeData e)
        {
            ModMain.OnGameCMDMelonLoader(e);
        }
    }
}
