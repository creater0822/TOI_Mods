using System;

namespace MOD_tlRCB
{
    public static class Events
    {
        private static Il2CppSystem.Action<ETypeData> HandleMonthStart = (Action<ETypeData>)OnMonthStart;
        private static Il2CppSystem.Action<ETypeData> HandleMonthEnd = (Action<ETypeData>)OnMonthEnd;

        public static void Init()
        {
            g.events.On(EGameType.WorldRunEnd, HandleMonthStart, 0);
            g.events.On(EGameType.WorldRunStart, HandleMonthEnd, 0);
        }

        public static void Exit()
        {
            g.events.Off(EGameType.WorldRunEnd, HandleMonthStart);
            g.events.Off(EGameType.WorldRunStart, HandleMonthEnd);
        }

        private static void OnMonthStart(ETypeData e)
        {
            Birth.OnMonthStart(e);
            BringUp.HandleMonthStart();
        }

        private static void OnMonthEnd(ETypeData e)
        {
            Birth.OnMonthEnd(e);
            Brothel.OnMonthEnd();
        }

        public static void On(string eventName, Action<ETypeData> methods)
        {
            g.events.On(eventName, methods, 0);
        }

        public static void Off(string eventName, Action<ETypeData> methods)
        {
            try
            {
                Log.Debug(g.events.Off(eventName, methods) ? ("卸载事件" + eventName + "成功") : ("卸载事件" + eventName + "失败"));
            }
            catch (Exception ex)
            {
                Log.Debug("卸载事件" + eventName + "失败: $" + ex.Message);
            }
        }

        public static void Once(string eventName, Action<ETypeData> methods)
        {
            g.events.On(eventName, methods, 1);
        }
    }
}
