using System;
using System.Collections.Generic; // this
using System.Linq;
using EGameTypeData;
using Harmony;
using MOD_tlRCB.Rape;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB.EvilFall
{
    public class EvilFall : IDisposable
    {
        public EvilFall()
        {
            Events.On(EGameType.OpenUIEnd, OnOpenPlayerInfoUI);
            AddDramaOptions();
        }

        public void Dispose()
        {
            Events.Off(EGameType.OpenUIEnd, OnOpenPlayerInfoUI);
        }

        private void OnOpenPlayerInfoUI(ETypeData data)
        {
            if (!(data.Cast<OpenUIEnd>().uiType.uiName != UIType.PlayerInfo.uiName))
            {
                UIPlayerInfo uI = g.ui.GetUI<UIPlayerInfo>(UIType.PlayerInfo);
                if (!(uI == null) && uI.unit.IsWoman())
                {
                    Transform parent = uI.uiProperty.goInfoRoot.transform.parent;
                    Text text = UnityEngine.Object.Instantiate(uI.uiProperty.textStand1, parent, worldPositionStays: false);
                    Vector2 sizeDelta = text.transform.GetComponent<RectTransform>().sizeDelta;
                    sizeDelta = new Vector2(sizeDelta.x * 2f, sizeDelta.y);
                    text.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;
                    text.alignment = TextAnchor.MiddleLeft;
                    text.transform.name = "堕落";
                    text.transform.localPosition = new Vector3(300f, -103f);
                    string evilFallLevelFormatter = GetEvilFallLevelFormatter(uI.unit);
                    text.text = "堕落：" + evilFallLevelFormatter;
                    UISkyTipEffect uISkyTipEffect = text.gameObject.AddComponent<UISkyTipEffect>();
                    uISkyTipEffect.InitData(evilFallLevelFormatter + "\n" + SexStatistics(uI.unit));
                    uISkyTipEffect.isLeftAligen = true;
                }
            }
        }

        public static void HandleSexToys(WorldUnitBase unit)
        {
            Il2CppSystem.Collections.Generic.List<DataProps.PropsData>.Enumerator enumerator = unit.data.unitData.propData.GetEquipProps().GetEnumerator();
            while (enumerator.MoveNext())
            {
                DataProps.PropsData current = enumerator.Current;
                if (current.propsID == -141631521)
                {
                    unit.AddDepravation(20);
                    Log.Debug(unit.GetName() + "小穴里塞了玉如意，堕落值 +20");
                    if (unit.IsPlayer())
                    {
                        UICostItemTool.AddTipText("穿戴了<color=#BC1717>玉如意</color> 堕落值 +20");
                    }
                }
                else if (current.propsID == 264188970)
                {
                    unit.AddDepravation(10);
                    Log.Debug(unit.GetName() + "佩带了乳环，堕落值 +10");
                    if (unit.IsPlayer())
                    {
                        UICostItemTool.AddTipText("佩带了<color=#BC1717>乳环</color> 堕落值 +10");
                    }
                }
            }
        }

        public static void HandleNpcShowEvilFall(UINPCInfoProperty __instance)
        {
            if (!__instance.nPCInfo.unit.IsWoman())
            {
                return;
            }
            UINPCInfo ui = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);
            Transform transform = __instance.goGroupRoot.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.name == "堕落")
                {
                    child.GetComponent<Text>().text = "堕落：" + GetEvilFallLevelFormatter(ui.unit);
                    child.GetComponent<UISkyTipEffect>().OnDestroy();
                    UISkyTipEffect uISkyTipEffect = child.gameObject.AddComponent<UISkyTipEffect>();
                    uISkyTipEffect.InitData(SexStatistics(__instance.nPCInfo.unit));
                    uISkyTipEffect.isLeftAligen = true;
                    return;
                }
            }
            Text text = UnityEngine.Object.Instantiate(ui.uiUnitInfo.textStand1, transform, worldPositionStays: false);
            text.transform.name = "堕落";
            Vector2 sizeDelta = text.transform.GetComponent<RectTransform>().sizeDelta;
            sizeDelta = new Vector2(sizeDelta.x * 2f, sizeDelta.y);
            text.transform.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            text.transform.localPosition = new Vector3(-147f, 45f);
            string evilFallLevelFormatter = GetEvilFallLevelFormatter(ui.unit);
            text.text = "堕落：" + evilFallLevelFormatter;
            UISkyTipEffect uISkyTipEffect2 = text.gameObject.AddComponent<UISkyTipEffect>();
            uISkyTipEffect2.InitData(evilFallLevelFormatter + "\n" + SexStatistics(ui.unit));
            uISkyTipEffect2.isLeftAligen = true;
            if (!ui.unit.IsBitch())
            {
                return;
            }
            MapBuildTown mapBuildTown = (from it in Brothel.Bitchs
                                         where it.Value.Contains(ui.unit.GetID())
                                         select g.world.build.GetBuild<MapBuildTown>(it.Key)).FirstOrDefault();
            if (mapBuildTown != null)
            {
                Text text2 = UnityEngine.Object.Instantiate(ui.uiUnitInfo.textStand1, transform, worldPositionStays: false);
                text2.text = mapBuildTown.name + "妓女";
                int @int = ui.unit.GetInt("进入青楼时间");
                if (g.world.run.roundMonth - @int >= Config.Instance.BrothelSettings.CanRedemptionMonth)
                {
                    text2.text += "（可赎身）";
                }
                else
                {
                    text2.text += $"（{Config.Instance.BrothelSettings.CanRedemptionMonth - (g.world.run.roundMonth - @int)}月后可赎身）";
                }
                Transform transform2 = text2.transform;
                transform2.name = "address";
                transform2.GetComponent<RectTransform>().sizeDelta = new Vector2(180f, 30f);
                transform2.localPosition = new Vector3(58f, 90f);
                return;
            }
            bool breaks = false;
            foreach (KeyValuePair<string, List<string>> item in Brothel.Bitchs.TakeWhile((KeyValuePair<string, List<string>> pair) => !breaks))
            {
                using (System.Collections.Generic.IEnumerator<string> enumerator2 = item.Value.Where((string s) => s == ui.unit.GetID()).GetEnumerator())
                {
                    if (enumerator2.MoveNext())
                    {
                        string current2 = enumerator2.Current;
                        Brothel.Bitchs[item.Key].Remove(current2);
                        breaks = true;
                    }
                }
            }
            for (int j = 527000001; j <= 527000006; j++)
            {
                ui.unit.RemoveLuck(j);
            }
            Log.Debug(ui.unit.GetName() + "所在青楼位置异常，已将其从青楼中移除");
        }

        public static string GetEvilFallLevelFormatter(WorldUnitBase target)
        {
            int @int = target.GetInt("corruption_val");
            int evilFallLevel = target.GetEvilFallLevel();
            string[] array = new string[5]
            {
            "正常",
            GameTool.LS("L11CgdMW"),
            GameTool.LS("NJoZC9JT"),
            GameTool.LS("MzEV9eS3"),
            GameTool.LS("EL8wVz68")
            };
            string result = $"{array[evilFallLevel - 1]}（{@int}）";
            if (evilFallLevel == 1 && (target.data.unitData.propertyData.age < 25 || target.HasLuck(-527021125)))
            {
                result = string.Format("{0}（{1}）", GameTool.LS("8XvCq6eD"), @int);
            }
            return result;
        }

        public static string SexStatistics(WorldUnitBase target)
        {
            Dictionary<string, string> dictionary = target.ListFuckers();
            string arg = ((dictionary.Count >= 20) ? (dictionary.Take(20).Join((KeyValuePair<string, string> s) => s.Value) + $"等{dictionary.Count}人") : dictionary.Join((KeyValuePair<string, string> s) => s.Value));
            string text = "";
            foreach (KeyValuePair<string, string> item in dictionary)
            {
                int rapeNum = target.GetRapeNum(item.Key);
                if (rapeNum > 0)
                {
                    text += $"  {item.Value} {rapeNum}次";
                }
            }
            string text2 = "";
            int @int = target.GetInt("卖身次数");
            if (@int > 0)
            {
                text2 += $"卖身次数：{@int}\n";
                Dictionary<string, string> dictionary2 = target.ListBrothelFucker();
                text2 += ((dictionary2.Count < 20) ? dictionary2.Join((KeyValuePair<string, string> s) => s.Value) : dictionary2.Take(20).Join((KeyValuePair<string, string> s) => s.Value));
            }
            return "破处者：" + target.GetFirstManName() + "\n" + $"双修对象共{dictionary.Count}人： {arg}\n" + string.Format("流产次数：{0}\n", target.GetInt("流产次数")) + string.Format("被强奸次数：{0}\n", target.GetInt("被强奸次数")) + text + "\n" + text2;
        }

        public static bool HandleUseItem(WorldUnitBase unit, int propsID)
        {
            switch (propsID)
            {
                case -829499283:
                    unit.AddLuck(unit.IsMan() ? 112 : (-523596344));
                    return true;
                case -1442461760:
                    if (unit.IsMan())
                    {
                        20.Percent(delegate
                        {
                            unit.AddLuck(1840247464);
                        });
                    }
                    else
                    {
                        unit.AddLuck(-523596344);
                    }
                    return true;
                default:
                    return false;
            }
        }

        public static void AddDramaOptions()
        {
            Drama.ModifyOptions(22703, "2270310|2270311|2270312|2270313");
        }
    }
}
