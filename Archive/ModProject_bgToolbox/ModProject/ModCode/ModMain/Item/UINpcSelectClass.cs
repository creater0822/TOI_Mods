using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UINpcSelectClass : MonoBehaviour
    {
        public class SelectData
        {
            public List<int> selectSex = new List<int>();
            public List<int> selectMeili = new List<int>();
            public List<int> selectShengwang = new List<int>();
            public List<int> selectXingge0 = new List<int>();
            public List<int> selectXingge1 = new List<int>();
            public List<int> selectXingge2 = new List<int>();
            public List<int> selectJingjie = new List<int>();
            public List<int> selectJieduan = new List<int>();
            public List<int> selectQinggan = new List<int>();
            public List<int> selectHero = new List<int>();
            public List<int> selectDaoxin = new List<int>();
            public List<string> selectSchool = new List<string>();

            public bool selectSexAll = true;
            public bool selectMeiliAll = true;
            public bool selectShengwangAll = true;
            public bool selectXingge0All = true;
            public bool selectXingge1All = true;
            public bool selectXingge2All = true;
            public bool selectJingjieAll = true;
            public bool selectJieduanAll = true;
            public bool selectQingganAll = true;
            public bool selectHeroAll = true;
            public bool selectDaoxinAll = true;
            public bool selectSchoolAll = true;
        }

        private static SelectData m_selectData;
        public Transform rightRoot;
        public Button btnOk;
        public Button btnClose;
        public GameObject itemTitle;
        public GameObject itemToggle;
        public Action okCall;
        private SelectData tmpData;
        public static bool isOpenSel;

        public static SelectData selectData
        {
            get
            {
                if (m_selectData == null)
                {
                    bool flag = false;
                    if (PlayerPrefs.HasKey("UINpcSelectClass_m_selectData"))
                    {
                        try
                        {
                            SelectData selectData = JsonConvert.DeserializeObject<SelectData>(PlayerPrefs.GetString("UINpcSelectClass_m_selectData"));
                            if (selectData != null)
                            {
                                m_selectData = selectData;
                                flag = true;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (!flag)
                    {
                        m_selectData = new SelectData();
                    }
                }
                return m_selectData;
            }
            set
            {
                m_selectData = value;
                string value2 = JsonConvert.SerializeObject(m_selectData);
                PlayerPrefs.SetString("UINpcSelectClass_m_selectData", value2);
            }
        }

        public static SelectData copyData => JsonConvert.DeserializeObject<SelectData>(JsonConvert.SerializeObject(selectData));

        public UINpcSelectClass(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            rightRoot = base.transform.Find("Root/Right/View/Root");
            base.gameObject.AddComponent<UIFastClose>();
            btnClose = base.transform.Find("Root/btnClose").GetComponent<Button>();
            btnOk = base.transform.Find("Root/BtnOk").GetComponent<Button>();
            btnClose.onClick.AddListener((Action)CloseUI);
            btnOk.onClick.AddListener((Action)OnBtnOk);
            itemTitle = base.transform.Find("ItemTitle").gameObject;
            itemToggle = base.transform.Find("itemToggle").gameObject;
            base.transform.Find("Root/BtnRest").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                tmpData = new SelectData();
                UpdateUI();
            });
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }

        public void OnBtnOk()
        {
            selectData = tmpData;
            okCall?.Invoke();
            CloseUI();
        }

        private void Start()
        {
            tmpData = copyData;
            UpdateUI();
        }

        private void UpdateUI()
        {
            UnityAPIEx.DestroyChild(rightRoot);
            int num = 8;
            int num2 = -20;
            int[] allSexs = new int[2] { 1, 2 };
            GameObject obj = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj.GetComponent<Text>().text = "gender:";
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj.SetActive(value: true);
            for (int i = 0; i < allSexs.Length; i++)
            {
                if (i > 0 && i % num == 0)
                {
                    num2 -= 40;
                }
                int sex = allSexs[i];
                GameObject obj2 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj2.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + i % num * 180, num2);
                Toggle component = obj2.GetComponent<Toggle>();
                obj2.GetComponentInChildren<Text>().text = ((sex == 1) ? GameTool.LS("common_nan") : GameTool.LS("common_nv"));
                component.isOn = tmpData.selectSex.Contains(sex) || tmpData.selectSexAll;
                component.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectSex.Add(sex);
                        bool flag12 = true;
                        int[] array5 = allSexs;
                        foreach (int item2 in array5)
                        {
                            if (!tmpData.selectSex.Contains(item2))
                            {
                                flag12 = false;
                                break;
                            }
                        }
                        if (flag12)
                        {
                            tmpData.selectSexAll = true;
                            tmpData.selectSex.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectSexAll)
                        {
                            tmpData.selectSexAll = false;
                            tmpData.selectSex.Clear();
                            int[] array5 = allSexs;
                            foreach (int item3 in array5)
                            {
                                tmpData.selectSex.Add(item3);
                            }
                        }
                        tmpData.selectSex.Remove(sex);
                    }
                    Console.WriteLine(isOn + " " + string.Join(",", tmpData.selectSex));
                });
                obj2.SetActive(value: true);
            }
            num2 -= 40;
            Il2CppSystem.Collections.Generic.List<ConfRoleBeautyItem> allMeili = g.conf.roleBeauty._allConfList;
            GameObject obj3 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj3.GetComponent<Text>().text = "charm:";
            obj3.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj3.SetActive(value: true);
            for (int j = 0; j < allMeili.Count; j++)
            {
                if (j > 0 && j % num == 0)
                {
                    num2 -= 40;
                }
                ConfRoleBeautyItem item = allMeili[j];
                GameObject obj4 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj4.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + j % num * 180, num2);
                Toggle component2 = obj4.GetComponent<Toggle>();
                obj4.GetComponentInChildren<Text>().text = GameTool.LS(item.text);
                component2.isOn = tmpData.selectMeili.Contains(item.id) || tmpData.selectMeiliAll;
                component2.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectMeili.Add(item.id);
                        bool flag11 = true;
                        Il2CppSystem.Collections.Generic.List<ConfRoleBeautyItem>.Enumerator enumerator7 = allMeili.GetEnumerator();
                        while (enumerator7.MoveNext())
                        {
                            ConfRoleBeautyItem current13 = enumerator7.Current;
                            if (!tmpData.selectMeili.Contains(current13.id))
                            {
                                flag11 = false;
                                break;
                            }
                        }
                        if (flag11)
                        {
                            tmpData.selectMeiliAll = true;
                            tmpData.selectMeili.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectMeiliAll)
                        {
                            tmpData.selectMeiliAll = false;
                            tmpData.selectMeili.Clear();
                            Il2CppSystem.Collections.Generic.List<ConfRoleBeautyItem>.Enumerator enumerator7 = allMeili.GetEnumerator();
                            while (enumerator7.MoveNext())
                            {
                                ConfRoleBeautyItem current14 = enumerator7.Current;
                                tmpData.selectMeili.Add(current14.id);
                            }
                        }
                        tmpData.selectMeili.Remove(item.id);
                    }
                });
                obj4.SetActive(value: true);
            }
            num2 -= 40;
            Il2CppSystem.Collections.Generic.List<ConfRoleReputationItem> allShengwang = g.conf.roleReputation._allConfList;
            GameObject obj5 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj5.GetComponent<Text>().text = "prestige:";
            obj5.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj5.SetActive(value: true);
            for (int k = 0; k < allShengwang.Count; k++)
            {
                if (k > 0 && k % num == 0)
                {
                    num2 -= 40;
                }
                ConfRoleReputationItem item = allShengwang[k];
                GameObject obj6 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj6.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + k % num * 180, num2);
                Toggle component3 = obj6.GetComponent<Toggle>();
                obj6.GetComponentInChildren<Text>().text = GameTool.LS(item.text);
                component3.isOn = tmpData.selectShengwang.Contains(item.id) || tmpData.selectShengwangAll;
                component3.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectShengwang.Add(item.id);
                        bool flag10 = true;
                        Il2CppSystem.Collections.Generic.List<ConfRoleReputationItem>.Enumerator enumerator6 = allShengwang.GetEnumerator();
                        while (enumerator6.MoveNext())
                        {
                            ConfRoleReputationItem current11 = enumerator6.Current;
                            if (!tmpData.selectShengwang.Contains(current11.id))
                            {
                                flag10 = false;
                                break;
                            }
                        }
                        if (flag10)
                        {
                            tmpData.selectShengwangAll = true;
                            tmpData.selectShengwang.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectShengwangAll)
                        {
                            tmpData.selectShengwangAll = false;
                            tmpData.selectShengwang.Clear();
                            Il2CppSystem.Collections.Generic.List<ConfRoleReputationItem>.Enumerator enumerator6 = allShengwang.GetEnumerator();
                            while (enumerator6.MoveNext())
                            {
                                ConfRoleReputationItem current12 = enumerator6.Current;
                                tmpData.selectShengwang.Add(current12.id);
                            }
                        }
                        tmpData.selectShengwang.Remove(item.id);
                    }
                });
                obj6.SetActive(value: true);
            }
            Il2CppSystem.Collections.Generic.List<ConfRoleCreateCharacterItem> allXingge = g.conf.roleCreateCharacter._allConfList;
            num2 -= 40;
            GameObject obj7 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj7.GetComponent<Text>().text = "Inner character:";
            obj7.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj7.SetActive(value: true);
            int num3 = 0;
            for (int l = 0; l < allXingge.Count; l++)
            {
                ConfRoleCreateCharacterItem item = allXingge[l];
                if (item.type != 1)
                {
                    continue;
                }
                if (num3 > 0 && num3 % num == 0)
                {
                    num2 -= 40;
                }
                GameObject obj8 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj8.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + num3 % num * 180, num2);
                Toggle component4 = obj8.GetComponent<Toggle>();
                obj8.GetComponentInChildren<Text>().text = GameTool.LS(item.sc5asd_sd34);
                component4.isOn = tmpData.selectXingge0.Contains(item.id) || tmpData.selectXingge0All;
                component4.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectXingge0.Add(item.id);
                        bool flag9 = true;
                        Il2CppSystem.Collections.Generic.List<ConfRoleCreateCharacterItem>.Enumerator enumerator5 = allXingge.GetEnumerator();
                        while (enumerator5.MoveNext())
                        {
                            ConfRoleCreateCharacterItem current9 = enumerator5.Current;
                            if (item.type == 1 && !tmpData.selectXingge0.Contains(current9.id))
                            {
                                flag9 = false;
                                break;
                            }
                        }
                        if (flag9)
                        {
                            tmpData.selectXingge0All = true;
                            tmpData.selectXingge0.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectXingge0All)
                        {
                            tmpData.selectXingge0All = false;
                            tmpData.selectXingge0.Clear();
                            Il2CppSystem.Collections.Generic.List<ConfRoleCreateCharacterItem>.Enumerator enumerator5 = allXingge.GetEnumerator();
                            while (enumerator5.MoveNext())
                            {
                                ConfRoleCreateCharacterItem current10 = enumerator5.Current;
                                if (item.type == 1)
                                {
                                    tmpData.selectXingge0.Add(current10.id);
                                }
                            }
                        }
                        tmpData.selectXingge0.Remove(item.id);
                    }
                });
                obj8.SetActive(value: true);
                num3++;
            }
            num2 -= 40;
            GameObject obj9 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj9.GetComponent<Text>().text = "External personality 1:";
            obj9.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj9.SetActive(value: true);
            num3 = 0;
            for (int m = 0; m < allXingge.Count; m++)
            {
                ConfRoleCreateCharacterItem item = allXingge[m];
                if (item.type != 2)
                {
                    continue;
                }
                if (num3 > 0 && num3 % num == 0)
                {
                    num2 -= 40;
                }
                GameObject obj10 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj10.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + num3 % num * 180, num2);
                Toggle component5 = obj10.GetComponent<Toggle>();
                obj10.GetComponentInChildren<Text>().text = GameTool.LS(item.sc5asd_sd34);
                component5.isOn = tmpData.selectXingge1.Contains(item.id) || tmpData.selectXingge1All;
                component5.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectXingge1.Add(item.id);
                        bool flag8 = true;
                        Il2CppSystem.Collections.Generic.List<ConfRoleCreateCharacterItem>.Enumerator enumerator4 = allXingge.GetEnumerator();
                        while (enumerator4.MoveNext())
                        {
                            ConfRoleCreateCharacterItem current7 = enumerator4.Current;
                            if (item.type == 2 && !tmpData.selectXingge1.Contains(current7.id))
                            {
                                flag8 = false;
                                break;
                            }
                        }
                        if (flag8)
                        {
                            tmpData.selectXingge1All = true;
                            tmpData.selectXingge1.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectXingge1All)
                        {
                            tmpData.selectXingge1All = false;
                            tmpData.selectXingge1.Clear();
                            Il2CppSystem.Collections.Generic.List<ConfRoleCreateCharacterItem>.Enumerator enumerator4 = allXingge.GetEnumerator();
                            while (enumerator4.MoveNext())
                            {
                                ConfRoleCreateCharacterItem current8 = enumerator4.Current;
                                if (item.type == 2)
                                {
                                    tmpData.selectXingge1.Add(current8.id);
                                }
                            }
                        }
                        tmpData.selectXingge1.Remove(item.id);
                    }
                });
                obj10.SetActive(value: true);
                num3++;
            }
            num2 -= 40;
            GameObject obj11 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj11.GetComponent<Text>().text = "External personality 2:";
            obj11.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj11.SetActive(value: true);
            num3 = 0;
            for (int n = 0; n < allXingge.Count; n++)
            {
                ConfRoleCreateCharacterItem item = allXingge[n];
                if (item.type != 2)
                {
                    continue;
                }
                if (num3 > 0 && num3 % num == 0)
                {
                    num2 -= 40;
                }
                GameObject obj12 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj12.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + num3 % num * 180, num2);
                Toggle component6 = obj12.GetComponent<Toggle>();
                obj12.GetComponentInChildren<Text>().text = GameTool.LS(item.sc5asd_sd34);
                component6.isOn = tmpData.selectXingge2.Contains(item.id) || tmpData.selectXingge2All;
                component6.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectXingge2.Add(item.id);
                        bool flag7 = true;
                        Il2CppSystem.Collections.Generic.List<ConfRoleCreateCharacterItem>.Enumerator enumerator3 = allXingge.GetEnumerator();
                        while (enumerator3.MoveNext())
                        {
                            ConfRoleCreateCharacterItem current5 = enumerator3.Current;
                            if (item.type == 2 && !tmpData.selectXingge2.Contains(current5.id))
                            {
                                flag7 = false;
                                break;
                            }
                        }
                        if (flag7)
                        {
                            tmpData.selectXingge2All = true;
                            tmpData.selectXingge2.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectXingge2All)
                        {
                            tmpData.selectXingge2All = false;
                            tmpData.selectXingge2.Clear();
                            Il2CppSystem.Collections.Generic.List<ConfRoleCreateCharacterItem>.Enumerator enumerator3 = allXingge.GetEnumerator();
                            while (enumerator3.MoveNext())
                            {
                                ConfRoleCreateCharacterItem current6 = enumerator3.Current;
                                if (item.type == 2)
                                {
                                    tmpData.selectXingge2.Add(current6.id);
                                }
                            }
                        }
                        tmpData.selectXingge2.Remove(item.id);
                    }
                });
                obj12.SetActive(value: true);
                num3++;
            }
            num2 -= 40;
            DataStruct<string, string>[] allAttr = new DataStruct<string, string>[10]
            {
            new DataStruct<string, string>("1", "Qi Refinement"),
            new DataStruct<string, string>("2", "Foundation"),
            new DataStruct<string, string>("3", "Qi Condensation"),
            new DataStruct<string, string>("4", "Golden Core"),
            new DataStruct<string, string>("5", "Origin Spirit"),
            new DataStruct<string, string>("6", "Nascent Soul"),
            new DataStruct<string, string>("7", "Soul Formation"),
            new DataStruct<string, string>("8", "Enlightenment"),
            new DataStruct<string, string>("9", "Reborn"),
            new DataStruct<string, string>("10", "Transcendent")
            };
            GameObject obj13 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj13.GetComponent<Text>().text = "Big realm:";
            obj13.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj13.SetActive(value: true);
            for (int num4 = 0; num4 < allAttr.Length; num4++)
            {
                if (num4 > 0 && num4 % num == 0)
                {
                    num2 -= 40;
                }
                DataStruct<string, string> item = allAttr[num4];
                GameObject obj14 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj14.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + num4 % num * 180, num2);
                Toggle component7 = obj14.GetComponent<Toggle>();
                obj14.GetComponentInChildren<Text>().text = item.t2;
                component7.isOn = tmpData.selectJingjie.Contains(int.Parse(item.t1)) || tmpData.selectJingjieAll;
                component7.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectJingjie.Add(int.Parse(item.t1));
                        bool flag6 = true;
                        DataStruct<string, string>[] array4 = allAttr;
                        foreach (DataStruct<string, string> dataStruct7 in array4)
                        {
                            if (!tmpData.selectJingjie.Contains(int.Parse(dataStruct7.t1)))
                            {
                                flag6 = false;
                                break;
                            }
                        }
                        if (flag6)
                        {
                            tmpData.selectJingjieAll = true;
                            tmpData.selectJingjie.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectJingjieAll)
                        {
                            tmpData.selectJingjieAll = false;
                            tmpData.selectJingjie.Clear();
                            DataStruct<string, string>[] array4 = allAttr;
                            foreach (DataStruct<string, string> dataStruct8 in array4)
                            {
                                tmpData.selectJingjie.Add(int.Parse(dataStruct8.t1));
                            }
                        }
                        tmpData.selectJingjie.Remove(int.Parse(item.t1));
                    }
                });
                obj14.SetActive(value: true);
            }
            num2 -= 40;
            DataStruct<string, string>[] allAttr2 = new DataStruct<string, string>[3]
            {
            new DataStruct<string, string>("1", "Early"),
            new DataStruct<string, string>("2", "Middle"),
            new DataStruct<string, string>("3", "Late")
            };
            GameObject obj15 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj15.GetComponent<Text>().text = "Realm stage:";
            obj15.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj15.SetActive(value: true);
            for (int num5 = 0; num5 < allAttr2.Length; num5++)
            {
                if (num5 > 0 && num5 % num == 0)
                {
                    num2 -= 40;
                }
                DataStruct<string, string> item = allAttr2[num5];
                GameObject obj16 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj16.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + num5 % num * 180, num2);
                Toggle component8 = obj16.GetComponent<Toggle>();
                obj16.GetComponentInChildren<Text>().text = item.t2;
                component8.isOn = tmpData.selectJieduan.Contains(int.Parse(item.t1)) || tmpData.selectJieduanAll;
                component8.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectJieduan.Add(int.Parse(item.t1));
                        bool flag5 = true;
                        DataStruct<string, string>[] array3 = allAttr2;
                        foreach (DataStruct<string, string> dataStruct5 in array3)
                        {
                            if (!tmpData.selectJieduan.Contains(int.Parse(dataStruct5.t1)))
                            {
                                flag5 = false;
                                break;
                            }
                        }
                        if (flag5)
                        {
                            tmpData.selectJieduanAll = true;
                            tmpData.selectJieduan.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectJieduanAll)
                        {
                            tmpData.selectJieduanAll = false;
                            tmpData.selectJieduan.Clear();
                            DataStruct<string, string>[] array3 = allAttr2;
                            foreach (DataStruct<string, string> dataStruct6 in array3)
                            {
                                tmpData.selectJieduan.Add(int.Parse(dataStruct6.t1));
                            }
                        }
                        tmpData.selectJieduan.Remove(int.Parse(item.t1));
                    }
                });
                obj16.SetActive(value: true);
            }
            num2 -= 40;
            DataStruct<string, string>[] allAttr3 = new DataStruct<string, string>[3]
            {
            new DataStruct<string, string>("0", "Normal person [no Heart]"),
            new DataStruct<string, string>("1", "Paragon [Dao Heart]"),
            new DataStruct<string, string>("2", "Heaven's Chosen [Dao Heart]")
            };
            GameObject obj17 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj17.GetComponent<Text>().text = "The pursuit of Tao:";
            obj17.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj17.SetActive(value: true);
            for (int num6 = 0; num6 < allAttr3.Length; num6++)
            {
                if (num6 > 0 && num6 % num == 0)
                {
                    num2 -= 40;
                }
                DataStruct<string, string> item = allAttr3[num6];
                GameObject obj18 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj18.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + num6 % num * 180, num2);
                Toggle component9 = obj18.GetComponent<Toggle>();
                obj18.GetComponentInChildren<Text>().text = item.t2;
                component9.isOn = tmpData.selectHero.Contains(int.Parse(item.t1)) || tmpData.selectHeroAll;
                component9.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectHero.Add(int.Parse(item.t1));
                        bool flag4 = true;
                        DataStruct<string, string>[] array2 = allAttr3;
                        foreach (DataStruct<string, string> dataStruct3 in array2)
                        {
                            if (!tmpData.selectHero.Contains(int.Parse(dataStruct3.t1)))
                            {
                                flag4 = false;
                                break;
                            }
                        }
                        if (flag4)
                        {
                            tmpData.selectHeroAll = true;
                            tmpData.selectHero.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectHeroAll)
                        {
                            tmpData.selectHeroAll = false;
                            tmpData.selectHero.Clear();
                            DataStruct<string, string>[] array2 = allAttr3;
                            foreach (DataStruct<string, string> dataStruct4 in array2)
                            {
                                tmpData.selectHero.Add(int.Parse(dataStruct4.t1));
                            }
                        }
                        tmpData.selectHero.Remove(int.Parse(item.t1));
                    }
                });
                obj18.SetActive(value: true);
            }
            num2 -= 40;
            Il2CppSystem.Collections.Generic.List<ConfTaoistHeartItem> allDaoxin = g.conf.taoistHeart._allConfList;
            GameObject obj19 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj19.GetComponent<Text>().text = "Daoxin:";
            obj19.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj19.SetActive(value: true);
            for (int num7 = 0; num7 < allDaoxin.Count; num7++)
            {
                if (num7 > 0 && num7 % num == 0)
                {
                    num2 -= 40;
                }
                ConfTaoistHeartItem item = allDaoxin[num7];
                GameObject obj20 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj20.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + num7 % num * 180, num2);
                Toggle component10 = obj20.GetComponent<Toggle>();
                obj20.GetComponentInChildren<Text>().text = GameTool.LS(item.heartName);
                component10.isOn = tmpData.selectDaoxin.Contains(item.id) || tmpData.selectDaoxinAll;
                component10.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectDaoxin.Add(item.id);
                        bool flag3 = true;
                        Il2CppSystem.Collections.Generic.List<ConfTaoistHeartItem>.Enumerator enumerator2 = allDaoxin.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            ConfTaoistHeartItem current3 = enumerator2.Current;
                            if (!tmpData.selectDaoxin.Contains(current3.id))
                            {
                                flag3 = false;
                                break;
                            }
                        }
                        if (flag3)
                        {
                            tmpData.selectDaoxinAll = true;
                            tmpData.selectDaoxin.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectDaoxinAll)
                        {
                            tmpData.selectDaoxinAll = false;
                            tmpData.selectDaoxin.Clear();
                            Il2CppSystem.Collections.Generic.List<ConfTaoistHeartItem>.Enumerator enumerator2 = allDaoxin.GetEnumerator();
                            while (enumerator2.MoveNext())
                            {
                                ConfTaoistHeartItem current4 = enumerator2.Current;
                                tmpData.selectDaoxin.Add(current4.id);
                            }
                        }
                        tmpData.selectDaoxin.Remove(item.id);
                    }
                });
                obj20.SetActive(value: true);
            }
            num2 -= 40;
            DataStruct<string, string>[] allAttr4 = new DataStruct<string, string>[2]
            {
            new DataStruct<string, string>("0", "Single"),
            new DataStruct<string, string>("1", "Married")
            };
            GameObject obj21 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj21.GetComponent<Text>().text = "marriage:";
            obj21.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj21.SetActive(value: true);
            for (int num8 = 0; num8 < allAttr4.Length; num8++)
            {
                if (num8 > 0 && num8 % num == 0)
                {
                    num2 -= 40;
                }
                DataStruct<string, string> item = allAttr4[num8];
                GameObject obj22 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj22.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + num8 % num * 180, num2);
                Toggle component11 = obj22.GetComponent<Toggle>();
                obj22.GetComponentInChildren<Text>().text = item.t2;
                component11.isOn = tmpData.selectQinggan.Contains(int.Parse(item.t1)) || tmpData.selectQingganAll;
                component11.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectQinggan.Add(int.Parse(item.t1));
                        bool flag2 = true;
                        DataStruct<string, string>[] array = allAttr4;
                        foreach (DataStruct<string, string> dataStruct in array)
                        {
                            if (!tmpData.selectQinggan.Contains(int.Parse(dataStruct.t1)))
                            {
                                flag2 = false;
                                break;
                            }
                        }
                        if (flag2)
                        {
                            tmpData.selectQingganAll = true;
                            tmpData.selectQinggan.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectQingganAll)
                        {
                            tmpData.selectQingganAll = false;
                            tmpData.selectQinggan.Clear();
                            DataStruct<string, string>[] array = allAttr4;
                            foreach (DataStruct<string, string> dataStruct2 in array)
                            {
                                tmpData.selectQinggan.Add(int.Parse(dataStruct2.t1));
                            }
                        }
                        tmpData.selectQinggan.Remove(int.Parse(item.t1));
                    }
                });
                obj22.SetActive(value: true);
            }
            num2 -= 40;
            List<MapBuildSchool> builds = new List<MapBuildSchool>(g.world.build.GetBuilds<MapBuildSchool>().ToArray());
            GameObject obj23 = UnityEngine.Object.Instantiate(itemTitle, rightRoot);
            obj23.GetComponent<Text>().text = "Sect:";
            obj23.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, num2);
            obj23.SetActive(value: true);
            builds.Insert(0, null);
            for (int num9 = 0; num9 < builds.Count; num9++)
            {
                if (num9 > 0 && num9 % num == 0)
                {
                    num2 -= 40;
                }
                MapBuildSchool mapBuildSchool = builds[num9];
                string text = ((mapBuildSchool == null) ? "casual cultivator" : mapBuildSchool.name);
                string id = ((mapBuildSchool == null) ? "" : mapBuildSchool.buildData.id);
                GameObject obj24 = UnityEngine.Object.Instantiate(itemToggle, rightRoot);
                obj24.GetComponent<RectTransform>().anchoredPosition = new Vector2(150 + num9 % num * 180, num2);
                Toggle component12 = obj24.GetComponent<Toggle>();
                obj24.GetComponentInChildren<Text>().text = text;
                component12.isOn = tmpData.selectSchool.Contains(id) || tmpData.selectSchoolAll;
                component12.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
                {
                    if (isOn)
                    {
                        tmpData.selectSchool.Add(id);
                        bool flag = true;
                        foreach (MapBuildSchool item4 in builds)
                        {
                            if (!tmpData.selectSchool.Contains((item4 == null) ? "" : item4.buildData.id))
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            tmpData.selectSchoolAll = true;
                            tmpData.selectSchool.Clear();
                        }
                    }
                    else
                    {
                        if (tmpData.selectSchoolAll)
                        {
                            tmpData.selectSchoolAll = false;
                            tmpData.selectSchool.Clear();
                            foreach (MapBuildSchool item5 in builds)
                            {
                                tmpData.selectSchool.Add((item5 == null) ? "" : item5.buildData.id);
                            }
                        }
                        tmpData.selectSchool.Remove(id);
                    }
                });
                obj24.SetActive(value: true);
            }
            num2 -= 40;
            rightRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, Mathf.Abs(num2));
        }

        public static void InitSel(Transform root, Action updateCall)
        {
            Toggle component = root.Find("Root/selToggle").GetComponent<Toggle>();
            Button btn = root.Find("Root/selBtn").GetComponent<Button>();
            component.GetComponentInChildren<Text>().text = "Activate filtering";
            isOpenSel = PlayerPrefs.GetInt("SelectOneCharselTgl", 0) == 1;
            component.isOn = isOpenSel;
            btn.gameObject.SetActive(isOpenSel);
            btn.GetComponentInChildren<Text>().text = "Select filter criteria";
            component.onValueChanged.AddListener((Action<bool>)delegate (bool isOn)
            {
                isOpenSel = isOn;
                btn.gameObject.SetActive(isOpenSel);
                updateCall?.Invoke();
            });
            btn.onClick.AddListener((Action)delegate
            {
                ModMain.OpenUI<UINpcSelectClass>("NpcSelectClass").okCall = updateCall;
            });
        }

        public static bool CheckAll()
        {
            SelectData selectData = UINpcSelectClass.selectData;
            if (selectData.selectSexAll && selectData.selectMeiliAll && selectData.selectShengwangAll && selectData.selectXingge0All && selectData.selectXingge1All && selectData.selectXingge2All && selectData.selectJingjieAll && selectData.selectJieduanAll && selectData.selectJieduanAll && selectData.selectQingganAll && selectData.selectHeroAll && selectData.selectDaoxinAll)
            {
                return selectData.selectSchoolAll;
            }
            return false;
        }

        public static bool CheckUnit(WorldUnitBase unit)
        {
            WorldUnitDynData dynUnitData = unit.data.dynUnitData;
            SelectData selectData = UINpcSelectClass.selectData;
            if (!selectData.selectSchoolAll)
            {
                string schoolID = unit.data.unitData.schoolID;
                if (!selectData.selectSchool.Contains(schoolID))
                {
                    return false;
                }
            }
            if (!selectData.selectDaoxinAll)
            {
                int confID = unit.data.unitData.heart.confID;
                if (!selectData.selectDaoxin.Contains(confID))
                {
                    return false;
                }
            }
            if (!selectData.selectHeroAll)
            {
                int state = (int)unit.data.unitData.heart.state;
                if (!selectData.selectHero.Contains(state))
                {
                    return false;
                }
            }
            if (!selectData.selectQingganAll)
            {
                int item = ((!string.IsNullOrEmpty(unit.data.unitData.relationData.married)) ? 1 : 0);
                if (!selectData.selectQinggan.Contains(item))
                {
                    return false;
                }
            }
            if (!selectData.selectJieduanAll)
            {
                ConfRoleGradeItem item2 = g.conf.roleGrade.GetItem(dynUnitData.gradeID.value);
                if (!selectData.selectJieduan.Contains(item2.phase))
                {
                    return false;
                }
            }
            if (!selectData.selectJingjieAll && !selectData.selectJingjie.Contains(dynUnitData.GetGrade()))
            {
                return false;
            }
            if (!selectData.selectXingge2All && !selectData.selectXingge2.Contains(dynUnitData.outTrait2.value))
            {
                return false;
            }
            if (!selectData.selectXingge1All && !selectData.selectXingge1.Contains(dynUnitData.outTrait1.value))
            {
                return false;
            }
            if (!selectData.selectXingge0All && !selectData.selectXingge0.Contains(dynUnitData.inTrait.value))
            {
                return false;
            }
            if (!selectData.selectShengwangAll)
            {
                ConfRoleReputationItem itemInReputation = g.conf.roleReputation.GetItemInReputation(dynUnitData.reputation.value);
                if (!selectData.selectShengwang.Contains(itemInReputation.id))
                {
                    return false;
                }
            }
            if (!selectData.selectMeiliAll)
            {
                ConfRoleBeautyItem itemInBeauty = g.conf.roleBeauty.GetItemInBeauty(dynUnitData.beauty.value);
                if (!selectData.selectMeili.Contains(itemInBeauty.id))
                {
                    return false;
                }
            }
            if (!selectData.selectSexAll && !selectData.selectSex.Contains(dynUnitData.sex.value))
            {
                return false;
            }
            return true;
        }
    }
}
