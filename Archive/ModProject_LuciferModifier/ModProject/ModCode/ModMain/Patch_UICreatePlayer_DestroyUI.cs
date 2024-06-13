using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace LuciferModifier
{
    [HarmonyPatch(typeof(UICreatePlayer), "DestroyUI")]
    internal class Patch_UICreatePlayer_DestroyUI
    {
        public static TimerCoroutine corMoveCall;

        public static bool onCreateFace;

        [HarmonyPrefix]
        private static bool Prefix(UICreatePlayer __instance)
        {
            if (onCreateFace)
            {
                if (corMoveCall != null)
                {
                    corMoveCall.Stop();
                }
                if (__instance.onCloseCall != null)
                {
                    __instance.onCloseCall.Invoke();
                }
                for (int i = 0; i < __instance.allCor.Count; i++)
                {
                    g.timer.Stop(__instance.allCor[i]);
                }
                __instance.allCor = new List<TimerCoroutine>();
                for (int j = 0; j < __instance.allCq.Count; j++)
                {
                    __instance.allCq[j].Destroy();
                }
                onCreateFace = false;
                return false;
            }
            return true;
        }
    }
}
