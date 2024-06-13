﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseArtifact : MonoBehaviour
    {
        public System.Action<string, string> call;
        public WorldUnitBase unit;
        public DataStruct<string, DataProps.PropsData> selectItem;
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

        public DataStruct<string, DataProps.PropsData>[] allItems
        {
            get
            {
                List<DataStruct<string, DataProps.PropsData>> list = new List<DataStruct<string, DataProps.PropsData>>();
                DataStruct<string, DataProps.PropsData> item = new DataStruct<string, DataProps.PropsData>("last", (ModMain.lastAddArtifact != null) ? ModMain.lastAddArtifact.t2 : null);
                list.Add(item);
                foreach (DataProps.PropsData item2 in new List<DataProps.PropsData>(g.world.playerUnit.data.unitData.propData._allShowProps.ToArray()))
                {
                    if (item2.propsType == DataProps.PropsDataType.Props && item2.propsItem.className == 401)
                    {
                        list.Add(new DataStruct<string, DataProps.PropsData>(item2.soleID, item2));
                    }
                }
                return list.ToArray();
            }
        }

        public UIChooseArtifact(System.IntPtr ptr)
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
            textTitle.text = "Choose magic weapon";
            leftTitle.text = "Choose magic weapons below..";
            rightTitle.text = "The chosen magic weapon";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, WorldUnitBase unit)
        {
            this.unit = unit;
            DataStruct<string, DataProps.PropsData>[] array = allItems;
            foreach (DataStruct<string, DataProps.PropsData> dataStruct in array)
            {
                DataStruct<string, DataProps.PropsData> selectItem = dataStruct;
                string text = "";
                if (selectItem.t1 == "last")
                {
                    text = "The last magic weapon added:";
                }
                if (selectItem.t2 != null)
                {
                    text += GameTool.SetTextReplaceColorKey(GameTool.LS(selectItem.t2.propsItem.name), GameTool.LevelToColorKey(selectItem.t2.propsItem.level), 1);
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

        public void AddTip(GameObject go, DataStruct<string, DataProps.PropsData> data)
        {
            UIEventListener uIEventListener = go.AddComponent<UIEventListener>();
            uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                DataProps.PropsData propsData = ((data == null) ? null : data.t2);
                if (propsData != null)
                {
                    g.ui.OpenUI<UIArtifactInfo>(UIType.ArtifactInfo).InitData(g.world.playerUnit, propsData, go.transform.position, isOperation: false);
                }
            };
            uIEventListener.onMouseExitCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                g.ui.CloseUI(UIType.ArtifactInfo);
            };
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
                text = "The last magic weapon added:";
            }
            if (selectItem.t2 != null)
            {
                text += GameTool.SetTextReplaceColorKey(GameTool.LS(selectItem.t2.propsItem.name), GameTool.LevelToColorKey(selectItem.t2.propsItem.level), 1);
            }
            GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
            obj.GetComponent<Text>().text = text;
            obj.AddComponent<Button>().onClick.AddListener((System.Action)delegate
            {
                UnityAPIEx.DestroyChild(leftRoot);
                selectItem = null;
            });
            obj.SetActive(value: true);
        }

        public void OnBtnOk()
        {
            if (selectItem == null)
            {
                UITipItem.AddTip("Please choose your magic weapon first!", 0f);
                return;
            }
            string arg = "";
            if (selectItem.t1 == "last")
            {
                arg = "The last magic weapon added";
            }
            else if (selectItem.t2 != null)
            {
                arg = GameTool.SetTextReplaceColorKey(GameTool.LS(selectItem.t2.propsItem.name), GameTool.LevelToColorKey(selectItem.t2.propsItem.level), 1);
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
