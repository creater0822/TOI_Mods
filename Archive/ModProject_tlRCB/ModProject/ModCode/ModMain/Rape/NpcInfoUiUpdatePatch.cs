using Harmony;
using MOD_tlRCB.EvilFall;

namespace MOD_tlRCB.Rape
{
    [HarmonyPatch(typeof(UINPCInfoProperty), "UpdateUI")]
    public class NpcInfoUiUpdatePatch
    {
        [HarmonyPrefix]
        public static void Prefix(ref UINPCInfoProperty __instance)
        {
            EvilFall.EvilFall.HandleNpcShowEvilFall(__instance);
        }
    }
}
