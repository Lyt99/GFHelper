using GFHelper.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace GFHelper
{
    public class AuthCode
    {
        private enum Enum0
        {
            const_0,
            const_1
        }

        private static Encoding encoding_0 = Encoding.UTF8;

        private static string smethod_0(string string_0, int int_0, int int_1)
        {
            if (int_0 >= 0)
            {
                if (int_1 < 0)
                {
                    int_1 *= -1;
                    if (int_0 - int_1 < 0)
                    {
                        int_1 = int_0;
                        int_0 = 0;
                    }
                    else
                    {
                        int_0 -= int_1;
                    }
                }
                if (int_0 > string_0.Length)
                {
                    return string.Empty;
                }
            }
            else
            {
                if (int_1 < 0)
                {
                    return string.Empty;
                }
                if (int_1 + int_0 <= 0)
                {
                    return string.Empty;
                }
                int_1 += int_0;
                int_0 = 0;
            }
            if (string_0.Length - int_0 < int_1)
            {
                int_1 = string_0.Length - int_0;
            }
            return string_0.Substring(int_0, int_1);
        }

        private static string smethod_1(string string_0, int int_0)
        {
            return AuthCode.smethod_0(string_0, int_0, string_0.Length);
        }

        public static string MD5(string string_0)
        {
            byte[] array = AuthCode.encoding_0.GetBytes(string_0);
            array = new MD5CryptoServiceProvider().ComputeHash(array);
            string text = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                text += array[i].ToString("x").PadLeft(2, '0');
            }
            return text;
        }

        private static byte[] smethod_2(byte[] byte_0, int int_0)
        {
            byte[] array = new byte[int_0];
            for (long num = 0L; num < (long)int_0; num += 1L)
            {
                array[(int)(checked((IntPtr)num))] = (byte)num;
            }
            long num2 = 0L;
            for (long num3 = 0L; num3 < (long)int_0; num3 += 1L)
            {
                num2 = (num2 + (long)array[(int)(checked((IntPtr)num3))] + (long)byte_0[(int)(checked((IntPtr)(num3 % unchecked((long)byte_0.Length))))]) % (long)int_0;
                checked
                {
                    byte b = array[(int)((IntPtr)num3)];
                    array[(int)((IntPtr)num3)] = array[(int)((IntPtr)num2)];
                    array[(int)((IntPtr)num2)] = b;
                }
            }
            return array;
        }

        private static string smethod_3(int int_0)
        {
            char[] array = new char[]
            {
                'a',
                'b',
                'c',
                'd',
                'e',
                'f',
                'g',
                'h',
                'j',
                'k',
                'l',
                'm',
                'n',
                'o',
                'p',
                'q',
                'r',
                's',
                't',
                'u',
                'v',
                'w',
                'x',
                'y',
                'z',
                'A',
                'B',
                'C',
                'D',
                'E',
                'F',
                'G',
                'H',
                'J',
                'K',
                'L',
                'M',
                'N',
                'O',
                'P',
                'Q',
                'R',
                'S',
                'T',
                'U',
                'V',
                'W',
                'X',
                'Y',
                'Z',
                '0',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9'
            };
            int num = array.Length;
            string text = string.Empty;
            Random random = new Random();
            for (int i = 0; i < int_0; i++)
            {
                text += array[random.Next(num)];
            }
            return text;
        }

        public static string Encode(string source, string string_0)
        {
            return AuthCode.smethod_4(source, string_0, AuthCode.Enum0.const_0, 3600);
        }

        public static string Decode(string source, string string_0)
        {
            return AuthCode.smethod_4(source, string_0, AuthCode.Enum0.const_1, 3600);
        }

        private static string smethod_4(string string_0, string string_1, AuthCode.Enum0 enum0_0, int int_0)
        {
            if (string_0 == null || string_1 == null)
            {
                return string.Empty;
            }
            int int_ = 0;
            string_1 = AuthCode.MD5(string_1);
            string text = AuthCode.MD5(AuthCode.smethod_0(string_1, 16, 16));
            string text2 = AuthCode.MD5(AuthCode.smethod_0(string_1, 0, 16));
            string empty = string.Empty;
            string string_2 = text + AuthCode.MD5(text + empty);
            if (enum0_0 != AuthCode.Enum0.const_1)
            {
                string_0 = ((int_0 != 0) ? ((long)int_0 + AuthCode.UnixTimestamp()).ToString() : "0000000000") + AuthCode.smethod_0(AuthCode.MD5(string_0 + text2), 0, 16) + string_0;
                byte[] array = AuthCode.smethod_5(AuthCode.encoding_0.GetBytes(string_0), string_2);
                return empty + Convert.ToBase64String(array);
            }
            byte[] byte_;
            string empty2;
            try
            {
                byte_ = Convert.FromBase64String(AuthCode.smethod_1(string_0, int_));
                goto IL_B7;
            }
            catch
            {
                try
                {
                    byte_ = Convert.FromBase64String(AuthCode.smethod_1(string_0 + "=", int_));
                }
                catch
                {
                    try
                    {
                        byte_ = Convert.FromBase64String(AuthCode.smethod_1(string_0 + "==", int_));
                    }
                    catch
                    {
                        empty2 = string.Empty;
                        return empty2;
                    }
                }
                goto IL_B7;
            }
            return empty2;
        IL_B7:
            string @string = AuthCode.encoding_0.GetString(AuthCode.smethod_5(byte_, string_2));
            long num = long.Parse(AuthCode.smethod_0(@string, 0, 10));
            if ((num == 0L || num - AuthCode.UnixTimestamp() > 0L) && AuthCode.smethod_0(@string, 10, 16) == AuthCode.smethod_0(AuthCode.MD5(AuthCode.smethod_1(@string, 26) + text2), 0, 16))
            {
                return AuthCode.smethod_1(@string, 26);
            }
            return string.Empty;
        }

        private static byte[] smethod_5(byte[] byte_0, string string_0)
        {
            if (byte_0 != null && string_0 != null)
            {
                byte[] array = new byte[byte_0.Length];
                byte[] array2 = AuthCode.smethod_2(AuthCode.encoding_0.GetBytes(string_0), 256);
                long num = 0L;
                long num2 = 0L;
                for (long num3 = 0L; num3 < (long)byte_0.Length; num3 += 1L)
                {
                    num = (num + 1L) % (long)array2.Length;
                    num2 = (num2 + (long)array2[(int)(checked((IntPtr)num))]) % (long)array2.Length;
                    checked
                    {
                        byte b = array2[(int)((IntPtr)num)];
                        array2[(int)((IntPtr)num)] = array2[(int)((IntPtr)num2)];
                        array2[(int)((IntPtr)num2)] = b;
                        byte b2 = byte_0[(int)((IntPtr)num3)];
                        byte b3 = array2[(int)(unchecked(array2[(int)(checked((IntPtr)num))] + array2[(int)(checked((IntPtr)num2))])) % array2.Length];
                        array[(int)((IntPtr)num3)] = (byte)(b2 ^ b3);
                    }
                }
                return array;
            }
            return null;
        }

        private static string smethod_6(byte[] byte_0)
        {
            return Encoding.Unicode.GetString(Encoding.Convert(Encoding.ASCII, Encoding.Unicode, byte_0));
        }

        public static long UnixTimestamp()
        {
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            string text = DateTime.Parse(DateTime.Now.ToString()).Subtract(dateTime).Ticks.ToString();
            return long.Parse(text.Substring(0, text.Length - 7)) + (long)SimpleInfo.timeoffset;
        }

        public static string urlencode(string string_0)
        {
            string text = string.Empty;
            string text2 = "_-.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < string_0.Length; i++)
            {
                string text3 = string_0.Substring(i, 1);
                if (text2.Contains(text3))
                {
                    text += text3;
                }
                else
                {
                    byte[] bytes = AuthCode.encoding_0.GetBytes(text3);
                    byte[] array = bytes;
                    for (int j = 0; j < array.Length; j++)
                    {
                        byte b = array[j];
                        text = text + "%" + b.ToString("X");
                    }
                }
            }
            return text;
        }

        public static long time()
        {
            long arg_27_0 = DateTime.UtcNow.Ticks;
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeSpan timeSpan = new TimeSpan(arg_27_0 - dateTime.Ticks);
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}
