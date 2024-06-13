using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseSecretBook : MonoBehaviour
    {
        public System.Action<string, string> call;
        public static DataStruct<string, string, string>[] cache_allItems;
        public DataStruct<string, string, string> selectItem;
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

        public static DataStruct<string, string, string>[] allItems
        {
            get
            {
                if (cache_allItems == null)
                {
                    List<DataStruct<string, string, string>> list = new List<DataStruct<string, string, string>>();
                    Il2CppSystem.Collections.Generic.List<ConfBattleSkillAttackItem>.Enumerator enumerator = g.conf.battleSkillAttack._allConfList.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        ConfBattleSkillAttackItem current = enumerator.Current;
                        if (current.className.Split('|').Contains("1"))
                        {
                            MartialType martialType = ConfBattleSkillAttackItemEx.martialType(current);
                            int num = (int)martialType;
                            list.Add(new DataStruct<string, string, string>(num.ToString() ?? "", current.id.ToString() ?? "", GameTool.LS(current.name) ?? ""));
                        }
                    }
                    Il2CppSystem.Collections.Generic.List<ConfBattleStepBaseItem>.Enumerator enumerator2 = g.conf.battleStepBase._allConfList.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        ConfBattleStepBaseItem current2 = enumerator2.Current;
                        if (current2.className.Split('|').Contains("1"))
                        {
                            list.Add(new DataStruct<string, string, string>(3.ToString() ?? "", current2.id.ToString() ?? "", GameTool.LS(current2.name) ?? ""));
                        }
                    }
                    Il2CppSystem.Collections.Generic.List<ConfBattleAbilityBaseItem>.Enumerator enumerator3 = g.conf.battleAbilityBase._allConfList.GetEnumerator();
                    while (enumerator3.MoveNext())
                    {
                        ConfBattleAbilityBaseItem current3 = enumerator3.Current;
                        if (current3.className.Contains(1))
                        {
                            string text = GameTool.LS(current3.name) ?? "";
                            if (current3.className.Contains(101))
                            {
                                text += "（Blade）";
                            }
                            else if (current3.className.Contains(102))
                            {
                                text += "（Spear）";
                            }
                            else if (current3.className.Contains(103))
                            {
                                text += "（Sword）";
                            }
                            else if (current3.className.Contains(104))
                            {
                                text += "（Fist）";
                            }
                            else if (current3.className.Contains(105))
                            {
                                text += "（Palm）";
                            }
                            else if (current3.className.Contains(106))
                            {
                                text += "（Finger）";
                            }
                            else if (current3.className.Contains(111))
                            {
                                text += "（Fire）";
                            }
                            else if (current3.className.Contains(112))
                            {
                                text += "（Water）";
                            }
                            else if (current3.className.Contains(113))
                            {
                                text += "（Lightning）";
                            }
                            else if (current3.className.Contains(114))
                            {
                                text += "（Wind）";
                            }
                            else if (current3.className.Contains(115))
                            {
                                text += "（Earth）";
                            }
                            else if (current3.className.Contains(116))
                            {
                                text += "（Wood）";
                            }
                            list.Add(new DataStruct<string, string, string>(4.ToString() ?? "", current3.id.ToString() ?? "", text ?? ""));
                        }
                    }
                    cache_allItems = list.ToArray();
                }
                return cache_allItems;
            }
        }

        public UIChooseSecretBook(System.IntPtr ptr)
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
            textTitle.text = "Select cheats";
            leftTitle.text = "Choose cheats below..";
            rightTitle.text = "Selected cheats";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            for (int i = 0; i < allItems.Length; i++)
            {
                DataStruct<string, string, string> selectItem = allItems[i];
                string t = selectItem.t3;
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
            GameObject obj = UnityEngine.Object.Instantiate(typeItem, typeRoot);
            obj.GetComponentInChildren<Text>().text = "All cheats";
            obj.SetActive(value: true);
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void UpdateLeft()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            string t = selectItem.t3;
            GameObject gameObject = UnityEngine.Object.Instantiate(goItem, leftRoot);
            gameObject.GetComponent<Text>().text = t;
            gameObject.AddComponent<Button>().onClick.AddListener((System.Action)delegate
            {
                UnityAPIEx.DestroyChild(leftRoot);
                selectItem = null;
            });
            gameObject.SetActive(value: true);
            AddTip(gameObject, selectItem);
        }

        public void AddTip(GameObject go, DataStruct<string, string, string> selectItem)
        {
            try
            {
                UIEventListener uIEventListener = go.AddComponent<UIEventListener>();
                uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
                {
                    int type = int.Parse(selectItem.t1);
                    int num = int.Parse(selectItem.t2);
                    int[] array = new int[2]
                    {
                    num,
                    g.world.playerUnit.data.dynUnitData.GetGrade()
                    };
                    DataProps.PropsData propsData = DataProps.PropsData.NewMartial((MartialType)type, array);
                    g.ui.OpenUI<UIMartialPropPreview>(UIType.MartialPropPreview).InitData(g.world.playerUnit, propsData.To<DataProps.MartialData>(), go.transform.position);
                };
                uIEventListener.onMouseExitCall += (Il2CppSystem.Action)(System.Action)delegate
                {
                    g.ui.CloseUI(UIType.MartialPropPreview);
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void OnBtnOk()
        {
            if (selectItem == null)
            {
                UITipItem.AddTip("Please select a cheat first!", 0f);
                return;
            }
            call(selectItem.t1 + "," + selectItem.t2, selectItem.t3);
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
