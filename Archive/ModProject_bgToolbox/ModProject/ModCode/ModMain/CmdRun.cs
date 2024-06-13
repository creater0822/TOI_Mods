using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// using MOD_Tc85Yz; (Garbage mod)
using UnhollowerBaseLib;
using UnityEngine;

namespace MOD_bgToolbox
{
    public class CmdRun : MonoBehaviour
    {
        public bool isRun;
        public List<string> logs = new List<string>();
        private string p1;
        private string p2;
        private string p3;
        private string p4;

        public CmdRun(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Update()
        {
            try
            {
                if (!isRun)
                {
                    if (!(g.game == null) && g.ui != null && g.cache != null && g.ui.GetUI(UIType.Login) != null)
                    {
                        isRun = true;
                    }
                    return;
                }
                ModMain.OnUpdate();
                foreach (CmdItem allCmdItem in ModMain.allCmdItems)
                {
                    if (allCmdItem.key.IsKeyDown())
                    {
                        allCmdItem.Run();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(base.name + " Update " + ex.ToString());
            }
        }

        public void AddLog(string log)
        {
            Console.WriteLine(log);
            logs.Add(log);
            UICostItemTool.AddTipText(log);
            while (logs.Count > 200)
            {
                logs.RemoveAt(0);
            }
            UIBase uI = g.ui.GetUI(new UIType.UITypeBase("DaguiTool", UILayer.UI));
            if (uI != null)
            {
                UIDaguiTool component = uI.GetComponent<UIDaguiTool>();
                if ((bool)component)
                {
                    component.AddLogItem(log);
                }
            }
        }

        public DataProps.MartialData GetMartial(WorldUnitBase unit, string soleId)
        {
            if (soleId == "last")
            {
                if (ModMain.lastAddMartial == null)
                {
                    return null;
                }
                return ModMain.lastAddMartial.t2;
            }
            Il2CppSystem.Collections.Generic.List<DataProps.PropsData>.Enumerator enumerator = unit.data.unitData.propData.CloneAllProps().GetEnumerator();
            while (enumerator.MoveNext())
            {
                DataProps.PropsData current = enumerator.Current;
                if (current.propsType == DataProps.PropsDataType.Martial && current.soleID == soleId)
                {
                    return current.To<DataProps.MartialData>();
                }
            }
            return null;
        }

        public DataUnit.ActionMartialData GetSkill(WorldUnitBase unit, string soleId)
        {
            if (soleId == "last")
            {
                if (ModMain.lastStudySkill == null)
                {
                    return null;
                }
                return ModMain.lastStudySkill.t2;
            }
            return unit.data.unitData.GetActionMartial(soleId);
        }

        public DataUnit.ActionMartialData GetAbility(WorldUnitBase unit, string soleId)
        {
            if (soleId == "last")
            {
                if (ModMain.lastStudyAbility == null)
                {
                    return null;
                }
                return ModMain.lastStudyAbility.t2;
            }
            return unit.data.unitData.GetActionMartial(soleId);
        }

        public DataProps.PropsData GetElder(WorldUnitBase unit, string soleId)
        {
            if (soleId == "last")
            {
                if (ModMain.lastAddElder == null)
                {
                    return null;
                }
                return ModMain.lastAddElder.t2;
            }
            return unit.data.unitData.propData.GetProps(soleId);
        }

        public DataProps.PropsData GetRule(WorldUnitBase unit, string soleId)
        {
            if (soleId == "last")
            {
                if (ModMain.lastAddRule == null)
                {
                    return null;
                }
                return ModMain.lastAddRule.t2;
            }
            return unit.data.unitData.propData.GetProps(soleId);
        }

        public DataProps.PropsData GetArtifact(WorldUnitBase unit, string soleId)
        {
            if (soleId == "last")
            {
                if (ModMain.lastAddArtifact == null)
                {
                    return null;
                }
                return ModMain.lastAddArtifact.t2;
            }
            return unit.data.unitData.propData.GetProps(soleId);
        }

        public WorldUnitBase GetUnit(string unit)
        {
            WorldUnitBase worldUnitBase = null;
            if (unit == "player" || unit == g.world.playerUnit.data.unitData.unitID)
            {
                worldUnitBase = g.world.playerUnit;
            }
            else
            {
                switch (unit)
                {
                    case "unitA":
                        {
                            UIDramaDialogue uI2 = g.ui.GetUI<UIDramaDialogue>(UIType.DramaDialogue);
                            if (uI2 != null)
                            {
                                worldUnitBase = uI2.dramaData.unitLeft;
                            }
                            break;
                        }
                    case "unitB":
                        {
                            UIDramaDialogue uI3 = g.ui.GetUI<UIDramaDialogue>(UIType.DramaDialogue);
                            if (uI3 != null)
                            {
                                worldUnitBase = uI3.dramaData.unitRight;
                            }
                            break;
                        }
                    case "lookUnit":
                        {
                            UINPCInfo uI = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);
                            if (uI != null)
                            {
                                worldUnitBase = uI.unit;
                            }
                            break;
                        }
                    case "playerWife":
                        worldUnitBase = g.world.unit.GetUnit(g.world.playerUnit.data.unitData.relationData.married);
                        break;
                    default:
                        worldUnitBase = g.world.unit.GetUnit(unit);
                        break;
                }
            }
            ModMain.lastUnit = worldUnitBase;
            return worldUnitBase;
        }

        public string GetUnitName(WorldUnitBase unit)
        {
            if (unit == null)
            {
                return "No such person found";
            }
            string partName = g.conf.npcPartFitting.GetPartName(unit.data.unitData.unitID, g.world.playerUnit);
            string text = unit.data.unitData.propertyData.GetName();
            if (partName != "")
            {
                text = text + "(" + partName + ")";
            }
            return text;
        }

        public void Cmd(string[] para)
        {
            p1 = para[1];
            p2 = para[2];
            p3 = para[3];
            p4 = para[4];
            AddLog("implement: " + string.Join(" ", para));
            MethodInfo method = typeof(CmdRun).GetMethod(para[0]);
            if (method != null)
            {
                method.Invoke(this, null);
                return;
            }
            Console.WriteLine("Method not found");
            AddLog("Stay tuned!");
        }

        public void UpGrade()
        {
        }

        public void SetRoleGrade()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                result = 0;
            }
            int num = result / 3 + 1;
            int num2 = result % 3 + 1;
            Il2CppSystem.Collections.Generic.List<ConfTestAttrItem>.Enumerator enumerator = g.conf.testAttr._allConfList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ConfTestAttrItem current = enumerator.Current;
                if (current.grade == num && current.phase == num2)
                {
                    unit.data.dynUnitData.hpMax.baseValue = current.hp;
                    unit.data.dynUnitData.mpMax.baseValue = current.mp;
                    unit.data.dynUnitData.spMax.baseValue = current.sp;
                    unit.data.dynUnitData.moveSpeed.baseValue = current.moveSpeed;
                    unit.data.dynUnitData.attack.baseValue = current.attack;
                    unit.data.dynUnitData.defense.baseValue = current.defense;
                    unit.data.dynUnitData.basisBlade.baseValue = current.basBlade;
                    unit.data.dynUnitData.basisSpear.baseValue = current.basSpear;
                    unit.data.dynUnitData.basisSword.baseValue = current.basSword;
                    unit.data.dynUnitData.basisFist.baseValue = current.basFist;
                    unit.data.dynUnitData.basisPalm.baseValue = current.basPalm;
                    unit.data.dynUnitData.basisFinger.baseValue = current.basFinger;
                    unit.data.dynUnitData.basisFire.baseValue = current.basisFire;
                    unit.data.dynUnitData.basisFroze.baseValue = current.basisFroze;
                    unit.data.dynUnitData.basisThunder.baseValue = current.basisThunder;
                    unit.data.dynUnitData.basisWind.baseValue = current.basisWind;
                    unit.data.dynUnitData.basisEarth.baseValue = current.basisEarth;
                    unit.data.dynUnitData.basisWood.baseValue = current.basisWood;
                    unit.data.dynUnitData.phycicalFree.baseValue = current.phycicalFree;
                    unit.data.dynUnitData.magicFree.baseValue = current.magicFree;
                    unit.data.dynUnitData.crit.baseValue = current.crit;
                    unit.data.dynUnitData.guard.baseValue = current.guard;
                    unit.data.dynUnitData.critValue.baseValue = current.critValue;
                    unit.data.dynUnitData.guardValue.baseValue = current.guardValue;
                    unit.data.dynUnitData.abilityPoint.baseValue = current.abilityPoint;
                    unit.data.dynUnitData.gradeID.baseValue = g.conf.roleGrade.GetGradeItem(current.grade, current.phase).id;
                    unit.data.dynUnitData.exp.baseValue = g.conf.roleGrade.GetGradeItem(current.grade, current.phase).exp;
                    AddLog("execution succeed");
                    return;
                }
            }
            AddLog("Execution failed: Unable to find input realm:" + result + " " + num + "-" + num2);
        }

