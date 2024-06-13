using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseLiaojiEffect : MonoBehaviour
    {
        public Action<string, string> call;
        public List<DataStruct<string, string>> selectItem = new List<DataStruct<string, string>>();
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

        public UIChooseLiaojiEffect(IntPtr ptr)
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
            TextMeshProUGUI component = g.res.Load<GameObject>("UI/Item/CostArrtItem").transform.Find("Root/Item/Text/Text").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI component2 = goItem.GetComponent<TextMeshProUGUI>();
            component2.font = component.font;
            component2.spriteAsset = component.spriteAsset;
            textTitle.text = "Select modified ethereal power effects";
            leftTitle.text = "Choose Ethereal power effect below..";
            rightTitle.text = "Selected ethereal power effect";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, int id)
        {
            GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
            obj.GetComponent<TextMeshProUGUI>().text = "<color=red>---It is recommended to choose 4 items below.---</color>";
            obj.SetActive(value: true);
            Il2CppSystem.Collections.Generic.List<ConfWingmanEffectItem>.Enumerator enumerator = g.conf.wingmanEffect._allConfList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ConfWingmanEffectItem current = enumerator.Current;
                ConfWingmanEffectItem selectItem = current;
                if (current.name == "0")
                {
                    continue;
                }
                string tips;
                try
                {
                    tips = GameTool.LS(selectItem.name) + ":" + UIMartialInfoTool.GetDescRichText(GameTool.LS(current.desc), new BattleSkillValueData(0, 0)
                    {
                        grade = 1,
                        level = 1
                    }, 2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    tips = GameTool.LS(selectItem.name) + ":" + GameTool.LS(selectItem.desc);
                }
                GameObject obj2 = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj2.GetComponent<TextMeshProUGUI>().text = tips;
                obj2.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    if (this.selectItem.Count >= 11)
                    {
                        UITipItem.AddTip("Choose up to 11 ethereal power effects!", 0f);
                    }
                    else
                    {
                        this.selectItem.Add(new DataStruct<string, string>("e" + selectItem.id, tips));
                        UpdateLeft();
                    }
                });
                obj2.SetActive(value: true);
            }
            GameObject obj3 = UnityEngine.Object.Instantiate(goItem, rightRoot);
            obj3.GetComponent<TextMeshProUGUI>().text = "<color=red>---It is recommended to choose 4 of the following effects.---</color>";
            obj3.SetActive(value: true);
            Il2CppSystem.Collections.Generic.List<ConfWingmanFixValueItem>.Enumerator enumerator2 = g.conf.wingmanFixValue._allConfList.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                ConfWingmanFixValueItem current2 = enumerator2.Current;
                ConfWingmanFixValueItem selectItem = current2;
                if (current2.name == "0" || current2.wingManID != id)
                {
                    continue;
                }
                string tips;
                try
                {
                    tips = GameTool.LS(selectItem.name) + ":" + UIMartialInfoTool.GetDescRichText(GameTool.LS(current2.desc), new BattleSkillValueData(0, 0)
                    {
                        grade = 1,
                        level = 1
                    }, 2);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.ToString());
                    tips = GameTool.LS(selectItem.name) + ":" + GameTool.LS(selectItem.desc);
                }
                GameObject obj4 = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj4.GetComponent<TextMeshProUGUI>().text = tips;
                obj4.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    if (this.selectItem.Count >= 99)
                    {
                        UITipItem.AddTip("Choose up to 99 ethereal power effects!", 0f);
                    }
                    else
                    {
                        this.selectItem.Add(new DataStruct<string, string>("f" + selectItem.id, tips));
                        UpdateLeft();
                    }
                });
                obj4.SetActive(value: true);
            }
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void UpdateLeft()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            for (int i = 0; i < selectItem.Count; i++)
            {
                int index = i;
                DataStruct<string, string> dataStruct = selectItem[i];
                GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
                obj.GetComponent<TextMeshProUGUI>().text = dataStruct.t2;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    selectItem.RemoveAt(index);
                    UpdateLeft();
                });
                obj.SetActive(value: true);
            }
        }

        public void OnBtnOk()
        {
            if (selectItem.Count < 1)
            {
                UITipItem.AddTip("Choose at least 1 ethereal power effect!", 0f);
                return;
            }
            List<string> list = new List<string>();
            foreach (DataStruct<string, string> item in selectItem)
            {
                list.Add(item.t1.ToString());
            }
            call(string.Join(",", list), "chosen" + list.Count + "An ethereal power effect");
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
