using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GFHelper.Models
{
    public class GunWithUserInfo
    {
        public int Pow;
        public int Rate;
        public int Ammo;
        public int Dodge;
        public int Hit;
        public int FixEndTime;
        public int GunExp;
        public int GunID;
        public int GunLevel;
        public int ID;
        public bool IfModification;
        public bool IsLocked;
        public int Life;
        public int Location;
        public int Mre;
        public int Number;
        public int Position;
        public int Skill1;
        public int Skill2;
        public int TeamID;
        public int UserID;
        
        GunInfo info;

        //whatever
        public int maxLife;
        private int basePow;
        private int maxAddPow;
        private int pow;
        private int baseRate;
        private int maxAddRate;
        private int baseHit;
        private int rate;
        private int maxAddHit;
        private int hit;
        private int baseDodge;
        private int maxAddDodge;
        private int dodge;
        private int range;
        private int speed;
        private float radian;

        public GunWithUserInfo()
        {
            
        }

        //或许能用了
        
        public int GetFixDuration()
        {
            
            float num =  0 - this.Life;
            float num2 = 1f;
            if (this.info.type != GunType.handgun)
            {
                if (this.info.type == GunType.machinegun)
                {
                    num2 = 2f;
                }
            }
            else
            {
                num2 = 0.5f;
            }
            if (this.isDamaged)
            {
                float num5 = 20f;
                float p = 5f;
                float num7 = 400f;
                float num8 = 35000f;
                float num9 = 0.7f;
                float num10 = 150f;
                return (int)Math.Ceiling(((num2 * (this.GunLevel + num5)) * Math.Pow(num / ((float)this.maxLife), p)) * (num7 - (num8 / ((num - (this.maxLife * num9)) + num10))));
            }
            float num3 = 20f;
            float num4 = 40f;
            return (int)Math.Ceiling((((num2 * (this.GunLevel + num3)) * num) / ((float)this.maxLife)) * num4);
        }

        public void UpdateData()
        {
            this.info = Data.gunInfo[this.GunID];
            float[] numArray = Ratio.arrAbilityRatio[((int)this.info.type) - 1];
            float num = 55f;
            float num2 = 0.555f;
            float num3 = 100f;
            this.maxLife = (int)Math.Ceiling((((num + ((this.GunLevel - 1f) * num2)) * numArray[0]) * this.info.ratioLife) / num3) * this.Number;
            num = 16f;
            num2 = 100f;
            this.basePow = (int)Math.Ceiling(((num * numArray[1]) * this.info.ratioPow) / num2);
            num = 0.242f;
            num2 = 100f;
            num3 = 100f;
            this.maxAddPow = (int)Math.Ceiling(((((((this.GunLevel - 1) * num) * numArray[1]) * this.info.ratioPow) * this.info.eatRatio) / num2) / num3);
            this.pow = this.basePow + this.Pow;
            num = 45f;
            num2 = 100f;
            this.baseRate = (int)Math.Ceiling(((num * numArray[2]) * this.info.ratioRate) / num2);
            num = 0.181f;
            num2 = 100f;
            num3 = 100f;
            this.maxAddRate = (int)Math.Ceiling(((((((this.GunLevel - 1) * num) * numArray[2]) * this.info.ratioRate) * this.info.eatRatio) / num2) / num3);
            this.rate = this.baseRate + this.Rate;
            num = 5f;
            num2 = 100f;
            this.baseHit = (int)Math.Ceiling(((num * numArray[4]) * this.info.ratioHit) / num2);
            num = 0.303f;
            num2 = 100f;
            num3 = 100f;
            this.maxAddHit = (int)Math.Ceiling(((((((this.GunLevel - 1) * num) * numArray[4]) * this.info.ratioHit) * this.info.eatRatio) / num2) / num3);
            this.hit = this.baseHit + this.Hit;
            num = 5f;
            num2 = 100f;
            this.baseDodge = (int)Math.Ceiling(((num * numArray[5]) * this.info.ratioDodge) / num2);
            num = 0.303f;
            num2 = 100f;
            num3 = 100f;
            this.maxAddDodge = (int)Math.Ceiling(((((((this.GunLevel - 1) * num) * numArray[5]) * this.info.ratioDodge) * this.info.eatRatio) / num2) / num3);
            this.dodge = this.baseDodge + this.Dodge;
            num = 70f;
            num2 = 100f;
            this.range = (int)Math.Ceiling(((num * numArray[6]) * this.info.ratioRange) / num2);
            num = 10f;
            num2 = 100f;
            this.speed = (int)Math.Ceiling(((num * numArray[3]) * this.info.ratioSpeed) / num2);
            this.radian = 1.570796f;
        }

        public bool isDamaged
        {
            get
            {
                return (this.Life < (this.maxLife * 0.3f));
            }
        }

    }
}
