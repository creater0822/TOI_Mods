using System.Collections.Generic;
using Harmony;

namespace MOD_LE2lAt
{
    [HarmonyPatch(typeof(DramaFunction), "OptionsFunction")]
    internal class Patch_DramaFunction_OptionsFunction
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
            string[] array = function.Split('|');
            List<string> list = new List<string>();
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split('_');
                if (array2[0] == ModMain.nspace())
                {
                    switch (array2[1])
                    {
                        case "dramaShareSkills":
                            ActionMartialDataDeal.dramaShareSkills(__instance, array2);
                            break;
                        case "dramaShareBasis":
                            BasisDeal.dramaShareBasis(__instance, array2);
                            break;
                        case "grade":
                            GradeDeal.ME.grade(__instance, array2);
                            break;
                        case "mod":
                            ModDeal.mod(__instance, array2);
                            break;
                        case "dramaWantShareAll":
                            ShareAllDeal.ME.dramaWantShareAll(__instance, array2);
                            break;
                        case "dramaShareAll":
                            ShareAllDeal.ME.dramaShareAll(__instance, array2);
                            break;
                        case "shareBasAbility":
                            ShareAllDeal.ME.shareBasAbility(__instance, array2);
                            break;
                    }
                }
                else
                {
                    list.Add(array[i]);
                }
            }
            if (list.Count > 0)
            {
                function = string.Join("|", list);
                return true;
            }
            return false;
        }
    }
}
