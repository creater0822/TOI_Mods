using System;
using MOD_bgToolbox.Patch;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UILookChar : MonoBehaviour
    {
        public Action<string, string> call;
        public Transform rightRoot;
        public GameObject goItem;
        public Text textTitle;
        public Button btnOk;
        public Button btnClose;
        public Text textPage;
        public Button btnLastPage;
        public Button btnNextPage;
        public int pageIndex;
        public int pageShowCount = 36;
        public int pageMax;

        public UILookChar(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            rightRoot = base.transform.Find("Root/Right/View/Root");
            goItem = base.transform.Find("Item").gameObject;
            textTitle = base.transform.Find("Root/Text1").GetComponent<Text>();
            btnClose = base.transform.Find("Root/btnClose").GetComponent<Button>();
            btnOk = base.transform.Find("Root/BtnOk").GetComponent<Button>();
            btnClose.onClick.AddListener((Action)CloseUI);
            btnOk.gameObject.SetActive(value: false);
            base.gameObject.AddComponent<UIFastClose>();
            goItem.GetComponent<Text>().color = Color.black;
            textTitle.text = "View all roles";
            textPage = base.transform.Find("Root/Text4").GetComponent<Text>();
            btnLastPage = base.transform.Find("Root/BtnL").GetComponent<Button>();
            btnNextPage = base.transform.Find("Root/BtnN").GetComponent<Button>();
            UISelectChar.inputFind = base.transform.Find("Root/InputField").GetComponent<InputField>();
            btnLastPage.onClick.AddListener((Action)delegate
            {
                if (pageMax > 1)
                {
                    pageIndex--;
                    if (pageIndex < 0)
                    {
                        pageIndex = pageMax - 1;
                    }
                    UpdateUI();
                }
            });
            btnNextPage.onClick.AddListener((Action)delegate
            {
                if (pageMax > 1)
                {
                    pageIndex++;
                    if (pageIndex >= pageMax)
                    {
                        pageIndex = 0;
                    }
                    UpdateUI();
                }
            });
            UISelectChar.finxStr = PlayerPrefs.GetString("SelectOneCharfindStr", "");
            UISelectChar.lastFinxStr = UISelectChar.finxStr;
            UISelectChar.inputFind.text = UISelectChar.finxStr;
            pageIndex = PlayerPrefs.GetInt("SelectOneCharpageIndex", 0);
            base.transform.Find("Root/BtnUpdate").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                UISelectChar.findItems = null;
                UISelectChar.allItems = null;
                UISelectChar.Init(reRead: true);
                UpdateUI();
            });
            string tglKey = Patch_UINpcInfo.openKey;
            Toggle component = base.transform.Find("Root/TglOpen").GetComponent<Toggle>();
            component.isOn = PlayerPrefs.GetInt(tglKey, 0) == 1;
            component.onValueChanged.AddListener((Action<bool>)delegate (bool v)
            {
                PlayerPrefs.SetInt(tglKey, v ? 1 : 0);
            });
            component.gameObject.SetActive(value: false);
            UINpcSelectClass.InitSel(base.transform, delegate
            {
                UISelectChar.UpdateSelect();
                UpdateUI();
            });
        }

        private void Update()
        {
            try
            {
                if (UISelectChar.finxStr != UISelectChar.inputFind.text && UISelectChar.lastFinxStr != UISelectChar.inputFind.text)
                {
                    CancelInvoke();
                    UISelectChar.finxStr = UISelectChar.inputFind.text;
                    Invoke("DelayUpdateUI", 0.5f);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(base.name + " Update " + ex.ToString());
            }
        }

        private void DelayUpdateUI()
        {
            if (UISelectChar.lastFinxStr != UISelectChar.inputFind.text && UISelectChar.finxStr == UISelectChar.inputFind.text)
            {
                UISelectChar.UpdateFind();
                UpdateUI();
            }
        }

        public void InitData()
        {
            UISelectChar.Init(reRead: false);
            UpdateUI();
        }

        private void UpdateUI()
        {
            DataStruct<string, string>[] selItems = UISelectChar.selItems;
            UnityAPIEx.DestroyChild(rightRoot);
            pageMax = Mathf.CeilToInt((float)selItems.Length * 1f / (float)pageShowCount);
            if (pageIndex >= pageMax)
            {
                pageIndex = 0;
            }
            for (int i = pageIndex * pageShowCount; i < (pageIndex + 1) * pageShowCount; i++)
            {
                if (i >= selItems.Length)
                {
                    break;
                }
                DataStruct<string, string> selectItem = selItems[i];
                string text = GameTool.LS(selectItem.t2);
                GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj.GetComponent<Text>().text = text;
                obj.transform.Find("BtnLook").GetComponent<Button>().onClick.AddListener((Action)delegate
                {
                    WorldUnitBase unit2 = ModMain.cmdRun.GetUnit(selectItem.t1);
                    if (unit2 != null)
                    {
                        g.ui.OpenUI<UINPCInfo>(UIType.NPCInfo).InitData(unit2, isOption: true);
                    }
                });
                obj.transform.Find("BtnFace").GetComponent<Button>().onClick.AddListener((Action)delegate
                {
                    WorldUnitBase unit = ModMain.cmdRun.GetUnit(selectItem.t1);
                    if (unit != null)
                    {
                        new CreateFace().InitData(unit);
                    }
                });
                obj.SetActive(value: true);
                obj.AddComponent<UISkyTipEffect>().InitData(Tool.UnitTip(selectItem.t1));
            }
            textPage.text = $"{pageIndex + 1}/{pageMax}";
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        private void Start()
        {
            UIDaguiTool.InitScroll(GetComponent<UIBase>());
            InitData();
        }

        private void OnDestroy()
        {
            UIDaguiTool.DelScroll(GetComponent<UIBase>());
            PlayerPrefs.SetString("SelectOneCharfindStr", UISelectChar.finxStr);
            PlayerPrefs.SetInt("SelectOneCharpageIndex", pageIndex);
            PlayerPrefs.SetInt("SelectOneCharselTgl", UINpcSelectClass.isOpenSel ? 1 : 0);
        }
    }
}
