using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseHeroSkill : MonoBehaviour
    {
        public Action<string, string> call;
        public ConfTaoistHeartSkillGroupNameItem selectItem;
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

        public ConfTaoistHeartSkillGroupNameItem[] allItems => g.conf.taoistHeartSkillGroupName._allConfList.ToArray();

        public UIChooseHeroSkill(IntPtr ptr)
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
            textTitle.text = "Choose a routine";
            leftTitle.text = "Choose routine below..";
            rightTitle.text = "The chosen routine";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            ConfTaoistHeartSkillGroupNameItem selectItem = new ConfTaoistHeartSkillGroupNameItem();
            string text = "default";
            GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
            obj.GetComponent<Text>().text = text;
            obj.AddComponent<Button>().onClick.AddListener((Action)delegate
            {
                this.selectItem = selectItem;
                UpdateLeft();
            });
            obj.SetActive(value: true);
            ConfTaoistHeartSkillGroupNameItem[] array = allItems;
            foreach (ConfTaoistHeartSkillGroupNameItem confTaoistHeartSkillGroupNameItem in array)
            {
                selectItem = confTaoistHeartSkillGroupNameItem;
                string text2 = GameTool.LS(confTaoistHeartSkillGroupNameItem.skillGroupName);
                GameObject obj2 = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj2.GetComponent<Text>().text = text2;
                obj2.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    this.selectItem = selectItem;
                    UpdateLeft();
                });
                obj2.SetActive(value: true);
            }
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void UpdateLeft()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            string text = GameTool.LS(selectItem.skillGroupName);
            if (string.IsNullOrEmpty(text))
            {
                text = "default";
            }
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
                UITipItem.AddTip("Please choose a routine first!", 0f);
                return;
            }
            string text = GameTool.LS(selectItem.skillGroupName);
            if (string.IsNullOrEmpty(text))
            {
                text = "default";
            }
            call(selectItem.id.ToString(), text);
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
