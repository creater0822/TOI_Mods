using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_NFKFqQ
{
    [HarmonyPatch(typeof(UIPlayerInfoBase))]
    [HarmonyPatch("Init")]
    public class Patch_UIPlayerInfoBase_Init
    {
        // private static WorldUnitBase unit222;

        [HarmonyPostfix]
        public static void PostfixInit(UINPCInfoBase __instance)
        {
            g.ui.GetUI<UIPlayerInfo>(UIType.PlayerInfo);
            GameObject gameObject = GameObject.Find("Game/UIRoot/Canvas/Root/UI/PlayerInfo/Root/Group:Property");
            GameObject renameBtn = new GameObject("NewButton");
            renameBtn.transform.SetParent(gameObject.gameObject.transform, worldPositionStays: false);
            Button button = renameBtn.AddComponent<Button>();
            renameBtn.AddComponent<Image>().sprite = SpriteTool.GetSprite("Common", "NameItme");
            RectTransform component = renameBtn.GetComponent<RectTransform>();
            component.localScale = new Vector3(0.26f, 0.26f, 1f);
            component.anchoredPosition = new Vector3(-200f, 158f, 0f);
            // Open rename textbox-interface
            button.onClick.AddListener((Action)delegate
            {
                // unit222 = g.world.playerUnit;
                UIBase inputField = g.ui.OpenUI(new UIType.UITypeBase("InputItem", UILayer.UI));
                RectTransform inFieldTransf = inputField.GetComponent<RectTransform>();
                inFieldTransf.localScale = new Vector3(0.7418f, 0.8f, 1f);
                inFieldTransf.anchoredPosition = new Vector3(33.0258f, 158.0235f, 0f);
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
                        // unit222.data.unitData.propertyData.GetName();
                        string text = inputItem.Find("InputField").GetComponent<InputField>().text;
                        string[] FullName = text.Split(' '); // UTF-16
                        if (FullName.Length == 2) // Name must have exactly 1 space, e.g. it must be two words.
                        {
                            DramaFunctionTool.OptionsFunction($"modifPlayerName_{FullName[0]}_{FullName[1]}");
                            DramaFunctionTool.OptionsFunction("systemTips_Has been renamed:" + text + ", turn off interface refresh._5000");
                        }
                        else
                        {
                            DramaFunctionTool.OptionsFunction("systemTips_Name must have exactly 1 space._5000");
                        }
                        g.ui.CloseUI(inputField);
                    });
                    // Close textboxes
                    closeBtn.onClick.AddListener((Action)delegate{ g.ui.CloseUI(inputField); });
                }
            });
        }
    }
}
