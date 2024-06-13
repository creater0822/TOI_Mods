using System;
using System.Collections.Generic;

namespace MOD_tlRCB
{
    internal static class SchoolHelper
    {
        private static void GiveMoney(string uuid, int money)
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(uuid);
            if (build == null)
            {
                return;
            }
            foreach (string text in build.buildData.GetPostUnit(SchoolPostType.None).Cast<Il2CppSystem.Collections.Generic.List<string>>())
            {
                WorldUnitBase unit = g.world.unit.GetUnit(text, true);
                if (unit != null)
                {
                    foreach (DataProps.PropsData propsData in unit.data.unitData.propData.allProps)
                    {
                        if (propsData.propsID == 10001)
                        {
                            propsData.propsCount += money;
                            break;
                        }
                    }
                }
            }
        }

        private static void ReviceSchoolUnit(List<string> schoolUnits)
        {
            foreach (string schoolUnit in schoolUnits)
            {
                WorldUnitBase unit = g.world.unit.GetUnit(schoolUnit);
                if (unit == null)
                {
                    continue;
                }
                int id = int.Parse(g.conf.roleDying.GetItem(unit.data.dynUnitData.GetGrade()).itemID);
                unit.data.RewardPropItem(id, 1, showUI: false);
                unit.CreateAction(new UnitActionRoleSave());
                Il2CppSystem.Collections.Generic.List<WorldUnitLuckBase>.Enumerator enumerator2 = unit.allLuck.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    WorldUnitLuckBase current2 = enumerator2.Current;
                    if (current2.luckData.id == 102)
                    {
                        current2.luckData.duration = 1;
                        break;
                    }
                }
            }
        }

        private static void SetBasType(string school, string weaponType)
        {
            if (string.IsNullOrWhiteSpace(weaponType))
            {
                Log.Debug("Set the sect’s skills. The type cannot be empty.");
                return;
            }
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(school);
            if (build != null)
            {
                MapBuildSchoolLibrary buildSub = build.GetBuildSub<MapBuildSchoolLibrary>();
                build.buildData.propertyData.fixAddBasTypes.Clear();
                buildSub.data.weaponType.Clear();
                buildSub.data.magicType.Clear();
                buildSub.data.weaponType.Add(weaponType);
                Il2CppSystem.Collections.Generic.List<string>.Enumerator enumerator = buildSub.data.weaponType.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    build.buildData.propertyData.fixAddBasTypes.Add(current);
                }
                enumerator = buildSub.data.magicType.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current2 = enumerator.Current;
                    build.buildData.propertyData.fixAddBasTypes.Add(current2);
                }
            }
        }

        public static void ExitSchool(this WorldUnitBase unit)
        {
            g.world.build.GetBuild<MapBuildSchool>(unit.data.unitData.schoolID).ExitSchool(unit);
        }

        private static void AddUnit(string schoolID, SchoolDepartmentType department, SchoolPostType post, List<string> units)
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(schoolID);
            if (build == null)
            {
                return;
            }
            foreach (string unit2 in units)
            {
                WorldUnitBase unit = g.world.unit.GetUnit(unit2);
                if (unit != null)
                {
                    if (!string.IsNullOrEmpty(unit.data.unitData.schoolID))
                    {
                        unit.ExitSchool();
                    }
                    build.buildData.postData.postElders[department].manageOut.Add(unit.data.unitData.unitID);
                    unit.data.unitData.schoolID = schoolID;
                    build.buildData.npcOut.Add(unit.data.unitData.unitID);
                    switch (post)
                    {
                        case SchoolPostType.SchoolMain:
                            build.buildData.npcSchoolMain = unit.data.unitData.unitID;
                            break;
                        case SchoolPostType.BigElders:
                            build.buildData.npcBigElders.Add(unit.data.unitData.unitID);
                            break;
                        case SchoolPostType.Elders:
                            build.buildData.npcElders.Add(unit.data.unitData.unitID);
                            break;
                        case SchoolPostType.Inherit:
                            build.buildData.npcInherit.Add(unit.data.unitData.unitID);
                            break;
                        case SchoolPostType.In:
                            build.buildData.npcIn.Add(unit.data.unitData.unitID);
                            break;
                        case SchoolPostType.Out:
                            build.buildData.npcOut.Add(unit.data.unitData.unitID);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("post", post, null);
                    }
                }
            }
        }

        public static void AddUnit(MapBuildSchool school, SchoolDepartmentType department, SchoolPostType post, List<string> units)
        {
            if (school == null)
            {
                return;
            }
            string id = school.buildData.id;
            foreach (string unit2 in units)
            {
                WorldUnitBase unit = g.world.unit.GetUnit(unit2);
                if (unit != null)
                {
                    if (!string.IsNullOrEmpty(unit.data.unitData.schoolID))
                    {
                        unit.ExitSchool();
                    }
                    school.buildData.postData.postElders[department].manageOut.Add(unit.data.unitData.unitID);
                    unit.data.unitData.schoolID = id;
                    school.buildData.npcOut.Add(unit.data.unitData.unitID);
                    switch (post)
                    {
                        case SchoolPostType.SchoolMain:
                            school.buildData.npcSchoolMain = unit.data.unitData.unitID;
                            break;
                        case SchoolPostType.BigElders:
                            school.buildData.npcBigElders.Add(unit.data.unitData.unitID);
                            break;
                        case SchoolPostType.Elders:
                            school.buildData.npcElders.Add(unit.data.unitData.unitID);
                            break;
                        case SchoolPostType.Inherit:
                            school.buildData.npcInherit.Add(unit.data.unitData.unitID);
                            break;
                        case SchoolPostType.In:
                            school.buildData.npcIn.Add(unit.data.unitData.unitID);
                            break;
                        case SchoolPostType.Out:
                            school.buildData.npcOut.Add(unit.data.unitData.unitID);
                            break;
                    }
                }
            }
        }

        public static void AddSchoolMain(string schoolID, string unitID)
        {
            MapBuildSchool build = g.world.build.GetBuild<MapBuildSchool>(schoolID);
            if (build == null)
            {
                return;
            }
            WorldUnitBase unit = g.world.unit.GetUnit(unitID);
            if (unit == null)
            {
                return;
            }
            DelSchoolUnit(build, SchoolDepartmentType.None, SchoolPostType.SchoolMain);
            if (!string.IsNullOrEmpty(unit.data.unitData.schoolID))
            {
                unit.ExitSchool();
            }
            MapBuildSchool topSchool = build.GetTopSchool();
            if (topSchool != null)
            {
                Il2CppSystem.Collections.Generic.List<MapBuildSchool>.Enumerator enumerator = topSchool.GetAllSchools(isIncludeSelf: true).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    MapBuildSchool current = enumerator.Current;
                    current.buildData.npcSchoolMain = unit.data.unitData.unitID;
                    current.buildData.postData.postMain.unitData.unitID = unit.data.unitData.unitID;
                    unit.data.unitData.schoolID = topSchool.buildData.id;
                }
            }
        }

        private static void DelSchoolUnit(MapBuildSchool school, SchoolDepartmentType departmentType, SchoolPostType postType)
        {
            foreach (string item in GetSchoolUnit(school, departmentType, postType))
            {
                g.world.unit.GetUnit(item)?.ExitSchool();
            }
        }

        private static void DelSchoolUnit(string uuid)
        {
            g.world.unit.GetUnit(uuid)?.ExitSchool();
        }

        private static List<string> GetSchoolUnit(MapBuildSchool school, SchoolDepartmentType departmentType, SchoolPostType postType)
        {
            switch (postType)
            {
                case SchoolPostType.SchoolMain:
                    return new List<string> { school.buildData.npcSchoolMain };
                case SchoolPostType.BigElders:
                    {
                        List<string> list = new List<string>();
                        Il2CppSystem.Collections.Generic.Dictionary<SchoolDepartmentType, DataBuildSchool.PostData.PostElders>.Enumerator enumerator = school.buildData.postData.postElders.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            Il2CppSystem.Collections.Generic.KeyValuePair<SchoolDepartmentType, DataBuildSchool.PostData.PostElders> current = enumerator.Current;
                            list.Add(current.value.unitData.unitID);
                        }
                        return list;
                    }
                default:
                    return GetElderUnit(school, departmentType);
            }
        }

        private static List<string> GetElderUnit(MapBuildSchool school, SchoolDepartmentType departmentType)
        {
            List<string> list = new List<string>();
            if (school.buildData.postData.postElders[departmentType].unitData != null)
            {
                list.Add(school.buildData.postData.postElders[departmentType].unitData.unitID);
            }
            return list;
        }

        private static void SetSloganItemType(MapBuildSchool school, int type1_1, int type1_2, int type2_1, int type2_2)
        {
            school.schoolData.buildSchoolData.schoolSloganItem1Type1 = type1_1;
            school.schoolData.buildSchoolData.schoolSloganItem1Type2 = type1_2;
            school.schoolData.buildSchoolData.schoolSloganItem2Type1 = type2_1;
            school.schoolData.buildSchoolData.schoolSloganItem2Type2 = type2_2;
            school.schoolData.SetAdvantage();
        }
    }
}
