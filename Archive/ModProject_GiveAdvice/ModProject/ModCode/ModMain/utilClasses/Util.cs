using System;
using System.Collections.Generic;
using Il2CppNewtonsoft.Json;

namespace MOD_LE2lAt
{
    internal class Util
    {
        //private static Random random = new Random();

        public static string addCommand(string command, int add)
        {
            string s = Convert.ToString(add);
            return addCommand(command, s);
        }
        public static string addCommand(string command, string adds)
        {
            if (adds == null || adds.Equals("") || adds.Equals("0"))
            {

            }
            else if (command == null || command.Equals("") || command.Equals("0"))
            {
                command = adds;
            }
            else
            {
                string[] coms = command.Split('|');
                string[] addsArr = adds.Split('|');
                Dictionary<string, int> exists = new Dictionary<string, int>();

                foreach (string co in coms)
                {
                    if (!exists.ContainsKey(co)) exists.Add(co, 0);
                }
                foreach (string add in addsArr)
                {
                    if (!exists.ContainsKey(add))
                    {
                        command = command + "|" + add;
                        exists.Add(add, 0);
                    }
                }
            }
            return command;
        }
        public static string LS(string s)
        {
            while (true) // deal with &
            {
                int idx = s.IndexOf("&"); //0123&56&8
                if (idx > -1)//4
                {
                    string tmp = s.Substring(idx + 1);//56&8
                    int idx2 = tmp.IndexOf("&");//2
                    if (idx2 > -1)
                    {
                        s = s.Substring(0, idx) + "?" + tmp.Substring(idx2 + 1);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            while (true) // Process $
            {
                int idx = s.IndexOf("$"); //0123$56$8
                if (idx > -1)//4
                {
                    string tmp = s.Substring(idx + 1);//56$8
                    int idx2 = tmp.IndexOf("$");//2
                    if (idx2 > -1)
                    {
                        string tmp2 = tmp.Substring(0, idx2);
                        s = s.Substring(0, idx) + GameTool.LS(tmp2) + tmp.Substring(idx2 + 1);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return s;
        }
        public static string SerializeObject(Il2CppSystem.Object val)
        {
            return JsonConvert.SerializeObject(val);
        }

        public static T DeserializeObject<T>(string jsonText)
        {
            return JsonConvert.DeserializeObject<T>(jsonText);
        }

        public static T Clone<T>(T obj)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            xmlSerializer.Serialize(stream, obj);
            // The main thing is to add the process of converting the information on the Internet into a string.
            string temp = System.Text.Encoding.Default.GetString(stream.ToArray());
            stream = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(temp));
            System.Xml.XmlReaderSettings xmlReaderSettings = new System.Xml.XmlReaderSettings();
            xmlReaderSettings.IgnoreComments = true;
            System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stream, xmlReaderSettings);
            if (xmlReader != null)
            {
                T t = (T)xmlSerializer.Deserialize(xmlReader);
                return t;
            }
            return default(T);
        }
        public static string getGradeName(int grade)
        {
            /*var name = g.conf.modEnum.GetNameInType(1001, grade.ToString());
            if (!String.IsNullOrEmpty(name))
            {
                string [] s = name.Split('-');
                if(s.Length > 1)
                {
                    return s[1];
                }   
            }*/
            if (grade == 1)
            {
                return "Qi Refining";
            }
            else if (grade == 2)
            {
                return "Foundation";
            }
            else if (grade == 3)
            {
                return "Qi Condensation";
            }
            else if (grade == 4)
            {
                return "Golden Core";
            }
            else if (grade == 5)
            {
                return "Origin Spirit";
            }
            else if (grade == 6)
            {
                return "Nascent Soul";
            }
            else if (grade == 7)
            {
                return "Soul Formation";
            }
            else if (grade == 8)
            {
                return "Enlightenment";
            }
            else
            if (grade == 9)
            {
                return "Reborn";
            }
            else if (grade >= 10)
            {
                return "Transcendent";
            }
            return "";
        }

        public static void iListAddToIList<T>(Il2CppSystem.Collections.Generic.List<T> from, Il2CppSystem.Collections.Generic.List<T> to)
        {
            foreach (var item in from)
            {
                to.Add(item);
            }
        }

        public static void iListAddToSList<T>(Il2CppSystem.Collections.Generic.List<T> from, System.Collections.Generic.List<T> to)
        {
            foreach (var item in from)
            {
                to.Add(item);
            }
        }
        public static void sListAddToIList<T>(Il2CppSystem.Collections.Generic.List<T> to, System.Collections.Generic.List<T> from)
        {
            foreach (var item in from)
            {
                to.Add(item);
            }
        }
        public static void sListAddToSList<T>(System.Collections.Generic.List<T> to, System.Collections.Generic.List<T> from)
        {
            foreach (var item in from)
            {
                to.Add(item);
            }
        }
        public static string IReadOnlyListToString<T>(Il2CppSystem.Collections.Generic.IReadOnlyList<T> list) where T : Il2CppSystem.Object
        {
            string s = "";
            for (int i = 0; i < 999999; i++)
            {
                try
                {
                    var item = list[i];
                    s = s + "," + JsonConvert.SerializeObject(item);
                }
                catch (Exception e)
                {
                    break;
                }
            }
            if (s.StartsWith(",")) s = s.Substring(1);
            return s;
        }
        public static Il2CppSystem.Collections.Generic.List<T> sListToIList<T>(List<T> list)
        {
            Il2CppSystem.Collections.Generic.List<T> list2 = new Il2CppSystem.Collections.Generic.List<T>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                list2.Add(list[i]);
            }
            return list2;
        }
        public static List<T> iListToSList<T>(Il2CppSystem.Collections.Generic.List<T> IList)
        {
            List<T> list = new List<T>(IList.Count);
            for (int i = 0; i < IList.Count; i++)
            {
                list.Add(IList[i]);
            }
            return list;
        }
        public static void iListToIList<T>(List<T> from, List<T> to)
        {
            for (int i = 0; i < from.Count; i++)
            {
                to.Add(from[i]);
            }
        }

        public static int randomNext(int min, int max)
        {
            //return random.Next(min, max);
            return CommonTool.Random(min, max);
        }
    }
}
