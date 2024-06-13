using System;
using Il2CppSystem.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Patch
{
    public class CreateFace
    {
        public UICreatePlayer InitData(WorldUnitBase unit)
        {
            UICreatePlayer ui = g.ui.GetUI<UICreatePlayer>(UIType.CreatePlayer);
            if (ui != null)
            {
                return ui;
            }
            Patch_UICreatePlayer_DestroyUI.onCreateFace = true;
            ui = g.ui.OpenUI<UICreatePlayer>(UIType.CreatePlayer);
            ui.transform.Find("Root/Meum").localScale = Vector3.zero;
            ui.facade.textLife.gameObject.SetActive(value: false);
            ui.facade.textCharm.gameObject.SetActive(value: false);
            ui.facade.textRace.gameObject.SetActive(value: false);
            ui.facade.textLevel.gameObject.SetActive(value: false);
            TMP_InputField inputName = ui.facade.goName.GetComponent<TMP_InputField>();
            _ = ui.facade.tglWoman;
            Toggle tglMan = ui.facade.tglMan;
            ui.InitData(100, GameLevelType.Common, g.data.world.npcCountId);
            Action action = delegate
            {
                try
                {
                    try
                    {
                        ui.playerData.dynUnitData.inTrait.baseValue = unit.data.unitData.propertyData.inTrait;
                        ui.playerData.dynUnitData.outTrait1.baseValue = unit.data.unitData.propertyData.outTrait1;
                        ui.playerData.dynUnitData.outTrait2.baseValue = unit.data.unitData.propertyData.outTrait2;
                        int num = 0;
                        int num2 = 0;
                        List<ConfRoleCreateCharacterItem>.Enumerator enumerator = g.conf.roleCreateCharacter._allConfList.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            ConfRoleCreateCharacterItem current = enumerator.Current;
                            if (current.type == 1)
                            {
                                if (current.id == unit.data.unitData.propertyData.inTrait)
                                {
                                    ui.facade.goInTraitRoot.transform.GetChild(num).GetComponent<Toggle>().isOn = true;
                                }
                                num++;
                            }
                            if (current.type == 2)
                            {
                                if (current.id == unit.data.unitData.propertyData.outTrait1 || current.id == unit.data.unitData.propertyData.outTrait2)
                                {
                                    ui.facade.goOutTrait.transform.GetChild(num2).GetComponent<Toggle>().isOn = true;
                                }
                                num2++;
                            }
                        }
                    }
                    catch (Exception ex5)
                    {
                        Console.WriteLine("Failed to initialize character " + ex5.ToString());
                    }
                    try
                    {
                        inputName.text = unit.data.unitData.propertyData.GetName();
                    }
                    catch (Exception ex6)
                    {
                        Console.WriteLine("InitName " + ex6.ToString());
                    }
                    int sex = (int)unit.data.unitData.propertyData.sex;
                    ui.playerData.dynUnitData.sex.baseValue = sex;
                    if (unit.data.unitData.propertyData.sex == UnitSexType.Woman)
                    {
                        ui.facade.tglMan.isOn = false;
                        ui.facade.tglWoman.isOn = true;
                    }
                    else
                    {
                        ui.facade.tglWoman.isOn = false;
                        ui.facade.tglMan.isOn = true;
                    }
                    ui.playerData.unitData.propertyData.battleModelData = unit.data.unitData.propertyData.battleModelData.Clone();
                    PortraitModelData portraitModelData = unit.data.unitData.propertyData.modelData.Clone();
                    ui.playerData.unitData.propertyData.modelData = portraitModelData;
                    List<UICreatePlayerFacade.FacadeItemData> list = (ui.uiFacade.tglMan.isOn ? ui.uiFacade.manDressItems : ui.uiFacade.womanDressItems);
                    list[0].SetValueInID(portraitModelData.hat);
                    list[1].SetValueInID(portraitModelData.head);
                    list[2].SetValueInID(portraitModelData.hair);
                    list[3].SetValueInID(portraitModelData.hairFront);
                    list[4].SetValueInID(portraitModelData.eyebrows);
                    list[5].SetValueInID(portraitModelData.eyes);
                    list[6].SetValueInID(portraitModelData.nose);
                    list[7].SetValueInID(portraitModelData.mouth);
                    list[8].SetValueInID(portraitModelData.body);
                    list[9].SetValueInID(portraitModelData.back);
                    if (portraitModelData.forehead != 0)
                    {
                        list[10].SetValueInID(portraitModelData.forehead);
                    }
                    else if (portraitModelData.faceLeft != 0)
                    {
                        list[10].SetValueInID(portraitModelData.faceLeft);
                    }
                    else if (portraitModelData.faceRight != 0)
                    {
                        list[10].SetValueInID(portraitModelData.faceRight);
                    }
                    else if (portraitModelData.faceFull != 0)
                    {
                        list[10].SetValueInID(portraitModelData.faceFull);
                    }
                    else
                    {
                        list[10].SetValueInID(0);
                    }
                    list[4].offsetY = portraitModelData.eyebrowsOffsetY;
                    list[5].offsetY = portraitModelData.eyesOffsetY;
                    list[6].offsetY = portraitModelData.noseOffsetY;
                    list[7].offsetY = portraitModelData.mouthOffsetY;
                }
                catch (Exception ex7)
                {
                    Console.WriteLine("Initialization model error：" + ex7.Message + "\n" + ex7.StackTrace);
                }
                try
                {
                    ui.uiFacade.UpdateModelData();
                }
                catch (Exception ex8)
                {
                    Console.WriteLine("Refresh model error：" + ex8.Message + "\n" + ex8.StackTrace);
                }
                try
                {
                    ui.uiFacade.UpdateFacadeUI();
                }
                catch (Exception ex9)
                {
                    Console.WriteLine("Refresh interface error：" + ex9.Message + "\n" + ex9.StackTrace);
                }
            };
            ui.AddCor(g.timer.Frame(action, 2));
            GameObject gameObject = CreateUI.NewImage(SpriteTool.GetSprite("Common", "kaishichuangguan"));
            gameObject.transform.SetParent(ui.transform, worldPositionStays: false);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(600f, -380f);
            GameObject gameObject2 = CreateUI.NewText("Finish", gameObject.GetComponent<RectTransform>().sizeDelta);
            gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
            gameObject2.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            gameObject2.GetComponent<Text>().color = Color.black;
            try
            {
                ui.playerData.dynUnitData.inTrait.baseValue = unit.data.unitData.propertyData.inTrait;
                ui.playerData.dynUnitData.outTrait1.baseValue = unit.data.unitData.propertyData.outTrait1;
                ui.playerData.dynUnitData.outTrait2.baseValue = unit.data.unitData.propertyData.outTrait2;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to initialize character " + ex.ToString());
            }
            Action action2 = delegate
            {
                Action action5 = delegate
                {
                    try
                    {
                        BattleModelHumanData battleModelData = ui.playerData.unitData.propertyData.battleModelData;
                        PortraitModelData modelData = ui.playerData.unitData.propertyData.modelData.Clone();
                        unit.data.unitData.propertyData.modelData = modelData;
                        unit.data.dynUnitData.modelData = modelData;
                        unit.data.unitData.propertyData.battleModelData = battleModelData;
                        unit.data.dynUnitData.battleModelData = unit.data.unitData.propertyData.battleModelData.Clone();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Error saving face pinching data：" + ex2.Message + "\n" + ex2.StackTrace);
                    }
                    try
                    {
                        unit.data.unitData.propertyData.sex = (tglMan.isOn ? UnitSexType.Man : UnitSexType.Woman);
                        string text = inputName.text;
                        if (!string.IsNullOrEmpty(text))
                        {
                            string text2 = text.Substring(0, 1);
                            string text3 = text.Substring(1, text.Length - 1);
                            unit.data.unitData.propertyData.name = new string[2] { text2, text3 };
                        }
                        unit.data.unitData.propertyData.inTrait = ui.playerData.dynUnitData.inTrait.baseValue;
                        unit.data.unitData.propertyData.outTrait1 = ui.playerData.dynUnitData.outTrait1.baseValue;
                        unit.data.unitData.propertyData.outTrait2 = ui.playerData.dynUnitData.outTrait2.baseValue;
                    }
                    catch (Exception ex3)
                    {
                        Console.WriteLine("Error saving attribute data：" + ex3.Message + "\n" + ex3.StackTrace);
                    }
                    try
                    {
                        g.ui.CloseUI(UIType.CreatePlayer);
                        UIMapMain uI = g.ui.GetUI<UIMapMain>(UIType.MapMain);
                        if (uI != null)
                        {
                            uI.uiPlayerInfo.OnPlayerEquipCloth();
                        }
                        SceneType.map.world.UpdatePlayerModel(updatePlayerModel: true);
                    }
                    catch (Exception ex4)
                    {
                        Console.WriteLine("Refresh interface error error：" + ex4.Message + "\n" + ex4.StackTrace);
                    }
                };
                g.ui.OpenUI<UICheckPopup>(UIType.CheckPopup).InitData(GameTool.LS("common_tishi"), GameTool.LS("Cave_BaocunNielian"), 2, action5);
            };
            gameObject.AddComponent<Button>().onClick.AddListener(action2);
            gameObject = CreateUI.NewImage(SpriteTool.GetSprite("Common", "tuichu"));
            gameObject.transform.SetParent(ui.transform, worldPositionStays: false);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(850f, 350f);
            action2 = delegate
            {
                Action action4 = delegate
                {
                    g.ui.CloseUI(UIType.CreatePlayer);
                };
                g.ui.OpenUI<UICheckPopup>(UIType.CheckPopup).InitData(GameTool.LS("common_tishi"), "Are you sure you want to exit directly? Modified data will not be saved!", 2, action4);
            };
            gameObject.AddComponent<Button>().onClick.AddListener(action2);
            gameObject = CreateUI.NewText("Portrait modification：" + unit.data.unitData.propertyData.GetName());
            gameObject.transform.SetParent(ui.transform, worldPositionStays: false);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 380f);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 50f);
            gameObject.GetComponent<Text>().color = Color.black;
            gameObject.GetComponent<Text>().fontSize = 30;
            gameObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            action2 = delegate
            {
                Action action3 = delegate
                {
                    g.ui.CloseUI(UIType.CreatePlayer);
                };
                g.ui.OpenUI<UICheckPopup>(UIType.CheckPopup).InitData(GameTool.LS("common_tishi"), GameTool.LS("Cave_TuichuNielian"), 2, action3);
            };
            gameObject.AddComponent<Button>().onClick.AddListener(action2);
            return ui;
        }
    }
}
