using EGameTypeData;
using System;
using System.Reflection;

namespace MOD_termFix
{
    public class ModMain
    {
		private static HarmonyLib.Harmony harmony;
        private Il2CppSystem.Action<ETypeData> callOpenUIEnd;
        private static int[] fixIDs = new int[4]
        {
            2000, // discuss (absolutely harmless)
            2247, // standard (absolutely harmness)
            2249, // kong (no reason to censor this word alone)
            2253  // times (absolutely harmless)
        };

        public void Init()
        {
			if (harmony != null)
			{
				harmony.UnpatchSelf();
				harmony = null;
			}
			if (harmony == null)
			{
				harmony = new HarmonyLib.Harmony("MOD_termFix");
			}
			harmony.PatchAll(Assembly.GetExecutingAssembly());

            callOpenUIEnd = (Action<ETypeData>)OnOpenUIEnd;
            g.events.On(EGameType.OpenUIEnd, callOpenUIEnd, 0);
        }
        
        private void OnOpenUIEnd(ETypeData e)
        {
            if (e.Cast<OpenUIEnd>().uiType.uiName == UIType.ModMainProject.uiName)
            {
                Console.WriteLine("Fix harmless term filter");
                foreach (int i in fixIDs)
                { // Replace harmless terms with a random harmful term which we want to block anyway
                    g.conf.modTextBlock.GetItem(i).ch = "";
                    g.conf.modTextBlock.GetItem(i).tc = "";
                }
                // isis (annoying when 2 totally unrelated words have their last and first letters be this word)
                g.conf.modTextBlock.GetItem(2240).ch = "'isis'";
                g.conf.modTextBlock.GetItem(2240).tc = "'isis'";
            }
        }

        public void Destroy()
        {
            g.events.Off(EGameType.OpenUIEnd, callOpenUIEnd);
        }
    }
}
