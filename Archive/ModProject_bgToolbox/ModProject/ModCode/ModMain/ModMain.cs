using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using EGameTypeData;
using MOD_bgToolbox.Item;
using Newtonsoft.Json;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox
{
    public class ModMain
    {
        private static HarmonyLib.Harmony harmony;
        public const string MOD_SOLE_ID = "bgToolbox";

        public static ConfDaguiToolBase confTool;
        public static DataStruct<TimeScaleType, DataStruct<float>> changeSpeed;
        public static float gameSpeed = 1f;
        public static WorldUnitBase lastUnit;
        public static int chinaInit = 0;
        public static float scrollSpeed = 100f;
        private static CmdData m_cmdData;
        public static CmdRun cmdRun;

        private Il2CppSystem.Action<ETypeData> onOnInitWorldCall;
        private Il2CppSystem.Action<ETypeData> onOpenUICall;
        public static DataStruct<WorldUnitBase, DataProps.MartialData> lastAddMartial { get; set; }
        public static DataStruct<WorldUnitBase, DataUnit.ActionMartialData> lastStudySkill { get; set; }
        public static DataStruct<WorldUnitBase, DataProps.PropsData> lastAddProp { get; set; }
        public static DataStruct<WorldUnitBase, DataProps.PropsData> lastAddElder { get; set; }
        public static DataStruct<WorldUnitBase, DataProps.PropsData> lastAddRule { get; set; }
        public static DataStruct<WorldUnitBase, DataProps.PropsData> lastAddArtifact { get; set; }
        public static DataStruct<WorldUnitBase, DataUnit.ActionMartialData> lastStudyAbility { get; set; }

        public static string dataPath => g.cache.cachePath + "/DaguiToolCmdData.cache";
        public static string newDataPath => g.cache.cachePath + "/../DaguiToolCmdData.cache";
        public static List<CmdItem> allCmdItems => cmdData.allCmdItems;

        public static CmdData cmdData
        {
            get
            {
                if (m_cmdData == null)
                {
                    System.Action action = delegate
                    {
                        m_cmdData = new CmdData();
                        m_cmdData.key.key = KeyCode.F2.ToString();
                    };
                    try
                    {
                        if (!File.Exists(newDataPath))
                        {
                            if (File.Exists(dataPath))
                            {
                                File.Copy(dataPath, newDataPath, overwrite: true);
                                Console.WriteLine("Copying old data successfully");
                                chinaInit = 1;
                            }
                            else
                            {
                                File.Copy(g.mod.GetModPathRoot("bgToolbox") + "/ModAssets/DaguiToolCmdData.cache", newDataPath, overwrite: true);
                                Console.WriteLine("Copying preset data successfully");
                                chinaInit = 1;
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                    }
                    try
                    {
                        if (File.Exists(newDataPath))
                        {
                            string text = File.ReadAllText(newDataPath);
                            Console.WriteLine("save data\n" + text);
                            m_cmdData = JsonConvert.DeserializeObject<CmdData>(text);
                            if (m_cmdData == null)
                            {
                                action();
                            }
                        }
                        else
                        {
                            action();
                        }
                    }
                    catch (System.Exception)
                    {
                        action();
                    }
                    Console.WriteLine("Initialize the configuration of Dagui Toolbox");
                    if (g.res.Load<TextAsset>("ModConf/DafuiToolBase") != null)
                    {
                        confTool = new ConfDaguiToolBase(g.res.Load<TextAsset>("ModConf/DafuiToolBase").text);
                    }
                }
                return m_cmdData;
            }
        }

        public static void SaveCmdItems()
        {
            string text = JsonConvert.SerializeObject(cmdData);
            Console.WriteLine("save data\n" + text);
            File.WriteAllText(newDataPath, text);
        }

        public void Init()
        {
            try
            {
                if (harmony != null)
                {
                    harmony.UnpatchSelf();
                    harmony = null;
                }
                if (harmony == null)
                {
                    harmony = new HarmonyLib.Harmony("MOD_bgToolbox");
                }
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                onOnInitWorldCall = (System.Action<ETypeData>)OnIntoWorld;
                onOpenUICall = (System.Action<ETypeData>)OnOpenUI;
                g.events.On(EGameType.IntoWorld, onOnInitWorldCall, 0);
                g.events.On(EGameType.OpenUIEnd, onOpenUICall, 0);
                Console.WriteLine("Initializing the Big Ghost Toolbox: ok");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Initializing the Big Ghost Toolbox: f " + ex.ToString());
            }
            try
            {
                string text = "MelonLoader/Managed/";
                if (!File.Exists(text + "NStandard.dll"))
                {
                    File.Copy(g.mod.GetModPathRoot("bgToolbox") + "/ModCode/dll/NStandard.dll", text + "NStandard.dll", overwrite: true);
                    Console.WriteLine("copy NStandard.dll success");
                    chinaInit = 1;
                }
                else
                {
                    Console.WriteLine("existed NStandard.dll");
                }
            }
            catch (System.Exception ex2)
            {
                Console.WriteLine("copy NStandard.dll fail " + ex2.ToString());
                chinaInit = 2;
            }
            if (cmdRun == null)
            {
                GameObject gameObject = new GameObject("DaguiCmdRun");
                ClassInjector.RegisterTypeInIl2Cpp<CmdRun>();
                cmdRun = gameObject.AddComponent<CmdRun>();
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }
        }

        public void Destroy()
        {
        }

        private void OnIntoWorld(ETypeData e)
        {
            Console.WriteLine("Enter the World Initialization Toolbox");
            UISelectChar.findItems = null;
            UISelectChar.allItems = null;
        }

        public static void OnUpdate()
        {
            if (cmdData.key.IsKeyDown())
            {
                UIBase uI = g.ui.GetUI(new UIType.UITypeBase("DaguiTool", UILayer.UI));
                if (uI == null)
                {
                    OpenUI<UIDaguiTool>("DaguiTool").InitData();
                }
                else
                {
                    while (g.ui.GetLayerTopUI(UILayer.UI, 0) != uI)
                    {
                        g.ui.CloseUI(g.ui.GetLayerTopUI(UILayer.UI, 0));
                    }
                    g.ui.CloseUI(uI);
                }
            }
            if (changeSpeed != null && GameTool.timeScales != null && !GameTool.timeScales.Contains(changeSpeed))
            {
                UpdateSpeed();
            }
        }

        private void OnOpenUI(ETypeData e)
        {
            if (e.Cast<OpenUIEnd>().uiType.uiName == UIType.MapMain.uiName)
            {
                MapMainAddBtn();
            }
        }

        private void MapMainAddBtn()
        {
            UIMapMain uI = g.ui.GetUI<UIMapMain>(UIType.MapMain);
            if (uI != null && uI.playerInfo.btnPlayer.transform.parent.Find("BtnOpenDaguiTool") == null)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(g.res.Load<GameObject>("UI/BtnOpenDaguiTool"), uI.playerInfo.btnPlayer.transform.parent);
                gameObject.name = "BtnOpenDaguiTool";
                gameObject.transform.localPosition = new Vector3(40f, 420f);
                gameObject.GetComponent<Button>().onClick.AddListener((System.Action)delegate
                {
                    OpenUI<UIDaguiTool>("DaguiTool").InitData();
                });
                gameObject.gameObject.AddComponent<UISkyTipEffect>().InitData("Big ghost tool box");
            }
        }

        public static T OpenUI<T>(string uiName) where T : MonoBehaviour
        {
            ClassInjector.RegisterTypeInIl2Cpp<T>();
            return g.ui.OpenUI(new UIType.UITypeBase(uiName, UILayer.UI)).gameObject.AddComponent<T>();
        }

        public static void FixGameSpeed(float speed)
        {
            gameSpeed = speed;
            UpdateSpeed();
        }

        private static void UpdateSpeed()
        {
            if (Mathf.Approximately(gameSpeed, 1f))
            {
                changeSpeed = null;
                GameTool.ResetTimeScale();
                return;
            }
            if (changeSpeed != null && GameTool.timeScales.Contains(changeSpeed))
            {
                GameTool.timeScales.Remove(changeSpeed);
            }
            changeSpeed = new DataStruct<TimeScaleType, DataStruct<float>>(TimeScaleType.SlowTime, new DataStruct<float>(gameSpeed));
            GameTool.timeScales.Add(changeSpeed);
            GameTool.SetTimeScale(GameTool.GetMinTimeScale());
        }
    }
}
