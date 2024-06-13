using System;
using System.Collections.Generic;
using System.Reflection;
using MOD_tlRCB.EvilFall;
using MOD_tlRCB.Rape;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB
{
    public class ModMain
    {
        private TimerCoroutine corUpdate;
		private static HarmonyLib.Harmony harmony;

        internal List<IDisposable> mods = new List<IDisposable>();
        private static Il2CppSystem.Action<ETypeData> onSaveData = (Action<ETypeData>)SaveData;

        public void Init()
        {
			if (harmony != null)
			{
				harmony.UnpatchSelf();
				harmony = null;
			}
			if (harmony == null)
			{
				harmony = new HarmonyLib.Harmony("MOD_tlRCB");
			}
			harmony.PatchAll(Assembly.GetExecutingAssembly());

            DramaConditionPatch.Init();
            mods = new List<IDisposable>
            {
                new Birth(),
                new SexSlave(),
                new RapeBattle(),
                new EvilFall.EvilFall(),
                new BringUp(),
                new Brothel()
            };
            Config.Init();
            corUpdate = g.timer.Frame((Action)OnUpdate, 1, loop: true);
            g.events.On(EGameType.SaveData, onSaveData, 0);
            Events.Init();
            g.events.On(EGameType.OneOpenUIEnd(UIType.CreatePlayer), (Action)delegate
            {
                UICreatePlayer ui = g.ui.GetUI<UICreatePlayer>(UIType.CreatePlayer);
                if (ui != null)
                {
                    ui.uiFacade.textSex.transform.GetChild(1).GetComponent<Toggle>().onValueChanged.AddListener((Action<bool>)delegate
                    {
                        ui.uiProperty.btnPropertyRandom.onClick.Invoke();
                    });
                }
            }, 0);
        }

        private static void SaveData(ETypeData e)
	    {
	    }

        public void Destroy()
        {
            g.timer.Stop(corUpdate);
            foreach (IDisposable mod in mods)
            {
                mod?.Dispose();
            }
            g.events.Off(EGameType.SaveData, onSaveData);
            Events.Exit();
        }

        private void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F7))
            {
                if (Settings.Instance != null)
                {
                    Settings.Close();
                }
                else
                {
                    Settings.Open();
                }
            }
            else
            {
                Input.GetKeyDown(KeyCode.T);
            }
        }
    }
}
