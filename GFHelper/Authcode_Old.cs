using System;
using System.Text;
using System.Security.Cryptography;

namespace GFHelper
{
    enum AuthcodeMode
    {
        Encode = 1,
        Decode = 2
    }

    public class AuthCode_
    {
        private static Encoding encoding = Encoding.UTF8;

        private static string AscArr2Str(byte[] b)
        {
            return Encoding.Unicode.GetString(Encoding.Convert(Encoding.ASCII, Encoding.Unicode, b));
        }

        private static string Authcode(string source, string key, AuthcodeMode operation, int expiry, long timeoffset = 0)
        {
            if ((source == null) || (key == null))
            {
                return string.Empty;
            } 
            int lens = 0;
            key = MD5(key);
            string str = MD5(CutString(key, 0, 0x10));
            string str2 = MD5(CutString(key, 0x10, 0x10));
            string str3 = (lens <= 0) ? string.Empty : ((operation != AuthcodeMode.Decode) ? RandomString(lens) : CutString(source, 0, lens));
            string pass = str + MD5(str + str3);
            if (operation == AuthcodeMode.Decode)
            {
                byte[] buffer;
                try
                {
                    buffer = Convert.FromBase64String(CutString(source, lens));
                }
                catch
                {
                    try
                    {
                        buffer = Convert.FromBase64String(CutString(source + "=", lens));
                    }
                    catch
                    {
                        try
                        {
                            buffer = Convert.FromBase64String(CutString(source + "==", lens));
                        }
                        catch
                        {
                            return string.Empty;
                        }
                    }
                }
                string str5 = encoding.GetString(RC4(buffer, pass));
                long num2 = long.Parse(CutString(str5, 0, 10));
                if (((num2 == 0) || ((num2 - UnixTimestamp(timeoffset)) > 0L)) && (CutString(str5, 10, 0x10) == CutString(MD5(CutString(str5, 0x1a) + str2), 0, 0x10)))
                {
                    return CutString(str5, 0x1a);
                }
                return string.Empty;
            }
            source = ((expiry != 0) ? ((expiry + UnixTimestamp(timeoffset))).ToString() : "0000000000") + CutString(MD5(source + str2), 0, 0x10) + source;
            byte[] inArray = RC4(encoding.GetBytes(source), pass);
            return (str3 + Convert.ToBase64String(inArray));
        }

        private static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        private static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length *= -1;
                    if ((startIndex - length) < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex -= length;
                    }
                }
                if (startIndex > str.Length)
                {
                    return string.Empty;
                }
            }
            else if ((length >= 0) && ((length + startIndex) > 0))
            {
                length += startIndex;
                startIndex = 0;
            }
            else
            {
                return string.Empty;
            }
            if ((str.Length - startIndex) < length)
            {
                length = str.Length - startIndex;
            }
            return str.Substring(startIndex, length);
        }

        public static string Decode(string source, string key, long timeoffset = 0)
        {
            return Authcode(source, key, AuthcodeMode.Decode, 0xe10, timeoffset);
        }

        public static string DiscuzAuthcodeEncode(string source, string key, int expiry)
        {
            return Authcode(source, key, AuthcodeMode.Encode, expiry);
        }

        public static string Encode(string source, string key)
        {
            return Authcode(source, key, AuthcodeMode.Encode, 0xe10);
        }

        private static byte[] GetKey(byte[] pass, int kLen)
        {
            byte[] buffer = new byte[kLen];
            for (long i = 0L; i < kLen; i += 1L)
            {
                buffer[(int)((IntPtr)i)] = (byte)i;
            }
            long num2 = 0L;
            for (long j = 0L; j < kLen; j += 1L)
            {
                num2 = ((num2 + buffer[(int)((IntPtr)j)]) + pass[(int)((IntPtr)(j % ((long)pass.Length)))]) % ((long)kLen);
                byte num4 = buffer[(int)((IntPtr)j)];
                buffer[(int)((IntPtr)j)] = buffer[(int)((IntPtr)num2)];
                buffer[(int)((IntPtr)num2)] = num4;
            }
            return buffer;
        }

        public static string MD5(string str)
        {
            byte[] bytes = encoding.GetBytes(str);
            bytes = new MD5CryptoServiceProvider().ComputeHash(bytes);
            string str2 = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                str2 = str2 + bytes[i].ToString("x").PadLeft(2, '0');
            }
            return str2;
        }

        private static string RandomString(int lens)
        {
            char[] chArray = new char[] {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q',
            'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G',
            'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
         };
            int length = chArray.Length;
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < lens; i++)
            {
                str = str + chArray[random.Next(length)];
            }
            return str;
        }

        private static byte[] RC4(byte[] input, string pass)
        {
            if ((input == null) || (pass == null))
            {
                return null;
            }
            byte[] buffer = new byte[input.Length];
            byte[] key = GetKey(encoding.GetBytes(pass), 0x100);
            long num = 0L;
            long num2 = 0L;
            for (long i = 0L; i < input.Length; i += 1L)
            {
                num = (num + 1L) % ((long)key.Length);
                num2 = (num2 + key[(int)((IntPtr)num)]) % ((long)key.Length);
                byte num4 = key[(int)((IntPtr)num)];
                key[(int)((IntPtr)num)] = key[(int)((IntPtr)num2)];
                key[(int)((IntPtr)num2)] = num4;
                byte num5 = input[(int)((IntPtr)i)];
                byte num6 = key[(key[(int)((IntPtr)num)] + key[(int)((IntPtr)num2)]) % key.Length];
                buffer[(int)((IntPtr)i)] = (byte)(num5 ^ num6);
            }
            return buffer;
        }

        public static long time()
        {
            DateTime time2 = new DateTime(0x7b2, 1, 1, 0, 0, 0);
            TimeSpan span = new TimeSpan(DateTime.UtcNow.Ticks - time2.Ticks);
            return (long)span.TotalMilliseconds;
        }

        public static long UnixTimestamp(long timeoffset = 0)
        {
            DateTime time = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1));
            string str = DateTime.Parse(DateTime.Now.ToString()).Subtract(time).Ticks.ToString();
            return long.Parse(str.Substring(0, str.Length - 7)) + timeoffset;
        }

        public static string urlencode(string str)
        {
            string str2 = string.Empty;
            string str3 = "_-.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < str.Length; i++)
            {
                string str4 = str.Substring(i, 1);
                if (str3.Contains(str4))
                {
                    str2 = str2 + str4;
                }
                else
                {
                    foreach (byte num2 in encoding.GetBytes(str4))
                    {
                        str2 = str2 + "%" + num2.ToString("X");
                    }
                }
            }
            return str2;
        }
    }


}
