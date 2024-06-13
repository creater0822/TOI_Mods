using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIInputInt : MonoBehaviour
    {
        public Action<string, string> call;
        public Transform AttrRoot;
        public GameObject attrItem;
        public Text textTitle;
        public Button btnClose;
        public Button btnOk;

        public UIInputInt(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            AttrRoot = base.transform.Find("Root/AttrList");
            attrItem = base.transform.Find("AttrItem").gameObject;
            textTitle = base.transform.Find("Root/Text1").GetComponent<Text>();
            btnClose = base.transform.Find("Root/btnClose").GetComponent<Button>();
            btnOk = base.transform.Find("Root/BtnOk").GetComponent<Button>();
            btnClose.onClick.AddListener((Action)CloseUI);
            btnOk.onClick.AddListener((Action)OnBtnOk);
            base.gameObject.AddComponent<UIFastClose>();
            g.res.Load<GameObject>("UI/Item/CostArrtItem").transform.Find("Root/Item/Text/Text").GetComponent<TextMeshProUGUI>();
            textTitle.text = "input attributes";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, string[] attrName)
        {
            for (int i = 0; i < attrName.Length; i++)
            {
                GameObject obj = UnityEngine.Object.Instantiate(attrItem, AttrRoot);
                obj.transform.Find("Placeholder").GetComponent<Text>().text = attrName[i];
                obj.transform.Find("Name").GetComponent<Text>().text = attrName[i] + ":";
                obj.SetActive(value: true);
            }
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void OnBtnOk()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < AttrRoot.childCount; i++)
            {
                InputField componentInChildren = AttrRoot.GetChild(i).GetComponentInChildren<InputField>();
                list.Add(componentInChildren.text);
            }
            call(string.Join(",", list), list.Count.ToString());
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
