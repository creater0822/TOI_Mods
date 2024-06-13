using System;
using HarmonyLib;
using MOD_tlRCB.EvilFall;

namespace MOD_tlRCB.Hook
{
    [HarmonyPatch(typeof(WorldFactory))]
    public class CreateEffectSpecialBasePatch
    {
        private class WorldUnitEffectSpecialEvilFallAdd : WorldUnitEffectSpecial
        {
            public WorldUnitEffectSpecialEvilFallAdd()
            {
                g.timer.Frame((Action)delegate
                {
                    if (int.TryParse(base.data.values[0], out var result))
                    {
                        base.unit.AddDepravation(result);
                    }
                }, 1);
            }
        }

        private class WorldUnitEffectSpecialAgeAdd : WorldUnitEffectSpecial
        {
            public WorldUnitEffectSpecialAgeAdd()
            {
                g.timer.Frame((Action)delegate
                {
                    if (int.TryParse(base.data.values[0], out var result))
                    {
                        base.unit.AddAge(result);
                    }
                }, 1);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("CreateEffectSpecialBase")]
        private static bool CreateEffectSpecialBase_Prefix(string type, ref WorldUnitEffectSpecial __result)
        {
            if (type.Trim().Equals("加堕落", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    __result = new WorldUnitEffectSpecialEvilFallAdd();
                    return false;
                }
                catch (Exception value)
                {
                    Console.WriteLine(value);
                    return true;
                }
            }
            if (type == "长年龄")
            {
                try
                {
                    __result = new WorldUnitEffectSpecialAgeAdd();
                    return false;
                }
                catch (Exception value2)
                {
                    Console.WriteLine(value2);
                    return true;
                }
            }
            return true;
        }
    }
}
