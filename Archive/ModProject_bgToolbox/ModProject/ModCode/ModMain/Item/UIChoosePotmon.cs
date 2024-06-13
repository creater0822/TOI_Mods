using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChoosePotmon : MonoBehaviour
    {
        public Action<string, string> call;
        public ConfPotmonBaseItem[] allItems;
        public ConfPotmonBaseItem[] findItems;
        public ConfPotmonBaseItem selectItem;
        public Transform leftRoot;
        public Transform rightRoot;
        public Transform typeRoot;
        public GameObject goItem;
        public GameObject typeItem;
        public Text textTitle;
        public Text leftTitle;
        public Text rightTitle;
        public Button btnOk;
        public Button btnClose;
        public Text textPage;
        public Button btnLastPage;
        public Button btnNextPage;
        public InputField inputFind;
        public string lastFinxStr = "";
        public string finxStr = "";
        public int pageIndex;
        public int pageShowCount = 36;
        public int pageMax;

        public UIChoosePotmon(IntPtr ptr)
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
            textTitle.text = "Choose the Pot Demon";
            leftTitle.text = "Choose pot demon below..";
            rightTitle.text = "The selected pot demon";
            textPage = base.transform.Find("Root/Text4").GetComponent<Text>();
            btnLastPage = base.transform.Find("Root/BtnL").GetComponent<Button>();
            btnNextPage = base.transform.Find("Root/BtnN").GetComponent<Button>();
            inputFind = base.transform.Find("Root/InputField").GetComponent<InputField>();
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
        }

        private void Update()
        {
            try
            {
                if (finxStr != inputFind.text && lastFinxStr != inputFind.text)
                {
                    CancelInvoke();
                    finxStr = inputFind.text;
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
            if (lastFinxStr != inputFind.text && finxStr == inputFind.text)
            {
                UpdateFind();
                UpdateUI();
            }
        }

        public void Init(bool reRead)
        {
            if (reRead || allItems == null)
            {
                List<ConfPotmonBaseItem> list = new List<ConfPotmonBaseItem>(g.conf.potmonBase._allConfList.ToArray());
                allItems = list.ToArray();
            }
        }

        public void UpdateFind()
        {
            lastFinxStr = inputFind.text;
            finxStr = inputFind.text;
            FindTool findTool = new FindTool();
            findTool.SetFindStr(inputFind.text);
            if (findTool.findStr.Length == 0)
            {
                findItems = allItems;
                return;
            }
            List<ConfPotmonBaseItem> list = new List<ConfPotmonBaseItem>(allItems);
            list.RemoveAll((ConfPotmonBaseItem v) => !findTool.CheckFind(GameTool.LS(v.name)));
            findItems = list.ToArray();
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            Init(reRead: false);
            UpdateFind();
            UpdateUI();
        }

        private void UpdateUI()
        {
            ConfPotmonBaseItem[] array = findItems;
            UnityAPIEx.DestroyChild(rightRoot);
            pageMax = Mathf.CeilToInt((float)array.Length * 1f / (float)pageShowCount);
            if (pageIndex >= pageMax)
            {
                pageIndex = 0;
            }
            for (int i = pageIndex * pageShowCount; i < (pageIndex + 1) * pageShowCount; i++)
            {
                if (i >= array.Length)
                {
                    break;
                }
                ConfPotmonBaseItem selectItem = array[i];
                string text = GameTool.LS(selectItem.name);
                GameObject obj = UnityEngine.Object.Instantiate(goItem, rightRoot);
                obj.GetComponent<Text>().text = text;
                obj.AddComponent<Button>().onClick.AddListener((Action)delegate
                {
                    this.selectItem = selectItem;
                    UpdateLeft();
                });
                obj.SetActive(value: true);
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
            string text = GameTool.LS(selectItem.name);
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
                UITipItem.AddTip("Please choose the pot demon first!", 0f);
                return;
            }
            call(selectItem.id.ToString(), GameTool.LS(selectItem.name));
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
