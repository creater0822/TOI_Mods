using System;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB.Hook
{
    [HarmonyPatch(typeof(UIPlayerInfo), "Init")]
    public class UIPlayerInfoPatch
    {
        [HarmonyPostfix]
        public static void Postfix(UIPlayerInfo __instance)
        {
            if (__instance.unit.IsWoman())
            {
                return;
            }
            Toggle tglRelationTitle = __instance.tglRelationTitle1;
            GameObject gameObject = UnityEngine.Object.Instantiate(tglRelationTitle.gameObject, tglRelationTitle.transform.parent, worldPositionStays: false);
            gameObject.transform.localPosition = new Vector3(0f, -60f, 0f);
            gameObject.transform.Find("G:textRelationTitle1").GetComponent<Text>().text = "性奴列表";
            RectTransform group3 = new GameObject("Root").AddComponent<RectTransform>();
            group3.SetParent(__instance.goRelation.transform, worldPositionStays: false);
            group3.anchoredPosition = new Vector2(196f, 54f);
            RectTransform component = UnityEngine.Object.Instantiate(__instance.transform.Find("Root/G:goRelation/Group:Relation2/ScrollRect").gameObject, group3, worldPositionStays: false).GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(3200f, 480f);
            component.localPosition = new Vector3(-480f, -85f);
            RectTransform component2 = component.GetChild(0).GetComponent<RectTransform>();
            GameObject gameObject2 = UnityEngine.Object.Instantiate(__instance.transform.Find("Root/G:goRelation/Group:Relation1/Image").gameObject, group3, worldPositionStays: false);
            gameObject2.transform.GetChild(0).GetComponent<Text>().text = "性奴列表";
            gameObject2.transform.localPosition = new Vector3(-500f, 180f, 0f);
            List<string> list = new List<string>();
            Dictionary<string, WorldUnitBase>.Enumerator enumerator = g.world.unit.allUnit.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, WorldUnitBase> current = enumerator.Current;
                if (current.Value.IsSexSlave())
                {
                    list.Add(current.key);
                }
            }
            __instance.uiRelation2.UpdateItem(__instance.uiRelation2.goItem, component2.gameObject, list);
            Toggle component3 = gameObject.GetComponent<Toggle>();
            component3.onValueChanged.RemoveAllListeners();
            Action<bool> action = delegate (bool isOn)
            {
                group3.gameObject.SetActive(isOn);
                if (isOn)
                {
                    __instance.uiRelation1.goGroupRoot.SetActive(value: false);
                    __instance.uiRelation2.goGroupRoot.SetActive(value: false);
                }
            };
            component3.onValueChanged.AddListener(action);
        }
    }
}
