﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseRighteousnessOrEvil : MonoBehaviour
    {
        public Action<string, string> call;
        public Text textTitle;
        public Button btnClose;
        public Button btnOk1;
        public Button btnOk2;

        public UIChooseRighteousnessOrEvil(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            textTitle = base.transform.Find("Root/Text1").GetComponent<Text>();
            btnClose = base.transform.Find("Root/btnClose").GetComponent<Button>();
            btnOk1 = base.transform.Find("Root/BtnOk1").GetComponent<Button>();
            btnOk2 = base.transform.Find("Root/BtnOk2").GetComponent<Button>();
            btnClose.onClick.AddListener((Action)CloseUI);
            btnOk1.onClick.AddListener((Action)delegate
            {
                call?.Invoke("up", "Righteous");
                CloseUI();
            });
            btnOk2.onClick.AddListener((Action)delegate
            {
                call?.Invoke("down", "Demonic");
                CloseUI();
            });
            base.gameObject.AddComponent<UIFastClose>();
            textTitle.text = "Choose allignment";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
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