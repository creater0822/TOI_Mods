using System;
using HarmonyLib;

namespace MOD_tlRCB.Hook
{
    [HarmonyPatch(typeof(WorldUnitLuckBase))]
    public class WorldUnitLuckBasePatch
    {
        [HarmonyPatch("Destroy")]
        [HarmonyPrefix]
        private static void Postfix(WorldUnitLuckBase __instance)
        {
            try
            {
                if (__instance.luckData.id == -541655652)
                {
                    Brothel.Kidnapping.Remove(__instance.unit.GetID());
                    Log.Debug("Removed " + __instance.unit.GetID() + " from the list of kidnapped units.");
                }
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
            }
        }
    }
}
