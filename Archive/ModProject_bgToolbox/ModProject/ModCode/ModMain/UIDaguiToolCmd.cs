using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox
{
    public class UIDaguiToolCmd : MonoBehaviour
    {
        public Transform leftRoot;
        public Transform rightRoot;
        public GameObject leftItem;
        public GameObject rightItem;
        public InputField inputName;
        public Button btnSetKey;
        public CmdItem showCmdItem;
        public int selectIndex;
        private DataStruct<Text, Text> selectLeft;

        public UIDaguiToolCmd(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            selectIndex = PlayerPrefs.GetInt(base.name + "curIndex", 0);
            leftRoot = base.transform.Find("Root/Left/View/Root");
            rightRoot = base.transform.Find("Root/Right/View/Root");
            leftItem = base.transform.Find("LeftItem").gameObject;
            rightItem = base.transform.Find("RightItem").gameObject;
            inputName = base.transform.Find("Root/Right/InputName").GetComponent<InputField>();
            btnSetKey = base.transform.Find("Root/Right/BtnKey").GetComponent<Button>();
            base.transform.Find("Root/Right/BtnCopy").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                if (showCmdItem != null)
                {
                    GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(new CmdItem[1] { showCmdItem });
                    UITipItem.AddTip("Copy successfully!", 0f);
                }
                else
                {
                    UITipItem.AddTip("Copy failed!", 0f);
                }
            });
            btnSetKey.onClick.AddListener((Action)delegate
            {
                ModMain.OpenUI<UIDaguiToolSetKey>("DaguiToolSetKey").InitData(delegate (CmdKey key)
                {
                    selectLeft.t2.text = key.ToString();
                    showCmdItem.key = key;
                    SetKeyTip();
                    ModMain.SaveCmdItems();
                });
            });
            base.transform.Find("Root/BtnCopyAll").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                try
                {
                    GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(ModMain.allCmdItems.ToArray());
                    UITipItem.AddTip("Copy successfully!", 0f);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.ToString());
                    UITipItem.AddTip("Copy failed!", 0f);
                }
            });
            base.transform.Find("Root/BtnZTAll").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                try
                {
                    CmdItem[] collection = JsonConvert.DeserializeObject<CmdItem[]>(GUIUtility.systemCopyBuffer);
                    ModMain.allCmdItems.AddRange(collection);
                    UpdateUI();
                    UITipItem.AddTip("Import successful!", 0f);
                    ModMain.SaveCmdItems();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    UITipItem.AddTip("Import failed!", 0f);
                }
            });
        }

        private void SetKeyTip()
        {
            string text = showCmdItem.key.ToString();
            if (string.IsNullOrEmpty(text))
            {
                text = "Click to set shortcut keys";
            }
            btnSetKey.GetComponent<Text>().text = "Current shortcut key：" + text;
        }

        private void Start()
        {
            UIDaguiTool.InitScroll(GetComponent<UIBase>());
            base.transform.Find("Root/btnClose").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                g.ui.CloseUI(GetComponent<UIBase>());
            });
            base.gameObject.AddComponent<UIFastClose>();
            UpdateUI();
        }

        private void UpdateUI()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            List<CmdItem> list = ModMain.allCmdItems;
            for (int i = 0; i < list.Count; i++)
            {
                CmdItem cmd = list[i];
                GameObject go = UnityEngine.Object.Instantiate(leftItem, leftRoot);
                Text component = go.transform.Find("Name").GetComponent<Text>();
                Text component2 = go.transform.Find("Key").GetComponent<Text>();
                component.text = cmd.name;
                component2.text = cmd.key.ToString();
                Button component3 = go.transform.Find("btnDel").GetComponent<Button>();
                Button component4 = go.transform.Find("btnRun").GetComponent<Button>();
                component3.onClick.AddListener((Action)delegate
                {
                    list.Remove(cmd);
                    UnityEngine.Object.DestroyImmediate(go);
                    ModMain.SaveCmdItems();
                });
                component4.onClick.AddListener((Action)delegate
                {
                    cmd.Run();
                });
                DataStruct<Text, Text> left = new DataStruct<Text, Text>(component, component2);
                go.GetComponent<Button>().onClick.AddListener((Action)delegate
                {
                    selectLeft = left;
                    selectIndex = go.transform.GetSiblingIndex();
                    showCmdItem = cmd;
                    UpdateCmd();
                });
                go.SetActive(value: true);
            }
            if (leftRoot.childCount > 0)
            {
                if (selectIndex < 0 || selectIndex >= leftRoot.childCount)
                {
                    selectIndex = 0;
                }
                leftRoot.GetChild(selectIndex).GetComponent<Button>().onClick.Invoke();
            }
        }

        public void UpdateCmd()
        {
            inputName.onValueChanged.RemoveAllListeners();
            inputName.text = showCmdItem.name;
            inputName.onValueChanged.AddListener((Action<string>)delegate (string v)
            {
                selectLeft.t1.text = v;
                showCmdItem.name = v;
                ModMain.SaveCmdItems();
            });
            SetKeyTip();
            UnityAPIEx.DestroyChild(rightRoot);
            foreach (string cmd in showCmdItem.cmds)
            {
                GameObject obj = UnityEngine.Object.Instantiate(rightItem, rightRoot);
                obj.GetComponent<Text>().text = cmd;
                obj.SetActive(value: true);
            }
        }

        private void OnDestroy()
        {
            UIDaguiTool.DelScroll(GetComponent<UIBase>());
            PlayerPrefs.SetInt(base.name + "curIndex", selectIndex);
        }
    }
}
