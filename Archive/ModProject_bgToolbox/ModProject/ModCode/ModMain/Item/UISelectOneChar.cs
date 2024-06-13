using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UISelectOneChar : MonoBehaviour
    {
        public Action<string, string> call;
        public DataStruct<string, string> selectItem;
        public Transform leftRoot;
        public Transform rightRoot;
        public GameObject goItem;
        public Text textTitle;
        public Text leftTitle;
        public Text rightTitle;
        public Button btnOk;
        public Button btnClose;
        public Text textPage;
        public Button btnLastPage;
        public Button btnNextPage;
        public int pageIndex;
        public int pageShowCount = 36;
        public int pageMax;

        public UISelectOneChar(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            leftRoot = base.transform.Find("Root/Left/View/Root");
            rightRoot = base.transform.Find("Root/Right/View/Root");
            goItem = base.transform.Find("Item").gameObject;
            textTitle = base.transform.Find("Root/Text1").GetComponent<Text>();
            leftTitle = base.transform.Find("Root/Text2").GetComponent<Text>();
            rightTitle = base.transform.Find("Root/Text3").GetComponent<Text>();
            btnClose = base.transform.Find("Root/btnClose").GetComponent<Button>();
            btnOk = base.transform.Find("Root/BtnOk").GetComponent<Button>();
            btnClose.onClick.AddListener((Action)CloseUI);
            btnOk.onClick.AddListener((Action)OnBtnOk);
            base.gameObject.AddComponent<UIFastClose>();
            goItem.GetComponent<Text>().color = Color.black;
            textTitle.text = "Select role";
            leftTitle.text = "Choose a role below";
            rightTitle.text = "Selected role";
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
                if (Input.GetKeyDown(KeyCode.F5))
                {
                    UISelectChar.Init(reRead: true);
                    UpdateUI();
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

        public void InitData(UIDaguiToolItem toolItem, int index)
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
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    this.selectItem = selectItem;
                    UpdateLeft();
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
            obj.AddComponent<UISkyTipEffect>().InitData(Tool.UnitTip(selectItem.t1));
        }

        public void OnBtnOk()
        {
            if (selectItem == null)
            {
                UITipItem.AddTip("Please select your unit first!", 0f);
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
            PlayerPrefs.SetString("SelectOneCharfindStr", UISelectChar.finxStr);
            PlayerPrefs.SetInt("SelectOneCharpageIndex", pageIndex);
            PlayerPrefs.SetInt("SelectOneCharselTgl", UINpcSelectClass.isOpenSel ? 1 : 0);
        }
    }
}
