using System;
using System.Collections.Generic;
using HarmonyLib;
using MOD_tlRCB.EvilFall;

namespace MOD_tlRCB
{
    [HarmonyPatch(typeof(UnitCondition), "Condition")]
    public class DramaConditionPatch
    {
        public delegate bool ConditionCheck(string[] conditionParam, UnitCondition unitCondition);

        public static Dictionary<string, ConditionCheck> conditionChecks = new Dictionary<string, ConditionCheck>();

        public static void Init()
        {
            RegisterCondition("duoluozhi", DepravationCheck);
            RegisterCondition("duoluodu", DepravationLevelCheck);
            RegisterCondition("可赎身", CanReceptionCheck);
        }

        [HarmonyPrefix]
        public static bool Prefix(UnitCondition __instance, ref bool __result)
        {
            string condition = __instance.condition;
            if (condition == "0" || string.IsNullOrEmpty(condition))
            {
                return true;
            }
            string[] array = condition.Split('|');
            List<string> list = new List<string>();
            int num = 0;
            bool flag = false;
            if (array[0] == "or")
            {
                num = 1;
                flag = true;
            }
            for (int i = num; i < array.Length; i++)
            {
                string[] array2 = array[i].Split('_');
                string key = array2[0];
                if (conditionChecks.TryGetValue(key, out var value))
                {
                    bool flag2;
                    try
                    {
                        flag2 = value(array2, __instance);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(ex.ToString());
                        throw;
                    }
                    if (flag2 && flag)
                    {
                        __result = true;
                        return false;
                    }
                    if (!flag2 && !flag)
                    {
                        __result = false;
                        return false;
                    }
                }
                else
                {
                    list.Add(array[i]);
                }
            }
            if (list.Count > 0)
            {
                if (flag)
                {
                    list.Insert(0, "or");
                    __instance.condition = string.Join("|", list);
                    return true;
                }
                __instance.condition = string.Join("|", list);
                return true;
            }
            if (flag)
            {
                __result = false;
                return false;
            }
            __result = true;
            return false;
        }

        public static void RegisterCondition(string conditionName, ConditionCheck conditionCheck)
        {
            conditionChecks[conditionName] = conditionCheck;
        }

        public static string GetBoolDesc(bool get)
        {
            if (get)
            {
                return "√";
            }
            return "×";
        }

        private static bool CanReceptionCheck(string[] conditionparam, UnitCondition unitcondition)
        {
            WorldUnitBase unitByID = GetUnitByID(unitcondition, int.Parse(conditionparam[1]));
            WorldUnitBase unitByID2 = GetUnitByID(unitcondition, int.Parse(conditionparam[2]));
            return Brothel.CanReception(unitByID, unitByID2);
        }

        public static bool DepravationCheck(string[] conditionparam, UnitCondition unitcondition)
        {
            WorldUnitBase unitByID = GetUnitByID(unitcondition, int.Parse(conditionparam[1]));
            int num = int.Parse(conditionparam[2]);
            return unitByID.GetInt("corruption_val") >= num;
        }

        public static bool DepravationLevelCheck(string[] conditionparam, UnitCondition unitcondition)
        {
            WorldUnitBase unitByID = GetUnitByID(unitcondition, int.Parse(conditionparam[1]));
            string text = conditionparam[2];
            int num = int.Parse(conditionparam[3]);
            int evilFallLevel = unitByID.GetEvilFallLevel();
            switch (text)
            {
                case "g":
                    return evilFallLevel > num;
                case "ge":
                    return evilFallLevel >= num;
                case "e":
                    return evilFallLevel == num;
                case "l":
                    return evilFallLevel < num;
                case "le":
                    return evilFallLevel <= num;
                default:
                    Log.Debug("堕落度条件：参数不合法" + unitcondition.condition);
                    return false;
            }
        }

        public static WorldUnitBase GetUnitByID(UnitCondition unitCondition, int id)
        {
            switch (id)
            {
                case -1:
                    return g.world.playerUnit;
                case 0:
                    return unitCondition.data.unitA;
                case 1:
                    return unitCondition.data.unitB;
                case 2:
                    return unitCondition.data.unitC;
                default:
                    return null;
            }
        }
    }
}
