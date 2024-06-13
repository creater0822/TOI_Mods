namespace MOD_LE2lAt
{
    internal class ModDeal
    {
        public static void mod(DramaFunction __instance, string[] values)
        {
            _ = __instance.data.unitRight;
            if (values[2].Equals("existsMod"))
            {
                if (GameTool.LS(values[3]).Equals("exists"))
                {
                    DramaFunctionTool.OptionsFunction("setWorldData_existsMod" + values[3] + "_true");
                }
                else
                {
                    DramaFunctionTool.OptionsFunction("setWorldData_existsMod" + values[3] + "_false");
                }
            }
        }
    }
}
