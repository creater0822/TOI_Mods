using System;
using HarmonyLib;

namespace LuciferModifier.UI
{
    [HarmonyPatch(typeof(UICreatePlayerFacade), "OnMoveClick")]
    internal class Patch_UICreatePlayerFacade_OnMoveClick
    {
        [HarmonyPrefix]
        private static bool Prefix(UICreatePlayerFacade __instance)
        {
            if (Patch_UICreatePlayer_DestroyUI.onCreateFace)
            {
                __instance.playerCtrl.anim.SetBool(UnitAnimState.Run, value: true);
                if (Patch_UICreatePlayer_DestroyUI.corMoveCall != null)
                {
                    Patch_UICreatePlayer_DestroyUI.corMoveCall.Stop();
                }
                Action action = delegate
                {
                    __instance.playerCtrl.anim.SetBool(UnitAnimState.Attack, value: false);
                };
                Patch_UICreatePlayer_DestroyUI.corMoveCall = __instance.createPlayer.DelayTime(action, 1f);
                return false;
            }
            return true;
        }
    }
}
