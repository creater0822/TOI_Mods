using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseLiaojiEffectUp : MonoBehaviour
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

        public UIChooseLiaojiEffectUp(IntPtr ptr)
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
            textTitle.text = "Select upgraded ethereal power effects";
            leftTitle.text = "Choose Ethereal power effect below..";
            rightTitle.text = "Selected ethereal power effect";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, int id)
        {
            Il2CppSystem.Collections.Generic.List<ConfWingmanFixValueItem>.Enumerator enumerator = g.conf.wingmanFixValue._allConfList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ConfWingmanFixValueItem current = enumerator.Current;
                ConfWingmanFixValueItem selectItem = current;
                if (current.name == "0" || current.wingManID != id)
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
                GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj.GetComponent<TextMeshProUGUI>().text = tips;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    if (this.selectItem.Count >= 99)
                    {
                        UITipItem.AddTip("Choose up to 99 ethereal power effects!", 0f);
                    }
                    else
                    {
                        this.selectItem.Add(new DataStruct<string, string>(selectItem.id.ToString(), tips));
                        UpdateLeft();
                    }
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
