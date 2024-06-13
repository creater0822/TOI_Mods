using HarmonyLib;

namespace MOD_tlRCB
{
    [HarmonyPatch(typeof(UnitActionLuckDel), "OnCreate")]
    public class UnitActionLuckDelPatch
    {
        [HarmonyPostfix]
        public static void Postfix(UnitActionLuckDel __instance)
        {
            Brothel.HandleRemoveKidnapping(__instance);
            if (__instance.luckID == 527000000)
            {
                Brothel.HandelChildGrowUp(__instance.unit);
            }
        }
    }
}
