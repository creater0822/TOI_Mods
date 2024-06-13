using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace LuciferModifier
{
    [HarmonyPatch(typeof(UINPCInfo), "InitData")]
    internal class Patch_UINPCInfo_Init
    {
        [HarmonyPostfix]
        private static void Postfix(UINPCInfo __instance)
        {
            Debug.Log(__instance.unit.data.unitData.propertyData.GetName());
            Transform transform = __instance.transform;
            GameObject gameObject = CreateUI.NewImage(SpriteTool.GetSprite("PlayerTask", "renwubutton"));
            gameObject.name = "openChangeName";
            gameObject.transform.SetParent(transform, worldPositionStays: false);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 30f);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-326f, 228f);
            Button button = gameObject.AddComponent<Button>();
            Action action = delegate
            {
                GUIUtility.systemCopyBuffer = __instance.unit.data.unitData.propertyData.GetName();
            };
            button.onClick.AddListener(action);
        }
    }
}
