using System;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace MOD_LE2lAt
{
    internal class ActionMartialDataDeal
    {
        private static List<SkillData> skillDatas = null;
        private static int optionsPage = 1;
        private static int optionsSize = 10;

        public static void dramaShareSkills(DramaFunction __instance, string[] values)
        {
            WorldUnitBase playerUnit = g.world.playerUnit;
            WorldUnitBase unitRight = __instance.data.unitRight;
            if (values[2].Equals("share"))
            {
                if (values[3].Equals("all"))
                {
                    shareAllSkills(playerUnit, unitRight);
                    DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.shareSkill_1");
                    return;
                }
                MartialType typeByStr = getTypeByStr(values[3]);
                if (typeByStr != 0)
                {
                    if (skillDatas == null || skillDatas.Count == 0)
                    {
                        skillDatas = getEquipSkills(playerUnit, typeByStr);
                    }
                    int num = Convert.ToInt32(values[4]);
                    if (num + 1 > skillDatas.Count)
                    {
                        DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.shareSkill_2");
                        return;
                    }
                    shareSkill(playerUnit, unitRight, skillDatas[num], typeByStr);
                    skillDatas.Clear();
                }
            }
            else
            {
                if (!values[2].Equals("del"))
                {
                    return;
                }
                if (values[3].Equals("all"))
                {
                    DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.delSkill_0");
                    skillDatas = getEquipSkills(unitRight, MartialType.None);
                    for (int i = 0; i < skillDatas.Count; i++)
                    {
                        delSkill(unitRight, skillDatas[i]);
                    }
                    DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.delSkill_1");
                    skillDatas.Clear();
                    return;
                }
                MartialType typeByStr2 = getTypeByStr(values[3]);
                if (typeByStr2 != 0)
                {
                    if (skillDatas == null || skillDatas.Count == 0)
                    {
                        skillDatas = getEquipSkills(unitRight, typeByStr2);
                    }
                    int num2 = Convert.ToInt32(values[4]);
                    if (num2 + 1 > skillDatas.Count)
                    {
                        DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.delSkill_2");
                        return;
                    }
                    delSkill(unitRight, skillDatas[num2]);
                    skillDatas.Clear();
                }
            }
        }

        public static void shareAllSkills(WorldUnitBase unit, WorldUnitBase toUnit)
        {
            skillDatas = getEquipSkills(unit, MartialType.Ability);
            for (int i = 0; i < 8; i++)
            {
                try
                {
                    shareSkill(unit, toUnit, skillDatas[i], MartialType.Ability);
                }
                catch (Exception ex)
                {
                    ConsUtil.console(ex.Message);
                    ConsUtil.console(ex.StackTrace);
                }
            }
            skillDatas.Clear();
            skillDatas = getEquipSkills(unit, MartialType.Step);
            shareSkill(unit, toUnit, skillDatas[0], MartialType.Step);
            skillDatas.Clear();
            skillDatas = getEquipSkills(unit, MartialType.SkillLeft);
            shareSkill(unit, toUnit, skillDatas[0], MartialType.SkillLeft);
            skillDatas.Clear();
            skillDatas = getEquipSkills(unit, MartialType.SkillRight);
            shareSkill(unit, toUnit, skillDatas[0], MartialType.SkillRight);
            skillDatas.Clear();
            skillDatas = getEquipSkills(unit, MartialType.Ultimate);
            shareSkill(unit, toUnit, skillDatas[0], MartialType.Ultimate);
            skillDatas.Clear();
        }

        public static MartialType getTypeByStr(string s)
        {
            MartialType result = MartialType.None;
            if (s.Equals("ability"))
            {
                result = MartialType.Ability;
            }
            else if (s.Equals("step"))
            {
                result = MartialType.Step;
            }
            else if (s.Equals("skillLeft"))
            {
                result = MartialType.SkillLeft;
            }
            else if (s.Equals("skillRight"))
            {
                result = MartialType.SkillRight;
            }
            else if (s.Equals("ultimate"))
            {
                result = MartialType.Ultimate;
            }
            return result;
        }

        public static void delSkill(WorldUnitBase toUnit, SkillData skill)
        {
            DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.delSkill_0");
            toUnit.CreateAction(new UnitActionMartialUnequip(skill.martialData), isTip: true);
            toUnit.CreateAction(new UnitActionMartialDel(skill.martialData), isTip: true);
            DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.delSkill_1");
        }

        public static void dramaShareEquipAbilitys(WorldUnitBase unit, WorldUnitBase toUnit)
        {
            skillDatas = getEquipSkills(unit, MartialType.Ability);
            new UICustomDramaDyn(ModMain.mid() + 10).OpenUI();
        }

        public static void shareSkill(WorldUnitBase unit, WorldUnitBase toUnit, SkillData skill, MartialType type)
        {
            DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.shareSkill_0");
            Action<DataUnit.ActionMartialData> call = delegate (DataUnit.ActionMartialData item)
            {
                if (type == MartialType.Step || type == MartialType.SkillLeft || type == MartialType.SkillRight || type == MartialType.Ultimate)
                {
                    toUnit.CreateAction(new UnitActionMartialDel(item), isTip: true);
                }
            };
            List<SkillData> list = unequip(toUnit, type, call);
            DataProps.PropsData propsData = skill.martialData.data.Clone();
            Il2CppStructArray<int> il2CppStructArray = new Il2CppStructArray<int>(propsData.values.Length);
            for (int i = 0; i < propsData.values.Length; i++)
            {
                il2CppStructArray[i] = propsData.values[i];
            }
            propsData.values = il2CppStructArray;
            DataUnit.ActionMartialData actionMartialData = toUnit.data.unitData.StudyActionMartialProp(propsData);
            actionMartialData.exp = 999999;
            int num = toUnit.CreateAction(new UnitActionMartialEquip(actionMartialData, 0));
            if (num != 0 && num == 61)
            {
                if (toUnit.data.dynUnitData.abilityPoint.value < 6000)
                {
                    toUnit.data.dynUnitData.AddEffectBaseValues("abilityPoint", 1, 6666, isShowUI: false);
                }
                else
                {
                    toUnit.data.dynUnitData.AddEffectBaseValues("abilityPoint", 1, 800, isShowUI: false);
                }
                num = toUnit.CreateAction(new UnitActionMartialEquip(actionMartialData, 0));
            }
            ConsUtil.debug("NPC learning skill results:" + num);
            ConsUtil.debug("Number of NPC equipment skills:" + list.Count);
            if (num == 0)
            {
                if (type == MartialType.Ability)
                {
                    reEquipAbility(toUnit, list);
                }
                DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.shareSkill_1");
            }
        }

        public static List<SkillData> unequip(WorldUnitBase unit, MartialType type, Action<DataUnit.ActionMartialData> call)
        {
            List<SkillData> equipSkills = getEquipSkills(unit, type);
            for (int i = 0; i < equipSkills.Count; i++)
            {
                unit.CreateAction(new UnitActionMartialUnequip(equipSkills[i].martialData), isTip: true);
                call?.Invoke(equipSkills[i].martialData);
            }
            return equipSkills;
        }

        public static void reEquipAbility(WorldUnitBase toUnit, List<SkillData> npcEquipSkills)
        {
            int num = 1;
            for (int i = 0; i < npcEquipSkills.Count; i++)
            {
                try
                {
                    int num2 = toUnit.CreateAction(new UnitActionMartialEquip(npcEquipSkills[i].martialData, num), isTip: true);
                    if (num2 == 61)
                    {
                        if (toUnit.data.dynUnitData.abilityPoint.value < 6000)
                        {
                            toUnit.data.dynUnitData.AddEffectBaseValues("abilityPoint", 1, 6666, isShowUI: false);
                        }
                        else
                        {
                            toUnit.data.dynUnitData.AddEffectBaseValues("abilityPoint", 1, 800, isShowUI: false);
                        }
                        num2 = toUnit.CreateAction(new UnitActionMartialEquip(npcEquipSkills[i].martialData, num), isTip: true);
                    }
                    if (num2 == 63)
                    {
                        toUnit.CreateAction(new UnitActionMartialDel(npcEquipSkills[i].martialData), isTip: true);
                    }
                    if (num2 == 0)
                    {
                        num++;
                    }
                    ConsUtil.debug("The result of the npc putting the equipment back on is:" + num2 + ", equipidx:" + (num - 1));
                }
                catch (Exception ex)
                {
                    ConsUtil.console(ex.Message);
                    ConsUtil.console(ex.StackTrace);
                }
            }
        }

        public static void setOptions(UICustomDramaDyn dramaDyn, List<DataUnit.ActionMartialData> allMartialDatas)
        {
            for (int i = (optionsPage - 1) * optionsSize; i < optionsPage * optionsSize && i < allMartialDatas.Count; i++)
            {
                dramaDyn.dramaData.dialogueOptions[i] = (ModMain.mid() + 3010).ToString() ?? "";
                dramaDyn.dramaData.dialogueOptionsText[i] = allMartialDatas[i].data.soleID;
            }
        }

        public static string dramaStr(WorldUnitBase unit, List<DataUnit.ActionMartialData> equipMartialData, List<DataUnit.ActionMartialData> martialData)
        {
            string text = "The skills I have equipped are:";
            for (int i = 0; i < equipMartialData.Count; i++)
            {
                text = text + "{itemLink" + equipMartialData[i].data.soleID + "},";
            }
            text = text.Substring(0, text.Length - 1);
            string text2 = "The skills I learned are:";
            for (int j = 0; j < martialData.Count; j++)
            {
                text2 = text2 + "{itemLink" + martialData[j].data.soleID + "},";
            }
            text2 = text2.Substring(0, text2.Length - 1);
            Debug.Log("values:" + ConvertToJson.ListToJson(equipMartialData[0].data.values));
            Debug.Log("valuestr:" + equipMartialData[0].data.valuesStr);
            Debug.Log("_propsItem:" + equipMartialData[0].data._propsItem.type + ";" + equipMartialData[0].data._propsItem.className);
            Debug.Log("_propsItem:" + ConvertToJson.ToJson(equipMartialData[0].data._propsItem));
            return text + "\n" + text2;
        }

        public static List<SkillData> getEquipSkills(WorldUnitBase unit, MartialType type)
        {
            List<SkillData> list = new List<SkillData>();
            try
            {
                if (type == MartialType.None || type == MartialType.Ability)
                {
                    Il2CppStringArray abilitys = unit.data.unitData.abilitys;
                    for (int i = 0; i < abilitys.Length; i++)
                    {
                        string text = abilitys[i];
                        if (!string.IsNullOrEmpty(text))
                        {
                            DataUnit.ActionMartialData actionMartial = unit.data.unitData.GetActionMartial(text);
                            if (actionMartial != null)
                            {
                                SkillData item = new SkillData(actionMartial, MartialType.Ability, i);
                                list.Add(item);
                            }
                        }
                    }
                }
                if (type == MartialType.None || type == MartialType.Step)
                {
                    DataUnit.ActionMartialData actionMartial2 = unit.data.unitData.GetActionMartial(unit.data.unitData.step);
                    if (actionMartial2 != null)
                    {
                        SkillData item2 = new SkillData(actionMartial2, MartialType.Step, 0);
                        list.Add(item2);
                    }
                }
                if (type == MartialType.None || type == MartialType.SkillLeft)
                {
                    DataUnit.ActionMartialData actionMartial3 = unit.data.unitData.GetActionMartial(unit.data.unitData.skillLeft);
                    if (actionMartial3 != null)
                    {
                        SkillData item3 = new SkillData(actionMartial3, MartialType.SkillLeft, 0);
                        list.Add(item3);
                    }
                }
                if (type == MartialType.None || type == MartialType.SkillRight)
                {
                    DataUnit.ActionMartialData actionMartial4 = unit.data.unitData.GetActionMartial(unit.data.unitData.skillRight);
                    if (actionMartial4 != null)
                    {
                        SkillData item4 = new SkillData(actionMartial4, MartialType.SkillRight, 0);
                        list.Add(item4);
                    }
                }
                if (type == MartialType.None || type == MartialType.Ultimate)
                {
                    DataUnit.ActionMartialData actionMartial5 = unit.data.unitData.GetActionMartial(unit.data.unitData.ultimate);
                    if (actionMartial5 != null)
                    {
                        SkillData item5 = new SkillData(actionMartial5, MartialType.Ultimate, 0);
                        list.Add(item5);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return list;
        }

        public static List<DataUnit.ActionMartialData> allSkill(WorldUnitBase unit)
        {
            Il2CppSystem.Collections.Generic.Dictionary<string, DataUnit.ActionMartialData>.ValueCollection values = unit.data.unitData.allActionMartial.Values;
            List<DataUnit.ActionMartialData> list = new List<DataUnit.ActionMartialData>();
            if (values == null)
            {
                return list;
            }
            Il2CppSystem.Collections.Generic.Dictionary<string, DataUnit.ActionMartialData>.ValueCollection.Enumerator enumerator = values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DataUnit.ActionMartialData current = enumerator.Current;
                list.Add(current);
            }
            return list;
        }
    }
}
