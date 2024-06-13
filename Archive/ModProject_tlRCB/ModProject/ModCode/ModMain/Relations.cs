using System.Text.RegularExpressions;

namespace MOD_tlRCB
{
    public static class Relations
    {
        public static bool IsVital(this WorldUnitBase unit, WorldUnitBase toUnit)
        {
            UnitRelationType relation = toUnit.data.unitData.relationData.GetRelation(unit);
            if ((uint)(relation - 1) <= 3u || (uint)(relation - 8) <= 1u)
            {
                return true;
            }
            return false;
        }

        public static bool IsFriend(this WorldUnitBase unit, WorldUnitBase toUnit)
        {
            return unit.data.unitData.relationData.IsFriend(toUnit);
        }

        public static bool IsMarried(this WorldUnitBase unit, WorldUnitBase target)
        {
            return unit.data.unitData.relationData.GetRelation(target) == UnitRelationType.Married;
        }

        public static void ChangedParent(this DataUnit.UnitInfoData unitData, WorldUnitBase parent, bool isTwoWay = false, int intim = 5000)
        {
            if (unitData != null && parent != null)
            {
                DataUnit.UnitInfoData unitData2 = parent.data.unitData;
                string text = unitData.relationData.parent[0];
                string text2 = unitData.relationData.parent[1];
                if (unitData2.propertyData.sex == UnitSexType.Man)
                {
                    unitData.relationData.parent = new string[2] { unitData2.unitID, text2 };
                }
                else
                {
                    unitData.relationData.parent = new string[2] { text, unitData2.unitID };
                }
                unitData2.relationData.SetIntim(unitData.unitID, intim);
                if (isTwoWay)
                {
                    unitData2.relationData.children.Add(unitData.unitID);
                    unitData.relationData.SetIntim(unitData2.unitID, intim);
                }
            }
        }

        public static void ChangedMarried(this WorldUnitBase unit, WorldUnitBase target, bool isTwoWay)
        {
            if (unit == null || target == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(unit.data.unitData.relationData.married) && !object.Equals(unit.data.unitData.relationData.married, target.data.unitData.unitID))
            {
                WorldUnitBase unit2 = g.world.unit.GetUnit(unit.data.unitData.relationData.married);
                if (unit2 != null)
                {
                    unit2.data.unitData.relationData.married = "";
                    unit2.data.unitData.relationData.intimToUnit.Remove(unit.data.unitData.unitID);
                    unit.data.unitData.relationData.intimToUnit.Remove(unit2.data.unitData.unitID);
                }
            }
            unit.data.unitData.relationData.married = target.data.unitData.unitID;
            unit.data.unitData.relationData.SetIntim(target.data.unitData.unitID, 300f);
            if (!isTwoWay)
            {
                return;
            }
            if (!string.IsNullOrEmpty(target.data.unitData.relationData.married) && !object.Equals(target.data.unitData.relationData.married, unit.data.unitData.unitID))
            {
                WorldUnitBase unit3 = g.world.unit.GetUnit(target.data.unitData.relationData.married);
                if (unit3 != null)
                {
                    unit3.data.unitData.relationData.married = "";
                    unit3.data.unitData.relationData.intimToUnit.Remove(target.data.unitData.unitID);
                    target.data.unitData.relationData.intimToUnit.Remove(unit3.data.unitData.unitID);
                }
            }
            target.data.unitData.relationData.married = unit.data.unitData.unitID;
            target.data.unitData.relationData.SetIntim(unit.data.unitData.unitID, 300f);
        }

        public static string Call(this WorldUnitBase unit, WorldUnitBase target)
        {
            string text = g.conf.roleCall.RoleCall(unit, target, new UnitConditionData(unit, target));
            if (text.Contains("@"))
            {
                string value = Regex.Match(text, "\\(([\\S]*)\\)").Value;
                return string.IsNullOrEmpty(value) ? UnitTrainPatch.Utils.GetRelation(unit.data.GetRelationType(target)) : value.Substring(1, value.Length - 2);
            }
            return text;
        }

        public static string RelationCall(this WorldUnitBase unit, WorldUnitBase target)
        {
            switch (unit.data.GetRelationType(target))
            {
                case UnitBothRelationType.None:
                    return "萍水相逢";
                case UnitBothRelationType.Parents:
                    return "父母";
                case UnitBothRelationType.Children:
                    return "子女";
                case UnitBothRelationType.Married:
                    return "夫妻";
                case UnitBothRelationType.Lover:
                    return "道侣";
                case UnitBothRelationType.Brother:
                    return "兄弟姐妹";
                case UnitBothRelationType.Master:
                    return "师傅";
                case UnitBothRelationType.Student:
                    return "徒弟";
                case UnitBothRelationType.Friend:
                    return "朋友";
                case UnitBothRelationType.Hater:
                    return "仇人";
                case UnitBothRelationType.School:
                    return "同门";
                case UnitBothRelationType.Stand:
                    return "立场对立";
                case UnitBothRelationType.Doctrine:
                    return "同教";
                default:
                    return "无";
            }
        }
    }
}
