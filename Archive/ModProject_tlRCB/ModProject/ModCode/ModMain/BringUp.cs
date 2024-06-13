using System;
using System.Linq;
using EGameTypeData;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB
{
    public class BringUp : IDisposable
    {
        internal static List<WorldUnitBase> BabyList = new List<WorldUnitBase>();

        public BringUp()
        {
            Events.On(EGameType.OneOpenUIEnd(UIType.NPCInfo), ChangeNpcChildInfo);
            Events.Once(EGameType.IntoWorld, InitAllBabyList);
        }

        private void InitAllBabyList(ETypeData e)
        {
            Dictionary<string, WorldUnitBase>.ValueCollection.Enumerator enumerator = g.world.unit.allUnit.values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                WorldUnitBase unit = enumerator.Current;
                if (!unit.isDie && unit.GetAge() < 192 && BabyList.ToArray().All((WorldUnitBase s) => s.GetID() != unit.GetID()))
                {
                    BabyList.Add(unit);
                }
            }
        }

        private void ChangeNpcChildInfo(ETypeData e)
        {
            OneOpenUIEnd oneOpenUIEnd = e.Cast<OneOpenUIEnd>();
            UINPCInfo ui = g.ui.GetUI<UINPCInfo>(oneOpenUIEnd.uiType);
            if (ui == null || (!ui.unit.IsSexSlave() && ui.unit.data.GetRelationType(g.world.playerUnit) != UnitBothRelationType.Parents))
            {
                return;
            }
            AddButton(ui, "play", new Vector3(-444f, 150f), "互动", delegate
            {
                try
                {
                    DramaTool.OpenDrama(1811210044, new DramaData
                    {
                        unitLeft = g.world.playerUnit,
                        unitRight = ui.unit
                    });
                }
                catch (Exception value)
                {
                    Console.WriteLine(value);
                    throw;
                }
            });
        }

        private void AddButton(UINPCInfo ui, string name, Vector3 pos, string txt, Action click)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(ui.uiUnitInfo.goButton1, ui.transform, worldPositionStays: false);
            gameObject.SetActive(value: true);
            gameObject.transform.localPosition = pos;
            gameObject.name = name;
            gameObject.layer = int.MaxValue;
            gameObject.GetComponentInChildren<Text>().text = txt;
            Button componentInChildren = gameObject.GetComponentInChildren<Button>();
            componentInChildren.onClick.RemoveAllListeners();
            componentInChildren.onClick.AddListener(click);
        }

        public static void HandleMonthStart()
        {
            List<WorldUnitBase> list = new List<WorldUnitBase>();
            List<WorldUnitBase>.Enumerator enumerator = BabyList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                WorldUnitBase current = enumerator.Current;
                if (current.data.unitData.propertyData.age > 192)
                {
                    current.RemoveLuck(-748164927);
                    current.RemoveLuck(527000000);
                    list.Add(current);
                    continue;
                }
                if (current.HasLuck(527000000))
                {
                    current.AddLuck(-2040743491);
                    Log.Debug(current.GetName() + "被青楼喂下了催使快速成长的药，白色的，黏黏的，真的只是母乳吗？");
                }
                if (!current.HasLuck(-2040743491) && current.data.unitData.propData.GetPropsNum(-533659860) > 0)
                {
                    current.data.CostPropItem(-533659860, 1, showUI: false);
                    current.AddLuck(-2040743491);
                    Log.Debug(current.GetName() + "使用物品母乳");
                }
                foreach (string item in current.data.unitData.relationData.parent)
                {
                    WorldUnitBase unit = g.world.unit.GetUnit(item);
                    if (unit != null && unit.IsWoman())
                    {
                        current.MoveToTarget(unit);
                    }
                }
            }
            enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                WorldUnitBase current3 = enumerator.Current;
                BabyList.Remove(current3);
            }
        }

        public static void HandleMonthEnd(WorldUnitBase unit)
        {
            if (unit.HasLuck(-2040743491))
            {
                unit.data.unitData.propertyData.age += 12;
            }
        }

        public void Dispose()
        {
            Events.Off(EGameType.OneOpenUIEnd(UIType.NPCInfo), ChangeNpcChildInfo);
        }
    }
}
