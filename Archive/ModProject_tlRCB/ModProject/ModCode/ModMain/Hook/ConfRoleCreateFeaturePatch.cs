using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace MOD_tlRCB.Hook
{
    [HarmonyPatch(typeof(ConfRoleCreateFeature), "RandomItem")]
    public class ConfRoleCreateFeaturePatch
    {
        [HarmonyPostfix]
        private static void Postfix(ref ConfRoleCreateFeature __instance, List<ConfRoleCreateFeatureItem> __result, int __0, int __1, ReturnAction<int, ConfRoleCreateFeatureItem> __2)
        {
            UICreatePlayer uI = g.ui.GetUI<UICreatePlayer>(UIType.CreatePlayer);
            if (uI != null && uI.playerData.unitData.propertyData.sex == UnitSexType.Woman)
            {
                ConfRoleCreateFeatureItem item = g.conf.roleCreateFeature.GetItem(1561118811);
                if (item != null)
                {
                    __result[0] = item;
                }
            }
        }
    }
}
