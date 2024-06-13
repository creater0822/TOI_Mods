using System;
using UnityEngine;

namespace MOD_bgToolbox
{
    public class CmdKey
    {
        public bool shift;
        public bool ctrl;
        public bool alt;
        public string key = "";

        public override string ToString()
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            return (shift ? "Shift+" : "") + (ctrl ? "Ctrl+" : "") + (alt ? "Alt+" : "") + key;
        }

        public string GetState()
        {
            return (shift ? "Shift+" : "") + (ctrl ? "Ctrl+" : "") + (alt ? "Alt+" : "") + key;
        }

        public bool IsKeyDown()
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            if (shift && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                return false;
            }
            if (ctrl && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
            {
                return false;
            }
            if (alt && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
            {
                return false;
            }
            return Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), key));
        }
    }
}
