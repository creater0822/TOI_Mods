using System;
using Il2CppSystem.Collections.Generic;

namespace MOD_LE2lAt
{
    internal class GradeDeal
    {
        public static GradeDeal ME = new GradeDeal();

        public void grade(DramaFunction __instance, string[] values)
        {
            WorldUnitBase unitRight = __instance.data.unitRight;
            if (values[2].Equals("gradeUp"))
            {
                gradeUpTo(unitRight, Convert.ToInt32(values[3]));
            }
        }

        public void gradeUpTo(WorldUnitBase unit, int grade)
        {
            int num = unit.data.dynUnitData.GetGrade();
            int num2 = grade - num;
            if (num2 >= 0)
            {
                for (int i = 0; i < num2; i++)
                {
                    num++;
                    upGrade(unit);
                }
            }
        }

        public void upGrade(WorldUnitBase unit)
        {
            ConsUtil.debug("upgrade");
            Il2CppSystem.Action<List<DataStruct<int, ConfRoleGradeItem>>> onGradeItems = (Action<List<DataStruct<int, ConfRoleGradeItem>>>)upgrade;
            WorldUnitAIAction1038.UpGrade(unit, onGradeItems);
        }

        public void upgrade(List<DataStruct<int, ConfRoleGradeItem>> list)
        {
            ConsUtil.debug("upgrade");
            ConsUtil.debug("upgrade", list);
        }
    }
}
