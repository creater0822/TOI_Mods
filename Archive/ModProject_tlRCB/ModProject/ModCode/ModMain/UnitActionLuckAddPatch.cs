using HarmonyLib;

namespace MOD_tlRCB
{
    [HarmonyPatch(typeof(UnitActionLuckAdd), "OnCreate")]
    public class UnitActionLuckAddPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(UnitActionLuckAdd __instance)
        {
            if (__instance.id == 102)
            {
                if (__instance.unit.IsWoman())
                {
                    __instance.unit.AddLuck(-527021125);
                    __instance.unit.SetFirstMan("", "");
                    Log.Debug($"{__instance.unit}复活，再次成为处女");
                }
            }
            else
            {
                if (__instance.id == -541655652)
                {
                    if (__instance.unit.IsWoman() && !__instance.unit.isDie && !__instance.unit.IsBitch())
                    {
                        return true;
                    }
                    Log.Debug(__instance.unit.GetName() + "挣脱了你的龟甲缚直接逃跑了");
                    return false;
                }
                if (__instance.id == 744093962)
                {
                    if (__instance.unit.IsMan())
                    {
                        return false;
                    }
                }
                else if (__instance.id == -523596344)
                {
                    if (!__instance.unit.IsWoman())
                    {
                        return false;
                    }
                }
                else if (__instance.id == 1840247464)
                {
                    if (!__instance.unit.IsMan())
                    {
                        return false;
                    }
                }
                else if (__instance.id == 1561118811)
                {
                    if (__instance.unit.IsMan())
                    {
                        return false;
                    }
                }
                else if (__instance.id == -527021126 && __instance.unit.IsWoman())
                {
                    return false;
                }
            }
            return true;
        }

        [HarmonyPostfix]
        public static void Postfix(UnitActionLuckAdd __instance)
        {
            if (__instance != null && __instance.unit != null && __instance.id != 0 && __instance.unit.HasLuck(744093962) && (__instance.id == 101 || __instance.id == 1011 || __instance.id == 1012 || __instance.id == 1013))
            {
                Birth.HandleUnitDiying(__instance.unit);
                __instance.unit.RemoveLuck(-541655652);
                __instance.unit.RemoveLuck(-523596344);
                __instance.unit.RemoveLuck(1840247464);
            }
            if (__instance.id == -541655652)
            {
                Log.Debug(__instance.unit.GetName() + "被你绑架了");
                UICostItemTool.AddTipText("<color=#BC1717>" + __instance.unit.GetName() + "</color>被你绑架了");
                Brothel.Kidnapping.Add(__instance.unit.GetID());
            }
        }
    }
}
