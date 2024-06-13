using System;
using System.Collections.Generic;
using System.Diagnostics;
using Il2CppSystem.Collections.Generic;
using LuciferModifier;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LuciferModifier
{
    public class LuciferSystem : ModifierMain
    {
        public static GameObject root;

        public static Transform transformTittle;

        public static Text componentTittle;

        public static Il2CppSystem.Collections.Generic.List<ConfRoleCreateFeatureItem> allFeature = g.conf.roleCreateFeature._allConfList;

        public static Il2CppSystem.Collections.Generic.List<ConfLocalTextItem> allText = g.conf.localText.allConfList;

        public static Il2CppSystem.Collections.Generic.IReadOnlyList<ConfRoleCreateCharacterItem> allRCC = g.conf.roleCreateCharacter.allConfList;

        public static Il2CppSystem.Collections.Generic.List<ConfArtifactSpriteItem> allSprite = g.conf.artifactSprite._allConfList;

        public static GameObject rootBg;

        private static Vector2 dragOffset;

        public static void RestUIPosition()
        {
            if (Screen.width > 2000)
            {
                rootBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600f, -400f);
            }
            else
            {
                rootBg.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            }
        }

        public static void MainWindows()
        {
            DataUnit.PropertyData playerData = g.world.playerUnit.data.unitData.propertyData;
            Il2CppSystem.Collections.Generic.List<WorldUnitBase> allUnits2 = WorldInitNPCTool.GetAllUnits();
            Il2CppSystem.Collections.Generic.Dictionary<string, WorldUnitBase> allUnit = g.world.unit.allUnit;
            Il2CppSystem.Collections.Generic.List<WorldUnitBase> allUnits = new Il2CppSystem.Collections.Generic.List<WorldUnitBase>();
            Il2CppSystem.Collections.Generic.Dictionary<string, WorldUnitBase>.Enumerator enumerator = allUnit.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Il2CppSystem.Collections.Generic.KeyValuePair<string, WorldUnitBase> current = enumerator.Current;
                allUnits.Add(current.value);
            }
            Il2CppSystem.Collections.Generic.Dictionary<string, MapBuildBase> allBuild = g.world.build.allBuildInID;
            Il2CppSystem.Collections.Generic.Dictionary<string, DataBuildSchool.SchoolData> allSchool = g.data.buildSchool.allBuild;
            System.Collections.Generic.List<Transform> list = new System.Collections.Generic.List<Transform>();
            DramaFunction LuciferDrama = new DramaFunction();
            root = CreateUI.NewCanvas();
            root.name = "LuciferSystem";
            GameObject uiPanelRoot = CreateUI.NewImage(SpriteTool.GetSpriteBigTex("BG/huodebg"));
            uiPanelRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(1280f, 960f);
            uiPanelRoot.transform.SetParent(root.transform, worldPositionStays: false);
            uiPanelRoot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            GameObject uiPanel = new GameObject();
            uiPanel.transform.SetParent(uiPanelRoot.transform, worldPositionStays: false);
            uiPanel.AddComponent<RectTransform>().localPosition = new Vector2(-50f, 100f);
            Transform transform = CreateUI.NewText("<color=#004FCA>" + playerData.GetName() + "</color>The realm of clouds", new Vector2(1000f, 200f)).transform;
            transform.SetParent(uiPanel.transform, worldPositionStays: false);
            transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 300f);
            Text component = transform.GetComponent<Text>();
            component.fontSize = 48;
            component.alignment = TextAnchor.MiddleCenter;
            component.color = Color.black;
            rootBg = uiPanelRoot.gameObject;
            RectTransform tf2 = uiPanelRoot.GetComponent<RectTransform>();
            if (PlayerPrefs.HasKey("Lucifer.bgPos"))
            {
                string @string = PlayerPrefs.GetString("Lucifer.bgPos");
                try
                {
                    string[] array = @string.Split('|');
                    Vector2 anchoredPosition = new Vector2(int.Parse(array[0]), int.Parse(array[1]));
                    tf2.anchoredPosition = anchoredPosition;
                }
                catch (Exception)
                {
                    RestUIPosition();
                }
            }
            else
            {
                RestUIPosition();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.BeginDrag;
            Action<BaseEventData> action = delegate
            {
                dragOffset = tf2.anchoredPosition - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            };
            Action<BaseEventData> action2 = delegate
            {
                tf2.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y) + dragOffset;
                string text4 = tf2.anchoredPosition.x.ToString();
                string text5 = "|";
                string value = text4 + text5 + uiPanelRoot.GetComponent<RectTransform>().anchoredPosition.y;
                PlayerPrefs.SetString("Lucifer.bgPos", value);
            };
            Action<BaseEventData> action3 = delegate
            {
            };
            entry.callback.AddListener(action);
            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.Drag;
            entry2.callback.AddListener(action2);
            EventTrigger.Entry entry3 = new EventTrigger.Entry();
            entry3.eventID = EventTriggerType.EndDrag;
            entry3.callback.AddListener(action3);
            EventTrigger eventTrigger = uiPanelRoot.AddComponent<EventTrigger>();
            eventTrigger.triggers.Add(entry);
            eventTrigger.triggers.Add(entry2);
            eventTrigger.triggers.Add(entry3);
            Transform transform2 = CreateUI.NewText("作者：<color=#004FCA>Lucifer</color>   Version：<color=#004FCA>" + ModifierMain.BanBenHao + "</color>", new Vector2(1000f, 100f)).transform;
            transform2.SetParent(uiPanel.transform, worldPositionStays: false);
            transform2.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, -300f);
            Text component2 = transform2.GetComponent<Text>();
            component2.fontSize = 24;
            component2.alignment = TextAnchor.MiddleCenter;
            component2.color = Color.black;
            transformTittle = CreateUI.NewText("Please click the option below <color=#FF0000>[Panel can be dragged]</color>", new Vector2(500f, 100f)).transform;
            transformTittle.SetParent(uiPanel.transform, worldPositionStays: false);
            transformTittle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 250f);
            componentTittle = transformTittle.GetComponent<Text>();
            componentTittle.fontSize = 28;
            componentTittle.alignment = TextAnchor.MiddleCenter;
            componentTittle.color = Color.black;
            GameObject gameObject = CreateUI.NewImage(SpriteTool.GetSprite("Common", "tuichu"));
            gameObject.transform.SetParent(uiPanel.transform, worldPositionStays: false);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(540f, 320f);
            Action action4 = delegate
            {
                UnityEngine.Object.Destroy(uiPanel);
                ModifierMain.IsChange = !ModifierMain.IsChange;
            };
            gameObject.AddComponent<Button>().onClick.AddListener(action4);
            Transform transform3 = CreateUI.NewButton(delegate
            {
                Process.Start("https://mod.3dmgame.com/u/6969270/Home");
            }).transform;
            transform3.SetParent(uiPanel.transform, worldPositionStays: false);
            transform3.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
            transform3.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -300f);
            Transform transform4 = CreateUI.NewText("Other modules").transform;
            transform4.SetParent(transform3, worldPositionStays: false);
            transform4.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            transform4.GetComponent<Text>().color = Color.black;
            Transform transform5 = CreateUI.NewButton(delegate
            {
                Process.Start("https://steamcommunity.com/profiles/76561198192223818");
            }).transform;
            transform5.SetParent(uiPanel.transform, worldPositionStays: false);
            transform5.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
            transform5.GetComponent<RectTransform>().anchoredPosition = new Vector2(260f, -300f);
            Transform transform6 = CreateUI.NewText("My Steam", new Vector2(150f, 50f)).transform;
            transform6.SetParent(transform5, worldPositionStays: false);
            transform6.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            transform6.GetComponent<Text>().color = Color.black;
            Transform transform7 = CreateUI.NewButton(delegate
            {
                Process.Start("https://qm.qq.com/cgi-bin/qm/qr?k=z1hhox6DgGi08MBMYg6p9BLfjm8luk9-&jump_from=webapi&authKey=4a0ppDJnq1g8W8XRqDNovPzp+uPCzt3OM4Tej50LeSjPKob30cHxWSUCY1NV2dYk");
            }).transform;
            transform7.SetParent(uiPanel.transform, worldPositionStays: false);
            transform7.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
            transform7.GetComponent<RectTransform>().anchoredPosition = new Vector2(420f, -300f);
            Transform transform8 = CreateUI.NewText("Join Group").transform;
            transform8.SetParent(transform7, worldPositionStays: false);
            transform8.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            transform8.GetComponent<Text>().color = Color.black;
            Transform transform9 = CreateUI.NewButton(delegate
            {
                GameObject riZhi = CreateUI.NewImage(SpriteTool.GetSpriteBigTex("BG/huodebg"));
                riZhi.GetComponent<RectTransform>().sizeDelta = new Vector2(1000f, 800f);
                riZhi.transform.SetParent(uiPanel.transform, worldPositionStays: false);
                riZhi.GetComponent<RectTransform>().anchoredPosition = new Vector2(40f, -50f);
                GameObject gameObject7 = UnityEngine.Object.Instantiate(g.ui.canvas.transform.Find("Root/UI/MapMain/Group:PlayerInfo/G:goPlayerInfo/G:btnPlayer/Image/G:rimgPlayerIcon").gameObject, riZhi.transform);
                gameObject7.GetComponent<RectTransform>().sizeDelta = new Vector2(275f, 400f);
                gameObject7.GetComponent<RectTransform>().anchoredPosition = new Vector2(270f, 10f);
                Transform transform292 = CreateUI.NewText("<color=#004FCA>Restart version</color>Update log", new Vector2(800f, 200f)).transform;
                transform292.SetParent(riZhi.transform, worldPositionStays: false);
                transform292.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f, 350f);
                transform292.GetComponent<Text>().fontSize = 28;
                transform292.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                transform292.GetComponent<Text>().color = Color.black;
                Transform transform293 = CreateUI.NewText("<color=#0000FF>Note:</color> In the future, this tool will only be updated in the 3DMMOD station and my Q group. Guigu’s creative workshop is too rubbish=. =\\nIf you later find that this tool is invalid again and there are bugs in some functions, you can add group feedback and I will update according to my mood\\n<color=#0000FF>1.0:</color>Updated to adapt to the Tianyuan Mountain version\\nFixed facelift Function failure problem\\nFixed the problem of killing all male and female players in the map\\nAdded shortcut options such as \"Update Log\", \"Other Modules\", \"My Steam\" and \"One-Click Group Join\"\\n nNew panel draggable function\\n<color=#0000FF>1.1:</color>You can click the button on the left side of the NPC information interface-name to copy the NPC name to the clipboard. You can paste it directly when using the built-in\\nUpdate Adapted to the official version\\n<color=#0000FF>Follow-up plan:</color> There is no follow-up update plan yet\n", new Vector2(800f, 600f)).transform;
                transform293.SetParent(riZhi.transform, worldPositionStays: false);
                transform293.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f, 20f);
                transform293.GetComponent<Text>().fontSize = 18;
                transform293.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
                transform293.GetComponent<Text>().color = Color.black;
                GameObject gameObject8 = CreateUI.NewImage(SpriteTool.GetSprite("Common", "tuichu"));
                gameObject8.transform.SetParent(riZhi.transform, worldPositionStays: false);
                gameObject8.GetComponent<RectTransform>().anchoredPosition = new Vector2(375f, 347f);
                Action action5 = delegate
                {
                    UnityEngine.Object.Destroy(riZhi);
                };
                gameObject8.AddComponent<Button>().onClick.AddListener(action5);
            }).transform;
            transform9.SetParent(uiPanel.transform, worldPositionStays: false);
            transform9.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
            transform9.GetComponent<RectTransform>().anchoredPosition = new Vector2(-60f, -300f);
            Transform transform10 = CreateUI.NewText("Change log").transform;
            transform10.SetParent(transform9, worldPositionStays: false);
            transform10.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            transform10.GetComponent<Text>().color = Color.black;
            bool flag = true;
            if (flag)
            {
                Transform transform11 = CreateUI.NewButton(delegate
                {
                    if (uiPanel.transform.Find("chuansong") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("chuansong").gameObject);
                    }
                    else if (uiPanel.transform.Find("gaiMingZi") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("gaiMingZi").gameObject);
                    }
                    else if (uiPanel.transform.Find("shaQuanjia") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("shaQuanjia").gameObject);
                    }
                    else if (uiPanel.transform.Find("jieYuan") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("jieYuan").gameObject);
                    }
                    else if (uiPanel.transform.Find("zaXiang") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("zaXiang").gameObject);
                    }
                    else if (uiPanel.transform.Find("XingGeuiPanel") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("XingGeuiPanel").gameObject);
                    }
                    else if (uiPanel.transform.Find("QiLing") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("QiLing").gameObject);
                    }
                    else if (uiPanel.transform.Find("ZongMen") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("ZongMen").gameObject);
                    }
                    if (uiPanel.transform.Find("gaiMingZi") == null)
                    {
                        GameObject gameObject6 = new GameObject("gaiMingZi");
                        gameObject6.transform.SetParent(uiPanel.transform, worldPositionStays: false);
                        componentTittle.text = "****Change name****";
                        Transform transform275 = CreateUI.NewText("player name", new Vector2(500f, 90f)).transform;
                        transform275.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform275.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 150f);
                        Text component29 = transform275.GetComponent<Text>();
                        component29.fontSize = 28;
                        component29.alignment = TextAnchor.MiddleCenter;
                        component29.color = Color.black;
                        Transform transform276 = CreateUI.NewInputField(null, null, "surname").transform;
                        transform276.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform276.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform276.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 100f);
                        InputField iptFieldXing = transform276.GetComponent<InputField>();
                        iptFieldXing.contentType = InputField.ContentType.Name;
                        iptFieldXing.text = g.world.playerUnit.data.unitData.propertyData.name[0];
                        iptFieldXing.characterLimit = 4;
                        Transform transform277 = CreateUI.NewInputField(null, null, "name").transform;
                        transform277.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform277.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform277.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 50f);
                        InputField iptFieldMing = transform277.GetComponent<InputField>();
                        iptFieldMing.contentType = InputField.ContentType.Name;
                        iptFieldMing.text = g.world.playerUnit.data.unitData.propertyData.name[1];
                        iptFieldMing.characterLimit = 4;
                        Transform transform278 = CreateUI.NewButton(delegate
                        {
                            g.world.playerUnit.data.unitData.propertyData.name = new Il2CppStringArray(new string[2] { iptFieldXing.text, iptFieldMing.text });
                        }).transform;
                        transform278.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform278.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f, 0f);
                        Transform transform279 = CreateUI.NewText("Change name").transform;
                        transform279.SetParent(transform278, worldPositionStays: false);
                        transform279.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform279.GetComponent<Text>().color = Color.black;
                        Transform transform280 = CreateUI.NewButton(delegate
                        {
                            new CreateFace().InitData(g.world.playerUnit);
                            UnityEngine.Object.Destroy(uiPanel);
                            ModifierMain.IsChange = !ModifierMain.IsChange;
                        }).transform;
                        transform280.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform280.GetComponent<RectTransform>().anchoredPosition = new Vector2(50f, 0f);
                        Transform transform281 = CreateUI.NewText("Plastic surgery").transform;
                        transform281.SetParent(transform280, worldPositionStays: false);
                        transform281.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform281.GetComponent<Text>().color = Color.black;
                        Transform transform282 = CreateUI.NewText("Please enter the name to search for NPC first. If you change the person, please search again. NPC interface - the button to the left of the name can copy the NPC name to the clipboard", new Vector2(1200f, 100f)).transform;
                        transform282.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform282.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -250f);
                        Text componentResult = transform282.GetComponent<Text>();
                        componentResult.fontSize = 21;
                        componentResult.alignment = TextAnchor.MiddleCenter;
                        componentResult.color = Color.black;
                        Transform transform283 = CreateUI.NewText("NPC name", new Vector2(500f, 90f)).transform;
                        transform283.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform283.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -50f);
                        Text component30 = transform283.GetComponent<Text>();
                        component30.fontSize = 28;
                        component30.alignment = TextAnchor.MiddleCenter;
                        component30.color = Color.black;
                        Transform transform284 = CreateUI.NewInputField(null, null, "surname").transform;
                        transform284.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform284.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform284.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -100f);
                        InputField iptFieldXing2 = transform284.GetComponent<InputField>();
                        iptFieldXing2.contentType = InputField.ContentType.Name;
                        iptFieldXing2.characterLimit = 4;
                        Transform transform285 = CreateUI.NewInputField(null, null, "name").transform;
                        transform285.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform285.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform285.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -150f);
                        InputField iptFieldMing2 = transform285.GetComponent<InputField>();
                        iptFieldMing2.contentType = InputField.ContentType.Name;
                        iptFieldMing2.characterLimit = 4;
                        bool hasOne = false;
                        WorldUnitBase tarUnit = null;
                        Action clickAction27 = delegate
                        {
                            tarUnit = null;
                            hasOne = false;
                            Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator75 = allUnits.GetEnumerator();
                            while (enumerator75.MoveNext())
                            {
                                WorldUnitBase current75 = enumerator75.Current;
                                if (current75.data.unitData.propertyData.GetName() == iptFieldXing2.text + iptFieldMing2.text)
                                {
                                    hasOne = true;
                                    tarUnit = current75;
                                    break;
                                }
                            }
                            if (hasOne)
                            {
                                if (tarUnit.data.school != null)
                                {
                                    componentResult.text = "Searched<color=#004FCA>" + iptFieldXing2.text + iptFieldMing2.text + "</color>," + tarUnit.data.school.GetName(isAddSuffix: true) + ", please click Facelift or enter a new name and click Change Name";
                                }
                                else
                                {
                                    componentResult.text = "Searched<color=#004FCA>" + iptFieldXing2.text + iptFieldMing2.text + "</color>，Individuals, please click on plastic surgery or enter a new name and click on change name";
                                }
                            }
                            else
                            {
                                componentResult.text = "Not found<color=#004FCA>" + iptFieldXing2.text + iptFieldMing2.text + "</color>";
                            }
                        };
                        Transform transform286 = CreateUI.NewButton(clickAction27).transform;
                        transform286.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform286.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -200f);
                        Transform transform287 = CreateUI.NewText("search").transform;
                        transform287.SetParent(transform286, worldPositionStays: false);
                        transform287.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform287.GetComponent<Text>().color = Color.black;
                        Action clickAction28 = delegate
                        {
                            if (!hasOne || tarUnit == null)
                            {
                                componentResult.text = "Please search for the NPC and click to change the name!";
                            }
                            else
                            {
                                componentResult.text = "<color=#004FCA>" + tarUnit.data.unitData.propertyData.GetName() + "</color>Name has been changed to<color=#004FCA>" + iptFieldXing2.text + iptFieldMing2.text + "</color>";
                                tarUnit.data.unitData.propertyData.name = new Il2CppStringArray(new string[2] { iptFieldXing2.text, iptFieldMing2.text });
                            }
                        };
                        Transform transform288 = CreateUI.NewButton(clickAction28).transform;
                        transform288.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform288.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
                        Transform transform289 = CreateUI.NewText("Change name").transform;
                        transform289.SetParent(transform288, worldPositionStays: false);
                        transform289.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform289.GetComponent<Text>().color = Color.black;
                        Transform transform290 = CreateUI.NewButton(delegate
                        {
                            new CreateFace().InitData(tarUnit);
                            UnityEngine.Object.Destroy(uiPanel);
                            ModifierMain.IsChange = !ModifierMain.IsChange;
                        }).transform;
                        transform290.SetParent(gameObject6.transform, worldPositionStays: false);
                        transform290.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -200f);
                        Transform transform291 = CreateUI.NewText("Plastic surgery").transform;
                        transform291.SetParent(transform290, worldPositionStays: false);
                        transform291.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform291.GetComponent<Text>().color = Color.black;
                    }
                }).transform;
                list.Add(transform11);
                if (list.Contains(transform11))
                {
                    Transform transform12 = CreateUI.NewText("Change name").transform;
                    transform12.SetParent(transform11.transform, worldPositionStays: false);
                    transform12.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    transform12.GetComponent<Text>().color = Color.black;
                }
            }
            if (flag)
            {
                Transform transform13 = CreateUI.NewButton(delegate
                {
                    if (uiPanel.transform.Find("chuansong") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("chuansong").gameObject);
                    }
                    else if (uiPanel.transform.Find("gaiMingZi") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("gaiMingZi").gameObject);
                    }
                    else if (uiPanel.transform.Find("shaQuanjia") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("shaQuanjia").gameObject);
                    }
                    else if (uiPanel.transform.Find("jieYuan") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("jieYuan").gameObject);
                    }
                    else if (uiPanel.transform.Find("zaXiang") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("zaXiang").gameObject);
                    }
                    else if (uiPanel.transform.Find("XingGeuiPanel") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("XingGeuiPanel").gameObject);
                    }
                    else if (uiPanel.transform.Find("QiLing") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("QiLing").gameObject);
                    }
                    else if (uiPanel.transform.Find("ZongMen") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("ZongMen").gameObject);
                    }
                    if (uiPanel.transform.Find("shaQuanjia") == null)
                    {
                        GameObject shaQuanjia = new GameObject("shaQuanjia");
                        shaQuanjia.transform.SetParent(uiPanel.transform, worldPositionStays: false);
                        componentTittle.text = "****Kill****";
                        Transform transform242 = CreateUI.NewText("Please enter the name to search for NPC. If you change the person, please search again.", new Vector2(800f, 100f)).transform;
                        transform242.SetParent(shaQuanjia.transform, worldPositionStays: false);
                        transform242.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                        Text componentResult = transform242.GetComponent<Text>();
                        componentResult.fontSize = 24;
                        componentResult.alignment = TextAnchor.MiddleCenter;
                        componentResult.color = Color.black;
                        Transform transform243 = CreateUI.NewText("Party Name", new Vector2(500f, 90f)).transform;
                        transform243.SetParent(shaQuanjia.transform, worldPositionStays: false);
                        transform243.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 150f);
                        Text component28 = transform243.GetComponent<Text>();
                        component28.fontSize = 28;
                        component28.alignment = TextAnchor.MiddleCenter;
                        component28.color = Color.black;
                        Transform transform244 = CreateUI.NewInputField(null, null, "Name").transform;
                        transform244.SetParent(shaQuanjia.transform, worldPositionStays: false);
                        transform244.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
                        transform244.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 100f);
                        InputField iptFieldXing = transform244.GetComponent<InputField>();
                        iptFieldXing.contentType = InputField.ContentType.Name;
                        iptFieldXing.characterLimit = 6;
                        bool hasOne = false;
                        WorldUnitBase tarUnit = null;
                        string isDie = null;
                        Action clickAction15 = delegate
                        {
                            hasOne = false;
                            tarUnit = null;
                            Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator61 = allUnits.GetEnumerator();
                            while (enumerator61.MoveNext())
                            {
                                WorldUnitBase current61 = enumerator61.Current;
                                if (current61.data.unitData.propertyData.GetName() == iptFieldXing.text)
                                {
                                    hasOne = true;
                                    tarUnit = current61;
                                    break;
                                }
                            }
                            if (hasOne)
                            {
                                if (tarUnit.isDie)
                                {
                                    isDie = "dead";
                                }
                                else
                                {
                                    isDie = "not dead";
                                }
                                if (tarUnit.data.school != null)
                                {
                                    componentResult.text = "Searched<color=#004FCA>" + iptFieldXing.text + "</color>," + tarUnit.data.school.GetName(isAddSuffix: true) + "," + isDie;
                                }
                                else
                                {
                                    componentResult.text = "Searched<color=#004FCA>" + iptFieldXing.text + "</color>,San people," + isDie;
                                }
                                Action clickAction16 = delegate
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator74 = allUnits.GetEnumerator();
                                    while (enumerator74.MoveNext())
                                    {
                                        WorldUnitBase current74 = enumerator74.Current;
                                        if (tarUnit.data.unitData.relationData.lover.Contains(current74.data.unitData.unitID) && !current74.isDie)
                                        {
                                            current74.CreateAction(new UnitActionRoleKill(current74, isMercy: false));
                                            current74.Destroy();
                                            current74.Die();
                                        }
                                    }
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>All the Taoist companions have been killed! ";
                                };
                                Action clickAction17 = delegate
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator73 = allUnits.GetEnumerator();
                                    while (enumerator73.MoveNext())
                                    {
                                        WorldUnitBase current73 = enumerator73.Current;
                                        if (tarUnit.data.unitData.relationData.married.Contains(current73.data.unitData.unitID) && !current73.isDie)
                                        {
                                            current73.CreateAction(new UnitActionRoleKill(current73, isMercy: false));
                                            current73.Destroy();
                                            current73.Die();
                                        }
                                    }
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>All the couples have been killed! ";
                                };
                                Action clickAction18 = delegate
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator72 = allUnits.GetEnumerator();
                                    while (enumerator72.MoveNext())
                                    {
                                        WorldUnitBase current72 = enumerator72.Current;
                                        if ((tarUnit.data.unitData.relationData.parent.Contains(current72.data.unitData.unitID) || tarUnit.data.unitData.relationData.parentBack.Contains(current72.data.unitData.unitID)) && !current72.isDie)
                                        {
                                            current72.CreateAction(new UnitActionRoleKill(current72, isMercy: false));
                                            current72.Destroy();
                                            current72.Die();
                                        }
                                    }
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>His parents have been killed! ";
                                };
                                Action clickAction19 = delegate
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator71 = allUnits.GetEnumerator();
                                    while (enumerator71.MoveNext())
                                    {
                                        WorldUnitBase current71 = enumerator71.Current;
                                        if ((tarUnit.data.unitData.relationData.children.Contains(current71.data.unitData.unitID) || tarUnit.data.unitData.relationData.childrenBack.Contains(current71.data.unitData.unitID) || tarUnit.data.unitData.relationData.childrenPrivate.Contains(current71.data.unitData.unitID)) && !current71.isDie)
                                        {
                                            current71.CreateAction(new UnitActionRoleKill(current71, isMercy: false));
                                            current71.Destroy();
                                            current71.Die();
                                        }
                                    }
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>All his children have been killed! ";
                                };
                                Action clickAction20 = delegate
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator70 = allUnits.GetEnumerator();
                                    while (enumerator70.MoveNext())
                                    {
                                        WorldUnitBase current70 = enumerator70.Current;
                                        if (tarUnit.data.unitData.relationData.master.Contains(current70.data.unitData.unitID) && !current70.isDie)
                                        {
                                            current70.CreateAction(new UnitActionRoleKill(current70, isMercy: false));
                                            current70.Destroy();
                                            current70.Die();
                                        }
                                    }
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>All the masters have been killed! ";
                                };
                                Action clickAction21 = delegate
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator69 = allUnits.GetEnumerator();
                                    while (enumerator69.MoveNext())
                                    {
                                        WorldUnitBase current69 = enumerator69.Current;
                                        if (tarUnit.data.unitData.relationData.student.Contains(current69.data.unitData.unitID) && !current69.isDie)
                                        {
                                            current69.CreateAction(new UnitActionRoleKill(current69, isMercy: false));
                                            current69.Destroy();
                                            current69.Die();
                                        }
                                    }
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>All his disciples have been killed! ";
                                };
                                Action clickAction22 = delegate
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator68 = allUnits.GetEnumerator();
                                    while (enumerator68.MoveNext())
                                    {
                                        WorldUnitBase current68 = enumerator68.Current;
                                        if ((tarUnit.data.unitData.relationData.brother.Contains(current68.data.unitData.unitID) || tarUnit.data.unitData.relationData.brotherBack.Contains(current68.data.unitData.unitID)) && !current68.isDie)
                                        {
                                            current68.CreateAction(new UnitActionRoleKill(current68, isMercy: false));
                                            current68.Destroy();
                                            current68.Die();
                                        }
                                    }
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>All the brothers and sisters have been killed! ";
                                };
                                Action clickAction23 = delegate
                                {
                                    int num14 = 0;
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator67 = allUnits.GetEnumerator();
                                    while (enumerator67.MoveNext())
                                    {
                                        WorldUnitBase current67 = enumerator67.Current;
                                        if ((tarUnit.data.unitData.relationData.brother.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.brotherBack.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.children.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.childrenBack.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.childrenPrivate.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.lover.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.married.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.master.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.parent.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.parentBack.Contains(current67.data.unitData.unitID) || tarUnit.data.unitData.relationData.student.Contains(current67.data.unitData.unitID)) && !current67.isDie)
                                        {
                                            current67.CreateAction(new UnitActionRoleKill(current67, isMercy: false));
                                            current67.Destroy();
                                            current67.Die();
                                            num14++;
                                        }
                                    }
                                    MelonLogger.Msg("Killed" + num14 + "personal");
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>The whole family has been killed! ";
                                };
                                Action clickAction24 = delegate
                                {
                                    int num13 = 0;
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator66 = allUnits.GetEnumerator();
                                    while (enumerator66.MoveNext())
                                    {
                                        WorldUnitBase current66 = enumerator66.Current;
                                        if ((tarUnit.data.unitData.relationData.brother.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.brotherBack.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.children.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.childrenBack.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.childrenPrivate.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.lover.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.married.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.master.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.parent.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.parentBack.Contains(current66.data.unitData.unitID) || tarUnit.data.unitData.relationData.student.Contains(current66.data.unitData.unitID)) && !current66.isDie && current66.data.unitData.propertyData.sex == UnitSexType.Man)
                                        {
                                            current66.CreateAction(new UnitActionRoleKill(current66, isMercy: false));
                                            current66.Destroy();
                                            current66.Die();
                                            num13++;
                                        }
                                    }
                                    MelonLogger.Msg("Killed" + num13 + "personal");
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>All the male relatives have been killed! ";
                                };
                                Action clickAction25 = delegate
                                {
                                    int num12 = 0;
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator65 = allUnits.GetEnumerator();
                                    while (enumerator65.MoveNext())
                                    {
                                        WorldUnitBase current65 = enumerator65.Current;
                                        if ((tarUnit.data.unitData.relationData.brother.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.brotherBack.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.children.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.childrenBack.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.childrenPrivate.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.lover.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.married.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.master.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.parent.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.parentBack.Contains(current65.data.unitData.unitID) || tarUnit.data.unitData.relationData.student.Contains(current65.data.unitData.unitID)) && !current65.isDie && current65.data.unitData.propertyData.sex == UnitSexType.Woman)
                                        {
                                            current65.CreateAction(new UnitActionRoleKill(current65, isMercy: false));
                                            current65.Destroy();
                                            current65.Die();
                                            num12++;
                                        }
                                    }
                                    MelonLogger.Msg("Killed" + num12 + "personal");
                                    componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>All the female relatives have been killed! ";
                                };
                                Action clickAction26 = delegate
                                {
                                    if (tarUnit.data.school != null)
                                    {
                                        int num11 = 0;
                                        string schoolID = tarUnit.data.unitData.schoolID;
                                        Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator64 = allUnits.GetEnumerator();
                                        while (enumerator64.MoveNext())
                                        {
                                            WorldUnitBase current64 = enumerator64.Current;
                                            if (current64.data.unitData.schoolID == schoolID && !current64.isDie)
                                            {
                                                current64.CreateAction(new UnitActionRoleKill(current64, isMercy: false));
                                                current64.Destroy();
                                                current64.Die();
                                                num11++;
                                            }
                                        }
                                        componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>Total members of the sect" + num11 + "Everyone has been slaughtered!";
                                    }
                                    else
                                    {
                                        componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>The omnipresent sect!";
                                    }
                                };
                                Transform transform247 = CreateUI.NewButton(clickAction23).transform;
                                transform247.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform247.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -50f);
                                Transform transform248 = CreateUI.NewText("Kill entire family").transform;
                                transform248.SetParent(transform247, worldPositionStays: false);
                                transform248.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform248.GetComponent<Text>().color = Color.black;
                                Transform transform249 = CreateUI.NewButton(delegate
                                {
                                    if (!tarUnit.isDie)
                                    {
                                        tarUnit.CreateAction(new UnitActionRoleKill(tarUnit, isMercy: false));
                                        tarUnit.Destroy();
                                        tarUnit.Die();
                                        componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>has been killed! ";
                                    }
                                    else
                                    {
                                        componentResult.text = "<color=#004FCA>" + iptFieldXing.text + "</color>Dead! ";
                                    }
                                }).transform;
                                transform249.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform249.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -50f);
                                Transform transform250 = CreateUI.NewText("Kill each other").transform;
                                transform250.SetParent(transform249, worldPositionStays: false);
                                transform250.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform250.GetComponent<Text>().color = Color.black;
                                Transform transform251 = CreateUI.NewButton(clickAction17).transform;
                                transform251.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform251.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -100f);
                                Transform transform252 = CreateUI.NewText("Kill couple").transform;
                                transform252.SetParent(transform251, worldPositionStays: false);
                                transform252.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform252.GetComponent<Text>().color = Color.black;
                                Transform transform253 = CreateUI.NewButton(clickAction16).transform;
                                transform253.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform253.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -100f);
                                Transform transform254 = CreateUI.NewText("Kill companions").transform;
                                transform254.SetParent(transform253, worldPositionStays: false);
                                transform254.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform254.GetComponent<Text>().color = Color.black;
                                Transform transform255 = CreateUI.NewButton(clickAction18).transform;
                                transform255.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform255.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -100f);
                                Transform transform256 = CreateUI.NewText("Killing parents").transform;
                                transform256.SetParent(transform255, worldPositionStays: false);
                                transform256.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform256.GetComponent<Text>().color = Color.black;
                                Transform transform257 = CreateUI.NewButton(clickAction19).transform;
                                transform257.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform257.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -150f);
                                Transform transform258 = CreateUI.NewText("Kill children").transform;
                                transform258.SetParent(transform257, worldPositionStays: false);
                                transform258.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform258.GetComponent<Text>().color = Color.black;
                                Transform transform259 = CreateUI.NewButton(clickAction20).transform;
                                transform259.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform259.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -150f);
                                Transform transform260 = CreateUI.NewText("Kill master").transform;
                                transform260.SetParent(transform259, worldPositionStays: false);
                                transform260.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform260.GetComponent<Text>().color = Color.black;
                                Transform transform261 = CreateUI.NewButton(clickAction21).transform;
                                transform261.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform261.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -150f);
                                Transform transform262 = CreateUI.NewText("Kill apprentice").transform;
                                transform262.SetParent(transform261, worldPositionStays: false);
                                transform262.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform262.GetComponent<Text>().color = Color.black;
                                Transform transform263 = CreateUI.NewButton(clickAction22).transform;
                                transform263.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform263.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -200f);
                                Transform transform264 = CreateUI.NewText("Kill brothers").transform;
                                transform264.SetParent(transform263, worldPositionStays: false);
                                transform264.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform264.GetComponent<Text>().color = Color.black;
                                Transform transform265 = CreateUI.NewButton(clickAction24).transform;
                                transform265.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform265.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
                                Transform transform266 = CreateUI.NewText("Kill men").transform;
                                transform266.SetParent(transform265, worldPositionStays: false);
                                transform266.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform266.GetComponent<Text>().color = Color.black;
                                Transform transform267 = CreateUI.NewButton(clickAction25).transform;
                                transform267.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform267.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -200f);
                                Transform transform268 = CreateUI.NewText("Femicide").transform;
                                transform268.SetParent(transform267, worldPositionStays: false);
                                transform268.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform268.GetComponent<Text>().color = Color.black;
                                Transform transform269 = CreateUI.NewButton(clickAction26).transform;
                                transform269.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform269.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -50f);
                                Transform transform270 = CreateUI.NewText("Kill the clan").transform;
                                transform270.SetParent(transform269, worldPositionStays: false);
                                transform270.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform270.GetComponent<Text>().color = Color.black;
                                Transform transform271 = CreateUI.NewButton(delegate
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator63 = allUnits.GetEnumerator();
                                    while (enumerator63.MoveNext())
                                    {
                                        WorldUnitBase current63 = enumerator63.Current;
                                        if (current63.data.unitData.propertyData.sex == UnitSexType.Man && current63.data.unitData.propertyData.GetName() != playerData.GetName())
                                        {
                                            current63.CreateAction(new UnitActionRoleKill(current63, isMercy: false));
                                            current63.Destroy();
                                            current63.Die();
                                        }
                                    }
                                }).transform;
                                transform271.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform271.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                                transform271.GetComponent<RectTransform>().anchoredPosition = new Vector2(-75f, -250f);
                                Transform transform272 = CreateUI.NewText("Kill all men on the map", new Vector2(150f, 50f)).transform;
                                transform272.SetParent(transform271, worldPositionStays: false);
                                transform272.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform272.GetComponent<Text>().color = Color.black;
                                Transform transform273 = CreateUI.NewButton(delegate
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator62 = allUnits.GetEnumerator();
                                    while (enumerator62.MoveNext())
                                    {
                                        WorldUnitBase current62 = enumerator62.Current;
                                        if (current62.data.unitData.propertyData.sex == UnitSexType.Woman && current62.data.unitData.propertyData.GetName() != playerData.GetName())
                                        {
                                            current62.CreateAction(new UnitActionRoleKill(current62, isMercy: false));
                                            current62.Destroy();
                                            current62.Die();
                                        }
                                    }
                                }).transform;
                                transform273.SetParent(shaQuanjia.transform, worldPositionStays: false);
                                transform273.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                                transform273.GetComponent<RectTransform>().anchoredPosition = new Vector2(75f, -250f);
                                Transform transform274 = CreateUI.NewText("Kill all women on the map", new Vector2(150f, 50f)).transform;
                                transform274.SetParent(transform273, worldPositionStays: false);
                                transform274.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform274.GetComponent<Text>().color = Color.black;
                            }
                            else
                            {
                                componentResult.text = "Not found<color=#004FCA>" + iptFieldXing.text + "</color>";
                            }
                        };
                        Transform transform245 = CreateUI.NewButton(clickAction15).transform;
                        transform245.SetParent(shaQuanjia.transform, worldPositionStays: false);
                        transform245.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 50f);
                        Transform transform246 = CreateUI.NewText("search").transform;
                        transform246.SetParent(transform245.transform, worldPositionStays: false);
                        transform246.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform246.GetComponent<Text>().color = Color.black;
                    }
                }).transform;
                list.Add(transform13);
                if (list.Contains(transform13))
                {
                    Transform transform14 = CreateUI.NewText("kill").transform;
                    transform14.SetParent(transform13.transform, worldPositionStays: false);
                    transform14.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    transform14.GetComponent<Text>().color = Color.black;
                }
            }
            if (flag)
            {
                Transform transform15 = CreateUI.NewButton(delegate
                {
                    if (uiPanel.transform.Find("chuansong") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("chuansong").gameObject);
                    }
                    else if (uiPanel.transform.Find("gaiMingZi") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("gaiMingZi").gameObject);
                    }
                    else if (uiPanel.transform.Find("shaQuanjia") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("shaQuanjia").gameObject);
                    }
                    else if (uiPanel.transform.Find("jieYuan") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("jieYuan").gameObject);
                    }
                    else if (uiPanel.transform.Find("zaXiang") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("zaXiang").gameObject);
                    }
                    else if (uiPanel.transform.Find("XingGeuiPanel") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("XingGeuiPanel").gameObject);
                    }
                    else if (uiPanel.transform.Find("QiLing") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("QiLing").gameObject);
                    }
                    else if (uiPanel.transform.Find("ZongMen") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("ZongMen").gameObject);
                    }
                    if (uiPanel.transform.Find("chuansong") == null)
                    {
                        GameObject cuanSong = new GameObject("chuansong");
                        cuanSong.transform.SetParent(uiPanel.transform, worldPositionStays: false);
                        componentTittle.text = "****Transmit****";
                        Transform transform231 = CreateUI.NewText("Please enter the name to search for NPC. If you change the person, please search again.", new Vector2(800f, 100f)).transform;
                        transform231.SetParent(cuanSong.transform, worldPositionStays: false);
                        transform231.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -50f);
                        Text componentResult3 = transform231.GetComponent<Text>();
                        componentResult3.fontSize = 24;
                        componentResult3.alignment = TextAnchor.MiddleCenter;
                        componentResult3.color = Color.black;
                        Transform transform232 = CreateUI.NewText("Party Name", new Vector2(500f, 90f)).transform;
                        transform232.SetParent(cuanSong.transform, worldPositionStays: false);
                        transform232.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 100f);
                        Text component27 = transform232.GetComponent<Text>();
                        component27.fontSize = 28;
                        component27.alignment = TextAnchor.MiddleCenter;
                        component27.color = Color.black;
                        Transform transform233 = CreateUI.NewInputField(null, null, "Name").transform;
                        transform233.SetParent(cuanSong.transform, worldPositionStays: false);
                        transform233.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
                        transform233.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 50f);
                        InputField iptFieldXing3 = transform233.GetComponent<InputField>();
                        iptFieldXing3.contentType = InputField.ContentType.Name;
                        iptFieldXing3.characterLimit = 6;
                        bool hasOne2 = false;
                        WorldUnitBase tarUnit2 = null;
                        string isDie2 = null;
                        Action clickAction14 = delegate
                        {
                            tarUnit2 = null;
                            hasOne2 = false;
                            Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator60 = allUnits.GetEnumerator();
                            while (enumerator60.MoveNext())
                            {
                                WorldUnitBase current60 = enumerator60.Current;
                                if (current60.data.unitData.propertyData.GetName() == iptFieldXing3.text)
                                {
                                    hasOne2 = true;
                                    tarUnit2 = current60;
                                    break;
                                }
                            }
                            if (hasOne2)
                            {
                                if (tarUnit2.isDie)
                                {
                                    isDie2 = "dead";
                                }
                                else
                                {
                                    isDie2 = "not dead";
                                }
                                if (tarUnit2.data.school != null)
                                {
                                    componentResult3.text = "Searched<color=#004FCA>" + iptFieldXing3.text + "</color>," + tarUnit2.data.school.GetName(isAddSuffix: true) + "," + isDie2;
                                }
                                else
                                {
                                    componentResult3.text = "Searched<color=#004FCA>" + iptFieldXing3.text + "</color>,San people," + isDie2;
                                }
                                Transform transform238 = CreateUI.NewButton(delegate
                                {
                                    tarUnit2.data.unitData.SetPoint(g.world.playerUnit.data.unitData.GetPoint());
                                    componentResult3.text = "<color=#004FCA>" + iptFieldXing3.text + "</color>The transmission has been successful. Please click on any grid and click back.";
                                }).transform;
                                transform238.SetParent(cuanSong.transform, worldPositionStays: false);
                                transform238.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f, -100f);
                                Transform transform239 = CreateUI.NewText("pull over").transform;
                                transform239.SetParent(transform238, worldPositionStays: false);
                                transform239.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform239.GetComponent<Text>().color = Color.black;
                                Transform transform240 = CreateUI.NewButton(delegate
                                {
                                    UnityEngine.Object.Destroy(uiPanel);
                                    ModifierMain.IsChange = !ModifierMain.IsChange;
                                    g.world.playerUnit.CreateAction(new UnitActionSetPoint(tarUnit2.data.unitData.GetPoint()));
                                }).transform;
                                transform240.SetParent(cuanSong.transform, worldPositionStays: false);
                                transform240.GetComponent<RectTransform>().anchoredPosition = new Vector2(50f, -100f);
                                Transform transform241 = CreateUI.NewText("Pass it over").transform;
                                transform241.SetParent(transform240, worldPositionStays: false);
                                transform241.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform241.GetComponent<Text>().color = Color.black;
                            }
                            else
                            {
                                componentResult3.text = "Not found<color=#004FCA>" + iptFieldXing3.text + "</color>";
                            }
                        };
                        Transform transform234 = CreateUI.NewButton(clickAction14).transform;
                        transform234.SetParent(cuanSong.transform, worldPositionStays: false);
                        transform234.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                        Transform transform235 = CreateUI.NewText("search").transform;
                        transform235.SetParent(transform234.transform, worldPositionStays: false);
                        transform235.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform235.GetComponent<Text>().color = Color.black;
                        Transform transform236 = CreateUI.NewButton(delegate
                        {
                            MapBuildTownTransfer.OpenTransfer(null);
                            UnityEngine.Object.Destroy(uiPanel);
                            ModifierMain.IsChange = !ModifierMain.IsChange;
                        }).transform;
                        transform236.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform236.SetParent(cuanSong.transform, worldPositionStays: false);
                        transform236.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 150f);
                        Transform transform237 = CreateUI.NewText("Start teleportation array", new Vector2(150f, 50f)).transform;
                        transform237.SetParent(transform236, worldPositionStays: false);
                        transform237.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform237.GetComponent<Text>().color = Color.black;
                    }
                }).transform;
                list.Add(transform15);
                if (list.Contains(transform15))
                {
                    Transform transform16 = CreateUI.NewText("transmit").transform;
                    transform16.SetParent(transform15.transform, worldPositionStays: false);
                    transform16.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    transform16.GetComponent<Text>().color = Color.black;
                }
            }
            if (flag)
            {
                Transform transform17 = CreateUI.NewButton(delegate
                {
                    if (uiPanel.transform.Find("chuansong") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("chuansong").gameObject);
                    }
                    else if (uiPanel.transform.Find("gaiMingZi") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("gaiMingZi").gameObject);
                    }
                    else if (uiPanel.transform.Find("shaQuanjia") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("shaQuanjia").gameObject);
                    }
                    else if (uiPanel.transform.Find("jieYuan") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("jieYuan").gameObject);
                    }
                    else if (uiPanel.transform.Find("zaXiang") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("zaXiang").gameObject);
                    }
                    else if (uiPanel.transform.Find("XingGeuiPanel") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("XingGeuiPanel").gameObject);
                    }
                    else if (uiPanel.transform.Find("QiLing") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("QiLing").gameObject);
                    }
                    else if (uiPanel.transform.Find("ZongMen") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("ZongMen").gameObject);
                    }
                    if (uiPanel.transform.Find("jieYuan") == null)
                    {
                        GameObject jieYuan = new GameObject("jieYuan");
                        jieYuan.transform.SetParent(uiPanel.transform, worldPositionStays: false);
                        componentTittle.text = "****Become acquainted****";
                        Transform transform195 = CreateUI.NewText("Let any two characters become friends or enemies", new Vector2(500f, 90f)).transform;
                        transform195.SetParent(jieYuan.transform, worldPositionStays: false);
                        transform195.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 150f);
                        Text component26 = transform195.GetComponent<Text>();
                        component26.fontSize = 24;
                        component26.alignment = TextAnchor.MiddleCenter;
                        component26.color = Color.black;
                        Transform transform196 = CreateUI.NewInputField(null, null, "Character 1 name").transform;
                        transform196.SetParent(jieYuan.transform, worldPositionStays: false);
                        transform196.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
                        transform196.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 100f);
                        InputField iptFieldXing4 = transform196.GetComponent<InputField>();
                        iptFieldXing4.contentType = InputField.ContentType.Name;
                        iptFieldXing4.characterLimit = 8;
                        Transform transform197 = CreateUI.NewInputField(null, null, "Character 2 name").transform;
                        transform197.SetParent(jieYuan.transform, worldPositionStays: false);
                        transform197.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
                        transform197.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 50f);
                        InputField iptFieldXing41 = transform197.GetComponent<InputField>();
                        iptFieldXing41.text = g.world.playerUnit.data.unitData.propertyData.GetName();
                        iptFieldXing41.contentType = InputField.ContentType.Name;
                        iptFieldXing41.characterLimit = 8;
                        Transform transform198 = CreateUI.NewText("Please enter the names of two characters to search for NPCs. Please search again if you change characters.", new Vector2(800f, 100f)).transform;
                        transform198.SetParent(jieYuan.transform, worldPositionStays: false);
                        transform198.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -50f);
                        Text componentResult4 = transform198.GetComponent<Text>();
                        componentResult4.fontSize = 20;
                        componentResult4.alignment = TextAnchor.MiddleCenter;
                        componentResult4.color = Color.black;
                        bool hasOne4 = false;
                        bool hasOne41 = false;
                        WorldUnitBase tarUnit4 = null;
                        WorldUnitBase tarUnit41 = null;
                        string isDie4 = null;
                        string isDie41 = null;
                        Action clickAction = delegate
                        {
                            hasOne4 = false;
                            hasOne41 = false;
                            tarUnit4 = null;
                            tarUnit41 = null;
                            if (iptFieldXing4.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarUnit4 = g.world.playerUnit;
                                hasOne4 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator56 = allUnits.GetEnumerator();
                                while (enumerator56.MoveNext())
                                {
                                    WorldUnitBase current56 = enumerator56.Current;
                                    if (current56.data.unitData.propertyData.GetName() == iptFieldXing4.text)
                                    {
                                        hasOne4 = true;
                                        tarUnit4 = current56;
                                        break;
                                    }
                                }
                            }
                            if (iptFieldXing41.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarUnit41 = g.world.playerUnit;
                                hasOne41 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator57 = allUnits.GetEnumerator();
                                while (enumerator57.MoveNext())
                                {
                                    WorldUnitBase current57 = enumerator57.Current;
                                    if (current57.data.unitData.propertyData.GetName() == iptFieldXing41.text)
                                    {
                                        hasOne41 = true;
                                        tarUnit41 = current57;
                                        break;
                                    }
                                }
                            }
                            if (hasOne4 && hasOne41)
                            {
                                if (tarUnit4.isDie)
                                {
                                    isDie4 = "dead";
                                }
                                else
                                {
                                    isDie4 = "not dead";
                                }
                                if (tarUnit41.isDie)
                                {
                                    isDie41 = "dead";
                                }
                                else
                                {
                                    isDie41 = "not dead";
                                }
                                string text2 = null;
                                string text3 = null;
                                text2 = ((tarUnit4.data.school == null) ? "(San people)" : ("(" + tarUnit4.data.school.GetName(isAddSuffix: true) + ")"));
                                text3 = ((tarUnit41.data.school == null) ? "(San people)" : ("(" + tarUnit41.data.school.GetName(isAddSuffix: true) + ")"));
                                componentResult4.text = "Search successful! do you want<color=#004FCA>" + iptFieldXing4.text + "</color>" + text2 + "do<color=#004FCA>" + iptFieldXing41.text + "</color>" + text3 + "of...";
                                Action clickAction2 = delegate
                                {
                                    if (tarUnit4.data.unitData.propertyData.sex != tarUnit41.data.unitData.propertyData.sex)
                                    {
                                        if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                        {
                                            WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.Lover, 3000);
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color> Taoist companion! ";
                                        }
                                        else
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color> Relatives! ";
                                        }
                                    }
                                    else
                                    {
                                        componentResult4.text = "Zhang San refused to give in<color=#004FCA>" + iptFieldXing41.text + "</color>and<color=#004FCA>" + iptFieldXing4.text + "</color>Get gay! ";
                                    }
                                };
                                Action clickAction3 = delegate
                                {
                                    if (tarUnit4.data.unitData.propertyData.sex != tarUnit41.data.unitData.propertyData.sex)
                                    {
                                        if (tarUnit4.data.unitData.relationData.married == null && tarUnit41.data.unitData.relationData.married == null)
                                        {
                                            if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.Lover)
                                            {
                                                WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.Married, 3000);
                                                if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                                {
                                                    componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>s husband! ";
                                                }
                                                else
                                                {
                                                    componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>s wife! ";
                                                }
                                            }
                                            else if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                            {
                                                componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>and<color=#004FCA>" + iptFieldXing41.text + "</color>Not a Taoist couple yet (a prerequisite for being a couple)! ";
                                            }
                                            else
                                            {
                                                componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color>relatives! ";
                                            }
                                        }
                                        if (tarUnit4.data.unitData.relationData.married != null && tarUnit41.data.unitData.relationData.married != null)
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>and<color=#004FCA>" + iptFieldXing41.text + "</color>They are all married, Zhang San does not allow bigamy! ";
                                        }
                                        else if (tarUnit4.data.unitData.relationData.married != null)
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>Marriage, Zhang San does not allow bigamy! ";
                                        }
                                        else if (tarUnit41.data.unitData.relationData.married != null)
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing41.text + "</color>Marriage, Zhang San does not allow bigamy! ";
                                        }
                                    }
                                    else
                                    {
                                        componentResult4.text = "Zhang San refused to give in<color=#004FCA>" + iptFieldXing41.text + "</color>and<color=#004FCA>" + iptFieldXing4.text + "</color>Get gay! ";
                                    }
                                };
                                Action clickAction4 = delegate
                                {
                                    bool flag5 = true;
                                    if (tarUnit41.data.GetRelation(UnitBothRelationType.Parents).Count != 0)
                                    {
                                        Il2CppSystem.Collections.Generic.List<string>.Enumerator enumerator59 = tarUnit41.data.GetRelation(UnitBothRelationType.Parents).GetEnumerator();
                                        while (enumerator59.MoveNext())
                                        {
                                            string current59 = enumerator59.Current;
                                            if (tarUnit4.data.unitData.propertyData.sex == g.data.unit.GetUnit(current59).unit.data.unitData.propertyData.sex)
                                            {
                                                flag5 = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (flag5)
                                    {
                                        if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                        {
                                            WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.Children, 3000);
                                            if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                            {
                                                componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>father! ";
                                            }
                                            else
                                            {
                                                componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>mother! ";
                                            }
                                        }
                                        else
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color>relatives!";
                                        }
                                    }
                                    else if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing41.text + "</color>Already has a biological father!";
                                    }
                                    else
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing41.text + "</color>Already have a biological mother! ";
                                    }
                                };
                                Action clickAction5 = delegate
                                {
                                    bool flag4 = true;
                                    if (tarUnit4.data.GetRelation(UnitBothRelationType.Parents).Count != 0)
                                    {
                                        Il2CppSystem.Collections.Generic.List<string>.Enumerator enumerator58 = tarUnit4.data.GetRelation(UnitBothRelationType.Parents).GetEnumerator();
                                        while (enumerator58.MoveNext())
                                        {
                                            string current58 = enumerator58.Current;
                                            if (tarUnit41.data.unitData.propertyData.sex == g.data.unit.GetUnit(current58).unit.data.unitData.propertyData.sex)
                                            {
                                                flag4 = false;
                                                break;
                                            }
                                        }
                                    }
                                    if (flag4)
                                    {
                                        if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                        {
                                            WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.Parent, 3000);
                                            if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                            {
                                                componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>son!";
                                            }
                                            else
                                            {
                                                componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>daughter!";
                                            }
                                        }
                                        else
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color>relatives!";
                                        }
                                    }
                                    else if (tarUnit41.data.unitData.propertyData.sex == UnitSexType.Man)
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>Already has a biological father!";
                                    }
                                    else
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>Already have a biological mother!";
                                    }
                                };
                                Action clickAction6 = delegate
                                {
                                    if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                    {
                                        WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.Student, 3000);
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>The master!";
                                    }
                                    else
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color>relatives!";
                                    }
                                };
                                Action clickAction7 = delegate
                                {
                                    if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                    {
                                        WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.Master, 3000);
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>Disciple!";
                                    }
                                    else
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color>relatives!";
                                    }
                                };
                                Action clickAction8 = delegate
                                {
                                    if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                    {
                                        WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.ChildrenBack, 3000);
                                        if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>'s foster father!";
                                        }
                                        else
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>’s adoptive mother!";
                                        }
                                    }
                                    else
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color>'s relatives!";
                                    }
                                };
                                Action clickAction9 = delegate
                                {
                                    if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                    {
                                        WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.ParentBack, 3000);
                                        if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>'s adopted son! ";
                                        }
                                        else
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>’s adopted daughter! ";
                                        }
                                    }
                                    else
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color>'s relatives!";
                                    }
                                };
                                Action clickAction10 = delegate
                                {
                                    if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                    {
                                        if (tarUnit4.data.unitData.propertyData.sex == tarUnit41.data.unitData.propertyData.sex)
                                        {
                                            WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.BrotherBack, 3000);
                                            if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                            {
                                                componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>sworn brothers! ";
                                            }
                                            else
                                            {
                                                componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>sworn sisters! ";
                                            }
                                        }
                                        else
                                        {
                                            componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>and<color=#004FCA>" + iptFieldXing41.text + "</color>I don’t want to be together if my gender is different<color=#004FCA>" + iptFieldXing41.text + "</color>Become sworn brothers!";
                                        }
                                    }
                                    else
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color>relatives! ";
                                    }
                                };
                                Action clickAction11 = delegate
                                {
                                    if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) == UnitRelationType.None)
                                    {
                                        WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.None, 3000);
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>has become<color=#004FCA>" + iptFieldXing41.text + "</color>best friend! ";
                                    }
                                    else
                                    {
                                        componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>already<color=#004FCA>" + iptFieldXing41.text + "</color>relatives! ";
                                    }
                                };
                                Action clickAction12 = delegate
                                {
                                    if (tarUnit4.data.unitData.relationData.GetRelation(tarUnit41) != 0)
                                    {
                                        WorldInitNPCTool.SetRelationUnit(tarUnit4, tarUnit41, UnitRelationType.None, -3000);
                                    }
                                    tarUnit41.data.unitData.relationData.SetIntim(tarUnit4.data.unitData.unitID, 0f);
                                    tarUnit4.data.unitData.relationData.SetIntim(tarUnit41.data.unitData.unitID, 0f);
                                    componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>Has reconciled<color=#004FCA>" + iptFieldXing41.text + "</color>Endless love and righteousness! ";
                                };
                                Action clickAction13 = delegate
                                {
                                    tarUnit41.data.unitData.relationData.SetIntim(tarUnit4.data.unitData.unitID, -300f);
                                    tarUnit4.data.unitData.relationData.SetIntim(tarUnit41.data.unitData.unitID, -300f);
                                    componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>Has reconciled<color=#004FCA>" + iptFieldXing41.text + "</color>Turn against each other! ";
                                };
                                Transform transform201 = CreateUI.NewButton(clickAction2).transform;
                                transform201.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform201.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -100f);
                                Transform transform202 = CreateUI.NewText("couple").transform;
                                transform202.SetParent(transform201, worldPositionStays: false);
                                transform202.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform202.GetComponent<Text>().color = Color.black;
                                Transform transform203 = CreateUI.NewButton(clickAction3).transform;
                                transform203.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform203.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -100f);
                                if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                {
                                    Transform transform204 = CreateUI.NewText("husband").transform;
                                    transform204.SetParent(transform203, worldPositionStays: false);
                                    transform204.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform204.GetComponent<Text>().color = Color.black;
                                }
                                else
                                {
                                    Transform transform205 = CreateUI.NewText("wife").transform;
                                    transform205.SetParent(transform203, worldPositionStays: false);
                                    transform205.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform205.GetComponent<Text>().color = Color.black;
                                }
                                Transform transform206 = CreateUI.NewButton(clickAction4).transform;
                                transform206.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform206.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -100f);
                                if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                {
                                    Transform transform207 = CreateUI.NewText("Father").transform;
                                    transform207.SetParent(transform206, worldPositionStays: false);
                                    transform207.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform207.GetComponent<Text>().color = Color.black;
                                }
                                else
                                {
                                    Transform transform208 = CreateUI.NewText("Mother").transform;
                                    transform208.SetParent(transform206, worldPositionStays: false);
                                    transform208.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform208.GetComponent<Text>().color = Color.black;
                                }
                                Transform transform209 = CreateUI.NewButton(clickAction5).transform;
                                transform209.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform209.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -150f);
                                if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                {
                                    Transform transform210 = CreateUI.NewText("son").transform;
                                    transform210.SetParent(transform209, worldPositionStays: false);
                                    transform210.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform210.GetComponent<Text>().color = Color.black;
                                }
                                else
                                {
                                    Transform transform211 = CreateUI.NewText("daughter").transform;
                                    transform211.SetParent(transform209, worldPositionStays: false);
                                    transform211.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform211.GetComponent<Text>().color = Color.black;
                                }
                                Transform transform212 = CreateUI.NewButton(clickAction9).transform;
                                transform212.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform212.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -150f);
                                if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                {
                                    Transform transform213 = CreateUI.NewText("adopted son").transform;
                                    transform213.SetParent(transform212, worldPositionStays: false);
                                    transform213.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform213.GetComponent<Text>().color = Color.black;
                                }
                                else
                                {
                                    Transform transform214 = CreateUI.NewText("adopted daughter").transform;
                                    transform214.SetParent(transform212, worldPositionStays: false);
                                    transform214.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform214.GetComponent<Text>().color = Color.black;
                                }
                                Transform transform215 = CreateUI.NewButton(clickAction8).transform;
                                transform215.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform215.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -150f);
                                if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                {
                                    Transform transform216 = CreateUI.NewText("foster father").transform;
                                    transform216.SetParent(transform215, worldPositionStays: false);
                                    transform216.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform216.GetComponent<Text>().color = Color.black;
                                }
                                else
                                {
                                    Transform transform217 = CreateUI.NewText("foster mother").transform;
                                    transform217.SetParent(transform215, worldPositionStays: false);
                                    transform217.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform217.GetComponent<Text>().color = Color.black;
                                }
                                Transform transform218 = CreateUI.NewButton(clickAction6).transform;
                                transform218.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform218.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -200f);
                                Transform transform219 = CreateUI.NewText("master").transform;
                                transform219.SetParent(transform218, worldPositionStays: false);
                                transform219.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform219.GetComponent<Text>().color = Color.black;
                                Transform transform220 = CreateUI.NewButton(clickAction7).transform;
                                transform220.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform220.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
                                Transform transform221 = CreateUI.NewText("apprentice").transform;
                                transform221.SetParent(transform220, worldPositionStays: false);
                                transform221.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform221.GetComponent<Text>().color = Color.black;
                                Transform transform222 = CreateUI.NewButton(clickAction10).transform;
                                transform222.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform222.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -200f);
                                if (tarUnit4.data.unitData.propertyData.sex == UnitSexType.Man)
                                {
                                    Transform transform223 = CreateUI.NewText("brother").transform;
                                    transform223.SetParent(transform222, worldPositionStays: false);
                                    transform223.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform223.GetComponent<Text>().color = Color.black;
                                }
                                else
                                {
                                    Transform transform224 = CreateUI.NewText("sister").transform;
                                    transform224.SetParent(transform222, worldPositionStays: false);
                                    transform224.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                    transform224.GetComponent<Text>().color = Color.black;
                                }
                                Transform transform225 = CreateUI.NewButton(clickAction11).transform;
                                transform225.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform225.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -250f);
                                Transform transform226 = CreateUI.NewText("friends").transform;
                                transform226.SetParent(transform225, worldPositionStays: false);
                                transform226.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform226.GetComponent<Text>().color = Color.black;
                                Transform transform227 = CreateUI.NewButton(clickAction12).transform;
                                transform227.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform227.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -250f);
                                Transform transform228 = CreateUI.NewText("stranger").transform;
                                transform228.SetParent(transform227, worldPositionStays: false);
                                transform228.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform228.GetComponent<Text>().color = Color.black;
                                Transform transform229 = CreateUI.NewButton(clickAction13).transform;
                                transform229.SetParent(jieYuan.transform, worldPositionStays: false);
                                transform229.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -250f);
                                Transform transform230 = CreateUI.NewText("enemy").transform;
                                transform230.SetParent(transform229, worldPositionStays: false);
                                transform230.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                                transform230.GetComponent<Text>().color = Color.black;
                            }
                            else if (!hasOne4 && !hasOne41)
                            {
                                componentResult4.text = "<color=#004FCA>" + iptFieldXing4.text + "</color>&<color=#004FCA>" + iptFieldXing41.text + "</color>None found";
                            }
                            else if (!hasOne4)
                            {
                                componentResult4.text = "Not found<color=#004FCA>" + iptFieldXing4.text + "</color>";
                            }
                            else if (!hasOne41)
                            {
                                componentResult4.text = "Not found<color=#004FCA>" + iptFieldXing41.text + "</color>";
                            }
                        };
                        Transform transform199 = CreateUI.NewButton(clickAction).transform;
                        transform199.SetParent(jieYuan.transform, worldPositionStays: false);
                        transform199.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                        Transform transform200 = CreateUI.NewText("search").transform;
                        transform200.SetParent(transform199.transform, worldPositionStays: false);
                        transform200.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform200.GetComponent<Text>().color = Color.black;
                    }
                }).transform;
                list.Add(transform17);
                if (list.Contains(transform17))
                {
                    Transform transform18 = CreateUI.NewText("bonding").transform;
                    transform18.SetParent(transform17.transform, worldPositionStays: false);
                    transform18.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    transform18.GetComponent<Text>().color = Color.black;
                }
            }
            if (flag)
            {
                Transform transform19 = CreateUI.NewButton(delegate
                {
                    if (uiPanel.transform.Find("chuansong") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("chuansong").gameObject);
                    }
                    else if (uiPanel.transform.Find("gaiMingZi") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("gaiMingZi").gameObject);
                    }
                    else if (uiPanel.transform.Find("shaQuanjia") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("shaQuanjia").gameObject);
                    }
                    else if (uiPanel.transform.Find("jieYuan") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("jieYuan").gameObject);
                    }
                    else if (uiPanel.transform.Find("zaXiang") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("zaXiang").gameObject);
                    }
                    else if (uiPanel.transform.Find("XingGeuiPanel") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("XingGeuiPanel").gameObject);
                    }
                    else if (uiPanel.transform.Find("QiLing") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("QiLing").gameObject);
                    }
                    else if (uiPanel.transform.Find("ZongMen") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("ZongMen").gameObject);
                    }
                    if (uiPanel.transform.Find("XingGeuiPanel") == null)
                    {
                        GameObject gameObject5 = new GameObject("XingGeuiPanel");
                        gameObject5.transform.SetParent(uiPanel.transform, worldPositionStays: false);
                        componentTittle.text = "****Character****";
                        Transform transform168 = CreateUI.NewText("Character name", new Vector2(500f, 90f)).transform;
                        transform168.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform168.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, 150f);
                        Text component21 = transform168.GetComponent<Text>();
                        component21.fontSize = 28;
                        component21.alignment = TextAnchor.MiddleCenter;
                        component21.color = Color.black;
                        Transform transform169 = CreateUI.NewInputField(null, null, "Character name").transform;
                        transform169.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform169.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform169.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, 100f);
                        InputField ShuRuWen001 = transform169.GetComponent<InputField>();
                        ShuRuWen001.contentType = InputField.ContentType.Name;
                        ShuRuWen001.characterLimit = 4;
                        Transform transform170 = CreateUI.NewText("请先输入角色搜索NPC\n换人请重新搜索", new Vector2(800f, 100f)).transform;
                        transform170.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform170.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, 0f);
                        Text TiShiWen003 = transform170.GetComponent<Text>();
                        TiShiWen003.fontSize = 20;
                        TiShiWen003.alignment = TextAnchor.MiddleCenter;
                        TiShiWen003.color = Color.black;
                        Transform transform171 = CreateUI.NewText("inner character", new Vector2(500f, 90f)).transform;
                        transform171.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform171.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, 150f);
                        Text component22 = transform171.GetComponent<Text>();
                        component22.fontSize = 28;
                        component22.alignment = TextAnchor.MiddleCenter;
                        component22.color = Color.black;
                        Transform transform172 = CreateUI.NewInputField(null, null, "ID (enter an int)").transform;
                        transform172.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform172.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 50f);
                        transform172.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, 100f);
                        InputField ShuRuWen002 = transform172.GetComponent<InputField>();
                        ShuRuWen002.contentType = InputField.ContentType.IntegerNumber;
                        ShuRuWen002.characterLimit = 6;
                        Transform transform173 = CreateUI.NewText("Please enter id to search for personality", new Vector2(800f, 100f)).transform;
                        transform173.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform173.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -50f);
                        Text TiShiWen004 = transform173.GetComponent<Text>();
                        TiShiWen004.fontSize = 20;
                        TiShiWen004.alignment = TextAnchor.MiddleCenter;
                        TiShiWen004.color = Color.black;
                        Transform transform174 = CreateUI.NewText("External personality 1", new Vector2(500f, 90f)).transform;
                        transform174.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform174.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, 150f);
                        Text component23 = transform174.GetComponent<Text>();
                        component23.fontSize = 28;
                        component23.alignment = TextAnchor.MiddleCenter;
                        component23.color = Color.black;
                        Transform transform175 = CreateUI.NewInputField(null, null, "ID (enter an int)").transform;
                        transform175.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform175.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 50f);
                        transform175.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, 100f);
                        InputField ShuRuWen003 = transform175.GetComponent<InputField>();
                        ShuRuWen003.contentType = InputField.ContentType.DecimalNumber;
                        ShuRuWen003.characterLimit = 6;
                        Transform transform176 = CreateUI.NewText("Please enter id to search for personality", new Vector2(800f, 100f)).transform;
                        transform176.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform176.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -50f);
                        Text TiShiWen006 = transform176.GetComponent<Text>();
                        TiShiWen006.fontSize = 20;
                        TiShiWen006.alignment = TextAnchor.MiddleCenter;
                        TiShiWen006.color = Color.black;
                        Transform transform177 = CreateUI.NewText("External personality 2", new Vector2(500f, 90f)).transform;
                        transform177.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform177.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, 150f);
                        Text component24 = transform177.GetComponent<Text>();
                        component24.fontSize = 28;
                        component24.alignment = TextAnchor.MiddleCenter;
                        component24.color = Color.black;
                        Transform transform178 = CreateUI.NewInputField(null, null, "ID (enter an int)").transform;
                        transform178.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform178.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 50f);
                        transform178.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, 100f);
                        InputField ShuRuWen004 = transform178.GetComponent<InputField>();
                        ShuRuWen004.contentType = InputField.ContentType.DecimalNumber;
                        ShuRuWen004.characterLimit = 6;
                        Transform transform179 = CreateUI.NewText("Enter id to search for personality", new Vector2(800f, 100f)).transform;
                        transform179.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform179.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, -50f);
                        Text TiShiWen008 = transform179.GetComponent<Text>();
                        TiShiWen008.fontSize = 20;
                        TiShiWen008.alignment = TextAnchor.MiddleCenter;
                        TiShiWen008.color = Color.black;
                        bool hasNPC = false;
                        WorldUnitBase tarUnit = null;
                        bool hasWai = false;
                        bool hasNei1 = false;
                        bool hasNei2 = false;
                        string WaiZai = null;
                        string NeiZai1 = null;
                        string NeiZai2 = null;
                        int waiID = 0;
                        int nei1ID = 0;
                        int nei2ID = 0;
                        Transform transform180 = CreateUI.NewButton(delegate
                        {
                            hasNPC = false;
                            tarUnit = null;
                            if (ShuRuWen001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarUnit = g.world.playerUnit;
                                hasNPC = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator55 = allUnits.GetEnumerator();
                                while (enumerator55.MoveNext())
                                {
                                    WorldUnitBase current55 = enumerator55.Current;
                                    if (current55.data.unitData.propertyData.GetName() == ShuRuWen001.text)
                                    {
                                        hasNPC = true;
                                        tarUnit = current55;
                                        break;
                                    }
                                }
                            }
                            if (hasNPC)
                            {
                                TiShiWen003.text = "Searched<color=#004FCA>" + tarUnit.data.unitData.propertyData.GetName() + "</color>！";
                            }
                            else
                            {
                                TiShiWen003.text = "Not found<color=#004FCA>" + ShuRuWen001.text + "</color>！";
                            }
                        }).transform;
                        transform180.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform180.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, 50f);
                        Transform transform181 = CreateUI.NewText("search").transform;
                        transform181.SetParent(transform180, worldPositionStays: false);
                        transform181.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform181.GetComponent<Text>().color = Color.black;
                        Transform transform182 = CreateUI.NewButton(delegate
                        {
                            hasWai = false;
                            waiID = 0;
                            if (g.conf.roleCreateCharacter.GetItem(int.Parse(ShuRuWen002.text)) != null)
                            {
                                if (g.conf.roleCreateCharacter.GetItem(int.Parse(ShuRuWen002.text)).type == 1)
                                {
                                    hasWai = true;
                                    Il2CppSystem.Collections.Generic.Dictionary<string, ConfLocalTextItem>.Enumerator enumerator54 = g.conf.localText.allText.GetEnumerator();
                                    while (enumerator54.MoveNext())
                                    {
                                        Il2CppSystem.Collections.Generic.KeyValuePair<string, ConfLocalTextItem> current54 = enumerator54.Current;
                                        if (current54.value.key == g.conf.roleCreateCharacter.GetItem(int.Parse(ShuRuWen002.text)).sc5asd_sd34)
                                        {
                                            WaiZai = current54.value.ch;
                                            waiID = int.Parse(ShuRuWen002.text);
                                            TiShiWen004.text = "Searched<color=#004FCA>" + WaiZai + "</color>";
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    TiShiWen004.text = "This character is not an intrinsic character";
                                }
                            }
                            else
                            {
                                TiShiWen004.text = "The character was not found";
                            }
                        }).transform;
                        transform182.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform182.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, 50f);
                        Transform transform183 = CreateUI.NewText("search").transform;
                        transform183.SetParent(transform182, worldPositionStays: false);
                        transform183.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform183.GetComponent<Text>().color = Color.black;
                        Transform transform184 = CreateUI.NewButton(delegate
                        {
                            hasNei1 = false;
                            nei1ID = 0;
                            if (g.conf.roleCreateCharacter.GetItem(int.Parse(ShuRuWen003.text)) != null)
                            {
                                if (g.conf.roleCreateCharacter.GetItem(int.Parse(ShuRuWen003.text)).type == 2)
                                {
                                    hasNei1 = true;
                                    Il2CppSystem.Collections.Generic.Dictionary<string, ConfLocalTextItem>.Enumerator enumerator53 = g.conf.localText.allText.GetEnumerator();
                                    while (enumerator53.MoveNext())
                                    {
                                        Il2CppSystem.Collections.Generic.KeyValuePair<string, ConfLocalTextItem> current53 = enumerator53.Current;
                                        if (current53.value.key == g.conf.roleCreateCharacter.GetItem(int.Parse(ShuRuWen003.text)).sc5asd_sd34)
                                        {
                                            NeiZai1 = current53.value.ch;
                                            nei1ID = int.Parse(ShuRuWen003.text);
                                            TiShiWen006.text = "Searched<color=#004FCA>" + NeiZai1 + "</color>";
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    TiShiWen006.text = "This character is not an external character";
                                }
                            }
                            else
                            {
                                TiShiWen006.text = "The character was not found";
                            }
                        }).transform;
                        transform184.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform184.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, 50f);
                        Transform transform185 = CreateUI.NewText("search").transform;
                        transform185.SetParent(transform184, worldPositionStays: false);
                        transform185.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform185.GetComponent<Text>().color = Color.black;
                        Transform transform186 = CreateUI.NewButton(delegate
                        {
                            hasNei2 = false;
                            nei2ID = 0;
                            if (g.conf.roleCreateCharacter.GetItem(int.Parse(ShuRuWen004.text)) != null)
                            {
                                if (g.conf.roleCreateCharacter.GetItem(int.Parse(ShuRuWen004.text)).type == 2)
                                {
                                    hasNei2 = true;
                                    Il2CppSystem.Collections.Generic.Dictionary<string, ConfLocalTextItem>.Enumerator enumerator52 = g.conf.localText.allText.GetEnumerator();
                                    while (enumerator52.MoveNext())
                                    {
                                        Il2CppSystem.Collections.Generic.KeyValuePair<string, ConfLocalTextItem> current52 = enumerator52.Current;
                                        if (current52.value.key == g.conf.roleCreateCharacter.GetItem(int.Parse(ShuRuWen004.text)).sc5asd_sd34)
                                        {
                                            NeiZai2 = current52.value.ch;
                                            nei2ID = int.Parse(ShuRuWen004.text);
                                            TiShiWen008.text = "Searched<color=#004FCA>" + NeiZai2 + "</color>";
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    TiShiWen008.text = "This character is not an external character";
                                }
                            }
                            else
                            {
                                TiShiWen008.text = "The character was not found";
                            }
                        }).transform;
                        transform186.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform186.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, 50f);
                        Transform transform187 = CreateUI.NewText("search").transform;
                        transform187.SetParent(transform186, worldPositionStays: false);
                        transform187.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform187.GetComponent<Text>().color = Color.black;
                        Transform transform188 = CreateUI.NewButton(delegate
                        {
                            if (hasWai && hasNPC)
                            {
                                tarUnit.data.unitData.propertyData.inTrait = waiID;
                                TiShiWen004.text = "<color=#004FCA>" + WaiZai + "</color>Successfully modified!";
                            }
                            else if (!hasNPC && !hasWai)
                            {
                                TiShiWen004.text = "Please search for roles and personalities first!";
                            }
                            else if (!hasNPC)
                            {
                                TiShiWen004.text = "Please search for the role first!";
                            }
                            else if (!hasWai)
                            {
                                TiShiWen004.text = "Please search for the character first!";
                            }
                        }).transform;
                        transform188.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform188.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, 0f);
                        Transform transform189 = CreateUI.NewText("Revise").transform;
                        transform189.SetParent(transform188, worldPositionStays: false);
                        transform189.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform189.GetComponent<Text>().color = Color.black;
                        Transform transform190 = CreateUI.NewButton(delegate
                        {
                            if (hasNei1 && hasNPC)
                            {
                                tarUnit.data.unitData.propertyData.outTrait1 = nei1ID;
                                TiShiWen006.text = "<color=#004FCA>" + NeiZai1 + "</color>Successfully modified!";
                            }
                            else if (!hasNPC && !hasNei1)
                            {
                                TiShiWen006.text = "Please search for roles and personalities first!";
                            }
                            else if (!hasNPC)
                            {
                                TiShiWen006.text = "Please search for the role first!";
                            }
                            else if (!hasNei1)
                            {
                                TiShiWen006.text = "Please search for the character first!";
                            }
                        }).transform;
                        transform190.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform190.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, 0f);
                        Transform transform191 = CreateUI.NewText("Revise").transform;
                        transform191.SetParent(transform190, worldPositionStays: false);
                        transform191.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform191.GetComponent<Text>().color = Color.black;
                        Transform transform192 = CreateUI.NewButton(delegate
                        {
                            if (hasNei2 && hasNPC)
                            {
                                tarUnit.data.unitData.propertyData.outTrait2 = nei2ID;
                                TiShiWen008.text = "<color=#004FCA>" + NeiZai2 + "</color>Successfully modified!";
                            }
                            else if (!hasNPC && !hasNei2)
                            {
                                TiShiWen008.text = "Please search for roles and personalities first!";
                            }
                            else if (!hasNPC)
                            {
                                TiShiWen008.text = "Please search for the role first!";
                            }
                            else if (!hasNei2)
                            {
                                TiShiWen008.text = "Please search for the character first!";
                            }
                        }).transform;
                        transform192.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform192.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, 0f);
                        Transform transform193 = CreateUI.NewText("Revise").transform;
                        transform193.SetParent(transform192, worldPositionStays: false);
                        transform193.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform193.GetComponent<Text>().color = Color.black;
                        Transform transform194 = CreateUI.NewText("Instructions for use：\n1, please search the character name and the personality ID you want to modify first, and then click Modify after the search is successful.\n2, support MOD character, please refer to LocalText.json and RoleCreateCharacter.json to get the ID.\n3, original inner character: 1 selfless 2 integrity 3 benevolence 4 moderation 5 crazy evil 6 self-interest 7 evil\n4, the original external character: 8 family relations, 9 loyalty, 10 protection of shortcomings, 11 withdrawn, 12 love of family, 13 reputation, 14 power, 15 disdain, 16 let me go, 17 love, 18 inheritance, 19 loyalty.\n5, Xiaoyaoyou Mod's external personality: 20 Father controls 21 Oedipus 22 Brother controls 23 Sister controls 24 Love son 25 Controls daughter 26 Love teacher 27 Love disciple 28 Focus", new Vector2(1000f, 300f)).transform;
                        transform194.SetParent(gameObject5.transform, worldPositionStays: false);
                        transform194.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -150f);
                        Text component25 = transform194.GetComponent<Text>();
                        component25.fontSize = 20;
                        component25.alignment = TextAnchor.MiddleLeft;
                        component25.color = Color.black;
                    }
                }).transform;
                list.Add(transform19);
                if (list.Contains(transform19))
                {
                    Transform transform20 = CreateUI.NewText("character").transform;
                    transform20.SetParent(transform19.transform, worldPositionStays: false);
                    transform20.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    transform20.GetComponent<Text>().color = Color.black;
                }
            }
            if (flag)
            {
                Transform transform21 = CreateUI.NewButton(delegate
                {
                    if (uiPanel.transform.Find("chuansong") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("chuansong").gameObject);
                    }
                    else if (uiPanel.transform.Find("gaiMingZi") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("gaiMingZi").gameObject);
                    }
                    else if (uiPanel.transform.Find("shaQuanjia") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("shaQuanjia").gameObject);
                    }
                    else if (uiPanel.transform.Find("jieYuan") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("jieYuan").gameObject);
                    }
                    else if (uiPanel.transform.Find("zaXiang") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("zaXiang").gameObject);
                    }
                    else if (uiPanel.transform.Find("XingGeuiPanel") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("XingGeuiPanel").gameObject);
                    }
                    else if (uiPanel.transform.Find("QiLing") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("QiLing").gameObject);
                    }
                    else if (uiPanel.transform.Find("ZongMen") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("ZongMen").gameObject);
                    }
                    if (uiPanel.transform.Find("QiLing") == null)
                    {
                        GameObject gameObject4 = new GameObject("QiLing");
                        gameObject4.transform.SetParent(uiPanel.transform, worldPositionStays: false);
                        componentTittle.text = "****Weapon Spirit****";
                        float num10 = -150f;
                        float x = 150f;
                        Transform transform155 = CreateUI.NewText("Qi Ling ID", new Vector2(500f, 90f)).transform;
                        transform155.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform155.GetComponent<RectTransform>().anchoredPosition = new Vector2(num10, 150f);
                        Text component18 = transform155.GetComponent<Text>();
                        component18.fontSize = 28;
                        component18.alignment = TextAnchor.MiddleCenter;
                        component18.color = Color.black;
                        Transform transform156 = CreateUI.NewInputField(null, null, "Qi Ling ID").transform;
                        transform156.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform156.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
                        transform156.GetComponent<RectTransform>().anchoredPosition = new Vector2(num10, 100f);
                        InputField ShuRuWen002 = transform156.GetComponent<InputField>();
                        ShuRuWen002.contentType = InputField.ContentType.IntegerNumber;
                        ShuRuWen002.characterLimit = 10;
                        Transform transform157 = CreateUI.NewText("Please enter the tool spirit ID to obtain the tool spirit", new Vector2(800f, 100f)).transform;
                        transform157.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform157.GetComponent<RectTransform>().anchoredPosition = new Vector2(num10, 0f);
                        Text TiShiWen004 = transform157.GetComponent<Text>();
                        TiShiWen004.fontSize = 20;
                        TiShiWen004.alignment = TextAnchor.MiddleCenter;
                        TiShiWen004.color = Color.black;
                        Transform transform158 = CreateUI.NewText("Weapon Spirit Favorability", new Vector2(500f, 90f)).transform;
                        transform158.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform158.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 150f);
                        Text component19 = transform158.GetComponent<Text>();
                        component19.fontSize = 28;
                        component19.alignment = TextAnchor.MiddleCenter;
                        component19.color = Color.black;
                        Transform transform159 = CreateUI.NewInputField(null, null, "Increase favorability").transform;
                        transform159.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform159.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
                        transform159.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 100f);
                        InputField ShuRuWen001 = transform159.GetComponent<InputField>();
                        ShuRuWen001.contentType = InputField.ContentType.DecimalNumber;
                        ShuRuWen001.characterLimit = 10;
                        Transform transform160 = CreateUI.NewText("Please enter the tooling ID first to search for the tooling", new Vector2(800f, 100f)).transform;
                        transform160.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform160.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0f);
                        Text TiShiWen003 = transform160.GetComponent<Text>();
                        TiShiWen003.fontSize = 20;
                        TiShiWen003.alignment = TextAnchor.MiddleCenter;
                        TiShiWen003.color = Color.black;
                        bool hasQL = false;
                        int QLID = 0;
                        string QLName = null;
                        Transform transform161 = CreateUI.NewButton(delegate
                        {
                            hasQL = false;
                            QLID = 0;
                            QLName = null;
                            Il2CppSystem.Collections.Generic.List<ConfArtifactSpriteItem>.Enumerator enumerator50 = allSprite.GetEnumerator();
                            while (enumerator50.MoveNext())
                            {
                                ConfArtifactSpriteItem current50 = enumerator50.Current;
                                if (ShuRuWen002.text == current50.id.ToString())
                                {
                                    QLID = current50.id;
                                    Il2CppSystem.Collections.Generic.Dictionary<string, ConfLocalTextItem>.Enumerator enumerator51 = g.conf.localText.allText.GetEnumerator();
                                    while (enumerator51.MoveNext())
                                    {
                                        Il2CppSystem.Collections.Generic.KeyValuePair<string, ConfLocalTextItem> current51 = enumerator51.Current;
                                        if (current51.value.key == current50.name)
                                        {
                                            QLName = current51.value.ch;
                                            TiShiWen004.text = "Searched<color=#004FCA>" + current51.value.ch + "</color>";
                                            TiShiWen003.text = "Please enter the favorability value (integer) and click Add Favorite";
                                            hasQL = true;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            if (!hasQL)
                            {
                                TiShiWen004.text = "The weapon spirit was not found";
                            }
                        }).transform;
                        transform161.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform161.GetComponent<RectTransform>().anchoredPosition = new Vector2(num10 - 50f, 50f);
                        Transform transform162 = CreateUI.NewText("search").transform;
                        transform162.SetParent(transform161, worldPositionStays: false);
                        transform162.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform162.GetComponent<Text>().color = Color.black;
                        Transform transform163 = CreateUI.NewButton(delegate
                        {
                            if (hasQL && QLID != 0)
                            {
                                g.world.playerUnit.data.unitData.artifactSpriteData.AddSprite(QLID);
                                TiShiWen004.text = "Obtained<color=#004FCA>" + QLName + "</color>";
                            }
                            else
                            {
                                TiShiWen004.text = "Please search for the artifact spirit first!";
                            }
                        }).transform;
                        transform163.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform163.GetComponent<RectTransform>().anchoredPosition = new Vector2(num10 + 50f, 50f);
                        Transform transform164 = CreateUI.NewText("Obtain").transform;
                        transform164.SetParent(transform163, worldPositionStays: false);
                        transform164.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform164.GetComponent<Text>().color = Color.black;
                        Transform transform165 = CreateUI.NewButton(delegate
                        {
                            int result9;
                            bool flag3 = int.TryParse(ShuRuWen001.text, out result9);
                            if (hasQL && flag3)
                            {
                                LuciferDrama.SpriteClose(new Il2CppStringArray(new string[4]
                                {
                                "spriteClose",
                                QLID.ToString(),
                                result9.ToString(),
                                result9.ToString()
                                }));
                                TiShiWen003.text = "<color=#005FCA>" + QLName + "</color>The favorability of" + result9;
                            }
                            else
                            {
                                TiShiWen003.text = "Please search for the artifact spirit first!";
                            }
                        }).transform;
                        transform165.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform165.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 50f);
                        Transform transform166 = CreateUI.NewText("Add favorability").transform;
                        transform166.SetParent(transform165, worldPositionStays: false);
                        transform166.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform166.GetComponent<Text>().color = Color.black;
                        Transform transform167 = CreateUI.NewText("Instructions for use：\n1.Please enter the artifact spirit ID first to obtain the artifact spirit, and then click to obtain or increase the favorability.\n2. Supports MOD artifact spirit, please check the ID from LocalText.json and ArtifactSprite.json Obtain\n3、Original artifact spirit ID：1001 Yun Mengyi 1002 Cang Ya 1003 Jiu Chen 1004 Dongfang Mu Qing 1005 Li Ningshuang 1006 Jiang Lingge 1007 Zhu Yaoyue 1008 Leng Jingtang\n2001-2018 Ordinary weapon spirit", new Vector2(1000f, 300f)).transform;
                        transform167.SetParent(gameObject4.transform, worldPositionStays: false);
                        transform167.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -125f);
                        Text component20 = transform167.GetComponent<Text>();
                        component20.fontSize = 20;
                        component20.alignment = TextAnchor.MiddleLeft;
                        component20.color = Color.black;
                    }
                }).transform;
                list.Add(transform21);
                if (list.Contains(transform21))
                {
                    Transform transform22 = CreateUI.NewText("Weapon Spirit").transform;
                    transform22.SetParent(transform21.transform, worldPositionStays: false);
                    transform22.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    transform22.GetComponent<Text>().color = Color.black;
                }
            }
            if (flag)
            {
                Transform transform23 = CreateUI.NewButton(delegate
                {
                    if (uiPanel.transform.Find("chuansong") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("chuansong").gameObject);
                    }
                    else if (uiPanel.transform.Find("gaiMingZi") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("gaiMingZi").gameObject);
                    }
                    else if (uiPanel.transform.Find("shaQuanjia") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("shaQuanjia").gameObject);
                    }
                    else if (uiPanel.transform.Find("jieYuan") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("jieYuan").gameObject);
                    }
                    else if (uiPanel.transform.Find("zaXiang") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("zaXiang").gameObject);
                    }
                    else if (uiPanel.transform.Find("XingGeuiPanel") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("XingGeuiPanel").gameObject);
                    }
                    else if (uiPanel.transform.Find("QiLing") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("QiLing").gameObject);
                    }
                    else if (uiPanel.transform.Find("ZongMen") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("ZongMen").gameObject);
                    }
                    if (uiPanel.transform.Find("ZongMen") == null)
                    {
                        GameObject gameObject3 = new GameObject("ZongMen");
                        gameObject3.transform.SetParent(uiPanel.transform, worldPositionStays: false);
                        componentTittle.text = "****Sect****";
                        float num2 = -300f;
                        float num3 = 150f;
                        Transform transform79 = CreateUI.NewText("Let any character join any sect", new Vector2(500f, 90f)).transform;
                        transform79.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform79.GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, 150f);
                        Text component11 = transform79.GetComponent<Text>();
                        component11.fontSize = 28;
                        component11.alignment = TextAnchor.MiddleCenter;
                        component11.color = Color.black;
                        Transform transform80 = CreateUI.NewInputField(null, null, "Character name").transform;
                        transform80.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform80.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 50f);
                        transform80.GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, 100f);
                        InputField ShuRuWen001 = transform80.GetComponent<InputField>();
                        ShuRuWen001.contentType = InputField.ContentType.Name;
                        ShuRuWen001.text = g.world.playerUnit.data.unitData.propertyData.GetName();
                        ShuRuWen001.characterLimit = 10;
                        Transform transform81 = CreateUI.NewInputField(null, null, "Target sect name").transform;
                        transform81.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform81.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 50f);
                        transform81.GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, 50f);
                        InputField ShuRuWen002 = transform81.GetComponent<InputField>();
                        ShuRuWen002.contentType = InputField.ContentType.Name;
                        if (g.world.playerUnit.data.school != null)
                        {
                            ShuRuWen002.text = allBuild[g.world.playerUnit.data.unitData.schoolID].name;
                        }
                        else
                        {
                            ShuRuWen002.text = "Target sect name";
                        }
                        ShuRuWen002.characterLimit = 20;
                        Transform transform82 = CreateUI.NewText("please enter<color=#004FCA>target role</color>and<color=#004FCA>Target sect name</color>to get information\nCharacters and sects are both<color=#004FCA>After successful search</color>Click on the function on the right", new Vector2(500f, 100f)).transform;
                        transform82.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform82.GetComponent<RectTransform>().anchoredPosition = new Vector2(num2 + 50f, -50f);
                        Text component12 = transform82.GetComponent<Text>();
                        component12.fontSize = 20;
                        component12.alignment = TextAnchor.MiddleLeft;
                        component12.color = Color.black;
                        Transform transform83 = CreateUI.NewText("Prompt information:\n内Setting cheat tools cannot transcend the way of heaven, for example：\nYou cannot become the branch leader, you can only become the general leader.\nIf you want to become a true disciple, there must be an elder in the corresponding church.\nRejoining after withdrawing will be subject to the withdrawal time limit", new Vector2(500f, 150f)).transform;
                        transform83.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform83.GetComponent<RectTransform>().anchoredPosition = new Vector2(num2 + 50f, -150f);
                        Text TiShiWen001 = transform83.GetComponent<Text>();
                        TiShiWen001.fontSize = 20;
                        TiShiWen001.alignment = TextAnchor.MiddleLeft;
                        TiShiWen001.color = Color.black;
                        string tarUnitName = null;
                        string tarUnitSchoolName = null;
                        string tarSchoolName = null;
                        bool hasOne = false;
                        bool hasSchool = false;
                        WorldUnitBase tarUnit = null;
                        MapBuildBase tarSchoolBase = new MapBuildBase();
                        DataBuildSchool.SchoolData tarSchool = new DataBuildSchool.SchoolData();
                        Transform transform84 = CreateUI.NewButton(delegate
                        {
                            hasOne = false;
                            hasSchool = false;
                            tarUnit = null;
                            tarSchool = null;
                            tarSchoolBase = null;
                            tarSchoolName = null;
                            tarUnitSchoolName = null;
                            tarUnitName = null;
                            if (g.world.playerUnit.data.unitData.propertyData.GetName() == ShuRuWen001.text)
                            {
                                tarUnit = g.world.playerUnit;
                                hasOne = true;
                                tarUnitName = g.world.playerUnit.data.unitData.propertyData.GetName();
                                if (tarUnit.data.school != null)
                                {
                                    tarUnitSchoolName = allBuild[tarUnit.data.unitData.schoolID].name;
                                }
                                else
                                {
                                    tarUnitSchoolName = "No sect";
                                }
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator48 = allUnits.GetEnumerator();
                                while (enumerator48.MoveNext())
                                {
                                    WorldUnitBase current48 = enumerator48.Current;
                                    if (current48.data.unitData.propertyData.GetName() == ShuRuWen001.text)
                                    {
                                        tarUnit = current48;
                                        hasOne = true;
                                        tarUnitName = current48.data.unitData.propertyData.GetName();
                                        if (current48.data.school != null)
                                        {
                                            tarUnitSchoolName = allBuild[current48.data.unitData.schoolID].name;
                                        }
                                        else
                                        {
                                            tarUnitSchoolName = "No sect";
                                        }
                                        break;
                                    }
                                }
                            }
                            Il2CppSystem.Collections.Generic.Dictionary<string, MapBuildBase>.Enumerator enumerator49 = allBuild.GetEnumerator();
                            while (enumerator49.MoveNext())
                            {
                                Il2CppSystem.Collections.Generic.KeyValuePair<string, MapBuildBase> current49 = enumerator49.Current;
                                if (current49.Value.name == ShuRuWen002.text)
                                {
                                    tarSchoolBase = allBuild[current49.key];
                                    tarSchool = allSchool[current49.key];
                                    hasSchool = true;
                                    tarSchoolName = tarSchoolBase.name;
                                    break;
                                }
                            }
                            if (!hasOne)
                            {
                                tarUnitName = "none";
                            }
                            if (!hasSchool)
                            {
                                tarSchoolName = "none";
                            }
                            TiShiWen001.text = "Searched information\nSearched roles：<color=#004FCA>" + tarUnitName + "</color>，Sect：<color=#004FCA>" + tarUnitSchoolName + "</color>\nSearch for Zongmen：<color=#004FCA>" + tarSchoolName + "</color>";
                        }).transform;
                        transform84.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform84.GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, 0f);
                        Transform transform85 = CreateUI.NewText("Search Info").transform;
                        transform85.SetParent(transform84, worldPositionStays: false);
                        transform85.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform85.GetComponent<Text>().color = Color.black;
                        WorldUnitBase schoolUnit = new WorldUnitBase();
                        Transform transform86 = CreateUI.NewButton(delegate
                        {
                            if (tarSchool.npcSchoolMain != tarUnit.data.unitData.unitID)
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, SchoolDepartmentType.None));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, SchoolDepartmentType.Security));
                                }
                                if (tarSchool.npcSchoolMain.Length > 1)
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator46 = allUnits.GetEnumerator();
                                    while (enumerator46.MoveNext())
                                    {
                                        WorldUnitBase current46 = enumerator46.Current;
                                        if (current46.data.unitData.unitID == tarSchool.npcSchoolMain)
                                        {
                                            schoolUnit = current46;
                                            tarUnit.CreateAction(new UnitActionSchoolSwapPostType(schoolUnit));
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase18 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator47 = allUnits.GetEnumerator();
                                    while (enumerator47.MoveNext())
                                    {
                                        WorldUnitBase current47 = enumerator47.Current;
                                        if (current47.data.school == null)
                                        {
                                            worldUnitBase18 = g.world.unit.CreateUnit(current47.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase18.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.SchoolMain, SchoolDepartmentType.None));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase18));
                                    g.world.unit.DelUnit(worldUnitBase18);
                                }
                            }
                        }).transform;
                        transform86.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform86.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3, 150f);
                        Transform transform87 = CreateUI.NewText("metropolitan").transform;
                        transform87.SetParent(transform86, worldPositionStays: false);
                        transform87.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform87.GetComponent<Text>().color = Color.black;
                        Transform transform88 = CreateUI.NewButton(delegate
                        {
                            if (!tarSchool.npcBigElders.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, SchoolDepartmentType.None));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, SchoolDepartmentType.Security));
                                }
                                if (tarSchool.npcBigElders.Count == 2)
                                {
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator44 = allUnits.GetEnumerator();
                                    while (enumerator44.MoveNext())
                                    {
                                        WorldUnitBase current44 = enumerator44.Current;
                                        if (current44.data.unitData.unitID == tarSchool.npcBigElders[0])
                                        {
                                            schoolUnit = current44;
                                            tarUnit.CreateAction(new UnitActionSchoolSwapPostType(schoolUnit));
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase17 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator45 = allUnits.GetEnumerator();
                                    while (enumerator45.MoveNext())
                                    {
                                        WorldUnitBase current45 = enumerator45.Current;
                                        if (current45.data.school == null)
                                        {
                                            worldUnitBase17 = g.world.unit.CreateUnit(current45.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase17.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.BigElders, SchoolDepartmentType.None));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase17));
                                    g.world.unit.DelUnit(worldUnitBase17);
                                }
                            }
                        }).transform;
                        transform88.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform88.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3, 100f);
                        Transform transform89 = CreateUI.NewText("Grand Elder").transform;
                        transform89.SetParent(transform88, worldPositionStays: false);
                        transform89.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform89.GetComponent<Text>().color = Color.black;
                        Transform transform90 = CreateUI.NewButton(delegate
                        {
                            if (tarSchool.npcBigElders.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarSchool.npcBigElders.Count == 2)
                                {
                                    string text = tarSchool.npcBigElders[0];
                                    if (tarSchool.npcBigElders[0] == tarUnit.data.unitData.unitID)
                                    {
                                        text = tarSchool.npcBigElders[1];
                                    }
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator42 = allUnits.GetEnumerator();
                                    while (enumerator42.MoveNext())
                                    {
                                        WorldUnitBase current42 = enumerator42.Current;
                                        if (current42.data.unitData.unitID == text)
                                        {
                                            schoolUnit = current42;
                                            tarUnit.CreateAction(new UnitActionSchoolSwapPostType(schoolUnit));
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase16 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator43 = allUnits.GetEnumerator();
                                    while (enumerator43.MoveNext())
                                    {
                                        WorldUnitBase current43 = enumerator43.Current;
                                        if (current43.data.school == null)
                                        {
                                            worldUnitBase16 = g.world.unit.CreateUnit(current43.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase16.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.BigElders, SchoolDepartmentType.None));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase16));
                                    g.world.unit.DelUnit(worldUnitBase16);
                                }
                            }
                        }).transform;
                        transform90.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform90.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 100f, 100f);
                        Transform transform91 = CreateUI.NewText("Switch location").transform;
                        transform91.SetParent(transform90, worldPositionStays: false);
                        transform91.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform91.GetComponent<Text>().color = Color.black;
                        float y = 63f;
                        float num4 = 25f;
                        if (true)
                        {
                            Transform transform92 = CreateUI.NewText("xiuzhutang", new Vector2(100f, 50f)).transform;
                            transform92.SetParent(gameObject3.transform, worldPositionStays: false);
                            transform92.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 200f, y);
                            Text component13 = transform92.GetComponent<Text>();
                            component13.fontSize = 20;
                            component13.alignment = TextAnchor.MiddleCenter;
                            component13.color = Color.black;
                            Transform transform93 = CreateUI.NewText("Inspection Hall", new Vector2(100f, 50f)).transform;
                            transform93.SetParent(gameObject3.transform, worldPositionStays: false);
                            transform93.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 100f, y);
                            Text component14 = transform93.GetComponent<Text>();
                            component14.fontSize = 20;
                            component14.alignment = TextAnchor.MiddleCenter;
                            component14.color = Color.black;
                            Transform transform94 = CreateUI.NewText("Cubaotang", new Vector2(100f, 50f)).transform;
                            transform94.SetParent(gameObject3.transform, worldPositionStays: false);
                            transform94.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3, y);
                            Text component15 = transform94.GetComponent<Text>();
                            component15.fontSize = 20;
                            component15.alignment = TextAnchor.MiddleCenter;
                            component15.color = Color.black;
                            Transform transform95 = CreateUI.NewText("Zhaoxiantang", new Vector2(100f, 50f)).transform;
                            transform95.SetParent(gameObject3.transform, worldPositionStays: false);
                            transform95.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 100f, y);
                            Text component16 = transform95.GetComponent<Text>();
                            component16.fontSize = 20;
                            component16.alignment = TextAnchor.MiddleCenter;
                            component16.color = Color.black;
                            Transform transform96 = CreateUI.NewText("Foreign Affairs Hall", new Vector2(100f, 50f)).transform;
                            transform96.SetParent(gameObject3.transform, worldPositionStays: false);
                            transform96.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 200f, y);
                            Text component17 = transform96.GetComponent<Text>();
                            component17.fontSize = 20;
                            component17.alignment = TextAnchor.MiddleCenter;
                            component17.color = Color.black;
                        }
                        Transform transform97 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType20 = SchoolDepartmentType.Constructor;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType20 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcElders.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator40 = allUnits.GetEnumerator();
                                while (enumerator40.MoveNext())
                                {
                                    WorldUnitBase current40 = enumerator40.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current40.data.unitData.unitID) == schoolDepartmentType20 && current40.data.unitData.schoolID == tarSchool.id && tarSchool.npcElders.Contains(current40.data.unitData.unitID))
                                    {
                                        current40.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType20));
                                        break;
                                    }
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType20));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase15 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator41 = allUnits.GetEnumerator();
                                    while (enumerator41.MoveNext())
                                    {
                                        WorldUnitBase current41 = enumerator41.Current;
                                        if (current41.data.school == null)
                                        {
                                            worldUnitBase15 = g.world.unit.CreateUnit(current41.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase15.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType20));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase15));
                                    g.world.unit.DelUnit(worldUnitBase15);
                                }
                            }
                        }).transform;
                        transform97.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform97.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 200f, num4);
                        Transform transform98 = CreateUI.NewText("elder").transform;
                        transform98.SetParent(transform97, worldPositionStays: false);
                        transform98.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform98.GetComponent<Text>().color = Color.black;
                        Transform transform99 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType19 = SchoolDepartmentType.Security;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType19 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcElders.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator38 = allUnits.GetEnumerator();
                                while (enumerator38.MoveNext())
                                {
                                    WorldUnitBase current38 = enumerator38.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current38.data.unitData.unitID) == schoolDepartmentType19 && current38.data.unitData.schoolID == tarSchool.id && tarSchool.npcElders.Contains(current38.data.unitData.unitID))
                                    {
                                        current38.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType19));
                                        break;
                                    }
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType19));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase14 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator39 = allUnits.GetEnumerator();
                                    while (enumerator39.MoveNext())
                                    {
                                        WorldUnitBase current39 = enumerator39.Current;
                                        if (current39.data.school == null)
                                        {
                                            worldUnitBase14 = g.world.unit.CreateUnit(current39.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase14.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType19));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase14));
                                    g.world.unit.DelUnit(worldUnitBase14);
                                }
                            }
                        }).transform;
                        transform99.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform99.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 100f, num4);
                        Transform transform100 = CreateUI.NewText("elder").transform;
                        transform100.SetParent(transform99, worldPositionStays: false);
                        transform100.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform100.GetComponent<Text>().color = Color.black;
                        Transform transform101 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType18 = SchoolDepartmentType.Train;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType18 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcElders.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator36 = allUnits.GetEnumerator();
                                while (enumerator36.MoveNext())
                                {
                                    WorldUnitBase current36 = enumerator36.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current36.data.unitData.unitID) == schoolDepartmentType18 && current36.data.unitData.schoolID == tarSchool.id && tarSchool.npcElders.Contains(current36.data.unitData.unitID))
                                    {
                                        current36.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType18));
                                        break;
                                    }
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType18));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase13 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator37 = allUnits.GetEnumerator();
                                    while (enumerator37.MoveNext())
                                    {
                                        WorldUnitBase current37 = enumerator37.Current;
                                        if (current37.data.school == null)
                                        {
                                            worldUnitBase13 = g.world.unit.CreateUnit(current37.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase13.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType18));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase13));
                                    g.world.unit.DelUnit(worldUnitBase13);
                                }
                            }
                        }).transform;
                        transform101.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform101.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3, num4);
                        Transform transform102 = CreateUI.NewText("elder").transform;
                        transform102.SetParent(transform101, worldPositionStays: false);
                        transform102.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform102.GetComponent<Text>().color = Color.black;
                        Transform transform103 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType17 = SchoolDepartmentType.Personnel;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType17 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcElders.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator34 = allUnits.GetEnumerator();
                                while (enumerator34.MoveNext())
                                {
                                    WorldUnitBase current34 = enumerator34.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current34.data.unitData.unitID) == schoolDepartmentType17 && current34.data.unitData.schoolID == tarSchool.id && tarSchool.npcElders.Contains(current34.data.unitData.unitID))
                                    {
                                        current34.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType17));
                                        break;
                                    }
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType17));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase12 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator35 = allUnits.GetEnumerator();
                                    while (enumerator35.MoveNext())
                                    {
                                        WorldUnitBase current35 = enumerator35.Current;
                                        if (current35.data.school == null)
                                        {
                                            worldUnitBase12 = g.world.unit.CreateUnit(current35.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase12.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType17));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase12));
                                    g.world.unit.DelUnit(worldUnitBase12);
                                }
                            }
                        }).transform;
                        transform103.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform103.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 100f, num4);
                        Transform transform104 = CreateUI.NewText("elder").transform;
                        transform104.SetParent(transform103, worldPositionStays: false);
                        transform104.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform104.GetComponent<Text>().color = Color.black;
                        Transform transform105 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType16 = SchoolDepartmentType.Diplomacy;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType16 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcElders.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator32 = allUnits.GetEnumerator();
                                while (enumerator32.MoveNext())
                                {
                                    WorldUnitBase current32 = enumerator32.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current32.data.unitData.unitID) == schoolDepartmentType16 && current32.data.unitData.schoolID == tarSchool.id && tarSchool.npcElders.Contains(current32.data.unitData.unitID))
                                    {
                                        current32.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType16));
                                        break;
                                    }
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType16));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase11 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator33 = allUnits.GetEnumerator();
                                    while (enumerator33.MoveNext())
                                    {
                                        WorldUnitBase current33 = enumerator33.Current;
                                        if (current33.data.school == null)
                                        {
                                            worldUnitBase11 = g.world.unit.CreateUnit(current33.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase11.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Elders, schoolDepartmentType16));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase11));
                                    g.world.unit.DelUnit(worldUnitBase11);
                                }
                            }
                        }).transform;
                        transform105.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform105.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 200f, num4);
                        Transform transform106 = CreateUI.NewText("elder").transform;
                        transform106.SetParent(transform105, worldPositionStays: false);
                        transform106.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform106.GetComponent<Text>().color = Color.black;
                        Transform transform107 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType15 = SchoolDepartmentType.Constructor;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType15 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcInherit.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                int num9 = 0;
                                WorldUnitBase worldUnitBase9 = new WorldUnitBase();
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator30 = allUnits.GetEnumerator();
                                while (enumerator30.MoveNext())
                                {
                                    WorldUnitBase current30 = enumerator30.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current30.data.unitData.unitID) == schoolDepartmentType15 && current30.data.unitData.schoolID == tarSchool.id && tarSchool.npcInherit.Contains(current30.data.unitData.unitID))
                                    {
                                        num9++;
                                        worldUnitBase9 = current30;
                                    }
                                }
                                if (num9 == 2)
                                {
                                    worldUnitBase9.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType15));
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType15));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase10 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator31 = allUnits.GetEnumerator();
                                    while (enumerator31.MoveNext())
                                    {
                                        WorldUnitBase current31 = enumerator31.Current;
                                        if (current31.data.school == null)
                                        {
                                            worldUnitBase10 = g.world.unit.CreateUnit(current31.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase10.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType15));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase10));
                                    g.world.unit.DelUnit(worldUnitBase10);
                                }
                            }
                        }).transform;
                        transform107.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform107.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 200f, num4 - 50f);
                        Transform transform108 = CreateUI.NewText("true biography").transform;
                        transform108.SetParent(transform107, worldPositionStays: false);
                        transform108.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform108.GetComponent<Text>().color = Color.black;
                        Transform transform109 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType14 = SchoolDepartmentType.Security;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType14 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcInherit.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                int num8 = 0;
                                WorldUnitBase worldUnitBase7 = new WorldUnitBase();
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator28 = allUnits.GetEnumerator();
                                while (enumerator28.MoveNext())
                                {
                                    WorldUnitBase current28 = enumerator28.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current28.data.unitData.unitID) == schoolDepartmentType14 && current28.data.unitData.schoolID == tarSchool.id && tarSchool.npcInherit.Contains(current28.data.unitData.unitID))
                                    {
                                        num8++;
                                        worldUnitBase7 = current28;
                                    }
                                }
                                if (num8 == 2)
                                {
                                    worldUnitBase7.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType14));
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType14));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase8 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator29 = allUnits.GetEnumerator();
                                    while (enumerator29.MoveNext())
                                    {
                                        WorldUnitBase current29 = enumerator29.Current;
                                        if (current29.data.school == null)
                                        {
                                            worldUnitBase8 = g.world.unit.CreateUnit(current29.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase8.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType14));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase8));
                                    g.world.unit.DelUnit(worldUnitBase8);
                                }
                            }
                        }).transform;
                        transform109.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform109.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 100f, num4 - 50f);
                        Transform transform110 = CreateUI.NewText("true biography").transform;
                        transform110.SetParent(transform109, worldPositionStays: false);
                        transform110.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform110.GetComponent<Text>().color = Color.black;
                        Transform transform111 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType13 = SchoolDepartmentType.Train;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType13 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcInherit.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                int num7 = 0;
                                WorldUnitBase worldUnitBase5 = new WorldUnitBase();
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator26 = allUnits.GetEnumerator();
                                while (enumerator26.MoveNext())
                                {
                                    WorldUnitBase current26 = enumerator26.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current26.data.unitData.unitID) == schoolDepartmentType13 && current26.data.unitData.schoolID == tarSchool.id && tarSchool.npcInherit.Contains(current26.data.unitData.unitID))
                                    {
                                        num7++;
                                        worldUnitBase5 = current26;
                                    }
                                }
                                if (num7 == 2)
                                {
                                    worldUnitBase5.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType13));
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType13));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase6 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator27 = allUnits.GetEnumerator();
                                    while (enumerator27.MoveNext())
                                    {
                                        WorldUnitBase current27 = enumerator27.Current;
                                        if (current27.data.school == null)
                                        {
                                            worldUnitBase6 = g.world.unit.CreateUnit(current27.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase6.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType13));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase6));
                                    g.world.unit.DelUnit(worldUnitBase6);
                                }
                            }
                        }).transform;
                        transform111.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform111.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3, num4 - 50f);
                        Transform transform112 = CreateUI.NewText("true biography").transform;
                        transform112.SetParent(transform111, worldPositionStays: false);
                        transform112.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform112.GetComponent<Text>().color = Color.black;
                        Transform transform113 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType12 = SchoolDepartmentType.Personnel;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType12 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcInherit.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                int num6 = 0;
                                WorldUnitBase worldUnitBase3 = new WorldUnitBase();
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator24 = allUnits.GetEnumerator();
                                while (enumerator24.MoveNext())
                                {
                                    WorldUnitBase current24 = enumerator24.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current24.data.unitData.unitID) == schoolDepartmentType12 && current24.data.unitData.schoolID == tarSchool.id && tarSchool.npcInherit.Contains(current24.data.unitData.unitID))
                                    {
                                        num6++;
                                        worldUnitBase3 = current24;
                                    }
                                }
                                if (num6 == 2)
                                {
                                    worldUnitBase3.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType12));
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType12));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase4 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator25 = allUnits.GetEnumerator();
                                    while (enumerator25.MoveNext())
                                    {
                                        WorldUnitBase current25 = enumerator25.Current;
                                        if (current25.data.school == null)
                                        {
                                            worldUnitBase4 = g.world.unit.CreateUnit(current25.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase4.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType12));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase4));
                                    g.world.unit.DelUnit(worldUnitBase4);
                                }
                            }
                        }).transform;
                        transform113.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform113.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 100f, num4 - 50f);
                        Transform transform114 = CreateUI.NewText("true biography").transform;
                        transform114.SetParent(transform113, worldPositionStays: false);
                        transform114.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform114.GetComponent<Text>().color = Color.black;
                        Transform transform115 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType11 = SchoolDepartmentType.Diplomacy;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType11 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcInherit.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                int num5 = 0;
                                WorldUnitBase worldUnitBase = new WorldUnitBase();
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator22 = allUnits.GetEnumerator();
                                while (enumerator22.MoveNext())
                                {
                                    WorldUnitBase current22 = enumerator22.Current;
                                    if (tarSchool.postData.GetUnitDepartmentType(current22.data.unitData.unitID) == schoolDepartmentType11 && current22.data.unitData.schoolID == tarSchool.id && tarSchool.npcInherit.Contains(current22.data.unitData.unitID))
                                    {
                                        num5++;
                                        worldUnitBase = current22;
                                    }
                                }
                                if (num5 == 2)
                                {
                                    worldUnitBase.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType11));
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType11));
                                }
                                else
                                {
                                    WorldUnitBase worldUnitBase2 = new WorldUnitBase();
                                    Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator23 = allUnits.GetEnumerator();
                                    while (enumerator23.MoveNext())
                                    {
                                        WorldUnitBase current23 = enumerator23.Current;
                                        if (current23.data.school == null)
                                        {
                                            worldUnitBase2 = g.world.unit.CreateUnit(current23.data.unitData);
                                            break;
                                        }
                                    }
                                    worldUnitBase2.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Inherit, schoolDepartmentType11));
                                    tarUnit.CreateAction(new UnitActionSchoolSwapPostType(worldUnitBase2));
                                    g.world.unit.DelUnit(worldUnitBase2);
                                }
                            }
                        }).transform;
                        transform115.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform115.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 200f, num4 - 50f);
                        Transform transform116 = CreateUI.NewText("true biography").transform;
                        transform116.SetParent(transform115, worldPositionStays: false);
                        transform116.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform116.GetComponent<Text>().color = Color.black;
                        Transform transform117 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType10 = SchoolDepartmentType.Constructor;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType10 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcIn.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, schoolDepartmentType10));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType10));
                                }
                            }
                        }).transform;
                        transform117.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform117.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 200f, num4 - 100f);
                        Transform transform118 = CreateUI.NewText("Inner door").transform;
                        transform118.SetParent(transform117, worldPositionStays: false);
                        transform118.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform118.GetComponent<Text>().color = Color.black;
                        Transform transform119 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType9 = SchoolDepartmentType.Security;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType9 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcIn.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, schoolDepartmentType9));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType9));
                                }
                            }
                        }).transform;
                        transform119.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform119.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 100f, num4 - 100f);
                        Transform transform120 = CreateUI.NewText("Inner door").transform;
                        transform120.SetParent(transform119, worldPositionStays: false);
                        transform120.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform120.GetComponent<Text>().color = Color.black;
                        Transform transform121 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType8 = SchoolDepartmentType.Train;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType8 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcIn.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, schoolDepartmentType8));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType8));
                                }
                            }
                        }).transform;
                        transform121.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform121.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3, num4 - 100f);
                        Transform transform122 = CreateUI.NewText("Inner door").transform;
                        transform122.SetParent(transform121, worldPositionStays: false);
                        transform122.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform122.GetComponent<Text>().color = Color.black;
                        Transform transform123 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType7 = SchoolDepartmentType.Personnel;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType7 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcIn.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, schoolDepartmentType7));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType7));
                                }
                            }
                        }).transform;
                        transform123.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform123.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 100f, num4 - 100f);
                        Transform transform124 = CreateUI.NewText("Inner door").transform;
                        transform124.SetParent(transform123, worldPositionStays: false);
                        transform124.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform124.GetComponent<Text>().color = Color.black;
                        Transform transform125 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType6 = SchoolDepartmentType.Diplomacy;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType6 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcIn.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, schoolDepartmentType6));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.In, schoolDepartmentType6));
                                }
                            }
                        }).transform;
                        transform125.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform125.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 200f, num4 - 100f);
                        Transform transform126 = CreateUI.NewText("Inner door").transform;
                        transform126.SetParent(transform125, worldPositionStays: false);
                        transform126.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform126.GetComponent<Text>().color = Color.black;
                        Transform transform127 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType5 = SchoolDepartmentType.Constructor;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType5 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcOut.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Out, schoolDepartmentType5));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.Out, schoolDepartmentType5));
                                }
                            }
                        }).transform;
                        transform127.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform127.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 200f, num4 - 150f);
                        Transform transform128 = CreateUI.NewText("Exterior doors").transform;
                        transform128.SetParent(transform127, worldPositionStays: false);
                        transform128.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform128.GetComponent<Text>().color = Color.black;
                        Transform transform129 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType4 = SchoolDepartmentType.Security;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType4 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcOut.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Out, schoolDepartmentType4));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.Out, schoolDepartmentType4));
                                }
                            }
                        }).transform;
                        transform129.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform129.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 100f, num4 - 150f);
                        Transform transform130 = CreateUI.NewText("Exterior doors").transform;
                        transform130.SetParent(transform129, worldPositionStays: false);
                        transform130.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform130.GetComponent<Text>().color = Color.black;
                        Transform transform131 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType3 = SchoolDepartmentType.Train;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType3 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcOut.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Out, schoolDepartmentType3));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.Out, schoolDepartmentType3));
                                }
                            }
                        }).transform;
                        transform131.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform131.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3, num4 - 150f);
                        Transform transform132 = CreateUI.NewText("Exterior doors").transform;
                        transform132.SetParent(transform131, worldPositionStays: false);
                        transform132.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform132.GetComponent<Text>().color = Color.black;
                        Transform transform133 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType2 = SchoolDepartmentType.Personnel;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType2 || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcOut.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Out, schoolDepartmentType2));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.Out, schoolDepartmentType2));
                                }
                            }
                        }).transform;
                        transform133.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform133.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 100f, num4 - 150f);
                        Transform transform134 = CreateUI.NewText("Exterior doors").transform;
                        transform134.SetParent(transform133, worldPositionStays: false);
                        transform134.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform134.GetComponent<Text>().color = Color.black;
                        Transform transform135 = CreateUI.NewButton(delegate
                        {
                            SchoolDepartmentType schoolDepartmentType = SchoolDepartmentType.Diplomacy;
                            if (tarSchool.postData.GetUnitDepartmentType(tarUnit.data.unitData.unitID) != schoolDepartmentType || !(tarUnit.data.unitData.schoolID == tarSchool.id) || !tarSchool.npcOut.Contains(tarUnit.data.unitData.unitID))
                            {
                                if (tarUnit.data.school != null && tarUnit.data.unitData.schoolID != tarSchool.id)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolExit());
                                }
                                if (tarUnit.data.school == null)
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.Out, schoolDepartmentType));
                                }
                                else
                                {
                                    tarUnit.CreateAction(new UnitActionSchoolSetPostType(tarSchool.id, SchoolPostType.Out, schoolDepartmentType));
                                }
                            }
                        }).transform;
                        transform135.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform135.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 200f, num4 - 150f);
                        Transform transform136 = CreateUI.NewText("Exterior doors").transform;
                        transform136.SetParent(transform135, worldPositionStays: false);
                        transform136.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform136.GetComponent<Text>().color = Color.black;
                        Transform transform137 = CreateUI.NewButton(delegate
                        {
                            if (tarUnit.data.school != null)
                            {
                                tarUnit.CreateAction(new UnitActionSchoolExit());
                            }
                        }).transform;
                        transform137.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform137.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3, num4 - 200f);
                        Transform transform138 = CreateUI.NewText("Retirement").transform;
                        transform138.SetParent(transform137, worldPositionStays: false);
                        transform138.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform138.GetComponent<Text>().color = Color.black;
                        Transform transform139 = CreateUI.NewButton(delegate
                        {
                            Il2CppSystem.Collections.Generic.Dictionary<string, MapBuildBase>.Enumerator enumerator21 = allBuild.GetEnumerator();
                            while (enumerator21.MoveNext())
                            {
                                Il2CppSystem.Collections.Generic.KeyValuePair<string, MapBuildBase> current21 = enumerator21.Current;
                                if (allSchool.ContainsKey(current21.value.buildData.id))
                                {
                                    MelonLogger.Msg(current21.value.name);
                                }
                            }
                        }).transform;
                        transform139.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 50f);
                        transform139.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform139.GetComponent<RectTransform>().anchoredPosition = new Vector2(num2, -255f);
                        Transform transform140 = CreateUI.NewText("ConsOut all sect names", new Vector2(250f, 50f)).transform;
                        transform140.SetParent(transform139, worldPositionStays: false);
                        transform140.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform140.GetComponent<Text>().color = Color.black;
                        Transform transform141 = CreateUI.NewButton(delegate
                        {
                            Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator20 = allUnits.GetEnumerator();
                            while (enumerator20.MoveNext())
                            {
                                WorldUnitBase current20 = enumerator20.Current;
                                if (tarUnit.data.unitData.relationData.GetRelation(current20) == UnitRelationType.Lover)
                                {
                                    if (current20.data.school != null && current20.data.unitData.schoolID != tarSchool.id)
                                    {
                                        current20.CreateAction(new UnitActionSchoolExit());
                                    }
                                    if (current20.data.unitData.schoolID != tarSchool.id)
                                    {
                                        current20.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, SchoolDepartmentType.None));
                                    }
                                }
                            }
                        }).transform;
                        transform141.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform141.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 150f, num4 - 200f);
                        transform141.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 40f);
                        Transform transform142 = CreateUI.NewText("Taoist companions can join inner gate", new Vector2(200f, 50f)).transform;
                        transform142.SetParent(transform141, worldPositionStays: false);
                        transform142.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform142.GetComponent<Text>().color = Color.black;
                        Transform transform143 = CreateUI.NewButton(delegate
                        {
                            Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator19 = allUnits.GetEnumerator();
                            while (enumerator19.MoveNext())
                            {
                                WorldUnitBase current19 = enumerator19.Current;
                                if (tarUnit.data.unitData.relationData.GetRelation(current19) == UnitRelationType.Master)
                                {
                                    if (current19.data.school != null && current19.data.unitData.schoolID != tarSchool.id)
                                    {
                                        current19.CreateAction(new UnitActionSchoolExit());
                                    }
                                    if (current19.data.unitData.schoolID != tarSchool.id)
                                    {
                                        current19.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, SchoolDepartmentType.None));
                                    }
                                }
                            }
                        }).transform;
                        transform143.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform143.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 150f, num4 - 240f);
                        transform143.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 40f);
                        Transform transform144 = CreateUI.NewText("Master joins inner door", new Vector2(200f, 50f)).transform;
                        transform144.SetParent(transform143, worldPositionStays: false);
                        transform144.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform144.GetComponent<Text>().color = Color.black;
                        Transform transform145 = CreateUI.NewButton(delegate
                        {
                            Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator18 = allUnits.GetEnumerator();
                            while (enumerator18.MoveNext())
                            {
                                WorldUnitBase current18 = enumerator18.Current;
                                if (tarUnit.data.unitData.relationData.GetRelation(current18) == UnitRelationType.Children || tarUnit.data.unitData.relationData.GetRelation(current18) == UnitRelationType.ChildrenBack || tarUnit.data.unitData.relationData.GetRelation(current18) == UnitRelationType.ChildrenPrivate)
                                {
                                    if (current18.data.school != null && current18.data.unitData.schoolID != tarSchool.id)
                                    {
                                        current18.CreateAction(new UnitActionSchoolExit());
                                    }
                                    if (current18.data.unitData.schoolID != tarSchool.id)
                                    {
                                        current18.CreateAction(new UnitActionSchoolJoin(tarSchool.id, SchoolPostType.In, SchoolDepartmentType.None));
                                    }
                                }
                            }
                        }).transform;
                        transform145.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform145.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 150f, num4 - 280f);
                        transform145.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 40f);
                        Transform transform146 = CreateUI.NewText("Children can join inner door", new Vector2(200f, 50f)).transform;
                        transform146.SetParent(transform145, worldPositionStays: false);
                        transform146.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform146.GetComponent<Text>().color = Color.black;
                        Transform transform147 = CreateUI.NewButton(delegate
                        {
                            Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator17 = allUnits.GetEnumerator();
                            while (enumerator17.MoveNext())
                            {
                                WorldUnitBase current17 = enumerator17.Current;
                                if (current17.data.school != null && current17.data.unitData.schoolID == tarSchool.id)
                                {
                                    current17.CreateAction(new UnitActionSchoolExit());
                                }
                            }
                        }).transform;
                        transform147.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform147.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 150f, num4 - 200f);
                        transform147.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 40f);
                        Transform transform148 = CreateUI.NewText("Kick out all sect members", new Vector2(200f, 50f)).transform;
                        transform148.SetParent(transform147, worldPositionStays: false);
                        transform148.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform148.GetComponent<Text>().color = Color.black;
                        Transform transform149 = CreateUI.NewButton(delegate
                        {
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 1, 100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 2, 100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 3, 100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 4, 100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 5, 100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 7, 100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 8, 100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 9, 100000000);
                        }).transform;
                        transform149.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform149.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 150f, num4 - 240f);
                        transform149.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 40f);
                        Transform transform150 = CreateUI.NewText("Full sect resources", new Vector2(200f, 50f)).transform;
                        transform150.SetParent(transform149, worldPositionStays: false);
                        transform150.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform150.GetComponent<Text>().color = Color.black;
                        Transform transform151 = CreateUI.NewButton(delegate
                        {
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 1, -100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 2, -100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 3, -100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 4, -100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 5, -100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 7, -100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 8, -100000000);
                            tarSchool.AddSchoolAttr(g.world.build.GetBuild<MapBuildSchool>(tarSchool.id), 9, -100000000);
                        }).transform;
                        transform151.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform151.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 + 150f, num4 - 280f);
                        transform151.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 40f);
                        Transform transform152 = CreateUI.NewText("Clear sect resources", new Vector2(200f, 50f)).transform;
                        transform152.SetParent(transform151, worldPositionStays: false);
                        transform152.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform152.GetComponent<Text>().color = Color.black;
                        Transform transform153 = CreateUI.NewButton(delegate
                        {
                            g.world.build.GetBuild<MapBuildSchool>(tarSchool.id).OpenBuild();
                            UnityEngine.Object.Destroy(uiPanel);
                            ModifierMain.IsChange = !ModifierMain.IsChange;
                        }).transform;
                        transform153.SetParent(gameObject3.transform, worldPositionStays: false);
                        transform153.GetComponent<RectTransform>().anchoredPosition = new Vector2(num3 - 200f, 150f);
                        Transform transform154 = CreateUI.NewText("Open sect").transform;
                        transform154.SetParent(transform153, worldPositionStays: false);
                        transform154.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform154.GetComponent<Text>().color = Color.black;
                    }
                }).transform;
                list.Add(transform23);
                if (list.Contains(transform23))
                {
                    Transform transform24 = CreateUI.NewText("宗门").transform;
                    transform24.SetParent(transform23.transform, worldPositionStays: false);
                    transform24.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    transform24.GetComponent<Text>().color = Color.black;
                }
            }
            if (flag)
            {
                Transform transform25 = CreateUI.NewButton(delegate
                {
                    if (uiPanel.transform.Find("chuansong") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("chuansong").gameObject);
                    }
                    else if (uiPanel.transform.Find("gaiMingZi") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("gaiMingZi").gameObject);
                    }
                    else if (uiPanel.transform.Find("shaQuanjia") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("shaQuanjia").gameObject);
                    }
                    else if (uiPanel.transform.Find("jieYuan") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("jieYuan").gameObject);
                    }
                    else if (uiPanel.transform.Find("zaXiang") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("zaXiang").gameObject);
                    }
                    else if (uiPanel.transform.Find("XingGeuiPanel") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("XingGeuiPanel").gameObject);
                    }
                    else if (uiPanel.transform.Find("QiLing") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("QiLing").gameObject);
                    }
                    else if (uiPanel.transform.Find("ZongMen") != null)
                    {
                        UnityEngine.Object.Destroy(uiPanel.transform.Find("ZongMen").gameObject);
                    }
                    if (uiPanel.transform.Find("zaXiang") == null)
                    {
                        GameObject gameObject2 = new GameObject("zaXiang");
                        gameObject2.transform.SetParent(uiPanel.transform, worldPositionStays: false);
                        componentTittle.text = "****Miscellaneous****";
                        Transform transform27 = CreateUI.NewText("Changes in weather conditions", new Vector2(500f, 90f)).transform;
                        transform27.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform27.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, 150f);
                        Text component4 = transform27.GetComponent<Text>();
                        component4.fontSize = 28;
                        component4.alignment = TextAnchor.MiddleCenter;
                        component4.color = Color.black;
                        Transform transform28 = CreateUI.NewInputField(null, null, "target name").transform;
                        transform28.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform28.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
                        transform28.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, 100f);
                        InputField iptFieldXing51 = transform28.GetComponent<InputField>();
                        iptFieldXing51.contentType = InputField.ContentType.Name;
                        iptFieldXing51.text = g.world.playerUnit.data.unitData.propertyData.GetName();
                        iptFieldXing51.characterLimit = 6;
                        Transform transform29 = CreateUI.NewInputField(null, null, "Luck ID").transform;
                        transform29.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform29.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
                        transform29.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, 50f);
                        InputField iptFieldXing5 = transform29.GetComponent<InputField>();
                        iptFieldXing5.contentType = InputField.ContentType.DecimalNumber;
                        iptFieldXing5.characterLimit = 12;
                        Transform transform30 = CreateUI.NewText("Please enter the target's name and luck ID\nNote: The innate luck cannot be modified.", new Vector2(800f, 100f)).transform;
                        transform30.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform30.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, -50f);
                        Text componentResultt = transform30.GetComponent<Text>();
                        componentResultt.fontSize = 18;
                        componentResultt.alignment = TextAnchor.MiddleCenter;
                        componentResultt.color = Color.black;
                        WorldUnitBase tarunit5 = null;
                        bool hasUnit = false;
                        Transform transform31 = CreateUI.NewButton(delegate
                        {
                            hasUnit = false;
                            tarunit5 = null;
                            if (iptFieldXing51.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit5 = g.world.playerUnit;
                                hasUnit = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator16 = allUnits.GetEnumerator();
                                while (enumerator16.MoveNext())
                                {
                                    WorldUnitBase current16 = enumerator16.Current;
                                    if (current16.data.unitData.propertyData.GetName() == iptFieldXing51.text)
                                    {
                                        tarunit5 = current16;
                                        hasUnit = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit)
                            {
                                if (int.TryParse(iptFieldXing5.text, out var result8))
                                {
                                    if (g.conf.roleCreateFeature.GetItem(result8) != null)
                                    {
                                        if (g.conf.roleCreateFeature.GetItem(result8).type == 2)
                                        {
                                            if (tarunit5.GetLuck(result8) == null)
                                            {
                                                tarunit5.CreateAction(new UnitActionLuckAdd(result8));
                                                componentResultt.text = "Adding luck successfully!";
                                            }
                                            else
                                            {
                                                componentResultt.text = "Already have this luck!";
                                            }
                                        }
                                        else
                                        {
                                            componentResultt.text = "Only modified weather luck is allowed!";
                                        }
                                    }
                                    else
                                    {
                                        componentResultt.text = "There is no such luck!";
                                    }
                                }
                                else
                                {
                                    componentResultt.text = "Please fill in the correct luck!";
                                }
                            }
                            else
                            {
                                componentResultt.text = "The character does not exist!";
                            }
                        }).transform;
                        transform31.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform31.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, 0f);
                        Transform transform32 = CreateUI.NewText("Add luck").transform;
                        transform32.SetParent(transform31.transform, worldPositionStays: false);
                        transform32.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform32.GetComponent<Text>().color = Color.black;
                        Transform transform33 = CreateUI.NewButton(delegate
                        {
                            hasUnit = false;
                            tarunit5 = null;
                            if (iptFieldXing51.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit5 = g.world.playerUnit;
                                hasUnit = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator15 = allUnits.GetEnumerator();
                                while (enumerator15.MoveNext())
                                {
                                    WorldUnitBase current15 = enumerator15.Current;
                                    if (current15.data.unitData.propertyData.GetName() == iptFieldXing51.text)
                                    {
                                        tarunit5 = current15;
                                        hasUnit = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit)
                            {
                                if (int.TryParse(iptFieldXing5.text, out var result7))
                                {
                                    if (g.conf.roleCreateFeature.GetItem(result7) != null)
                                    {
                                        if (g.conf.roleCreateFeature.GetItem(result7).type == 2)
                                        {
                                            if (tarunit5.GetLuck(result7) != null)
                                            {
                                                tarunit5.CreateAction(new UnitActionLuckDel(result7));
                                                componentResultt.text = "Luck removal successful!";
                                            }
                                            else
                                            {
                                                componentResultt.text = "Don’t have this luck!";
                                            }
                                        }
                                        else
                                        {
                                            componentResultt.text = "Modification of innate weather luck is not allowed!";
                                        }
                                    }
                                    else
                                    {
                                        componentResultt.text = "There is no such luck!";
                                    }
                                }
                                else
                                {
                                    componentResultt.text = "Please fill in the correct luck!";
                                }
                            }
                            else
                            {
                                componentResultt.text = "The character does not exist!";
                            }
                        }).transform;
                        transform33.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform33.GetComponent<RectTransform>().anchoredPosition = new Vector2(-250f, 0f);
                        Transform transform34 = CreateUI.NewText("Remove luck").transform;
                        transform34.SetParent(transform33.transform, worldPositionStays: false);
                        transform34.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform34.GetComponent<Text>().color = Color.black;
                        Transform transform35 = CreateUI.NewText("Qi Luck ID search", new Vector2(500f, 90f)).transform;
                        transform35.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform35.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, -100f);
                        Text component5 = transform35.GetComponent<Text>();
                        component5.fontSize = 28;
                        component5.alignment = TextAnchor.MiddleCenter;
                        component5.color = Color.black;
                        Transform transform36 = CreateUI.NewInputField(null, null, "Acquired lucky name").transform;
                        transform36.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform36.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
                        transform36.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, -150f);
                        InputField iptField002 = transform36.GetComponent<InputField>();
                        iptField002.contentType = InputField.ContentType.Name;
                        iptField002.characterLimit = 8;
                        Transform transform37 = CreateUI.NewText("Please enter your lucky name to search for ID\nNote: If there are duplicate names, only the first one can be searched", new Vector2(800f, 100f)).transform;
                        transform37.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform37.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, -250f);
                        Text component003 = transform37.GetComponent<Text>();
                        component003.fontSize = 18;
                        component003.alignment = TextAnchor.MiddleCenter;
                        component003.color = Color.black;
                        Transform transform38 = CreateUI.NewButton(delegate
                        {
                            bool flag2 = false;
                            int num = 0;
                            Il2CppSystem.Collections.Generic.Dictionary<string, ConfLocalTextItem>.Enumerator enumerator13 = g.conf.localText.allText.GetEnumerator();
                            while (enumerator13.MoveNext())
                            {
                                Il2CppSystem.Collections.Generic.KeyValuePair<string, ConfLocalTextItem> current13 = enumerator13.Current;
                                if (current13.value.ch == iptField002.text)
                                {
                                    Il2CppSystem.Collections.Generic.List<ConfRoleCreateFeatureItem>.Enumerator enumerator14 = allFeature.GetEnumerator();
                                    while (enumerator14.MoveNext())
                                    {
                                        ConfRoleCreateFeatureItem current14 = enumerator14.Current;
                                        if (current14.name == current13.value.key)
                                        {
                                            num = current14.id;
                                            flag2 = true;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            if (flag2)
                            {
                                component003.text = "Searched<color=#004FCA>" + iptField002.text + "</color>的ID为<color=#004FCA>" + num + "</color>";
                            }
                            else
                            {
                                component003.text = "The luck was not found";
                            }
                        }).transform;
                        transform38.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform38.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, -200f);
                        Transform transform39 = CreateUI.NewText("search").transform;
                        transform39.SetParent(transform38, worldPositionStays: false);
                        transform39.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform39.GetComponent<Text>().color = Color.black;
                        Transform transform40 = CreateUI.NewText("Lingshi/City Lord’s Order/Sect Contribution Modification", new Vector2(500f, 90f)).transform;
                        transform40.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform40.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 150f);
                        Text componentXingMing501 = transform40.GetComponent<Text>();
                        componentXingMing501.fontSize = 28;
                        componentXingMing501.alignment = TextAnchor.MiddleCenter;
                        componentXingMing501.color = Color.black;
                        Transform transform41 = CreateUI.NewInputField(null, null, "target name").transform;
                        transform41.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform41.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 50f);
                        transform41.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 100f);
                        InputField iptFieldXing5001 = transform41.GetComponent<InputField>();
                        iptFieldXing5001.contentType = InputField.ContentType.Name;
                        iptFieldXing5001.text = g.world.playerUnit.data.unitData.propertyData.GetName();
                        iptFieldXing5001.characterLimit = 6;
                        Transform transform42 = CreateUI.NewInputField(null, null, "Spiritual stones/tokens/number of contributions").transform;
                        transform42.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform42.GetComponent<RectTransform>().sizeDelta = new Vector2(250f, 50f);
                        transform42.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 50f);
                        InputField iptFieldXing502 = transform42.GetComponent<InputField>();
                        iptFieldXing502.contentType = InputField.ContentType.DecimalNumber;
                        iptFieldXing502.characterLimit = 12;
                        Transform transform43 = CreateUI.NewText("Please enter the target’s name and number of spirit stones/city lord’s order/sect contribution", new Vector2(400f, 100f)).transform;
                        transform43.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform43.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -150f);
                        Text component6 = transform43.GetComponent<Text>();
                        component6.fontSize = 18;
                        component6.alignment = TextAnchor.MiddleCenter;
                        component6.color = Color.black;
                        WorldUnitBase tarunit51 = null;
                        bool hasUnit1 = false;
                        Transform transform44 = CreateUI.NewButton(delegate
                        {
                            hasUnit1 = false;
                            tarunit51 = null;
                            if (iptFieldXing5001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit51 = g.world.playerUnit;
                                hasUnit1 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator12 = allUnits.GetEnumerator();
                                while (enumerator12.MoveNext())
                                {
                                    WorldUnitBase current12 = enumerator12.Current;
                                    if (current12.data.unitData.propertyData.GetName() == iptFieldXing5001.text)
                                    {
                                        tarunit51 = current12;
                                        hasUnit1 = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit1)
                            {
                                if (iptFieldXing502.text != null)
                                {
                                    if (int.TryParse(iptFieldXing502.text, out var result6))
                                    {
                                        tarunit51.data.RewardPropItem(PropsIDType.Money, result6);
                                        componentXingMing501.text = "Adding spiritual stones successfully!";
                                    }
                                    else
                                    {
                                        componentXingMing501.text = "Please fill in the number of spirit stones correctly!";
                                    }
                                }
                                else
                                {
                                    componentXingMing501.text = "Please fill in the number of spiritual stones!";
                                }
                            }
                            else
                            {
                                componentXingMing501.text = "The character does not exist!";
                            }
                        }).transform;
                        transform44.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform44.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, 0f);
                        Transform transform45 = CreateUI.NewText("Add spirit stone").transform;
                        transform45.SetParent(transform44.transform, worldPositionStays: false);
                        transform45.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform45.GetComponent<Text>().color = Color.black;
                        Transform transform46 = CreateUI.NewButton(delegate
                        {
                            hasUnit1 = false;
                            tarunit51 = null;
                            if (iptFieldXing5001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit51 = g.world.playerUnit;
                                hasUnit1 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator11 = allUnits.GetEnumerator();
                                while (enumerator11.MoveNext())
                                {
                                    WorldUnitBase current11 = enumerator11.Current;
                                    if (current11.data.unitData.propertyData.GetName() == iptFieldXing5001.text)
                                    {
                                        tarunit51 = current11;
                                        hasUnit1 = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit1)
                            {
                                if (iptFieldXing502.text != null)
                                {
                                    if (int.TryParse(iptFieldXing502.text, out var result5))
                                    {
                                        if (tarunit51.data.unitData.propData.GetPropsNum(PropsIDType.Money) >= result5)
                                        {
                                            tarunit51.data.CostPropItem(PropsIDType.Money, result5);
                                            componentXingMing501.text = "The spirit stone was successfully removed!";
                                        }
                                        else
                                        {
                                            componentXingMing501.text = "The target spirit stone is insufficient!";
                                        }
                                    }
                                    else
                                    {
                                        componentXingMing501.text = "Please fill in the number of spirit stones correctly!";
                                    }
                                }
                                else
                                {
                                    componentXingMing501.text = "Please fill in the number of spiritual stones!";
                                }
                            }
                            else
                            {
                                componentXingMing501.text = "The character does not exist!";
                            }
                        }).transform;
                        transform46.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform46.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                        Transform transform47 = CreateUI.NewText("Remove spirit stone").transform;
                        transform47.SetParent(transform46.transform, worldPositionStays: false);
                        transform47.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform47.GetComponent<Text>().color = Color.black;
                        Transform transform48 = CreateUI.NewButton(delegate
                        {
                            hasUnit1 = false;
                            tarunit51 = null;
                            if (iptFieldXing5001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit51 = g.world.playerUnit;
                                hasUnit1 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator10 = allUnits.GetEnumerator();
                                while (enumerator10.MoveNext())
                                {
                                    WorldUnitBase current10 = enumerator10.Current;
                                    if (current10.data.unitData.propertyData.GetName() == iptFieldXing5001.text)
                                    {
                                        tarunit51 = current10;
                                        hasUnit1 = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit1)
                            {
                                tarunit51.data.CostPropItem(PropsIDType.Money, tarunit51.data.unitData.propData.GetPropsNum(PropsIDType.Money));
                                componentXingMing501.text = "Clearing the spirit stone successfully!";
                            }
                            else
                            {
                                componentXingMing501.text = "The character does not exist!";
                            }
                        }).transform;
                        transform48.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform48.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, 0f);
                        Transform transform49 = CreateUI.NewText("Clear the spirit stone").transform;
                        transform49.SetParent(transform48.transform, worldPositionStays: false);
                        transform49.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform49.GetComponent<Text>().color = Color.black;
                        Transform transform50 = CreateUI.NewButton(delegate
                        {
                            hasUnit1 = false;
                            tarunit51 = null;
                            if (iptFieldXing5001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit51 = g.world.playerUnit;
                                hasUnit1 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator9 = allUnits.GetEnumerator();
                                while (enumerator9.MoveNext())
                                {
                                    WorldUnitBase current9 = enumerator9.Current;
                                    if (current9.data.unitData.propertyData.GetName() == iptFieldXing5001.text)
                                    {
                                        tarunit51 = current9;
                                        hasUnit1 = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit1)
                            {
                                if (iptFieldXing502.text != null)
                                {
                                    if (int.TryParse(iptFieldXing502.text, out var result4))
                                    {
                                        tarunit51.data.RewardPropItem(PropsIDType.SchoolMoney, result4);
                                        componentXingMing501.text = "Adding sect contribution successfully!";
                                    }
                                    else
                                    {
                                        componentXingMing501.text = "Please fill in the amount of sect contribution correctly!";
                                    }
                                }
                                else
                                {
                                    componentXingMing501.text = "Please fill in the amount of sect contribution!";
                                }
                            }
                            else
                            {
                                componentXingMing501.text = "The character does not exist!";
                            }
                        }).transform;
                        transform50.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform50.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -100f);
                        Transform transform51 = CreateUI.NewText("Add contribution").transform;
                        transform51.SetParent(transform50.transform, worldPositionStays: false);
                        transform51.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform51.GetComponent<Text>().color = Color.black;
                        Transform transform52 = CreateUI.NewButton(delegate
                        {
                            hasUnit1 = false;
                            tarunit51 = null;
                            if (iptFieldXing5001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit51 = g.world.playerUnit;
                                hasUnit1 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator8 = allUnits.GetEnumerator();
                                while (enumerator8.MoveNext())
                                {
                                    WorldUnitBase current8 = enumerator8.Current;
                                    if (current8.data.unitData.propertyData.GetName() == iptFieldXing5001.text)
                                    {
                                        tarunit51 = current8;
                                        hasUnit1 = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit1)
                            {
                                if (iptFieldXing502.text != null)
                                {
                                    if (int.TryParse(iptFieldXing502.text, out var result3))
                                    {
                                        if (tarunit51.data.unitData.propData.GetPropsNum(PropsIDType.SchoolMoney) >= result3)
                                        {
                                            tarunit51.data.CostPropItem(PropsIDType.SchoolMoney, result3);
                                            componentXingMing501.text = "Removing sect contribution successfully!";
                                        }
                                        else
                                        {
                                            componentXingMing501.text = "The target sect’s contribution is insufficient!";
                                        }
                                    }
                                    else
                                    {
                                        componentXingMing501.text = "Please fill in the amount of sect contribution correctly!";
                                    }
                                }
                                else
                                {
                                    componentXingMing501.text = "Please fill in the amount of sect contribution!";
                                }
                            }
                            else
                            {
                                componentXingMing501.text = "The character does not exist!";
                            }
                        }).transform;
                        transform52.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform52.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -100f);
                        Transform transform53 = CreateUI.NewText("Remove contribution").transform;
                        transform53.SetParent(transform52.transform, worldPositionStays: false);
                        transform53.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform53.GetComponent<Text>().color = Color.black;
                        Transform transform54 = CreateUI.NewButton(delegate
                        {
                            hasUnit1 = false;
                            tarunit51 = null;
                            if (iptFieldXing5001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit51 = g.world.playerUnit;
                                hasUnit1 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator7 = allUnits.GetEnumerator();
                                while (enumerator7.MoveNext())
                                {
                                    WorldUnitBase current7 = enumerator7.Current;
                                    if (current7.data.unitData.propertyData.GetName() == iptFieldXing5001.text)
                                    {
                                        tarunit51 = current7;
                                        hasUnit1 = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit1)
                            {
                                tarunit51.data.CostPropItem(PropsIDType.SchoolMoney, tarunit51.data.unitData.propData.GetPropsNum(PropsIDType.SchoolMoney));
                                componentXingMing501.text = "Clearing the sect contribution successfully!";
                            }
                            else
                            {
                                componentXingMing501.text = "The character does not exist!";
                            }
                        }).transform;
                        transform54.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform54.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -100f);
                        Transform transform55 = CreateUI.NewText("Clear contributions").transform;
                        transform55.SetParent(transform54.transform, worldPositionStays: false);
                        transform55.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform55.GetComponent<Text>().color = Color.black;
                        Transform transform56 = CreateUI.NewButton(delegate
                        {
                            hasUnit1 = false;
                            tarunit51 = null;
                            if (iptFieldXing5001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit51 = g.world.playerUnit;
                                hasUnit1 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator6 = allUnits.GetEnumerator();
                                while (enumerator6.MoveNext())
                                {
                                    WorldUnitBase current6 = enumerator6.Current;
                                    if (current6.data.unitData.propertyData.GetName() == iptFieldXing5001.text)
                                    {
                                        tarunit51 = current6;
                                        hasUnit1 = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit1)
                            {
                                if (iptFieldXing502.text != null)
                                {
                                    if (int.TryParse(iptFieldXing502.text, out var result2))
                                    {
                                        tarunit51.data.RewardPropItem(PropsIDType.CityToken, result2);
                                        componentXingMing501.text = "Adding the City Lord Order was successful!";
                                    }
                                    else
                                    {
                                        componentXingMing501.text = "Please fill in the number of City Lord Orders correctly!";
                                    }
                                }
                                else
                                {
                                    componentXingMing501.text = "Please fill in the number of City Lord Orders!";
                                }
                            }
                            else
                            {
                                componentXingMing501.text = "The character does not exist!";
                            }
                        }).transform;
                        transform56.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform56.GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f, -50f);
                        Transform transform57 = CreateUI.NewText("add token").transform;
                        transform57.SetParent(transform56.transform, worldPositionStays: false);
                        transform57.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform57.GetComponent<Text>().color = Color.black;
                        Transform transform58 = CreateUI.NewButton(delegate
                        {
                            hasUnit1 = false;
                            tarunit51 = null;
                            if (iptFieldXing5001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit51 = g.world.playerUnit;
                                hasUnit1 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator5 = allUnits.GetEnumerator();
                                while (enumerator5.MoveNext())
                                {
                                    WorldUnitBase current5 = enumerator5.Current;
                                    if (current5.data.unitData.propertyData.GetName() == iptFieldXing5001.text)
                                    {
                                        tarunit51 = current5;
                                        hasUnit1 = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit1)
                            {
                                if (iptFieldXing502.text != null)
                                {
                                    if (int.TryParse(iptFieldXing502.text, out var result))
                                    {
                                        if (tarunit51.data.unitData.propData.GetPropsNum(PropsIDType.CityToken) >= result)
                                        {
                                            tarunit51.data.CostPropItem(PropsIDType.CityToken, result);
                                            componentXingMing501.text = "The city lord order was removed successfully!";
                                        }
                                        else
                                        {
                                            componentXingMing501.text = "The target city lord's order is insufficient!";
                                        }
                                    }
                                    else
                                    {
                                        componentXingMing501.text = "Please fill in the number of City Lord Orders correctly!";
                                    }
                                }
                                else
                                {
                                    componentXingMing501.text = "Please fill in the number of City Lord Orders!";
                                }
                            }
                            else
                            {
                                componentXingMing501.text = "The character does not exist!";
                            }
                        }).transform;
                        transform58.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform58.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -50f);
                        Transform transform59 = CreateUI.NewText("Remove token").transform;
                        transform59.SetParent(transform58.transform, worldPositionStays: false);
                        transform59.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform59.GetComponent<Text>().color = Color.black;
                        Transform transform60 = CreateUI.NewButton(delegate
                        {
                            hasUnit1 = false;
                            tarunit51 = null;
                            if (iptFieldXing5001.text == g.world.playerUnit.data.unitData.propertyData.GetName())
                            {
                                tarunit51 = g.world.playerUnit;
                                hasUnit1 = true;
                            }
                            else
                            {
                                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator4 = allUnits.GetEnumerator();
                                while (enumerator4.MoveNext())
                                {
                                    WorldUnitBase current4 = enumerator4.Current;
                                    if (current4.data.unitData.propertyData.GetName() == iptFieldXing5001.text)
                                    {
                                        tarunit51 = current4;
                                        hasUnit1 = true;
                                        break;
                                    }
                                }
                            }
                            if (hasUnit1)
                            {
                                tarunit51.data.CostPropItem(PropsIDType.CityToken, tarunit51.data.unitData.propData.GetPropsNum(PropsIDType.CityToken));
                                componentXingMing501.text = "Clearing the City Lord Order was successful!";
                            }
                            else
                            {
                                componentXingMing501.text = "The character does not exist!";
                            }
                        }).transform;
                        transform60.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform60.GetComponent<RectTransform>().anchoredPosition = new Vector2(100f, -50f);
                        Transform transform61 = CreateUI.NewText("Clear token").transform;
                        transform61.SetParent(transform60.transform, worldPositionStays: false);
                        transform61.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform61.GetComponent<Text>().color = Color.black;
                        Transform transform62 = CreateUI.NewText("Complete task", new Vector2(500f, 90f)).transform;
                        transform62.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform62.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, 150f);
                        Text component7 = transform62.GetComponent<Text>();
                        component7.fontSize = 28;
                        component7.alignment = TextAnchor.MiddleCenter;
                        component7.color = Color.black;
                        Transform transform63 = CreateUI.NewButton(delegate
                        {
                            Il2CppSystem.Collections.Generic.List<TaskBase>.Enumerator enumerator3 = g.world.playerUnit.GetTaskInType(TaskType.School).GetEnumerator();
                            while (enumerator3.MoveNext())
                            {
                                TaskBase current3 = enumerator3.Current;
                                current3.TaskComplete();
                            }
                        }).transform;
                        transform63.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform63.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform63.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, 100f);
                        Transform transform64 = CreateUI.NewText("Complete sect mission", new Vector2(150f, 50f)).transform;
                        transform64.SetParent(transform63.transform, worldPositionStays: false);
                        transform64.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform64.GetComponent<Text>().color = Color.black;
                        Transform transform65 = CreateUI.NewButton(delegate
                        {
                            Il2CppSystem.Collections.Generic.List<TaskBase>.Enumerator enumerator2 = g.world.playerUnit.GetTaskInType(TaskType.Town).GetEnumerator();
                            while (enumerator2.MoveNext())
                            {
                                TaskBase current2 = enumerator2.Current;
                                current2.TaskComplete();
                            }
                        }).transform;
                        transform65.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform65.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform65.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, 50f);
                        Transform transform66 = CreateUI.NewText("Complete town missions", new Vector2(150f, 50f)).transform;
                        transform66.SetParent(transform65.transform, worldPositionStays: false);
                        transform66.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform66.GetComponent<Text>().color = Color.black;
                        Transform transform67 = CreateUI.NewText("End battle", new Vector2(500f, 90f)).transform;
                        transform67.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform67.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, 0f);
                        Text component8 = transform67.GetComponent<Text>();
                        component8.fontSize = 28;
                        component8.alignment = TextAnchor.MiddleCenter;
                        component8.color = Color.black;
                        Transform transform68 = CreateUI.NewButton(delegate
                        {
                            LuciferDrama.BattleExit(new Il2CppStringArray(new string[2] { "battleExit", "1" }));
                        }).transform;
                        transform68.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform68.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform68.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, -50f);
                        Transform transform69 = CreateUI.NewText("Battle victory", new Vector2(150f, 50f)).transform;
                        transform69.SetParent(transform68.transform, worldPositionStays: false);
                        transform69.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform69.GetComponent<Text>().color = Color.black;
                        Transform transform70 = CreateUI.NewButton(delegate
                        {
                            LuciferDrama.BattleExit(new Il2CppStringArray(new string[2] { "battleExit", "0" }));
                        }).transform;
                        transform70.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform70.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform70.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, -100f);
                        Transform transform71 = CreateUI.NewText("Battle lost", new Vector2(150f, 50f)).transform;
                        transform71.SetParent(transform70.transform, worldPositionStays: false);
                        transform71.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform71.GetComponent<Text>().color = Color.black;
                        Transform transform72 = CreateUI.NewText("Increased field of view", new Vector2(500f, 90f)).transform;
                        transform72.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform72.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, -150f);
                        Text component9 = transform72.GetComponent<Text>();
                        component9.fontSize = 28;
                        component9.alignment = TextAnchor.MiddleCenter;
                        component9.color = Color.black;
                        Transform transform73 = CreateUI.NewInputField(null, null, "View range (0-999)").transform;
                        transform73.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform73.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 50f);
                        transform73.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, -200f);
                        InputField iptField1002 = transform73.GetComponent<InputField>();
                        iptField1002.contentType = InputField.ContentType.DecimalNumber;
                        iptField1002.characterLimit = 3;
                        Transform transform74 = CreateUI.NewButton(delegate
                        {
                            LuciferDrama.Sight(new Il2CppStringArray(new string[2] { "sight", iptField1002.text }));
                        }).transform;
                        transform74.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform74.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform74.GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, -250f);
                        Transform transform75 = CreateUI.NewText("Increase vision").transform;
                        transform75.SetParent(transform74, worldPositionStays: false);
                        transform75.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform75.GetComponent<Text>().color = Color.black;
                        Transform transform76 = CreateUI.NewButton(delegate
                        {
                            LuciferDrama.LearnSkill(new Il2CppStringArray(new string[1] { "learnSkill" }));
                            UnityEngine.Object.Destroy(uiPanel);
                            ModifierMain.IsChange = !ModifierMain.IsChange;
                        }).transform;
                        transform76.GetComponent<RectTransform>().sizeDelta = new Vector2(150f, 50f);
                        transform76.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform76.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
                        Transform transform77 = CreateUI.NewText("Learn exercises quickly", new Vector2(150f, 50f)).transform;
                        transform77.SetParent(transform76.transform, worldPositionStays: false);
                        transform77.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                        transform77.GetComponent<Text>().color = Color.black;
                        Transform transform78 = CreateUI.NewText("Open cheat selection page", new Vector2(400f, 100f)).transform;
                        transform78.SetParent(gameObject2.transform, worldPositionStays: false);
                        transform78.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -250f);
                        Text component10 = transform78.GetComponent<Text>();
                        component10.fontSize = 20;
                        component10.alignment = TextAnchor.MiddleCenter;
                        component10.color = Color.black;
                    }
                }).transform;
                list.Add(transform25);
                if (list.Contains(transform25))
                {
                    Transform transform26 = CreateUI.NewText("Misc").transform;
                    transform26.SetParent(transform25.transform, worldPositionStays: false);
                    transform26.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                    transform26.GetComponent<Text>().color = Color.black;
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].SetParent(uiPanel.transform, worldPositionStays: false);
                if (list.Count % 2 == 0)
                {
                    list[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f * (float)(list.Count - 1) + (float)i * 100f, 200f);
                }
                else
                {
                    list[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-100f * (float)(list.Count - 1) / 2f + (float)i * 100f, 200f);
                }
            }
        }
    }
}
