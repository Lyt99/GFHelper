using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFHelper.Models
{
    using System;
    using System.Collections.Generic;

    public class GunInfo : IComparer<GunInfo>
    {
        public int additionAmmo;
        public int additionMre;
        public int baseAmmo;
        public int baseMre;
        public string code;
        public int developDuration;
        public string dialogue;
        public float eatRatio;
        public GunType effectGridGunType;
        public string Introduce;
        public string extra;
        public int id;
        public List<int> listEffectGrid = new List<int>();
        public Dictionary<EffectType, int> dictEffect = new Dictionary<EffectType, int>();
        public int maxEquip;
        public string name;
        public int rank;
        public int ratioCrit;
        public int ratioDodge;
        public int ratioHit;
        public float ratioLife;
        public int ratioPow;
        public int ratioRange;
        public int ratioRate;
        public int ratioSpeed;
        public int retireAmmo;
        public int retireMp;
        public int retireMre;
        public int retirePart;
        public int skill1;
        public int skill2;
        public int special;
        public int ammoAddWithnumber;
        public int mreAddWithNumber;
        public string[] subtitles;
        public GunType type;

        public int Compare(GunInfo x, GunInfo y)
        {
            return (x.id - y.id);
        }

    }


}
