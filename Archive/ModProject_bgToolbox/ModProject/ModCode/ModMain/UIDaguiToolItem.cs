using System;
using System.Collections.Generic;
using MOD_bgToolbox.Item;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox
{
    public class UIDaguiToolItem
    {
        public string func;
        public string[] allParams = new string[4];
        public string[] allParamsName = new string[4];
        public string[] list;
        public string[] listName;
        public GameObject[] listGo;
        public Func<WorldUnitBase> getUnit;
        public Func<DataProps.MartialData> getMatrialData;
        public Func<DataProps.PropsData> getArtifactData;
        public int selectLiaoji;
        public UIDaguiTool tool;

        public static string debugItemPara;
        public WorldUnitBase unit => getUnit?.Invoke();
        public DataProps.MartialData matrialData => getMatrialData?.Invoke();
        public DataProps.PropsData artifactData => getArtifactData?.Invoke();

        public void SaveData()
        {
            PlayerPrefs.SetString("UIDaguiToolItem_" + func, string.Join(" ", allParams));
            PlayerPrefs.SetString("UIDaguiToolItemName_" + func, string.Join(" ", allParamsName));
        }

        public void ReadData()
        {
            if (PlayerPrefs.HasKey("UIDaguiToolItem_" + func))
            {
                string[] array = PlayerPrefs.GetString("UIDaguiToolItem_" + func).Split(' ');
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        allParams[i] = array[i];
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (!PlayerPrefs.HasKey("UIDaguiToolItemName_" + func))
            {
                return;
            }
            string[] array2 = PlayerPrefs.GetString("UIDaguiToolItemName_" + func).Split(' ');
            for (int j = 0; j < 4; j++)
            {
                try
                {
                    allParamsName[j] = array2[j];
                }
                catch (Exception)
                {
                }
            }
        }

        public void InitData(UIDaguiTool tool, ConfDaguiToolItem item, Transform transform)
        {
            this.tool = tool;
            Transform parent = transform.Find("Root");
            GameObject rightTitleItem = tool.rightTitleItem;
            GameObject rightButtonItem = tool.rightButtonItem;
            GameObject rightInputItem = tool.rightInputItem;
            GameObject gameObject = UnityEngine.Object.Instantiate(rightTitleItem, parent);
            gameObject.GetComponentInChildren<Text>().text = item.titleName;
            gameObject.gameObject.SetActive(value: true);
            list = new string[4] { item.p1, item.p2, item.p3, item.p4 };
            listName = new string[4] { item.para1, item.para2, item.para3, item.para4 };
            listGo = new GameObject[4];
            func = item.func;
            ReadData();
            for (int i = 0; i < list.Length; i++)
            {
                int index = i;
                string text = list[i];
                if (string.IsNullOrEmpty(text) || text == "UpGrade")
                {
                    continue;
                }
                int itemType = 0;
                Action clickAction = null;
                GameObject gameObject2 = null;
                SetItemData(listName, index, text, ref itemType, ref clickAction);
                switch (itemType)
                {
                    case 0:
                        gameObject2 = UnityEngine.Object.Instantiate(rightButtonItem, parent);
                        gameObject2.transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + allParams[index];
                        gameObject2.AddComponent<Button>().onClick.AddListener((Action)delegate
                        {
                            clickAction?.Invoke();
                        });
                        break;
                    case 1:
                        {
                            gameObject2 = UnityEngine.Object.Instantiate(rightInputItem, parent);
                            gameObject2.transform.Find("Text").GetComponent<Text>().text = listName[index] + "：";
                            gameObject2.transform.Find("InputField/Placeholder").GetComponent<Text>().text = "";
                            InputField componentInChildren = gameObject2.GetComponentInChildren<InputField>();
                            componentInChildren.text = (string.IsNullOrEmpty(allParamsName[index]) ? allParams[index] : allParamsName[index]);
                            componentInChildren.onValueChanged.AddListener((Action<string>)delegate (string v)
                            {
                                allParams[index] = v;
                            });
                            break;
                        }
                    case 3:
                        gameObject2 = UnityEngine.Object.Instantiate(rightButtonItem, parent);
                        gameObject2.transform.Find("Text").GetComponent<Text>().text = "Input attributes";
                        gameObject2.AddComponent<Button>().onClick.AddListener((Action)delegate
                        {
                            clickAction?.Invoke();
                        });
                        break;
                    default:
                        Console.WriteLine("No match found " + itemType + " " + text);
                        break;
                }
                gameObject2.gameObject.SetActive(value: true);
                listGo[i] = gameObject2;
            }
            transform.Find("BtnRun").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                SaveData();
                List<string> list2 = new List<string>();
                list2.Add(item.func);
                list2.AddRange(allParams);
                ModMain.cmdRun.Cmd(list2.ToArray());
            });
            transform.Find("BtnAdd").GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                SaveData();
                List<string> list = new List<string>();
                list.Add(item.func);
                list.AddRange(allParams);
                string text2 = string.Join(" ", list);
                tool.AddCmdItem(text2);
                UIDaguiTool.tmpCmdItem.cmds.Add(text2);
                ModMain.cmdRun.Cmd(list.ToArray());
            });
        }

        private void SetItemData(string[] listName, int index, string para, ref int itemType, ref Action clickAction)
        {
            debugItemPara = para;
            if (para == "ChooseQiFortune")
            {
                clickAction = delegate
                {
                    UIChooseQiFortune uIChooseQiFortune = ModMain.OpenUI<UIChooseQiFortune>(para);
                    uIChooseQiFortune.InitData(this, index);
                    uIChooseQiFortune.call = delegate (string p, string attrName)
                    {
                        allParams[index] = p;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "DesignateQiFortune")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIDesignateQiFortune uIDesignateQiFortune = ModMain.OpenUI<UIDesignateQiFortune>(para);
                        uIDesignateQiFortune.InitData(this, index, unit);
                        uIDesignateQiFortune.call = delegate (string p, string attrName)
                        {
                            allParams[index] = p;
                            allParamsName[index] = listName[index] + ":" + attrName;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "AttributeType")
            {
                clickAction = delegate
                {
                    UIAttributeType uIAttributeType = ModMain.OpenUI<UIAttributeType>(para);
                    uIAttributeType.InitData(this, index);
                    uIAttributeType.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        allParamsName[index] = listName[index] + ":" + attrName;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "DaoHeartType")
            {
                clickAction = delegate
                {
                    UIDaoHeartType uIDaoHeartType = ModMain.OpenUI<UIDaoHeartType>(para);
                    uIDaoHeartType.InitData(this, index);
                    uIDaoHeartType.call = delegate (string p, int items)
                    {
                        allParams[index] = p;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + GameTool.LS(g.conf.taoistHeart.GetItem(items).heartName);
                        allParamsName[index] = listName[index] + ":" + GameTool.LS(g.conf.taoistHeart.GetItem(items).heartName);
                    };
                };
            }
            else if (para == "ChooseHeroSkill")
            {
                clickAction = delegate
                {
                    UIChooseHeroSkill uIChooseHeroSkill = ModMain.OpenUI<UIChooseHeroSkill>(para);
                    uIChooseHeroSkill.InitData(this, index);
                    uIChooseHeroSkill.call = delegate (string p, string name)
                    {
                        allParams[index] = p;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + name;
                        allParamsName[index] = listName[index] + ":" + name;
                    };
                };
            }
            else if (para == "DaoKindType")
            {
                clickAction = delegate
                {
                    UIDaoKindType uIDaoKindType = ModMain.OpenUI<UIDaoKindType>(para);
                    uIDaoKindType.InitData(this, index);
                    uIDaoKindType.call = delegate (string p, int items)
                    {
                        allParams[index] = p;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + GameTool.LS(g.conf.taoistSeed.GetItem(items).seedName);
                        allParamsName[index] = listName[index] + ":" + GameTool.LS(g.conf.taoistSeed.GetItem(items).seedName);
                    };
                };
            }
            else if (para == "ResidualHeartType")
            {
                clickAction = delegate
                {
                    UIResidualHeartType uIResidualHeartType = ModMain.OpenUI<UIResidualHeartType>(para);
                    uIResidualHeartType.InitData(this, index);
                    uIResidualHeartType.call = delegate (string p, int items)
                    {
                        allParams[index] = p;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + GameTool.LS(g.conf.taoistHeart.GetItem(items).heartName);
                        allParamsName[index] = listName[index] + ":" + GameTool.LS(g.conf.taoistHeart.GetItem(items).heartName);
                    };
                };
            }
            else if (para == "ChooseToReverseFate")
            {
                clickAction = delegate
                {
                    UIChooseToReverseFate uIChooseToReverseFate = ModMain.OpenUI<UIChooseToReverseFate>(para);
                    uIChooseToReverseFate.InitData(this, index);
                    uIChooseToReverseFate.call = delegate (string p, string attrName)
                    {
                        allParams[index] = p;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseToSchoolFate")
            {
                clickAction = delegate
                {
                    UIChooseToSchoolFate uIChooseToSchoolFate = ModMain.OpenUI<UIChooseToSchoolFate>(para);
                    uIChooseToSchoolFate.InitData(this, index);
                    uIChooseToSchoolFate.call = delegate (string p, string attrName)
                    {
                        allParams[index] = p;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseDaoNumber")
            {
                clickAction = delegate
                {
                    UIChooseDaoNumber uIChooseDaoNumber = ModMain.OpenUI<UIChooseDaoNumber>(para);
                    uIChooseDaoNumber.InitData(this, index);
                    uIChooseDaoNumber.call = delegate (string p, List<int> items)
                    {
                        allParams[index] = p;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + GameTool.LS(g.conf.appellationTitle.GetItem(items[0]).name);
                        allParamsName[index] = listName[index] + ":" + GameTool.LS(g.conf.appellationTitle.GetItem(items[0]).name);
                    };
                };
            }
            else if (para == "Position")
            {
                clickAction = delegate
                {
                    UIPosition uIPosition = ModMain.OpenUI<UIPosition>(para);
                    uIPosition.InitData(this, index);
                    uIPosition.call = delegate (string item, string name)
                    {
                        allParams[index] = item;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + name;
                        allParamsName[index] = listName[index] + ":" + name;
                    };
                };
            }
            else if (para == "DesignateHall")
            {
                clickAction = delegate
                {
                    UIDesignateHall uIDesignateHall = ModMain.OpenUI<UIDesignateHall>(para);
                    uIDesignateHall.InitData(this, index);
                    uIDesignateHall.call = delegate (string item, string name)
                    {
                        allParams[index] = item;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + name;
                        allParamsName[index] = listName[index] + ":" + name;
                    };
                };
            }
            else if (para == "DesignateSect")
            {
                clickAction = delegate
                {
                    UIDesignateSect uIDesignateSect = ModMain.OpenUI<UIDesignateSect>(para);
                    uIDesignateSect.InitData(this, index);
                    uIDesignateSect.call = delegate (string item, string name)
                    {
                        allParams[index] = item;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + name;
                        allParamsName[index] = listName[index] + ":" + name;
                    };
                };
            }
            else if (para == "DesignateAttribute")
            {
                clickAction = delegate
                {
                    UIDesignateAttribute uIDesignateAttribute = ModMain.OpenUI<UIDesignateAttribute>(para);
                    uIDesignateAttribute.InitData(this, index);
                    uIDesignateAttribute.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseTechniqueType")
            {
                clickAction = delegate
                {
                    UIChooseTechniqueType uIChooseTechniqueType = ModMain.OpenUI<UIChooseTechniqueType>(para);
                    uIChooseTechniqueType.InitData(this, index);
                    uIChooseTechniqueType.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseRealm")
            {
                clickAction = delegate
                {
                    UIChooseRealm uIChooseRealm = ModMain.OpenUI<UIChooseRealm>(para);
                    uIChooseRealm.InitData(this, index);
                    uIChooseRealm.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "Grade")
            {
                clickAction = delegate
                {
                    UIGrade uIGrade = ModMain.OpenUI<UIGrade>(para);
                    uIGrade.InitData(this, index);
                    uIGrade.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseSoul")
            {
                clickAction = delegate
                {
                    UIChooseSoul uIChooseSoul = ModMain.OpenUI<UIChooseSoul>(para);
                    uIChooseSoul.InitData(this, index);
                    uIChooseSoul.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseLiaoji")
            {
                clickAction = delegate
                {
                    UIChooseLiaoji uIChooseLiaoji = ModMain.OpenUI<UIChooseLiaoji>(para);
                    uIChooseLiaoji.InitData(this, index);
                    uIChooseLiaoji.call = delegate (string attr, string attrName)
                    {
                        int.TryParse(attr, out selectLiaoji);
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
                int.TryParse(allParams[index], out selectLiaoji);
            }
            else if (para == "ChooseLiaojiEffect")
            {
                clickAction = delegate
                {
                    if (selectLiaoji == 0)
                    {
                        UITipItem.AddTip("Please choose the ethereal power first!", 0f);
                    }
                    else
                    {
                        UIChooseLiaojiEffect uIChooseLiaojiEffect = ModMain.OpenUI<UIChooseLiaojiEffect>(para);
                        uIChooseLiaojiEffect.InitData(this, index, selectLiaoji);
                        uIChooseLiaojiEffect.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "ChooseLiaojiEffectUp")
            {
                clickAction = delegate
                {
                    if (selectLiaoji == 0)
                    {
                        UITipItem.AddTip("Please choose the power of mist first!", 0f);
                    }
                    else
                    {
                        UIChooseLiaojiEffectUp uIChooseLiaojiEffectUp = ModMain.OpenUI<UIChooseLiaojiEffectUp>(para);
                        uIChooseLiaojiEffectUp.InitData(this, index, selectLiaoji);
                        uIChooseLiaojiEffectUp.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "DesignatePurpose")
            {
                clickAction = delegate
                {
                    UIDesignatePurpose uIDesignatePurpose = ModMain.OpenUI<UIDesignatePurpose>(para);
                    uIDesignatePurpose.InitData(this, index);
                    uIDesignatePurpose.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "DesignateRules")
            {
                clickAction = delegate
                {
                    UIDesignateRules uIDesignateRules = ModMain.OpenUI<UIDesignateRules>(para);
                    uIDesignateRules.InitData(this, index);
                    uIDesignateRules.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "DesignateXingge")
            {
                clickAction = delegate
                {
                    UIDesignateTrait uIDesignateTrait = ModMain.OpenUI<UIDesignateTrait>("DesignateTrait");
                    uIDesignateTrait.InitData(this, index);
                    uIDesignateTrait.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseRighteousnessOrEvil")
            {
                clickAction = delegate
                {
                    UIChooseRighteousnessOrEvil uIChooseRighteousnessOrEvil = ModMain.OpenUI<UIChooseRighteousnessOrEvil>(para);
                    uIChooseRighteousnessOrEvil.InitData(this, index);
                    uIChooseRighteousnessOrEvil.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "RelationshipType")
            {
                clickAction = delegate
                {
                    UIRelationshipType uIRelationshipType = ModMain.OpenUI<UIRelationshipType>(para);
                    uIRelationshipType.InitData(this, index);
                    uIRelationshipType.call = delegate (string attr, int attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "DesignateSecretBook")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIDesignateSecretBook uIDesignateSecretBook = ModMain.OpenUI<UIDesignateSecretBook>(para);
                        uIDesignateSecretBook.InitData(this, index, unit);
                        uIDesignateSecretBook.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "DesignateTechnique")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIDesignateTechnique uIDesignateTechnique = ModMain.OpenUI<UIDesignateTechnique>(para);
                        uIDesignateTechnique.InitData(this, index, unit);
                        uIDesignateTechnique.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "DesignateInstrumentSpirit")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIDesignateInstrumentSpirit uIDesignateInstrumentSpirit = ModMain.OpenUI<UIDesignateInstrumentSpirit>(para);
                        uIDesignateInstrumentSpirit.InitData(this, index, unit);
                        uIDesignateInstrumentSpirit.call = delegate (string attr, DataUnit.ArtifactSpriteData.Sprite selectItem)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + GameTool.LS(g.conf.artifactSprite.GetItem(selectItem.spriteID).name);
                            allParamsName[index] = listName[index] + ":" + GameTool.LS(g.conf.artifactSprite.GetItem(selectItem.spriteID).name);
                        };
                    }
                };
            }
            else if (para == "ChooseInstrumentSpirit")
            {
                clickAction = delegate
                {
                    UIChooseInstrumentSpirit uIChooseInstrumentSpirit = ModMain.OpenUI<UIChooseInstrumentSpirit>(para);
                    uIChooseInstrumentSpirit.InitData(this, index);
                    uIChooseInstrumentSpirit.call = delegate (string attr, ConfArtifactSpriteItem selectItem)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + GameTool.LS(selectItem.name);
                        allParamsName[index] = listName[index] + ":" + GameTool.LS(selectItem.name);
                    };
                };
            }
            else if (para == "ChooseHaotianEyeSkill")
            {
                clickAction = delegate
                {
                    UIChooseHaotianEyeSkill uIChooseHaotianEyeSkill = ModMain.OpenUI<UIChooseHaotianEyeSkill>(para);
                    uIChooseHaotianEyeSkill.InitData(this, index);
                    uIChooseHaotianEyeSkill.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "DesignateHeartTechniqueSecretBook")
            {
                if (unit == null)
                {
                    UITipItem.AddTip("Please choose a valid character first!", 0f);
                    return;
                }
                clickAction = delegate
                {
                    UIDesignateHeartBook uIDesignateHeartBook = ModMain.OpenUI<UIDesignateHeartBook>("DesignateHeartBook");
                    uIDesignateHeartBook.InitData(this, index, unit);
                    uIDesignateHeartBook.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseFairyTechnique")
            {
                clickAction = delegate
                {
                    UIChooseFairyTechnique uIChooseFairyTechnique = ModMain.OpenUI<UIChooseFairyTechnique>(para);
                    uIChooseFairyTechnique.InitData(this, index);
                    uIChooseFairyTechnique.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseProps")
            {
                clickAction = delegate
                {
                    UIChooseProps uIChooseProps = ModMain.OpenUI<UIChooseProps>(para);
                    uIChooseProps.InitData(this, index);
                    uIChooseProps.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseSecretBook")
            {
                clickAction = delegate
                {
                    UIChooseSecretBook uIChooseSecretBook = ModMain.OpenUI<UIChooseSecretBook>(para);
                    uIChooseSecretBook.InitData(this, index);
                    uIChooseSecretBook.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseSuit")
            {
                clickAction = delegate
                {
                    UIChooseSuit uIChooseSuit = ModMain.OpenUI<UIChooseSuit>(para);
                    uIChooseSuit.InitData(this, index);
                    uIChooseSuit.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "DesignateSuit")
            {
                clickAction = delegate
                {
                    if (matrialData == null)
                    {
                        UITipItem.AddTip("Please select the skill secret book first", 0f);
                    }
                    else
                    {
                        UIDesignateSuit uIDesignateSuit = ModMain.OpenUI<UIDesignateSuit>(para);
                        uIDesignateSuit.InitData(this, index, matrialData.baseID);
                        uIDesignateSuit.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "WhetherToLearnAutomatically")
            {
                clickAction = delegate
                {
                    UIWhetherToLearnAutomatically uIWhetherToLearnAutomatically = ModMain.OpenUI<UIWhetherToLearnAutomatically>(para);
                    uIWhetherToLearnAutomatically.InitData(this, index);
                    uIWhetherToLearnAutomatically.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseToAddEntry")
            {
                clickAction = delegate
                {
                    if (matrialData == null)
                    {
                        UITipItem.AddTip("Please select the skill secret book first", 0f);
                    }
                    else
                    {
                        UIChooseToAddEntry uIChooseToAddEntry = ModMain.OpenUI<UIChooseToAddEntry>(para);
                        uIChooseToAddEntry.InitData(this, index, matrialData.martialType, matrialData.baseID);
                        uIChooseToAddEntry.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "charA")
            {
                clickAction = delegate
                {
                    UISelectChar uISelectChar2 = ModMain.OpenUI<UISelectChar>("SelectChar");
                    uISelectChar2.InitData(this, index);
                    uISelectChar2.call = delegate (string attr, string attrName)
                    {
                        ModMain.cmdRun.GetUnit(attr);
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "charB")
            {
                clickAction = delegate
                {
                    UISelectChar uISelectChar = ModMain.OpenUI<UISelectChar>("SelectChar");
                    uISelectChar.InitData(this, index);
                    uISelectChar.call = delegate (string attr, string attrName)
                    {
                        ModMain.cmdRun.GetUnit(attr);
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "charOneA")
            {
                clickAction = delegate
                {
                    UISelectOneChar uISelectOneChar2 = ModMain.OpenUI<UISelectOneChar>("SelectOneChar");
                    uISelectOneChar2.InitData(this, index);
                    uISelectOneChar2.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
                getUnit = () => ModMain.cmdRun.GetUnit(allParams[index]);
            }
            else if (para == "charOneB")
            {
                clickAction = delegate
                {
                    UISelectOneChar uISelectOneChar = ModMain.OpenUI<UISelectOneChar>("SelectOneChar");
                    uISelectOneChar.InitData(this, index);
                    uISelectOneChar.call = delegate (string attr, string attrName)
                    {
                        ModMain.cmdRun.GetUnit(attr);
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "DesignateOneSecretBook")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIDesignateOneSecretBook uIDesignateOneSecretBook = ModMain.OpenUI<UIDesignateOneSecretBook>(para);
                        uIDesignateOneSecretBook.InitData(this, index, unit);
                        uIDesignateOneSecretBook.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
                getMatrialData = () => ModMain.cmdRun.GetMartial(unit, allParams[index]);
            }
            else if (para == "DesignateOneTechnique")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIDesignateOneTechnique uIDesignateOneTechnique = ModMain.OpenUI<UIDesignateOneTechnique>(para);
                        uIDesignateOneTechnique.InitData(this, index, unit);
                        uIDesignateOneTechnique.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
                getMatrialData = () => ModMain.cmdRun.GetSkill(unit, allParams[index])?.data.To<DataProps.MartialData>();
            }
            else if (para == "DesignateHeartTechnique")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIDesignateHeartTechnique uIDesignateHeartTechnique = ModMain.OpenUI<UIDesignateHeartTechnique>(para);
                        uIDesignateHeartTechnique.InitData(this, index, unit);
                        uIDesignateHeartTechnique.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
                getMatrialData = () => ModMain.cmdRun.GetAbility(unit, allParams[index])?.data.To<DataProps.MartialData>();
            }
            else if (para == "ChoosePotmon")
            {
                clickAction = delegate
                {
                    UIChoosePotmon uIChoosePotmon = ModMain.OpenUI<UIChoosePotmon>(para);
                    uIChoosePotmon.InitData(this, index);
                    uIChoosePotmon.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                        allParamsName[index] = listName[index] + ":" + attrName;
                    };
                };
            }
            else if (para == "ChooseElder")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIChooseElder uIChooseElder = ModMain.OpenUI<UIChooseElder>(para);
                        uIChooseElder.InitData(this, index, unit);
                        uIChooseElder.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "ChooseRule")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIChooseRule uIChooseRule = ModMain.OpenUI<UIChooseRule>(para);
                        uIChooseRule.InitData(this, index, unit);
                        uIChooseRule.call = delegate (string attr, string attrName)
                        {
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "ChooseArtifact")
            {
                clickAction = delegate
                {
                    if (unit == null)
                    {
                        UITipItem.AddTip("Please choose a valid character first!", 0f);
                    }
                    else
                    {
                        UIChooseArtifact uIChooseArtifact = ModMain.OpenUI<UIChooseArtifact>(para);
                        uIChooseArtifact.InitData(this, index, unit);
                        uIChooseArtifact.call = delegate (string attr, string attrName)
                        {
                            getArtifactData = () => ModMain.cmdRun.GetArtifact(unit, allParams[index]);
                            allParams[index] = attr;
                            listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                            allParamsName[index] = listName[index] + ":" + attrName;
                        };
                    }
                };
            }
            else if (para == "EditArtifactAttr")
            {
                clickAction = delegate
                {
                    if (artifactData == null)
                    {
                        UITipItem.AddTip("You need to designate a magic weapon first", 0f);
                    }
                    else
                    {
                        DataProps.PropsArtifact propsArtifact = artifactData.To<DataProps.PropsArtifact>();
                        if (propsArtifact == null)
                        {
                            UITipItem.AddTip("You need to designate a magic weapon first", 0f);
                        }
                        else
                        {
                            UIEditArtifactAttr uIEditArtifactAttr = ModMain.OpenUI<UIEditArtifactAttr>(para);
                            uIEditArtifactAttr.InitData(this, index, propsArtifact);
                            uIEditArtifactAttr.call = delegate (string attr, string attrName)
                            {
                                allParams[index] = attr;
                                listGo[index].transform.Find("Text").GetComponent<Text>().text = listName[index] + ":" + attrName;
                                allParamsName[index] = listName[index] + ":" + attrName;
                            };
                        }
                    }
                };
            }
            else if (para == "InputInt")
            {
                clickAction = delegate
                {
                    UIInputInt uIInputInt = ModMain.OpenUI<UIInputInt>(para);
                    uIInputInt.InitData(this, index, listName[index].Split('|'));
                    uIInputInt.call = delegate (string attr, string attrName)
                    {
                        allParams[index] = attr;
                        listGo[index].transform.Find("Text").GetComponent<Text>().text = "input attributes";
                    };
                };
                itemType = 3;
            }
            else
            {
                itemType = 1;
            }
        }
    }
}
