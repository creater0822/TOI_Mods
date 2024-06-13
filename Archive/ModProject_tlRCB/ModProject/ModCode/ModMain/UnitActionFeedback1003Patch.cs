using System;
using HarmonyLib;

namespace MOD_tlRCB
{
    [HarmonyPatch(typeof(UnitActionFeedback1003), "OnCreate")]
    public class UnitActionFeedback1003Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(UnitActionFeedback1003 __instance)
        {
            try
            {
                WorldUnitBase unit = __instance.unit;
                WorldUnitBase askforUnit = __instance.askforUnit;
                if (unit == null || askforUnit == null || !askforUnit.IsPlayer())
                {
                    return true;
                }
                if (unit.IsSexSlave())
                {
                    __instance.state = 1;
                    __instance.OnEndCall();
                    return false;
                }
                DataProps.PropsData propsData = __instance.propsData;
                if (propsData != null && propsData.propsID == -533659860 && (unit.IsMarried(askforUnit) || unit.data.unitData.relationData.IsFriend(askforUnit)))
                {
                    __instance.state = 1;
                    __instance.OnEndCall();
                    return false;
                }
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
            }
            return true;
        }
    }
}
