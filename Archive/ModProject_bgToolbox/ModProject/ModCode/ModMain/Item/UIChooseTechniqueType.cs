using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseTechniqueType : MonoBehaviour
    {
        public Action<string, string> call;

        public static DataStruct<string, string>[] allAttr = new DataStruct<string, string>[12]
        {
        new DataStruct<string, string>("basBlade", "Blade"),
        new DataStruct<string, string>("basSpear", "Spear"),
        new DataStruct<string, string>("basSword", "Sword"),
        new DataStruct<string, string>("basFist", "Fist"),
        new DataStruct<string, string>("basPalm", "Palm"),
        new DataStruct<string, string>("basFinger", "Finger"),
        new DataStruct<string, string>("basFire", "Fire"),
        new DataStruct<string, string>("basFroze", "Water"),
        new DataStruct<string, string>("basThunder", "Lightning"),
        new DataStruct<string, string>("basWind", "Wind"),
        new DataStruct<string, string>("basEarth", "Earth"),
        new DataStruct<string, string>("basWood", "Wood")
        };

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

        public UIChooseTechniqueType(IntPtr ptr)
            : base(ptr)
        {
        }

        public static string GetAttrName(string attr)
        {
            DataStruct<string, string>[] array = allAttr;
            foreach (DataStruct<string, string> dataStruct in array)
            {
                if (dataStruct.t1 == attr)
                {
                    return dataStruct.t2;
                }
            }
            return "";
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
            textTitle.text = "Select exercise type";
            leftTitle.text = "Choose exercise type below";
            rightTitle.text = "Selected exercise type";
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            DataStruct<string, string>[] array = allAttr;
            foreach (DataStruct<string, string> dataStruct in array)
            {
                DataStruct<string, string> selectItem = dataStruct;
                _ = dataStruct.t1;
                string text = GameTool.LS(dataStruct.t2);
                GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj.GetComponent<Text>().text = text;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    if (this.selectItem.Contains(selectItem))
                    {
                        UITipItem.AddTip("This property has already been selected!", 0f);
                    }
                    else
                    {
                        this.selectItem.Add(selectItem);
                        UpdateLeft();
                    }
                });
                obj.SetActive(value: true);
            }
            GameObject obj2 = UnityEngine.Object.Instantiate(typeItem, typeRoot);
            obj2.GetComponentInChildren<Text>().text = "All exercise types";
            obj2.SetActive(value: true);
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
                string t = selectItem[i].t2;
                GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
                obj.GetComponent<Text>().text = t;
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
                UITipItem.AddTip("Choose at least 1 exercise type!", 0f);
                return;
            }
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            foreach (DataStruct<string, string> item in selectItem)
            {
                list.Add(item.t1);
                list2.Add(item.t2);
            }
            call(string.Join(",", list), string.Join(",", list2));
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
