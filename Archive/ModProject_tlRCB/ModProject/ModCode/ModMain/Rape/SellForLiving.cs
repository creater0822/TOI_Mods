using MOD_tlRCB.EvilFall;

namespace MOD_tlRCB.Rape
{
    public static class SellForLiving
    {
        public static bool SellPussy(WorldUnitBase unit)
        {
            if (unit.IsMan())
            {
                return false;
            }
            if (unit.GetAge() < 192)
            {
                return false;
            }
            return ((float)unit.GetEvilFallLevel() - 1.5f) * 20f + (float)((unit.data.unitData.propertyData.inTrait - 4) * 10) + (float)((unit.data.unitData.propertyData.standUp - unit.data.unitData.propertyData.standDown > 100) ? (-50) : 0) > 0f;
        }

        public static bool NpcAcceptSellPussy(WorldUnitBase win, WorldUnitBase fail)
        {
            if (!win.IsMan() || !fail.IsWoman())
            {
                return false;
            }
            return RapeBattle.JudgeAction(win, fail).HasFlag(RapeBattle.Action.Rape);
        }
    }
}
