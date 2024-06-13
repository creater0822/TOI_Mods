using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseToAddEntry : MonoBehaviour
    {
        public Action<string, string> call;
        public List<ConfBattleSkillPrefixValueItem> selectItem = new List<ConfBattleSkillPrefixValueItem>();
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

        public UIChooseToAddEntry(IntPtr ptr)
            : base(ptr)
        {
        }

        public ConfBattleSkillPrefixValueItem[] GetAllItems(MartialType martialType, int skillId)
        {
            return g.conf.battleSkillPrefixValue.GetItems((int)martialType, skillId).ToArray();
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
            textTitle.text = "Select the modified term";
            leftTitle.text = "Select up to 11 items";
            rightTitle.text = "Selected terms";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, MartialType martialType, int skillId)
        {
            ConfBattleSkillPrefixValueItem[] allItems = GetAllItems(martialType, skillId);
            foreach (ConfBattleSkillPrefixValueItem confBattleSkillPrefixValueItem in allItems)
            {
                ConfBattleSkillPrefixValueItem selectItem = confBattleSkillPrefixValueItem;
                string text;
                try
                {
                    text = UIMartialInfoTool.GetDescRichText(GameTool.LS(confBattleSkillPrefixValueItem.desc), new BattleSkillValueData(0, 0)
                    {
                        grade = 1,
                        level = 1
                    }, 2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    text = GameTool.LS(selectItem.desc);
                }
                GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj.GetComponent<TextMeshProUGUI>().text = text;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    if (this.selectItem.Count >= 11)
                    {
                        UITipItem.AddTip("Choose up to 11 entries!", 0f);
                    }
                    else
                    {
                        this.selectItem.Add(selectItem);
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
                ConfBattleSkillPrefixValueItem confBattleSkillPrefixValueItem = selectItem[i];
                string text;
                try
                {
                    text = UIMartialInfoTool.GetDescRichText(GameTool.LS(confBattleSkillPrefixValueItem.desc), new BattleSkillValueData(0, 0)
                    {
                        grade = 1,
                        level = 1
                    }, 2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    text = GameTool.LS(confBattleSkillPrefixValueItem.desc);
                }
                GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
                obj.GetComponent<TextMeshProUGUI>().text = text;
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
                UITipItem.AddTip("Choose at least 1 term!", 0f);
                return;
            }
            List<string> list = new List<string>();
            List<int> list2 = new List<int>();
            foreach (ConfBattleSkillPrefixValueItem item in selectItem)
            {
                list.Add(item.number.ToString());
                list2.Add(item.id);
            }
            call(string.Join(",", list), "chosen" + list2.Count + "entries");
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
