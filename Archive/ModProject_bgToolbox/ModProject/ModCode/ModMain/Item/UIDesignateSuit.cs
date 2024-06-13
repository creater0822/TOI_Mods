using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIDesignateSuit : MonoBehaviour
    {
        public Action<string, string> call;
        public ConfBattleAbilitySuitBaseItem selectItem;
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

        public ConfBattleAbilitySuitBaseItem[] allItems => g.conf.battleAbilitySuitBase._allConfList.ToArray();

        public UIDesignateSuit(IntPtr ptr)
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
            textTitle.text = "Specify a package";
            leftTitle.text = "Choose package below";
            rightTitle.text = "Selected package";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, int abilityId)
        {
            ConfBattleAbilitySuitBaseItem[] array = allItems;
            foreach (ConfBattleAbilitySuitBaseItem confBattleAbilitySuitBaseItem in array)
            {
                if (confBattleAbilitySuitBaseItem.suitMember.Split('|').Contains(abilityId.ToString()))
                {
                    ConfBattleAbilitySuitBaseItem selectItem = confBattleAbilitySuitBaseItem;
                    string text = GameTool.LS(confBattleAbilitySuitBaseItem.suitName);
                    GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                    obj.GetComponent<Text>().text = text;
                    obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                    {
                        this.selectItem = selectItem;
                        UpdateLeft();
                    });
                    obj.SetActive(value: true);
                    obj.AddComponent<UISkyTipEffect>().InitData(UIMartialInfoTool.GetDescRichText(GameTool.LS(selectItem.suitDesc1), new BattleSkillValueData(0, 0)
                    {
                        grade = g.world.playerUnit.data.dynUnitData.GetGrade(),
                        level = 1
                    }, 1));
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
            string text = GameTool.LS(selectItem.suitName);
            GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
            obj.GetComponent<Text>().text = text;
            obj.AddComponent<Button>().onClick.AddListener((Action)delegate
            {
                UnityAPIEx.DestroyChild(leftRoot);
                selectItem = null;
            });
            obj.SetActive(value: true);
            obj.AddComponent<UISkyTipEffect>().InitData(UIMartialInfoTool.GetDescRichText(GameTool.LS(selectItem.suitDesc1), new BattleSkillValueData(0, 0)
            {
                grade = g.world.playerUnit.data.dynUnitData.GetGrade(),
                level = 1
            }, 1));
        }

        public void OnBtnOk()
        {
            if (selectItem == null)
            {
                UITipItem.AddTip("Please choose a package first!", 0f);
                return;
            }
            call(selectItem.id.ToString(), GameTool.LS(selectItem.suitName));
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
