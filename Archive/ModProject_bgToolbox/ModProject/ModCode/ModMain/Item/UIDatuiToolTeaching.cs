using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_bgToolbox.Item
{
    public class UIDatuiToolTeaching : MonoBehaviour
    {
        public Text textTitle;

        public UIDatuiToolTeaching(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Awake()
        {
            textTitle = base.transform.Find("Root/Text1").GetComponent<Text>();
            textTitle.text = "Toolbox tutorial";
            base.gameObject.AddComponent<UIFastClose>();
            base.transform.Find("Root/btnClose").GetComponent<Button>().onClick.AddListener((Action)CloseUI);
        }

        public void CloseUI()
        {
            g.ui.CloseUI(GetComponent<UIBase>());
        }
    }
}
