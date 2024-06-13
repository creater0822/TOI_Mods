using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using MelonLoader;

namespace LuciferModifier
{
    internal class FileUtils
    {
        private static string GetFilePath()
        {
            return Path.Combine(MelonHandler.ModsDirectory, "LuciferModifier.ini");
        }

        public static void GuiguInit()
        {
            string filePath = GetFilePath();
            if (!File.Exists(filePath))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("# illustrate:");
                stringBuilder.AppendLine("# Just modify the content on the right side of the equal sign. The content on the left side of the equal sign cannot be modified, otherwise the MOD will not take effect.");
                stringBuilder.AppendLine("\n# LuciferGuigu built-in tool calls shortcut key 1, letters must be lowercase");
                stringBuilder.AppendLine("config1 = f4");
                stringBuilder.AppendLine("\n# LuciferGuigu built-in tool calls shortcut key 2, the letters must be lowercase");
                stringBuilder.AppendLine("config2 = p");
                File.WriteAllText(filePath, stringBuilder.ToString());
            }
        }

        public static Dictionary<string, string> readFile()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string filePath = GetFilePath();
            string[] array = File.ReadAllLines(filePath);
            string[] array2 = array;
            foreach (string text in array2)
            {
                if (!text.StartsWith("#"))
                {
                    string[] array3 = text.Split('=');
                    if (array3.Length == 2)
                    {
                        string key = array3[0].Trim();
                        string value = array3[1].Trim();
                        dictionary.Add(key, value);
                    }
                }
            }
            return dictionary;
        }

        public static string getProperties<T>(T t)
        {
            string text = string.Empty;
            if (t == null)
            {
                return text;
            }
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length == 0)
            {
                return text;
            }
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                string name = propertyInfo.Name;
                object value = propertyInfo.GetValue(t, null);
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name.StartsWith("String"))
                {
                    text += $"{name}:{value},";
                }
                else
                {
                    getProperties(value);
                }
            }
            return text;
        }
    }
}
