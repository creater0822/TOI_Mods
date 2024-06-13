using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MOD_tlRCB.EvilFall;

namespace MOD_tlRCB.Rape
{
    [HarmonyPatch(typeof(DramaFunction), "OptionsFunction")]
    public class DramaOptionsFunctionPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(DramaFunction __instance, ref string function, DramaFunctionData functionData)
        {
            if (string.IsNullOrEmpty(function) || function == "0")
            {
                return false;
            }
            __instance._data = functionData;
            if (__instance.data == null)
            {
                __instance._data = new DramaFunctionData();
            }
            List<string> list = (from f in function.Split('|')
                                                            where !Handle(__instance, f)
                                                            select f).ToList();
            if (list.Count <= 0)
            {
                return false;
            }
            function = string.Join("|", list);
            return true;
        }

        private static bool Handle(DramaFunction __instance, string function)
        {
            string[] array = function.Split('_');
            string text = array[0];
            switch (text)
            {
                case "内射":
                case "做爱":
                case "强奸":
                case "强奸内射":
                    {
                        if (array.Length < 3)
                        {
                            Log.Debug(text + "function少参数" + function);
                            return false;
                        }
                        WorldUnitBase unit = GetUnit(__instance, array[1]);
                        if (unit == null)
                        {
                            Log.Debug("内射function男性参数非法" + function);
                            return false;
                        }
                        WorldUnitBase unit2 = GetUnit(__instance, array[2]);
                        if (unit2 == null)
                        {
                            Log.Debug("内射function女性参数非法" + function);
                            return false;
                        }
                        Birth.Fuck(unit, unit2, text == "强奸" || text == "强奸内射", text == "内射" || text == "强奸内射", showDrama: true);
                        return true;
                    }
                case "加堕落":
                    {
                        if (array.Length < 3)
                        {
                            Log.Debug("加堕落命令参数非法：" + function);
                            return false;
                        }
                        WorldUnitBase unit4 = GetUnit(__instance, array[1]);
                        if (unit4 == null)
                        {
                            Log.Debug("加堕落命令参数非法：" + function);
                            return false;
                        }
                        if (int.TryParse(array[2], out var result2))
                        {
                            unit4.AddDepravation(result2);
                            return true;
                        }
                        return false;
                    }
                case "青楼":
                    Brothel.HandleDramaOptionsFunction(array, __instance);
                    return true;
                case "LastItemAddEffect":
                    LastItemAddEffect(__instance);
                    return true;
                case "nymphomaniac":
                    LustfulLuck.HandleFunction(__instance, array);
                    return true;
                case "换装":
                    {
                        WorldUnitBase unit3 = GetUnit(__instance, array[1]);
                        if (array.Length >= 3 && int.TryParse(array[2], out var result))
                        {
                            Dress.Open(unit3, result, __instance.data.dramaData);
                        }
                        else
                        {
                            Dress.Open(unit3);
                        }
                        return true;
                    }
                case "换戒指":
                    {
                        WorldUnitBase unitRight = __instance.data.unitRight;
                        if (unitRight == null)
                        {
                            return true;
                        }
                        Il2CppSystem.Collections.Generic.List<DataProps.PropsData>.Enumerator enumerator = UIPropSelect.allSlectItems.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            DataProps.PropsData current = enumerator.Current;
                            unitRight.AddProp(current.propsID);
                            unitRight.Equip(0, current.propsID);
                        }
                        return true;
                    }
                default:
                    return false;
            }
        }

        private static void LastItemAddEffect(DramaFunction __intsance)
        {
            WorldUnitBase unitRight = __intsance.data.unitRight;
            if (unitRight == null)
            {
                return;
            }
            Il2CppSystem.Collections.Generic.List<DataProps.PropsData>.Enumerator enumerator = UIPropSelect.allSlectItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DataProps.PropsData current = enumerator.Current;
                ConfItemPillItem item = g.conf.itemPill.GetItem(current.propsID);
                if (item != null)
                {
                    unitRight.AddEffect(item.effectValue);
                }
            }
        }

        private static WorldUnitBase GetUnit(DramaFunction __instance, string type)
        {
            switch (type)
            {
                case "0":
                    return g.world.playerUnit;
                case "1":
                    return __instance.data.unitLeft;
                case "2":
                    return __instance.data.unitRight;
                case "3":
                    return __instance.data.unit;
                default:
                    return null;
            }
        }
    }
}
