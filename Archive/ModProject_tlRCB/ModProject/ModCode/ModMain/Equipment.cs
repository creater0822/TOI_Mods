using System.Linq;
using Il2CppSystem.Collections.Generic;

namespace MOD_tlRCB
{
    public static class Equipment
    {
        public static void Equip(this WorldUnitBase unit, int slot, int propsID)
        {
            if (slot < 0 || slot > 2 || unit.data.unitData.propData.GetPropsNum(propsID) < 1)
            {
                Log.Debug($"{unit.GetName()}换装备失败, 请检查参数 slot:{slot} propsID:{propsID}");
                return;
            }
            string soleID = unit.data.unitData.propData.allProps.ToArray().First((DataProps.PropsData it) => it.propsID == propsID).soleID;
            unit.data.unitData.equips[slot] = soleID;
            unit.CreateAllEquip();
        }

        public static void Equip(this WorldUnitBase unit, int slot, DataProps.PropsData prop)
        {
            if (slot < 0 || slot > 2 || prop.propsCount < 1)
            {
                Log.Debug($"{unit.GetName()}换装备失败, 请检查参数 slot:{slot} PropsData:{prop.propsID}");
                return;
            }
            unit.data.unitData.equips[slot] = prop.soleID;
            unit.CreateAllEquip();
        }

        public static void AddProp(this WorldUnitBase unit, int propsID, int propCount = 1)
        {
            if (unit == null)
            {
                return;
            }
            bool flag = false;
            List<DataProps.PropsData>.Enumerator enumerator = unit.data.unitData.propData.allProps.GetEnumerator();
            while (enumerator.MoveNext())
            {
                DataProps.PropsData current = enumerator.Current;
                if (current.propsID == propsID)
                {
                    flag = true;
                    current.propsCount += propCount;
                    break;
                }
            }
            if (!flag && propCount > 0)
            {
                unit.data.unitData.propData.AddProps(propsID, propCount);
            }
        }
    }
}
