using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace LuciferModifier
{
    public static class ByteConverters
    {
        public static string LongToHex(long val, OffSetPanelFixedWidth offsetwight = OffSetPanelFixedWidth.Dynamic)
        {
            return val.ToString((offsetwight == OffSetPanelFixedWidth.Dynamic) ? ConstantReadOnly.HexStringFormat : ConstantReadOnly.HexLineInfoStringFormat, CultureInfo.InvariantCulture);
        }

        public static string LongToString(long val, int saveBits = -1)
        {
            if (saveBits == -1)
            {
                return FormattableString.Invariant($"{val}");
            }
            char[] array = new char[saveBits];
            for (int i = 1; i <= saveBits; i++)
            {
                array[saveBits - i] = (char)(val % 10 + 48);
                val /= 10;
            }
            return new string(array);
        }

        public static char ByteToChar(byte val)
        {
            return (char)((val > 31 && (val <= 126 || val >= 160)) ? val : 46);
        }

        public static byte CharToByte(char val)
        {
            return (byte)val;
        }

        public static string ByteToHex(byte[] data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte val in data)
            {
                string value = ByteToHex(val);
                stringBuilder.Append(value);
                stringBuilder.Append(' ');
            }
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            return stringBuilder.ToString();
        }

        public static char[] ByteToHexCharArray(byte val)
        {
            char[] array = new char[2];
            ByteToHexCharArray(val, array);
            return array;
        }

        public static void ByteToHexCharArray(byte val, char[] charArr)
        {
            if (charArr == null)
            {
                throw new ArgumentNullException("charArr");
            }
            if (charArr.Length != 2)
            {
                throw new ArgumentException($"The length of {charArr} should be 2.");
            }
            charArr[0] = ByteToHexChar(val >> 4);
            charArr[1] = ByteToHexChar(val - (val >> 4 << 4));
        }

        public static char ByteToHexChar(int val)
		{
			if (val < 10)
			{
				return (char)(48 + val);
			}
            char c;
            switch (val)
            {
                case 10:
                    c = 'A';
                    break;
                case 11:
                    c = 'B';
                    break;
                case 12:
                    c = 'C';
                    break;
                case 13:
                    c = 'D';
                    break;
                case 14:
                    c = 'E';
                    break;
                case 15:
                    c = 'F';
                    break;
                default:
                    c = 's';
                    break;
            }
            return c;
		}

        public static string ByteToHex(byte val)
        {
            return new string(ByteToHexCharArray(val));
        }

        public static string BytesToString(byte[] buffer, ByteToString converter = ByteToString.ByteToCharProcess)
        {
            if (buffer == null)
            {
                return string.Empty;
            }
            switch (converter)
            {
                case ByteToString.AsciiEncoding:
                    return Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                case ByteToString.ByteToCharProcess:
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (byte val in buffer)
                        {
                            stringBuilder.Append(ByteToChar(val));
                        }
                        return stringBuilder.ToString();
                    }
                default:
                    return string.Empty;
            }
        }

        public static byte[] HexToByte(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return null;
            }
            hex = hex.Trim();
            string[] array = hex.Split(' ');
            byte[] array2 = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                string hex2 = array[i];
                var (flag, b) = HexToUniqueByte(hex2);
                if (!flag)
                {
                    return null;
                }
                array2[i] = b;
            }
            return array2;
        }

        public static (bool success, byte val) HexToUniqueByte(string hex)
        {
            byte result;
            return (success: byte.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result), val: result);
        }

        public static (bool success, long position) HexLiteralToLong(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return (success: false, position: -1L);
            }
            int num = ((hex.Length > 1 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X')) ? 2 : 0);
            long num2 = 0L;
            while (num < hex.Length)
            {
                int num3 = hex[num++];
                if (num3 >= 48 && num3 <= 57)
                {
                    num3 -= 48;
                }
                else if (num3 >= 65 && num3 <= 70)
                {
                    num3 = num3 - 65 + 10;
                }
                else
                {
                    if (num3 < 97 || num3 > 102)
                    {
                        return (success: false, position: -1L);
                    }
                    num3 = num3 - 97 + 10;
                }
                num2 = 16 * num2 + num3;
            }
            return (success: true, position: num2);
        }

        public static (bool success, long value) IsHexValue(string hexastring)
        {
            return HexLiteralToLong(hexastring);
        }

        public static (bool success, byte[] value) IsHexaByteStringValue(string hexastring)
        {
            return (HexToByte(hexastring) == null) ? (success: false, value: null) : (success: true, value: HexToByte(hexastring));
        }

        public static byte[] StringToByte(string str)
        {
            return str.Select(CharToByte).ToArray();
        }

        public static string StringToHex(string str)
        {
            return ByteToHex(StringToByte(str));
        }
    }
}
