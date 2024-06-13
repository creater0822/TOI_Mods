using HarmonyLib;
using MOD_tlRCB.EvilFall;

namespace MOD_tlRCB.Hook
{
    [HarmonyPatch(typeof(UnitActionPropUse), "OnCreate")]
    public class UnitActionPropUsePatch
    {
        [HarmonyPostfix]
        public static void Postfix(UnitActionPropUse __instance)
        {
            WorldUnitBase unit = __instance.unit;
            int? num = __instance.propsData?.propsID;
            if (unit != null && num.HasValue)
            {
                EvilFall.EvilFall.HandleUseItem(unit, num.Value);
            }
        }
    }
}
