using System;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace LuciferModifier
{
    public class CreateFace
    {
        public UICreatePlayer InitData(WorldUnitBase unit)
        {
            MelonLogger.Msg("Turn on portrait " + unit.data.unitData.propertyData.GetName());
            Patch_UICreatePlayer_DestroyUI.onCreateFace = true;
            UICreatePlayer ui = g.ui.OpenUI<UICreatePlayer>(UIType.CreatePlayer);
            ui.transform.Find("Root/Meum").gameObject.SetActive(value: false);
            ui.transform.Find("Root/Group:Facade/LanguageGroup").gameObject.SetActive(value: false);
            ui.transform.Find("Root/Group:Facade/InTrait").gameObject.SetActive(value: false);
            ui.transform.Find("Root/Group:Facade/OutTrait").gameObject.SetActive(value: false);
            ui.InitData(100, GameLevelType.Common, 1);
            Action action = delegate
            {
                try
                {
                    int sex = (int)unit.data.unitData.propertyData.sex;
                    ui.playerData.dynUnitData.sex.baseValue = sex;
                    bool flag = unit.data.unitData.propertyData.sex == UnitSexType.Woman;
                    MelonLogger.Msg("sex = " + sex + "          isWoman = " + flag);
                    if (flag)
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
                catch (Exception ex3)
                {
                    MelonLogger.Msg("Initialization model error：" + ex3.Message + "\n" + ex3.StackTrace, 99);
                }
                try
                {
                    ui.uiFacade.UpdateModelData();
                }
                catch (Exception ex4)
                {
                    MelonLogger.Msg("Refresh model error：" + ex4.Message + "\n" + ex4.StackTrace, 99);
                }
                try
                {
                    ui.uiFacade.UpdateFacadeUI();
                }
                catch (Exception ex5)
                {
                    MelonLogger.Msg("Refresh interface error：" + ex5.Message + "\n" + ex5.StackTrace, 99);
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
            Action __9__4 = null;
            Action action2 = delegate
            {
                Action action5;
                if ((action5 = __9__4) == null)
                {
                    action5 = (__9__4 = delegate
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
                        catch (Exception ex)
                        {
                            MelonLogger.Msg("Error saving data：" + ex.Message + "\n" + ex.StackTrace, 99);
                        }
                        MelonLogger.Msg("Test 0");
                        try
                        {
                            MelonLogger.Msg("Test1");
                            g.ui.CloseUI(UIType.CreatePlayer);
                            MelonLogger.Msg("Test2");
                            UIMapMain uI = g.ui.GetUI<UIMapMain>(UIType.MapMain);
                            MelonLogger.Msg("Test3");
                            if (uI != null)
                            {
                                uI.uiPlayerInfo.OnPlayerEquipCloth();
                                MelonLogger.Msg("Test4");
                            }
                            MelonLogger.Msg("Test5");
                            SceneType.map.world.UpdatePlayerModel(updatePlayerModel: true);
                            MelonLogger.Msg("Test6");
                        }
                        catch (Exception ex2)
                        {
                            MelonLogger.Msg("Test7");
                            MelonLogger.Msg("Refresh interface error error：" + ex2.Message + "\n" + ex2.StackTrace, 99);
                        }
                    });
                }
                Action action6 = action5;
                g.ui.OpenUI<UICheckPopup>(UIType.CheckPopup).InitData(GameTool.LS("common_tishi"), "Are you sure you want to save the portrait and exit?", 2, action6);
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
                g.ui.OpenUI<UICheckPopup>(UIType.CheckPopup).InitData(GameTool.LS("common_tishi"), "Are you sure you want to exit directly? The portrait results will not be saved!", 2, action4);
            };
            gameObject.AddComponent<Button>().onClick.AddListener(action2);
            gameObject = CreateUI.NewText("Plastic surgery-" + unit.data.unitData.propertyData.GetName());
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
                g.ui.OpenUI<UICheckPopup>(UIType.CheckPopup).InitData(GameTool.LS("common_tishi"), "Are you sure you want to exit directly? The portrait results will not be saved!", 2, action3);
            };
            gameObject.AddComponent<Button>().onClick.AddListener(action2);
            return ui;
        }
    }
}
