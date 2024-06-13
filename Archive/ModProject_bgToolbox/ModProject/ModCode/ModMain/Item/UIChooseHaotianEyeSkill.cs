using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseHaotianEyeSkill : MonoBehaviour
    {
        public Action<string, string> call;
        public ConfGodEyeSkillsItem selectItem;
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

        public List<int> allItems
        {
            get
            {
                List<int> list = new List<int>();
                Il2CppSystem.Collections.Generic.List<int>.Enumerator enumerator = g.conf.godEyeSkills.allBossID.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    int current = enumerator.Current;
                    Il2CppSystem.Collections.Generic.List<int> skillsID = g.conf.godEyeSkills.GetSkillsID(current);
                    list.AddRange(skillsID.ToArray());
                }
                return list;
            }
        }

        public UIChooseHaotianEyeSkill(IntPtr ptr)
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
            textTitle.text = "Choose Haotian Eye skill";
            leftTitle.text = "Choose Haotian Eye skills below..";
            rightTitle.text = "Selected Haotian Eye skill";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            foreach (int allItem in allItems)
            {
                ConfGodEyeSkillsItem selectItem;
                string text = GameTool.LS((selectItem = g.conf.godEyeSkills.GetItem(allItem)).name);
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
                UITipItem.AddTip("Please select the Haotian Eye skill first!", 0f);
                return;
            }
            call(selectItem.id.ToString(), GameTool.LS(selectItem.name));
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
