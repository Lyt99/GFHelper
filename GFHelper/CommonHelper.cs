using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GFHelper
{
    class CommonHelper
    {

        public static int ConvertDateTimeInt(System.DateTime time, bool ifoffset = false)
        {
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //intResult = (time- startTime).TotalMilliseconds;
            long t = (time.Ticks - startTime.Ticks) / 10000000;
            if (ifoffset)
                return (int)t + Models.SimpleInfo.timeoffset;
            else
                return (int)t;
        }

        public static string formatDuration(int duration)
        {
            int h, m, s;
            h = duration / 3600;
            m = duration / 60 % 60;
            s = duration % 60;
            string result = String.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
            return result;
        }

        public static int LevelToSumExp(int level, bool isUser = false)
        {
            int num = 0;
            while (level != 0)
            {
                num += CurrentLeveMaxExp(level--, isUser);
            }
            return num;
        }

        public static int ExpToLevel(int exp, bool isUser = false)
        {
            int num = 0;
            while ((exp -= CurrentLeveMaxExp(++num, isUser)) >= 0)
            {
            }
            return num;
        }

        public static int CurrentLeveMaxExp(int level, bool isUser = false)
        {
            
            if (!isUser)
            {
                if (level <= 25)
                {
                    return (level * 100);//等级小于等于25
                }
                if (level > 29)
                {
                    if (level > 69)
                    {
                        if (level > 89)
                        {
                            return (100 * (int)Math.Floor((float)Math.Pow(level * 0.15f, 2.7f)));//90+
                        }
                        return (100 * (int)Math.Floor((float)Math.Pow(level * 0.15f, 2.6f)));//70-89
                    }
                    return (100 * (int)Math.Floor((float)Math.Pow(level * 0.15f, 2.5f)));//30-69
                }
                return (100 * (int)Math.Floor((float)Math.Pow(level * 0.15f, 2.4f)));//26-29
            }

            
            if (level <= 25)
            {
                return (level * 100);
            }
            if (level > 99)
            {
                return (100 * (int)Math.Floor((float)Math.Pow(level * 0.11f, 2.5f)));
            }
            return (100 * (int)Math.Floor((float)Math.Pow(level * 0.2f, 2f)));
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
                return value;
            }
            if (value > max)
            {
                value = max;
            }
            return value;
        }




    }
}
