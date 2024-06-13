using System;

namespace MOD_LE2lAt
{
    internal class ShareAllDeal
    {
        public static ShareAllDeal ME = new ShareAllDeal();

        private int needCost;

        public void dramaWantShareAll(DramaFunction __instance, string[] values)
        {
            WorldUnitBase playerUnit = g.world.playerUnit;
            WorldUnitBase unitRight = __instance.data.unitRight;
            int grade = playerUnit.data.dynUnitData.GetGrade();
            int grade2 = unitRight.data.dynUnitData.GetGrade();
            int num = grade - grade2;
            if (num < 0)
            {
                DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.toShareAll_false");
                return;
            }
            needCost = 10 + num * 10;
            DramaFunctionTool.OptionsFunction("setWorldData_LE2lAt.toShareAll_true");
            string text = "Confirm initiation (consume" + needCost + "life)";
            g.conf.dramaOptions.GetItem(ModMain.mid() + 6020).text = text;
        }

        public void dramaShareAll(DramaFunction __instance, string[] values)
        {
            WorldUnitBase playerUnit = g.world.playerUnit;
            WorldUnitBase unitRight = __instance.data.unitRight;
            playerUnit.data.dynUnitData.AddEffectBaseValues("life", 1, -needCost * 12, isShowUI: false);
            shareAll(playerUnit, unitRight);
            clear();
        }

        public void shareBasAbility(DramaFunction __instance, string[] values)
        {
            WorldUnitBase playerUnit = g.world.playerUnit;
            WorldUnitBase unitRight = __instance.data.unitRight;
            shareBasAbility(playerUnit, unitRight);
        }

        public void shareAll(WorldUnitBase unit, WorldUnitBase toUnit)
        {
            int grade = toUnit.data.dynUnitData.GetGrade();
            int grade2 = unit.data.dynUnitData.GetGrade();
            GradeDeal.ME.gradeUpTo(toUnit, grade2);
            if (grade2 > grade)
            {
                for (int i = 120; i < 140; i++)
                {
                    if (toUnit.GetLuck(i) != null)
                    {
                        toUnit.CreateAction(new UnitActionLuckDel(i));
                        break;
                    }
                }
            }
            shareBasAbility(unit, toUnit);
            shareBasAbilityPercent(unit, toUnit);
        }

        public void setHero(WorldUnitBase toUnit)
        {
            if (!toUnit.data.unitData.heart.IsHeroes())
            {
                try
                {
                    toUnit.data.unitData.heart.InitHeart(toUnit, null, 0, 0);
                }
                catch (Exception ex)
                {
                    ConsUtil.console("Failure in imparting Taoism", ex.Message);
                    ConsUtil.console("Failure in imparting Taoism", ex.StackTrace);
                }
            }
        }

        public int needAddValue(int v1, int v2)
        {
            if (v1 <= v2)
            {
                return 0;
            }
            return v1 - v2;
        }

        public void shareBasAbility(WorldUnitBase unit, WorldUnitBase toUnit)
        {
            WorldUnitDynData dynUnitData = unit.data.dynUnitData;
            WorldUnitDynData dynUnitData2 = toUnit.data.dynUnitData;
            string[] array = "basPil,basEqp,basGeo,basSym,basHerb,basMine,basSword,basSpear,basBlade,basFist,basPalm,basFinger,basFire,basFroze,basThunder,basWind,basEarth,basWood,abilityPoint".Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                int value = dynUnitData.GetValues(array[i])[0].value;
                int value2 = dynUnitData2.GetValues(array[i])[0].value;
                int value3 = ((value > value2) ? (value - value2) : 0);
                dynUnitData2.AddEffectBaseValues(array[i], 1, value3, isShowUI: false);
            }
        }

        public void shareBasAbilityPercent(WorldUnitBase unit, WorldUnitBase toUnit)
        {
            WorldUnitDynData dynUnitData = unit.data.dynUnitData;
            WorldUnitDynData dynUnitData2 = toUnit.data.dynUnitData;
            string[] array = "hpMax,mpMax,spMax,dpMax,crit,guard,critV,guardV,atk,def".Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                int value = dynUnitData.GetValues(array[i])[0].value;
                int value2 = dynUnitData2.GetValues(array[i])[0].value;
                int value3 = ((value > value2) ? (value * 10 / 100) : 0);
                dynUnitData2.AddEffectBaseValues(array[i], 1, value3, isShowUI: false);
            }
        }

        public void clear()
        {
            needCost = 0;
        }
    }
}
