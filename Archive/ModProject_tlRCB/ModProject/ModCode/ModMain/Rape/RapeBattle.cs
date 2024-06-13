using System;
using System.Collections.Generic;
using EBattleTypeData;
using MOD_tlRCB.Rape;

namespace MOD_tlRCB.Rape
{
    public class RapeBattle : IDisposable
    {
        [Flags]
        public enum Action
        {
            System = 0,
            Rape = 1,
            Sell = 2
        }

        public RapeBattle()
        {
            Events.On(EBattleType.UnitHitDynIntHandler, CallUnitHit);
            AddDramaOptions();
        }

        public void Dispose()
        {
            Events.Off(EBattleType.UnitHitDynIntHandler, CallUnitHit);
        }

        public static void AddDramaOptions()
        {
            Drama.ModifyOptions(20403, new List<string> { "204036", "2019707427" });
            Drama.ModifyOptions(20404, new List<string> { "204036", "2019707427" });
            Drama.ModifyOptions(20406, "2019707423|2019707424");
        }

        private void CallUnitHit(ETypeData e)
        {
            float num = 5f;
            int num2 = 10;
            UnitHitDynIntHandler unitHitDynIntHandler = e.Cast<UnitHitDynIntHandler>();
            UnitDataHuman unitDataHuman = unitHitDynIntHandler.hitUnit.data.TryCast<UnitDataHuman>();
            if (unitDataHuman == null)
            {
                return;
            }
            UnitDataHuman unitDataHuman2 = unitHitDynIntHandler.hitData.attackUnit.data.TryCast<UnitDataHuman>();
            if (unitDataHuman2 == null)
            {
                return;
            }
            WorldUnitBase unit = unitDataHuman.worldUnitData.unit;
            WorldUnitBase unit2 = unitDataHuman2.worldUnitData.unit;
            if (!unit.HasLuck(-406173830) && !unit2.HasLuck(-406173830))
            {
                return;
            }
            int rapeNum = unit2.GetRapeNum(unit);
            if (rapeNum > 0)
            {
                if ((float)rapeNum * num / 100f * (float)unitHitDynIntHandler.dynV.baseValue > (float)num2)
                {
                    unitHitDynIntHandler.dynV.baseValue = (int)((100f - (float)rapeNum * num) / 100f * (float)unitHitDynIntHandler.dynV.baseValue);
                }
                else
                {
                    unitHitDynIntHandler.dynV.baseValue -= num2;
                }
            }
            rapeNum = unit.GetRapeNum(unit2);
            if (rapeNum > 0)
            {
                if ((float)rapeNum * num / 100f * (float)unitHitDynIntHandler.dynV.baseValue > (float)num2)
                {
                    unitHitDynIntHandler.dynV.baseValue = (int)((100f + (float)rapeNum * num) / 100f * (float)unitHitDynIntHandler.dynV.baseValue);
                }
                else
                {
                    unitHitDynIntHandler.dynV.baseValue += num2;
                }
            }
        }

        public static void HandleNpcBattleEndRape(UnitActionRoleBattle __instance, WorldUnitBase winUnit, WorldUnitBase failUnit)
        {
            if (!Config.Instance.NpcSettings.Enable)
            {
                return;
            }
            if (__instance.battleType == "zhandou" && winUnit.data.unitData.propertyData.sex == UnitSexType.Man && failUnit.IsWoman() && !failUnit.IsPlayer() && !winUnit.IsPlayer() && (__instance.isPassUnit || __instance.isBattleInsultUnit || !__instance.isKillToUnit) && !failUnit.isDie && !__instance.isBattleBackEscape && !__instance.isBattleFrontEscapeComplete && __instance.isComplete)
            {
                Action action = JudgeAction(winUnit, failUnit);
                if (action.HasFlag(Action.Rape))
                {
                    Birth.Fuck(winUnit, failUnit, isRape: true);
                }
                if (action.HasFlag(Action.Sell))
                {
                    Brothel.Sell(winUnit, failUnit);
                }
            }
        }

        public static Action JudgeAction(WorldUnitBase winUnit, WorldUnitBase failUnit)
        {
            if (winUnit.GetStr("nymphomaniac") == "true")
            {
                return Action.Rape;
            }
            if (Config.Instance.NpcSettings.NpcCannotFuckPlayersFriend && (failUnit.IsSexSlave() || failUnit.IsVital(g.world.playerUnit) || failUnit.IsFriend(g.world.playerUnit)))
            {
                return Action.System;
            }
            int num = (winUnit.data.unitData.propertyData.inTrait - 4) * 50;
            int num2 = failUnit.data.unitData.propertyData.beauty - 450;
            int num3 = ((winUnit.data.unitData.propertyData.standDown > winUnit.data.unitData.propertyData.standUp) ? (winUnit.data.unitData.propertyData.standDown - 100) : 0);
            int num4 = -winUnit.data.unitData.relationData.GetIntim(failUnit) - 240;
            int num5 = failUnit.GetInt("corruption_val") - 300;
            if (num5 < 0)
            {
                num5 = 0;
            }
            int num6 = (failUnit.HasLuck(1561118811) ? 200 : 0);
            int num7 = num + num2 + num3 + num4 + num6;
            if (num7 <= 0)
            {
                return Action.System;
            }
            Action action = Action.System;
            if (CommonTool.Random(0, 100) < Config.Instance.NpcSettings.RapePercent)
            {
                Log.Debug(winUnit.GetName() + "击败了" + failUnit.GetName() + "，" + $"性格值{num} 魅力值{num2} 魔道值{num3} 仇恨值{num4} 总值{num7} " + $"败者堕落加成{num5} " + "心生恶念，决定强奸" + failUnit.GetName());
                action = Action.Rape;
            }
            if (winUnit.data.unitData.relationData.GetIntim(failUnit) < -Config.Instance.NpcSettings.KidnapMinHate && !Config.Instance.NpcSettings.NpcCannotSellFemalePlayer && CommonTool.Random(0, 100) < Config.Instance.NpcSettings.SellPercent)
            {
                Log.Debug(winUnit.GetName() + "击败了" + failUnit.GetName() + "，" + $"性格值{num} 魅力值{num2} 魔道值{num3} 仇恨值{num4} 总值{num7} " + $"败者堕落加成{num5} " + "心生恶念，决定将" + failUnit.GetName() + "卖到青楼");
                action |= Action.Sell;
            }
            return action;
        }
    }
}
