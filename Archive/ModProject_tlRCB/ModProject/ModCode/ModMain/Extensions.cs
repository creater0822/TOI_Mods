using System;
using System.Collections.Generic;
using HarmonyLib;
using Newtonsoft.Json;

namespace MOD_tlRCB
{
    public static class Extensions
    {
        public static bool IsPlayer(this WorldUnitBase unit)
        {
            return unit.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID;
        }

        public static bool HasLuck(this WorldUnitBase unit, int luckId)
        {
            return unit.GetLuck(luckId) != null;
        }

        public static WorldUnitLuckBase FindLuck(this DataUnit.UnitInfoData unitInfo, int luckId)
        {
            return unitInfo.unit.GetLuck(luckId);
        }

        public static void AddLuck(this WorldUnitBase unit, int luckId)
        {
            unit.CreateAction(new UnitActionLuckAdd(luckId));
        }

        public static void AddLuck(this WorldUnitBase unit, int luckId, int duration)
        {
            unit.CreateAction(new UnitActionLuckAdd(luckId)
            {
                duration = duration
            });
            JsonConvert.SerializeObject(g.conf.roleCreateFeature.GetItem(-2040743491));
        }

        public static void RemoveLuck(this WorldUnitBase unit, int luckId)
        {
            unit.CreateAction(new UnitActionLuckDel(luckId));
        }

        public static string GetName(this WorldUnitBase unit)
        {
            return unit.data.unitData.propertyData.GetName();
        }

        public static string GetID(this WorldUnitBase unit)
        {
            return unit.data.unitData.unitID;
        }

        public static void Percent(this int percent, Action action)
        {
            if (CommonTool.Random(0, 100) <= percent)
            {
                action();
            }
        }

        public static bool IsBitch(this WorldUnitBase unit)
        {
            Il2CppSystem.Collections.Generic.List<WorldUnitLuckBase>.Enumerator enumerator = unit.allLuck.GetEnumerator();
            while (enumerator.MoveNext())
            {
                WorldUnitLuckBase current = enumerator.Current;
                if (current.luckData.id >= 527000001 && current.luckData.id <= 527000006)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSexSlave(this WorldUnitBase unit)
        {
            if (!unit.HasLuck(-1019384845))
            {
                return unit.HasLuck(582935472);
            }
            return true;
        }

        public static Dictionary<string, string> ListFuckers(this WorldUnitBase unit)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string str = unit.GetStr("做爱名单");
            if (string.IsNullOrWhiteSpace(str))
            {
                return dictionary;
            }
            string[] array = str.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split('_');
                dictionary.Add(array2[0], array2[1]);
            }
            return dictionary;
        }

        public static int AddFucker(this WorldUnitBase unit, WorldUnitBase fucker)
        {
            return unit.AddFucker(fucker.GetID(), fucker.GetName());
        }

        private static int AddFucker(this WorldUnitBase unit, string id, string name)
        {
            Dictionary<string, string> dictionary = unit.ListFuckers();
            if (dictionary.ContainsKey(id))
            {
                return -1;
            }
            switch (dictionary.Count + 1)
            {
                case 100:
                    Log.Debug(unit.GetName() + "达成百人斩");
                    unit.AddMonthLog("eventLogRolelewd24328062");
                    unit.AddVitalLog("eventLogRolelewd24328062");
                    unit.AddLuck(1363739398);
                    break;
                case 1000:
                    Log.Debug(unit.GetName() + "达成千人斩");
                    unit.AddMonthLog("eventLogRolelewd24328063");
                    unit.AddVitalLog("eventLogRolelewd24328063");
                    unit.AddLuck(1511891858);
                    break;
            }
            dictionary.Add(id, name);
            unit.SetStr("做爱名单", dictionary.Join((KeyValuePair<string, string> p) => p.Key + "_" + p.Value, "|"));
            return dictionary.Count;
        }

        public static string GetFirstManName(this WorldUnitBase unit)
        {
            string str = unit.GetStr("破处者_name");
            if (!string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            string str2 = unit.GetStr("破处者");
            if (string.IsNullOrWhiteSpace(str2))
            {
                return "无";
            }
            if (str2 == "-1")
            {
                return "未知";
            }
            DataUnitDie.DieData unitOrDie = g.data.unitDie.GetUnitOrDie(str2);
            if (unitOrDie == null)
            {
                return "未知";
            }
            string name = unitOrDie.GetName();
            unit.SetStr("破处者_name", name);
            return name;
        }

        public static void SetFirstMan(this WorldUnitBase unit, string unitId, string name)
        {
            unit.SetStr("破处者", unitId);
            unit.SetStr("破处者_name", name);
            unit.AddFucker(unitId, name);
        }

        public static int InRange(this int value, int min, int max)
        {
            if (value > max)
            {
                return max;
            }
            if (value >= min)
            {
                return value;
            }
            return min;
        }

        public static int AddBrothelFucker(this WorldUnitBase target, WorldUnitBase fucker)
        {
            Dictionary<string, string> dictionary = target.ListBrothelFucker();
            if (dictionary.ContainsKey(fucker.GetID()))
            {
                return -1;
            }
            dictionary.Add(fucker.GetID(), fucker.GetName());
            target.AddFucker(fucker);
            target.SetStr("嫖客", dictionary.Join((KeyValuePair<string, string> p) => p.Key + "_" + p.Value, "|"));
            return dictionary.Count;
        }

        public static Dictionary<string, string> ListBrothelFucker(this WorldUnitBase target)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string str = target.GetStr("嫖客");
            if (string.IsNullOrWhiteSpace(str))
            {
                return dictionary;
            }
            string[] array = str.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split('_');
                dictionary.Add(array2[0], array2[1]);
            }
            return dictionary;
        }

        public static void SetSeller(this WorldUnitBase target, WorldUnitBase trafficker)
        {
            target.SetStr("拐卖者", trafficker.GetID() + "_" + trafficker.GetName());
        }

        public static System.Collections.Generic.KeyValuePair<string, string>? GetSeller(this WorldUnitBase target)
        {
            string str = target.GetStr("拐卖者");
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            string[] array = str.Split('_');
            return new System.Collections.Generic.KeyValuePair<string, string>(array[0], array[1]);
        }
    }
}
