using System;
using System.Collections.Generic;

namespace MOD_tlRCB
{
    public static class Tools
    {
        public static void OpenUI(UIType.UITypeBase type)
        {
            DramaFunctionTool.OptionsFunction("openUI_" + type.uiName);
        }

        public static Il2CppSystem.Random GetRandom()
        {
            return new Il2CppSystem.Random(Il2CppSystem.Guid.NewGuid().GetHashCode());
        }

        public static string GetUUID()
        {
            string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int maxValue = text.Length - 1;
            Il2CppSystem.Random random = new Il2CppSystem.Random(Il2CppSystem.Guid.NewGuid().GetHashCode());
            string text2 = string.Empty;
            for (int i = 0; i <= 5; i++)
            {
                text2 += text[random.Next(0, maxValue)];
            }
            return text2;
        }

        public static void Toast(string text)
        {
            UICostItemTool.AddTipText(text);
        }

        public static void ToastBig(string text, float second = 2.5f)
        {
            UITipItem.AddTip(text, second);
        }

        public static T RandomRate<T>(IReadOnlyList<T> rate, System.Func<T, int> weightCall)
        {
            if (rate.Count == 0)
            {
                return default(T);
            }
            int num = RandomRateIndex(rate, weightCall);
            if (num != -1)
            {
                return rate[num];
            }
            return default(T);
        }

        public static int RandomRateIndex<T>(IReadOnlyList<T> rate, System.Func<T, int> weightCall)
        {
            int[] array = new int[rate.Count];
            for (int i = 0; i < rate.Count; i++)
            {
                array[i] = weightCall(rate[i]);
            }
            return RandomRate(array);
        }

        public static int RandomRate(IList<int> rate)
        {
            for (int i = 0; i < rate.Count; i++)
            {
                if (rate[i] == -1)
                {
                    return i;
                }
            }
            int[] array = new int[rate.Count];
            int[] array2 = new int[rate.Count];
            int num = 0;
            for (int j = 0; j < rate.Count; j++)
            {
                array[j] = num;
                if (rate[j] > 0)
                {
                    num += rate[j];
                }
                array2[j] = num;
            }
            int num2 = CommonTool.Random(1, num + 1);
            for (int k = 0; k < array.Length; k++)
            {
                if (num2 >= array[k] && num2 <= array2[k] && rate[k] > 0)
                {
                    return k;
                }
            }
            return -1;
        }
    }
}
