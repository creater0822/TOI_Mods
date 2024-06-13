using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIDesignateHall : MonoBehaviour
    {
        public Action<string, string> call;

        public static DataStruct<string, string>[] allAttr = new DataStruct<string, string>[5]
        {
        new DataStruct<string, string>("4", "Construction"),
        new DataStruct<string, string>("1", "Recruitment"),
        new DataStruct<string, string>("5", "Treasure Department"),
        new DataStruct<string, string>("3", "External Affairs"),
        new DataStruct<string, string>("2", "Patrol")
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

        public UIDesignateHall(IntPtr ptr)
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
            textTitle.text = "Select a position";
            leftTitle.text = "You can choose a position from below";
            rightTitle.text = "Selected positions";
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
            GameObject obj2 = UnityEngine.Object.Instantiate(typeItem, typeRoot);
            obj2.GetComponentInChildren<Text>().text = "All positions";
            obj2.SetActive(value: true);
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void UpdateLeft()
        {
            UnityAPIEx.DestroyChild(leftRoot);
            string text = GameTool.LS(selectItem.t2);
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
                UITipItem.AddTip("Please select a position first!", 0f);
                return;
            }
            call(selectItem.t1, selectItem.t2);
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
