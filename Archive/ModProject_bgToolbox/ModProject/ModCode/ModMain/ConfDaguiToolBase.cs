using System;
using System.Collections.Generic;

namespace MOD_bgToolbox
{
    public class ConfDaguiToolBase
    {
        public readonly Dictionary<string, ConfDaguiToolItem> allItems = new Dictionary<string, ConfDaguiToolItem>();

        public readonly Dictionary<string, List<ConfDaguiToolItem>> items = new Dictionary<string, List<ConfDaguiToolItem>>();

        public ConfDaguiToolBase(string text)
        {
            string[] array = text.Split('\n');
            foreach (string text2 in array)
            {
                if (string.IsNullOrEmpty(text2))
                {
                    continue;
                }
                string[] array2 = text2.Split('\t');
                if (array2.Length >= 13 && !(array2[12].Trim() != "y"))
                {
                    ConfDaguiToolItem confDaguiToolItem = new ConfDaguiToolItem(array2[0], array2[1], array2[2], array2[3], array2[4], array2[5], array2[6], array2[7], array2[8], array2[9], array2[10], array2[11]);
                    if (!items.ContainsKey(confDaguiToolItem.type))
                    {
                        items.Add(confDaguiToolItem.type, new List<ConfDaguiToolItem>());
                    }
                    items[confDaguiToolItem.type].Add(confDaguiToolItem);
                    allItems.Add(confDaguiToolItem.func, confDaguiToolItem);
                }
            }
            Console.WriteLine("Command categories populated:" + items.Count);
            foreach (KeyValuePair<string, List<ConfDaguiToolItem>> item in items)
            {
                Console.WriteLine(item.Key + " quantity: " + item.Value.Count);
            }
        }
    }
}
