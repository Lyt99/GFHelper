using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFHelper.Models
{
    using System;
    using System.Collections.Generic;

    public class UserInfo
    {
        public int bp;
        public int ammo;
        public int autoBattle;
        public int coin1;
        public int coin2;
        public int coin3;
        public int contract;
        public int contract16;
        public int core;
        public Dictionary<string, int> dictionaryDailyStatistics = new Dictionary<string, int>();
        public Dictionary<string, int> dictionaryWeeklyStatistics = new Dictionary<string, int>();
        private int exp;
        public int gem;
        public int lastBpRecoverTime;
        public int level;
        public List<int> listGunCollect = new List<int>();
        public int maxDevelopSlot;
        public int maxEquip;
        public int maxFixSlot;
        public int maxGun;
        public int maxResearchSlot = 2;
        public int maxTeam;
        public int monthlyCardExpirationGem;
        public int monthlyCardExpirationRes;
        public int mp;
        public int mre;
        public string name;
        public int part;
        public int pauseTurnChance;
        public int quickDevelop;
        public int quickReinforce;
        public int quickSkillTraining;
        public int ring;
        public string userId;
        public int maxUpgradeSlot;

        public List<GunWithUserInfo> gunWithUserID = new List<GunWithUserInfo>();
        public Dictionary<int, int> item = new Dictionary<int, int>();

        public int Exp
        {
            get
            {
                return this.exp;
            }
            set
            {
                this.exp = value;
                int level = this.level;
                this.level = CommonHelper.ExpToLevel(this.exp, true);
            }
        }
    }

}
