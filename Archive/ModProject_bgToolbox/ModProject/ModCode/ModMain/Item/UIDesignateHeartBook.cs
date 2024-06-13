using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIDesignateHeartBook : MonoBehaviour
    {
        public System.Action<string, string> call;
        public WorldUnitBase unit;
        public DataProps.MartialData selectItem;
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

        public DataProps.MartialData[] allItems
        {
            get
            {
                List<DataProps.MartialData> list = new List<DataProps.MartialData>();
                Il2CppSystem.Collections.Generic.List<DataProps.PropsData>.Enumerator enumerator = unit.data.unitData.propData.CloneAllProps().GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DataProps.PropsData current = enumerator.Current;
                    if (current.propsType == DataProps.PropsDataType.Martial)
                    {
                        DataProps.MartialData martialData = current.To<DataProps.MartialData>();
                        if (martialData.martialType == MartialType.Ability)
                        {
                            list.Add(martialData);
                        }
                    }
                }
                return list.ToArray();
            }
        }

        public UIDesignateHeartBook(System.IntPtr ptr)
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
            textTitle.text = "Choose mind manual";
            leftTitle.text = "Choose from secret manuals";
            rightTitle.text = "Selected mental secrets";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, WorldUnitBase unit)
        {
            this.unit = unit;
            DataProps.MartialData[] array = allItems;
            foreach (DataProps.MartialData martialData in array)
            {
                DataProps.MartialData selectItem = martialData;
                string text = GameTool.SetTextReplaceColorKey(selectItem.martialInfo.name, GameTool.LevelToColorKey(selectItem.martialInfo.level), 1);
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
            string text = GameTool.SetTextReplaceColorKey(selectItem.martialInfo.name, GameTool.LevelToColorKey(selectItem.martialInfo.level), 1);
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

        public void AddTip(GameObject go, DataProps.MartialData data)
        {
            UIEventListener uIEventListener = go.AddComponent<UIEventListener>();
            uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                g.ui.OpenUI<UIMartialPropPreview>(UIType.MartialPropPreview).InitData(unit, data, go.transform.position);
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
            call(selectItem.data.soleID, GameTool.SetTextReplaceColorKey(selectItem.martialInfo.name, GameTool.LevelToColorKey(selectItem.martialInfo.level), 1));
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
