using System;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIDesignateQiFortune : MonoBehaviour
    {
        public Action<string, string> call;
        public WorldUnitBase unit;
        public ConfRoleCreateFeatureItem selectItem;
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

        public ConfRoleCreateFeatureItem[] allItems
        {
            get
            {
                List<ConfRoleCreateFeatureItem> list = new List<ConfRoleCreateFeatureItem>();
                Il2CppReferenceArray<DataUnit.LuckData> bornLuck = unit.data.unitData.propertyData.bornLuck;
                Il2CppSystem.Collections.Generic.List<DataUnit.LuckData> addLuck = unit.data.unitData.propertyData.addLuck;
                foreach (DataUnit.LuckData item2 in bornLuck)
                {
                    ConfRoleCreateFeatureItem item = g.conf.roleCreateFeature.GetItem(item2.id);
                    list.Add(item);
                }
                Il2CppSystem.Collections.Generic.List<DataUnit.LuckData>.Enumerator enumerator2 = addLuck.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    DataUnit.LuckData current2 = enumerator2.Current;
                    ConfRoleCreateFeatureItem featureItem = g.conf.roleCreateFeature.GetItem(current2.id);
                    if (list.Find((ConfRoleCreateFeatureItem v) => v.id == featureItem.id) == null)
                    {
                        list.Add(featureItem);
                    }
                }
                Il2CppSystem.Collections.Generic.Dictionary<int, DataWorld.World.PlayerLogData.GradeData> dictionary = ((!(unit.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID)) ? unit.data.unitData.npcUpGrade : g.data.world.playerLog.upGrade);
                Il2CppSystem.Collections.Generic.Dictionary<int, DataWorld.World.PlayerLogData.GradeData>.Enumerator enumerator3 = dictionary.GetEnumerator();
                while (enumerator3.MoveNext())
                {
                    Il2CppSystem.Collections.Generic.KeyValuePair<int, DataWorld.World.PlayerLogData.GradeData> current3 = enumerator3.Current;
                    ConfRoleCreateFeatureItem featureItem = g.conf.roleCreateFeature.GetItem(current3.Value.luck);
                    if (featureItem != null && list.Find((ConfRoleCreateFeatureItem v) => v.id == featureItem.id) == null)
                    {
                        list.Add(featureItem);
                    }
                }
                return list.ToArray();
            }
        }

        public UIDesignateQiFortune(IntPtr ptr)
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
            textTitle.text = "Choose luck";
            leftTitle.text = "Choose luck below";
            rightTitle.text = "Chosen luck";
        }

        public void InitData(UIDaguiToolItem toolItem, int index, WorldUnitBase unit)
        {
            this.unit = unit;
            ConfRoleCreateFeatureItem[] array = allItems;
            foreach (ConfRoleCreateFeatureItem confRoleCreateFeatureItem in array)
            {
                ConfRoleCreateFeatureItem selectItem = confRoleCreateFeatureItem;
                string text = CommonTool.SetTextColor(GameTool.LS(selectItem.name), GameTool.LevelToColor(selectItem.level, 1)) + "(" + selectItem.id + ")";
                GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj.GetComponent<Text>().text = text;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    this.selectItem = selectItem;
                    UpdateLeft();
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
            string text = CommonTool.SetTextColor(GameTool.LS(selectItem.name), GameTool.LevelToColor(selectItem.level, 1)) + "(" + selectItem.id + ")";
            GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
            obj.GetComponent<Text>().text = text;
            obj.AddComponent<Button>().onClick.AddListener((Action)delegate
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
                UITipItem.AddTip("Please choose luck first!", 0f);
                return;
            }
            call(selectItem.id.ToString(), CommonTool.SetTextColor(GameTool.LS(selectItem.name), GameTool.LevelToColor(selectItem.level, 1)) + "(" + selectItem.id + ")");
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
