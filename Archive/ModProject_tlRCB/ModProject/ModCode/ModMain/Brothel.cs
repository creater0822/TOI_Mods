using System;
using System.Collections.Generic;
using System.Linq;
using MOD_tlRCB.EvilFall;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB
{
    public class Brothel : IDisposable
    {
        public class UnitCompare : IEqualityComparer<WorldUnitBase>
        {
            public bool Equals(WorldUnitBase x, WorldUnitBase y)
            {
                return x.GetID() == y.GetID();
            }

            public int GetHashCode(WorldUnitBase obj)
            {
                return obj.GetID().GetHashCode();
            }
        }

        private enum RedemptionWay
        {
            Fight,
            Money
        }

        internal static Dictionary<string, List<string>> Bitchs = new Dictionary<string, List<string>>();

        internal static List<string> Kidnapping = new List<string>();

        private static Il2CppSystem.Action<ETypeData> OnInitBuilding = (System.Action<ETypeData>)InitBuilding;

        private static int[] FuckMoneyLevel = new int[6] { 100, 200, 400, 800, 1400, 1900 };

        public Brothel()
        {
            Bitchs.Clear();
            g.events.On(EGameType.IntoWorld, (System.Action<ETypeData>)delegate
            {
                string @string = g.data.obj.GetString("妓女列表");
                if (!string.IsNullOrWhiteSpace(@string))
                {
                    Bitchs = JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, List<string>>>(@string);
                }
                string string2 = g.data.obj.GetString("绑架列表");
                if (!string.IsNullOrWhiteSpace(string2) && string2 != "null")
                {
                    Kidnapping = JsonConvert.DeserializeObject<System.Collections.Generic.List<string>>(string2);
                }
            }, 1);
            g.events.On(EGameType.OneOpenUIEnd(UIType.Town), OnInitBuilding, 0);
            Events.On(EGameType.SaveData, SaveData);
        }

        private void SaveData(ETypeData e)
        {
            g.data.obj.SetString("妓女列表", JsonConvert.SerializeObject(Bitchs));
            g.data.obj.SetString("绑架列表", JsonConvert.SerializeObject(Kidnapping));
        }

        public static void HandleRemoveKidnapping(UnitActionLuckDel instance)
        {
            if (instance.luckID == -541655652)
            {
                Kidnapping.Remove(instance.unit.GetID());
            }
        }

        private static void InitBuilding(ETypeData e)
        {
            UITown uI = g.ui.GetUI<UITown>(UIType.Town);
            if (!(uI == null) && uI.town.buildTownData.isMainTown)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(uI.goBuildItem.gameObject, uI.goPosiRoot.transform.parent, worldPositionStays: false);
                gameObject.SetActive(value: true);
                gameObject.GetComponent<RectTransform>().anchoredPosition = GetPosition(g.data.grid.GetGridData(g.world.playerUnit.data.unitData.GetPoint()).areaBaseID) - new Vector2(100f, 100f);
                gameObject.GetComponentInChildren<Text>().text = "青楼";
                gameObject.GetComponentInChildren<Button>().onClick.AddListener((System.Action)OnOpenBrothel);
                Sprite sprite = SpriteTool.GetSprite("SchoolCommon", "BuildDown1101_4");
                gameObject.GetComponentInChildren<Image>().sprite = sprite;
                gameObject.GetComponentInChildren<Image>().SetNativeSize();
            }
        }

        private static void OnOpenBrothel()
        {
            DramaTool.OpenDrama("1811210000|1811210006|1811210007", new DramaData
            {
                unitLeft = g.world.playerUnit
            });
        }

        private static Vector2 GetPosition(int area)
        {
            Vector2[] array = new Vector2[11]
            {
            default(Vector2),
            new Vector2(-333f, -291f),
            new Vector2(-333f, -291f),
            default(Vector2),
            new Vector2(-333f, -291f),
            default(Vector2),
            new Vector2(-353f, -346f),
            default(Vector2),
            new Vector2(-572f, -279f),
            new Vector2(333f, -291f),
            new Vector2(333f, -291f)
            };
            if (area < 0 || area >= array.Length)
            {
                return new Vector2(-602.9f, -602.9f);
            }
            return array[area];
        }

        public void Dispose()
        {
            g.events.Off(EGameType.OneOpenUIEnd(UIType.Town), OnInitBuilding);
            Events.Off(EGameType.SaveData, SaveData);
        }

        public static void OnMonthEnd()
        {
            foreach (KeyValuePair<string, List<string>> bitch in Bitchs)
            {
                string key = bitch.Key;
                MapBuildTown build = g.world.build.GetBuild<MapBuildTown>(key);
                if (build == null)
                {
                    Log.Debug("ID为" + key + "的主城不存在");
                    continue;
                }
                List<string> value = bitch.Value;
                if (value.Count == 0)
                {
                    Log.Debug(build.name + "目前没有妓女");
                    continue;
                }
                Il2CppSystem.Collections.Generic.List<WorldUnitBase> unitExact = g.world.unit.GetUnitExact(build.GetOrigiPoint(), 20, isGetHideUnit: false);
                List<string> list = value.Where((string s) => s != g.world.playerUnit.GetID()).ToList();
                Il2CppSystem.Collections.Generic.List<WorldUnitBase>.Enumerator enumerator2 = unitExact.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    WhoringLogic(enumerator2.Current, Config.Instance.BrothelSettings.FemalePlayerAutoFuck ? value : list);
                }
                string[] array = value.ToArray();
                foreach (string text in array)
                {
                    WorldUnitBase unit = g.world.unit.GetUnit(text);
                    if (unit == null)
                    {
                        Bitchs[key].Remove(text);
                        continue;
                    }
                    ReSetBitchLevel(unit);
                    ResetCanFuckCount(unit);
                    unit.MoveTo(build.GetOrigiPoint());
                    if (Config.Instance.BrothelSettings.NpcCanRedemptionSelf)
                    {
                        CheckSelfReception(unit, key, build);
                    }
                }
            }
        }

        private static void WhoringLogic(WorldUnitBase man, System.Collections.Generic.IEnumerable<string> bitchIdList)
        {
            if (man == g.world.playerUnit || man.isDie || man.IsWoman() || man.GetAge() < 192)
            {
                return;
            }
            int num = 15;
            if (man.data.GetCharacter().Contains(17))
            {
                num += 100;
            }
            if (man.data.GetCharacter().Contains(19))
            {
                num = ((!bitchIdList.Contains(man.data.unitData.relationData.married)) ? (num - 200) : (num + 100));
            }
            if (string.IsNullOrWhiteSpace(man.data.unitData.relationData.married))
            {
                num += System.Math.Min((man.GetAge() / 12 - 30) / 5 * 20, 100);
            }
            int propsNum = man.data.unitData.propData.GetPropsNum(10001);
            num += (System.Math.Min(FuckMoneyLevel.Last(), propsNum) - 800) / 40;
            if (num >= 0 && CommonTool.Random(0, 100) >= 50)
            {
                man.AddMonthLog("eventLogRoleBitch24328073", man);
                WorldUnitBase satisfiedBitch = GetSatisfiedBitch(man, bitchIdList);
                if (satisfiedBitch == null)
                {
                    man.AddMonthLog("eventLogRoleBitch24328074", "在青楼没有找到喜欢的妓女");
                }
                else
                {
                    Fuck(man, satisfiedBitch);
                }
            }
        }

        public static System.Collections.Generic.IEnumerable<WorldUnitBase> GetPlayerTodayBrothelFuckers()
        {
            WorldUnitBase player = g.world.playerUnit;
            if (player.IsMan())
            {
                return new List<WorldUnitBase>();
            }
            return from b in g.world.unit.GetUnitExact(player.data.unitData.GetPoint(), 5, isGetHideUnit: false).ToArray().Distinct(new UnitCompare())
                    .Where(CheckTargetCanFuckPlayerOrNot)
                   orderby b.data.unitData.relationData.GetIntim(player)
                   select b;
        }

        private static bool CheckTargetCanFuckPlayerOrNot(WorldUnitBase target)
        {
            if (target.isDie || target.GetAge() <= 192 || !target.IsMan())
            {
                return false;
            }
            if (target.data.unitData.residueDay < 0)
            {
                return false;
            }
            if (CommonTool.Random(0, 2) > 0)
            {
                return false;
            }
            int bitchLevel = GetBitchLevel(g.world.playerUnit);
            return GetMoneyCanFuckLevel(target.data.unitData.propData.GetPropsNum(10001)) >= bitchLevel;
        }

        private static int GetFuckMoney(int bitchLevel)
        {
            int num = (bitchLevel - 1).InRange(0, 5);
            return FuckMoneyLevel[num];
        }

        private static int GetMoneyCanFuckLevel(int money)
        {
            for (int num = 6; num >= 1; num--)
            {
                if (money > GetFuckMoney(num))
                {
                    return num;
                }
            }
            return 0;
        }

        private static WorldUnitBase GetSatisfiedBitch(WorldUnitBase man, System.Collections.Generic.IEnumerable<string> bitchIdList)
        {
            List<WorldUnitBase> list = (from id in bitchIdList
                                                                   select g.world.unit.GetUnit(id) into npc
                                                                   where npc != null && !npc.isDie && GetCanFuckCount(npc) > 0
                                                                   select npc).ToList();
            if (list.Count == 0)
            {
                man.AddMonthLog("eventLogRoleBitch24328074", "在青楼没有点到可以接客的妓女");
                return null;
            }
            int money = man.GetMoney();
            int canFuckLevel = GetMoneyCanFuckLevel(money);
            if (canFuckLevel < 1)
            {
                man.AddMonthLog("eventLogRoleBitch24328074", "太穷");
                return null;
            }
            List<WorldUnitBase> list2 = list.Where((WorldUnitBase u) => GetBitchLevel(u) > 0 && GetBitchLevel(u) <= canFuckLevel).ToList();
            if (list2.Count == 0)
            {
                man.AddMonthLog("eventLogRoleBitch24328074", "没有嫖得起的妓女");
                return null;
            }
            if (man.data.GetCharacter().Contains(21))
            {
                WorldUnitBase worldUnitBase = list2.FirstOrDefault((WorldUnitBase b) => man.data.unitData.relationData.parent.Contains(b.GetID()));
                if (worldUnitBase != null)
                {
                    Log.Debug(man.GetName() + "在青楼中点了自己的母亲" + worldUnitBase.GetName());
                    return worldUnitBase;
                }
            }
            if (man.data.GetCharacter().Contains(23))
            {
                using (System.Collections.Generic.IEnumerator<WorldUnitBase> enumerator = list2.Where((WorldUnitBase b) => man.data.unitData.relationData.brother.Contains(b.GetID())).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        WorldUnitBase current = enumerator.Current;
                        Log.Debug(man.GetName() + "在青楼中点了自己的姐妹" + current.GetName());
                        return current;
                    }
                }
            }
            if (man.data.GetCharacter().Contains(25))
            {
                using (System.Collections.Generic.IEnumerator<WorldUnitBase> enumerator = list2.Where((WorldUnitBase b) => man.data.unitData.relationData.children.Contains(b.GetID()) || man.data.unitData.relationData.childrenPrivate.Contains(b.GetID())).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        WorldUnitBase current2 = enumerator.Current;
                        Log.Debug(man.GetName() + "在青楼中点了自己的女儿" + current2.GetName());
                        return current2;
                    }
                }
            }
            if (man.data.GetCharacter().Contains(26))
            {
                using (System.Collections.Generic.IEnumerator<WorldUnitBase> enumerator = list2.Where((WorldUnitBase b) => man.data.unitData.relationData.master.Contains(b.GetID())).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        WorldUnitBase current3 = enumerator.Current;
                        Log.Debug(man.GetName() + "在青楼中点了自己的师父" + current3.GetName());
                        return current3;
                    }
                }
            }
            if (man.data.GetCharacter().Contains(27))
            {
                using (System.Collections.Generic.IEnumerator<WorldUnitBase> enumerator = list2.Where((WorldUnitBase b) => man.data.unitData.relationData.student.Contains(b.GetID())).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        WorldUnitBase current4 = enumerator.Current;
                        Log.Debug(man.GetName() + "在青楼中点了自己的徒儿" + current4.GetName());
                        return current4;
                    }
                }
            }
            if (man.data.GetCharacter().Contains(19))
            {
                using (System.Collections.Generic.IEnumerator<WorldUnitBase> enumerator = list2.Where((WorldUnitBase b) => man.data.unitData.relationData.married == b.GetID()).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        WorldUnitBase current5 = enumerator.Current;
                        Log.Debug(man.GetName() + "在青楼中点了自己的妻子" + current5.GetName());
                        return current5;
                    }
                }
            }
            return RandomBitchByWight(list2);
        }

        private static WorldUnitBase RandomBitchByWight(List<WorldUnitBase> bitches)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < bitches.Count; i++)
            {
                WorldUnitBase worldUnitBase = bitches[i];
                if (worldUnitBase != null)
                {
                    int num = GetBitchLevel(worldUnitBase) + worldUnitBase.GetEvilFallLevel();
                    for (int j = 0; j < num; j++)
                    {
                        list.Add(i);
                    }
                }
            }
            int index = CommonTool.Random(0, list.Count - 1);
            int index2 = list[index].InRange(0, bitches.Count - 1);
            return bitches[index2];
        }

        public static void HandelChildGrowUp(WorldUnitBase unit)
        {
            WorldUnitBase worldUnitBase = null;
            foreach (string item in unit.data.unitData.relationData.parent)
            {
                WorldUnitBase unit2 = g.world.unit.GetUnit(item);
                if (unit2 != null && unit2.IsWoman())
                {
                    worldUnitBase = unit2;
                    break;
                }
            }
            if (worldUnitBase != null && worldUnitBase.IsBitch())
            {
                Log.Debug(unit.GetName() + "开始青楼考核");
                MapBuildTown mapBuildTown = g.world.build.GetBuilds<MapBuildTown>().ToArray().First((MapBuildTown t) => t.buildTownData.isMainTown && unit.data.unitData.pointGridData.areaBaseID == t.gridData.areaBaseID);
                if (Bitchs.ContainsKey(mapBuildTown.buildData.id))
                {
                    Bitchs[mapBuildTown.buildData.id].Add(unit.GetID());
                }
                else
                {
                    Bitchs.Add(mapBuildTown.buildData.id, new List<string> { unit.GetID() });
                }
                unit.SetInt("进入青楼时间", g.world.run.roundMonth);
                unit.AddMonthLog("eventLogRoleBitch24328093", worldUnitBase);
                unit.AddVitalLog("eventLogRoleBitch24328093", worldUnitBase);
                ReSetBitchLevel(unit);
                ResetCanFuckCount(unit);
            }
        }

        public static int Sell(WorldUnitBase trafficker, WorldUnitBase target, MapBuildBase town = null)
        {
            if (target.IsMan())
            {
                Log.Debug("- -怎么总有人搞骚操作，男人都被我过滤了");
                return -1;
            }
            if (town == null)
            {
                town = g.world.build.GetBuilds<MapBuildTown>().ToArray().FirstOrDefault((MapBuildTown t) => t.buildTownData.isMainTown && trafficker.data.unitData.pointGridData.areaBaseID == t.gridData.areaBaseID);
            }
            if (town == null)
            {
                Log.Debug("无法获取到" + trafficker.GetName() + "所在的城镇");
                return -1;
            }
            if (Bitchs.ContainsKey(town.buildData.id))
            {
                Bitchs[town.buildData.id].Add(target.GetID());
            }
            else
            {
                Bitchs.Add(town.buildData.id, new List<string> { target.GetID() });
            }
            if (Bitchs[town.buildData.id].Count >= Config.Instance.BrothelSettings.BitchCountForCity)
            {
                if (trafficker.IsPlayer())
                {
                    Tools.ToastBig("城镇妓女数量已达上限，无法接纳更多妓女");
                }
                Log.Debug(town.name + "妓女数量已达上限，无法接纳更多妓女");
                return -1;
            }
            target.SetSeller(trafficker);
            target.SetInt("进入青楼时间", g.world.run.roundMonth);
            if (target != trafficker)
            {
                target.AddMonthLog("eventLogRoleBitch24328065", trafficker);
                target.AddVitalLog("eventLogRoleBitch24328065", trafficker);
                trafficker.AddMonthLog("eventLogRoleBitch24328066", target);
                if (trafficker.IsVital(target) || (trafficker.IsPlayer() && target.IsSexSlave()))
                {
                    trafficker.AddVitalLog("eventLogRoleBitch24328066", target);
                }
                Log.Debug(trafficker.GetName() + "将" + target.GetName() + "卖入了" + town.name + "(" + town.buildData.id + ")青楼");
                target.RemoveLuck(-541655652);
                target.data.unitData.SetPoint(town.GetOrigiPoint());
                int money = target.GetMoney();
                target.AddMoney(-money);
                trafficker.AddMoney(money);
                if (trafficker.IsPlayer())
                {
                    UICostItemTool.AddTipText($"你扒光了<color=#BC1717>{target.GetName()}</color>搜身，获得了{money}灵石");
                }
            }
            else
            {
                target.AddMonthLog("eventLogRoleBitch24328071", trafficker);
                target.AddVitalLog("eventLogRoleBitch24328071", trafficker);
                Log.Debug(trafficker.GetName() + "自愿进入了" + town.name + "青楼当妓女");
            }
            var (result, num) = ReSetBitchLevel(target);
            ResetCanFuckCount(target);
            if (num < 4 && target != trafficker)
            {
                target.data.unitData.relationData.AddHate(trafficker.GetID(), 300f, 0);
            }
            return result;
        }

        private static (int bitchLevel, int evilFallLevel) ReSetBitchLevel(WorldUnitBase target)
        {
            float num = 0f;
            int evilFallLevel = target.GetEvilFallLevel();
            num += (float)(evilFallLevel - 1) * 0.5f;
            int num2 = target.data.unitData.propertyData.beauty;
            switch (evilFallLevel)
            {
                case 2:
                    num2 -= 80;
                    break;
                case 3:
                    num2 -= 100;
                    break;
                case 4:
                    num2 -= 200;
                    break;
                case 5:
                    num2 -= 300;
                    break;
            }
            num2 = System.Math.Min(num2, 1200);
            num += (float)((num2 - 400) / 250) * 0.4f;
            int num3 = target.data.unitData.propertyData.reputation;
            MapBuildSchool school = target.data.school;
            if (school != null)
            {
                if (school.buildData.npcSchoolMain == target.GetID())
                {
                    num3 -= 25000;
                    num += (school.IsTopSchool() ? 1.5f : 1f);
                }
                else if (school.buildData.npcBigElders.Contains(target.GetID()))
                {
                    num += (school.IsTopSchool() ? 0.6f : 0.4f);
                }
                else if (school.buildData.npcElders.Contains(target.GetID()))
                {
                    num += (school.IsTopSchool() ? 0.3f : 0.2f);
                }
            }
            num3 = num3.InRange(0, 120000);
            if (num3 >= 100000)
            {
                num += 5f;
            }
            else if (num3 >= 80000)
            {
                num += 4f;
            }
            else if (num3 >= 60000)
            {
                num += 3f;
            }
            else if (num3 >= 40000)
            {
                num += 2.5f;
            }
            else if (num3 >= 25000)
            {
                num += 2f;
            }
            else if (num3 >= 15000)
            {
                num += 1.5f;
            }
            else if (num3 >= 8000)
            {
                num += 1f;
            }
            else if (num3 >= 5000)
            {
                num += 0.5f;
            }
            else if (num3 >= 3000)
            {
                num += 0.2f;
            }
            int @int = target.GetInt("卖身次数");
            num += (float)(@int - 30) / 60f;
            for (int i = 527000001; i <= 527000006; i++)
            {
                target.RemoveLuck(i);
            }
            int num4 = ((int)num).InRange(1, 6);
            target.AddLuck(527000000 + num4);
            if (GetBitchLevel(target) != num4)
            {
                string text = new string[6] { "6l1EMgCO", "m8xlKfFZ", "mU51Omwb", "Q1Nj9HlG", "xUaNlGwe", "qpRdkqEN" }.Select((string s) => GameTool.LS(s)).ToList()[num4 - 1];
                target.AddMonthLog("eventLogRoleBitch24328067", text);
                Log.Debug($"{target.GetName()}成为了{num4}级妓女{text}");
            }
            target.SetInt("妓女等级", num4);
            return (bitchLevel: evilFallLevel, evilFallLevel: num4);
        }

        public static int GetBitchLevel(WorldUnitBase target)
        {
            return target.GetInt("妓女等级");
        }

        private static int GetCanFuckCount(WorldUnitBase target)
        {
            return target.GetInt("本月可卖身次数");
        }

        private static void DelCanFuckCount(WorldUnitBase target)
        {
            int @int = target.GetInt("本月可卖身次数");
            if (@int > 0)
            {
                target.SetInt("本月可卖身次数", @int - 1);
            }
        }

        private static void ResetCanFuckCount(WorldUnitBase target)
        {
            int value = (int)((float)GetBitchLevel(target) * 0.5f * 30f);
            target.SetInt("本月可卖身次数", value);
        }

        private static bool CheckSelfReception(WorldUnitBase target, string townId, MapBuildTown town)
        {
            if (target.IsPlayer() || target.isDie)
            {
                return false;
            }
            if (target.GetInt("进入青楼时间") + Config.Instance.BrothelSettings.CanRedemptionMonth < g.world.run.roundMonth)
            {
                return false;
            }
            int money = target.GetMoney();
            int num = GetBitchLevel(target) * 1000;
            if (money < num)
            {
                return false;
            }
            int num2 = 4 - target.data.unitData.propertyData.inTrait;
            int num3 = (target.data.GetCharacter().Contains(19) ? 2 : 0);
            num3 -= (target.data.GetCharacter().Contains(17) ? 2 : 0);
            int num4 = 2 * (3 - target.GetEvilFallLevel());
            int num5 = (money - num) / 15000;
            int num6 = num2 + num3 + num4 + num5;
            if (num6 > 0 && CommonTool.Random(0, num6) > 0)
            {
                if (target.IsSexSlave())
                {
                    System.Collections.Generic.KeyValuePair<string, string>? seller = target.GetSeller();
                    if (seller.HasValue && seller.Value.Key == g.world.playerUnit.GetID())
                    {
                        Log.Debug("性奴" + target.GetName() + "想离开青楼，但是迫于你的淫威不敢");
                        return false;
                    }
                }
                if (Bitchs[townId].Remove(target.GetID()))
                {
                    target.AddMoney(-num);
                    Log.Debug(target.GetName() + "攒够了钱，将自己从青楼赎了出来");
                    target.AddMonthLog("eventLogRoleBitch24328081");
                    target.AddVitalLog("eventLogRoleBitch24328081");
                    for (int i = 1; i <= 6; i++)
                    {
                        target.RemoveLuck(527000000 + i);
                    }
                    return true;
                }
            }
            return false;
        }

        public static void Fuck(WorldUnitBase fucker, WorldUnitBase target)
        {
            if (fucker.IsWoman() || target.IsMan())
            {
                Log.Debug("你用修改器了改性别了吧- -");
                return;
            }
            if (fucker.IsPlayer())
            {
                fucker.RoundStart(1);
                GameTool.AutoDayNotTip();
            }
            if (target.IsPlayer())
            {
                target.RoundStart(1);
                GameTool.AutoDayNotTip();
            }
            int bitchLevel = GetBitchLevel(target);
            int num = (int)((float)CommonTool.Random(0, 10) / 100f * (float)FuckMoneyLevel[bitchLevel - 1]);
            int num2 = FuckMoneyLevel[bitchLevel - 1] + CommonTool.Random(-num, num);
            int money = fucker.GetMoney();
            int num3 = ((num2 > money) ? money : num2);
            fucker.AddMoney(-num3);
            System.Collections.Generic.KeyValuePair<string, string>? seller = target.GetSeller();
            WorldUnitBase worldUnitBase = null;
            if (seller.HasValue)
            {
                string key = seller.Value.Key;
                worldUnitBase = ((key == target.GetID()) ? target : g.world.unit.GetUnit(key));
            }
            if (worldUnitBase == null)
            {
                worldUnitBase = target;
            }
            int num4 = (int)((float)(num3 * bitchLevel) * 10f / 100f);
            if (worldUnitBase == target)
            {
                target.AddMoney(num4);
                if (worldUnitBase.IsPlayer() && !g.world.run.isRunning)
                {
                    UICostItemTool.AddTipText($"你服侍<color=#BC1717>{fucker.GetName()}</color>赚取了{num4}灵石");
                }
            }
            else
            {
                target.AddMoney(num4 / 2);
                worldUnitBase.AddMoney(num4 / 2);
                if (worldUnitBase.IsPlayer() && !fucker.IsPlayer() && !g.world.run.isRunning)
                {
                    UICostItemTool.AddTipText($"妓女<color=#BC1717>{target.GetName()}</color>为你赚取了{num4 / 2}灵石");
                }
            }
            if (target.data.unitData.propertyData.energy >= target.data.unitData.propertyData.energyMax)
            {
                20.Percent(delegate
                {
                    target.data.RewardPropItem(-829499283, 1, target.IsPlayer());
                });
            }
            else
            {
                target.data.unitData.propertyData.energy++;
                20.Percent(delegate
                {
                    target.data.RewardPropItem(-1442461760, 1, target.IsPlayer());
                });
            }
            int num5 = 8 - target.GetEvilFallLevel();
            if (target.HasLuck(1561118811))
            {
                num5 += 5;
            }
            int num6 = target.AddBrothelFucker(fucker);
            if (num6 > 0)
            {
                if (num6 == 1)
                {
                    if (target.HasLuck(-527021125))
                    {
                        num5 += 40;
                        Log.Debug(target.GetName() + "以处女身第一次接客，客人是[" + fucker.GetName() + "]。 " + target.GetName() + " 堕落+40");
                        target.AddMonthLog("eventLogRoleBitch24328068", fucker);
                        target.AddVitalLog("eventLogRoleBitch24328068", fucker);
                        target.SetFirstMan(fucker.GetID(), fucker.GetName());
                        target.RemoveLuck(-527021125);
                        if (fucker.IsPlayer())
                        {
                            UICostItemTool.AddTipText("你将妓女<color=#BC1717>" + target.GetName() + "</color>破处了！");
                        }
                        if (target.IsPlayer())
                        {
                            UICostItemTool.AddTipText("淫贱的你被<color=#BC1717>" + target.GetName() + "</color>破处了！");
                        }
                    }
                    else
                    {
                        num5 += 20;
                        Log.Debug(target.GetName() + "第一次接客，客人是[" + fucker.GetName() + "]。 " + target.GetName() + " 堕落+20");
                        target.AddMonthLog("eventLogRoleBitch24328069", fucker);
                        target.AddVitalLog("eventLogRoleBitch24328069", fucker);
                    }
                }
                else
                {
                    int num7 = 7 - bitchLevel;
                    num5 += num7;
                    Log.Debug($"{target.GetName()}增加接客对象，客人是[{fucker.GetName()}]。堕落+{num7}");
                    target.AddMonthLog("eventLogRoleBitch24328070", fucker);
                }
            }
            else
            {
                target.AddMonthLog("eventLogRoleBitch24328070", fucker);
            }
            fucker.AddMonthLog("eventLogRoleBitch24328072", target);
            Log.Debug($"{target.GetName()}本次卖身给{fucker.GetName()},共增加堕落值{num5}");
            target.AddDepravation(num5);
            int @int = target.GetInt("卖身次数");
            target.SetInt("卖身次数", @int + 1);
            if (@int + 1 > 100)
            {
                target.data.unitData.propertyData.beauty += 5;
            }
            DelCanFuckCount(target);
            WorldUnitLuckBase luck = target.GetLuck(744093962);
            if (luck != null)
            {
                if (Config.Instance.WillAbortion(luck.luckData.duration))
                {
                    Log.Debug(target.GetName() + "怀孕时被嫖客" + fucker.GetName() + "操到流产");
                    target.AddMonthLog("eventLogRoleHuaiYun24328053", fucker);
                    target.RemoveLuck(744093962);
                    int int2 = target.GetInt("流产次数");
                    if (int2 > Config.Instance.BirthSettings.AbortionCount)
                    {
                        target.AddLuck(-336709509);
                        Log.Debug("母狗" + target.GetName() + "流产次数过多，已经不会怀孕了");
                        if (fucker.IsPlayer())
                        {
                            UICostItemTool.AddTipText("<color=#BC1717>" + target.GetName() + "</color>流产且不会再怀孕了");
                        }
                        target.AddMonthLog("eventLogRoleHuaiYun24328054");
                    }
                    else
                    {
                        target.AddLuck(-1782743473);
                        target.SetInt("流产次数", int2 + 1);
                        Log.Debug($"母狗{target.GetName()}流产{int2 + 1}次");
                        if (fucker.IsPlayer())
                        {
                            UICostItemTool.AddTipText("<color=#BC1717>" + target.GetName() + "</color>流产了");
                        }
                    }
                }
                else if (Config.Instance.WillPrematureBirth(luck.luckData.duration))
                {
                    WorldUnitBase worldUnitBase2 = target;
                    WorldUnitBase playerUnit = g.world.playerUnit;
                    Log.Debug($"母狗早产：[{target.GetName()}], 怀孕时长：{Config.Instance.BirthSettings.PregnancyDuration + 1 - luck.luckData.duration}");
                    string str = target.GetStr("被谁操");
                    WorldUnitBase worldUnitBase3 = null;
                    if (str == null)
                    {
                        if (target.IsSexSlave() && !target.IsPlayer())
                        {
                            worldUnitBase3 = playerUnit;
                        }
                    }
                    else
                    {
                        worldUnitBase3 = g.world.unit.GetUnit(str);
                    }
                    string text = ((worldUnitBase3 != null) ? ("<color=#BC1717>" + worldUnitBase2.GetName() + "</color>与<color=#BC1717>" + fucker.GetName() + "</color>孕期性交导致早产, 生下了<color=#BC1717>" + worldUnitBase3.GetName() + "</color>的孩子") : ("<color=#BC1717>" + worldUnitBase2.GetName() + "</color>与<color=#BC1717>" + fucker.GetName() + "</color>孕期性交导致早产, 生下没有父亲的野种"));
                    Log.Debug(text);
                    target.AddMonthLog("eventLogRoleHuaiYun24328057", fucker);
                    if (fucker.IsPlayer())
                    {
                        UICostItemTool.AddTipText(text);
                    }
                    worldUnitBase2.AddLuck(-1892102884);
                    worldUnitBase2.RemoveLuck(744093962);
                    UnitSexType unitSexType = ((worldUnitBase3 == null || !worldUnitBase3.IsPlayer() || !Config.Instance.BirthSettings.ChildMustBeWomen) ? ((CommonTool.Random(0, 100) <= Config.Instance.BirthSettings.ChildIsMalePercent) ? UnitSexType.Man : UnitSexType.Woman) : UnitSexType.Woman);
                    int npcSpecialID = 1008612;
                    if (worldUnitBase3 != null && worldUnitBase3.IsPlayer())
                    {
                        npcSpecialID = ((worldUnitBase2.IsSexSlave() && Config.Instance.SexSlaveSettings.SexSlaveInherit && unitSexType == UnitSexType.Woman) ? 1008613 : 1008611);
                    }
                    WorldUnitBase worldUnitBase4 = Units.CreateChild(npcSpecialID, worldUnitBase3, worldUnitBase2, unitSexType, Config.Instance.BirthSettings.NewbornGrowUp16 ? 192 : 12);
                    worldUnitBase4.SetFaceInherit(worldUnitBase3, worldUnitBase2, Config.Instance.BirthSettings.FaceInheritPercent);
                    worldUnitBase4.AddLuck(-748164927);
                    if (worldUnitBase4.IsWoman())
                    {
                        worldUnitBase4.AddLuck(527000000);
                    }
                    BringUp.BabyList.Add(worldUnitBase4);
                    if (worldUnitBase2.IsSexSlave())
                    {
                        if (worldUnitBase4.IsWoman() && Config.Instance.SexSlaveSettings.SexSlaveChildMarryFather)
                        {
                            worldUnitBase4.ChangedMarried(playerUnit, isTwoWay: false);
                        }
                        Log.Debug("母狗[" + worldUnitBase2.GetName() + "]生下了新的精盆[" + worldUnitBase4.GetName() + "(" + worldUnitBase4.data.unitData.unitID + ")]");
                    }
                    worldUnitBase2.AddMonthLog("eventLogRoleHuaiYun24328056", (worldUnitBase3 != null) ? worldUnitBase2.LogCall(worldUnitBase3) : "不知道谁", worldUnitBase4.LogName());
                }
            }
            else if (!target.HasLuck(-336709509) && !target.HasLuck(-1782743473) && !target.HasLuck(321486433) && CommonTool.Random(1, 100) <= Config.Instance.BirthSettings.PregnancyPercent)
            {
                Log.Debug("[" + fucker.GetName() + "]与[" + target.GetName() + "]双修，[" + target.GetName() + "]怀孕了");
                target.AddLuck(744093962, Config.Instance.BirthSettings.PregnancyDuration);
                target.SetStr("被谁操", fucker.data.unitData.unitID);
                target.AddMonthLog("eventLogRoleHuaiYun24328052", fucker);
            }
            fucker.RemoveLuck(1840247464);
            target.RemoveLuck(-523596344);
            List<string> list = new List<string>();
            list.AddRange(target.data.unitData.relationData.children.ToArray());
            list.AddRange(target.data.unitData.relationData.childrenPrivate.ToArray());
            foreach (string item in list)
            {
                WorldUnitBase unit = g.world.unit.GetUnit(item);
                if (unit != null && unit.HasLuck(527000000))
                {
                    unit.AddDepravation(num5 / 2);
                    unit.AddMonthLog("eventLogRoleBitch24328094", target);
                    Log.Debug($"目睹母亲接客，{unit.GetName()}的淫乱值增加{num5 / 2}");
                }
            }
        }

        public static void Reception()
        {
            List<WorldUnitBase> list = GetPlayerTodayBrothelFuckers().ToList();
            if (list.Count == 0)
            {
                UITipItem.AddTip(GameTool.LS("tip_2019707429"), 2.5f);
                return;
            }
            int bitchLevel = GetBitchLevel(g.world.playerUnit);
            if (bitchLevel <= 0)
            {
                return;
            }
            if (bitchLevel <= 4)
            {
                WorldUnitBase worldUnitBase = list.OrderBy((WorldUnitBase b) => b.data.unitData.relationData.GetIntim(g.world.playerUnit)).First();
                DramaTool.OpenDrama(1811210010, new DramaData
                {
                    unit = worldUnitBase,
                    unitLeft = g.world.playerUnit,
                    unitRight = worldUnitBase
                });
                return;
            }
            PersonList personList = new PersonList(list);
            personList.OnItemClick = delegate (WorldUnitBase target)
            {
                DramaTool.OpenDrama(1811210010, new DramaData
                {
                    unit = target,
                    unitLeft = g.world.playerUnit,
                    unitRight = target
                });
            };
            personList.Open();
        }

        public static void SellKidnapping()
        {
            WorldUnitBase player = g.world.playerUnit;
            MapBuildBase town = g.world.build.GetBuild(player.data.unitData.GetPoint());
            if (town == null)
            {
                return;
            }
            List<WorldUnitBase> list = (from b in Kidnapping?.Select((string s) => g.world.unit.GetUnit(s))
                                                                   where b != null && !b.isDie && !b.IsBitch()
                                                                   orderby b.data.unitData.relationData.GetIntim(player)
                                                                   select b).ToList();
            if (list == null || list.Count == 0)
            {
                UITipItem.AddTip(GameTool.LS("tip_2019707430"), 2.5f);
                return;
            }
            PersonList personList = new PersonList(list);
            personList.OnItemClick = delegate (WorldUnitBase target)
            {
                DramaTool.OpenDrama(1811210008, new DramaData
                {
                    unitLeft = g.world.playerUnit,
                    onDramaEndCall = (System.Action)delegate
                    {
                        Sell(player, target, town);
                    }
                });
            };
            personList.Open();
        }

        public static void PlayerSelectBitch()
        {
            WorldUnitBase player = g.world.playerUnit;
            MapBuildBase build = g.world.build.GetBuild(player.data.unitData.GetPoint());
            if (build == null)
            {
                return;
            }
            Il2CppSystem.Collections.Generic.List<string> list = new Il2CppSystem.Collections.Generic.List<string>();
            if (!Bitchs.ContainsKey(build.buildData.id))
            {
                UITipItem.AddTip(GameTool.LS("tip_2019707431"), 2.5f);
                return;
            }
            foreach (string item in Bitchs[build.buildData.id])
            {
                list.Add(item);
            }
            PersonList personList = new PersonList((from b in g.world.unit.GetUnit(list).ToArray()
                                                    where b != null && !b.isDie
                                                    select b).OrderByDescending(GetBitchLevel).ThenBy((WorldUnitBase b) => b.data.unitData.relationData.GetIntim(player)).ToList());
            personList.OnItemClick = delegate (WorldUnitBase target)
            {
                DramaTool.OpenDrama(1249271967, new DramaData
                {
                    unit = target,
                    unitLeft = player,
                    unitRight = target,
                    onDramaEndCall = (System.Action)delegate
                    {
                        Fuck(player, target);
                    }
                });
            };
            personList.Open();
        }

        public static void Redemption()
        {
            WorldUnitBase player = g.world.playerUnit;
            MapBuildBase town = g.world.build.GetBuild(player.data.unitData.GetPoint());
            if (town == null)
            {
                return;
            }
            DramaTool.OpenDrama(1811210015, new DramaData
            {
                unitLeft = player,
                onDramaEndCall = (System.Action)delegate
                {
                    if (!Bitchs.ContainsKey(town.buildData.id) || Bitchs[town.buildData.id].Count == 0)
                    {
                        Tools.ToastBig("当前青楼尚未开张");
                    }
                    else
                    {
                        PersonList personList = new PersonList((from s in Bitchs[town.buildData.id]
                                                                where s != player.GetID()
                                                                select s into id
                                                                select g.world.unit.GetUnit(id) into b
                                                                where b != null && !b.isDie
                                                                orderby b.data.unitData.relationData.GetIntim(player) descending
                                                                select b).ThenByDescending(GetBitchLevel).ToList());
                        personList.OnItemClick = delegate (WorldUnitBase target)
                        {
                            if (!CanReception(player, target))
                            {
                                Tools.ToastBig("你当前无法为" + target.GetName() + "赎身");
                            }
                            else
                            {
                                DramaTool.OpenDrama(1811210016, new DramaData
                                {
                                    unit = target,
                                    unitLeft = player
                                });
                            }
                        };
                        personList.Open();
                    }
                }
            });
        }

        public static void HandleDramaOptionsFunction(string[] data, DramaFunction dramaFunction)
        {
            WorldUnitBase unitRight = dramaFunction.data.dramaData.unitRight;
            WorldUnitBase playerUnit = dramaFunction.playerUnit;
            MapBuildBase build = g.world.build.GetBuild(playerUnit.data.unitData.GetPoint());
            switch (data[1])
            {
                case "接客":
                    Reception();
                    break;
                case "卖仇人":
                    SellKidnapping();
                    break;
                case "卖自己":
                    if (build != null)
                    {
                        int num = Sell(playerUnit, playerUnit, build);
                        if (num > 0)
                        {
                            Log.Debug($"自愿进入了{build.name}青楼，成为了{num}级妓女");
                            Tools.ToastBig($"自愿进入了{build.name}青楼，成为了{num}级妓女");
                        }
                    }
                    break;
                case "选妓女":
                    PlayerSelectBitch();
                    break;
                case "赎人":
                    Redemption();
                    break;
                case "赎自己":
                    {
                        foreach (KeyValuePair<string, List<string>> bitch in Bitchs)
                        {
                            if (bitch.Value.Contains(playerUnit.GetID()) && Bitchs[bitch.Key].Remove(playerUnit.GetID()))
                            {
                                playerUnit.AddMonthLog("eventLogRoleBitch24328081");
                                playerUnit.AddVitalLog("eventLogRoleBitch24328081");
                                Log.Debug("将自己从青楼赎了出来");
                                for (int i = 1; i <= 6; i++)
                                {
                                    playerUnit.RemoveLuck(527000000 + i);
                                }
                                UICostItemTool.AddTipText("你离开了青楼");
                                break;
                            }
                        }
                        break;
                    }
                case "抢人成功":
                    Redemption(unitRight, playerUnit, RedemptionWay.Fight);
                    break;
                case "赎人成功":
                    Redemption(unitRight, playerUnit, RedemptionWay.Money);
                    break;
                case "抢人失败":
                    if (playerUnit.IsMan())
                    {
                        playerUnit.AddLuck(118);
                        playerUnit.AddLuck(5010);
                        playerUnit.AddLuck(5011);
                        playerUnit.AddMonthLog("eventLogRoleBitch24328075", unitRight);
                        break;
                    }
                    dramaFunction.playerUnit.AddLuck(-541655652);
                    if (build == null)
                    {
                        Log.Debug("无法获取到" + playerUnit.GetName() + "所在的城镇");
                        break;
                    }
                    Log.Debug("你前去" + build.name + "青楼解救" + unitRight.GetName() + "不成，反而被青楼抓去做了妓女");
                    playerUnit.AddMonthLog("eventLogRoleBitch24328076", build.name, unitRight);
                    playerUnit.AddVitalLog("eventLogRoleBitch24328076", build.name, unitRight);
                    if (Bitchs.ContainsKey(build.buildData.id))
                    {
                        Bitchs[build.buildData.id].Add(playerUnit.GetID());
                    }
                    else
                    {
                        Bitchs.Add(build.buildData.id, new List<string> { playerUnit.GetID() });
                    }
                    ReSetBitchLevel(playerUnit);
                    ResetCanFuckCount(playerUnit);
                    if (playerUnit.data.GetCharacter().Contains(6))
                    {
                        playerUnit.data.unitData.relationData.AddHate(unitRight.GetID(), 300f, 0);
                    }
                    break;
                case "被嫖":
                    Fuck(unitRight, g.world.playerUnit);
                    UICostItemTool.AddTipText("你服侍了<color=#BC1717>" + unitRight.GetName() + "</color>");
                    break;
            }
        }

        private static void Redemption(WorldUnitBase target, WorldUnitBase redeemer, RedemptionWay way)
        {
            foreach (KeyValuePair<string, List<string>> bitch in Bitchs)
            {
                if (bitch.Value.Contains(target.GetID()) && Bitchs[bitch.Key].Remove(target.GetID()))
                {
                    switch (way)
                    {
                        case RedemptionWay.Fight:
                            target.AddMonthLog("eventLogRoleBitch24328078", redeemer);
                            redeemer.AddMonthLog("eventLogRoleBitch24328080", target);
                            Log.Debug(redeemer.GetName() + "从青楼抢出了" + target.GetName());
                            break;
                        case RedemptionWay.Money:
                            target.AddMonthLog("eventLogRoleBitch24328077", redeemer);
                            redeemer.AddMonthLog("eventLogRoleBitch24328079", target);
                            redeemer.AddMoney(-(GetBitchLevel(target) * 1000));
                            Log.Debug(redeemer.GetName() + "从青楼买下了" + target.GetName());
                            break;
                    }
                    for (int i = 1; i <= 6; i++)
                    {
                        target.RemoveLuck(527000000 + i);
                    }
                    break;
                }
            }
        }

        public static bool CanReception(WorldUnitBase unit, WorldUnitBase target)
        {
            if (target.isDie)
            {
                return false;
            }
            int @int = target.GetInt("进入青楼时间");
            if (g.world.run.roundMonth - @int < Config.Instance.BrothelSettings.CanRedemptionMonth)
            {
                Log.Debug(unit.GetName() + "想为" + target.GetName() + "赎身，" + $"但是{target.GetName()}{@int}月进入青楼，" + $"前只在青楼工作了{g.world.run.roundMonth - @int}个月，" + $"小于设置的{Config.Instance.BrothelSettings.CanRedemptionMonth}个月");
                return false;
            }
            int money = unit.GetMoney();
            int num = GetBitchLevel(target) * 1000;
            bool flag = money >= num;
            if (!flag)
            {
                Log.Debug(unit.GetName() + "想为" + target.GetName() + "赎身，" + $"但{target.GetName()}是{GetBitchLevel(target)}级妓女，需要{num}灵石，" + unit.GetName() + "灵石不够");
            }
            return flag;
        }

        public static void HandleCheckKidnapping(WorldUnitBase unit)
        {
        }
    }
}
