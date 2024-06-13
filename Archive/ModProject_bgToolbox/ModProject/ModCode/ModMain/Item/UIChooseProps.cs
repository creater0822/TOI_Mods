using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIChooseProps : MonoBehaviour
    {
        public System.Action<string, string> call;
        public static Dictionary<string, ConfItemPropsItem[]> cacheFind = new Dictionary<string, ConfItemPropsItem[]>();
        public static ConfItemPropsItem[] allItems;
        public static ConfItemPropsItem[] findItems;
        public static ConfItemPropsItem[] selectItems;
        public ConfItemPropsItem selectItem;
        public Transform leftRoot;
        public Transform rightRoot;
        public Transform typeRoot;
        public Transform typeSubRoot;
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
        public int selectType;
        public List<string> selectSubType = new List<string>();
        public bool isInitData;

        public UIChooseProps(System.IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            leftRoot = base.transform.Find("Root/Left/View/Root");
            rightRoot = base.transform.Find("Root/Right/View/Root");
            typeRoot = base.transform.Find("Root/typeRoot");
            typeSubRoot = base.transform.Find("Root/typeSubRoot");
            goItem = base.transform.Find("Item").gameObject;
            typeItem = base.transform.Find("typeItem").gameObject;
            textTitle = base.transform.Find("Root/Text1").GetComponent<Text>();
            leftTitle = base.transform.Find("Root/Text2").GetComponent<Text>();
            rightTitle = base.transform.Find("Root/Text3").GetComponent<Text>();
            btnClose = base.transform.Find("Root/btnClose").GetComponent<Button>();
            btnOk = base.transform.Find("Root/BtnOk").GetComponent<Button>();
            btnClose.onClick.AddListener((System.Action)CloseUI);
            btnOk.onClick.AddListener((System.Action)OnBtnOk);
            base.gameObject.AddComponent<UIFastClose>();
            goItem.GetComponent<Text>().color = Color.black;
            textTitle.text = "Select props";
            leftTitle.text = "Choose props below..";
            rightTitle.text = "Selected props";
            textPage = base.transform.Find("Root/Text4").GetComponent<Text>();
            btnLastPage = base.transform.Find("Root/BtnL").GetComponent<Button>();
            btnNextPage = base.transform.Find("Root/BtnN").GetComponent<Button>();
            inputFind = base.transform.Find("Root/InputField").GetComponent<InputField>();
            btnLastPage.onClick.AddListener((System.Action)delegate
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
            btnNextPage.onClick.AddListener((System.Action)delegate
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
            finxStr = PlayerPrefs.GetString(base.name + "findStr", "");
            lastFinxStr = finxStr;
            inputFind.text = finxStr;
            pageIndex = PlayerPrefs.GetInt(base.name + "pageIndex", 0);
            base.transform.Find("Root/BtnUpdate").GetComponent<Button>().onClick.AddListener((System.Action)delegate
            {
                findItems = null;
                cacheFind.Clear();
                allItems = null;
                selectItems = null;
                Init(reRead: true);
                UpdateUI();
            });
            selectType = PlayerPrefs.GetInt(base.name + "selectType", 0);
            string @string = PlayerPrefs.GetString(base.name + "selectSubType", "");
            selectSubType = new List<string>();
            if (!string.IsNullOrEmpty(@string))
            {
                selectSubType.AddRange(@string.Split(','));
            }
            InitTypeRoot();
        }

        private void InitTypeRoot()
        {
            int isInit = 0;
            List<int> list = new List<int> { 0 };
            Il2CppSystem.Collections.Generic.List<int>.Enumerator enumerator = g.conf.itemType.types.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int current = enumerator.Current;
                list.Add(current);
            }
            list.Remove(2);
            UnityAPIEx.DestroyChild(typeRoot);
            typeRoot.gameObject.SetActive(value: false);
            ToggleGroup component = typeRoot.GetComponent<ToggleGroup>();
            Toggle toggle = null;
            foreach (int item in list)
            {
                int selType = item;
                GameObject obj = UnityEngine.Object.Instantiate(typeItem, typeRoot);
                obj.GetComponentInChildren<Text>().text = GameTool.LS((item == 0) ? "playerInfo_quanbu" : g.conf.itemType.GetItem(item).name);
                Toggle component2 = obj.transform.Find("Toggle").GetComponent<Toggle>();
                component2.onValueChanged.AddListener((System.Action<bool>)delegate (bool b)
                {
                    if (b && isInit > 0)
                    {
                        selectType = selType;
                        InitSubTypeRoot(isInit == 1);
                    }
                });
                component2.group = component;
                if (item == selectType)
                {
                    toggle = component2;
                }
                obj.SetActive(value: true);
            }
            typeRoot.gameObject.SetActive(value: true);
            isInit = 1;
            if ((bool)toggle)
            {
                toggle.isOn = true;
            }
            isInit = 2;
        }

        private void InitSubTypeRoot(bool isInit)
        {
            UnityAPIEx.DestroyChild(typeSubRoot);
            List<ConfItemTypeItem> list = new List<ConfItemTypeItem>();
            Il2CppSystem.Collections.Generic.List<ConfItemTypeItem>.Enumerator enumerator = g.conf.itemType._allConfList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ConfItemTypeItem current = enumerator.Current;
                ConfItemTypeItem conf = ItemTypeFixedEx.Fixed(current);
                if (conf.sort > 0 && !(conf.uiLabel == "0") && conf.type == selectType && !list.Exists((ConfItemTypeItem w) => w.uiLabel == conf.uiLabel))
                {
                    list.Add(conf);
                }
            }
            List<string> list2 = new List<string>();
            list2.AddRange(list.Select((ConfItemTypeItem w) => w.uiLabel));
            if (!isInit)
            {
                selectSubType = new List<string>();
                selectSubType.AddRange(list2);
            }
            foreach (string item in list2)
            {
                string selSubType = item;
                GameObject obj = UnityEngine.Object.Instantiate(typeItem, typeSubRoot);
                obj.GetComponentInChildren<Text>().text = GameTool.LS(selSubType);
                Toggle component = obj.transform.Find("Toggle").GetComponent<Toggle>();
                component.isOn = selectSubType.Contains(selSubType);
                component.onValueChanged.AddListener((System.Action<bool>)delegate (bool b)
                {
                    if (b)
                    {
                        selectSubType.Add(selSubType);
                    }
                    else
                    {
                        selectSubType.Remove(selSubType);
                    }
                    UpdateSelectItems();
                    if (isInitData)
                    {
                        UpdateUI();
                    }
                });
                obj.SetActive(value: true);
            }
            UpdateSelectItems();
            if (isInitData)
            {
                UpdateUI();
            }
        }

        private void UpdateSelectItems()
        {
            Console.WriteLine(selectType + ":" + string.Join(",", selectSubType));
            if (selectType == 0)
            {
                selectItems = findItems;
                return;
            }
            List<ConfItemPropsItem> list = new List<ConfItemPropsItem>();
            ConfItemPropsItem[] array = findItems;
            foreach (ConfItemPropsItem confItemPropsItem in array)
            {
                int className = confItemPropsItem.className;
                ConfItemTypeItem confItemTypeItem = ItemTypeFixedEx.Fixed(g.conf.itemType.GetItem(className));
                if ((selectType == 0 || selectType == confItemTypeItem.type) && selectSubType.Contains(confItemTypeItem.uiLabel))
                {
                    list.Add(confItemPropsItem);
                }
            }
            selectItems = list.ToArray();
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
            catch (System.Exception ex)
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
                List<ConfItemPropsItem> list = new List<ConfItemPropsItem>(g.conf.itemProps._allConfList.ToArray());
                list.RemoveAll((ConfItemPropsItem v) => string.IsNullOrEmpty(GameTool.LS(v.name)));
                allItems = list.ToArray();
                UpdateFind();
            }
        }

        public void UpdateFind()
        {
            lastFinxStr = inputFind.text;
            finxStr = inputFind.text;
            FindTool findTool = new FindTool();
            findTool.SetFindStr(finxStr);
            if (findTool.findStr.Length == 0)
            {
                findItems = allItems;
            }
            else if (cacheFind.ContainsKey(finxStr))
            {
                findItems = cacheFind[finxStr];
            }
            else
            {
                List<ConfItemPropsItem> list = new List<ConfItemPropsItem>(allItems);
                list.RemoveAll((ConfItemPropsItem v) => !findTool.CheckFind(GameTool.LS(v.name)));
                findItems = list.ToArray();
                cacheFind.Add(finxStr, findItems);
            }
            UpdateSelectItems();
        }

        public void AddTip(GameObject go, ConfItemPropsItem selectItem)
        {
            UIEventListener uIEventListener = go.AddComponent<UIEventListener>();
            uIEventListener.onMouseEnterCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                try
                {
                    DataProps.PropsData propsData = DataProps.PropsData.NewPropsNotData(selectItem.id);
                    if (propsData.propsItem.className == 401)
                    {
                        DataProps.PropsData propsData2 = propsData.Clone();
                        if (propsData2.To<DataProps.PropsArtifact>().grade == 0)
                        {
                            propsData2.To<DataProps.PropsArtifact>().grade = 1;
                        }
                        g.ui.OpenUI<UIArtifactInfo>(UIType.ArtifactInfo).InitData(g.world.playerUnit, propsData2, go.transform.position, isOperation: false);
                    }
                    else
                    {
                        g.ui.OpenUI<UIPropInfo>(UIType.PropInfo).InitData(g.world.playerUnit, propsData, go.transform.position);
                    }
                }
                catch (System.Exception)
                {
                }
            };
            uIEventListener.onMouseExitCall += (Il2CppSystem.Action)(System.Action)delegate
            {
                g.ui.CloseUI(UIType.PropInfo);
                g.ui.CloseUI(UIType.ArtifactInfo);
            };
        }

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            isInitData = true;
            Init(reRead: false);
            UpdateUI();
        }

        private void UpdateUI()
        {
            ConfItemPropsItem[] array = selectItems;
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
                ConfItemPropsItem selectItem = array[i];
                string text = CommonTool.SetTextColor(GameTool.LS(selectItem.name), GameTool.LevelToColor(selectItem.level, 1));
                GameObject gameObject = UnityEngine.Object.Instantiate(goItem, rightRoot);
                gameObject.GetComponent<Text>().text = i + 1 + "." + text;
                gameObject.AddComponent<Button>().onClick.AddListener((System.Action)delegate
                {
                    this.selectItem = selectItem;
                    UpdateLeft();
                });
                gameObject.SetActive(value: true);
                AddTip(gameObject, selectItem);
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
            string text = CommonTool.SetTextColor(GameTool.LS(selectItem.name), GameTool.LevelToColor(selectItem.level, 1));
            GameObject gameObject = UnityEngine.Object.Instantiate(goItem, leftRoot);
            gameObject.GetComponent<Text>().text = text;
            gameObject.AddComponent<Button>().onClick.AddListener((System.Action)delegate
            {
                UnityAPIEx.DestroyChild(leftRoot);
                selectItem = null;
            });
            gameObject.SetActive(value: true);
            AddTip(gameObject, selectItem);
        }

        public void OnBtnOk()
        {
            if (selectItem == null)
            {
                UITipItem.AddTip("Please choose props first!", 0f);
                return;
            }
            call(selectItem.id.ToString(), CommonTool.SetTextColor(GameTool.LS(selectItem.name), GameTool.LevelToColor(selectItem.level, 1)));
            CloseUI();
        }

        private void Start()
        {
            UIDaguiTool.InitScroll(GetComponent<UIBase>());
        }

        private void OnDestroy()
        {
            UIDaguiTool.DelScroll(GetComponent<UIBase>());
            PlayerPrefs.SetString(base.name + "findStr", finxStr);
            PlayerPrefs.SetInt(base.name + "pageIndex", pageIndex);
            PlayerPrefs.SetInt(base.name + "selectType", selectType);
            PlayerPrefs.SetString(base.name + "selectSubType", string.Join(",", selectSubType));
        }
    }
}
