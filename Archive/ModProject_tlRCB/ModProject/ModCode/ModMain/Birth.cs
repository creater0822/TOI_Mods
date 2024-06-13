using System;
using Il2CppSystem.Collections.Generic;
using MOD_tlRCB.EvilFall;
using MOD_tlRCB.Rape;

namespace MOD_tlRCB
{
    public class Birth : IDisposable
    {
        internal static List<WorldUnitBase> _dogs = new List<WorldUnitBase>();

        public void Dispose()
        {
        }

        public static void OnMonthEnd(ETypeData e)
        {
            Log.Split();
            List<WorldUnitBase> list = new List<WorldUnitBase>();
            Dictionary<string, WorldUnitBase>.ValueCollection.Enumerator enumerator = g.world.unit.allUnit.values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                WorldUnitBase current = enumerator.Current;
                if (!current.isDie)
                {
                    WorldUnitLuckBase luck = current.GetLuck(744093962);
                    if (luck != null)
                    {
                        Log.Debug($"怀孕母狗：{current.GetName()}, 怀孕时长：{Config.Instance.BirthSettings.PregnancyDuration - luck.luckData.duration + 1}");
                        if (luck.luckData.duration == 1)
                        {
                            list.Add(current);
                        }
                    }
                }
                LustfulLuck.HandleLustfulLuck(current);
                EvilFall.EvilFall.HandleSexToys(current);
                Brothel.HandleCheckKidnapping(current);
            }
            List<WorldUnitBase>.Enumerator enumerator2 = list.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                Births(enumerator2.Current);
            }
        }

        public static void OnMonthStart(ETypeData e)
        {
        }

        private static void Births(WorldUnitBase dog)
        {
            WorldUnitBase playerUnit = g.world.playerUnit;
            string str = dog.GetStr("被谁操");
            WorldUnitBase worldUnitBase = null;
            if (string.IsNullOrWhiteSpace(str) || str == "-1")
            {
                if (dog.IsSexSlave())
                {
                    worldUnitBase = playerUnit;
                }
            }
            else
            {
                worldUnitBase = g.world.unit.GetUnit(str);
            }
            Log.Debug("[" + dog.GetName() + "]开始分娩");
            UnitSexType unitSexType = ((worldUnitBase == null || !worldUnitBase.IsPlayer() || !Config.Instance.BirthSettings.ChildMustBeWomen) ? ((CommonTool.Random(0, 100) <= Config.Instance.BirthSettings.ChildIsMalePercent) ? UnitSexType.Man : UnitSexType.Woman) : UnitSexType.Woman);
            int npcSpecialID = 1008612;
            if (worldUnitBase != null && worldUnitBase.IsPlayer())
            {
                npcSpecialID = ((dog.IsSexSlave() && Config.Instance.SexSlaveSettings.SexSlaveInherit && unitSexType == UnitSexType.Woman) ? 1008613 : 1008611);
            }
            WorldUnitBase worldUnitBase2 = Units.CreateChild(npcSpecialID, worldUnitBase, dog, unitSexType, Config.Instance.BirthSettings.NewbornGrowUp16 ? 192 : 12);
            worldUnitBase2.SetFaceInherit(worldUnitBase, dog, Config.Instance.BirthSettings.FaceInheritPercent);
            dog.AddLuck(-1892102884);
            worldUnitBase2.AddLuck(-748164927);
            BringUp.BabyList.Add(worldUnitBase2);
            if (dog.IsSexSlave())
            {
                if (worldUnitBase2.IsWoman() && Config.Instance.SexSlaveSettings.SexSlaveChildMarryFather)
                {
                    worldUnitBase2.ChangedMarried(playerUnit, isTwoWay: false);
                }
                Log.Debug("母狗[" + dog.GetName() + "]生下了新的精盆[" + worldUnitBase2.GetName() + "(" + worldUnitBase2.data.unitData.unitID + ")]");
                Drama.OpenDrama(dog, 1878250177, dog.Call(worldUnitBase) + ",骚逼为您生下了小母狗" + worldUnitBase2.GetName() + "，请您有空来操我们");
            }
            else if (worldUnitBase != null && worldUnitBase.IsPlayer())
            {
                string text = "<color=#BC1717>" + dog.GetName() + "</color>生下了<color=#BC1717>" + worldUnitBase.GetName() + "</color>的孩子<color=#BC1717>" + worldUnitBase2.GetName() + "</color>";
                Log.Debug(text);
                UICostItemTool.AddTipText(text);
            }
            dog.AddMonthLog("eventLogRoleHuaiYun24328056", (worldUnitBase != null) ? dog.LogCall(worldUnitBase) : "不知道谁", worldUnitBase2.LogName());
        }

        public static void Fuck(WorldUnitBase fucker, WorldUnitBase target, bool isRape = false, bool isCreampie = false, bool showDrama = false)
        {
            if (fucker.IsMan() && target.IsMan())
            {
                Log.Debug("你怎么搞两个男人来搞基啊！他喵的这块逻辑老子不做！" + fucker.GetName() + "-" + target.GetName());
                return;
            }
            bool flag = false;
            WorldUnitBase worldUnitBase;
            WorldUnitBase worldUnitBase2;
            if (fucker.IsWoman())
            {
                flag = true;
                worldUnitBase = target;
                worldUnitBase2 = fucker;
            }
            else
            {
                flag = false;
                worldUnitBase = fucker;
                worldUnitBase2 = target;
            }
            int num = 0;
            if (worldUnitBase2.GetInt("corruption_val") < 296)
            {
                num += 5;
            }
            if (worldUnitBase2.AddFucker(worldUnitBase) > -1)
            {
                num += 20;
            }
            if (isRape)
            {
                fucker.Rape(target);
                if (!flag)
                {
                    fucker.AddMonthLog("eventLogRoleRape24328059", target);
                    if (target.HasLuck(-527021125))
                    {
                        target.data.unitData.relationData.AddHate(fucker.data.unitData.unitID, 200f, 0);
                        if (fucker.IsPlayer() || target.IsFriend(g.world.playerUnit) || target.IsVital(g.world.playerUnit))
                        {
                            UICostItemTool.AddTipText("<color=#BC1717>" + target.GetName() + "</color>被<color=#BC1717>" + fucker.GetName() + "</color>强奸并破处了！");
                        }
                        target.AddMonthLog("eventLogRoleRape24328051", fucker);
                        target.AddVitalLog("eventLogRoleRape24328051", fucker);
                    }
                    else
                    {
                        target.AddMonthLog("eventLogRoleRape24328050", fucker);
                    }
                    num += 10;
                    if (target.HasLuck(1561118811))
                    {
                        num += 10;
                    }
                }
                else
                {
                    if (fucker.HasLuck(-527021125))
                    {
                        target.data.unitData.relationData.AddIntim(fucker.data.unitData.unitID, 200f, 0);
                        worldUnitBase.AddMonthLog("eventLogRoleRape24328061", worldUnitBase2);
                        if (worldUnitBase2.HasLuck(1561118811))
                        {
                            num += 10;
                        }
                    }
                    else
                    {
                        fucker.AddMonthLog("eventLogRoleRape24328059", target);
                        target.AddMonthLog("eventLogRoleRape24328050", fucker);
                    }
                    num += 20;
                    if (worldUnitBase2.HasLuck(1561118811))
                    {
                        num += 10;
                    }
                }
            }
            if (worldUnitBase2.HasLuck(-527021125) || string.IsNullOrWhiteSpace(worldUnitBase2.GetStr("破处者")))
            {
                worldUnitBase2.SetFirstMan(worldUnitBase.GetID(), worldUnitBase.GetName());
                worldUnitBase2.RemoveLuck(-527021125);
                if (!isRape)
                {
                    worldUnitBase2.AddMonthLog("eventLogRoleRape24328060", worldUnitBase);
                    worldUnitBase2.AddVitalLog("eventLogRoleRape24328060", worldUnitBase);
                }
            }
            worldUnitBase2.AddDepravation(num);
            WorldUnitLuckBase luck = worldUnitBase2.GetLuck(744093962);
            if (luck != null)
            {
                if (Config.Instance.WillAbortion(luck.luckData.duration))
                {
                    Log.Debug(fucker.GetName() + "与" + target.GetName() + "孕期双修， " + worldUnitBase2.GetName() + "流产了");
                    worldUnitBase2.AddMonthLog(isRape ? "eventLogRoleHuaiYun24328055" : "eventLogRoleHuaiYun24328053", fucker);
                    int @int = worldUnitBase2.GetInt("流产次数");
                    if (@int > Config.Instance.BirthSettings.AbortionCount)
                    {
                        worldUnitBase2.AddLuck(-336709509);
                        Log.Debug("母狗" + worldUnitBase2.GetName() + "流产次数过多，已经不会怀孕了");
                        if (fucker.IsPlayer())
                        {
                            UICostItemTool.AddTipText("<color=#BC1717>" + worldUnitBase2.GetName() + "</color>流产且不会再怀孕了");
                        }
                        worldUnitBase2.AddMonthLog("eventLogRoleHuaiYun24328054");
                    }
                    else
                    {
                        worldUnitBase2.AddLuck(-1782743473);
                        worldUnitBase2.SetInt("流产次数", @int + 1);
                        Log.Debug($"母狗{worldUnitBase2.GetName()}流产{@int + 1}次");
                        if (fucker.IsPlayer())
                        {
                            UICostItemTool.AddTipText("<color=#BC1717>" + worldUnitBase2.GetName() + "</color>流产了");
                        }
                    }
                    worldUnitBase2.RemoveLuck(744093962);
                    if (showDrama && fucker.IsPlayer())
                    {
                        if (!flag)
                        {
                            if (isRape)
                            {
                                Drama.OpenDrama(target, 1122977303, "啊，子宫好痛啊！" + target.Call(fucker) + ", 你这个淫贼，我的孩子，我的孩子被你操没了");
                            }
                            else
                            {
                                DramaTool.OpenDrama(1122977303, new DramaData
                                {
                                    unitRight = target,
                                    _unitRight = target
                                });
                            }
                        }
                        else
                        {
                            Drama.OpenDrama(target, 1122977303, "啊，你的鸡巴好大，捅到子宫里了，我还有……啊~快到了，快，加快点，捅死骚逼吧！");
                        }
                    }
                }
                else if (Config.Instance.WillPrematureBirth(luck.luckData.duration))
                {
                    WorldUnitBase worldUnitBase3 = worldUnitBase2;
                    WorldUnitBase playerUnit = g.world.playerUnit;
                    string str = worldUnitBase2.GetStr("被谁操");
                    WorldUnitBase worldUnitBase4 = null;
                    if (string.IsNullOrWhiteSpace(str) || str == "-1")
                    {
                        if (worldUnitBase2.IsSexSlave() && !worldUnitBase2.IsPlayer())
                        {
                            worldUnitBase4 = playerUnit;
                        }
                    }
                    else
                    {
                        worldUnitBase4 = g.world.unit.GetUnit(str);
                    }
                    string text = ((worldUnitBase4 != null) ? ("<color=#BC1717>" + worldUnitBase3.GetName() + "</color>与<color=#BC1717>" + fucker.GetName() + "</color>孕期性交导致早产, 生下了<color=#BC1717>" + worldUnitBase4.GetName() + "</color>的孩子") : ("<color=#BC1717>" + worldUnitBase3.GetName() + "</color>与<color=#BC1717>" + fucker.GetName() + "</color>孕期性交导致早产, 生下没有父亲的野种"));
                    Log.Debug(text);
                    worldUnitBase2.AddMonthLog("eventLogRoleHuaiYun24328057", worldUnitBase);
                    if (fucker.IsPlayer())
                    {
                        UICostItemTool.AddTipText(text);
                    }
                    worldUnitBase3.AddLuck(-1892102884);
                    worldUnitBase3.RemoveLuck(744093962);
                    UnitSexType unitSexType = ((worldUnitBase4 == null || !worldUnitBase4.IsPlayer() || !Config.Instance.BirthSettings.ChildMustBeWomen) ? ((CommonTool.Random(0, 100) <= Config.Instance.BirthSettings.ChildIsMalePercent) ? UnitSexType.Man : UnitSexType.Woman) : UnitSexType.Woman);
                    int npcSpecialID = 1008612;
                    if (worldUnitBase4 != null && worldUnitBase4.IsPlayer())
                    {
                        npcSpecialID = ((worldUnitBase3.IsSexSlave() && Config.Instance.SexSlaveSettings.SexSlaveInherit && unitSexType == UnitSexType.Woman) ? 1008613 : 1008611);
                    }
                    WorldUnitBase worldUnitBase5 = Units.CreateChild(npcSpecialID, worldUnitBase4, worldUnitBase3, unitSexType, Config.Instance.BirthSettings.NewbornGrowUp16 ? 192 : 12);
                    worldUnitBase5.SetFaceInherit(worldUnitBase4, worldUnitBase3, Config.Instance.BirthSettings.FaceInheritPercent);
                    worldUnitBase5.AddLuck(-748164927);
                    BringUp.BabyList.Add(worldUnitBase5);
                    if (worldUnitBase3.IsSexSlave())
                    {
                        if (worldUnitBase5.IsWoman() && Config.Instance.SexSlaveSettings.SexSlaveChildMarryFather)
                        {
                            worldUnitBase5.ChangedMarried(playerUnit, isTwoWay: false);
                        }
                        Log.Debug("母狗[" + worldUnitBase3.GetName() + "]生下了新的精盆[" + worldUnitBase5.GetName() + "(" + worldUnitBase5.data.unitData.unitID + ")]");
                        if (showDrama)
                        {
                            Drama.OpenDrama(worldUnitBase3, 1878250177, worldUnitBase3.Call(worldUnitBase4) + ",骚逼为您生下了小母狗" + worldUnitBase5.GetName() + "，请您有空来操我们");
                        }
                    }
                    worldUnitBase3.AddMonthLog("eventLogRoleHuaiYun24328056", (worldUnitBase4 != null) ? worldUnitBase3.LogCall(worldUnitBase4) : "不知道谁", worldUnitBase5.LogName());
                    if (_dogs.Contains(worldUnitBase3))
                    {
                        _dogs.Remove(worldUnitBase3);
                    }
                }
            }
            else if (!worldUnitBase2.HasLuck(-336709509) && !worldUnitBase2.HasLuck(-1782743473) && !worldUnitBase2.HasLuck(321486433) && worldUnitBase2.GetAge() >= 192 && (isCreampie || CommonTool.Random(1, 100) <= Config.Instance.BirthSettings.PregnancyPercent))
            {
                Log.Debug("[" + fucker.GetName() + "]与[" + target.GetName() + "]双修，[" + worldUnitBase2.GetName() + "]怀孕了");
                worldUnitBase2.AddLuck(744093962, Config.Instance.BirthSettings.PregnancyDuration);
                worldUnitBase2.SetStr("被谁操", worldUnitBase.data.unitData.unitID);
                worldUnitBase2.AddMonthLog("eventLogRoleHuaiYun24328052", worldUnitBase);
            }
            worldUnitBase.RemoveLuck(1840247464);
            worldUnitBase2.RemoveLuck(-523596344);
        }

        public static void HandleHuanYunBuffDel(UnitActionLuckDel instance)
        {
            if (instance.luckID == 744093962)
            {
                Log.Debug("---> " + instance.unit.GetName() + " 怀孕buff消失 " + $"死亡 {instance.unit.isDie} " + string.Format("|早产 {0} ", instance.unit.GetInt("早产")) + $"|流产 {instance.unit.HasLuck(-1782743473) || instance.unit.HasLuck(-336709509)}");
                if (!instance.unit.isDie && !instance.unit.HasLuck(-1782743473) && !instance.unit.HasLuck(-336709509) && instance.unit.GetInt("早产") >= 1)
                {
                    instance.unit.SetInt("早产", 0);
                    Births(instance.unit);
                }
            }
        }

        public static void HandleUnitDiying(WorldUnitBase unit)
        {
            Log.Debug(unit.GetName() + "怀孕时死于非命");
            unit.AddLuck(10100);
            unit.RemoveLuck(-1892102884);
            unit.RemoveLuck(744093962);
            unit.RemoveLuck(-2040743491);
        }
    }
}
