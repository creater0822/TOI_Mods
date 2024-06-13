using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseDaoNumber : MonoBehaviour
    {
        public Action<string, List<int>> call;
        public ConfAppellationTitleItem selectItem;
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

        public ConfAppellationTitleItem[] allItems => g.conf.appellationTitle._allConfList.ToArray();

        public UIChooseDaoNumber(IntPtr ptr)
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
            textTitle.text = "Select road number";
            leftTitle.text = "Select track number below..";
            rightTitle.text = "Selected road number";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            ConfAppellationTitleItem[] array = allItems;
            foreach (ConfAppellationTitleItem confAppellationTitleItem in array)
            {
                ConfAppellationTitleItem selectItem = confAppellationTitleItem;
                string text = GameTool.LS(confAppellationTitleItem.name);
                GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj.GetComponent<Text>().text = text;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    this.selectItem = selectItem;
                    UpdateLeft();
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
            string text = GameTool.LS(selectItem.name);
            GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
            obj.GetComponent<Text>().text = text;
            obj.AddComponent<Button>().onClick.AddListener((Action)delegate
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
                UITipItem.AddTip("Please select the road number first!", 0f);
                return;
            }
            call(selectItem.id.ToString(), new List<int> { selectItem.id });
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