        public void SetFate()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                result = unit.data.dynUnitData.GetGrade();
            }
            if (!int.TryParse(p3, out var result2))
            {
                AddLog("Execution failed: The entered luck ID is illegal:" + p3);
                return;
            }
            ConfFateFeatureItem item = g.conf.fateFeature.GetItem(result2);
            if (item == null)
            {
                AddLog("Execution failed: The entered luck id does not exist:" + p3);
                return;
            }
            DelFate(unit, result);
            Il2CppSystem.Collections.Generic.Dictionary<int, DataWorld.World.PlayerLogData.GradeData> dictionary = ((!(unit.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID)) ? unit.data.unitData.npcUpGrade : g.data.world.playerLog.upGrade);
            DataWorld.World.PlayerLogData.GradeData gradeData = new DataWorld.World.PlayerLogData.GradeData();
            gradeData.quality = 5;
            dictionary.Add(result, gradeData);
            UnitActionLuckAdd unitActionLuckAdd = new UnitActionLuckAdd(item.id)
            {
                fateFeatureGrade = result
            };
            unitActionLuckAdd.Init(unit);
            unit.CreateAction(unitActionLuckAdd);
            if (unit != g.world.playerUnit && unit.data.unitData.heart.IsHeroes())
            {
                unit.data.unitData.heart.InitSkillHeart(unit);
            }
            AddLog("execution succeed!");
        }

        private void DelFate(WorldUnitBase unit, int grade)
        {
            Il2CppSystem.Collections.Generic.Dictionary<int, DataWorld.World.PlayerLogData.GradeData> dictionary = ((!(unit.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID)) ? unit.data.unitData.npcUpGrade : g.data.world.playerLog.upGrade);
            if (!dictionary.ContainsKey(grade))
            {
                return;
            }
            DataWorld.World.PlayerLogData.GradeData gradeData = dictionary[grade];
            dictionary.Remove(grade);
            ConfFateFeatureItem item = g.conf.fateFeature.GetItem(gradeData.luck);
            WorldUnitLuckBase luck = g.world.playerUnit.GetLuck(gradeData.luck);
            if (luck != null)
            {
                unit.CreateAction(new UnitActionLuckDel(luck));
            }
            Il2CppSystem.Collections.Generic.List<ConfFateFeatureItem> groupFeatureItems = g.conf.fateFeature.GetGroupFeatureItems(item.group);
            if (groupFeatureItems.Count <= 0)
            {
                return;
            }
            ConfFateFeatureItem confFateFeatureItem = null;
            Il2CppSystem.Collections.Generic.List<ConfFateFeatureItem>.Enumerator enumerator = groupFeatureItems.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ConfFateFeatureItem current = enumerator.Current;
                if (g.conf.fateFeature.GetItemIndex(current.id) < g.conf.fateFeature.GetItemIndex(gradeData.luck) && (confFateFeatureItem == null || confFateFeatureItem.id < current.id))
                {
                    confFateFeatureItem = current;
                }
            }
            if (confFateFeatureItem != null)
            {
                ConfRoleCreateFeatureItem item2 = g.conf.roleCreateFeature.GetItem(groupFeatureItems[groupFeatureItems.Count - 1].id);
                DataUnit.LuckData luckData = new DataUnit.LuckData();
                luckData.id = groupFeatureItems[groupFeatureItems.Count - 1].id;
                luckData.duration = int.Parse(item2.duration);
                unit.data.unitData.propertyData.AddAddLuck(luckData);
                unit.CreateLuck(luckData);
            }
        }

        public void AddLuck()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered luck ID is illegal:" + p2);
                return;
            }
            ConfRoleCreateFeatureItem item = g.conf.roleCreateFeature.GetItem(result);
            if (item == null)
            {
                AddLog("Execution failed: The entered luck id does not exist:" + p2);
                return;
            }
            UnitActionLuckAdd unitActionLuckAdd = new UnitActionLuckAdd(result);
            int num = unit.CreateAction(unitActionLuckAdd);
            if (unitActionLuckAdd.luck != null)
            {
                if (item.type == 1 || item.type == 11)
                {
                    List<DataUnit.LuckData> list = new List<DataUnit.LuckData>(unit.data.unitData.propertyData.bornLuck);
                    list.Add(unitActionLuckAdd.luckData);
                    unit.data.unitData.propertyData.bornLuck = list.ToArray();
                }
                if (unit.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID)
                {
                    g.ui.OpenUI<UIGetReward>(UIType.GetReward).UpdateLuck(unitActionLuckAdd.luck.luckData);
                }
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed:" + num);
            }
        }

        public void DelLuck()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var fateId))
            {
                AddLog("Execution failed: The entered Qiyun ID is illegal:" + p2);
                return;
            }
            if (g.conf.roleCreateFeature.GetItem(fateId) == null)
            {
                AddLog("Execution failed: The entered luck id does not exist:" + p2);
                return;
            }
            int num = 1;
            int num2 = 0;
            WorldUnitLuckBase luck = unit.GetLuck(fateId);
            if (luck != null)
            {
                num2 = unit.CreateAction(new UnitActionLuckDel(luck));
                if (num2 == 0)
                {
                    List<DataUnit.LuckData> list = new List<DataUnit.LuckData>(unit.data.unitData.propertyData.bornLuck);
                    DataUnit.LuckData luckData = list.Find((DataUnit.LuckData v) => v.id == fateId);
                    if (luckData != null)
                    {
                        list.Remove(luckData);
                    }
                    unit.data.unitData.propertyData.bornLuck = list.ToArray();
                    num--;
                }
            }
            Il2CppSystem.Collections.Generic.Dictionary<int, DataWorld.World.PlayerLogData.GradeData> dictionary = ((!(unit.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID)) ? unit.data.unitData.npcUpGrade : g.data.world.playerLog.upGrade);
            List<int> list2 = new List<int>();
            Il2CppSystem.Collections.Generic.Dictionary<int, DataWorld.World.PlayerLogData.GradeData>.KeyCollection.Enumerator enumerator = dictionary.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int current = enumerator.Current;
                if (dictionary[current].luck == fateId)
                {
                    list2.Add(current);
                }
            }
            foreach (int item in list2)
            {
                dictionary.Remove(item);
                num--;
            }
            if (num < 1)
            {
                AddLog("execution succeed");
            }
            else
            {
                AddLog("Execution failed:" + num2);
            }
        }

        public void AddAttr()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p3, out var result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p3);
                return;
            }
            Il2CppSystem.Collections.Generic.IReadOnlyList<DynInt> values = unit.data.dynUnitData.GetValues(p2);
            int num = 0;
            while (true)
            {
                try
                {
                    values[num++].baseValue += result;
                }
                catch (Exception)
                {
                    break;
                }
            }
            AddLog("execution succeed!");
        }

        public void FixHeart1()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.taoistHeart.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result2))
            {
                result2 = 0;
            }
            if (!int.TryParse(p4, out var result3))
            {
                result3 = 0;
            }
            unit.data.unitData.heart.state = DataUnit.TaoistHeart.HeartState.Complete;
            unit.data.unitData.heart.confID = result;
            unit.data.unitData.heart.hp = 100;
            unit.data.unitData.heart.seedGrowCool = result2;
            unit.data.unitData.heart.heartEffectHP = 100;
            if (result3 != 0)
            {
                unit.data.unitData.heart.heroesSkillGroupID = result3;
            }
            AddLog("execution succeed!");
        }

        public void FixHeart2()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.taoistSeed.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result2))
            {
                result2 = 0;
            }
            unit.data.unitData.heart.state = DataUnit.TaoistHeart.HeartState.Seed;
            unit.data.unitData.heart.confID = result;
            unit.data.unitData.heart.hp = 100;
            unit.data.unitData.heart.seedNeed = result2;
            unit.data.unitData.heart.heartEffectHP = 100;
            AddLog("execution succeed!");
        }

        public void FixHeart3()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.taoistHeart.GetItem(result) == null && result != 0)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            unit.data.unitData.heart.state = DataUnit.TaoistHeart.HeartState.Empty;
            unit.data.unitData.heart.confID = result;
            unit.data.unitData.heart.hp = 100;
            unit.data.unitData.heart.seedNeed = 0;
            unit.data.unitData.heart.heartEffectHP = 100;
            if (result != 0)
            {
                unit.data.unitData.heart.heartEffectLevel = CommonTool.GetMinValue(g.conf.taoistHeartEffect.GetItemInID(result), (Func<ConfTaoistHeartEffectItem, float>)((ConfTaoistHeartEffectItem v) => v.level)).level;
                unit.data.unitData.heart.heartEffectLastConfID = result;
                unit.data.unitData.heart.lastAttackUnitID = "";
                unit.data.unitData.heart.SetEmpty(unit, ignoreFail: false);
            }
            AddLog("execution succeed!");
        }

        public void AddExp()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p3);
                return;
            }
            if (unit.data.dynUnitData.curGrade >= 10 && result > 0)
            {
                unit.data.unitData.propertyData.AddFixExp(result);
            }
            else
            {
                unit.data.unitData.propertyData.AddExp(result);
            }
            AddLog("execution succeed!");
        }

        public void Recover()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            unit.data.dynUnitData.hp.baseValue += unit.data.dynUnitData.hpMax.value;
            unit.data.dynUnitData.mp.baseValue += unit.data.dynUnitData.mpMax.value;
            unit.data.dynUnitData.sp.baseValue += unit.data.dynUnitData.spMax.value;
            unit.data.dynUnitData.health.baseValue += unit.data.dynUnitData.healthMax.value;
            unit.data.dynUnitData.energy.baseValue += unit.data.dynUnitData.energyMax.value;
            unit.data.dynUnitData.mood.baseValue += unit.data.dynUnitData.moodMax.value;
            AddLog("execution succeed!");
        }

        public void AKillB()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            WorldUnitBase unit2 = GetUnit(p2);
            if (unit2 == null)
            {
                AddLog("Execution failed: Unable to get unit b:" + p2);
                return;
            }
            int num = unit.CreateAction(new UnitActionRoleKill(unit2, isMercy: false));
            if (num == 0)
            {
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed!" + num);
            }
        }

        public void FixName()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (p2.Length < 1)
            {
                AddLog("Execution failed: name must be at least 1 character:" + p2);
                return;
            }
            string text = p2.Substring(0, 1);
            string text2 = p2.Substring(1, p2.Length - 1);
            unit.data.unitData.propertyData.name = new string[2] { text, text2 };
            AddLog("execution succeed!");
        }

        public void AddHeartHp()
        {
            WorldUnitBase unit = GetUnit(p1);
            int result;
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
            }
            else if (!int.TryParse(p2, out result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p2);
            }
            else
            {
                unit.data.unitData.heart.AddHeartEffectHP(unit, unit, result);
            }
        }

        public void AHeartB()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            WorldUnitBase unit2 = GetUnit(p2);
            if (unit2 == null)
            {
                AddLog("Execution failed: Unable to get unit b:" + p2);
                return;
            }
            UnitActionSetTaoistHeart action = new UnitActionSetTaoistHeart(unit2, unit.data.unitData.heart.HeartConf());
            int num = unit.CreateAction(action);
            if (num == 0)
            {
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed!" + num);
            }
        }

        public void AddTitle()
        {
            WorldUnitBase unit = GetUnit(p1);
            int result;
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
            }
            else if (!int.TryParse(p2, out result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
            }
            else if (g.conf.appellationTitle.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
            }
            else if (unit.appellation.AddAppellationID(result))
            {
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed!");
            }
        }

        public void SetSchoolPost()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (unit.data.school == null)
            {
                AddLog("Execution failed:" + unit.data.unitData.propertyData.GetName() + "No sect:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The position id is illegal:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result2))
            {
                result2 = 1;
            }
            SchoolPostType schoolPostType = (SchoolPostType)result;
            SchoolDepartmentType schoolDepartmentType = (SchoolDepartmentType)result2;
            MapBuildSchool school = unit.data.school;
            WorldUnitBase worldUnitBase = null;
            switch (schoolPostType)
            {
                case SchoolPostType.SchoolMain:
                    worldUnitBase = g.world.unit.GetUnit(school.buildData.npcSchoolMain);
                    break;
                case SchoolPostType.BigElders:
                    {
                        DataBuildSchool.PostData.PostBigElders bigElders = school.buildData.postData.GetBigElders(schoolDepartmentType);
                        if (bigElders != null)
                        {
                            worldUnitBase = g.world.unit.GetUnit(bigElders.unitData.unitID);
                        }
                        break;
                    }
                case SchoolPostType.Elders:
                    worldUnitBase = g.world.unit.GetUnit(school.buildData.postData.postElders[schoolDepartmentType].unitData.unitID);
                    break;
                case SchoolPostType.Inherit:
                    if (school.buildData.postData.postElders[schoolDepartmentType].manageInherit.Count > 0)
                    {
                        worldUnitBase = g.world.unit.GetUnit(school.buildData.postData.postElders[schoolDepartmentType].manageInherit[0]);
                    }
                    break;
                case SchoolPostType.In:
                    if (school.buildData.postData.postElders[schoolDepartmentType].manageIn.Count > 0)
                    {
                        worldUnitBase = g.world.unit.GetUnit(school.buildData.postData.postElders[schoolDepartmentType].manageIn[0]);
                    }
                    break;
                case SchoolPostType.Out:
                    if (school.buildData.postData.postElders[schoolDepartmentType].manageOut.Count > 0)
                    {
                        worldUnitBase = g.world.unit.GetUnit(school.buildData.postData.postElders[schoolDepartmentType].manageOut[0]);
                    }
                    break;
            }
            if (worldUnitBase != null && worldUnitBase.data.unitData.unitID != unit.data.unitData.unitID)
            {
                worldUnitBase.CreateAction(new UnitActionSchoolExit());
            }
            UnitActionSchoolSetPostType unitActionSchoolSetPostType = new UnitActionSchoolSetPostType(unit.data.school.buildData.id, schoolPostType, schoolDepartmentType);
            unitActionSchoolSetPostType.Init(unit);
            unitActionSchoolSetPostType.isTest = true;
            int num = unitActionSchoolSetPostType.IsCreate(isTip: true);
            if (num == 0)
            {
                unit.CreateAction(unitActionSchoolSetPostType);
                if (result == 3)
                {
                    unit.data.school.GetBuildSub<MapBuildSchoolTaskHall>().SetDepartmentElder(schoolDepartmentType, unit.data.unitData.unitID);
                }
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed!" + num);
            }
        }

        public void JoinSchool()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (unit.data.school != null)
            {
                AddLog("Execution failed:" + unit.data.unitData.propertyData.GetName() + "There is already a sect：" + p1);
                return;
            }
            if (g.world.build.GetBuild<MapBuildSchool>(p2) == null)
            {
                AddLog("Execution failed: Unable to obtain sect:" + p2);
                return;
            }
            UnitActionSchoolJoin action = new UnitActionSchoolJoin(p2, SchoolPostType.Out, SchoolDepartmentType.None)
            {
                isTest = true
            };
            int num = unit.CreateAction(action, isTip: true);
            if (num == 0)
            {
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed!" + num);
            }
        }

        public void ExitSchool()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (unit.data.school == null)
            {
                AddLog("Execution failed:" + unit.data.unitData.propertyData.GetName() + "No sect:" + p1);
                return;
            }
            UnitActionSchoolExit action = new UnitActionSchoolExit();
            int num = unit.CreateAction(action, isTip: true);
            if (num == 0)
            {
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed!" + num);
            }
        }

        public void FixSchoolAttr()
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(p1);
            if (build == null)
            {
                AddLog("Execution failed: Unable to obtain sect:" + p1);
                return;
            }
            int result = 0;
            if (p2 != "10" && p2 != "11")
            {
                if (!int.TryParse(p3, out result))
                {
                    AddLog("Execution failed: The number entered is illegal:" + p3);
                    return;
                }
            }
            else if (p3.Length < 1)
            {
                AddLog("Execution failed: The entered name is at least 1 character:" + p3);
                return;
            }
            if (p2 == "1")
            {
                build.buildData.manorData.mainManor.AddStable(result);
            }
            else if (p2 == "2")
            {
                build.buildData.propertyData.AddProsperous(result);
            }
            else if (p2 == "3")
            {
                build.buildData.AddTotalMember(build, result);
            }
            else if (p2 == "4")
            {
                build.buildData.AddReputation(result);
            }
            else if (p2 == "5")
            {
                build.buildData.propertyData.AddLoyal(result);
            }
            else if (p2 == "7")
            {
                build.buildData.AddMoney(result);
            }
            else if (p2 == "8")
            {
                build.buildData.propertyData.AddMedicina(result);
            }
            else if (p2 == "9")
            {
                build.buildData.propertyData.AddMine(result);
            }
            else if (p2 == "10")
            {
                Il2CppSystem.Collections.Generic.List<MapBuildSchool>.Enumerator enumerator = build.GetAllSchools(isIncludeSelf: true).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    enumerator.Current.buildData.propertyData.fixName = p3;
                }
            }
            else if (p2 == "11")
            {
                build.buildData.propertyData.fixAreaName = p3;
            }
            AddLog("execution succeed!");
        }

        public void FixSchoolSkill()
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(p1);
            if (build == null)
            {
                AddLog("Execution failed: Unable to obtain sect:" + p1);
                return;
            }
            List<string> list = new List<string>(p2.Split(','));
            Console.WriteLine("Old data:" + build.schoolData.buildSchoolData.values[0]);
            BuildSchoolData buildSchoolData = build.schoolData.buildSchoolData;
            SchoolInitScaleData schoolInitScaleData = CommonTool.JsonToObject<SchoolInitScaleData>(buildSchoolData.values[0]);
            schoolInitScaleData.basTypes = new Il2CppSystem.Collections.Generic.List<string>();
            build.buildData.propertyData.fixAddBasTypes = new Il2CppSystem.Collections.Generic.List<string>();
            MapBuildSchoolLibrary.Data data = build.GetBuildSub<MapBuildSchoolLibrary>().data;
            data.weaponType.Clear();
            data.magicType.Clear();
            foreach (string item in list)
            {
                schoolInitScaleData.basTypes.Add(item);
                DataStruct<bool, int, int> basTypeID = g.conf.roleEffect.GetBasTypeID(item);
                if (basTypeID != null)
                {
                    if (basTypeID.t1)
                    {
                        data.weaponType.Add(item);
                    }
                    else
                    {
                        data.magicType.Add(item);
                    }
                }
            }
            buildSchoolData.SetData(schoolInitScaleData);
            Console.WriteLine("New data:" + build.schoolData.buildSchoolData.values[0]);
            AddLog("execution succeed!");
        }

        public void FixSchoolUseSkill()
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(p1);
            if (build == null)
            {
                AddLog("Execution failed: Unable to obtain sect:" + p1);
                return;
            }
            List<string> list = new List<string>(p2.Split(','));
            MapBuildSchoolLibrary.Data data = build.GetBuildSub<MapBuildSchoolLibrary>().data;
            data.weaponType.Clear();
            data.magicType.Clear();
            foreach (string item in list)
            {
                DataStruct<bool, int, int> basTypeID = g.conf.roleEffect.GetBasTypeID(item);
                if (basTypeID != null)
                {
                    if (basTypeID.t1)
                    {
                        data.weaponType.Add(item);
                    }
                    else
                    {
                        data.magicType.Add(item);
                    }
                }
            }
            AddLog("execution succeed!");
        }

        public void FixSchoolFate()
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(p1);
            if (build == null)
            {
                AddLog("Execution failed: Unable to obtain sect:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered sect's fate-defying change ID is illegal:" + p2);
                return;
            }
            if (g.conf.schoolFate.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered sect's name change ID does not exist:" + p2);
                return;
            }
            Console.WriteLine("old data" + build.buildData.propertyData.fixFateID);
            build.buildData.propertyData.fixFateID = result;
            Console.WriteLine("new data" + build.buildData.propertyData.fixFateID);
            AddLog("execution succeed!");
        }

        public void FixSchoolTenet()
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(p1);
            if (build == null)
            {
                AddLog("Execution failed: Unable to obtain sect:" + p1);
                return;
            }
            List<int> list = new List<int>();
            string[] array = p2.Split(',');
            if (array.Length != 2)
            {
                AddLog("Execution failed: The number of objectives entered must be 2:" + p2);
                return;
            }
            string[] array2 = array;
            foreach (string text in array2)
            {
                if (!int.TryParse(text, out var result))
                {
                    AddLog("Execution failed: The entered id is illegal:" + text + "/" + p2);
                    return;
                }
                if (g.conf.schoolSlogan.GetItem(result) == null)
                {
                    AddLog("Execution failed: The entered id does not exist:" + text + "/" + p2);
                    return;
                }
                list.Add(result);
            }
            Il2CppSystem.Collections.Generic.List<MapBuildSchool>.Enumerator enumerator = build.GetAllSchools(isIncludeSelf: true).GetEnumerator();
            while (enumerator.MoveNext())
            {
                MapBuildSchool current = enumerator.Current;
                current.buildData.propertyData.fixSloganItem1Type1 = list[0];
                current.buildData.propertyData.fixSloganItem1Type2 = list[1];
            }
            AddLog("execution succeed!");
        }

        public void FixSchoolGauge()
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(p1);
            if (build == null)
            {
                AddLog("Execution failed: Unable to obtain sect:" + p1);
                return;
            }
            List<int> list = new List<int>();
            string[] array = p2.Split(',');
            if (array.Length != 2)
            {
                AddLog("Execution failed: The number of gate rules entered must be 2:" + p2);
                return;
            }
            string[] array2 = array;
            foreach (string text in array2)
            {
                if (!int.TryParse(text, out var result))
                {
                    AddLog("Execution failed: The entered id is illegal:" + text + "/" + p2);
                    return;
                }
                if (g.conf.schoolSlogan.GetItem(result) == null)
                {
                    AddLog("Execution failed: The entered id does not exist:" + text + "/" + p2);
                    return;
                }
                list.Add(result);
            }
            Il2CppSystem.Collections.Generic.List<MapBuildSchool>.Enumerator enumerator = build.GetAllSchools(isIncludeSelf: true).GetEnumerator();
            while (enumerator.MoveNext())
            {
                MapBuildSchool current = enumerator.Current;
                current.buildData.propertyData.fixSloganItem2Type1 = list[0];
                current.buildData.propertyData.fixSloganItem2Type2 = list[1];
            }
            AddLog("execution succeed!");
        }

        public void FixSchoolStand()
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(p1);
            if (build == null)
            {
                AddLog("Execution failed: Unable to obtain sect:" + p1);
                return;
            }
            int fixStand = ((!(p2 == "down")) ? 1 : 2);
            Il2CppSystem.Collections.Generic.List<MapBuildSchool>.Enumerator enumerator = build.GetAllSchools(isIncludeSelf: true).GetEnumerator();
            while (enumerator.MoveNext())
            {
                MapBuildSchool current = enumerator.Current;
                current.buildData.propertyData.fixStand = fixStand;
                MapBuildSchool.UpdateSchoolRoundDecorate(current);
            }
            DramaFunction.UpdateMapAllUI();
            AddLog("execution succeed!");
        }

        public void SetRelation()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            WorldUnitBase unit2 = GetUnit(p2);
            if (unit2 == null)
            {
                AddLog("Execution failed: Unable to get unit b:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p3);
                return;
            }
            UnitRelationType unitRelationType = (UnitRelationType)result;
            if (unitRelationType < UnitRelationType.Parent || unitRelationType > UnitRelationType.Student)
            {
                AddLog("Execution failed: The entered relationship is illegal:" + p3);
                return;
            }
            switch (unitRelationType)
            {
                case UnitRelationType.Parent:
                    if (!unit.data.unitData.relationData.children.Contains(unit2.data.unitData.unitID))
                    {
                        unit.data.unitData.relationData.children.Add(unit2.data.unitData.unitID);
                    }
                    unit2.data.unitData.relationData.parent[0] = unit.data.unitData.unitID;
                    break;
                case UnitRelationType.Children:
                    if (!unit2.data.unitData.relationData.children.Contains(unit.data.unitData.unitID))
                    {
                        unit2.data.unitData.relationData.children.Add(unit.data.unitData.unitID);
                    }
                    unit.data.unitData.relationData.parent[0] = unit2.data.unitData.unitID;
                    break;
                case UnitRelationType.ChildrenPrivate:
                    if (!unit2.data.unitData.relationData.childrenPrivate.Contains(unit.data.unitData.unitID))
                    {
                        unit2.data.unitData.relationData.childrenPrivate.Add(unit.data.unitData.unitID);
                    }
                    unit.data.unitData.relationData.parent[0] = unit2.data.unitData.unitID;
                    break;
                case UnitRelationType.Brother:
                    if (!unit.data.unitData.relationData.brother.Contains(unit2.data.unitData.unitID))
                    {
                        unit.data.unitData.relationData.brother.Add(unit2.data.unitData.unitID);
                    }
                    if (!unit2.data.unitData.relationData.brother.Contains(unit.data.unitData.unitID))
                    {
                        unit2.data.unitData.relationData.brother.Add(unit.data.unitData.unitID);
                    }
                    break;
                case UnitRelationType.ParentBack:
                    if (!unit.data.unitData.relationData.childrenBack.Contains(unit2.data.unitData.unitID))
                    {
                        unit.data.unitData.relationData.childrenBack.Add(unit2.data.unitData.unitID);
                    }
                    if (!unit2.data.unitData.relationData.parentBack.Contains(unit.data.unitData.unitID))
                    {
                        unit2.data.unitData.relationData.parentBack.Add(unit.data.unitData.unitID);
                    }
                    break;
                case UnitRelationType.ChildrenBack:
                    if (!unit.data.unitData.relationData.parentBack.Contains(unit2.data.unitData.unitID))
                    {
                        unit.data.unitData.relationData.parentBack.Add(unit2.data.unitData.unitID);
                    }
                    if (!unit2.data.unitData.relationData.childrenBack.Contains(unit.data.unitData.unitID))
                    {
                        unit2.data.unitData.relationData.childrenBack.Add(unit.data.unitData.unitID);
                    }
                    break;
                case UnitRelationType.BrotherBack:
                    if (!unit.data.unitData.relationData.brotherBack.Contains(unit2.data.unitData.unitID))
                    {
                        unit.data.unitData.relationData.brotherBack.Add(unit2.data.unitData.unitID);
                    }
                    if (!unit2.data.unitData.relationData.brotherBack.Contains(unit.data.unitData.unitID))
                    {
                        unit2.data.unitData.relationData.brotherBack.Add(unit.data.unitData.unitID);
                    }
                    break;
                case UnitRelationType.Married:
                    unit.data.unitData.relationData.married = unit2.data.unitData.unitID;
                    unit2.data.unitData.relationData.married = unit.data.unitData.unitID;
                    break;
                case UnitRelationType.Lover:
                    if (!unit.data.unitData.relationData.lover.Contains(unit2.data.unitData.unitID))
                    {
                        unit.data.unitData.relationData.lover.Add(unit2.data.unitData.unitID);
                    }
                    if (!unit2.data.unitData.relationData.lover.Contains(unit.data.unitData.unitID))
                    {
                        unit2.data.unitData.relationData.lover.Add(unit.data.unitData.unitID);
                    }
                    break;
                case UnitRelationType.Master:
                    if (!unit.data.unitData.relationData.student.Contains(unit2.data.unitData.unitID))
                    {
                        unit.data.unitData.relationData.student.Add(unit2.data.unitData.unitID);
                    }
                    if (!unit2.data.unitData.relationData.master.Contains(unit.data.unitData.unitID))
                    {
                        unit2.data.unitData.relationData.master.Add(unit.data.unitData.unitID);
                    }
                    break;
                case UnitRelationType.Student:
                    if (!unit.data.unitData.relationData.master.Contains(unit2.data.unitData.unitID))
                    {
                        unit.data.unitData.relationData.master.Add(unit2.data.unitData.unitID);
                    }
                    if (!unit2.data.unitData.relationData.student.Contains(unit.data.unitData.unitID))
                    {
                        unit2.data.unitData.relationData.student.Add(unit.data.unitData.unitID);
                    }
                    break;
            }
            unit.data.UpdateAllUnitRelation();
            unit2.data.UpdateAllUnitRelation();
            AddLog("execution succeed!");
        }

        public void SetOneLove()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            WorldUnitBase unit2 = GetUnit(p2);
            if (unit2 == null)
            {
                AddLog("Execution failed: Unable to get unit b:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p3);
                return;
            }
            unit.data.unitData.relationData.AddIntim(unit2.data.unitData.unitID, result, 0);
            unit2.data.unitData.relationData.AddIntim(unit.data.unitData.unitID, result, 0);
            AddLog("execution succeed!");
        }

        public void SetTwoLove()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            WorldUnitBase unit2 = GetUnit(p2);
            if (unit2 == null)
            {
                AddLog("Execution failed: Unable to get unit b:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p3);
                return;
            }
            unit.data.unitData.relationData.AddIntim(unit2.data.unitData.unitID, result, 0);
            AddLog("execution succeed!");
        }

        public void SetOneHate()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            WorldUnitBase unit2 = GetUnit(p2);
            if (unit2 == null)
            {
                AddLog("Execution failed: Unable to get unit b:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p3);
                return;
            }
            unit.data.unitData.relationData.AddHate(unit2.data.unitData.unitID, result, 0);
            unit2.data.unitData.relationData.AddHate(unit.data.unitData.unitID, result, 0);
            AddLog("execution succeed!");
        }

        public void SetTwoHate()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            WorldUnitBase unit2 = GetUnit(p2);
            if (unit2 == null)
            {
                AddLog("Execution failed: Unable to get unit b:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p3);
                return;
            }
            unit.data.unitData.relationData.AddHate(unit2.data.unitData.unitID, result, 0);
            AddLog("execution succeed!");
        }

        public void MartialBookMax()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataProps.MartialData martial = GetMartial(unit, p2);
            if (martial == null)
            {
                AddLog("Execution failed: Unable to obtain unit's cheats:" + p1 + " " + p2);
                return;
            }
            Il2CppSystem.Collections.Generic.IReadOnlyList<DataProps.MartialData.Prefix> prefixs = martial.martialInfo.prefixs;
            int num = 0;
            while (true)
            {
                DataProps.MartialData.Prefix prefix;
                try
                {
                    prefix = prefixs[num];
                }
                catch (Exception)
                {
                    break;
                }
                martial.SetPrefixLevel(num, g.conf.battleSkillPrefixValue.GetPrefixMaxLevel(martial, prefix.prefixValueItem) * 100);
                num++;
            }
            martial.martialInfo.initPrefixs = false;
            AddLog("execution succeed!");
        }

        public void MartialSkillMax()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataUnit.ActionMartialData skill = GetSkill(unit, p2);
            if (skill == null)
            {
                AddLog("Execution failed: Unable to obtain unit's skill:" + p1 + " " + p2);
                return;
            }
            DataProps.MartialData martialData = skill.data.To<DataProps.MartialData>();
            Il2CppSystem.Collections.Generic.IReadOnlyList<DataProps.MartialData.Prefix> prefixs = martialData.martialInfo.prefixs;
            int num = 0;
            while (true)
            {
                DataProps.MartialData.Prefix prefix;
                try
                {
                    prefix = prefixs[num];
                }
                catch (Exception)
                {
                    break;
                }
                martialData.SetPrefixLevel(num, g.conf.battleSkillPrefixValue.GetPrefixMaxLevel(martialData, prefix.prefixValueItem) * 100);
                num++;
            }
            martialData.martialInfo.initPrefixs = false;
            AddLog("execution succeed!");
        }

        public void MartialBookStudy()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataProps.MartialData martial = GetMartial(unit, p2);
            if (martial == null)
            {
                AddLog("Execution failed: Unable to obtain unit's cheats:" + p1 + " " + p2);
            }
            else
            {
                StudyAndEquip(unit, martial.data);
            }
        }

        public void DelSkill()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataUnit.ActionMartialData skill = GetSkill(unit, p2);
            if (skill == null)
            {
                AddLog("Execution failed: Unable to obtain unit's skill:" + p1 + " " + p2);
                return;
            }
            MartialType martialType = skill.data.To<DataProps.MartialData>().martialType;
            string soleID = skill.data.soleID;
            if (martialType == MartialType.Ability)
            {
                for (int i = 0; i < unit.data.unitData.abilitys.Length; i++)
                {
                    if (soleID == unit.data.unitData.abilitys[i])
                    {
                        unit.CreateAction(new UnitActionMartialUnequip(MartialType.Ability, i), isTip: true);
                    }
                }
            }
            if (soleID == unit.data.unitData.skillLeft)
            {
                unit.CreateAction(new UnitActionMartialUnequip(MartialType.SkillLeft, 0), isTip: true);
            }
            if (soleID == unit.data.unitData.skillRight)
            {
                unit.CreateAction(new UnitActionMartialUnequip(MartialType.SkillRight, 0), isTip: true);
            }
            if (soleID == unit.data.unitData.step)
            {
                unit.CreateAction(new UnitActionMartialUnequip(MartialType.Step, 0), isTip: true);
            }
            if (soleID == unit.data.unitData.ultimate)
            {
                unit.CreateAction(new UnitActionMartialUnequip(MartialType.Ultimate, 0), isTip: true);
            }
            int num = unit.CreateAction(new UnitActionMartialDel(skill), isTip: true);
            if (num == 0)
            {
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed!" + num);
            }
        }

        public void DelAllSkill()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            List<DataUnit.ActionMartialData> list = new List<DataUnit.ActionMartialData>();
            Il2CppSystem.Collections.Generic.Dictionary<string, DataUnit.ActionMartialData>.Enumerator enumerator = unit.data.unitData.allActionMartial.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Il2CppSystem.Collections.Generic.KeyValuePair<string, DataUnit.ActionMartialData> current = enumerator.Current;
                list.Add(current.value);
            }
            foreach (DataUnit.ActionMartialData item in list)
            {
                MartialType martialType = item.data.To<DataProps.MartialData>().martialType;
                string soleID = item.data.soleID;
                if (martialType == MartialType.Ability)
                {
                    for (int i = 0; i < unit.data.unitData.abilitys.Length; i++)
                    {
                        if (soleID == unit.data.unitData.abilitys[i])
                        {
                            unit.CreateAction(new UnitActionMartialUnequip(MartialType.Ability, i), isTip: true);
                        }
                    }
                }
                if (soleID == unit.data.unitData.skillLeft)
                {
                    unit.CreateAction(new UnitActionMartialUnequip(MartialType.SkillLeft, 0), isTip: true);
                }
                if (soleID == unit.data.unitData.skillRight)
                {
                    unit.CreateAction(new UnitActionMartialUnequip(MartialType.SkillRight, 0), isTip: true);
                }
                if (soleID == unit.data.unitData.step)
                {
                    unit.CreateAction(new UnitActionMartialUnequip(MartialType.Step, 0), isTip: true);
                }
                if (soleID == unit.data.unitData.ultimate)
                {
                    unit.CreateAction(new UnitActionMartialUnequip(MartialType.Ultimate, 0), isTip: true);
                }
                unit.CreateAction(new UnitActionMartialDel(item), isTip: true);
            }
            AddLog("execution succeed!");
        }

        public void DelAllItem()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!float.TryParse(p2, out var result))
            {
                result = 99f;
            }
            Il2CppArrayBase<DataProps.PropsData> il2CppArrayBase = unit.data.unitData.propData._allShowProps.ToArray();
            for (int i = 0; i < il2CppArrayBase.Count; i++)
            {
                DataProps.PropsData propsData = il2CppArrayBase[i];
                if (!((float)propsData.propsInfoBase.level > result))
                {
                    unit.data.CostPropItem(propsData.soleID, propsData.propsCount);
                }
            }
        }

        public void UpSkill()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataUnit.ActionMartialData skill = GetSkill(unit, p2);
            if (skill == null)
            {
                AddLog("Execution failed: Unable to obtain unit's skill:" + p1 + " " + p2);
                return;
            }
            string soleID = skill.data.soleID;
            unit.data.unitData.AddMartialExp(soleID, 100000000);
            AddLog("execution succeed!");
        }

        public void GameSpeed()
        {
            if (!float.TryParse(p1, out var result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p1);
                return;
            }
            ModMain.FixGameSpeed(result);
            AddLog("execution succeed!");
        }

        public void BagSize()
        {
            if (!int.TryParse(p1, out var value))
            {
                AddLog("Execution failed: The number entered is illegal:" + p1);
                return;
            }
            g.world.playerUnit.data.dynUnitData.propGridNum.AddValue((Func<int>)(() => value));
            AddLog("execution succeed!");
        }

        public void AddSprite()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.artifactSprite.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            unit.data.unitData.artifactSpriteData.AddSprite(result);
            AddLog("execution succeed!");
        }

        public void DelSprite()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.artifactSprite.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            for (int i = 0; i < unit.data.unitData.artifactSpriteData.sprites.Count; i++)
            {
                DataUnit.ArtifactSpriteData.Sprite sprite = unit.data.unitData.artifactSpriteData.sprites[i];
                if (sprite.spriteID == result)
                {
                    unit.data.unitData.artifactSpriteData.DelSpriteInSoleID(sprite.soleID);
                }
            }
            AddLog("execution succeed!");
        }

        public void LuckSpriteSkill()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.artifactSprite.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            Il2CppSystem.Collections.Generic.List<DataUnit.ArtifactSpriteData.Sprite> spriteInSpriteID = unit.data.unitData.artifactSpriteData.GetSpriteInSpriteID(result);
            if (spriteInSpriteID.Count > 0)
            {
                DataUnit.ArtifactSpriteData.Sprite qiling = spriteInSpriteID[0];
                Il2CppSystem.Collections.Generic.List<ConfArtifactSpriteTalentItem>.Enumerator enumerator = g.conf.artifactSpriteTalent.GetItemsInSpriteID(result).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ConfArtifactSpriteTalentItem current = enumerator.Current;
                    if (current.isShow != 0)
                    {
                        unit.data.unitData.artifactSpriteData.ActiveTanlet(qiling.soleID, current.number);
                    }
                }
                DataProps.PropsData propsData = unit.data.unitData.propData.CloneAllProps().Find((Func<DataProps.PropsData, bool>)((DataProps.PropsData v) => g.conf.artifactShape.GetItem(v.propsID) != null && v.To<DataProps.PropsArtifact>().spriteSoleID == qiling.soleID));
                if (propsData != null)
                {
                    WorldUnitEquipProps.UpdatePlayerArtifactPropEffect(unit, propsData.soleID);
                }
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed!");
            }
        }

        public void AddSpriteLove()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.artifactSprite.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result2))
            {
                AddLog("Execution failed: The number entered is illegal:" + p3);
                return;
            }
            for (int i = 0; i < unit.data.unitData.artifactSpriteData.sprites.Count; i++)
            {
                if (unit.data.unitData.artifactSpriteData.sprites[i].spriteID == result)
                {
                    unit.data.unitData.artifactSpriteData.sprites[i].intimacy = Mathf.Clamp(result2, g.conf.artifactSpriteClose.initClose, g.conf.artifactSpriteClose.closeMax);
                }
            }
            AddLog("execution succeed!");
        }

        public void LuckEyeSkill()
        {
            if (!int.TryParse(p1, out var result))
            {
                AddLog("Execution failed: The number entered is illegal:" + p1);
                return;
            }
            DataWorld.World.GodEyeData godEyeData = g.data.world.godEyeData;
            int num = result;
            if (!godEyeData.skillsID.Contains(num))
            {
                godEyeData.skillsID.Add(num);
            }
            ConfGodEyeSkillsItem item = g.conf.godEyeSkills.GetItem(num);
            if (!godEyeData.bossGrade.ContainsKey(item.bossID))
            {
                godEyeData.bossGrade.Add(item.bossID, 1);
            }
            AddLog("execution succeed!");
        }

        public void SetSuit()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataUnit.ActionMartialData ability = GetAbility(unit, p2);
            if (ability == null)
            {
                AddLog("Execution failed: Unable to obtain unit's skill:" + p1 + " " + p2);
                return;
            }
            DataProps.MartialData martialData = ability.data.To<DataProps.MartialData>();
            _ = martialData.martialType;
            _ = ability.data.soleID;
            if (!int.TryParse(p3, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p3);
                return;
            }
            ConfBattleAbilitySuitBaseItem item = g.conf.battleAbilitySuitBase.GetItem(result);
            if (item == null && result != 0)
            {
                AddLog("Execution failed: The entered id does not exist:" + p3);
                return;
            }
            if (item.suitMember.Split('|').Contains(martialData.baseID.ToString()))
            {
                martialData.data.To<DataProps.PropsAbilityData>().suitID = result;
                AddLog("execution succeed!");
                return;
            }
            AddLog("Execution failed: The exercise does not match the set suit. Exercise ID=" + martialData.baseID + "  Suit ID=" + result + "  This set supports exercises =" + item.suitMember);
        }

        public void UnlockImmortalSkill()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.immortalHuman.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            List<int> list = new List<int>();
            for (int i = 0; i < g.conf.immortalHuman._allConfList.Count; i++)
            {
                if (g.conf.immortalHuman.allConfList[i].id == result)
                {
                    string[] array = g.conf.immortalHuman.allConfList[i].activeSkills.Split('|');
                    for (int j = 0; j < array.Length; j++)
                    {
                        int num = int.Parse(array[j]);
                        unit.data.unitData.immortalCard.AddImmortalCard(num, g.conf.immortalHuman.allConfList[i].id);
                        list.Add(num);
                    }
                    string[] array2 = g.conf.immortalHuman.allConfList[i].passiveSkills.Split('|');
                    for (int k = 0; k < array2.Length; k++)
                    {
                        int num2 = int.Parse(array2[k]);
                        unit.data.unitData.immortalCard.AddImmortalCard(num2, g.conf.immortalHuman.allConfList[i].id);
                        list.Add(num2);
                    }
                }
            }
            UIGetImmortalSkill.OpenAddImmortalCardUI(unit, list.ToArray());
            AddLog("execution succeed!");
        }

        public void AddItem()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.itemProps.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result2))
            {
                AddLog("Execution failed: The number entered is illegal:" + p3);
                return;
            }
            AddItemId(unit, result, result2);
            AddLog("execution succeed!");
        }

        private void AddItemId(WorldUnitBase unit, int id, int value)
        {
            if (new List<int> { 2823001, 2823002, 2823003, 2823004 }.Contains(id))
            { // Is a potmon item
                if (value > 0)
                {
                    Il2CppSystem.Collections.Generic.List<DataProps.PropsData> list = g.world.devilDemonMgr.dataProps.AddProps(id, value);
                    if (list.Count > 0)
                    {
                        ModMain.lastAddProp = new DataStruct<WorldUnitBase, DataProps.PropsData>(unit, list[0]);
                    }
                }
                else
                {
                    g.world.devilDemonMgr.dataProps.DelProps(id, value);
                }
            }
            else if (value > 0)
            {
                Il2CppSystem.Collections.Generic.List<GameItemRewardData> list2 = new Il2CppSystem.Collections.Generic.List<GameItemRewardData>();
                list2.Add(new GameItemRewardData(1, new int[1] { id }, value));
                Il2CppSystem.Collections.Generic.List<DataProps.PropsData>.Enumerator enumerator = unit.data.RewardItem(list2).succeedPropsDatas.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DataProps.PropsData current = enumerator.Current;
                    DataProps.PropsData props = unit.data.unitData.propData.GetProps(current.soleID);
                    Console.WriteLine("Add props:" + current.soleID + " >> " + ((object)props)?.ToString() + " " + current.propsID);
                    if (props == null)
                    {
                        continue;
                    }
                    ModMain.lastAddProp = new DataStruct<WorldUnitBase, DataProps.PropsData>(unit, props);
                    if (props.propsID == 5081996)
                    {
                        DataProps.PropsElderData propsElderData = props.To<DataProps.PropsElderData>();
                        int num = UnityEngine.Random.Range(g.conf.npcGrade7.pieceMin, g.conf.npcGrade7.pieceMax + 1);
                        for (int i = 0; i < num; i++)
                        {
                            DataProps.PropsElderData propsElderData2 = DataProps.PropsData.NewProps(5082005, 1).To<DataProps.PropsElderData>();
                            propsElderData.atk += propsElderData2.atk;
                            propsElderData.def += propsElderData2.def;
                            propsElderData.hpMax += propsElderData2.hpMax;
                            propsElderData.mpMax += propsElderData2.mpMax;
                            propsElderData.spMax += propsElderData2.spMax;
                        }
                        ModMain.lastAddElder = new DataStruct<WorldUnitBase, DataProps.PropsData>(unit, props);
                    }
                    if (props.propsID == 5082066)
                    {
                        Func<int> func = delegate
                        {
                            int num6 = UnityEngine.Random.Range(1, 12);
                            if (num6 > 6)
                            {
                                num6 += 4;
                            }
                            return num6;
                        };
                        int num2 = func();
                        int num3 = 0;
                        int num4 = 0;
                        do
                        {
                            num3 = func();
                        }
                        while (num3 == num2);
                        do
                        {
                            num4 = func();
                        }
                        while (num3 == num4 || num4 == num2);
                        int num5 = 0;
                        if (UnityEngine.Random.Range(0, 100) < 90)
                        {
                            num5 = UnityEngine.Random.Range(85, 101);
                        }
                        num5 = ((UnityEngine.Random.Range(0, 100) >= 50) ? UnityEngine.Random.Range(0, 101) : UnityEngine.Random.Range(50, 101));
                        List<int> list3 = new List<int>(props.values);
                        list3[3] = num2;
                        list3[4] = num3;
                        list3[5] = num4;
                        list3[6] = num5;
                        props.SetValues(list3.ToArray());
                        ModMain.lastAddRule = new DataStruct<WorldUnitBase, DataProps.PropsData>(unit, props);
                    }
                    if (props.propsType == DataProps.PropsDataType.Props && props.propsItem.className == 401)
                    {
                        ModMain.lastAddArtifact = new DataStruct<WorldUnitBase, DataProps.PropsData>(unit, props);
                    }
                    current.SetValues(props.values);
                    Console.WriteLine("Item data" + string.Join(",", current.values));
                }
            }
            else
            {
                unit.data.CostPropItem(id, -value);
            }
        }

        public void AddMartial()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            string[] array = p2.Split(',');
            if (array.Length != 2)
            {
                AddLog("Execution failed: The entered cheat code is illegal:" + p2);
                return;
            }
            if (!int.TryParse(array[1], out var result))
            {
                AddLog("Execution failed: The entered cheat id is illegal:" + p2);
                return;
            }
            if (!int.TryParse(array[0], out var result2))
            {
                AddLog("Execution failed: The input cheat type is illegal:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result3))
            {
                result3 = unit.data.dynUnitData.GetGrade();
            }
            else if (result3 < 0 && result3 > 10)
            {
                AddLog("Execution failed: The input realm is not between 1-10:" + p2);
                return;
            }
            MartialType martialType = (MartialType)result2;
            switch (martialType)
            {
                case MartialType.SkillLeft:
                    if (g.conf.battleSkillAttack.GetItem(result) == null)
                    {
                        AddLog("Execution failed: Unable to obtain the martial arts/spiritual skill cheats:" + p2);
                        return;
                    }
                    break;
                case MartialType.SkillRight:
                    if (g.conf.battleSkillAttack.GetItem(result) == null)
                    {
                        AddLog("Execution failed: Unable to obtain the trick code:" + p2);
                        return;
                    }
                    break;
                case MartialType.Ultimate:
                    if (g.conf.battleSkillAttack.GetItem(result) == null)
                    {
                        AddLog("Execution failed: Unable to obtain the magical secret code:" + p2);
                        return;
                    }
                    break;
                case MartialType.Step:
                    if (g.conf.battleStepBase.GetItem(result) == null)
                    {
                        AddLog("Execution failed: Unable to obtain the secret code:" + p2);
                        return;
                    }
                    break;
                case MartialType.Ability:
                    if (g.conf.battleAbilityBase.GetItem(result) == null)
                    {
                        AddLog("Execution failed: Unable to obtain the secret code:" + p2);
                        return;
                    }
                    break;
                default:
                    AddLog("Execution failed: Unable to obtain this type of cheat code:" + p2);
                    return;
            }
            RandomGetRewardData randomGetRewardData = new RandomGetRewardData(martialType, 6, result3);
            randomGetRewardData.baseID = result;
            WorldUnitData.RewardItemData rewardItemData = unit.data.RewardItem(RewardTool.GetReward(randomGetRewardData), showUI: false);
            DataProps.MartialData martialData = unit.data.unitData.propData.GetProps(rewardItemData.succeedPropsDatas[0].soleID).To<DataProps.MartialData>();
            if (martialData != null)
            {
                ModMain.lastAddMartial = new DataStruct<WorldUnitBase, DataProps.MartialData>(unit, martialData);
                Console.WriteLine("Set the last added cheat code" + martialData.data.soleID + " >> " + (object)ModMain.lastAddMartial.t2);
                if (p4 == "true")
                {
                    StudyAndEquip(unit, martialData.data);
                }
                AddLog("execution succeed!");
            }
            else
            {
                AddLog("Execution failed!");
            }
        }

        public void AddSuit()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.battleAbilitySuitBase.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result2))
            {
                result2 = unit.data.dynUnitData.GetGrade();
            }
            else if (result2 < 0 && result2 > 10)
            {
                AddLog("Execution failed: The input realm is not between 1-10:" + p2);
                return;
            }
            string[] array = g.conf.battleAbilitySuitBase.GetItem(result).suitMember.Split('|');
            new List<DataProps.PropsData>();
            int num = 0;
            for (int i = 0; i < array.Length; i++)
            {
                int baseID = int.Parse(array[i]);
                int grade = unit.data.dynUnitData.GetGrade();
                _ = unit.data.unitData.unitID;
                RandomGetRewardData randomGetRewardData = new RandomGetRewardData(MartialType.Ability, 6, grade);
                randomGetRewardData.baseID = baseID;
                WorldUnitData.RewardItemData rewardItemData = unit.data.RewardItem(RewardTool.GetReward(randomGetRewardData), showUI: false);
                DataProps.PropsData props = unit.data.unitData.propData.GetProps(rewardItemData.succeedPropsDatas[0].soleID);
                if (props != null)
                {
                    props.To<DataProps.PropsAbilityData>().suitID = result;
                    if (p4 == "true")
                    {
                        StudyAndEquip(unit, props);
                    }
                    num++;
                }
            }
            if (num == array.Length)
            {
                AddLog("execution succeed!");
            }
            else if (num == 0)
            {
                AddLog("Execution failed!");
            }
            else
            {
                AddLog("Partial execution successful" + num + "/" + array.Length);
            }
        }

        public void FixMartialPrefix()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataProps.MartialData martial = GetMartial(unit, p2);
            if (martial == null)
            {
                AddLog("Execution failed: Unable to obtain unit's cheats:" + p1 + " " + p2);
                return;
            }
            string[] array = p3.Split(',');
            List<int> list = new List<int>();
            string[] array2 = array;
            foreach (string text in array2)
            {
                if (!int.TryParse(text, out var result))
                {
                    AddLog("Execution failed: The entered term is illegal:" + text + "/" + p3);
                    return;
                }
                list.Add(result);
            }
            FixPrefix(martial, list);
            AddLog("execution succeed!");
        }

        public void FixSkillPrefix()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataUnit.ActionMartialData skill = GetSkill(unit, p2);
            if (skill == null)
            {
                AddLog("Execution failed: Unable to obtain unit's skill:" + p1 + " " + p2);
                return;
            }
            DataProps.MartialData martialData = skill.data.To<DataProps.MartialData>();
            string[] array = p3.Split(',');
            List<int> list = new List<int>();
            string[] array2 = array;
            foreach (string text in array2)
            {
                if (!int.TryParse(text, out var result))
                {
                    AddLog("Execution failed: The entered term is illegal:" + text + "/" + p3);
                    return;
                }
                list.Add(result);
            }
            FixPrefix(martialData, list);
            AddLog("execution succeed!");
        }

        private void FixPrefixMaxLevel(DataProps.MartialData martialData)
        {
            DataProps.PropsData data = martialData.data;
            Il2CppStructArray<int> values = data.values;
            Console.WriteLine("old entry data" + string.Join(", ", values));
            List<int> list = new List<int>();
            bool flag = false;
            for (int i = 0; i < values.Length; i++)
            {
                if (flag)
                {
                    list.Add(600);
                }
                else
                {
                    list.Add(values[i]);
                }
                if (values[i] == -1)
                {
                    flag = true;
                }
            }
            Console.WriteLine("New entry data" + string.Join(", ", list));
            data.SetValues(list.ToArray());
            martialData.martialInfo.initPrefixs = false;
        }

        private void FixPrefix(DataProps.MartialData martialData, List<int> prefix)
        {
            Console.WriteLine("Modify entry" + string.Join(", ", prefix));
            DataProps.PropsData data = martialData.data;
            Il2CppStructArray<int> values = data.values;
            Console.WriteLine("old entry data" + string.Join(", ", values));
            List<int> list = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                list.Add(values[i]);
            }
            for (int j = 0; j < prefix.Count; j++)
            {
                list.Add(prefix[j]);
            }
            list.Add(-1);
            for (int k = 0; k < prefix.Count; k++)
            {
                list.Add(600);
            }
            Console.WriteLine("New entry data" + string.Join(", ", list));
            data.SetValues(list.ToArray());
            martialData.martialInfo.initPrefixs = false;
        }

        private void StudyAndEquip(WorldUnitBase unit, DataProps.PropsData propsData)
        {
            DataProps.MartialData martialData = propsData.To<DataProps.MartialData>();
            string basType = martialData.martialInfo.basType;
            if (basType != "")
            {
                int basRequire = martialData.martialInfo.basRequire;
                basRequire = Mathf.Clamp(basRequire, 0, g.conf.roleAttributeLimit.basisBladeMax);
                Il2CppSystem.Collections.Generic.IReadOnlyList<DynInt> values = unit.data.dynUnitData.GetValues(basType);
                if (basType == "basAllAny")
                {
                    int num = CommonTool.GetItemsAdding(values, (Func<DynInt, int>)((DynInt v) => v.value)) / 12;
                    int num2 = basRequire - num;
                    if (num2 > 0)
                    {
                        int num3 = 0;
                        while (true)
                        {
                            try
                            {
                                values[num3++].baseValue += num2;
                            }
                            catch (Exception)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    int num4 = 0;
                    while (true)
                    {
                        DynInt dynInt;
                        try
                        {
                            dynInt = values[num4];
                        }
                        catch (Exception)
                        {
                            break;
                        }
                        if (dynInt.value < basRequire)
                        {
                            dynInt.baseValue += basRequire - dynInt.value + CommonTool.Random(0, 10);
                        }
                        num4++;
                    }
                }
            }
            if (martialData.martialType == MartialType.Ability)
            {
                int abilityPoint = martialData.martialInfo.abilityPoint;
                if (abilityPoint + unit.data.dynUnitData.GetCurUseAbilityPoint() > unit.data.dynUnitData.abilityPoint.value)
                {
                    unit.data.dynUnitData.abilityPoint.baseValue += abilityPoint + unit.data.dynUnitData.GetCurUseAbilityPoint() - unit.data.dynUnitData.abilityPoint.value + UnityEngine.Random.Range(0, 10);
                }
            }
            UnitActionPropMartialStudy unitActionPropMartialStudy = new UnitActionPropMartialStudy(martialData.data);
            unitActionPropMartialStudy.isCost = false;
            unitActionPropMartialStudy.isOneStudy = true;
            int num5 = unit.CreateAction(unitActionPropMartialStudy);
            if (num5 != 0)
            {
                Console.WriteLine("Automatic learning failed" + num5 + $"  day={unitActionPropMartialStudy.day} isCost={unitActionPropMartialStudy.isCost} money={unitActionPropMartialStudy.money} costMood={unitActionPropMartialStudy.costMood}");
            }
            else
            {
                Console.WriteLine("Automatic learning successful" + num5 + $"  day={unitActionPropMartialStudy.day} isCost={unitActionPropMartialStudy.isCost} money={unitActionPropMartialStudy.money} costMood={unitActionPropMartialStudy.costMood}");
                ModMain.lastStudySkill = new DataStruct<WorldUnitBase, DataUnit.ActionMartialData>(unit, unit.data.unitData.GetActionMartial(martialData.data.soleID));
                Console.WriteLine("Set the last learned technique" + martialData.data.soleID + " >> " + (object)ModMain.lastStudySkill.t2);
                if (martialData.martialType == MartialType.Ability)
                {
                    ModMain.lastStudyAbility = new DataStruct<WorldUnitBase, DataUnit.ActionMartialData>(unit, unit.data.unitData.GetActionMartial(martialData.data.soleID));
                }
            }
            DataUnit.ActionMartialData actionMartial = unit.data.unitData.GetActionMartial(martialData.data.soleID);
            if (actionMartial == null)
            {
                return;
            }
            if (unit.data.unitData.heart.IsHeroes())
            {
                WorldUnitAIAction1035.RandomUnlockPrefix(unit, actionMartial.data.To<DataProps.MartialData>());
            }
            int num6 = 0;
            if (martialData.martialType == MartialType.Ability)
            {
                num6 = UnitActionMartialUnequip.GetAbilityIndexInType(unit, martialData);
                if (num6 != -1)
                {
                    unit.CreateAction(new UnitActionMartialUnequip(MartialType.Ability, num6));
                }
                else
                {
                    num6 = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(unit.data.unitData.abilitys[i]))
                            {
                                num6 = i;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            else
            {
                unit.CreateAction(new UnitActionMartialUnequip(null)
                {
                    type = martialData.martialType
                });
            }
            int num7 = unit.CreateAction(new UnitActionMartialEquip(actionMartial, num6));
            unit.data.unitData.AddMartialExp(actionMartial.data.soleID, 10000000);
            if (num7 != 0)
            {
                Console.WriteLine("Auto equip failed" + num7);
            }
        }

        public void OpenMapLight()
        {
            GridBaseUI.debugView = true;
            DramaFunction.UpdateMapAllUI();
            AddLog("execution succeed!");
        }

        public void CloseMapLight()
        {
            GridBaseUI.debugView = false;
            DramaFunction.UpdateMapAllUI();
            AddLog("execution succeed!");
        }

        public void OpenMap()
        {
            for (int i = 0; i < g.data.grid.mapWidth; i++)
            {
                for (int j = 0; j < g.data.grid.mapHeight; j++)
                {
                    Vector2Int p = new Vector2Int(i, j);
                    g.data.map.AddOpenGrid(p);
                }
            }
            DramaFunction.UpdateMapAllUI();
            AddLog("execution succeed!");
        }

        public void CloseMap()
        {
            for (int i = 0; i < g.data.grid.mapWidth; i++)
            {
                for (int j = 0; j < g.data.grid.mapHeight; j++)
                {
                    Vector2Int p = new Vector2Int(i, j);
                    g.data.map.DelOpenGrid(p);
                }
            }
            DramaFunction.UpdateMapAllUI();
            AddLog("execution succeed!");
        }

        public void MoveChar()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered X coordinate is illegal:" + p2);
                return;
            }
            if (!int.TryParse(p3, out var result2))
            {
                AddLog("Execution failed: The input Y coordinate is illegal:" + p3);
                return;
            }
            int num = unit.CreateAction(new UnitActionSetPoint(new Vector2Int(result, result2)));
            if (num == 0)
            {
                AddLog("execution succeed!");
                DramaFunction.UpdateMapAllUI();
            }
            else
            {
                AddLog("Execution failed!" + num);
            }
        }

        public void AddPotmon()
        {
            if (!int.TryParse(p1, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p1);
                return;
            }
            if (g.conf.potmonBase.GetItem(result) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result2))
            {
                result2 = g.world.playerUnit.data.dynUnitData.GetGrade();
            }
            else if (result2 < 0 && result2 > 10)
            {
                AddLog("Execution failed: The input realm is not between 1-10:" + p2);
                return;
            }
            try
            {
                _ = g.world.devilDemonMgr.potMonMgr.potMonDatas;
                g.world.devilDemonMgr.potMonMgr.AddPotMon(result, result2);
                UIPotmonList uI = g.ui.GetUI<UIPotmonList>(UIType.PotmonList);
                if (uI != null)
                {
                    uI.UpdateUI();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                AddLog("Execution failed!");
                return;
            }
            AddLog("execution succeed!");
        }

        public void FixShenqi()
        {
            try
            {
                if (g.data.world.resurgeCount < g.conf.gameDifficultyValue.curItem.fishJade)
                {
                    g.data.world.resurgeCount = g.conf.gameDifficultyValue.curItem.fishJade;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                if (g.data.world.godEyeData.isDamage && g.data.world.gameLevel != GameLevelType.CrueltyShura)
                {
                    g.data.world.godEyeData.Repair();
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.ToString());
            }
            try
            {
                if (g.world.devilDemonMgr.devilDemonData.isDamage)
                {
                    g.world.devilDemonMgr.devilDemonData.isDamage = false;
                    g.world.devilDemonMgr.devilDemonData.repairMonth = 0;
                    g.world.devilDemonMgr.devilDemonData.brokenCount = 0;
                    Il2CppSystem.Collections.Generic.List<PotMonMgr.PotMonData>.Enumerator enumerator = g.world.devilDemonMgr.potMonMgr.potMonDatas.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        PotMonMgr.PotMonData current = enumerator.Current;
                        if (current.healthState != 0)
                        {
                            current.SetHealthState(PotMonHealthState.Normal);
                        }
                    }
                }
            }
            catch (Exception ex3)
            {
                Console.WriteLine(ex3.ToString());
            }
            AddLog("execution succeed!");
        }

        public void FixElder()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataProps.PropsData elder = GetElder(unit, p2);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to obtain the spirit of transformation:" + p2);
                return;
            }
            try
            {
                string[] array = p3.Split(',');
                int.TryParse(array[0], out var result);
                int.TryParse(array[1], out var result2);
                int.TryParse(array[2], out var result3);
                int.TryParse(array[3], out var result4);
                int.TryParse(array[4], out var result5);
                DataProps.PropsElderData propsElderData = elder.To<DataProps.PropsElderData>();
                propsElderData.atk = result;
                propsElderData.def = result2;
                propsElderData.hpMax = result3;
                propsElderData.mpMax = result4;
                propsElderData.spMax = result5;
                elder.initValuesHashIndex = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                AddLog("Execution failed!");
                return;
            }
            AddLog("execution succeed!");
        }

        public void FixRule()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataProps.PropsData rule = GetRule(unit, p2);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to obtain Dao Soul:" + p2);
                return;
            }
            Dictionary<string, int> dictionary = new Dictionary<string, int>
            {
                { "basBlade", 1 },
                { "basSpear", 2 },
                { "basSword", 3 },
                { "basFist", 4 },
                { "basPalm", 5 },
                { "basFinger", 6 },
                { "basFire", 11 },
                { "basFroze", 12 },
                { "basThunder", 13 },
                { "basWind", 14 },
                { "basEarth", 15 },
                { "basWood", 16 }
            };
            int[] array = new int[3];
            try
            {
                string[] array2 = p3.Split(',');
                array[0] = dictionary[array2[0]];
                try
                {
                    array[1] = dictionary[array2[1]];
                    array[2] = dictionary[array2[2]];
                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.ToString());
                AddLog("Execution failed: Correct attributes required:" + p3);
                return;
            }
            int.TryParse(p4, out var result);
            result = Mathf.Clamp(result, 0, 100);
            try
            {
                Console.WriteLine("jiu's data" + string.Join(",", rule.values));
                DataProps.PropsRuleData propsRuleData = rule.To<DataProps.PropsRuleData>();
                propsRuleData.mainType = array[0];
                propsRuleData.viceType1 = array[1];
                propsRuleData.viceType2 = array[2];
                propsRuleData.purity = result;
                rule.initValuesHashIndex = false;
                Console.WriteLine("new data" + string.Join(",", rule.values));
            }
            catch (Exception ex3)
            {
                Console.WriteLine(ex3.ToString());
                AddLog("Execution failed!");
                return;
            }
            AddLog("execution succeed!");
        }

        public void FixArtifact()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            DataProps.PropsData artifact = GetArtifact(unit, p2);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to obtain magic weapon:" + p2);
                return;
            }
            try
            {
                string[] array = p3.Split(',');
                Il2CppStructArray<int> values = artifact.values;
                Console.WriteLine("old data" + string.Join(",", artifact.values));
                List<int> list = new List<int>();
                for (int i = 0; i < 3; i++)
                {
                    list.Add(values[i]);
                }
                for (int j = 3; j < 11; j++)
                {
                    if (int.TryParse(array[j - 3], out var result))
                    {
                        list.Add(result);
                    }
                    else
                    {
                        list.Add(values[j]);
                    }
                }
                list.Add(values[11]);
                int num = array.Length - 8;
                int num2 = 0;
                for (int k = 0; k < num; k++)
                {
                    if (int.TryParse(array[k + 8], out var result2))
                    {
                        list.Add(result2);
                        num2++;
                    }
                }
                list.Add(-1);
                for (int l = 0; l < num2; l++)
                {
                    list.Add(600);
                }
                artifact.SetValues(list.ToArray());
                Console.WriteLine("new data" + string.Join(",", artifact.values));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                AddLog("Execution failed:" + p3);
                return;
            }
            AddLog("execution succeed!");
        }

        public void FixShenhun()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            unit.data.unitData.elderData.id = result;
            AddLog("execution succeed");
        }

        public void FixDaoJie()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            Dictionary<string, int> dictionary = new Dictionary<string, int>
            {
                { "basBlade", 1 },
                { "basSpear", 2 },
                { "basSword", 3 },
                { "basFist", 4 },
                { "basPalm", 5 },
                { "basFinger", 6 },
                { "basFire", 11 },
                { "basFroze", 12 },
                { "basThunder", 13 },
                { "basWind", 14 },
                { "basEarth", 15 },
                { "basWood", 16 }
            };
            List<int> list = new List<int>();
            try
            {
                string[] array = p2.Split(',');
                for (int i = 0; i < 9; i++)
                {
                    list.Add(dictionary[array[i % array.Length]]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                AddLog("Execution failed: Correct attributes required:" + p2);
                return;
            }
            unit.data.unitData.fieldSkillData.mainType1 = list[0];
            unit.data.unitData.fieldSkillData.mainType2 = list[1];
            unit.data.unitData.fieldSkillData.mainType3 = list[2];
            unit.data.unitData.fieldSkillData.viceType1A = list[3];
            unit.data.unitData.fieldSkillData.viceType1B = list[4];
            unit.data.unitData.fieldSkillData.viceType2A = list[5];
            unit.data.unitData.fieldSkillData.viceType2B = list[6];
            unit.data.unitData.fieldSkillData.viceType3A = list[7];
            unit.data.unitData.fieldSkillData.viceType3B = list[8];
            AddLog("execution succeed");
        }

        public void FixLiaoji()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var wingManID))
            {
                AddLog("Execution failed: The entered id is illegal:" + p2);
                return;
            }
            if (g.conf.wingmanBase.GetItem(wingManID) == null)
            {
                AddLog("Execution failed: The entered id does not exist:" + p2);
                return;
            }
            List<int> EffectList = new List<int>();
            List<int> PrefixList = new List<int>();
            List<int> PrefixUpList = new List<int>();
            try
            {
                if (!string.IsNullOrEmpty(p3))
                {
                    string[] array = p3.Split(',');
                    foreach (string text in array)
                    {
                        int num = int.Parse(text.Substring(1, text.Length - 1));
                        if (text.StartsWith("e"))
                        {
                            if (g.conf.wingmanEffect.GetItem(num) == null)
                            {
                                AddLog("Execution failed: Unable to obtain effect:" + p3);
                                return;
                            }
                            EffectList.Add(num);
                        }
                        if (text.StartsWith("f"))
                        {
                            if (g.conf.wingmanFixValue.GetItem(num) == null)
                            {
                                AddLog("Execution failed: Unable to obtain entry:" + p3);
                                return;
                            }
                            PrefixList.Add(num);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            try
            {
                if (!string.IsNullOrEmpty(p4))
                {
                    string[] array = p4.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        int num2 = int.Parse(array[i]);
                        if (g.conf.wingmanFixValue.GetItem(num2) == null)
                        {
                            AddLog("Execution failed: Unable to obtain upgrade:" + p4);
                            return;
                        }
                        PrefixUpList.Add(num2);
                    }
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.ToString());
            }
            Console.WriteLine(string.Join(",", EffectList));
            Console.WriteLine(string.Join(",", PrefixList));
            Console.WriteLine(string.Join(",", PrefixUpList));
            unit.data.unitData.reborn.wingManID = wingManID;
            unit.data.unitData.reborn.wingmanEffectList.Clear();
            foreach (int item in EffectList)
            {
                unit.data.unitData.reborn.wingmanEffectList.Add(item);
            }
            unit.data.unitData.reborn.wingmanPrefixList.Clear();
            foreach (int item2 in PrefixList)
            {
                unit.data.unitData.reborn.wingmanPrefixList.Add(item2);
            }
            unit.data.unitData.reborn.wingmanPrefixUp.Clear();
            foreach (int item3 in PrefixUpList)
            {
                unit.data.unitData.reborn.wingmanPrefixUp.Add(item3);
            }
            AddLog("execution succeed");
        }

        public void AddUpGradeProps()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                result = 0;
            }
            for (int i = 1; i <= 10; i++)
            {
                if (result != 0 && result != i)
                {
                    continue;
                }
                switch (i)
                {
                    case 7:
                        {
                            int[] array = new int[8] { 1011945, 1011955, 5082016, 5082026, 5082036, 5082046, 5082056, 5081996 };
                            foreach (int id in array)
                            {
                                AddItemId(unit, id, 1);
                            }
                            break;
                        }
                    case 8:
                        {
                            int[] array = new int[4] { 1011965, 1011975, 1011605, 1011606 };
                            foreach (int id2 in array)
                            {
                                AddItemId(unit, id2, 1);
                            }
                            for (int k = 0; k < 12; k++)
                            {
                                AddItemId(unit, 5082066, 1);
                            }
                            break;
                        }
                    default:
                        AddPropForGrade(i);
                        if (i == 10)
                        {
                            AddPropForGrade(1);
                        }
                        break;
                    case 1:
                        break;
                }
            }
            AddLog("execution succeed");
        }

        private static void AddPropForGrade(int grade)
        {
            ConfRoleGradeItem confRoleGradeItem = g.conf.roleGrade.GetGradeUpQuality(grade)[g.conf.roleGrade.GetGradeUpQuality(grade).Count - 1];
            int[] array = CommonTool.StrSplitInt(g.conf.roleGrade.GetGradeItem(grade - 1, 2).itemShowA, '_');
            for (int i = 0; i < array.Length; i++)
            {
                g.world.playerUnit.data.RewardPropItem(DataProps.PropsData.NewProps(array[i], 1));
            }
            array = CommonTool.StrSplitInt(g.conf.roleGrade.GetGradeItem(grade - 1, 3).itemShowA, '_');
            for (int j = 0; j < array.Length; j++)
            {
                g.world.playerUnit.data.RewardPropItem(DataProps.PropsData.NewProps(array[j], 1));
            }
            array = CommonTool.StrSplitInt(confRoleGradeItem.itemShowA, '_');
            for (int k = 0; k < array.Length; k++)
            {
                g.world.playerUnit.data.RewardPropItem(DataProps.PropsData.NewProps(array[k], 1));
            }
            int[] array2 = CommonTool.StrSplitInt(confRoleGradeItem.itemShowB, '_');
            for (int l = 0; l < array2.Length; l++)
            {
                g.world.playerUnit.data.RewardPropItem(DataProps.PropsData.NewProps(array2[l], 1));
            }
        }

        public void LuckArt()
        {
            Il2CppSystem.Collections.Generic.List<ConfRuneFormulaItem>.Enumerator enumerator = g.conf.runeFormula._allConfList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ConfRuneFormulaItem current = enumerator.Current;
                if (current.grade <= 10 && g.conf.itemProps.GetItem(current.formulaId) != null && !g.data.world.allArtistrySkillID.Contains(current.formulaId))
                {
                    g.data.world.allArtistrySkillID.Add(current.formulaId);
                }
            }
            Il2CppSystem.Collections.Generic.List<ConfMakePillFormulaItem>.Enumerator enumerator2 = g.conf.makePillFormula._allConfList.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                ConfMakePillFormulaItem item = enumerator2.Current;
                if (item.grade <= 10 && g.conf.itemProps.GetItem(item.id) != null && g.data.world.pillFormulas.Find((Func<DataWorld.World.PillFormulaData, bool>)((DataWorld.World.PillFormulaData v) => v.id == item.id)) == null)
                {
                    DataWorld.World.PillFormulaData pillFormulaData = new DataWorld.World.PillFormulaData();
                    pillFormulaData.id = item.id;
                    g.data.world.pillFormulas.Add(pillFormulaData);
                }
            }
            Il2CppSystem.Collections.Generic.List<ConfGeomancySkillItem>.Enumerator enumerator3 = g.conf.geomancySkill._allConfList.GetEnumerator();
            while (enumerator3.MoveNext())
            {
                ConfGeomancySkillItem current2 = enumerator3.Current;
                if (current2.grade == 9 && g.conf.itemProps.GetItem(current2.id) != null && !g.data.world.allArtistrySkillID.Contains(current2.id))
                {
                    g.data.world.allArtistrySkillID.Add(current2.id);
                }
            }
            Il2CppSystem.Collections.Generic.List<ConfWorldBlockHerbItem>.Enumerator enumerator4 = g.conf.worldBlockHerb._allConfList.GetEnumerator();
            while (enumerator4.MoveNext())
            {
                ConfWorldBlockHerbItem current3 = enumerator4.Current;
                if (g.conf.itemProps.GetItem(current3.id) != null && !g.data.world.allArtistrySkillID.Contains(current3.id))
                {
                    g.data.world.allArtistrySkillID.Add(current3.id);
                }
            }
            Il2CppSystem.Collections.Generic.List<ConfWorldBlockMineBookItem>.Enumerator enumerator5 = g.conf.worldBlockMineBook._allConfList.GetEnumerator();
            while (enumerator5.MoveNext())
            {
                ConfWorldBlockMineBookItem current4 = enumerator5.Current;
                if (current4.conceal != 1 && g.conf.itemProps.GetItem(current4.id) != null && !g.data.world.allArtistrySkillID.Contains(current4.id))
                {
                    g.data.world.allArtistrySkillID.Add(current4.id);
                }
            }
            Il2CppSystem.Collections.Generic.List<ConfArtifactShapeMaterialItem>.Enumerator enumerator6 = g.conf.artifactShapeMaterial._allConfList.GetEnumerator();
            while (enumerator6.MoveNext())
            {
                ConfArtifactShapeMaterialItem current5 = enumerator6.Current;
                if (g.conf.itemProps.GetItem(current5.id) != null && !g.data.world.allArtistrySkillID.Contains(current5.id))
                {
                    g.data.world.allArtistrySkillID.Add(current5.id);
                }
            }
            AddLog("execution succeed");
        }

        public void StudySkillGroup()
        {
            WorldUnitBase worldUnitBase = GetUnit(p1);
            if (worldUnitBase == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (!int.TryParse(p2, out var result))
            {
                AddLog("Execution failed: No routine selected:" + p2);
                return;
            }
            if (result == 0)
            {
                result = g.conf.taoistHeartSkillGroupName._allConfList[0].id;
            }
            if (g.conf.taoistHeartSkillGroupName.GetItem(result) == null)
            {
                AddLog("Execution failed: No routine is invalid:" + p2);
                return;
            }
            try
            {
                DelAllSkill();
                worldUnitBase.data.unitData.heart.heroesSkillGroupID = result;
                if (worldUnitBase.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID)
                {
                    WorldUnitBase worldUnitBase2 = worldUnitBase;
                    worldUnitBase = new WorldUnitBase();
                    worldUnitBase.conf = worldUnitBase2.conf;
                    worldUnitBase.data = worldUnitBase2.data;
                    worldUnitBase.luckDyn = worldUnitBase2.luckDyn;
                    worldUnitBase.behavior = worldUnitBase2.behavior;
                    worldUnitBase.appellation = worldUnitBase2.appellation;
                    worldUnitBase.allHobby = worldUnitBase2.allHobby;
                    worldUnitBase.allLuck = worldUnitBase2.allLuck;
                    worldUnitBase.allTask = worldUnitBase2.allTask;
                    worldUnitBase.allTroubles = worldUnitBase2.allTroubles;
                    worldUnitBase.allEquips = worldUnitBase2.allEquips;
                    worldUnitBase.allEffects = worldUnitBase2.allEffects;
                    worldUnitBase.allEffectsCustom = worldUnitBase2.allEffectsCustom;
                    worldUnitBase._allWorldUnitEffectBaseCache = worldUnitBase2._allWorldUnitEffectBaseCache;
                    worldUnitBase._isDie = worldUnitBase2._isDie;
                    worldUnitBase.onDieCall = worldUnitBase2.onDieCall;
                    worldUnitBase.isInitOneEquip = worldUnitBase2.isInitOneEquip;
                }
                try
                {
                    g.conf.npcHeroes.InitMartial(worldUnitBase, 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    AddLog("Execution failed");
                    return;
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.ToString());
                AddLog("Execution failed!");
                return;
            }
            AddLog("execution succeed");
        }

        public void PlayNpc()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (unit.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID)
            {
                AddLog("Execution failed: Unable to seize self:" + p1);
                return;
            }
            bool flag = false;
            if (g.ui.GetUI<UIMapMain>(UIType.MapMain) != null)
            {
                flag = true;
                g.ui.OpenUI<UIMapMain>(UIType.MapMain);
                g.ui.CloseAllUI();
                g.data.world.playerUnitID = unit.data.unitData.unitID;
                g.world.playerUnit = g.world.unit.GetUnit(g.data.world.playerUnitID);
                g.world.playerUnit.RoundStart(g.world.run.roundDayResidue);
                g.world.playerUnit.UpdateConf();
                if (flag)
                {
                    g.ui.OpenUI<UIMapMain>(UIType.MapMain);
                }
                AddLog("execution succeed");
            }
            else
            {
                AddLog("Execution failed: The body capture is currently unavailable!");
            }
        }

        public void FixXingge()
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            int inTrait;
            int outTrait;
            int outTrait2;
            try
            {
                string[] array = p2.Split(',');
                inTrait = int.Parse(array[0]);
                outTrait = int.Parse(array[1]);
                outTrait2 = int.Parse(array[2]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                AddLog("Execution failed: The selected character is illegal:" + p2);
                return;
            }
            unit.data.unitData.propertyData.outTrait1 = outTrait;
            unit.data.unitData.propertyData.outTrait2 = outTrait2;
            unit.data.unitData.propertyData.inTrait = inTrait;
            AddLog("execution succeed");
        }

        public void RunGameCmd()
        {
            try
            {
                DramaFunctionTool.OptionsFunction(p1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                AddLog("Execution failed");
                return;
            }
            AddLog("execution succeed");
        }

        public void OpenGameDrama()
        {
            try
            {
                int.TryParse(p1, out var result);
                WorldUnitBase unit = GetUnit(p2);
                WorldUnitBase unit2 = GetUnit(p3);
                DramaTool.OpenDrama(result, new DramaData
                {
                    unitLeft = unit,
                    unitRight = unit2
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                AddLog("Execution failed");
                return;
            }
            AddLog("execution succeed");
        }
        /*
        public void Tc85YzAddNpc() // Here
        {
            WorldUnitBase unit = GetUnit(p1);
            if (unit == null)
            {
                AddLog("Execution failed: Unable to get units:" + p1);
                return;
            }
            if (unit.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID)
            {
                AddLog("Execution failed: cannot sell itself:" + p1);
                return;
            }
            if (IsJinvOrBangjia(unit.data.unitData.unitID))
            {
                AddLog("Execution failed: The person is already a prostitute or has been kidnapped:" + p1);
                return;
            }
            try
            {
                Brothel.Sell(g.world.playerUnit, unit, (MapBuildBase)null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                AddLog("Execution failed");
                return;
            }
            AddLog("execution succeed");
        }
        
        public void Tc85YzAutoNpc() // Here
        {
            int.TryParse(p1, out var result);
            int num = 0;
            try
            {
                List<string> list = new List<string>();
                Il2CppSystem.Collections.Generic.Dictionary<string, WorldUnitBase>.Enumerator enumerator = g.world.unit.allUnit.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    WorldUnitBase value = enumerator.Current.Value;
                    if (value.data.unitData.propertyData.sex == UnitSexType.Woman)
                    {
                        list.Add(value.data.unitData.unitID);
                    }
                }
                while (result > 0 && list.Count != 0)
                {
                    string text = list[UnityEngine.Random.Range(0, list.Count)];
                    list.Remove(text);
                    if (IsJinvOrBangjia(text) || text == g.world.playerUnit.data.unitData.unitID)
                    {
                        continue;
                    }
                    WorldUnitBase unit = g.world.unit.GetUnit(text);
                    if (unit != null)
                    {
                        if (Brothel.Sell(g.world.playerUnit, unit, (MapBuildBase)null) == -1)
                        {
                            break;
                        }
                        num++;
                        result--;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                AddLog("Execution failed");
                return;
            }
            AddLog("Execution successful! Sold quantity" + num);
        }

        private bool IsJinvOrBangjia(string id) // Here
        {
            Type typeFromHandle = typeof(Brothel);
            Dictionary<string, List<string>> dictionary = (System.Collections.Generic.Dictionary<string, List<string>>)typeFromHandle.GetField("Bitchs", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            List<string> list = (System.Collections.Generic.List<string>)typeFromHandle.GetField("Kidnapping", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            foreach (System.Collections.Generic.KeyValuePair<string, List<string>> item in dictionary)
            {
                foreach (string item2 in item.Value)
                {
                    if (item2 == id)
                    {
                        return true;
                    }
                }
            }
            foreach (string item3 in list)
            {
                if (item3 == id)
                {
                    return true;
                }
            }
            return false;
        }

        private void LogJinv()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, List<string>> item in (System.Collections.Generic.Dictionary<string, List<string>>)typeof(Brothel).GetField("Bitchs", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null))
            {
                string text = item.Key;
                MapBuildBase build = g.world.build.GetBuild(item.Key);
                if (build != null)
                {
                    text += build.name;
                }
                Console.WriteLine(text + " Number of prostitutes>> " + item.Value);
            }
        }
        */
    }
}
