using UnityEngine;

namespace MOD_tlRCB
{
    public static class GameObjects
    {
        public static UISkyTipEffect AddSkyTip(this GameObject gameObject, string tip, bool isLeftAligen = false)
        {
            UISkyTipEffect uISkyTipEffect = gameObject.AddComponent<UISkyTipEffect>();
            uISkyTipEffect.InitData(tip);
            uISkyTipEffect.isLeftAligen = isLeftAligen;
            return uISkyTipEffect;
        }

        public static UISkyTipEffect AddSkyTip(this Transform transform, string tip, bool isLeftAligen = false)
        {
            return AddSkyTip(transform.gameObject, tip, isLeftAligen);
        }
    }
}
