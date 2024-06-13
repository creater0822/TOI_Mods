namespace MOD_LE2lAt
{
    internal class SkillData
    {
        public DataUnit.ActionMartialData martialData { get; set; }
        public MartialType martialType { get; set; }
        public int idx { get; set; }

        public SkillData()
        {
        }

        public SkillData(DataUnit.ActionMartialData martialData, MartialType martialType, int idx)
        {
            this.martialData = martialData;
            this.martialType = martialType;
            this.idx = idx;
        }
    }
}
