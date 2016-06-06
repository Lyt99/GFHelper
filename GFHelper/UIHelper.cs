using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Codeplex.Data;
using System.Windows.Controls;
using GFHelper.Models;

namespace GFHelper
{
    class UIHelper
    {

        private InstanceManager im;

        public UIHelper(InstanceManager im)
        {
            this.im = im;
        }

        public void addComboBoxText(ComboBox cb, string text)
        {
            im.mainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                cb.Items.Add(text);
            }));

        }

        public void setStatusBarText(string text)
        {
            TextBlock tb = new TextBlock();
            tb.Text = ' ' + text;

            im.mainWindow.statusBar.Items.Clear();
            im.mainWindow.statusBar.Items.Add(tb);
        }

        public void setStatusBarText_InThread(string text)
        {
            im.mainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.setStatusBarText(text);
            }));

        }

        public void setTextBlockText(TextBlock tb, string text)
        {
            im.mainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                tb.Text = text;

            }));
        }

        public void refreshListOperation()
        {
            im.mainWindow.Dispatcher.BeginInvoke(new Action(() =>
            {
                im.mainWindow.listViewOperation.Items.Refresh();
            }));
        }


        public void setDevelopingTimer(Timer timer, int slot, int gun_id, int starttime)
        {
            TextBlock texttime;
            TextBlock textname;
            slot = (slot + 1) / 2;
            switch (slot)
            {
                case 1:
                    textname = im.mainWindow.textFactoryGunName1;
                    texttime = im.mainWindow.textFactoryGunTime1;
                    break;
                case 2:
                    textname = im.mainWindow.textFactoryGunName2;
                    texttime = im.mainWindow.textFactoryGunTime2;
                    break;
                case 3:
                    textname = im.mainWindow.textFactoryGunName3;
                    texttime = im.mainWindow.textFactoryGunTime3;
                    break;
                case 4:
                    textname = im.mainWindow.textFactoryGunName4;
                    texttime = im.mainWindow.textFactoryGunTime4;
                    break;
                case 5:
                    textname = im.mainWindow.textFactoryGunName5;
                    texttime = im.mainWindow.textFactoryGunTime5;
                    break;
                case 6:
                    textname = im.mainWindow.textFactoryGunName6;
                    texttime = im.mainWindow.textFactoryGunTime6;
                    break;
                case 7:
                    textname = im.mainWindow.textFactoryGunName7;
                    texttime = im.mainWindow.textFactoryGunTime7;
                    break;
                case 8:
                    textname = im.mainWindow.textFactoryGunName8;
                    texttime = im.mainWindow.textFactoryGunTime8;
                    break;

                default:
                    textname = im.mainWindow.textFactoryGunName1;
                    texttime = im.mainWindow.textFactoryGunTime1;
                    break;
            }
            string name = Data.gunInfo[gun_id].name;
            int duration = Data.gunInfo[gun_id].developDuration;
            this.setTextBlockText(textname, name);
            
            timer.AddTimerText(texttime, starttime, duration);
            timer.Start();
        }

        public void setFactoryTimerDefault(int slot)
        {
            TextBlock textname, texttime;
            slot = (slot + 1) / 2;
            switch (slot)
            {
                case 1:
                    textname = im.mainWindow.textFactoryGunName1;
                    texttime = im.mainWindow.textFactoryGunTime1;
                    break;
                case 2:
                    textname = im.mainWindow.textFactoryGunName2;
                    texttime = im.mainWindow.textFactoryGunTime2;
                    break;
                case 3:
                    textname = im.mainWindow.textFactoryGunName3;
                    texttime = im.mainWindow.textFactoryGunTime3;
                    break;
                case 4:
                    textname = im.mainWindow.textFactoryGunName4;
                    texttime = im.mainWindow.textFactoryGunTime4;
                    break;
                case 5:
                    textname = im.mainWindow.textFactoryGunName5;
                    texttime = im.mainWindow.textFactoryGunTime5;
                    break;
                case 6:
                    textname = im.mainWindow.textFactoryGunName6;
                    texttime = im.mainWindow.textFactoryGunTime6;
                    break;
                case 7:
                    textname = im.mainWindow.textFactoryGunName7;
                    texttime = im.mainWindow.textFactoryGunTime7;
                    break;
                case 8:
                    textname = im.mainWindow.textFactoryGunName8;
                    texttime = im.mainWindow.textFactoryGunTime8;
                    break;

                default:
                    textname = im.mainWindow.textFactoryGunName1;
                    texttime = im.mainWindow.textFactoryGunTime1;
                    break;
            }

            
            this.setTextBlockText(textname, "未使用");
            Console.WriteLine(im.timer.GetType());
            im.timer.DeleteTimerWithTextBlock(texttime);
            this.setTextBlockText(texttime, CommonHelper.formatDuration(0));
        }
        public void setUserInfo(UserInfo userInfo)
        {
            try {

                im.mainWindow.Dispatcher.BeginInvoke(new Action(() => {
                    //GridUserInfo
                    im.mainWindow.UserName.Text = userInfo.name;
                    im.mainWindow.UserLevel.Text = "Lv." + userInfo.level;

                    //GridResources
                    im.mainWindow.textammo.Text = userInfo.ammo.ToString();
                    im.mainWindow.textmre.Text = userInfo.mre.ToString();
                    im.mainWindow.textmp.Text = userInfo.mp.ToString();
                    im.mainWindow.textpart.Text = userInfo.part.ToString();

                    //items
                    //讲道理，难看
                    try
                    {
                        im.mainWindow.textitem1.Text = Convert.ToString(userInfo.item[1]);
                    }
                    catch (Exception) { im.mainWindow.textitem1.Text = "0"; }

                    try
                    {
                        im.mainWindow.textitem3.Text = Convert.ToString(userInfo.item[3]);
                    }
                    catch (Exception) { im.mainWindow.textitem3.Text = "0"; }

                    try
                    {
                        im.mainWindow.textitem4.Text = Convert.ToString(userInfo.item[4]);
                    }
                    catch (Exception) { im.mainWindow.textitem4.Text = "0"; }
                    try
                    {
                        im.mainWindow.textitem8.Text = Convert.ToString(userInfo.item[8]);
                    }
                    catch (Exception) { im.mainWindow.textitem8.Text = "0"; }



                    im.mainWindow.textDevelopSlot.Text = userInfo.maxDevelopSlot.ToString();
                    im.mainWindow.textFixSlot.Text = userInfo.maxFixSlot.ToString();
                    im.mainWindow.textUpgradeSlot.Text = userInfo.maxUpgradeSlot.ToString();

                    im.mainWindow.textGunNum.Text = String.Format("{0}/{1}", userInfo.gunWithUserID.Count, userInfo.maxGun);
                    im.mainWindow.textTeamNum.Text = userInfo.maxTeam.ToString();
                    im.mainWindow.textUnlockRatio.Text = ((int)((double)userInfo.listGunCollect.Count / (double)Data.gunInfo.Count * 100)).ToString() + "%";

                }));
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
