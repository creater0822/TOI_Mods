using System;
using System.Collections.Generic;
using UnityEngine;

namespace MOD_bgToolbox
{
    public class Tool
    {
        public static string UnitTip(string unitId)
        {
            return UnitTip(ModMain.cmdRun.GetUnit(unitId));
        }

        public static string UnitTip(WorldUnitBase unit)
        {
            if (unit == null)
            {
                return "";
            }
            List<string> list = new List<string>();
            try
            {
                WorldUnitData data = unit.data;
                DataUnit.UnitInfoData unitData = data.unitData;
                WorldUnitDynData dynUnitData = data.dynUnitData;
                DataUnit.PropertyData propertyData = unitData.propertyData;
                string unitID = unitData.unitID;
                try
                {
                    string text = ((dynUnitData.sex.value == 1) ? "he" : "she");
                    try
                    {
                        string partName = g.conf.npcPartFitting.GetPartName(unitID, g.world.playerUnit);
                        string text2 = unit.data.unitData.propertyData.GetName();
                        if (partName != "")
                        {
                            text2 = text2 + "(" + partName + ")";
                        }
                        list.Add(text2);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("e1 " + ex.ToString());
                    }
                    try
                    {
                        Vector2Int point = unit.data.unitData.GetPoint();
                        string namePoint = g.data.grid.GetNamePoint(point);
                        list.Add($"{namePoint}({point.x},{point.y})");
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("e666 " + ex2.ToString());
                    }
                    try
                    {
                        list.Add((string.IsNullOrEmpty(unitData.relationData.married) ? "Single" : "Married") + "\t" + unitData.relationData.lover.Count + "个道侣");
                    }
                    catch (Exception ex3)
                    {
                        Console.WriteLine("e2 " + ex3.ToString());
                    }
                    try
                    {
                        list.Add(GameTool.LS(g.conf.roleBeauty.GetItemInBeauty(dynUnitData.beauty.value).text) + "\t" + ((dynUnitData.sex.value == 1) ? GameTool.LS("common_nan") : GameTool.LS("common_nv")));
                    }
                    catch (Exception ex4)
                    {
                        Console.WriteLine("e3 " + ex4.ToString());
                    }
                    try
                    {
                        ConfRoleGradeItem item = g.conf.roleGrade.GetItem(dynUnitData.gradeID.value);
                        list.Add(GameTool.LS(g.conf.roleRace.GetItem(dynUnitData.race.value).race) + "\t" + GameTool.LS(item.gradeName) + GameTool.LS(item.phaseName));
                    }
                    catch (Exception ex5)
                    {
                        Console.WriteLine("e4 " + ex5.ToString());
                    }
                    try
                    {
                        SchoolPostType schoolPostType = ((data.school != null) ? data.school.buildData.GetPostType(unitID) : SchoolPostType.None);
                        list.Add(((schoolPostType == SchoolPostType.None) ? GameTool.LS("player_shanxiu") : g.conf.schoolPost.GetPostName(unit)) + "\t" + ((data.school == null) ? "" : data.school.name));
                    }
                    catch (Exception ex6)
                    {
                        Console.WriteLine("e5 " + ex6.ToString());
                    }
                    try
                    {
                        List<string> list2 = new List<string>();
                        for (int i = 0; i < propertyData.hobby.Length; i++)
                        {
                            ConfRoleCreateHobbyItem item2 = g.conf.roleCreateHobby.GetItem(propertyData.hobby[i]);
                            if (item2.show != 0)
                            {
                                list2.Add(GameTool.LS(item2.name));
                            }
                        }
                        list.Add(GameTool.LS("playerInfo_xingquaihao") + ":" + string.Join("、", list2));
                    }
                    catch (Exception ex7)
                    {
                        Console.WriteLine("e6 " + ex7.ToString());
                    }
                    try
                    {
                        list.Add(GameTool.LS(g.conf.roleCreateCharacter.GetItem(dynUnitData.inTrait.value).sc5asd_sd34) + "  " + GameTool.LS(g.conf.roleCreateCharacter.GetItem(dynUnitData.outTrait1.value).sc5asd_sd34) + "、" + GameTool.LS(g.conf.roleCreateCharacter.GetItem(dynUnitData.outTrait2.value).sc5asd_sd34));
                        float num = Mathf.RoundToInt(unitData.relationData.intimToPlayerUnit);
                        float num2 = g.world.playerUnit.data.unitData.relationData.GetIntim(unitID);
                        int num3 = int.Parse(g.conf.gameParameter.closeIconValueB);
                        float num4 = num / (float)num3;
                        float num5 = num2 / (float)num3;
                        list.Add($"{text}for me{num4:F1}heart I'm right{text}{num5:F1}Heart");
                    }
                    catch (Exception ex8)
                    {
                        Console.WriteLine("e7 " + ex8.ToString());
                    }
                }
                catch (Exception ex9)
                {
                    Console.WriteLine("a " + ex9.ToString());
                }
                try
                {
                    string text3 = "<space=0.3em><voffset=-0.1em><size=130%><sprite name=\"tili\"><size=100%></voffset><space=0.3em>" + dynUnitData.hp.valueIngoreMinClamp;
                    string text4 = "<space=0.3em><voffset=-0.1em><size=130%><sprite name=\"gongji\"><size=100%></voffset><space=0.3em>" + dynUnitData.attack.value;
                    string text5 = "<space=0.3em><voffset=-0.1em><size=130%><sprite name=\"fangyu\"><size=100%></voffset><space=0.3em>" + dynUnitData.defense.value;
                    list.Add(text3 + "  " + text4 + "  " + text5);
                }
                catch (Exception ex10)
                {
                    Console.WriteLine("b " + ex10.ToString());
                }
                try
                {
                    if (unitData.heart.IsHeroes())
                    {
                        list.Add("Daoxin：" + GameTool.LS(g.conf.taoistHeart.GetItem(unitData.heart.confID).heartName));
                    }
                }
                catch (Exception ex11)
                {
                    Console.WriteLine("c " + ex11.ToString());
                }
                try
                {
                    DataUnit.ActionMartialData actionMartial = unitData.GetActionMartial(unitData.skillLeft);
                    if (actionMartial != null)
                    {
                        DataProps.MartialData martialData = actionMartial.data.To<DataProps.MartialData>();
                        list.Add("Martial Arts:" + GameTool.SetTextReplaceColorKey(martialData.martialInfo.name, GameTool.LevelToColorKey(martialData.martialInfo.level), 2));
                    }
                    DataUnit.ActionMartialData actionMartial2 = unitData.GetActionMartial(unitData.skillRight);
                    if (actionMartial2 != null)
                    {
                        DataProps.MartialData martialData2 = actionMartial2.data.To<DataProps.MartialData>();
                        list.Add("stunt:" + GameTool.SetTextReplaceColorKey(martialData2.martialInfo.name, GameTool.LevelToColorKey(martialData2.martialInfo.level), 2));
                    }
                    DataUnit.ActionMartialData actionMartial3 = unitData.GetActionMartial(unitData.step);
                    if (actionMartial3 != null)
                    {
                        DataProps.MartialData martialData3 = actionMartial3.data.To<DataProps.MartialData>();
                        list.Add("Shenfa:" + GameTool.SetTextReplaceColorKey(martialData3.martialInfo.name, GameTool.LevelToColorKey(martialData3.martialInfo.level), 2));
                    }
                    DataUnit.ActionMartialData actionMartial4 = unitData.GetActionMartial(unitData.ultimate);
                    if (actionMartial4 != null)
                    {
                        DataProps.MartialData martialData4 = actionMartial4.data.To<DataProps.MartialData>();
                        list.Add("Ultimate:" + GameTool.SetTextReplaceColorKey(martialData4.martialInfo.name, GameTool.LevelToColorKey(martialData4.martialInfo.level), 2));
                    }
                    List<string> list3 = new List<string>();
                    foreach (string ability in unit.data.unitData.abilitys)
                    {
                        DataUnit.ActionMartialData actionMartial5 = unitData.GetActionMartial(ability);
                        if (actionMartial5 != null)
                        {
                            DataProps.MartialData martialData5 = actionMartial5.data.To<DataProps.MartialData>();
                            list3.Add(GameTool.SetTextReplaceColorKey(martialData5.martialInfo.name, GameTool.LevelToColorKey(martialData5.martialInfo.level), 2));
                        }
                    }
                    list.Add("Mental method:" + string.Join(" ", list3));
                }
                catch (Exception ex12)
                {
                    Console.WriteLine("d " + ex12.ToString());
                }
                try
                {
                    list.Add("Recent experience:");
                    DataUnitLog.LogData logDataSync = g.world.unitLog.GetLogDataSync(unit.data.unitData.unitID);
                    int num6 = 0;
                    int num7 = logDataSync.allLogData.Count - 1;
                    while (num7 >= 0)
                    {
                        string logText = GetLogText(logDataSync.allLogData[num7].logs.ToArray());
                        list.Add(logText ?? "");
                        num6++;
                        if (num6 < 3)
                        {
                            num7--;
                            continue;
                        }
                        break;
                    }
                }
                catch (Exception ex13)
                {
                    Console.WriteLine("e " + ex13.ToString());
                }
            }
            catch (Exception ex14)
            {
                Console.WriteLine("aaaa " + ex14.ToString());
            }
            return string.Join("\n", list);
        }

