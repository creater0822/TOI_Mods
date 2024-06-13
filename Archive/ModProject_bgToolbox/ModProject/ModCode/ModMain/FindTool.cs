using System;
using System.Collections.Generic;
using Chinese;
using UnityEngine;

namespace MOD_bgToolbox
{
    public class FindTool
    {
        public Dictionary<string, string> cache_py = new Dictionary<string, string>();

        public Dictionary<string, string> cache_pinyin = new Dictionary<string, string>();

        public string[] findStr = new string[0];

        public void SetFindStr(string findName)
        {
            if (string.IsNullOrWhiteSpace(findName))
            {
                findStr = new string[0];
                return;
            }
            findStr = findName.Split(' ');
        }

        public bool CheckFind(string itemName)
        {
            if (findStr.Length == 0)
            {
                return true;
            }
            string[] array = findStr;
            foreach (string value in array)
            {
                if (!string.IsNullOrWhiteSpace(value) && itemName.Contains(value))
                {
                    return true;
                }
            }
            string @string;
            try
            {
                @string = Pinyin.GetString(itemName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("itemName=[" + itemName + "]");
                return false;
            }
            string text = null;
            string text2 = null;
            if (cache_pinyin.ContainsKey(itemName))
            {
                text = cache_pinyin[itemName];
            }
            else if (PlayerPrefs.HasKey(itemName + "_pinyin"))
            {
                text = PlayerPrefs.GetString(itemName + "_pinyin");
            }
            if (cache_py.ContainsKey(itemName))
            {
                text2 = cache_py[itemName];
            }
            else if (PlayerPrefs.HasKey(itemName + "_py"))
            {
                text2 = PlayerPrefs.GetString(itemName + "_py");
            }
            try
            {
                if (text == null || text2 == null)
                {
                    if (ModMain.chinaInit != 0)
                    {
                        return false;
                    }
                    string[] array2 = @string.Split(' ');
                    List<string> list = new List<string>();
                    List<string> list2 = new List<string>();
                    array = array2;
                    foreach (string text3 in array)
                    {
                        if (!string.IsNullOrWhiteSpace(text3) && text3.Length >= 2)
                        {
                            list.Add(text3.Substring(0, text3.Length - 1));
                            list2.Add(text3.Substring(0, 1));
                        }
                    }
                    text = string.Join("", list);
                    text2 = string.Join("", list2);
                    PlayerPrefs.SetString(itemName + "_pinyin", text);
                    PlayerPrefs.SetString(itemName + "_py", text2);
                    cache_pinyin.Add(itemName, text);
                    cache_py.Add(itemName, text2);
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine("Pinyin search error：[" + itemName + "]\n" + ex2.ToString());
            }
            array = findStr;
            foreach (string value2 in array)
            {
                if (!string.IsNullOrWhiteSpace(value2))
                {
                    if (text.Contains(value2))
                    {
                        return true;
                    }
                    if (text2.Contains(value2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
