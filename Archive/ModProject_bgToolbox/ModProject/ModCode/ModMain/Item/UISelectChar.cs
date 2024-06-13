using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UISelectChar : MonoBehaviour
    {
        public Action<string, string> call;
        public static DataStruct<string, string>[] allItems;
        public static DataStruct<string, string>[] findItems;
        public static DataStruct<string, string>[] selItems;
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
        public static InputField inputFind;
        public static string lastFinxStr = "";
        public static string finxStr = "";

        public UISelectChar(IntPtr ptr)
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
            finxStr = PlayerPrefs.GetString("SelectOneCharfindStr", "");
            lastFinxStr = finxStr;
            inputFind.text = finxStr;
            pageIndex = PlayerPrefs.GetInt("SelectOneCharpageIndex", 0);
            base.transform.Find("Root/BtnUpdate").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                findItems = null;
                allItems = null;
                Init(reRead: true);
                UpdateUI();
            });
            UINpcSelectClass.InitSel(base.transform, delegate
            {
                UpdateSelect();
                UpdateUI();
            });
        }

        public static void UpdateSelect()
        {
            if (UINpcSelectClass.isOpenSel && !UINpcSelectClass.CheckAll())
            {
                List<DataStruct<string, string>> list = new List<DataStruct<string, string>>();
                DataStruct<string, string>[] array = findItems;
                foreach (DataStruct<string, string> dataStruct in array)
                {
                    WorldUnitBase unit = ModMain.cmdRun.GetUnit(dataStruct.t1);
                    if (unit != null && UINpcSelectClass.CheckUnit(unit))
                    {
                        list.Add(dataStruct);
                    }
                }
                selItems = list.ToArray();
            }
            else
            {
                selItems = findItems;
            }
        }

        public static void Init(bool reRead)
        {
            if (reRead || allItems == null)
            {
                Il2CppSystem.Collections.Generic.Dictionary<string, WorldUnitBase> allUnit = g.world.unit.allUnit;
                int num = allUnit.Count + 5;
                Console.WriteLine("应获取角色 " + num);
                DataStruct<string, string>[] array = new DataStruct<string, string>[num];
                int num2 = 0;
                array[num2++] = new DataStruct<string, string>("player", "player");
                array[num2++] = new DataStruct<string, string>("unitA", "Current plot unit A");
                array[num2++] = new DataStruct<string, string>("unitB", "Current plot unit B");
                array[num2++] = new DataStruct<string, string>("lookUnit", "Unit being viewed");
                array[num2++] = new DataStruct<string, string>("playerWife", "Player's marriage partner");
                Il2CppSystem.Collections.Generic.Dictionary<string, WorldUnitBase>.Enumerator enumerator = allUnit.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Il2CppSystem.Collections.Generic.KeyValuePair<string, WorldUnitBase> current = enumerator.Current;
                    array[num2++] = new DataStruct<string, string>(current.Key, current.value.data.unitData.propertyData.GetName());
                }
                allItems = array.ToArray();
                UpdateFind();
            }
        }

        public static void UpdateFind()
        {
            lastFinxStr = inputFind.text;
            finxStr = inputFind.text;
            FindTool findTool = new FindTool();
            findTool.SetFindStr(inputFind.text);
            if (findTool.findStr.Length == 0)
            {
                findItems = allItems;
            }
            else
            {
                List<DataStruct<string, string>> list = new List<DataStruct<string, string>>(allItems);
                list.RemoveAll((DataStruct<string, string> v) => !findTool.CheckFind(v.t2) && !v.t1.Contains(finxStr));
                findItems = list.ToArray();
            }
            UpdateSelect();
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

        public void InitData(UIDaguiToolItem toolItem, int index)
        {
            Init(reRead: false);
            UpdateUI();
        }

        private void UpdateUI()
        {
            DataStruct<string, string>[] array = selItems;
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
                DataStruct<string, string> selectItem = array[i];
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
            PlayerPrefs.SetString("SelectOneCharfindStr", finxStr);
            PlayerPrefs.SetInt("SelectOneCharpageIndex", pageIndex);
            PlayerPrefs.SetInt("SelectOneCharselTgl", UINpcSelectClass.isOpenSel ? 1 : 0);
        }
    }
}
