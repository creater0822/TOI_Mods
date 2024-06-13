using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB
{
    public static class UITools
    {
        public static T OpenUI<T>(this UIMgr ui, string name) where T : Component
        {
            UIBase uIBase = ui.OpenUI(new UIType.UITypeBase(name, UILayer.UI));
            T val = uIBase.gameObject.GetComponent<T>();
            if (val == null)
            {
                val = uIBase.gameObject.AddComponent<T>();
            }
            return val;
        }

        public static void Bind(this Slider slider, int value, Action<int> onValueChange)
        {
            slider.value = value.InRange((int)slider.minValue, (int)slider.maxValue);
            slider.onValueChanged.AddListener((Action<float>)delegate (float f)
            {
                onValueChange((int)f);
            });
        }

        public static void Bind(this Toggle toggle, bool value, Action<bool> onValueChange, string tip = null)
        {
            toggle.isOn = value;
            toggle.onValueChanged.AddListener((Action<bool>)onValueChange.Invoke);
            if (!string.IsNullOrWhiteSpace(tip))
            {
                toggle.transform.parent.AddSkyTip(tip);
            }
        }

        public static StringBuilder Append(this StringBuilder sb, bool bold = false, bool italics = false, int size = 0, string color = null, string text = null, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return sb;
            }
            if (bold)
            {
                sb.Append("<b>");
            }
            if (italics)
            {
                sb.Append("<i>");
            }
            if (size > 0)
            {
                sb.Append("<size=").Append(size).Append(">");
            }
            if (!string.IsNullOrWhiteSpace(color))
            {
                sb.Append("<color=").Append(color).Append(">");
            }
            sb.AppendFormat(text, args);
            if (!string.IsNullOrWhiteSpace(color))
            {
                sb.Append("</color>");
            }
            if (size > 0)
            {
                sb.Append("</size>");
            }
            if (italics)
            {
                sb.Append("</i>");
            }
            if (bold)
            {
                sb.Append("</b>");
            }
            return sb;
        }
    }
}
