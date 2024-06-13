using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseToSchoolFate : MonoBehaviour
    {
        public System.Action<string, string> call;
        public ConfSchoolFateItem selectItem;
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

        public ConfSchoolFateItem[] allItems => g.conf.schoolFate._allConfList.ToArray();

        public UIChooseToSchoolFate(System.IntPtr ptr)
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
            textTitle.text = "Change destiny against heaven";
            leftTitle.text = "You can choose to change your destiny from the following";
            rightTitle.text = "Chosen to change fate against fate";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            ConfSchoolFateItem[] array = allItems;
            foreach (ConfSchoolFateItem confSchoolFateItem in array)
            {
                ConfSchoolFateItem selectItem = confSchoolFateItem;
                ConfRoleCreateFeatureItem item = g.conf.roleCreateFeature.GetItem(selectItem.fateFeature);
                if (item == null)
                {
                    continue;
                }
                string text = GameTool.LS(item.name);
                if (!string.IsNullOrEmpty(text))
                {
                    text = CommonTool.SetTextColor(text, GameTool.LevelToColor(item.level, 2));
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
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void UpdateLeft()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            ConfRoleCreateFeatureItem item = g.conf.roleCreateFeature.GetItem(selectItem.fateFeature);
            string text = CommonTool.SetTextColor(GameTool.LS(item.name), GameTool.LevelToColor(item.level, 2));
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

        public void OnBtnOk()
        {
            if (selectItem == null)
            {
                UITipItem.AddTip("Please choose to change your destiny first!", 0f);
                return;
            }
            ConfRoleCreateFeatureItem item = g.conf.roleCreateFeature.GetItem(selectItem.fateFeature);
            string arg = CommonTool.SetTextColor(GameTool.LS(item.name), GameTool.LevelToColor(item.level, 2));
            call(selectItem.id.ToString(), arg);
            CloseUI();
        }

        public void AddTip(GameObject go, ConfSchoolFateItem selectItem)
        {
            UIEventListener uIEventListener = go.AddComponent<UIEventListener>();
            uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                string descRichText = UIMartialInfoTool.GetDescRichText(GameTool.LS(g.conf.roleCreateFeature.GetItem(selectItem.fateFeature).tips), new BattleSkillValueData(0, 0)
                {
                    grade = 1,
                    level = 1
                }, 1);
                g.ui.OpenUI<UISkyTip>(UIType.SkyTip).InitData(descRichText, go.transform.position);
            };
            uIEventListener.onMouseExitCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                g.ui.CloseUI(UIType.SkyTip);
            };
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
