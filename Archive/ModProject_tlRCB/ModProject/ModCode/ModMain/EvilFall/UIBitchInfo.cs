using System;
using UnhollowerRuntimeLib;
using UnityEngine.UI;

namespace MOD_tlRCB.EvilFall
{
    public class UIBitchInfo : UIBase
    {
        private string _name;

        private Text title;

        private Image bg;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                Update();
            }
        }

        static UIBitchInfo()
        {
            ClassInjector.RegisterTypeInIl2Cpp<UIBitchInfo>();
        }

        public UIBitchInfo(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Start()
        {
            title = base.transform.Find("Root/Title").GetComponent<Text>();
            bg = base.transform.Find("Image").GetComponent<Image>();
            bg.sprite = SpriteTool.GetSpriteBigTex("BG/huodebg");
        }

        private void Update()
        {
            title.text = Name;
        }
    }
}
