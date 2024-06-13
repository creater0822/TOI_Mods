using System;
using System.Collections.Generic;
using TMPro;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIEditArtifactAttr : MonoBehaviour
    {
        public Action<string, string> call;
        public List<ConfBattleSkillPrefixValueItem> selectItem = new List<ConfBattleSkillPrefixValueItem>();
        public Transform leftRoot;
        public Transform rightRoot;
        public Transform AttrRoot;
        public GameObject goItem;
        public GameObject attrItem;
        public Text textTitle;
        public Text leftTitle;
        public Text rightTitle;
        public Button btnClose;
        public Button btnOk;

        public UIEditArtifactAttr(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            leftRoot = base.transform.Find("Root/Left/View/Root");
            rightRoot = base.transform.Find("Root/Right/View/Root");
            AttrRoot = base.transform.Find("Root/AttrList");
            goItem = base.transform.Find("Item").gameObject;
            attrItem = base.transform.Find("AttrItem").gameObject;
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
            textTitle.text = "Edit artifact entries and attributes";
            leftTitle.text = "Select up to 11 artifact entries below";
            rightTitle.text = "Selected terms";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, DataProps.PropsArtifact propData)
        {
            ConfArtifactShapeItem item = g.conf.artifactShape.GetItem(propData.data.propsID);
            Il2CppSystem.Collections.Generic.List<ConfBattleSkillPrefixValueItem>.Enumerator enumerator = g.conf.battleSkillPrefixValue.items[11][item.skillID].GetEnumerator();
            while (enumerator.MoveNext())
            {
                ConfBattleSkillPrefixValueItem current = enumerator.Current;
                ConfBattleSkillPrefixValueItem selectItem = current;
                string text;
                try
                {
                    text = UIMartialInfoTool.GetDescRichText(GameTool.LS(current.desc), new BattleSkillValueData(0, 0)
                    {
                        grade = 1,
                        level = 1
                    }, 2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    text = GameTool.LS(current.desc);
                }
                GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj.GetComponent<TextMeshProUGUI>().text = text;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    this.selectItem.Add(selectItem);
                    UpdateLeft();
                });
                obj.SetActive(value: true);
            }
            Il2CppStructArray<int> values = propData.data.values;
            string[] array = new string[8] { "realm（1-10）", "Durability", "quality（1-6）", "life ratio", "ATK ratio", "DEF ratio", "Soul power ratio", "SP" };
            for (int i = 0; i < array.Length; i++)
            {
                int index2 = i + 3;
                GameObject obj2 = UnityEngine.Object.Instantiate(attrItem, AttrRoot);
                obj2.transform.Find("Placeholder").GetComponent<Text>().text = array[i];
                obj2.transform.Find("Name").GetComponent<Text>().text = array[i] + ":";
                obj2.GetComponentInChildren<InputField>().text = values[index2].ToString();
                obj2.SetActive(value: true);
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
            List<string> list3 = new List<string>();
            for (int i = 0; i < AttrRoot.childCount; i++)
            {
                InputField componentInChildren = AttrRoot.GetChild(i).GetComponentInChildren<InputField>();
                list3.Add(componentInChildren.text);
            }
            list3.AddRange(list);
            call(string.Join(",", list3), "chosen" + list2.Count + "entries");
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
