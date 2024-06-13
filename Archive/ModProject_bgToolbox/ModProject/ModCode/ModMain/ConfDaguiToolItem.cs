namespace MOD_bgToolbox
{
    public class ConfDaguiToolItem
    {
        public string func;
        public string p1;
        public string p2;
        public string p3;
        public string p4;
        public string type;
        public string funccn;
        public string p1cn;
        public string p2cn;
        public string p3cn;
        public string p4cn;
        public string typecn;

        public string para1 => p1cn;
        public string para2 => p2cn;
        public string para3 => p3cn;
        public string para4 => p4cn;
        public string typeName => typecn;
        public string titleName => funccn;

        public ConfDaguiToolItem(string q1, string q2, string q3, string q4, string q5, string q6, string q7, string q8, string q9, string q10, string q11, string q12)
        {
            func = q1;
            p1 = q2;
            p2 = q3;
            p3 = q4;
            p4 = q5;
            type = q6;
            funccn = q7;
            p1cn = q8;
            p2cn = q9;
            p3cn = q10;
            p4cn = q11;
            typecn = q12;
        }
    }
}
