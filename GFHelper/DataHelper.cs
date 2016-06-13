using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GFHelper.Models;
using Codeplex.Data;
using System.IO;

namespace GFHelper
{
    class DataHelper
    {
        public Dictionary<int, GunInfo> gunInfoDict;

        private InstanceManager im;

       
        public DataHelper(InstanceManager im)
        {
            this.gunInfoDict = new Dictionary<int, GunInfo>();
            this.im = im;

        }

        public void ClearData()
        {
            im.autoOperation.operationList.Clear();

            Data.userInfo.dictionaryDailyStatistics.Clear();
            Data.userInfo.dictionaryWeeklyStatistics.Clear();
            Data.userInfo.listGunCollect.Clear();
            Data.userInfo.gunWithUserID.Clear();
            Data.teamInfo.Clear();
            Data.userInfo.item.Clear();

        }

        public GunWithUserInfo SimpleGetTeamGun(int team, int location)
        {
            try
            {
                return Data.teamInfo[team][location];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool ReadCatchData()
        {

            string catchdatafile = "catchdata";
            string jsondata;
            try
            {
                jsondata = File.ReadAllText(catchdatafile);

                Console.WriteLine(jsondata.Length);
                var jsonobj = DynamicJson.Parse(jsondata); //讲道理，我真不想写了

                //gun_info

                foreach (var gun in jsonobj.gun_info)
                {
                    GunInfo g = new GunInfo();
                    g.id = Convert.ToInt32(gun.id);
                    g.name = gun.name;
                    g.Introduce = gun.introduce;
                    g.code = gun.code;
                    g.type = (GunType)Convert.ToInt32(gun.type);
                    g.rank = Convert.ToInt32(gun.rank);
                    g.maxEquip = Convert.ToInt32(gun.max_equip);
                    g.ratioLife = Convert.ToInt32(gun.ratio_life);
                    g.baseAmmo = Convert.ToInt32(gun.baseammo);
                    g.baseMre = Convert.ToInt32(gun.basemre);
                    g.ammoAddWithnumber = Convert.ToInt32(gun.ammo_add_withnumber);
                    g.mreAddWithNumber = Convert.ToInt32(gun.mre_add_withnumber);
                    g.ratioPow = Convert.ToInt32(gun.ratio_pow);
                    g.ratioHit = Convert.ToInt32(gun.ratio_hit);
                    g.ratioRange = Convert.ToInt32(gun.ratio_range);
                    g.ratioSpeed = Convert.ToInt32(gun.ratio_speed);
                    g.ratioRate = Convert.ToInt32(gun.ratio_rate);
                    g.ratioCrit = Convert.ToInt32(gun.crit);
                    g.retireMp = Convert.ToInt32(gun.retiremp);
                    g.retireAmmo = Convert.ToInt32(gun.retireammo);
                    g.retireMre = Convert.ToInt32(gun.retiremre);
                    g.retirePart = Convert.ToInt32(gun.retirepart);
                    g.eatRatio = Convert.ToInt32(gun.eat_ratio);
                    g.developDuration = Convert.ToInt32(gun.develop_duration);
                    g.dialogue = gun.dialogue;
                    g.effectGridGunType = (GunType)Convert.ToInt32(gun.effect_guntype);
                    {
                        foreach (var item in gun.effect_grid_pos.Split(','))
                        {
                            if (String.IsNullOrEmpty(item)) continue;
                            g.listEffectGrid.Add(Convert.ToInt32(item));
                        }

                        foreach (var item in gun.effect_grid_effect.Split(';'))
                        {
                            if (String.IsNullOrEmpty(item)) continue;
                            var s = item.Split(',');
                            g.dictEffect.Add((EffectType)Convert.ToInt32(s[0]), Convert.ToInt32(s[1]));
                        }
                    }

                    g.skill1 = Convert.ToInt32(gun.skill1);
                    g.skill2 = Convert.ToInt32(gun.skill2);
                    g.special = Convert.ToInt32(gun.special);
                    g.extra = gun.extra;

                    Data.gunInfo.Add(g.id, g);
                }

                //operation_info

                foreach(var item in jsonobj.operation_info)
                {
                    OperationInfo o = new OperationInfo();
                    o.id = Convert.ToInt32(item.id);
                    o.campaign = Convert.ToInt32(item.campaign);
                    o.name = item.name;
                    o.description = item.description;
                    o.duration = Convert.ToInt32(item.duration);
                    o.mp = Convert.ToInt32(item.mp);
                    o.ammo = Convert.ToInt32(item.ammo);
                    o.mre = Convert.ToInt32(item.mre);
                    o.part = Convert.ToInt32(item.part);

                    {
                        foreach(var i in item.item_pool.Split(','))
                        {
                            if (String.IsNullOrEmpty(i)) continue;
                            o.itemPool.Add(Convert.ToInt32(i));
                        }
                    }

                    o.teamLeaderMinLevel = Convert.ToInt32(item.team_leader_min_level);
                    o.gunMin = Convert.ToInt32(item.gun_min);

                    {
                        o.gunTypeMin[0] = Convert.ToInt32(item.guntype1_min);
                        o.gunTypeMin[1] = Convert.ToInt32(item.guntype2_min);
                        o.gunTypeMin[2] = Convert.ToInt32(item.guntype3_min);
                        o.gunTypeMin[3] = Convert.ToInt32(item.guntype4_min);
                        o.gunTypeMin[4] = Convert.ToInt32(item.guntype5_min);
                        o.gunTypeMin[5] = Convert.ToInt32(item.guntype6_min);
                        o.gunTypeMin[6] = Convert.ToInt32(item.guntype7_min);
                    }

                    Data.operationInfo.Add(o.id, o);

                }


            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return true;
        }



        public bool ReadUserInfo(string jsondata)
        {
            try
            {
                ClearData();

                dynamic jsonobj = DynamicJson.Parse(jsondata);
                dynamic userdata = jsonobj.user_info;

                UserInfo userInfo = Data.userInfo;

                //user_info
                userInfo.userId = userdata.user_id;
                userInfo.name = userdata.name;
                userInfo.pauseTurnChance = Convert.ToInt32(userdata.pause_turn_chance);
                userInfo.gem = Convert.ToInt32(userdata.gem);
                userInfo.Exp = Convert.ToInt32(userdata.experience);
                userInfo.maxTeam = Convert.ToInt32(userdata.maxteam);
                userInfo.mp = Convert.ToInt32(userdata.mp);
                userInfo.ammo = Convert.ToInt32(userdata.ammo);
                userInfo.mre = Convert.ToInt32(userdata.mre);
                userInfo.part = Convert.ToInt32(userdata.part);
                userInfo.core = Convert.ToInt32(userdata.core);
                userInfo.maxGun = Convert.ToInt32(userdata.maxgun);
                userInfo.maxEquip = Convert.ToInt32(userdata.maxequip);
                userInfo.maxDevelopSlot = Convert.ToInt32(userdata.max_build_slot) / 2;
                userInfo.maxFixSlot = Convert.ToInt32(userdata.max_fix_slot);
                userInfo.maxUpgradeSlot = Convert.ToInt32(userdata.max_upgrade_slot);
                userInfo.coin1 = Convert.ToInt32(userdata.coin1);
                userInfo.coin2 = Convert.ToInt32(userdata.coin2);
                userInfo.coin3 = Convert.ToInt32(userdata.coin3);
                userInfo.bp = Convert.ToInt32(userdata.bp);
                userInfo.lastBpRecoverTime = Convert.ToInt32(userdata.last_bp_recover_time);
                userInfo.monthlyCardExpirationGem = Convert.ToInt32(userdata.monthlycard1_end_time);
                userInfo.monthlyCardExpirationRes = Convert.ToInt32(userdata.monthlycard2_end_time);

                //gun_collect
                string guncollect = userdata.gun_collect;
                if (guncollect[0] != ',')
                {
                    foreach (string id in guncollect.Split(','))
                    {
                        userInfo.listGunCollect.Add(Convert.ToInt32(id));
                    }

                }

                //gun_with_user_info
                foreach(var gun in jsonobj.gun_with_user_info)
                {
                    GunWithUserInfo g = new GunWithUserInfo();
                    g.Ammo = Convert.ToInt32(gun.ammo);
                    g.Dodge = Convert.ToInt32(gun.dodge);
                    g.FixEndTime = Convert.ToInt32(gun.fix_end_time);
                    g.GunExp = Convert.ToInt32(gun.gun_exp);
                    g.GunID = Convert.ToInt32(gun.gun_id);
                    g.GunLevel = Convert.ToInt32(gun.gun_level);
                    g.Hit = Convert.ToInt32(gun.hit);
                    g.ID = Convert.ToInt32(gun.id);
                    g.IfModification = Convert.ToBoolean(Convert.ToInt32(gun.if_modification));
                    g.IsLocked = Convert.ToBoolean(Convert.ToInt32(gun.is_locked));
                    g.Life = Convert.ToInt32(gun.life);
                    g.Location = Convert.ToInt32(gun.location);
                    g.Mre = Convert.ToInt32(gun.mre);
                    g.Number = Convert.ToInt32(gun.number);
                    g.Position = Convert.ToInt32(gun.position);
                    g.Pow = Convert.ToInt32(gun.pow);
                    g.Rate = Convert.ToInt32(gun.rate);
                    g.Skill1 = Convert.ToInt32(gun.skill1);
                    g.Skill2 = Convert.ToInt32(gun.skill2);
                    g.TeamID = Convert.ToInt32(gun.team_id);
                    g.UserID = Convert.ToInt32(gun.user_id);

                    g.UpdateData();

                    userInfo.gunWithUserID.Add(g);
                    if(g.TeamID != 0)
                    {
                        if (!Data.teamInfo.ContainsKey(g.TeamID))
                        {
                            Data.teamInfo.Add(g.TeamID, new Dictionary<int, GunWithUserInfo>());
                        }

                        Data.teamInfo[g.TeamID].Add(g.Location, g);
                    }

                }

                
                //item_with_user_info
                foreach(var item in jsonobj.item_with_user_info)
                {
                    int id = Convert.ToInt32(item.item_id);
                    int num = Convert.ToInt32(item.number);

                    userInfo.item.Add(id, num);
                }

                //develop_act_info
                foreach(var item in jsonobj.develop_act_info)
                {
                    im.uiHelper.setDevelopingTimer(im.timer, Convert.ToInt32(item.build_slot), Convert.ToInt32(item.gun_id), Convert.ToInt32(item.start_time) - SimpleUserInfo.timeoffset);
                }

                //operation_act_info
                foreach (var item in jsonobj.operation_act_info)
                {
                    im.mainWindow.Dispatcher.Invoke(() =>
                    {
                        AutoOperationInfo ao = new AutoOperationInfo(Convert.ToInt32(item.team_id), Convert.ToInt32(item.operation_id));
                        ao.LastTime = Convert.ToInt32(item.start_time) + ao.LastTime - CommonHelper.ConvertDateTimeInt(DateTime.Now, true);
                        im.autoOperation.AddTimerStartOperation(ao);
                    });

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;

        }
    }
}
