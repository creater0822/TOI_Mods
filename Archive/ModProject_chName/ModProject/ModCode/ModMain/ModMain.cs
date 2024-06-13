using System;
using System.Reflection;
using UnityEngine;

namespace MOD_NFKFqQ
{
    public class ModMain
    {
        // private TimerCoroutine corUpdate;
		private static HarmonyLib.Harmony harmony;

        public void Init()
        {
			if (harmony != null)
			{
				harmony.UnpatchSelf();
				harmony = null;
			}
			if (harmony == null)
			{
				harmony = new HarmonyLib.Harmony("MOD_NFKFqQ");
			}
			harmony.PatchAll(Assembly.GetExecutingAssembly());

            // corUpdate = g.timer.Frame(new Action(OnUpdate), 1, true);
        }

        public void Destroy()
        {
            // g.timer.Stop(corUpdate);
        }

        private void OnUpdate()
        {

        }
    }
}
