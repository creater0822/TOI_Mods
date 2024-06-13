using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;

namespace MOD_tlRCB
{
    public static class Units
    {
        private const string GROUP_NAME = "淫女宫";

        public static DataUnit.UnitInfoData GetUnitByName(string name)
        {
            Il2CppSystem.Collections.Generic.Dictionary<string, DataUnit.UnitInfoData>.Enumerator enumerator = g.data.unit.allUnit.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Il2CppSystem.Collections.Generic.KeyValuePair<string, DataUnit.UnitInfoData> current = enumerator.Current;
                if (current.value.propertyData.GetName() == name)
                {
                    return current.value;
                }
            }
            return null;
        }

        public static List<DataUnit.UnitInfoData> Filter(Func<DataUnit.UnitInfoData, bool> filter)
        {
            List<DataUnit.UnitInfoData> list = new List<DataUnit.UnitInfoData>();
            Il2CppSystem.Collections.Generic.Dictionary<string, DataUnit.UnitInfoData>.ValueCollection.Enumerator enumerator = g.data.unit.allUnit.values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DataUnit.UnitInfoData current = enumerator.Current;
                if (filter(current))
                {
                    list.Add(current);
                }
            }
            return list;
        }

        public static int AddMoney(this WorldUnitBase unit, int count)
        {
            Il2CppSystem.Collections.Generic.List<DataProps.PropsData>.Enumerator enumerator = unit.data.unitData.propData.allProps.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DataProps.PropsData current = enumerator.Current;
                if (current.propsID == 10001)
                {
                    int propsCount = current.propsCount;
                    current.propsCount = Math.Max(0, propsCount + count);
                    return current.propsCount;
                }
            }
            return -1;
        }

        public static int GetMoney(this WorldUnitBase unit)
        {
            return unit.data.unitData.propData.GetPropsNum(10001);
        }

        public static bool IsWoman(this WorldUnitBase unit)
        {
            return unit.data.unitData.propertyData.sex == UnitSexType.Woman;
        }

        public static bool IsMan(this WorldUnitBase unit)
        {
            return unit.data.unitData.propertyData.sex == UnitSexType.Man;
        }

        public static void SetAge(this WorldUnitBase unit, int age)
        {
            if (age < 1)
            {
                Log.Debug("Units.SetAge 年龄不能小于0");
            }
            else
            {
                unit.data.unitData.propertyData.age = age;
            }
        }

        public static int GetAge(this WorldUnitBase unit)
        {
            return unit.data.unitData.propertyData.age;
        }

        public static void AddAge(this WorldUnitBase unit, int month)
        {
            int age = unit.data.unitData.propertyData.age;
            if (age + month < 1)
            {
                Log.Debug("Units.AddAge 修改后年龄小于0了，所以没修改");
            }
            else
            {
                unit.data.unitData.propertyData.age = age + month;
            }
        }

        public static void SetFirstName(this WorldUnitBase unit, string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                Log.Debug("SetFirstName 姓不可为空！");
            }
            else
            {
                unit.data.unitData.propertyData.name[0] = firstName;
            }
        }

        public static void SetLastName(this WorldUnitBase unit, string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Log.Debug("SetLastName 名字不可为空！");
            }
            else
            {
                unit.data.unitData.propertyData.name[1] = lastName;
            }
        }

        public static void SetName(this WorldUnitBase unit, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                Log.Debug("SetFirstName 姓不可为空！");
                return;
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Log.Debug("SetLastName 名不可为空！");
                return;
            }
            unit.data.unitData.propertyData.name = new Il2CppStringArray(new string[2] { firstName, lastName });
        }

        public static WorldUnitBase CreateChild(int npcSpecialID, WorldUnitBase father, WorldUnitBase mather, UnitSexType sex = UnitSexType.None, int age = 12, int gradeId = -1, string lastName = null)
        {
            if (father == null && mather == null)
            {
                return null;
            }
            ConfMgr conf = g.conf;
            ConfDramaNpc dramaNpc = conf.dramaNpc;
            ConfRoleDress roleDress = conf.roleDress;
            ConfNpcSpecial npcSpecial = g.conf.npcSpecial;
            ConfRoleAttributeCoefficient roleAttributeCoefficient = g.conf.roleAttributeCoefficient;
            DataUnit.UnitInfoData unitInfoData = new DataUnit.UnitInfoData();
            DataUnit.PropertyData propertyData = unitInfoData.propertyData;
            ConfNpcSpecialItem specialItem = npcSpecial.GetItem(npcSpecialID);
            roleAttributeCoefficient.UnitInitProperty(unitInfoData);
            roleAttributeCoefficient.UnitRandomProperty(unitInfoData, 100);
            if (sex != 0)
            {
                roleAttributeCoefficient.UnitRandomSex(unitInfoData, (int)sex, 0);
            }
            else
            {
                roleAttributeCoefficient.UnitRandomSex(unitInfoData, specialItem.gender, 0);
            }
            propertyData.age = age;
            if (father != null && mather != null)
            {
                string value = ((father.IsMarried(mather) || mather.IsPlayer()) ? father : mather).data.unitData.propertyData.name[0];
                propertyData.name[0] = value;
            }
            else if (father != null)
            {
                propertyData.name[0] = father.data.unitData.propertyData.name[0];
            }
            else if (mather != null)
            {
                propertyData.name[0] = mather.data.unitData.propertyData.name[0];
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                propertyData.name[1] = lastName;
            }
            if (gradeId == -1)
            {
                ConfRoleGrade roleGrade = conf.roleGrade;
                propertyData.SetGradeID(roleGrade.GetGradeItem(specialItem.grade, 1).id);
            }
            else
            {
                propertyData.SetGradeID(gradeId);
            }
            WorldUnitBase worldUnit = g.world.unit.AddUnit(unitInfoData);
            Action action = delegate
            {
                npcSpecial.ModifProperty(worldUnit, specialItem);
            };
            Action action2 = delegate
            {
            };
            WorldInitNPCTool.InitUnit(worldUnit, action, action2);
            string text = ((propertyData.sex == UnitSexType.Man) ? specialItem.dressMan : specialItem.dressWoman);
            if (text != "0")
            {
                Il2CppSystem.Collections.Generic.List<int> list = CommonTool.StrSplitIntList(text, '|');
                int[] array = new int[list.Count];
                int num = 0;
                Il2CppSystem.Collections.Generic.List<int>.Enumerator enumerator = list.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    int current = enumerator.Current;
                    array[num++] = current;
                }
                int id = array[UnityEngine.Random.Range(0, list.Count)];
                PortraitModelData portraitModelDataInModel = dramaNpc.GetPortraitModelDataInModel(id);
                BattleModelHumanData modelHumanDataInModel = dramaNpc.GetModelHumanDataInModel(id);
                worldUnit.data.SetModelData(portraitModelDataInModel, modelHumanDataInModel);
            }
            propertyData.beauty = roleDress.GetBeautyValue(propertyData.modelData);
            npcSpecial.ModifPropertyCreateEnd(worldUnit, specialItem);
            worldUnit.SetUnitBornLuck(specialItem);
            unitInfoData.SetPoint(mather?.data.unitData.GetPoint() ?? npcSpecial.GetPoint(specialItem));
            if (father != null && mather != null)
            {
                if (father.IsMarried(mather))
                {
                    Il2CppSystem.Collections.Generic.List<string>.Enumerator enumerator2 = father.data.unitData.relationData.children.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        string current2 = enumerator2.Current;
                        if (current2 != worldUnit.data.unitData.unitID)
                        {
                            worldUnit.data.unitData.relationData.brother.Add(current2);
                            g.world.unit.GetUnit(current2)?.data.unitData.relationData.brother.Add(worldUnit.data.unitData.unitID);
                        }
                    }
                    father.data.unitData.relationData.children.Add(worldUnit.data.unitData.unitID);
                    mather.data.unitData.relationData.children.Add(worldUnit.data.unitData.unitID);
                }
                else
                {
                    Il2CppSystem.Collections.Generic.List<string>.Enumerator enumerator2 = mather.data.unitData.relationData.childrenPrivate.GetEnumerator();
                    while (enumerator2.MoveNext())
                    {
                        string current3 = enumerator2.Current;
                        if (current3 != worldUnit.data.unitData.unitID)
                        {
                            worldUnit.data.unitData.relationData.brother.Add(current3);
                            g.world.unit.GetUnit(current3)?.data.unitData.relationData.brother.Add(worldUnit.data.unitData.unitID);
                        }
                    }
                    father.data.unitData.relationData.childrenPrivate.Add(worldUnit.data.unitData.unitID);
                    mather.data.unitData.relationData.childrenPrivate.Add(worldUnit.data.unitData.unitID);
                }
                worldUnit.data.unitData.ChangedParent(father);
                worldUnit.data.unitData.ChangedParent(mather);
                worldUnit.data.unitData.relationData.SetIntim(father.GetID(), (age >= 192) ? 5000 : 120);
                worldUnit.data.unitData.relationData.SetIntim(mather.GetID(), (age >= 192) ? 5000 : 120);
            }
            else if (father != null)
            {
                Il2CppSystem.Collections.Generic.List<string>.Enumerator enumerator2 = father.data.unitData.relationData.children.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    string current4 = enumerator2.Current;
                    worldUnit.data.unitData.relationData.brother.Add(current4);
                    g.world.unit.GetUnit(current4)?.data.unitData.relationData.brother.Add(current4);
                }
                worldUnit.data.unitData.ChangedParent(father, isTwoWay: true);
            }
            else if (mather != null)
            {
                worldUnit.data.unitData.ChangedParent(mather, isTwoWay: true);
                Il2CppSystem.Collections.Generic.List<string>.Enumerator enumerator2 = mather.data.unitData.relationData.children.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    string current5 = enumerator2.Current;
                    worldUnit.data.unitData.relationData.brother.Add(current5);
                    g.world.unit.GetUnit(current5)?.data.unitData.relationData.brother.Add(current5);
                }
            }
            if (age > 0)
            {
                worldUnit.SetAge(age);
            }
            if (worldUnit.IsWoman())
            {
                worldUnit.AddLuck(-527021125);
            }
            return worldUnit;
        }

        public static void SetFaceInherit(this WorldUnitBase children, WorldUnitBase father, WorldUnitBase mather, int percent)
        {
            WorldUnitBase unit = (children.IsMan() ? father : mather) ?? father ?? mather;
            if (unit != null)
            {
                percent.Percent(delegate
                {
                    children.data.unitData.propertyData.modelData.eyes = unit.data.unitData.propertyData.modelData.eyes;
                    children.data.unitData.propertyData.modelData.eyesBeauty = unit.data.unitData.propertyData.modelData.eyesBeauty;
                    children.data.unitData.propertyData.modelData.eyesOffsetY = unit.data.unitData.propertyData.modelData.eyesOffsetY;
                });
                percent.Percent(delegate
                {
                    children.data.unitData.propertyData.modelData.eyebrows = unit.data.unitData.propertyData.modelData.eyebrows;
                    children.data.unitData.propertyData.modelData.eyebrowsBeauty = unit.data.unitData.propertyData.modelData.eyebrowsBeauty;
                    children.data.unitData.propertyData.modelData.eyebrowsOffsetY = unit.data.unitData.propertyData.modelData.eyebrowsOffsetY;
                });
                percent.Percent(delegate
                {
                    children.data.unitData.propertyData.modelData.mouth = unit.data.unitData.propertyData.modelData.mouth;
                    children.data.unitData.propertyData.modelData.mouthBeauty = unit.data.unitData.propertyData.modelData.mouthBeauty;
                    children.data.unitData.propertyData.modelData.mouthOffsetY = unit.data.unitData.propertyData.modelData.mouthOffsetY;
                });
                percent.Percent(delegate
                {
                    children.data.unitData.propertyData.modelData.nose = unit.data.unitData.propertyData.modelData.nose;
                    children.data.unitData.propertyData.modelData.noseBeauty = unit.data.unitData.propertyData.modelData.noseBeauty;
                    children.data.unitData.propertyData.modelData.noseOffsetY = unit.data.unitData.propertyData.modelData.noseOffsetY;
                });
                percent.Percent(delegate
                {
                    children.data.unitData.propertyData.modelData.head = unit.data.unitData.propertyData.modelData.head;
                    children.data.unitData.propertyData.modelData.headBeauty = unit.data.unitData.propertyData.modelData.headBeauty;
                });
                children.data.unitData.propertyData.beauty = g.conf.roleDress.GetBeautyValue(children.data.unitData.propertyData.modelData);
            }
        }

        public static WorldUnitBase CreateNpc(int npcSpecialID, WorldUnitBase relationUnit)
        {
            ConfMgr conf = g.conf;
            WorldMgr world = g.world;
            _ = g.events;
            ConfNpcSpecial npcSpecial = conf.npcSpecial;
            ConfRoleAttributeCoefficient roleAttributeCoefficient = conf.roleAttributeCoefficient;
            ConfRoleGrade roleGrade = conf.roleGrade;
            ConfDramaNpc dramaNpc = conf.dramaNpc;
            ConfRoleDress roleDress = conf.roleDress;
            int sex = 2;
            DataUnit.UnitInfoData unitInfoData = new DataUnit.UnitInfoData();
            DataUnit.PropertyData propertyData = unitInfoData.propertyData;
            ConfNpcSpecialItem specialItem = npcSpecial.GetItem(npcSpecialID);
            roleAttributeCoefficient.UnitInitProperty(unitInfoData);
            roleAttributeCoefficient.UnitRandomProperty(unitInfoData, 100);
            roleAttributeCoefficient.UnitRandomSex(unitInfoData, sex, int.MaxValue);
            int id = roleGrade.GetGradeItem(specialItem.grade, 1).id;
            propertyData.SetGradeID(id);
            WorldUnitBase worldUnit = world.unit.AddUnit(unitInfoData);
            Action action = delegate
            {
                npcSpecial.ModifProperty(worldUnit, specialItem);
            };
            Action action2 = delegate
            {
            };
            WorldInitNPCTool.InitUnit(worldUnit, action, action2);
            worldUnit.SetUnitRelation(specialItem, relationUnit);
            string text = ((propertyData.sex != UnitSexType.Man) ? specialItem.dressWoman : specialItem.dressMan);
            if (text != "0")
            {
                Il2CppSystem.Collections.Generic.List<int> list = CommonTool.StrSplitIntList(text, '|');
                int[] array = new int[list.Count];
                int num = 0;
                Il2CppSystem.Collections.Generic.List<int>.Enumerator enumerator = list.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    int current = enumerator.Current;
                    array[num++] = current;
                }
                int id2 = array[UnityEngine.Random.Range(0, list.Count)];
                PortraitModelData portraitModelDataInModel = dramaNpc.GetPortraitModelDataInModel(id2);
                BattleModelHumanData modelHumanDataInModel = dramaNpc.GetModelHumanDataInModel(id2);
                worldUnit.data.SetModelData(portraitModelDataInModel, modelHumanDataInModel);
            }
            propertyData.beauty = roleDress.GetBeautyValue(propertyData.modelData);
            npcSpecial.ModifPropertyCreateEnd(worldUnit, specialItem);
            worldUnit.SetUnitLuck(specialItem);
            unitInfoData.SetPoint(npcSpecial.GetPoint(specialItem));
            return worldUnit;
        }

        public static void SetUnitRelation(this WorldUnitBase worldUnit, ConfNpcSpecialItem specialItem, WorldUnitBase relationUnit)
        {
            worldUnit.SetUnitRelation(specialItem.relationFix, relationUnit);
        }

        public static void SetUnitRelation(this WorldUnitBase worldUnit, string relationFix, WorldUnitBase relationUnit)
        {
            char[] separator = new char[1] { '|' };
            string[] array = relationFix.Split(separator);
            foreach (string text in array)
            {
                string[] array2 = text.Split('_');
                if (array2.Length <= 1)
                {
                    continue;
                }
                string text2 = array2[0];
                if (!(text2 == "addRelation"))
                {
                    if (text2 == "addIntimHate")
                    {
                        if (array2.Length == 3)
                        {
                            int friendCount = int.Parse(array2[1]);
                            int enemyCount = int.Parse(array2[2]);
                            WorldInitNPCTool.SetIntimHateUnit(worldUnit, WorldInitNPCTool.GetAllUnits(), friendCount, enemyCount);
                        }
                        else
                        {
                            Log.Debug("函数 " + text + " 参数不匹配");
                        }
                    }
                }
                else if (array2.Length == 4)
                {
                    WorldUnitBase worldUnitBase = null;
                    UnitRelationType relationType = (UnitRelationType)int.Parse(array2[1]);
                    switch (int.Parse(array2[2]))
                    {
                        case 1:
                            worldUnitBase = g.world.playerUnit;
                            break;
                        case 0:
                        case 2:
                            worldUnitBase = relationUnit;
                            break;
                        case 101:
                            worldUnitBase = WorldInitNPCTool.RandomRelationUnit(worldUnit, WorldInitNPCTool.GetAllUnits(), relationType);
                            break;
                    }
                    if (worldUnitBase == null)
                    {
                        Log.Debug("函数 " + text + " 捕获角色失败");
                        continue;
                    }
                    int addIntim = int.Parse(array2[3]);
                    WorldInitNPCTool.SetRelationUnit(worldUnit, worldUnitBase, relationType, addIntim);
                }
                else
                {
                    Log.Debug("函数 " + text + " 参数不匹配");
                }
            }
        }

        private static void SetUnitLuck(this WorldUnitBase worldUnit, ConfNpcSpecialItem specialItem)
        {
            string[] array = specialItem.featureFix.Split('|');
            foreach (string text in array)
            {
                char[] separator = new char[1] { '_' };
                string[] array2 = text.Split(separator);
                if (array2.Length > 1 && array2[0] == "addFeature")
                {
                    worldUnit.AddLuck(int.Parse(array2[1]));
                }
            }
        }

        public static void SetUnitBornLuck(this WorldUnitBase worldUnit, ConfNpcSpecialItem specialItem)
        {
            string[] array = specialItem.featureFix.Split('|');
            DataUnit.PropertyData propertyData = worldUnit.data.unitData.propertyData;
            List<DataUnit.LuckData> list = propertyData.bornLuck.ToList();
            string[] array2 = array;
            foreach (string text in array2)
            {
                char[] separator = new char[1] { '_' };
                string[] array3 = text.Split(separator);
                if (array3.Length > 1 && array3[0] == "addFeature")
                {
                    DataUnit.LuckData luckData = list[0];
                    WorldUnitLuckBase luck = worldUnit.GetLuck(luckData.id);
                    worldUnit.DestroyLuck(luck);
                    list.RemoveAt(0);
                    DataUnit.LuckData luckData2 = new DataUnit.LuckData();
                    luckData2.id = int.Parse(array3[1]);
                    list.Add(luckData2);
                    worldUnit.CreateLuck(luckData2);
                }
            }
            propertyData.bornLuck = new Il2CppReferenceArray<DataUnit.LuckData>(list.ToArray());
        }

        public static void SetInt(this WorldUnitBase unit, string key, int value)
        {
            unit.data.unitData.objData.SetString("淫女宫", key, value);
        }

        public static void SetStr(this WorldUnitBase unit, string key, string value)
        {
            unit.data.unitData.objData.SetString("淫女宫", key, value);
        }

        public static int GetInt(this WorldUnitBase unit, string key)
        {
            return unit.data.unitData.objData.GetInt("淫女宫", key);
        }

        public static string GetStr(this WorldUnitBase unit, string key)
        {
            return unit.data.unitData.objData.GetString("淫女宫", key);
        }

        public static void SendLetter(this WorldUnitBase unit, int id)
        {
            bool flag = false;
            string uUID = Tools.GetUUID();
            Il2CppSystem.Collections.Generic.Dictionary<string, DataWorld.World.NewTipData.GroupData>.Enumerator enumerator = g.data.dataWorld.data.newTip.allNewTip.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Key == "Email")
                {
                    flag = true;
                    g.data.dataWorld.data.newTip.allNewTip["Email"].openNewTip.Add(uUID);
                    break;
                }
            }
            if (!flag)
            {
                DataWorld.World.NewTipData.GroupData groupData = new DataWorld.World.NewTipData.GroupData();
                groupData.openNewTip = new Il2CppSystem.Collections.Generic.List<string>();
                groupData.openNewTip.Add(uUID);
                g.data.dataWorld.data.newTip.allNewTip.Add("Email", groupData);
            }
            g.data.dataWorld.data.allLetter.Add(new DataWorld.World.LetterData
            {
                soleID = uUID,
                sendUnitName = unit.data.unitData.propertyData.GetName(),
                sendUnitID = unit.data.unitData.unitID,
                month = g.world.run.roundMonth,
                state = 1,
                values = new Il2CppStringArray(0L),
                letterID = id
            });
        }

        public static void AddMonthLog(this WorldUnitBase npcUnit, string logKey, params object[] values)
        {
            if (values != null && values.Length != 0)
            {
                string[] array = new string[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    object obj = values[i];
                    if (obj is WorldUnitBase target)
                    {
                        array[i] = npcUnit.LogCall(target);
                    }
                    else if (obj is DataUnit.UnitInfoData unitInfoData)
                    {
                        array[i] = "@q_" + unitInfoData.propertyData.GetName() + "(" + npcUnit.Call(unitInfoData.unit) + ")|" + unitInfoData.unitID + "@";
                    }
                    else if (obj is string text)
                    {
                        array[i] = text;
                    }
                    else
                    {
                        array[i] = obj.ToString();
                    }
                }
                g.world.unitLog.AddLogData(npcUnit, logKey, array);
            }
            else
            {
                g.world.unitLog.AddLogData(npcUnit, logKey);
            }
        }

        public static void AddVitalLog(this WorldUnitBase npcUnit, string logKey, params object[] values)
        {
            if (values == null || values.Length == 0)
            {
                g.world.unitLog.AddVitalLogData(npcUnit, logKey);
                return;
            }
            string[] array = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                object obj = values[i];
                if (obj is WorldUnitBase target)
                {
                    array[i] = npcUnit.LogCall(target);
                }
                else if (obj is DataUnit.UnitInfoData unitInfoData)
                {
                    array[i] = "@q_" + unitInfoData.propertyData.GetName() + "(" + npcUnit.Call(unitInfoData.unit) + ")|" + unitInfoData.unitID + "@";
                }
                else if (obj is string text)
                {
                    array[i] = text;
                }
                else
                {
                    array[i] = obj.ToString();
                }
            }
            g.world.unitLog.AddVitalLogData(npcUnit, logKey, array);
        }

        public static string LogCall(this WorldUnitBase npcUnit, WorldUnitBase target)
        {
            return "@q_" + target.GetName() + "(" + npcUnit.Call(target) + ")|" + target.GetID() + "@";
        }

        public static string LogName(this WorldUnitBase target)
        {
            return "@q_" + target.GetName() + "|" + target.GetID() + "@";
        }

        public static void MoveToTarget(this WorldUnitBase unit, WorldUnitBase target, bool isTip = false)
        {
            WorldUnitAITool.MovePoint(unit, target);
        }

        public static void MoveTo(this WorldUnitBase unit, Vector2Int pos, bool isTip = false)
        {
            WorldUnitAITool.MovePoint(unit, pos);
        }

        public static void AddEffectEx(this WorldUnitBase unit, ConfRoleEffectItem effectItem)
        {
            int featureID = -1379079087;
            if (g.conf.roleCreateFeature.GetItem(featureID) != null)
            {
                g.conf.roleCreateFeature.DelItem(featureID);
            }
            g.conf.roleCreateFeature.AddItem(new ConfRoleCreateFeatureItem(featureID, 2, 0, 6, "1", 2, 1, "0", effectItem.id.ToString(), "0", "0", 0, "0", 0));
            Action action = delegate
            {
                try
                {
                    unit.CreateAction(new UnitActionLuckAdd(featureID));
                }
                catch (Exception ex)
                {
                    Log.Debug("AddEffectEx UnitActionLuckAdd发生异常： " + ex);
                }
            };
            if (SceneType.battle != null && effectItem.effectType == 101)
            {
                g.events.On(EBattleType.BattleExit, action, 1);
            }
            else
            {
                action();
            }
        }

        public static void AddEffect(this WorldUnitBase unit, string effectIdList)
        {
            foreach (int item in CommonTool.StrSplitInt(effectIdList, '|', ingoreZero: true))
            {
                ConfRoleEffectItem effectItem = g.conf.roleEffect.GetItem(item);
                if (effectItem == null)
                {
                    continue;
                }
                if (effectItem.effectType == 101)
                {
                    g.events.On(EBattleType.BattleExit, (Action)delegate
                    {
                        unit.AddEffect(effectItem.id, null);
                    }, 1);
                }
                else
                {
                    unit.AddEffect(effectItem.id, null);
                }
            }
        }

        public static string GetUnitDressString(this WorldUnitBase unit)
        {
            PortraitModelData modelData = unit.data.unitData.propertyData.modelData;
            return unit.data.dynUnitData.sex.value + "|" + modelData.hat + "|" + modelData.hair + "|" + modelData.hairFront + "|" + modelData.head + "|" + modelData.eyebrows + "|" + modelData.eyes + "|" + modelData.nose + "|" + modelData.mouth + "|" + modelData.body + "|" + modelData.back + "|" + modelData.forehead + "|" + modelData.faceFull + "|" + modelData.faceLeft + "|" + modelData.faceRight;
        }

        public static void ChangeDress(this WorldUnitBase unit, string dress)
        {
            PortraitModelData modelData = unit.data.unitData.propertyData.modelData;
            Il2CppStructArray<int> il2CppStructArray = CommonTool.StrSplitInt(dress, '|', ingoreZero: false);
            modelData.sex = il2CppStructArray[0];
            modelData.hat = il2CppStructArray[1];
            modelData.hair = il2CppStructArray[2];
            modelData.hairFront = il2CppStructArray[3];
            modelData.head = il2CppStructArray[4];
            modelData.eyebrows = il2CppStructArray[5];
            modelData.eyes = il2CppStructArray[6];
            modelData.nose = il2CppStructArray[7];
            modelData.mouth = il2CppStructArray[8];
            modelData.body = il2CppStructArray[9];
            modelData.back = il2CppStructArray[10];
            modelData.forehead = il2CppStructArray[11];
            modelData.faceFull = il2CppStructArray[12];
            modelData.faceLeft = il2CppStructArray[13];
            modelData.faceRight = il2CppStructArray[14];
            BattleModelHumanData battleModelData = unit.data.unitData.propertyData.battleModelData;
            battleModelData.back = modelData.back;
            battleModelData.body = modelData.body;
            battleModelData.hair = modelData.hair;
            battleModelData.hat = modelData.hat;
            unit.data.SetModelData(modelData, battleModelData);
        }
    }
}
