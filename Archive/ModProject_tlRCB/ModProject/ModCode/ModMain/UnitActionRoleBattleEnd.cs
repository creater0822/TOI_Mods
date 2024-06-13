using HarmonyLib;
using MOD_tlRCB.Rape;

namespace MOD_tlRCB
{
    [HarmonyPatch(typeof(UnitActionRoleBattle))]
    public class UnitActionRoleBattleEnd
    {
        [HarmonyPatch("BattleEnd")]
        [HarmonyPostfix]
        public static void Postfix(UnitActionRoleBattle __instance, WorldUnitBase winUnit, WorldUnitBase failUnit)
        {
            RapeBattle.HandleNpcBattleEndRape(__instance, winUnit, failUnit);
        }
    }
}
