using System;
using MelonLoader;

namespace MOD_tlRCB
{
    public static class Log
    {
        public static void Debug(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                Console.WriteLine();
            }
            else
            {
                MelonLogger.Msg("[淫女宫] " + msg);
            }
        }

        public static void Split()
        {
            MelonLogger.Msg("-----------------------");
        }
    }
}
