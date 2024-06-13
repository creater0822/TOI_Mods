using System;
using HarmonyLib;
using MOD_tlRCB.Rape;

namespace MOD_tlRCB.Hook
{
    [HarmonyPatch(typeof(UnitActionFeedback102), "OnCreate")]
    public class UnitActionFeedback102Patch
    {
        [HarmonyPrefix]
        public static bool ModifyFeedback102(UnitActionFeedback102 __instance, out int __state)
        {
            try
            {
                if (Config.Instance.NpcSettings.Enable && !Config.Instance.NpcSettings.NpcCannotFuckFemalePlayer && __instance.battleUnit != null && __instance.battleUnit.IsPlayer() && __instance.battleUnit.IsWoman())
                {
                    return ModifyFeedback102Content(__instance, out __state);
                }
                __state = 0;
                return true;
            }
            catch (Exception ex)
            {
                __state = 0;
                Log.Debug(ex.ToString());
                return true;
            }
        }

        private static bool ModifyFeedback102Content(UnitActionFeedback102 __instance, out int __state)
        {
            bool isPlayer = __instance.battleUnit.IsPlayer();
            AddSubLog("eventLogFeedback1021008677");
            RapeBattle.Action action = RapeBattle.JudgeAction(__instance.unit, __instance.battleUnit);
            int num2;
            if (action.HasFlag(RapeBattle.Action.Rape) || action.HasFlag(RapeBattle.Action.Sell))
            {
                num2 = (__instance.state = 3);
                __state = num2;
                AddSubLog("eventLogFeedback1021008678");
                if (action.HasFlag(RapeBattle.Action.Rape))
                {
                    AddSubLog("eventLogFeedback1021008679");
                    if (action == RapeBattle.Action.Rape)
                    {
                        __instance.onEndCall = (Action)delegate
                        {
                            DramaTool.OpenDrama(-656836651, new DramaData
                            {
                                unitLeft = __instance.battleUnit,
                                unitRight = __instance.unit
                            });
                            Birth.Fuck(__instance.unit, __instance.battleUnit, isRape: true);
                        };
                    }
                }
                if (action.HasFlag(RapeBattle.Action.Sell))
                {
                    AddSubLog("eventLogFeedback1021008680");
                    if (Brothel.Sell(__instance.unit, __instance.battleUnit) >= 1)
                    {
                        DramaTool.OpenDrama(1811210024, new DramaData
                        {
                            unitLeft = __instance.battleUnit,
                            unitRight = __instance.unit
                        });
                    }
                }
                __instance.OnEndCall();
                return false;
            }
            num2 = (__instance.state = 0);
            __state = num2;
            return true;
            void AddSubLog(string log, string[] array = null)
            {
                if (isPlayer)
                {
                    __instance.feedbackLog.AddLogSub(log, array);
                }
            }
        }
    }
}
