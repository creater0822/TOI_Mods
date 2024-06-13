using System;
using Il2CppSystem.Collections.Generic;

namespace MOD_tlRCB.EvilFall
{
    public class LustfulLuck
    {
        private static void Test()
        {
            WorldUnitBase unit = g.world.unit.GetUnit("8ZSL4g");
            WorldUnitBase playerUnit = g.world.playerUnit;
            g.conf.roleCall.RoleCall(unit, playerUnit, new UnitConditionData(unit, playerUnit));
        }

        public static void HandleMan(WorldUnitBase unit)
        {
            if (unit.isDie || !unit.HasLuck(1840247464))
            {
                return;
            }
            List<WorldUnitBase> unitExact = g.world.unit.GetUnitExact(unit.data.unitData.GetPoint(), 10, isGetHideUnit: false, isGetPlayer: true);
            WorldUnitBase worldUnitBase = null;
            int num = 0;
            try
            {
                List<WorldUnitBase>.Enumerator enumerator = unitExact.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    WorldUnitBase current = enumerator.Current;
                    if (current != unit && current.data.unitData.propertyData.sex != unit.data.unitData.propertyData.sex && current.GetAge() >= 192 && unit.data.GetRelationType(current) != UnitBothRelationType.Married)
                    {
                        int num2 = Math.Abs(unit.data.unitData.relationData.GetIntim(current));
                        if (unit.IsVital(current))
                        {
                            num2 += 100;
                        }
                        if (current.HasLuck(1561118811))
                        {
                            num2 += 50;
                        }
                        if (current.IsPlayer())
                        {
                            num2 += 50;
                        }
                        if (worldUnitBase == null)
                        {
                            worldUnitBase = current;
                            num = num2;
                        }
                        else if (num2 > num)
                        {
                            worldUnitBase = current;
                            num = num2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to get a stronger person：" + ex);
                throw;
            }
            if (unit.IsPlayer() || worldUnitBase == null)
            {
                Log.Debug(unit.GetName() + "He was possessed by animalistic desires but could not find anyone to have sex with. He was injured after the poison took over.");
                int num3 = (int)((float)unit.data.unitData.propertyData.healthMax * 0.3f);
                if (num3 > unit.data.unitData.propertyData.health)
                {
                    unit.data.unitData.propertyData.health = 1;
                }
                else
                {
                    unit.data.unitData.propertyData.health -= num3;
                }
                return;
            }
            Log.Debug(unit.GetName() + "Intoxicated by beastly desires, intent on raping" + worldUnitBase.GetName());
            unit.MoveToTarget(worldUnitBase);
            if (worldUnitBase.IsPlayer())
            {
                Drama.OpenDramaAfterMonthRun(1811210040, new DramaFunctionData
                {
                    unitLeft = unit,
                    unitRight = worldUnitBase
                });
            }
            else if (worldUnitBase.IsVital(unit) || worldUnitBase.data.unitData.relationData.GetIntim(unit) >= 240)
            {
                unit.AddMonthLog("eventLogRoleManLustful001", worldUnitBase);
                worldUnitBase.AddMonthLog("eventLogRoleManLustful002", unit);
                Birth.Fuck(unit, worldUnitBase, isRape: true);
            }
            else if (FormulaTool.UnitPower.TotalPower(unit.data) >= FormulaTool.UnitPower.TotalPower(worldUnitBase.data))
            {
                unit.AddMonthLog("eventLogRoleManLustful005", worldUnitBase);
                worldUnitBase.AddMonthLog("eventLogRoleManLustful006", unit);
                Birth.Fuck(unit, worldUnitBase, isRape: true);
            }
            else
            {
                unit.AddMonthLog("eventLogRoleManLustful003", worldUnitBase);
                worldUnitBase.AddMonthLog("eventLogRoleManLustful004", unit);
            }
        }

        public static void HandleWoman(WorldUnitBase unit)
        {
            if (unit.isDie || !unit.IsWoman() || !unit.HasLuck(-523596344))
            {
                return;
            }
            if (unit.IsBitch())
            {
                Log.Debug(unit.GetName() + "She was possessed by the lust of a woman, and was found to be acting strangely by Gu Zhangtai, who took him to the backyard to breed with Dangkang and then solved the problem.");
                return;
            }
            WorldUnitBase worldUnitBase = null;
            if (unit.IsSexSlave())
            {
                worldUnitBase = g.world.playerUnit;
            }
            else
            {
                List<WorldUnitBase> unitExact = g.world.unit.GetUnitExact(unit.data.unitData.GetPoint(), 10, isGetHideUnit: false, isGetPlayer: true);
                int num = 0;
                List<WorldUnitBase>.Enumerator enumerator = unitExact.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    WorldUnitBase current = enumerator.Current;
                    if (current != unit && current.GetAge() >= 192 && current.data.unitData.propertyData.sex != unit.data.unitData.propertyData.sex)
                    {
                        if (worldUnitBase == null)
                        {
                            worldUnitBase = current;
                            num = unit.data.unitData.relationData.GetIntim(current);
                        }
                        else if (unit.data.unitData.relationData.GetIntim(current) < num)
                        {
                            worldUnitBase = current;
                            num = unit.data.unitData.relationData.GetIntim(current);
                        }
                    }
                }
            }
            if (worldUnitBase == null)
            {
                Log.Debug(unit.GetName() + "I was obsessed with a prostitute, but I never found anyone to have sex with. I was injured after being poisoned.");
                int num2 = (int)((float)unit.data.unitData.propertyData.healthMax * 0.3f);
                if (num2 > unit.data.unitData.propertyData.health)
                {
                    unit.data.unitData.propertyData.health = 1;
                }
                else
                {
                    unit.data.unitData.propertyData.health -= num2;
                }
                return;
            }
            Log.Debug(unit.GetName() + "Enchanted by lust, request" + worldUnitBase.GetName() + "Address sexual desire");
            unit.AddMonthLog("eventLogRoleLewd24328064", worldUnitBase);
            if (unit.IsSexSlave() && worldUnitBase.IsPlayer())
            {
                worldUnitBase.AddMonthLog("eventLogRoleLewd243280642", unit);
            }
            else
            {
                worldUnitBase.AddMonthLog("eventLogRoleLewd243280641", unit);
            }
            if (unit.IsPlayer())
            {
                Drama.OpenDramaAfterMonthRun(811210025, new DramaFunctionData
                {
                    dramaData = new DramaData
                    {
                        unitLeft = unit,
                        unitRight = worldUnitBase
                    }
                });
            }
            else if (worldUnitBase.IsPlayer())
            {
                Drama.OpenDramaAfterMonthRun(1811210040, new DramaFunctionData
                {
                    dramaData = new DramaData
                    {
                        unitLeft = unit,
                        unitRight = worldUnitBase
                    }
                });
            }
            else
            {
                Birth.Fuck(unit, worldUnitBase);
            }
        }

        public static void HandleFunction(DramaFunction dramaFunction, string[] data)
        {
            WorldUnitBase unitLeft = dramaFunction.data.unitLeft;
            WorldUnitBase unitRight = dramaFunction.data.unitRight;
            if (data[1] == "同意")
            {
                Birth.Fuck(unitLeft, unitRight);
                unitRight.AddMonthLog("eventLogRoleManLustful007", unitLeft);
                unitLeft.AddMonthLog("eventLogRoleManLustful008", unitRight);
            }
            else if (data[1] == "战斗")
            {
                unitLeft.SetStr("nymphomaniac", "true");
                unitLeft.CreateAction(new UnitActionRoleBattle(unitRight)
                {
                    forceAttack = true,
                    isBattleInsultUnit = true,
                    isBattleBackEscape = false,
                    state = 3
                });
            }
        }

        public static void HandleLustfulLuck(WorldUnitBase unit)
        {
            if (!unit.isDie)
            {
                if (unit.IsMan())
                {
                    HandleMan(unit);
                }
                else
                {
                    HandleWoman(unit);
                }
            }
        }
    }
}
