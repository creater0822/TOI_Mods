using System;

namespace MOD_LE2lAt
{
    internal class ConsUtil
    {
        private static int type = 0;
        public static void alert(string s, float duration = 3f)
        {
            UITipItem.AddTip(s, duration);
        }
        public static void debug(string s)
        {
            if (type > 0)
                console(s);
        }
        public static void debug(string s, Il2CppSystem.Object o)
        {
            if (type > 0)
                console(s, o);
        }
        public static void debug(string s, Object o)
        {
            if (type > 0)
                console(s, o);
        }

        public static void error(string s, Exception e, Object o = null, Il2CppSystem.Object o1 = null)
        {
            if (o != null)
            {
                console(s, o);
            }
            if (o1 != null)
            {
                console(s, o1);
            }
            console(s, e.Message);
            console(s, e.StackTrace);
        }

        public static void console(string s)
        {
            Console.WriteLine(ModMain.nspace() + ":" + s);
        }
        public static void console(string s, Il2CppSystem.Object o)
        {
            try
            {
                Console.WriteLine(ModMain.nspace() + ":" + s + " = " + Il2CppNewtonsoft.Json.JsonConvert.SerializeObject(o));
            }
            catch (Exception e)
            {
                Console.WriteLine(ModMain.nspace() + ": E = " + e.Message);
                Console.WriteLine(ModMain.nspace() + ": E = " + e.StackTrace);
            }
        }
        public static void console(string s, Object o)
        {
            try
            {
                Console.WriteLine(ModMain.nspace() + ":" + s + " = " + Newtonsoft.Json.JsonConvert.SerializeObject(o));
            }
            catch (Exception e)
            {
                Console.WriteLine(ModMain.nspace() + ": E = " + e.Message);
                Console.WriteLine(ModMain.nspace() + ": E = " + e.StackTrace);
            }
        }
    }
}
