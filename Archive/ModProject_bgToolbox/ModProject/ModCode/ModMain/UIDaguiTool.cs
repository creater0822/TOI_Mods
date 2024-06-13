using System;
using System.Collections.Generic;
using MOD_bgToolbox.Item;
using Newtonsoft.Json;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox
{
    public class UIDaguiTool : MonoBehaviour
    {
        public Transform leftRoot;
        public Transform rightRoot;
        public GameObject leftItem;
        public GameObject leftItemFind;
        public GameObject rightItem;
        public GameObject rightTitleItem;
        public GameObject rightButtonItem;
        public GameObject rightInputItem;
        public Transform recordRoot;
        public Transform cmdListRoot;
        public Button btnCmd;
        public Button btnNew;
        public Button btnSave;
        public Button btnZT;
        public Button btnExitCmd;
        public Button btnClearRecord;
        public GameObject itemRecord;
        public GameObject itemCmd;
        public InputField inputCmdName;
        public static CmdItem tmpCmdItem;
        public int openCmdSet;
        public static List<UIBase> allDaguiUI = new List<UIBase>();

        public UIDaguiTool(System.IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            leftRoot = base.transform.Find("Root/Left/View/Root");
            rightRoot = base.transform.Find("Root/Right/View/Root");
            leftItem = base.transform.Find("LeftItem").gameObject;
            leftItemFind = base.transform.Find("LeftItemFind").gameObject;
            rightButtonItem = base.transform.Find("RightButtonItem").gameObject;
            rightTitleItem = base.transform.Find("RightTitleItem").gameObject;
            rightInputItem = base.transform.Find("RightInputItem").gameObject;
            rightItem = base.transform.Find("RightItem").gameObject;
            recordRoot = base.transform.Find("Root/Record/View/Root");
            cmdListRoot = base.transform.Find("Root/CmdList/View/Root");
            itemRecord = base.transform.Find("ItemRecord").gameObject;
            itemCmd = base.transform.Find("ItemCmd").gameObject;
            btnCmd = base.transform.Find("Root/BtnCmd").GetComponent<Button>();
            btnNew = base.transform.Find("Root/BtnNew").GetComponent<Button>();
            btnSave = base.transform.Find("Root/BtnSave").GetComponent<Button>();
            btnZT = base.transform.Find("Root/BtnZT").GetComponent<Button>();
            inputCmdName = base.transform.Find("Root/CmdList/InpitCmdName").GetComponent<InputField>();
            btnExitCmd = base.transform.Find("Root/CmdList/BtnExit").GetComponent<Button>();
            btnClearRecord = base.transform.Find("Root/Record/BtnClear").GetComponent<Button>();
            btnClearRecord.onClick.AddListener((System.Action)delegate
            {
                UnityAPIEx.DestroyChild(recordRoot);
                ModMain.cmdRun.logs.Clear();
            });
            btnCmd.onClick.AddListener((System.Action)delegate
            {
                ModMain.OpenUI<UIDaguiToolCmd>("DaguiToolCmd");
            });
            btnNew.onClick.AddListener((System.Action)delegate
            {
                openCmdSet = 1;
                tmpCmdItem = new CmdItem();
                OpenCmdList();
            });
            btnSave.onClick.AddListener((System.Action)delegate
            {
                tmpCmdItem.name = inputCmdName.text;
                ModMain.allCmdItems.Add(tmpCmdItem);
                ModMain.SaveCmdItems();
                openCmdSet = 0;
                while (cmdListRoot.childCount > 0)
                {
                    UnityEngine.Object.DestroyImmediate(cmdListRoot.GetChild(0).gameObject);
                }
                tmpCmdItem = new CmdItem();
                inputCmdName.text = "";
                OpenRecord();
            });
            btnZT.onClick.AddListener((System.Action)delegate
            {
                openCmdSet = 1;
                try
                {
                    CmdItem[] array = JsonConvert.DeserializeObject<CmdItem[]>(GUIUtility.systemCopyBuffer);
                    if (array != null && array.Length != 0)
                    {
                        tmpCmdItem = array[0];
                        OpenCmdList();
                        UITipItem.AddTip("Import successful!", 0f);
                    }
                    else
                    {
                        UITipItem.AddTip("Import failed!", 0f);
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    UITipItem.AddTip("Import failed!", 0f);
                }
            });
            btnExitCmd.onClick.AddListener((System.Action)delegate
            {
                openCmdSet = 0;
                OpenRecord();
            });
            Text text5 = base.transform.Find("Root/Text5").GetComponent<Text>();
            Button component = text5.GetComponent<Button>();
            System.Action updateTest5 = delegate
            {
                string text6 = ModMain.cmdData.key.ToString();
                if (string.IsNullOrEmpty(text6))
                {
                    text5.text = "Click to set the shortcut key for opening/closing the interface";
                }
                else
                {
                    text5.text = $"use{text6}quickly open or close the interface";
                }
            };
            component.onClick.AddListener((System.Action)delegate
            {
                ModMain.OpenUI<UIDaguiToolSetKey>("DaguiToolSetKey").InitData(delegate (CmdKey key)
                {
                    ModMain.cmdData.key = key;
                    updateTest5();
                    ModMain.SaveCmdItems();
                });
            });
            updateTest5();
            text5.gameObject.AddComponent<UISkyTipEffect>().InitData("Click to modify the shortcut keys");
            base.transform.Find("Root/BtnAllUnit").GetComponent<Button>().onClick.AddListener((System.Action)delegate
            {
                ModMain.OpenUI<UILookChar>("LookChar");
            });
            base.transform.Find("Root/BtnTeaching").GetComponent<Button>().onClick.AddListener((System.Action)delegate
            {
                ModMain.OpenUI<UIDatuiToolTeaching>("DatuiToolTeaching");
            });
        }

        private void OnEnable()
        {
            UpdateUI();
            UIMask.disableFastKey++;
            openCmdSet = PlayerPrefs.GetInt(base.name + "openCmdSet", 0);
            if (openCmdSet == 1)
            {
                if (tmpCmdItem == null)
                {
                    tmpCmdItem = CmdItem.FromString(PlayerPrefs.GetString(base.name + "tmpCmdItem"));
                }
                OpenCmdList();
            }
            else
            {
                OpenRecord();
                UpdateRecord();
            }
        }

        private void OnDisable()
        {
            UIMask.disableFastKey--;
            PlayerPrefs.SetInt(base.name + "openCmdSet", openCmdSet);
            if (openCmdSet == 1)
            {
                PlayerPrefs.SetString(base.name + "tmpCmdItem", tmpCmdItem.ToString());
            }
        }

        public void OpenRecord()
        {
            btnCmd.gameObject.SetActive(value: true);
            btnNew.gameObject.SetActive(value: true);
            btnSave.gameObject.SetActive(value: false);
            btnZT.gameObject.SetActive(value: false);
            base.transform.Find("Root/Record").gameObject.SetActive(value: true);
            base.transform.Find("Root/CmdList").gameObject.SetActive(value: false);
            UpdateListBtn();
        }

        public void OpenCmdList()
        {
            btnCmd.gameObject.SetActive(value: false);
            btnNew.gameObject.SetActive(value: false);
            btnSave.gameObject.SetActive(value: true);
            btnZT.gameObject.SetActive(value: true);
            base.transform.Find("Root/Record").gameObject.SetActive(value: false);
            base.transform.Find("Root/CmdList").gameObject.SetActive(value: true);
            UpdateCmdList();
        }

        public void UpdateRecord()
        {
            UnityAPIEx.DestroyChild(recordRoot);
            foreach (string log in ModMain.cmdRun.logs)
            {
                AddLogItem(log);
            }
        }

        public void UpdateCmdList()
        {
            inputCmdName.text = tmpCmdItem.name;
            while (cmdListRoot.childCount > 0)
            {
                UnityEngine.Object.DestroyImmediate(cmdListRoot.GetChild(0).gameObject);
            }
            for (int i = 0; i < tmpCmdItem.cmds.Count; i++)
            {
                string cmd = tmpCmdItem.cmds[i];
                AddCmdItem(cmd);
            }
            UpdateCmdListButton();
            UpdateListBtn();
        }

        public void AddCmdItem(string cmd)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(itemCmd, cmdListRoot);
            gameObject.GetComponent<Text>().text = cmd;
            GameObject btns = gameObject.transform.Find("btns").gameObject;
            UIEventListener uIEventListener = gameObject.AddComponent<UIEventListener>();
            uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                btns.SetActive(value: true);
            };
            uIEventListener.onMouseExitCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                btns.SetActive(value: false);
            };
            gameObject.SetActive(value: true);
        }

        public void UpdateCmdListButton()
        {
        }

        public static void DelScroll(UIBase ui)
        {
            allDaguiUI.Remove(ui);
            Il2CppArrayBase<ScrollRect> componentsInChildren = ui.GetComponentsInChildren<ScrollRect>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                ScrollRect scrollRect = componentsInChildren[i];
                PlayerPrefs.SetFloat(ui.gameObject.name + i, scrollRect.verticalNormalizedPosition);
            }
        }

        public static void InitScroll(UIBase ui)
        {
            allDaguiUI.Add(ui);
            Il2CppArrayBase<ScrollRect> componentsInChildren = ui.GetComponentsInChildren<ScrollRect>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                ScrollRect scrollRect = componentsInChildren[i];
                scrollRect.horizontal = false;
                scrollRect.movementType = ScrollRect.MovementType.Clamped;
                scrollRect.scrollSensitivity = ModMain.scrollSpeed;
                string key = ui.gameObject.name + i;
                if (PlayerPrefs.HasKey(key))
                {
                    float @float = PlayerPrefs.GetFloat(key);
                    scrollRect.verticalNormalizedPosition = @float;
                }
            }
        }

        private void OnDestroy()
        {
            UIBase component = GetComponent<UIBase>();
            foreach (UIBase item in new List<UIBase>(allDaguiUI))
            {
                if (item != component)
                {
                    g.ui.CloseUI(component);
                }
            }
            allDaguiUI.Clear();
            Il2CppArrayBase<ScrollRect> componentsInChildren = component.GetComponentsInChildren<ScrollRect>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                string key = base.gameObject.name + i;
                ScrollRect scrollRect = componentsInChildren[i];
                PlayerPrefs.SetFloat(key, scrollRect.verticalNormalizedPosition);
            }
        }

        private void Start()
        {
            InitScroll(GetComponent<UIBase>());
            base.transform.Find("Root/btnClose").GetComponent<Button>().onClick.AddListener((System.Action)delegate
            {
                g.ui.CloseUI(GetComponent<UIBase>());
            });
            base.gameObject.AddComponent<UIFastClose>();
        }

        public void InitData()
        {
        }

        public void UpdateListBtn()
        {
            bool flag = openCmdSet == 1;
            Console.WriteLine(flag + " rightRoot " + rightRoot.childCount);
            for (int i = 0; i < rightRoot.childCount; i++)
            {
                Transform child = rightRoot.GetChild(i);
                child.Find("BtnRun").gameObject.SetActive(!flag);
                child.Find("BtnAdd").gameObject.SetActive(flag);
            }
        }

        private void UpdateUI()
        {
            Dictionary<string, List<ConfDaguiToolItem>> items = ModMain.confTool.items;
            UnityAPIEx.DestroyChild(leftRoot);
            List<Button> list = new List<Button>();
            Button curBtn = null;
            int num = 0;
            Button btnLeft;
            foreach (KeyValuePair<string, List<ConfDaguiToolItem>> item in items)
            {
                int index = num;
                string type = item.Key;
                List<ConfDaguiToolItem> allitems = item.Value;
                GameObject gameObject = UnityEngine.Object.Instantiate(leftItem, leftRoot);
                gameObject.gameObject.SetActive(value: true);
                btnLeft = gameObject.GetComponent<Button>();
                list.Add(btnLeft);
                btnLeft.onClick.AddListener((System.Action)delegate
                {
                    Console.WriteLine(index + " " + type + " The number of commands in this category：" + allitems.Count);
                    UpdateRight(allitems);
                    PlayerPrefs.SetInt(base.name + "BtnIndex", index);
                    if (curBtn != null)
                    {
                        curBtn.transform.Find("Mark").gameObject.SetActive(value: false);
                    }
                    curBtn = btnLeft;
                    curBtn.transform.Find("Mark").gameObject.SetActive(value: true);
                });
                gameObject.transform.Find("Text").GetComponent<Text>().text = allitems[0].typeName;
                num++;
            }
            GameObject gameObject2 = UnityEngine.Object.Instantiate(leftItemFind, leftRoot);
            gameObject2.gameObject.SetActive(value: true);
            btnLeft = gameObject2.GetComponent<Button>();
            InputField inputField = gameObject2.transform.Find("InputField").GetComponent<InputField>();
            list.Add(btnLeft);
            System.Action findItems = delegate
            {
                List<ConfDaguiToolItem> list2 = new List<ConfDaguiToolItem>();
                list2.AddRange(ModMain.confTool.allItems.Values);
                FindTool findTool = new FindTool();
                findTool.SetFindStr(inputField.text);
                if (findTool.findStr.Length != 0)
                {
                    list2.RemoveAll((ConfDaguiToolItem v) => !findTool.CheckFind(GameTool.LS(v.funccn)));
                }
                UpdateRight(list2);
            };
            btnLeft.onClick.AddListener((System.Action)delegate
            {
                PlayerPrefs.SetInt(base.name + "BtnIndex", items.Count);
                if (curBtn != null)
                {
                    curBtn.transform.Find("Mark").gameObject.SetActive(value: false);
                }
                curBtn = btnLeft;
                curBtn.transform.Find("Mark").gameObject.SetActive(value: true);
                findItems();
            });
            inputField.text = PlayerPrefs.GetString(base.name + "leftItemFind", "");
            inputField.onValueChanged.AddListener((System.Action<string>)delegate (string v)
            {
                PlayerPrefs.SetString(base.name + "leftItemFind", v);
                btnLeft.onClick.Invoke();
            });
            int @int = PlayerPrefs.GetInt(base.name + "BtnIndex", 0);
            if (@int >= 0 && @int < list.Count)
            {
                list[@int].onClick.Invoke();
            }
        }

        private void UpdateRight(List<ConfDaguiToolItem> items)
        {
            while (rightRoot.childCount > 0)
            {
                UnityEngine.Object.DestroyImmediate(rightRoot.GetChild(0).gameObject);
            }
            foreach (ConfDaguiToolItem item in items)
            {
                try
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate(rightItem, rightRoot);
                    new UIDaguiToolItem().InitData(this, item, gameObject.transform);
                    gameObject.gameObject.SetActive(value: true);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(item.func + ">>" + UIDaguiToolItem.debugItemPara + "\n" + ex.ToString());
                }
            }
            UpdateListBtn();
        }

        public void AddLogItem(string log)
        {
            GameObject obj = UnityEngine.Object.Instantiate(itemRecord, recordRoot);
            obj.GetComponent<Text>().text = log;
            obj.SetActive(value: true);
        }
    }
}
