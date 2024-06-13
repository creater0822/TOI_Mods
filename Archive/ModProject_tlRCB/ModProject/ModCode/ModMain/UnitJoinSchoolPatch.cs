using System;
using HarmonyLib;

namespace MOD_tlRCB
{
    [HarmonyPatch(typeof(UnitActionFeedback115), "OnCreate")]
    public class UnitJoinSchoolPatch
    {
        [HarmonyPrefix]
        public static bool ModifyFeedback115(UnitActionFeedback115 __instance)
        {
            try
            {
                return ModifyFeedback115Content(__instance);
            }
            catch (Exception ex)
            {
                Log.Debug(ex.ToString());
                return true;
            }
        }

        private static bool ModifyFeedback115Content(UnitActionFeedback115 feedback115)
        {
            Log.Debug(feedback115.inviteUnit.data.unitData.propertyData.GetName() + " 邀请 " + feedback115.unit.data.unitData.propertyData.GetName() + " 加入宗门");
            Log.Debug($"是否玩家：{feedback115.inviteUnit.data.unitData.unitID == g.world.playerUnit.data.unitData.unitID}");
            Log.Debug($"是否奴隶：{feedback115.unit.IsSexSlave()}");
            if (feedback115.inviteUnit.data.unitData.unitID != g.world.playerUnit.data.unitData.unitID || !feedback115.unit.IsSexSlave())
            {
                return true;
            }
            Log.Debug("邀请性奴加入宗门，强制同意。");
            feedback115.state = 1;
            feedback115.feedbackLog.AddLogSub("eventLogFeedback11511211");
            feedback115.feedbackLog.AddLogSub("eventLogFeedback11511221");
            feedback115.feedbackLog.AddLogSub("eventLogFeedback11511231");
            feedback115.isLastExitSchoolMonth = false;
            feedback115._OnCreate_b__6_0(state: true, "");
            return false;
        }
    }
}
