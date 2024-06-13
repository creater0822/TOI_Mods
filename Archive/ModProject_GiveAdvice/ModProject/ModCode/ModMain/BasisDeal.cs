using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MOD_LE2lAt
{
    internal class BasisDeal
    {
        public static void dramaShareBasis(DramaFunction __instance, string[] values)
        {
            WorldUnitBase playerUnit = g.world.playerUnit;
            WorldUnitBase unitRight = __instance.data.unitRight;
            values[2].Equals(1);
        }

        public static void dramaBasis(WorldUnitBase player, WorldUnitBase toUnit, int type)
        {
            List<OptionData> list = new List<OptionData>();
            if (type == 1)
            {
                Action action = delegate
                {
                };
                if (player.data.dynUnitData.basisBlade.value > toUnit.data.dynUnitData.basisBlade.value)
                {
                    OptionData optionData = new OptionData();
                    optionData.text = "Blade";
                    optionData.onClick = delegate
                    {
                    };
                    list.Add(optionData);
                }
                DynInt[] array = toUnit.data.dynUnitData.allBasWp;
                DynInt[] array2 = toUnit.data.dynUnitData.allBasWp;
                for (int i = 0; i < array.Length; i++)
                {
                    int value = array2[i].value;
                    int value2 = array[i].value;
                }
            }
        }

        public void shareBasis()
        {
        }
    }
}
