using System;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseSoul : MonoBehaviour
    {
        public System.Action<string, string> call;

        public static DataStruct<string, string>[] allAttr = new DataStruct<string, string>[5]
        {
        new DataStruct<string, string>("1", "Jiaolong"),
        new DataStruct<string, string>("2", "Gouchen"),
        new DataStruct<string, string>("3", "Xuangui"),
        new DataStruct<string, string>("4", "Luwu"),
        new DataStruct<string, string>("5", "Chongming")
        };

        public DataStruct<string, string> selectItem;
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

        public UIChooseSoul(System.IntPtr ptr)
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
            textTitle.text = "Choose soul";
            leftTitle.text = "Choose soul below..";
            rightTitle.text = "The chosen soul";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            DataStruct<string, string>[] array = allAttr;
            foreach (DataStruct<string, string> dataStruct in array)
            {
                DataStruct<string, string> selectItem = dataStruct;
                string t = selectItem.t2;
                GameObject gameObject = UnityEngine.Object.Instantiate(goItem, rightRoot);
                gameObject.GetComponent<Text>().text = t;
                gameObject.AddComponent<Button>().onClick.AddListener((System.Action)delegate
                {
                    this.selectItem = selectItem;
                    UpdateLeft();
                });
                gameObject.SetActive(value: true);
                AddTip(gameObject, selectItem);
            }
        }

        public void AddTip(GameObject go, DataStruct<string, string> data)
        {
            UIEventListener uIEventListener = go.AddComponent<UIEventListener>();
            uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                int num = int.Parse(data.t1);
                ConfElderBaseItem item = g.conf.elderBase.GetItem(num);
                Console.WriteLine(num + " " + (object)item);
                string text = UIMartialInfoTool.GetDescRichText(GameTool.LS(item.desc), new BattleSkillValueData(g.world.playerUnit), 2);
                try
                {
                    List<ConfElderLevelItem> itemInElderID = g.conf.elderLevel.GetItemInElderID(num);
                    itemInElderID.Sort((System.Func<ConfElderLevelItem, ConfElderLevelItem, int>)((ConfElderLevelItem a, ConfElderLevelItem b) => a.level - b.level));
                    for (int i = 0; i < itemInElderID.Count; i++)
                    {
                        text = text + "\n  " + itemInElderID[i].level + "." + UIMartialInfoTool.GetDescRichText(GameTool.LS(itemInElderID[i].desc), new BattleSkillValueData(g.world.playerUnit), 2);
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(text);
                    Console.WriteLine(ex.ToString());
                }
                g.ui.OpenUI<UISkyTip>(UIType.SkyTip).InitData(text, go.transform.position);
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
            string t = selectItem.t2;
            GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
            obj.GetComponent<Text>().text = t;
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
                UITipItem.AddTip("Please choose the soul first!", 0f);
                return;
            }
            string t = selectItem.t2;
            call(selectItem.t1, t);
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
