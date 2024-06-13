using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseToReverseFate : MonoBehaviour
    {
        public System.Action<string, string> call;
        public ConfFateFeatureItem selectItem;
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

        public ConfFateFeatureItem[] allItems => g.conf.fateFeature._allConfList.ToArray();

        public UIChooseToReverseFate(System.IntPtr ptr)
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
            textTitle.text = "Choose to change fate";
            leftTitle.text = "Change your destiny";
            rightTitle.text = "Chosen to change fate against fate";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            ConfFateFeatureItem[] array = allItems;
            foreach (ConfFateFeatureItem confFateFeatureItem in array)
            {
                ConfFateFeatureItem selectItem = confFateFeatureItem;
                string text = GameTool.LS(g.conf.roleCreateFeature.GetItem(confFateFeatureItem.id).name);
                if (!string.IsNullOrEmpty(text))
                {
                    ConfRoleCreateFeatureItem item = g.conf.roleCreateFeature.GetItem(selectItem.id);
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
            ConfRoleCreateFeatureItem item = g.conf.roleCreateFeature.GetItem(selectItem.id);
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
            ConfRoleCreateFeatureItem item = g.conf.roleCreateFeature.GetItem(selectItem.id);
            string arg = CommonTool.SetTextColor(GameTool.LS(item.name), GameTool.LevelToColor(item.level, 2));
            call(selectItem.id.ToString(), arg);
            CloseUI();
        }

        public void AddTip(GameObject go, ConfFateFeatureItem selectItem)
        {
            UIEventListener uIEventListener = go.AddComponent<UIEventListener>();
            uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                string descRichText = UIMartialInfoTool.GetDescRichText(GameTool.LS(g.conf.roleCreateFeature.GetItem(selectItem.id).tips), new BattleSkillValueData(0, 0)
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