        private static string GetLogText(DataUnitLog.LogData.Data[] logs)
        {
            string text = "";
            for (int i = 0; i < logs.Length; i++)
            {
                text = ((logs[i].id[0] != 0) ? (text + logs[i].GetLogString()) : (text + "\n"));
            }
            text = TextDelLineFeed(text);
            text = text.Replace("\n\n", "\n");
            return HretHandler(text, 1);
        }

        private static string HretHandler(string text, int bgType)
        {
            UITextHretTool.HretData hretData = new UITextHretTool.HretData();
            int num = 0;
            int num2 = 0;
            string text2 = text;
            string text3 = "";
            while (num < text2.Length)
            {
                num = text2.IndexOf("@");
                if (num == -1)
                {
                    break;
                }
                string text4 = text2.Substring(num + 1, text2.IndexOf("@", num + 1) - num - 1);
                string[] array = text4.Split('_');
                if (array.Length < 2)
                {
                    text2 = text2.Remove(0, num + text4.Length + 2);
                    num = 0;
                    num2++;
                    continue;
                }
                string text5 = array[0];
                _ = text5 + num2;
                string data = array[1];
                UITextHretTool.HretHandlerBase hretHandler = GetHretHandler(text5);
                hretHandler.Init(data, bgType, hretData);
                if (num > 0)
                {
                    text3 += text2.Substring(0, num);
                }
                text2 = text2.Remove(0, num + text4.Length + 2);
                text3 += hretHandler.GetHretText();
                num = 0;
                num2++;
            }
            text3 += text2;
            text = text3;
            text = text.Replace("</color>", "");
            text = text.Replace("<color=#FF5A1C>", "");
            text = text.Replace("<color=#004FCA>", "");
            text = text.Replace("<color=#057600>", "");
            return text;
        }

