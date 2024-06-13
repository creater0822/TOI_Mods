using HarmonyLib;

namespace MOD_tlRCB.Hook
{
    [HarmonyPatch(typeof(UnitActionFeedback1020), "OnCreate")]
    public class UnitActionFeedback1020Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(UnitActionFeedback1020 __instance)
        {
            if (__instance.propsData.propsID == -1330138045 || __instance.propsData.propsID == -533659860 || (__instance.unit.IsSexSlave() && __instance.giveUnit.IsPlayer()))
            {
                Log.Debug($"{__instance.giveUnit.GetName()} 赠送了 {__instance.unit.GetName()} {__instance.propsData.propsID}");
                __instance.state = 1;
                __instance.OnEndCall();
                return false;
            }
            return true;
        }
    }
}
