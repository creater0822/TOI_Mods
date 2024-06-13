namespace MOD_tlRCB.EvilFall
{
    public static class EvilFallLevel
    {
        private static int GetEvilFallLevel(int value)
        {
            if (value > 1000)
            {
                return 5;
            }
            if (value > 600)
            {
                return 4;
            }
            if (value > 300)
            {
                return 3;
            }
            if (value <= 100)
            {
                return 1;
            }
            return 2;
        }

        public static int GetEvilFallLevel(this WorldUnitBase target)
        {
            return GetEvilFallLevel(target.GetInt("corruption_val"));
        }

        public static int AddDepravation(this WorldUnitBase unit, int value)
        {
            int num = unit.GetInt("corruption_val") + value;
            unit.SetInt("corruption_val", num);
            unit.RemoveLuck(-629032638);
            unit.RemoveLuck(-593851244);
            unit.RemoveLuck(604486182);
            unit.RemoveLuck(920354678);
            unit.RemoveLuck(-1306624676);
            int evilFallLevel = GetEvilFallLevel(num);
            if (evilFallLevel >= 5)
            {
                unit.AddLuck(-1306624676);
            }
            else
            {
                switch (evilFallLevel)
                {
                    case 4:
                        unit.AddLuck(920354678);
                        break;
                    case 3:
                        unit.AddLuck(604486182);
                        break;
                    case 2:
                        unit.AddLuck(-593851244);
                        break;
                    default:
                        if (unit.data.unitData.propertyData.age < 25)
                        {
                            unit.AddLuck(-629032638);
                        }
                        break;
                }
            }
            g.ui.GetUI<UINPCInfo>(UIType.NPCInfo)?.uiProperty?.UpdateUI();
            return num;
        }
    }
}
