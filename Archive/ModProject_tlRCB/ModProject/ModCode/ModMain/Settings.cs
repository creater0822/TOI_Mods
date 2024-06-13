using System;
using System.Text;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB
{
    public class Settings : UIBase
    {
        public static Settings Instance { get; private set; }

        private Text UpdateText { get; set; }

        static Settings()
        {
            ClassInjector.RegisterTypeInIl2Cpp<Settings>();
        }

        public static void Open()
        {
            g.ui.OpenUI<Settings>("Settings");
        }

        public static void Close()
        {
            if (Instance != null)
            {
                Instance.Destroy();
            }
        }

        public Settings(IntPtr ptr)
            : base(ptr)
        {
            Instance = this;
        }

        private void Start()
        {
            try
            {
                base.gameObject.AddComponent<UIFastClose>();
                Transform transform = base.transform.Find("Root");
                transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener((Action)Close);
                Transform buttons = transform.Find("Content/ButtonLayout");
                Transform panels = transform.Find("Content/Scroll View/Viewport/Content");
                for (int i = 0; i < buttons.childCount; i++)
                {
                    int index = i;
                    buttons.GetChild(i).GetComponent<Button>().onClick.AddListener((Action)delegate
                    {
                        for (int j = 0; j < panels.childCount; j++)
                        {
                            panels.GetChild(j).gameObject.SetActive(index == j);
                        }
                        if (index == buttons.childCount - 1)
                        {
                            ModApi.Init();
                            UpdateInfo();
                        }
                    });
                }
                InitBirthPanel(panels.Find("BirthPanel"));
                InitSexSlavePanel(panels.Find("SexSlavePanel"));
                InitNpcPanel(panels.Find("NpcPanel"));
                InitBrothelPanel(panels.Find("BrothelPanel"));
                InitInfoPanel(panels.Find("InfoPanel"));
            }
            catch (Exception value)
            {
                Console.WriteLine(value);
                throw;
            }
        }

        public void Destroy()
        {
            g.ui.CloseUI(new UIType.UITypeBase("Settings", UILayer.UI));
            Instance = null;
            Config.Instance.Save();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void InitBirthPanel(Transform panel)
        {
            BirthSettings settings = Config.Instance.BirthSettings;
            BindSlider(panel.Find("PregnancyDuration/Slider").GetComponent<Slider>(), settings.PregnancyDuration, delegate (int v)
            {
                settings.PregnancyDuration = v;
            }, "月", "默认为9个月");
            BindSlider(panel.Find("PregnancyPercent/Slider").GetComponent<Slider>(), settings.PregnancyPercent, delegate (int v)
            {
                settings.PregnancyPercent = v;
            }, "%", "玩家有必怀选项，不受此项影响");
            panel.Find("Abortion/Toggle").GetComponent<Toggle>().Bind(settings.Abortion, delegate (bool v)
            {
                settings.Abortion = v;
            });
            BindSlider(panel.Find("AbortionPercent/Slider").GetComponent<Slider>(), settings.AbortionPercent, delegate (int v)
            {
                settings.AbortionPercent = v;
            }, "%", "第二和第三个月双修有可能流产");
            panel.Find("PrematureBirth/Toggle").GetComponent<Toggle>().Bind(settings.PrematureBirth, delegate (bool v)
            {
                settings.PrematureBirth = v;
            });
            BindSlider(panel.Find("PrematureBirthPercent/Slider").GetComponent<Slider>(), settings.PrematureBirthPercent, delegate (int v)
            {
                settings.PrematureBirthPercent = v;
            }, "%");
            panel.Find("ChildMustBeWomen/Toggle").GetComponent<Toggle>().Bind(settings.ChildMustBeWomen, delegate (bool v)
            {
                settings.ChildMustBeWomen = v;
            });
            BindSlider(panel.Find("ChildIsMalePercent/Slider").GetComponent<Slider>(), settings.ChildIsMalePercent, delegate (int v)
            {
                settings.ChildIsMalePercent = v;
            }, "%");
            panel.Find("NewbornGrowUp16/Toggle").GetComponent<Toggle>().Bind(settings.NewbornGrowUp16, delegate (bool v)
            {
                settings.NewbornGrowUp16 = v;
            });
            BindSlider(panel.Find("FaceInheritPercent/Slider").GetComponent<Slider>(), settings.FaceInheritPercent, delegate (int v)
            {
                settings.FaceInheritPercent = v;
            }, "%");
            BindSlider(panel.Find("AbortionCount/Slider").GetComponent<Slider>(), settings.AbortionCount, delegate (int v)
            {
                settings.AbortionCount = v;
            }, "次后不孕");
        }

        private void InitSexSlavePanel(Transform panel)
        {
            panel.gameObject.SetActive(value: false);
            SexSlaveSettings settings = Config.Instance.SexSlaveSettings;
            panel.Find("SexSlaveInherit/Toggle").GetComponent<Toggle>().Bind(settings.SexSlaveInherit, delegate (bool v)
            {
                settings.SexSlaveInherit = v;
            });
            panel.Find("SexSlaveChildMarryFather/Toggle").GetComponent<Toggle>().Bind(settings.SexSlaveChildMarryFather, delegate (bool v)
            {
                settings.SexSlaveChildMarryFather = v;
            });
        }

        private void InitNpcPanel(Transform root)
        {
            root.gameObject.SetActive(value: false);
            NpcSettings settings = Config.Instance.NpcSettings;
            root.Find("Enable/Toggle").GetComponent<Toggle>().Bind(settings.Enable, delegate (bool v)
            {
                settings.Enable = v;
            });
            root.Find("NpcCannotFuckPlayersFriend/Toggle").GetComponent<Toggle>().Bind(settings.NpcCannotFuckPlayersFriend, delegate (bool v)
            {
                settings.NpcCannotFuckPlayersFriend = v;
            });
            root.Find("NpcCannotFuckFemalePlayer/Toggle").GetComponent<Toggle>().Bind(settings.NpcCannotFuckFemalePlayer, delegate (bool v)
            {
                settings.NpcCannotFuckFemalePlayer = v;
            });
            BindSlider(root.Find("RapePercent/Slider").GetComponent<Slider>(), settings.RapePercent, delegate (int v)
            {
                settings.RapePercent = v;
            }, "%");
            root.Find("NpcCannotSellFemalePlayer/Toggle").GetComponent<Toggle>().Bind(settings.NpcCannotSellFemalePlayer, delegate (bool v)
            {
                settings.NpcCannotSellFemalePlayer = v;
            });
            BindSlider(root.Find("SellPercent/Slider").GetComponent<Slider>(), settings.SellPercent, delegate (int v)
            {
                settings.SellPercent = v;
            }, "%");
            BindSlider(root.Find("KidnapMinHate/Slider").GetComponent<Slider>(), settings.KidnapMinHate, delegate (int v)
            {
                settings.KidnapMinHate = v;
            }, "仇恨");
        }

        private void InitBrothelPanel(Transform root)
        {
            BrothelSettings settings = Config.Instance.BrothelSettings;
            root.gameObject.SetActive(value: false);
            root.Find("NpcCanRedemptionSelf/Toggle").GetComponent<Toggle>().Bind(settings.NpcCanRedemptionSelf, delegate (bool v)
            {
                settings.NpcCanRedemptionSelf = v;
            });
            BindSlider(root.Find("CanRedemptionMonth/Slider").GetComponent<Slider>(), settings.CanRedemptionMonth, delegate (int v)
            {
                settings.CanRedemptionMonth = v;
            }, "月");
            root.Find("FemalePlayerAutoFuck/Toggle").GetComponent<Toggle>().Bind(settings.FemalePlayerAutoFuck, delegate (bool v)
            {
                settings.FemalePlayerAutoFuck = v;
            });
            BindSlider(root.Find("BitchCountForCity/Slider").GetComponent<Slider>(), settings.BitchCountForCity, delegate (int v)
            {
                settings.BitchCountForCity = v;
            }, "个");
        }

        private void InitInfoPanel(Transform root)
        {
            root.gameObject.SetActive(value: false);
            UpdateText = root.Find("Last").GetComponent<Text>();
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            if (UpdateText != null)
            {
                UpdateText.text = Info();
            }
        }

        private static void BindSlider(Slider slider, int value, Action<int> action, string label, string tip = null)
        {
            Text text = slider.transform.parent.GetChild(2).GetComponent<Text>();
            slider.value = value.InRange((int)slider.minValue, (int)slider.maxValue);
            slider.onValueChanged.AddListener((Action<float>)delegate (float v)
            {
                text.text = $"{v}{label}";
                action((int)v);
            });
            text.text = $"{value}{label}";
            if (!string.IsNullOrWhiteSpace(tip))
            {
                slider.transform.parent.GetChild(0).AddSkyTip(tip);
            }
        }

        private static string Info()
        {
            MOD_tlRCB.ModData lastDat = ModApi.LastDat;
            return new StringBuilder().Append("<b><color=#F26C4F>").AppendLine("<size=18>最近更新</size>").AppendLine("最新版本：" + lastDat.Version)
                .AppendLine("发布日期: " + lastDat.UpdateTime)
                .AppendLine("$下载地址：https://mod.3dmgame.com/mod/189612")
                .AppendLine("</color></b>")
                .Append(false, false, 16, "red", "在更新时，请关闭游戏，然后覆盖原mod文件，然后再打开游戏！！！")
                .AppendLine()
                .ToString();
        }
    }
}
