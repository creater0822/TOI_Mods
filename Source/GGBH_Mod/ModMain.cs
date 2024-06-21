using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using EGameTypeData;
using MelonLoader;

namespace GGBH_MOD
{
    public class ModMain : MelonMod
    {
        private class ModItem
        {
            public string modNamespace;
            public List<Assembly> assembly = new List<Assembly>();
            public object modMainBase;
            public bool isLoad;
        }

        private static Dictionary<string, ModItem> allMod = new Dictionary<string, ModItem>();
        private static int initModMgrHashCode;

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            File.WriteAllText(Environment.CurrentDirectory + "/MelonLoader/GGBH_MOD_LOAD_COMPLETE.txt", "");
        }

        public static void OnGameCMDMelonLoader(ETypeData e)
        {
            CMDMelonLoader cMDMelonLoader = e.Cast<CMDMelonLoader>();
            if (cMDMelonLoader.cmd == "LoadDll")
            {
                string filePath = cMDMelonLoader.values[0].ToString();
                string nameSpace = cMDMelonLoader.values[1].ToString();
                try
                {
                    MelonHandler.LoadFromFile(filePath, nameSpace);
                    if (!allMod.ContainsKey(nameSpace))
                    {
                        ModItem modItem = new ModItem();
                        modItem.modNamespace = nameSpace;
                        allMod[nameSpace] = modItem;
                    }
                    Assembly modAsm = Assembly.LoadFrom(filePath);
                    allMod[nameSpace].assembly.Add(modAsm);
                    return;
                }
                catch (Exception)
                {
                    return;
                }
            }
            if (cMDMelonLoader.cmd == "InitModMain")
            {
                if (initModMgrHashCode == g.mod.GetHashCode())
                {
                    return;
                }
                initModMgrHashCode = g.mod.GetHashCode();
                foreach (KeyValuePair<string, ModItem> modItemDel in allMod)
                { // Try to destroy any loaded TOI mods
                    if (modItemDel.Value.modMainBase != null && modItemDel.Value.isLoad)
                    {
                        try
                        {
                            modItemDel.Value.isLoad = false;
                            Console.WriteLine("DestroyModMain：" + modItemDel.Key);
                            modItemDel.Value.modMainBase.GetType().GetMethod("Destroy").Invoke(modItemDel.Value.modMainBase, new object[0]);
                            modItemDel.Value.modMainBase = null;
                        }
                        catch (Exception ex2)
                        {
                            Console.WriteLine(ex2.ToString());
                        }
                    }
                }
                List<string> list = new List<string>();
                list.Add(cMDMelonLoader.values.Count.ToString());
                for (int i = 0; i < cMDMelonLoader.values.Count; i++)
                {
                    if (cMDMelonLoader.values[i] != null)
                    {
                        list.Add(cMDMelonLoader.values[i].ToString());
                    }
                }
                foreach (KeyValuePair<string, ModItem> modItemInit in allMod)
                {
                    if (!list.Contains(modItemInit.Value.modNamespace))
                    {
                        continue;
                    }
                    if (modItemInit.Value.modMainBase == null && !modItemInit.Value.isLoad)
                    {
                        modItemInit.Value.isLoad = true;
                        for (int j = 0; j < modItemInit.Value.assembly.Count; j++)
                        {
                            if (modItemInit.Value.assembly[j].GetType(modItemInit.Key + ".ModMain") != null)
                            {
                                object obj = modItemInit.Value.assembly[j].CreateInstance(modItemInit.Key + ".ModMain");
                                if (obj == null)
                                {
                                    Console.WriteLine("Class initializations “" + modItemInit.Key + ".ModMain” failed");
                                    Console.WriteLine("Resemble “" + modItemInit.Key + ".ModMain” no inheritance ModMainBase");
                                }
                                else if (obj != null)
                                {
                                    modItemInit.Value.modMainBase = obj;
                                    break;
                                }
                            }
                        }
                        if (modItemInit.Value.modMainBase == null)
                        {
                            Console.WriteLine("Can't find Type“" + modItemInit.Key + ".ModMain”");
                        }
                    }
                    if (modItemInit.Value.modMainBase != null)
                    {
                        try
                        {
                            Console.WriteLine("InitModMain：" + modItemInit.Key);
                            modItemInit.Value.modMainBase.GetType().GetMethod("Init").Invoke(modItemInit.Value.modMainBase, new object[0]);
                        }
                        catch (Exception ex3)
                        {
                            Console.WriteLine(ex3.ToString());
                        }
                    }
                }
                return;
            }
            _ = cMDMelonLoader.cmd == "DestroyModMain";
        }
    }
}
