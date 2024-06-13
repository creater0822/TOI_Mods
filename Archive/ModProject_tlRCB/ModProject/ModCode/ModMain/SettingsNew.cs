using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB
{
    public static class SettingsNew
    {
        private static bool _isOpen;

        private static GameObject root;

        private static List<GameObject> Panels;

        private static GameObject GameSlider;

        public static float x;

        private static float y;

        private static float width;

        private static float height;

        public static bool IsOpen
        {
            get
            {
                return _isOpen;
            }
            set
            {
                if (value)
                {
                    Open();
                }
                else
                {
                    Close();
                }
            }
        }

        static SettingsNew()
        {
            _isOpen = false;
            x = Screen.width;
            y = Screen.height;
            GetSlider();
        }

        private static void GetSlider()
        {
            UIBase ui = g.ui.OpenUI(UIType.GameSetting);
            foreach (GameObject item in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (item.GetComponent<Slider>() != null)
                {
                    GameSlider = UnityEngine.Object.Instantiate(item);
                    break;
                }
            }
            g.ui.CloseUI(ui);
        }

        public static void Open()
        {
            if (!_isOpen)
            {
                _isOpen = true;
                root = NewBackground(x, y, ref width, ref height);
                Transform transform = root.transform;
                float num = height / 5f * 2f + height / 20f;
                Panels = new List<GameObject>();
                GameObject panel = CreatePanel(ref transform, "BirthPanel");
                Panels.Add(panel);
                GameObject panel2 = CreatePanel(ref transform, "SexPanel");
                panel2.SetActive(value: false);
                Panels.Add(panel2);
                GameObject panel3 = CreatePanel(ref transform, "NPCPanel");
                panel3.SetActive(value: false);
                Panels.Add(panel3);
                GameObject panel4 = CreatePanel(ref transform, "BrothelPanel");
                panel4.SetActive(value: false);
                Panels.Add(panel4);
                GameObject gameObject = CreateButton("生育配置");
                gameObject.transform.SetParent(transform);
                gameObject.GetComponent<RectTransform>().localPosition = new Vector2(-375f, num);
                Action action = delegate
                {
                    ChangePanel(0);
                };
                gameObject.GetComponent<Button>().onClick.AddListener(action);
                CreateBirthPanel(ref panel);
                GameObject gameObject2 = CreateButton("性奴配置");
                gameObject2.transform.SetParent(transform);
                gameObject2.GetComponent<RectTransform>().localPosition = new Vector2(-175f, num);
                Action action2 = delegate
                {
                    ChangePanel(1);
                };
                gameObject2.GetComponent<Button>().onClick.AddListener(action2);
                CreateSexSlavePanel(ref panel2);
                GameObject gameObject3 = CreateButton("交互配置");
                gameObject3.transform.SetParent(transform);
                gameObject3.GetComponent<RectTransform>().localPosition = new Vector2(25f, num);
                Action action3 = delegate
                {
                    ChangePanel(2);
                };
                gameObject3.GetComponent<Button>().onClick.AddListener(action3);
                CreateNpcPanel(ref panel3);
                GameObject gameObject4 = CreateButton("青楼配置");
                gameObject4.transform.SetParent(transform);
                gameObject4.GetComponent<RectTransform>().localPosition = new Vector2(225f, num);
                Action action4 = delegate
                {
                    ChangePanel(3);
                };
                gameObject4.GetComponent<Button>().onClick.AddListener(action4);
                CreateBrothelPanel(ref panel4);
                GameObject gameObject5 = CreateUI.NewImage(SpriteTool.GetSprite("Common", "tuichu"));
                gameObject5.transform.SetParent(root.transform, worldPositionStays: false);
                gameObject5.GetComponent<RectTransform>().anchoredPosition = new Vector2(width / 2f, height / 2f);
                Action action5 = delegate
                {
                    Close();
                };
                gameObject5.AddComponent<Button>().onClick.AddListener(action5);
            }
        }

        public static void CreateSliderItem(this GameObject father, string name, string nameUI, float posX, float posY, int value, int min = 0, int max = 100, string label = null, Action<int> onValueChange = null, string skyTip = null)
        {
            if (GameSlider == null)
            {
                GetSlider();
            }
            GameObject gameObject5 = CreateUI.NewTextSliderInput(nameUI, GameSlider, skyTip);
            gameObject5.transform.SetParent(father.transform);
            gameObject5.GetComponent<RectTransform>().localPosition = new Vector2(posX, posY);
            GameObject gameObject4 = null;
            if (label != null)
            {
                gameObject4 = CreateUI.NewText(value + label, new Vector2(100f, 50f));
                gameObject4.transform.SetParent(gameObject5.transform);
                gameObject4.GetComponent<RectTransform>().localPosition = new Vector2(0f, 0f);
                gameObject4.GetComponent<RectTransform>().anchoredPosition = new Vector2(400f, 0f);
                gameObject4.GetComponent<Text>().color = Color.black;
                gameObject4.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            }
            Slider componentInChildren = gameObject5.GetComponentInChildren<Slider>();
            if (!(componentInChildren != null))
            {
                return;
            }
            componentInChildren.minValue = min;
            componentInChildren.maxValue = max;
            componentInChildren.value = value;
            componentInChildren.onValueChanged.AddListener((Action<float>)delegate (float v)
            {
                onValueChange?.Invoke((int)v);
                if (gameObject4 != null)
                {
                    gameObject4.GetComponent<Text>().text = (int)v + label;
                }
            });
        }

        public static void CreateToggleItem(this GameObject father, string name, string nameUI, float posX, float posY, bool value, Action<bool> onValueChange = null, string skyTip = null)
        {
            GameObject gameObject = CreateUI.NewToggle2(nameUI, skyTip);
            gameObject.transform.SetParent(father.transform);
            gameObject.GetComponent<RectTransform>().localPosition = new Vector2(posX, posY);
            Toggle component = gameObject.transform.GetChild(0).GetComponent<Toggle>();
            component.isOn = value;
            component.onValueChanged.AddListener(onValueChange);
        }

        public static void Close(bool save = true)
        {
            if (save)
            {
                try
                {
                    Config.Instance.Save();
                    UITipItem.AddTip("保存配置成功", 3f);
                }
                catch (Exception value)
                {
                    Console.WriteLine(value);
                    UITipItem.AddTip("保存配置失败", 3f);
                }
            }
            if (root != null)
            {
                UnityEngine.Object.Destroy(root);
            }
            _isOpen = false;
        }

        private static void CreateBirthPanel(ref GameObject panel)
        {
            BirthSettings settings = Config.Instance.BirthSettings;
            float posX = (float)((0.0 - (double)width) / 2.0 + (double)width / 10.0);
            float num = (float)((double)height / 5.0 * 2.0 - (double)height / 15.0);
            panel.CreateSliderItem("PregnancyDuration", "怀孕时长", posX, num, settings.PregnancyDuration, 1, 12, "月", delegate (int v)
            {
                settings.PregnancyDuration = v;
            });
            panel.CreateSliderItem("PregnancyPercent", "怀孕机率", posX, num - height / 15f, settings.PregnancyPercent, 0, 100, "%", delegate (int v)
            {
                settings.PregnancyPercent = v;
            }, "玩家和NPC双修都受此项控制，为0普通双修不会怀孕\n玩家内射必怀，不受此项控制");
            panel.CreateToggleItem("Abortion", "是否流产", posX, num - height / 15f * 2f, settings.Abortion, delegate (bool v)
            {
                settings.Abortion = v;
            }, "怀孕第二个月、第三个月可能流产\n如果孕期设置过短，导致流产期和早产期重叠，则只会早产");
            panel.CreateSliderItem("AbortionPercent", "流产机率", posX, num - height / 15f * 3f, settings.AbortionPercent, 0, 100, "%", delegate (int v)
            {
                settings.AbortionPercent = v;
            });
            panel.CreateToggleItem("PrematureBirth", "是否早产", posX, num - height / 15f * 4f, settings.PrematureBirth, delegate (bool v)
            {
                settings.PrematureBirth = v;
            }, "怀孕最后三个月可能早产\n怀孕第一个月不可能早产");
            panel.CreateSliderItem("PrematureBirthPercent", "早产机率", posX, num - height / 15f * 5f, settings.PrematureBirthPercent, 0, 100, "%", delegate (int v)
            {
                settings.PrematureBirthPercent = v;
            });
            panel.CreateToggleItem("ChildMustBeWomen", "玩家必生女儿", posX, num - height / 15f * 6f, settings.ChildMustBeWomen, delegate (bool v)
            {
                settings.ChildMustBeWomen = v;
            }, "选中后必生女儿，不受新生儿男女比例控制");
            panel.CreateSliderItem("ChildIsMalePercent", "新生儿男性比例", posX, num - height / 15f * 7f, settings.ChildIsMalePercent, 0, 100, "%", delegate (int v)
            {
                settings.ChildIsMalePercent = v;
            }, "玩家子女关闭【玩家必生女儿】选项后才会生效\nNPC始终受此项控制");
            panel.CreateToggleItem("NewbornGrowUp16", "新生儿直接16岁", posX, num - height / 15f * 8f, settings.NewbornGrowUp16, delegate (bool v)
            {
                settings.NewbornGrowUp16 = v;
            });
            panel.CreateSliderItem("NewbornGrowUp16", "子女长相继承比例", posX, num - height / 15f * 9f, settings.FaceInheritPercent, 0, 100, "%", delegate (int v)
            {
                settings.FaceInheritPercent = v;
            });
        }

        private static void CreateSexSlavePanel(ref GameObject panel)
        {
            SexSlaveSettings settings = Config.Instance.SexSlaveSettings;
            float posX = (float)((0.0 - (double)width) / 2.0 + (double)width / 10.0);
            float num = (float)((double)height / 5.0 * 2.0 - (double)height / 15.0);
            panel.CreateToggleItem("SexSlaveInherit", "性奴生的女儿也是性奴", posX, num, settings.SexSlaveInherit, delegate (bool v)
            {
                settings.SexSlaveInherit = v;
            }, "不管先天性奴还是后来收服的性奴，生的女儿都会是先天性奴");
            panel.CreateToggleItem("SexSlaveChildMarryFather", "性奴生的女儿夫君为玩家", posX, num - height / 15f, settings.SexSlaveChildMarryFather, delegate (bool v)
            {
                settings.SexSlaveChildMarryFather = v;
            }, "性奴生的女儿将自动嫁给玩家");
        }

        private static void CreateNpcPanel(ref GameObject panel)
        {
            NpcSettings settings = Config.Instance.NpcSettings;
            float posX = (float)((0.0 - (double)width) / 2.0 + (double)width / 10.0);
            float num = (float)((double)height / 5.0 * 2.0 - (double)height / 15.0);
            panel.CreateToggleItem("Enable", "开启NPC更多交互", posX, num, settings.Enable, delegate (bool v)
            {
                settings.Enable = v;
            }, "开启后男NPC可能强奸战败女仙");
            panel.CreateToggleItem("NpcCannotFuckPlayersFriend", "NPC不会强奸玩家亲友", posX, num - height / 15f, settings.NpcCannotFuckPlayersFriend, delegate (bool v)
            {
                settings.NpcCannotFuckPlayersFriend = v;
            }, "关闭后NPC将可能强奸玩家的性奴和亲友");
            panel.CreateToggleItem("NpcCannotFuckFemalePlayer", "NPC不会强奸女玩家", posX, num - height / 15f * 2f, settings.NpcCannotFuckFemalePlayer, delegate (bool v)
            {
                settings.NpcCannotFuckFemalePlayer = v;
            }, "开启后女玩家不会被NPC强奸");
            panel.CreateSliderItem("RapePercent", "NPC战胜后可强上时强上机率", posX, num - height / 15f * 3f, settings.RapePercent, 0, 100, "%", delegate (int v)
            {
                settings.RapePercent = v;
            }, "NPC在满足强奸条件后，触发强奸的机率");
            panel.CreateToggleItem("NpcCannotFuckFemalePlayer", "NPC不会拐卖女玩家", posX, num - height / 15f * 4f, settings.NpcCannotSellFemalePlayer, delegate (bool v)
            {
                settings.NpcCannotSellFemalePlayer = v;
            }, "开启后女玩家不会被NPC绑架卖到青楼");
            panel.CreateSliderItem("SellPercent", "NPC战胜后可拐卖时拐卖机率", posX, num - height / 15f * 5f, settings.SellPercent, 0, 100, "%", delegate (int v)
            {
                settings.SellPercent = v;
            }, "NPC在满足拐卖条件后，触发拐卖的机率");
        }

        private static void CreateBrothelPanel(ref GameObject panel)
        {
            BrothelSettings settings = Config.Instance.BrothelSettings;
            float posX = (float)((0.0 - (double)width) / 2.0 + (double)width / 10.0);
            float num = (float)((double)height / 5.0 * 2.0 - (double)height / 15.0);
            panel.CreateToggleItem("NpcCanRedemptionSelf", "NPC可以自赎出青楼", posX, num, settings.NpcCanRedemptionSelf, delegate (bool v)
            {
                settings.NpcCanRedemptionSelf = v;
            }, "当NPC灵石足够，堕落较低，或者灵石很多时，可能会自赎出青楼");
            panel.CreateSliderItem("NpcCannotFuckPlayersFriend", "被卖到青楼最少工作几个月", posX, num - height / 15f, settings.CanRedemptionMonth, 0, 100, "个月后可以赎身", delegate (int v)
            {
                settings.CanRedemptionMonth = v;
            });
            panel.CreateToggleItem("FemalePlayerAutoFuck", "过月时女档玩家自动接客", posX, num - height / 15f * 2f, settings.FemalePlayerAutoFuck, delegate (bool v)
            {
                settings.FemalePlayerAutoFuck = v;
            }, "只有在还有接客次数时才可以触发");
        }

        public static GameObject NewBackground(float x, float y, ref float width, ref float height)
        {
            GameObject gameObject = CreateUI.NewCanvas();
            Transform transform = gameObject.transform;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.position.x.ToString();
            component.position.y.ToString();
            Transform transform2 = CreateUI.NewImage().transform;
            transform2.SetParent(transform, worldPositionStays: false);
            transform2.GetComponent<RectTransform>().sizeDelta = new Vector2(10000f, 10000f);
            transform2.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
            Transform transform3 = CreateUI.NewImage(SpriteTool.GetSpriteBigTex("BG/huodebg")).transform;
            transform3.SetParent(transform, worldPositionStays: false);
            RectTransform component2 = transform3.GetComponent<RectTransform>();
            float num = (float)((double)x / 10.0 * 8.0);
            component2.localPosition = new Vector2(0f, 0f);
            component2.position.x.ToString();
            component2.position.y.ToString();
            component2.sizeDelta = new Vector2(num, y);
            component2.anchoredPosition = new Vector2(num / 96f, (float)((0.0 - (double)y) / 20.0));
            GameObject gameObject2 = CreateUI.NewText("鬼畜八荒", new Vector2(300f, 50f));
            Text component3 = gameObject2.GetComponent<Text>();
            component3.fontSize = 30;
            component3.alignment = TextAnchor.MiddleCenter;
            gameObject2.transform.SetParent(transform);
            component3.color = Color.black;
            gameObject2.GetComponent<RectTransform>().localPosition = new Vector2(0f, (float)((double)y * 2.0 / 5.0));
            width = num - num / 5f;
            height = y - y / 5f;
            return gameObject;
        }

        public static GameObject CreateButton(string name)
        {
            GameObject gameObject = CreateUI.NewButton(name);
            RectTransform component = gameObject.GetComponent<RectTransform>();
            Text component2 = gameObject.transform.GetChild(0).GetComponent<Text>();
            component.sizeDelta = new Vector2(150f, 50f);
            component2.fontSize = 20;
            component.pivot = new Vector2(0f, 1f);
            return gameObject;
        }

        public static void ChangePanel(int value)
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                if (i == value)
                {
                    Panels[i].SetActive(value: true);
                }
                else
                {
                    Panels[i].SetActive(value: false);
                }
            }
        }

        public static GameObject CreatePanel(ref Transform root, string name)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.SetParent(root);
            gameObject.AddComponent<RectTransform>().localPosition = new Vector2(150f, 0f);
            return gameObject;
        }
    }
}