        private static string TextDelLineFeed(string logStr)
        {
            int num = 0;
            while (num < logStr.Length && logStr[num] == '\n')
            {
                logStr = logStr.Remove(num, 1);
                num--;
                num++;
            }
            int num2 = logStr.Length - 1;
            while (num2 >= 0 && logStr[num2] == '\n')
            {
                logStr = logStr.Remove(num2, 1);
                num2--;
            }
            return logStr;
        }

        private static UITextHretTool.HretHandlerBase GetHretHandler(string type)
        {
            switch (type)
            {
                case "q":
                    return new UITextHretTool.HretHandlerUnit();
                case "w":
                    return new UITextHretTool.HretHandlerProps();
                case "e":
                    return new UITextHretTool.HretHandlerLuck();
                case "r":
                    return new UITextHretTool.HretHandlerSchoolMap();
                case "t":
                    return new UITextHretTool.HretHandlerString();
                case "ps":
                    return new UITextHretTool.HretHandlerPotmonSkill();
                case "p":
                    return new UITextHretTool.HretHandlerPotmon();
                default:
                    return null;
            }
        }

        public static string GetCmdCnStr(string cmd)
        {
            return GetCmdCnStr(cmd.Split(' '));
        }

        public static string GetCmdCnStr(string[] cmd)
        {
            try
            {
                List<string> list = new List<string>();
                ConfDaguiToolItem confDaguiToolItem = ModMain.confTool.allItems[cmd[0]];
                list.Add(confDaguiToolItem.funccn);
                list.Add(confDaguiToolItem.funccn);
                return string.Join(" ", list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Join(" ", cmd);
            }
        }
    }
}
