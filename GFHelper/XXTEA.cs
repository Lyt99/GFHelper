using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using boolean = System.Boolean;
//讲道理，java到C#，鬼知道能不能跑起来
//↑并不能
namespace GFHelper
{
    public class XXTEA
    {
        public static byte[] decrypt(byte[] barr1, byte[] barr2)
        {
            if (barr1.Length == 0)
            {
                return barr1;
            }
            return toByteArray(decrypt(toIntArray(barr1, false), toIntArray(barr2, false)), false);
        }

        public static int[] decrypt(int[] iarr1, int[] iarr2)
        {
            int num2;
            int num4;
            int expressionStack_14_1;
            int expressionStack_19_0;
            int length = iarr1.Length;
            int num8 = iarr1[length - 1];
            int num9 = iarr1[0];
            if (length != -1)
            {
                int expressionStack_18_1 = 0x34;
                int expressionStack_18_0 = length;
                expressionStack_19_0 = expressionStack_18_1 / expressionStack_18_0;
                goto Label_0019;
            }
            else
            {
                expressionStack_14_1 = 0x34;
                int expressionStack_14_0 = length;
            }
            expressionStack_19_0 = -expressionStack_14_1;
            Label_0019:
            num2 = (expressionStack_19_0 + 6) * -1640531527;
            int index = iarr1[0];
            Label_0026:
            num4 = (num2 >> 2) & 3;
            int num5 = length - 1;
            int num6 = index;
            index = num5;
            while (true)
            {
                if (index <= 0)
                {
                    num5 = iarr1[length - 1];
                    index = iarr1[0] - ((((num5 >> 5) ^ (num6 << 2)) + ((num6 >> 3) ^ (num5 << 4))) ^ ((num2 ^ num6) + (iarr2[(index & 3) ^ num4] ^ num5)));
                    iarr1[0] = index;
                    num6 = num2 + 0x61c88647;
                    num2 = num6;
                    if (num6 == 0)
                    {
                        return iarr1;
                    }
                    goto Label_0026;
                }
                num5 = iarr1[index - 1];
                iarr1[index] -= (((num5 >> 5) ^ (num6 << 2)) + ((num6 >> 3) ^ (num5 << 4))) ^ ((num2 ^ num6) + (iarr2[(index & 3) ^ num4] ^ num5));
                index--;
            }
        }

        public static byte[] encrypt(byte[] barr1, byte[] barr2)
        {
            if (barr1.Length == 0)
            {
                return barr1;
            }
            return toByteArray(encrypt(toIntArray(barr1, false), toIntArray(barr2, false)), false);
        }

        public static int[] encrypt(int[] iarr1, int[] iarr2)
        {
            int num2;
            int num5;
            int expressionStack_A_1;
            int expressionStack_F_0;
            int length = iarr1.Length;
            if (length != -1)
            {
                int expressionStack_E_1 = 0x34;
                int expressionStack_E_0 = length;
                expressionStack_F_0 = expressionStack_E_1 / expressionStack_E_0;
                goto Label_000F;
            }
            else
            {
                expressionStack_A_1 = 0x34;
                int expressionStack_A_0 = length;
            }
            expressionStack_F_0 = -expressionStack_A_1;
            Label_000F:
            num2 = expressionStack_F_0 + 6;
            int num3 = 0;
            int index = iarr1[length - 1];
            Label_001A:
            num5 = num3 - 0x61c88647;
            int num6 = (num5 >> 2) & 3;
            int num7 = 0;
            num3 = index;
            index = num7;
            while (true)
            {
                if (index >= (length - 1))
                {
                    int num8 = iarr1[0];
                    num7 = length - 1;
                    index = iarr1[num7] + ((((num3 >> 5) ^ (num8 << 2)) + ((num8 >> 3) ^ (num3 << 4))) ^ ((num5 ^ num8) + (iarr2[(index & 3) ^ num6] ^ num3)));
                    iarr1[num7] = index;
                    num7 = num2 - 1;
                    num2 = num7;
                    num3 = num5;
                    if (num7 <= 0)
                    {
                        return iarr1;
                    }
                    goto Label_001A;
                }
                num7 = iarr1[index + 1];
                iarr1[index] += (((num3 >> 5) ^ (num7 << 2)) + ((num7 >> 3) ^ (num3 << 4))) ^ ((num5 ^ num7) + (iarr2[(index & 3) ^ num6] ^ num3));
                index++;
            }
        }

        private static byte[] toByteArray(int[] numArray1, bool flag1)
        {
            int num = flag1 ? 1 : 0;
            int index = numArray1.Length << 2;
            int num3 = index;
            if (num != 0)
            {
                num3 = numArray1[numArray1.Length - 1];
                if (num3 > index)
                {
                    return null;
                }
            }
            byte[] buffer2 = new byte[num3];
            index = 0;
            while (true)
            {
                byte[] buffer = buffer2;
                if (index >= num3)
                {
                    return buffer;
                }
                buffer2[index] = (byte)((sbyte)((numArray1[index >> 2] >> ((index & 3) << 3)) & 0xff));
                index++;
            }
        }

        private static int[] toIntArray(byte[] buffer1, bool flag1)
        {
            int num2;
            int[] numArray;
            int num = flag1 ? 1 : 0;
            if ((buffer1.Length & 3) != 0)
            {
                num2 = (buffer1.Length >> 2) + 1;
            }
            else
            {
                num2 = buffer1.Length >> 2;
            }
            if (num == 0)
            {
                numArray = new int[num2];
            }
            else
            {
                numArray = new int[num2 + 1];
                numArray[num2] = buffer1.Length;
            }
            int length = buffer1.Length;
            for (num2 = 0; num2 < length; num2++)
            {
                int index = num2 >> 2;
                numArray[index] |= buffer1[num2] << ((num2 & 3) << 3);
            }
            return numArray;
        }
    }
}
