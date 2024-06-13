using System;
using HarmonyLib;
using UnityEngine;

namespace LuciferModifier.UI
{
    [HarmonyPatch(typeof(UICreatePlayerFacade), "OnAttackClick")]
    internal class Patch_UICreatePlayerFacade_OnAttackClick
    {
        [HarmonyPrefix]
        private static bool Prefix(UICreatePlayerFacade __instance)
        {
            if (Patch_UICreatePlayer_DestroyUI.onCreateFace)
            {
                if (Patch_UICreatePlayer_DestroyUI.corMoveCall != null)
                {
                    Patch_UICreatePlayer_DestroyUI.corMoveCall.Stop();
                }
                __instance.playerCtrl.anim.SetBool(UnitAnimState.Run, value: false);
                __instance.playerCtrl.anim.Play(UnitAnimState.Attack);
                GameObject original = g.res.Load<GameObject>("Effect/UI/WanjiaGongji");
                GameObject effectGo = UnityEngine.Object.Instantiate(original, __instance.rimgPlayer2.transform);
                if (effectGo != null)
                {
                    effectGo.transform.localPosition = new Vector3(40f, 5f);
                    effectGo.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
                    GameEffectTool.SetSortOrder(effectGo, __instance.createPlayer.sortingOrder + 1);
                }
                Action action = delegate
                {
                    if (effectGo != null)
                    {
                        UnityEngine.Object.Destroy(effectGo);
                    }
                };
                Patch_UICreatePlayer_DestroyUI.corMoveCall = __instance.createPlayer.DelayTime(action, 2f);
                return false;
            }
            return true;
        }
    }
}
