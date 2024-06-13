using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_NFKFqQ
{
    [HarmonyPatch(typeof(UINPCInfoBase))]
    [HarmonyPatch("Init")]
    public class Patch_UINPCInfoBase_Init
    {
        private static WorldUnitBase unit111;

        [HarmonyPostfix]
        public static void PostfixInit(UINPCInfoBase __instance)
        {
            UINPCInfo ui = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);
            GameObject gameObject = GameObject.Find("Game/UIRoot/Canvas/Root/UI/NPCInfo/Image/Group:Property");
            GameObject renameBtn = new GameObject("NewButton");
            renameBtn.transform.SetParent(gameObject.gameObject.transform, worldPositionStays: false);
            Button button = renameBtn.AddComponent<Button>();
            renameBtn.AddComponent<Image>().sprite = SpriteTool.GetSprite("Common", "XXYYZZ");
            RectTransform component = renameBtn.GetComponent<RectTransform>();
            component.localScale = new Vector3(0.26f, 0.26f, 1f);
            component.anchoredPosition = new Vector3(-524.3762f, 173.7891f, 0f);
            // Open rename textbox-interface
            button.onClick.AddListener((Action)delegate
            {
                unit111 = ui.unit;
                UIBase inputField = g.ui.OpenUI(new UIType.UITypeBase("InputItem", UILayer.UI));
                RectTransform inFieldTransf = inputField.GetComponent<RectTransform>();
                inFieldTransf.localScale = new Vector3(0.9f, 0.8f, 1f);
                inFieldTransf.anchoredPosition = new Vector3(-122.3051f, 226.8997f, 0f);
                Transform inputItem = inputField.transform;
                if (inputItem == null)
                {
                    Console.WriteLine("null! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! ! !");
                }
                else
                {
                    Button chNameBtn = inputItem.Find("Button").GetComponent<Button>();
                    Button closeBtn = inputItem.Find("ButtonTwo").GetComponent<Button>();
                    // Rename button
                    chNameBtn.onClick.AddListener((Action)delegate
                    {
                        unit111.data.unitData.propertyData.GetName();
                        string text = inputItem.Find("InputField").GetComponent<InputField>().text;
                        string[] FullName = text.Split(' '); // UTF-16
                        if (FullName.Length == 2) // Name must have exactly 1 space, e.g. it must be two words.
                        {
                            string[] array = new string[2] { FullName[0], FullName[1] };
                            unit111.data.unitData.propertyData.name = array;
                            DramaFunctionTool.OptionsFunction("systemTips_Has been renamed：" + text + ", turn off interface refresh._5000");
                        }
                        else
                        {
                            DramaFunctionTool.OptionsFunction("systemTips_Name must have exactly 1 space._5000");
                        }
                        g.ui.CloseUI(inputField);
                    });
                    // Close textboxes
                    closeBtn.onClick.AddListener((Action)delegate { g.ui.CloseUI(inputField); });
                }
            });
        }
    }
}
