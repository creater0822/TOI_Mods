using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseRealm : MonoBehaviour
    {
        public Action<string, string> call;

        public static DataStruct<string, string>[] allAttr = new DataStruct<string, string>[30]
        {
        new DataStruct<string, string>("0", "Qi Refining Early"),
        new DataStruct<string, string>("1", "Qi Refining Middle"),
        new DataStruct<string, string>("2", "Qi Refining Late"),
        new DataStruct<string, string>("3", "Foundation Early"),
        new DataStruct<string, string>("4", "Foundation Middle"),
        new DataStruct<string, string>("5", "Foundation Late"),
        new DataStruct<string, string>("6", "Qi Condensation Early"),
        new DataStruct<string, string>("7", "Qi Condensation Middle"),
        new DataStruct<string, string>("8", "Qi Condensation Late"),
        new DataStruct<string, string>("9", "Golden Core Early"),
        new DataStruct<string, string>("10", "Golden Core Middle"),
        new DataStruct<string, string>("11", "Golden Core Late"),
        new DataStruct<string, string>("12", "Origin Spriit Early"),
        new DataStruct<string, string>("13", "Origin Spriit Middle"),
        new DataStruct<string, string>("14", "Origin Spriit Late"),
        new DataStruct<string, string>("15", "Nascent Soul Early"),
        new DataStruct<string, string>("16", "Nascent Soul Middle"),
        new DataStruct<string, string>("17", "Nascent Soul Late"),
        new DataStruct<string, string>("18", "Soul Formation Early"),
        new DataStruct<string, string>("19", "Soul Formation Middle"),
        new DataStruct<string, string>("20", "Soul Formation Late"),
        new DataStruct<string, string>("21", "Enlightenment Early"),
        new DataStruct<string, string>("22", "Enlightenment Middle"),
        new DataStruct<string, string>("23", "Enlightenment Late"),
        new DataStruct<string, string>("24", "Reborn Early"),
        new DataStruct<string, string>("25", "Reborn Middle"),
        new DataStruct<string, string>("26", "Reborn Late"),
        new DataStruct<string, string>("27", "Transcendent Early"),
        new DataStruct<string, string>("28", "Transcendent Middle"),
        new DataStruct<string, string>("29", "Transcendent Late")
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

        public UIChooseRealm(IntPtr ptr)
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
            textTitle.text = "Choose realm";
            leftTitle.text = "Choose a realm below..";
            rightTitle.text = "The chosen realm";
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
            string t = selectItem.t2;
            GameObject obj = UnityEngine.Object.Instantiate(goItem, leftRoot);
            obj.GetComponent<Text>().text = t;
            obj.AddComponent<Button>().onClick.AddListener((Action)delegate
            {
                selectItem = null;
                UpdateLeft();
            });
            obj.SetActive(value: true);
        }

        public void OnBtnOk()
        {
            if (selectItem == null)
            {
                UITipItem.AddTip("Please choose a realm!", 0f);
                return;
            }
            call(selectItem.t1.ToString(), selectItem.t2);
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
