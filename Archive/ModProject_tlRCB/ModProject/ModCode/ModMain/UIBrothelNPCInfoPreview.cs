using System;
using UnityEngine;
using UnityEngine.UI;

namespace MOD_tlRCB
{
    public class UIBrothelNPCInfoPreview : UINPCInfoPreview
    {
        public void test()
        {
            UINPCInfo uI = g.ui.GetUI<UINPCInfo>(UIType.NPCInfo);
            GameObject gameObject = UnityEngine.Object.Instantiate(uI.uiUnitInfo.goButton1);
            Transform parent = uI.transform;
            int num = -444;
            int num2 = 150;
            gameObject.transform.SetParent(parent, worldPositionStays: false);
            gameObject.SetActive(value: true);
            gameObject.transform.localPosition = new Vector3(num, num2);
            gameObject.name = "fuck";
            gameObject.layer = int.MaxValue;
            gameObject.GetComponentInChildren<Text>().text = "调情";
            Button componentInChildren = gameObject.GetComponentInChildren<Button>();
            componentInChildren.onClick.RemoveAllListeners();
            componentInChildren.onClick.AddListener((Action)delegate
            {
                Console.WriteLine("测试");
            });
        }
    }
}
