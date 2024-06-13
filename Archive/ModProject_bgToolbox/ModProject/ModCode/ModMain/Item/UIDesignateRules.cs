using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIDesignateRules : MonoBehaviour
    {
        public Action<string, string> call;
        public List<ConfSchoolSloganItem> selectItem = new List<ConfSchoolSloganItem>();
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

        public ConfSchoolSloganItem[] allItems => g.conf.schoolSlogan._allConfList.ToArray();

        public UIDesignateRules(IntPtr ptr)
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
            btnClose.onClick.AddListener((Action)CloseUI);
            btnOk.onClick.AddListener((Action)OnBtnOk);
            base.gameObject.AddComponent<UIFastClose>();
            goItem.GetComponent<Text>().color = Color.black;
            textTitle.text = "Select door rules";
            leftTitle.text = "Choose door rules below";
            rightTitle.text = "Selected door rules";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            ConfSchoolSloganItem[] array = allItems;
            foreach (ConfSchoolSloganItem confSchoolSloganItem in array)
            {
                if (confSchoolSloganItem.type != 2 || confSchoolSloganItem.effect == "0" || string.IsNullOrWhiteSpace(GameTool.LS(confSchoolSloganItem.desc)) || string.IsNullOrWhiteSpace(GameTool.LS(confSchoolSloganItem.slogan)))
                {
                    continue;
                }
                ConfSchoolSloganItem selectItem = confSchoolSloganItem;
                string text = GameTool.LS(confSchoolSloganItem.slogan) + ":" + GameTool.LS(confSchoolSloganItem.desc);
                GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj.GetComponent<Text>().text = text;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    if (this.selectItem.Count >= 2)
                    {
                        UITipItem.AddTip("Choose up to 2 door rules!", 0f);
                    }
                    else
                    {
                        if (this.selectItem.Contains(selectItem))
                        {
                            UITipItem.AddTip("Choose from different door rules at most!", 0f);
                        }
                        this.selectItem.Add(selectItem);
                        UpdateLeft();
                    }
                });
                obj.SetActive(value: true);
            }
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void UpdateLeft()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            for (int i = 0; i < selectItem.Count; i++)
            {
                int index = i;
                ConfSchoolSloganItem confSchoolSloganItem = selectItem[i];
                string text = GameTool.LS(confSchoolSloganItem.slogan) + ":" + GameTool.LS(confSchoolSloganItem.desc);
                GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
                obj.GetComponent<Text>().text = text;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    selectItem.RemoveAt(index);
                    UpdateLeft();
                });
                obj.SetActive(value: true);
            }
        }

        public void OnBtnOk()
        {
            if (selectItem.Count < 2)
            {
                UITipItem.AddTip("Choose at least 2 door rules!", 0f);
                return;
            }
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            foreach (ConfSchoolSloganItem item in selectItem)
            {
                list.Add(item.id.ToString());
                list2.Add(GameTool.LS(item.slogan));
            }
            call(string.Join(",", list), string.Join(",", list2));
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
