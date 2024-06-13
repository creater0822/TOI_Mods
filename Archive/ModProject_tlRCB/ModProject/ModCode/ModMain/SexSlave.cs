using System;
using System.Collections.Generic;
using System.Linq;
using EGameTypeData;
using MOD_tlRCB.EvilFall;

namespace MOD_tlRCB
{
    public class SexSlave : IDisposable
    {
        public SexSlave()
        {
            Events.On(EGameType.OpenDrama, OpenDrama);
            g.events.On(EGameType.OneOpenUIEnd(UIType.IntoGameInfo), (Action<ETypeData>)OneOpenDrama610405, 1);
        }

        private static void OpenDrama(ETypeData data)
        {
            OpenDrama openDrama = data.Cast<OpenDrama>();
            int num = -1653090755;
            int num2 = -31412564;
            if (openDrama.dramaID == num || openDrama.dramaID == num2)
            {
                WorldUnitBase playerUnit = g.world.playerUnit;
                WorldUnitBase unitRight = openDrama.dramaData.unitRight;
                WorldUnitBase target = (unitRight.IsWoman() ? unitRight : playerUnit);
                Birth.Fuck(unitRight.IsWoman() ? playerUnit : unitRight, target, isRape: true, openDrama.dramaID == num2);
            }
        }

        private static void ReplaceBornLuck(WorldUnitBase unit, params int[] oldList)
        {
            List<DataUnit.LuckData> list = unit.data.unitData.propertyData.bornLuck.ToList();
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                DataUnit.LuckData luckData = list[i];
                if (oldList.Contains(luckData.id))
                {
                    unit.DestroyLuck(unit.GetLuck(luckData.id));
                    int num = -1;
                    do
                    {
                        num = g.conf.roleCreateFeature.RandomItem(1, 1).ToArray()[0].id;
                    }
                    while (num == -1 || oldList.Contains(num));
                    DataUnit.LuckData luckData2 = new DataUnit.LuckData
                    {
                        id = num,
                        createTime = g.world.run.roundMonth,
                        duration = -1,
                        objData = new DataObjectData()
                    };
                    unit.CreateLuck(luckData2);
                    list[i] = luckData2;
                    Log.Debug($"{unit.GetName()} 替换luck {luckData.id} 为 {num}");
                }
            }
            unit.data.unitData.propertyData.bornLuck = list.ToArray();
        }

        private static void OneOpenDrama610405(ETypeData e)
        {
            try
            {
                Il2CppSystem.Collections.Generic.Dictionary<string, WorldUnitBase>.ValueCollection.Enumerator enumerator = g.world.unit.allUnit.values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    WorldUnitBase current = enumerator.Current;
                    if (current.IsWoman())
                    {
                        ReplaceBornLuck(current, -527021126);
                        if (current.data.unitData.propertyData.bornLuck.Any((DataUnit.LuckData it) => it.id == 1561118811))
                        {
                            current.AddDepravation(301);
                        }
                        DataUnit.UnitInfoData unitData = current.data.unitData;
                        if (unitData.relationData.children.Count > 0 || unitData.relationData.childrenPrivate.Count > 0)
                        {
                            current.SetFirstMan("-1", "未知");
                            current.RemoveLuck(-527021125);
                        }
                        else if (!string.IsNullOrWhiteSpace(unitData.relationData.married))
                        {
                            WorldUnitBase unit = g.world.unit.GetUnit(unitData.relationData.married);
                            if (unit != null && unit.data.unitData.propertyData.bornLuck.Any((DataUnit.LuckData it) => it.id == -527021126))
                            {
                                current.SetFirstMan("", "");
                                current.AddLuck(-527021125);
                            }
                            else
                            {
                                current.SetFirstMan(unitData.relationData.married, g.data.unitDie.GetUnitOrDie(unitData.relationData.married)?.GetName() ?? "未知已死亡");
                                current.RemoveLuck(-527021125);
                            }
                        }
                        else
                        {
                            current.AddLuck(-527021125);
                        }
                    }
                    else if (current.IsMan())
                    {
                        if (current.data.unitData.relationData.children.Count > 0 || current.data.unitData.relationData.childrenPrivate.Count > 0)
                        {
                            ReplaceBornLuck(current, -527021126);
                        }
                        ReplaceBornLuck(current, 1561118811);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("替换先天失败：" + ex);
            }
        }

        public void Dispose()
        {
            Events.Off(EGameType.OpenDrama, OpenDrama);
        }

        public static DataUnit.UnitInfoData InitNpc(in DataUnit.UnitInfoData __result)
        {
            if (__result != null && __result.propertyData.sex == UnitSexType.Woman)
            {
                if (!string.IsNullOrWhiteSpace(__result.relationData.married))
                {
                    __result.propertyData.DelAddLuck(-527021125);
                    __result.objData.SetString("淫女宫", "破处者", __result.relationData.married);
                }
                else if (__result.relationData.children.Count > 0 || __result.relationData.childrenPrivate.Count > 0)
                {
                    __result.propertyData.DelAddLuck(-527021125);
                    __result.unit.SetFirstMan("-1", "未知");
                }
                else
                {
                    __result.propertyData.AddAddLuck(new DataUnit.LuckData
                    {
                        id = -527021125,
                        duration = -1
                    });
                }
            }
            return __result;
        }
    }
}
