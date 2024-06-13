using System;
using System.Text.RegularExpressions;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace MOD_tlRCB
{
    [HarmonyPatch(typeof(UnitActionFeedback1031), "OnCreate")]
    public class UnitTrainPatch
    {
        public static class Utils
        {
            public static string GetResultByBool(bool get)
            {
                if (!get)
                {
                    return "拒绝";
                }
                return "接受";
            }

            public static string GetStringByBool(bool get)
            {
                if (!get)
                {
                    return "×";
                }
                return "√";
            }

            public static string GetSex(UnitSexType sexType)
            {
                switch (sexType)
                {
                    case UnitSexType.None:
                        return "无";
                    case UnitSexType.Man:
                        return "男";
                    case UnitSexType.Woman:
                        return "女";
                    case (UnitSexType)3:
                        return "女(百合)";
                    default:
                        return "无";
                }
            }

            public static string GetRelation(UnitBothRelationType itemValue)
            {
                switch (itemValue)
                {
                    case UnitBothRelationType.None:
                        return "萍水相逢";
                    case UnitBothRelationType.Grow:
                        return "Grow";
                    case UnitBothRelationType.Self:
                        return "身外化身";
                    case UnitBothRelationType.Parents:
                        return "父母";
                    case UnitBothRelationType.Children:
                        return "子女";
                    case UnitBothRelationType.Married:
                        return "夫妻";
                    case UnitBothRelationType.Lover:
                        return "道侣";
                    case UnitBothRelationType.Brother:
                        return "兄弟姐妹";
                    case UnitBothRelationType.Master:
                        return "师傅";
                    case UnitBothRelationType.Student:
                        return "徒弟";
                    case UnitBothRelationType.Friend:
                        return "朋友";
                    case UnitBothRelationType.Hater:
                        return "仇人";
                    case UnitBothRelationType.School:
                        return "同门";
                    case UnitBothRelationType.Stand:
                        return "立场对立";
                    case UnitBothRelationType.Doctrine:
                        return "同教";
                    default:
                        return "无";
                }
            }

            public static string GetIntimScoreDesc(int score)
            {
                if (score >= 2500)
                {
                    return "极好";
                }
                if (score >= 1500)
                {
                    return "很好";
                }
                if (score >= 500)
                {
                    return "较好";
                }
                if (score >= 0)
                {
                    return "一般";
                }
                if (score >= -500)
                {
                    return "较差";
                }
                if (score < -1500)
                {
                    return "极差";
                }
                return "很差";
            }

            public static string GetScoreDesc(int score)
            {
                if (score >= 500)
                {
                    return "极高";
                }
                if (score >= 0)
                {
                    return "较高";
                }
                if (score < -500)
                {
                    return "极低";
                }
                return "较低";
            }
        }

        [HarmonyPrefix]
        public static bool ModifyFeedback1031(UnitActionFeedback1031 __instance, out int __state)
        {
            try
            {
                return ModifyFeedback1031Content(__instance, out __state);
            }
            catch (Exception ex)
            {
                __state = 0;
                Log.Debug(ex.ToString());
                return true;
            }
        }

        private static bool ModifyFeedback1031Content(UnitActionFeedback1031 __instance, out int __state)
        {
            WorldUnitBase unit = __instance.unit;
            WorldUnitBase trainsUnit = __instance.trainsUnit;
            bool isPlayer = trainsUnit.GetHashCode() == g.world.playerUnit.GetHashCode();
            WorldUnitData data = unit.data;
            WorldUnitData data2 = trainsUnit.data;
            DataUnit.PropertyData propertyData = data.unitData.propertyData;
            DataUnit.PropertyData propertyData2 = data2.unitData.propertyData;
            WorldUnitDynData dynUnitData = data.dynUnitData;
            WorldUnitDynData dynUnitData2 = data2.dynUnitData;
            UnitSexType sex = propertyData.sex;
            UnitSexType sex2 = propertyData2.sex;
            _ = propertyData.age / 12;
            _ = propertyData2.age / 12;
            string name = propertyData.GetName();
            string name2 = propertyData2.GetName();
            ConfNpcFeedback1031 npcFeedback = g.conf.npcFeedback1031;
            int scoreRequire = npcFeedback.scoreRequire;
            _ = npcFeedback.closeEnough;
            bool flag = false;
            bool flag2 = false;
            int itemValue = npcFeedback.GetItemValue("controlCharacterAdd");
            int itemValue2 = npcFeedback.GetItemValue("controlCharacterAddPram");
            int itemValue3 = npcFeedback.GetItemValue("controlCharacterSub");
            int intim = data.unitData.relationData.GetIntim(trainsUnit);
            int num = intim * 10;
            int num2 = 0 + num;
            AddSubLog("eventLogFeedback10311008600", new string[1] { Utils.GetIntimScoreDesc(num) });
            UnitBothRelationType relationType = data.GetRelationType(trainsUnit);
            data.GetRelationCharacterWeight(trainsUnit);
            int itemValue4 = npcFeedback.GetItemValue($"relationScoreChange{(int)relationType}");
            int num3 = num2 + itemValue4;
            string text = g.conf.roleCall.RoleCall(unit, trainsUnit, new UnitConditionData(unit, trainsUnit));
            string text2;
            if (text.Contains("@"))
            {
                string value = Regex.Match(text, "\\(([\\S]*)\\)").Value;
                text2 = (string.IsNullOrEmpty(value) ? Utils.GetRelation(relationType) : value.Substring(1, value.Length - 2));
            }
            else
            {
                text2 = text;
            }
            AddSubLog("eventLogFeedback103110086002", new string[1] { text2 });
            int num4 = (dynUnitData2.beauty.value - (dynUnitData.beauty.value - 200)) * 2;
            int num5 = num3 + num4;
            AddSubLog("eventLogFeedback10311008601", new string[1] { Utils.GetScoreDesc(num4) });
            int num6 = Mathf.Clamp((FormulaTool.UnitPower.TotalPower(data2) - FormulaTool.UnitPower.TotalPower(data)) * 1000 / FormulaTool.UnitPower.TotalPower(data), -3000, 1500);
            int num7 = num5 + num6;
            AddSubLog("eventLogFeedback10311008602", new string[1] { Utils.GetScoreDesc(num6) });
            int num8 = UnityEngine.Random.RandomRangeInt(-Mathf.Max(dynUnitData.luck.value, 0), Mathf.Max(dynUnitData2.luck.value, 0)) * 7;
            int num9 = num7 + num8;
            int num10 = 0;
            bool flag3 = true;
            List<int> character = data.GetCharacter();
            string married = data.unitData.relationData.married;
            if (string.IsNullOrEmpty(married))
            {
                if (character.Contains(19))
                {
                    num10 += -1500;
                }
                if (character.Contains(17))
                {
                    num9 += 1000;
                }
            }
            else
            {
                int intim2 = data.unitData.relationData.GetIntim(married);
                if (married.Equals(data2.unitData.unitID))
                {
                    num9 = ((!character.Contains(19) && !character.Contains(12)) ? (num9 + intim2 * 15) : (num9 + Mathf.Max(intim2, 0) * 15));
                }
                else
                {
                    DataUnit.UnitInfoData unit2 = g.data.unit.GetUnit(married);
                    if (unit2 != null)
                    {
                        AddSubLog("eventLogFeedback10311008604", new string[1] { unit2.propertyData.GetName() });
                    }
                    num9 = (character.Contains(19) ? (num9 + Mathf.Max(intim2, 0) * -15) : ((!character.Contains(12)) ? (num9 + intim2 * -5) : (num9 + Mathf.Max(intim2, 0) * -5)));
                }
            }
            if (character.Contains(20))
            {
                if (relationType == UnitBothRelationType.Parents && sex2 == UnitSexType.Man)
                {
                    num9 += itemValue + intim * itemValue2;
                    flag3 = false;
                    AddSubLog("eventLogFeedback10311008606");
                }
                else
                {
                    num10 += itemValue3;
                    AddSubLog("eventLogFeedback10311008605");
                }
            }
            if (character.Contains(21))
            {
                if (relationType == UnitBothRelationType.Parents && sex2 == UnitSexType.Woman)
                {
                    num9 += itemValue + intim * itemValue2;
                    flag3 = false;
                    AddSubLog("eventLogFeedback10311008608");
                }
                else
                {
                    num10 += itemValue3;
                    AddSubLog("eventLogFeedback10311008607");
                }
            }
            if (character.Contains(22))
            {
                if (relationType == UnitBothRelationType.Brother && sex2 == UnitSexType.Man)
                {
                    num9 += itemValue + intim * itemValue2;
                    flag3 = false;
                    AddSubLog("eventLogFeedback10311008610");
                }
                else
                {
                    num10 += itemValue3;
                    AddSubLog("eventLogFeedback10311008609");
                }
            }
            if (character.Contains(23))
            {
                if (relationType == UnitBothRelationType.Brother && sex2 == UnitSexType.Woman)
                {
                    num9 += itemValue + intim * itemValue2;
                    flag3 = false;
                    AddSubLog("eventLogFeedback10311008612");
                }
                else
                {
                    num10 += itemValue3;
                    AddSubLog("eventLogFeedback10311008611");
                }
            }
            if (character.Contains(24))
            {
                if (relationType == UnitBothRelationType.Children && sex2 == UnitSexType.Man)
                {
                    num9 += itemValue + intim * itemValue2;
                    flag3 = false;
                    AddSubLog("eventLogFeedback10311008614");
                }
                else
                {
                    num10 += itemValue3;
                    AddSubLog("eventLogFeedback10311008613");
                }
            }
            if (character.Contains(25))
            {
                if (relationType == UnitBothRelationType.Children && sex2 == UnitSexType.Woman)
                {
                    num9 += itemValue + intim * itemValue2;
                    flag3 = false;
                    AddSubLog("eventLogFeedback10311008616");
                }
                else
                {
                    num10 += itemValue3;
                    AddSubLog("eventLogFeedback10311008615");
                }
            }
            if (character.Contains(26))
            {
                if (relationType == UnitBothRelationType.Master)
                {
                    num9 += itemValue + intim * itemValue2;
                    flag3 = false;
                    AddSubLog("eventLogFeedback10311008618");
                }
                else
                {
                    num10 += itemValue3;
                    AddSubLog("eventLogFeedback10311008617");
                }
            }
            if (character.Contains(27))
            {
                if (relationType == UnitBothRelationType.Student)
                {
                    num9 += itemValue + intim * itemValue2;
                    flag3 = false;
                    AddSubLog("eventLogFeedback10311008620");
                }
                else
                {
                    num10 += itemValue3;
                    AddSubLog("eventLogFeedback10311008619");
                }
            }
            if (flag3)
            {
                num9 += num10;
            }
            if (propertyData.age < 192)
            {
                __instance.state = 2;
                __state = __instance.state;
                AddSubLog("eventLogFeedback103111321");
                Log.Debug($"{name}[{Utils.GetSex(sex)},{propertyData.age / 12}岁]({dynUnitData.beauty.value}) " + Utils.GetResultByBool(__state == 1) + "(" + Utils.GetStringByBool(__state == 1) + ")了 " + $"{name2}[{Utils.GetSex(sex2)},{propertyData2.age / 12}岁]({dynUnitData2.beauty.value})的双修请求。" + name + "连16岁都没到！");
                __instance.OnEndCall();
                return false;
            }
            if (unit.GetLuck(-1306624676) != null)
            {
                flag = true;
                AddSubLog("eventLogFeedback10311008651");
            }
            else if (unit.GetLuck(1840247464) != null || unit.GetLuck(-523596344) != null)
            {
                flag = true;
                AddSubLog("eventLogFeedback1008651002");
            }
            else if (unit.GetLuck(-1419630244) != null && isPlayer)
            {
                flag = true;
                AddSubLog("eventLogFeedback10311008652");
            }
            if (unit.IsSexSlave())
            {
                if (isPlayer)
                {
                    flag = true;
                    AddSubLog("eventLogFeedback103110086101");
                }
                else
                {
                    flag2 = true;
                }
            }
            float num11 = scoreRequire;
            if (unit.HasLuck(920354678))
            {
                num11 = (float)scoreRequire * 0.6f;
            }
            else if (unit.HasLuck(604486182))
            {
                num11 = (float)scoreRequire * 0.8f;
            }
            __instance.state = (((float)num9 > num11) ? 1 : 2);
            if (flag)
            {
                __instance.state = 1;
            }
            if (flag2)
            {
                __instance.state = 2;
            }
            __state = __instance.state;
            AddSubLog((__state == 2) ? "eventLogFeedback103111321" : "eventLogFeedback103111311");
            if (data.unitData.relationData.married == g.world.playerUnit.data.unitData.unitID || data.unitData.relationData.IsRelation(g.world.playerUnit, UnitRelationType.Parent) || data.unitData.relationData.IsRelation(g.world.playerUnit, UnitRelationType.Children) || unit.IsSexSlave())
            {
                Log.Debug($"{name}[{Utils.GetSex(sex)},{propertyData.age / 12}岁]({dynUnitData.beauty.value}) " + Utils.GetResultByBool(__state == 1) + "(" + Utils.GetStringByBool(__state == 1) + ")了 " + $"{name2}[{Utils.GetSex(sex2)},{propertyData2.age / 12}岁]({dynUnitData2.beauty.value})的双修请求。" + $"[关系:{Utils.GetRelation(relationType)}·{text2} : {itemValue4}] " + $"好感分:{num} " + $"魅力分:{num4} " + $"实力分:{num6} " + $"运气值:{num8} " + $"总分:{num9} " + "强制接受:" + Utils.GetStringByBool(flag) + " 强制拒绝:" + Utils.GetStringByBool(flag2));
            }
            __instance.OnEndCall();
            return false;
            void AddSubLog(string log, string[] array = null)
            {
                if (isPlayer)
                {
                    __instance.feedbackLog.AddLogSub(log, array);
                }
            }
        }

        [HarmonyPostfix]
        public static void ModifyFeedback1031Post(UnitActionFeedback1031 __instance, int __state)
        {
            if (__state > 0)
            {
                __instance.state = __state;
            }
        }
    }
}
