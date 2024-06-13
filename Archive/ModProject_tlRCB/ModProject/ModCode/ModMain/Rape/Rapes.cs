namespace MOD_tlRCB.Rape
{
    public static class Rapes
    {
        public static RapeData Rape(this WorldUnitBase raper, WorldUnitBase target)
        {
            return target.RapeBy(raper);
        }

        public static RapeData RapeBy(this WorldUnitBase unit, WorldUnitBase raper)
        {
            if (unit.IsMan())
            {
                return new RapeData();
            }
            string key = "强奸_" + raper.data.unitData.unitID;
            int rapeNum = unit.GetRapeNum(raper);
            unit.SetInt(key, rapeNum + 1);
            int rapeTotal = unit.GetRapeTotal();
            unit.SetInt("被强奸次数", rapeTotal + 1);
            unit.AddLuck(-406173830);
            Log.Debug($"{raper.GetName()}强奸了{unit.GetName()}, {unit.GetName()}已经被人强奸过{rapeTotal + 1}次");
            return new RapeData
            {
                Total = rapeTotal + 1,
                Num = rapeNum + 1
            };
        }

        public static int GetRapeTotal(this WorldUnitBase unit)
        {
            string key = "被强奸次数";
            return unit.GetInt(key);
        }

        public static int GetRapeNum(this WorldUnitBase unit, WorldUnitBase raper)
        {
            string key = "强奸_" + raper.data.unitData.unitID;
            return unit.GetInt(key);
        }

        public static int GetRapeNum(this WorldUnitBase unit, string raper)
        {
            string key = "强奸_" + raper;
            return unit.GetInt(key);
        }
    }

}
