using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace MOD_tlRCB
{
    internal class Drama
    {
        public static void OpenDramaAfterMonthRun(int dramaId, DramaFunctionData data)
        {
            DramaFunctionTool.OptionsFunction("callDramaRunEnd_" + dramaId, data);
        }

        public static void OpenDrama(WorldUnitBase unit, int dramaID, string str = "", Action call = null)
        {
            ConfDramaDialogueItem confDramaDialogueItem = g.conf.dramaDialogue.GetItem(1009037).Clone();
            confDramaDialogueItem.id = dramaID;
            confDramaDialogueItem.nextDialogue = "0";
            confDramaDialogueItem.speaker = 2;
            confDramaDialogueItem.npcRight = 11;
            g.conf.dramaDialogue.AddItem(confDramaDialogueItem);
            UICustomDramaDyn uICustomDramaDyn = new UICustomDramaDyn(dramaID);
            if (uICustomDramaDyn != null && unit != null)
            {
                uICustomDramaDyn.dramaData.dialogueText[dramaID] = str;
                uICustomDramaDyn.dramaData.unitLeft = unit;
                uICustomDramaDyn.dramaData.unitRight = unit;
                uICustomDramaDyn.dramaData.onDramaEndCall = call;
                uICustomDramaDyn.OpenUI();
            }
        }

        public static void OpenDrama(WorldUnitBase unit, int dramaID, string str = "")
        {
            ConfDramaDialogueItem confDramaDialogueItem = g.conf.dramaDialogue.GetItem(1009037).Clone();
            confDramaDialogueItem.id = dramaID;
            confDramaDialogueItem.nextDialogue = "0";
            confDramaDialogueItem.speaker = 2;
            confDramaDialogueItem.npcRight = 11;
            g.conf.dramaDialogue.AddItem(confDramaDialogueItem);
            UICustomDramaDyn uICustomDramaDyn = new UICustomDramaDyn(dramaID);
            if (uICustomDramaDyn != null && unit != null)
            {
                uICustomDramaDyn.dramaData.dialogueText[dramaID] = str;
                uICustomDramaDyn.dramaData.unitLeft = unit;
                uICustomDramaDyn.dramaData.unitRight = unit;
                uICustomDramaDyn.OpenUI();
            }
        }

        public static void OpenDrama(int speaker, int dramaID, WorldUnitBase unit, string str = "", string option1 = null, string option2 = null, Action call = null, Action call2 = null)
        {
            ConfDramaDialogueItem confDramaDialogueItem = g.conf.dramaDialogue.GetItem(1009037).Clone();
            confDramaDialogueItem.id = dramaID;
            confDramaDialogueItem.nextDialogue = "0";
            confDramaDialogueItem.speaker = speaker;
            confDramaDialogueItem.npcRight = 11;
            g.conf.dramaDialogue.AddItem(confDramaDialogueItem);
            UICustomDramaDyn uICustomDramaDyn = new UICustomDramaDyn(dramaID);
            uICustomDramaDyn.dramaData.dialogueText[dramaID] = str;
            uICustomDramaDyn.dramaData.unitLeft = unit;
            uICustomDramaDyn.dramaData.unitRight = unit;
            uICustomDramaDyn.dramaData.dialogueOptions.Add(1, option1);
            uICustomDramaDyn.dramaData.dialogueOptions.Add(2, option2);
            uICustomDramaDyn.dramaData.dialogueOptionsText.Add(1, option1);
            uICustomDramaDyn.dramaData.dialogueOptionsText.Add(2, option2);
            uICustomDramaDyn.OnOptionCall(1, call);
            if (call2 != null)
            {
                uICustomDramaDyn.OnOptionCall(2, call2);
            }
            uICustomDramaDyn.OpenUI();
        }

        public static void OpenDrama(bool isBattle, int speaker, int dramaID, WorldUnitBase unit, string str = "")
        {
            ConfDramaDialogueItem confDramaDialogueItem = g.conf.dramaDialogue.GetItem(1009037).Clone();
            confDramaDialogueItem.id = dramaID;
            confDramaDialogueItem.nextDialogue = "0";
            confDramaDialogueItem.speaker = speaker;
            confDramaDialogueItem.function = "npcAttackForce_unitB";
            confDramaDialogueItem.npcRight = 11;
            g.conf.dramaDialogue.AddItem(confDramaDialogueItem);
            UICustomDramaDyn uICustomDramaDyn = new UICustomDramaDyn(dramaID);
            uICustomDramaDyn.dramaData.dialogueText[dramaID] = str;
            uICustomDramaDyn.dramaData.unitLeft = unit;
            uICustomDramaDyn.dramaData.unitRight = unit;
            uICustomDramaDyn.OpenUI();
        }

        public static void ModifyOptions(int id, string options, bool prefix = true)
        {
            ConfDramaDialogueItem item = g.conf.dramaDialogue.GetItem(id);
            if (string.IsNullOrWhiteSpace(item.options) || item.options == "0")
            {
                item.options = options;
            }
            else if (prefix)
            {
                item.options = options + "|" + item.options;
            }
            else
            {
                item.options = item.options + "|" + options;
            }
        }

        public static void ModifyOptions(int id, string options, int index)
        {
            ConfDramaDialogueItem item = g.conf.dramaDialogue.GetItem(id);
            if (string.IsNullOrWhiteSpace(item.options) || item.options == "0")
            {
                item.options = options;
                return;
            }
            List<string> list = options.Split('|').ToList();
            if (list.Count > index)
            {
                item.options += options;
                return;
            }
            list.Insert(index, options);
            item.options = HarmonyLib.GeneralExtensions.Join(list, null, "|");
        }

        public static void ModifyOptions(int id, List<string> options, int index = -1)
        {
            if (!options.Any())
            {
                return;
            }
            ConfDramaDialogueItem item = g.conf.dramaDialogue.GetItem(id);
            if (string.IsNullOrWhiteSpace(item.options) || item.options == "0")
            {
                item.options = HarmonyLib.GeneralExtensions.Join(options, null, "|"); // Changed from Harmony -> HarmonyLib, not sure if it's for a reason
                return;
            }
            List<string> list = item.options.Split('|').ToList();
            options.RemoveAll((string o) => list.Contains(o));
            if (index == -1 || index >= list.Count)
            {
                list.AddRange(options);
            }
            else
            {
                list.InsertRange(index, options);
            }
            item.options = HarmonyLib.GeneralExtensions.Join(list.Distinct(), null, "|");
        }
    }
}
