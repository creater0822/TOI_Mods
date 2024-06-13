using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIDesignateTrait : MonoBehaviour
    {
        public Action<string, string> call;
        public List<ConfRoleCreateCharacterItem> selectItem1 = new List<ConfRoleCreateCharacterItem>();
        public List<ConfRoleCreateCharacterItem> selectItem2 = new List<ConfRoleCreateCharacterItem>();
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

        public ConfRoleCreateCharacterItem[] allItems => g.conf.roleCreateCharacter._allConfList.ToArray();

        public UIDesignateTrait(IntPtr ptr)
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
            textTitle.text = "Choose character";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            ConfRoleCreateCharacterItem[] array = allItems;
            foreach (ConfRoleCreateCharacterItem confRoleCreateCharacterItem in array)
            {
                ConfRoleCreateCharacterItem selectItem = confRoleCreateCharacterItem;
                string text = GameTool.LS(confRoleCreateCharacterItem.sc5asd_sd34);
                Transform parent = ((confRoleCreateCharacterItem.type == 1) ? leftRoot : rightRoot);
                GameObject obj = UnityEngine.Object.Instantiate(goItem, parent);
                List<ConfRoleCreateCharacterItem> list = ((confRoleCreateCharacterItem.type == 1) ? selectItem1 : selectItem2);
                obj.GetComponentInChildren<Text>().text = text;
                obj.GetComponent<Toggle>().onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        list.Add(selectItem);
                    }
                    else
                    {
                        list.Remove(selectItem);
                    }
                });
                obj.SetActive(value: true);
            }
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void OnBtnOk()
        {
            if (selectItem1.Count != 1)
            {
                UITipItem.AddTip("Please choose 1 inner character! No more, no less!", 0f);
                return;
            }
            if (selectItem2.Count != 2)
            {
                UITipItem.AddTip("Please choose 2 external personalities! No more, no less!", 0f);
                return;
            }
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            foreach (ConfRoleCreateCharacterItem item in selectItem1)
            {
                list.Add(item.id.ToString());
                list2.Add(GameTool.LS(item.sc5asd_sd34));
            }
            foreach (ConfRoleCreateCharacterItem item2 in selectItem2)
            {
                list.Add(item2.id.ToString());
                list2.Add(GameTool.LS(item2.sc5asd_sd34));
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
