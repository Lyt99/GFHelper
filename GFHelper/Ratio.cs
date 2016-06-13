using System;

namespace GFHelper
{
    public class Ratio
    {
        public static readonly float[][] arrAbilityRatio = new float[][] { new float[] { 0.6f, 0.6f, 0.8f, 1.5f, 1.2f, 1.8f, 0.75f }, new float[] { 1.6f, 0.6f, 1.2f, 1.2f, 0.3f, 1.6f, 0.75f }, new float[] { 0.8f, 2.4f, 0.5f, 0.7f, 1.6f, 0.8f, 1.4f }, new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f }, new float[] { 1.5f, 1.8f, 1.6f, 0.4f, 0.6f, 0.6f, 0.95f } };
        public static readonly int[] arrCoin_num;
        public static readonly int[] arrCoin_type;
        public static readonly float[] arrCostTime;
        public static readonly int[] arrExpectingEnhanceRatio;
        public static readonly float[][] arrFixRatio;
        public static readonly int[] arrLevelLimitNumber;
        internal static int maxGun;
        internal static int maxTeam;
        public static readonly int[] numberNeedRate;
        public static readonly int[] rankNeedCore;
        public static readonly int[] retireGetCore;

        static Ratio()
        {
            float[][] singleArrayArray2 = new float[5][];
            float[] singleArray1 = new float[4];
            singleArray1[0] = 2f;
            singleArray1[3] = 0.7f;
            singleArrayArray2[0] = singleArray1;
            float[] singleArray2 = new float[4];
            singleArray2[0] = 4.5f;
            singleArray2[3] = 1.2f;
            singleArrayArray2[1] = singleArray2;
            float[] singleArray3 = new float[4];
            singleArray3[0] = 3.5f;
            singleArray3[3] = 1.6f;
            singleArrayArray2[2] = singleArray3;
            float[] singleArray4 = new float[4];
            singleArray4[0] = 4f;
            singleArray4[3] = 1.4f;
            singleArrayArray2[3] = singleArray4;
            float[] singleArray5 = new float[4];
            singleArray5[0] = 7.5f;
            singleArray5[3] = 3f;
            singleArrayArray2[4] = singleArray5;
            arrFixRatio = singleArrayArray2;
            arrExpectingEnhanceRatio = new int[] { 100, 300, 50, 50 };
            arrLevelLimitNumber = new int[] { 10, 30, 70, 90, 100 };
            arrCoin_type = new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 3, 3 };
            arrCoin_num = new int[] { 50, 100, 200, 300, 120, 200, 300, 400, 200, 300 };
            arrCostTime = new float[] { 0.5f, 1f, 2f, 3f, 4f, 6f, 9f, 12f, 18f, 24f };
            rankNeedCore = new int[] { 1, 3, 9, 15 };
            numberNeedRate = new int[] { 1, 1, 2, 3 };
            int[] numArray1 = new int[4];
            numArray1[1] = 1;
            numArray1[2] = 3;
            numArray1[3] = 5;
            retireGetCore = numArray1;
        }

        public static int GetValue(int order, int[] Methed)
        {
            order = CommonHelper.Clamp(order, 0, Methed.Length - 1);
            return Methed[order];
        }

        public static float GetValue(int order, float[] Methed)
        {
            order = CommonHelper.Clamp(order, 0, Methed.Length - 1);
            return Methed[order];
        }
    }
}