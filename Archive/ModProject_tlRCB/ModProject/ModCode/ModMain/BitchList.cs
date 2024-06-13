using System;
using UnhollowerRuntimeLib;
using UnityEngine.UI;

namespace MOD_tlRCB
{
    public class BitchList : UIBase
    {
        private WorldUnitBase target;

        static BitchList()
        {
            ClassInjector.RegisterTypeInIl2Cpp<BitchList>();
        }

        public BitchList(IntPtr ptr)
            : base(ptr)
        {
        }

        private void Start()
        {
            Console.WriteLine("执行了start");
            base.gameObject.AddComponent<UIFastClose>();
        }

        public void Update(WorldUnitBase target)
        {
            this.target = target;
            base.transform.Find("TestExample/Root/Text").GetComponent<Text>().text = target.GetName();
        }

        private void Update()
        {
        }

        private void OnDestroy()
        {
            Console.WriteLine("关闭了");
        }
    }
}
