using HarmonyLib;

namespace MOD_tlRCB
{
    [HarmonyPatch(typeof(UnitActionFeedback1031), "OnCreate")]
    public class TrainBirth
    {
        [HarmonyPostfix]
        public static void Postfix(UnitActionFeedback1031 __instance)
        {
            if (__instance.state == 1)
            {
                WorldUnitBase unit = __instance.unit;
                WorldUnitBase trainsUnit = __instance.trainsUnit;
                WorldUnitBase target = ((unit.data.unitData.propertyData.sex == UnitSexType.Woman) ? unit : trainsUnit);
                WorldUnitBase worldUnitBase = ((unit.data.unitData.propertyData.sex == UnitSexType.Woman) ? trainsUnit : unit);
                if (!worldUnitBase.IsPlayer())
                {
                    Birth.Fuck(worldUnitBase, target);
                }
            }
        }
    }
}
