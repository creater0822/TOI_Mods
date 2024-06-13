using System;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseLiaoji : MonoBehaviour
    {
        public System.Action<string, string> call;
        public DataStruct<string, ConfWingmanBaseItem> selectItem;
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

        public UIChooseLiaoji(System.IntPtr ptr)
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
            textTitle.text = "Choose Ethereal power";
            leftTitle.text = "Choose Ethereal Power below..";
            rightTitle.text = "The chosen Ethereal power";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            List<ConfWingmanBaseItem>.Enumerator enumerator = g.conf.wingmanBase._allConfList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ConfWingmanBaseItem current = enumerator.Current;
                if (current.id != 1011)
                {
                    DataStruct<string, ConfWingmanBaseItem> selectItem = new DataStruct<string, ConfWingmanBaseItem>(current.id.ToString(), current);
                    string text = GameTool.LS(current.name);
                    GameObject gameObject = UnityEngine.Object.Instantiate(goItem, rightRoot);
                    gameObject.GetComponent<Text>().text = text;
                    gameObject.AddComponent<Button>().onClick.AddListener((System.Action)delegate
                    {
                        this.selectItem = selectItem;
                        UpdateLeft();
                    });
                    gameObject.SetActive(value: true);
                    AddTip(gameObject, selectItem.t2);
                }
            }
        }

        public void AddTip(GameObject go, ConfWingmanBaseItem conf)
        {
            UIEventListener uIEventListener = go.AddComponent<UIEventListener>();
            uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                string tip = UIMartialInfoTool.GetDescRichText(GameTool.LS(conf.effectDesc), new BattleSkillValueData(g.world.playerUnit), 1) + "\n\n" + UIMartialInfoTool.GetDescRichText(GameTool.LS(conf.desc), new BattleSkillValueData(g.world.playerUnit), 1);
                g.ui.OpenUI<UISkyTip>(UIType.SkyTip).InitData(tip, go.transform.position);
            };
            uIEventListener.onMouseExitCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                g.ui.CloseUI(UIType.SkyTip);
            };
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void UpdateLeft()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            string text = GameTool.LS(selectItem.t2.name);
            GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
            obj.GetComponent<Text>().text = text;
            obj.AddComponent<Button>().onClick.AddListener((System.Action)delegate
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
                UITipItem.AddTip("Please choose the power of mist first!", 0f);
                return;
            }
            string arg = GameTool.LS(selectItem.t2.name);
            call(selectItem.t1, arg);
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
