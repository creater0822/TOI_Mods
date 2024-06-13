using System;
using System.Reflection;

namespace MOD_LE2lAt
{
    public class ModMain
    {
		private static HarmonyLib.Harmony harmony;
        private Il2CppSystem.Action<ETypeData> callIntoWorld;
        //private Il2CppSystem.Action<ETypeData> callOpenUIEnd;
        public static string nspace() { return "LE2lAt"; }
        public static int mid() { return 303457160; }

        public void Init()
        {
			if (harmony != null)
			{
				harmony.UnpatchSelf();
				harmony = null;
			}
			if (harmony == null)
			{
				harmony = new HarmonyLib.Harmony("MOD_LE2lAt");
			}
			harmony.PatchAll(Assembly.GetExecutingAssembly());
            callIntoWorld = (Action<ETypeData>)OnIntoWorld;
            g.events.On(EGameType.IntoWorld, callIntoWorld, 0);
        }

        public void Destroy()
        {
            
        }

        private void OnUpdate()
        {

        }

        private void OnIntoWorld(ETypeData e)
        {
            ConfDramaDialogueItem item = g.conf.dramaDialogue.GetItem(22815); // 22815
            string options = Util.addCommand(item.options, 303457162 + "|" + 100002);
            item.options = options;
        }
    }
}
