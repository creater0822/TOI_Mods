using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIDesignateSecretBook : MonoBehaviour
    {
        public System.Action<string, string> call;
        public WorldUnitBase unit;
        public DataStruct<string, DataProps.MartialData> selectItem;
        public Transform leftRoot;
        public Transform rightRoot;
        public Transform typeRoot;
        public GameObject goItem;
        public GameObject typeItem;
        public Text textTitle;
        public Text leftTitle;
        public Text rightTitle;
        public Button btnClose;
        public Button btnOk;

        public DataStruct<string, DataProps.MartialData>[] allItems
        {
            get
            {
                List<DataStruct<string, DataProps.MartialData>> list = new List<DataStruct<string, DataProps.MartialData>>();
                list.Add(new DataStruct<string, DataProps.MartialData>("last", (ModMain.lastAddMartial != null) ? ModMain.lastAddMartial.t2 : null));
                Il2CppSystem.Collections.Generic.List<DataProps.PropsData>.Enumerator enumerator = unit.data.unitData.propData.CloneAllProps().GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DataProps.PropsData current = enumerator.Current;
                    if (current.propsType == DataProps.PropsDataType.Martial)
                    {
                        DataProps.MartialData martialData = current.To<DataProps.MartialData>();
                        list.Add(new DataStruct<string, DataProps.MartialData>(martialData.data.soleID, martialData));
                    }
                }
                return list.ToArray();
            }
        }

        public UIDesignateSecretBook(System.IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            leftRoot = base.transform.Find("Root/Left/View/Root");
            rightRoot = base.transform.Find("Root/Right/View/Root");
            typeRoot = base.transform.Find("Root/typeRoot");
            goItem = base.transform.Find("Item").gameObject;
            typeItem = base.transform.Find("typeItem").gameObject;
            textTitle = base.transform.Find("Root/Text1").GetComponent<Text>();
            leftTitle = base.transform.Find("Root/Text2").GetComponent<Text>();
            rightTitle = base.transform.Find("Root/Text3").GetComponent<Text>();
            btnClose = base.transform.Find("Root/btnClose").GetComponent<Button>();
            btnOk = base.transform.Find("Root/BtnOk").GetComponent<Button>();
            btnClose.onClick.AddListener((System.Action)CloseUI);
            btnOk.onClick.AddListener((System.Action)OnBtnOk);
            base.gameObject.AddComponent<UIFastClose>();
            goItem.GetComponent<Text>().color = Color.black;
            textTitle.text = "Select cheats";
            leftTitle.text = "You can choose cheats from below";
            rightTitle.text = "Selected cheats";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, WorldUnitBase unit)
        {
            this.unit = unit;
            DataStruct<string, DataProps.MartialData>[] array = allItems;
            foreach (DataStruct<string, DataProps.MartialData> dataStruct in array)
            {
                DataStruct<string, DataProps.MartialData> selectItem = dataStruct;
                string text = "";
                if (selectItem.t1 == "last")
                {
                    text = "Last added cheat:";
                }
                if (selectItem.t2 != null)
                {
                    text += GameTool.SetTextReplaceColorKey(selectItem.t2.martialInfo.name, GameTool.LevelToColorKey(selectItem.t2.martialInfo.level), 1);
                }
                GameObject gameObject = UnityEngine.Object.Instantiate(goItem, rightRoot);
                gameObject.GetComponent<Text>().text = text;
                gameObject.AddComponent<Button>().onClick.AddListener((System.Action)delegate
                {
                    this.selectItem = selectItem;
                    UpdateLeft();
                });
                gameObject.SetActive(value: true);
                AddTip(gameObject, selectItem);
            }
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void UpdateLeft()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            string text = "";
            if (selectItem.t1 == "last")
            {
                text = "Last added cheat:";
            }
            if (selectItem.t2 != null)
            {
                text += GameTool.SetTextReplaceColorKey(selectItem.t2.martialInfo.name, GameTool.LevelToColorKey(selectItem.t2.martialInfo.level), 1);
            }
            GameObject gameObject = UnityEngine.Object.Instantiate(goItem, leftRoot);
            gameObject.GetComponent<Text>().text = text;
            gameObject.AddComponent<Button>().onClick.AddListener((System.Action)delegate
            {
                UnityAPIEx.DestroyChild(leftRoot);
                selectItem = null;
            });
            gameObject.SetActive(value: true);
            AddTip(gameObject, selectItem);
        }

        public void AddTip(GameObject go, DataStruct<string, DataProps.MartialData> data)
        {
            UIEventListener uIEventListener = go.AddComponent<UIEventListener>();
            uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                if (data.t2 != null)
                {
                    g.ui.OpenUI<UIMartialPropPreview>(UIType.MartialPropPreview).InitData(unit, data.t2, go.transform.position);
                }
            };
            uIEventListener.onMouseExitCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                g.ui.CloseUI(UIType.MartialPropPreview);
            };
        }

        public void OnBtnOk()
        {
            if (selectItem == null)
            {
                UITipItem.AddTip("Please select a cheat first!", 0f);
                return;
            }
            string arg = "";
            if (selectItem.t1 == "last")
            {
                arg = "Last added cheats";
            }
            else if (selectItem.t2 != null)
            {
                arg = GameTool.SetTextReplaceColorKey(selectItem.t2.martialInfo.name, GameTool.LevelToColorKey(selectItem.t2.martialInfo.level), 1);
            }
            call(selectItem.t1, arg);
            CloseUI();
        }

        private void Start()
        {
            UIDaguiTool.InitScroll(GetComponent<UIBase>());
        }

        private void OnDestroy()
        {
            UIDaguiTool.DelScroll(GetComponent<UIBase>());
        }
    }
}
