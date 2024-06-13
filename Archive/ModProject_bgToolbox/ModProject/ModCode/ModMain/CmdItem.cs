using System.Collections.Generic;

namespace MOD_bgToolbox
{
    public class CmdItem
    {
        public List<string> cmds = new List<string>();
        public string name = "";
        public CmdKey key = new CmdKey();

        public override string ToString()
        {
            List<string> list = new List<string>(cmds.ToArray());
            list.Insert(0, name + "|" + (key.shift ? "@" : "") + (key.ctrl ? "#" : "") + (key.alt ? "%" : "") + key);
            return string.Join("\n", list);
        }

        public void Run()
        {
            foreach (string cmd in cmds)
            {
                ModMain.cmdRun.Cmd(cmd.Split(' '));
            }
        }

        public static CmdItem FromString(string str)
        {
            CmdItem cmdItem = new CmdItem();
            List<string> list = new List<string>(str.Split('\n'));
            if (list.Count > 0)
            {
                list.RemoveAll((string v) => string.IsNullOrWhiteSpace(v));
                for (int i = 1; i < list.Count; i++)
                {
                    cmdItem.cmds.Add(list[i]);
                }
                string[] array = list[0].Split('|');
                cmdItem.name = array[0];
                if (array.Length > 1)
                {
                    string text = array[1];
                    if (text.StartsWith("@"))
                    {
                        cmdItem.key.shift = true;
                        text = text.Substring(1, text.Length - 1);
                    }
                    if (text.StartsWith("#"))
                    {
                        cmdItem.key.ctrl = true;
                        text = text.Substring(1, text.Length - 1);
                    }
                    if (text.StartsWith("%"))
                    {
                        cmdItem.key.alt = true;
                        text = text.Substring(1, text.Length - 1);
                    }
                    cmdItem.name = text;
                }
            }
            return cmdItem;
        }
    }
}
