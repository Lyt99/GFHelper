using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Codeplex.Data;
using System.Windows.Controls;
using GFHelper.Models;
using System.Windows;
using System.Windows.Media;

namespace GFHelper
{
    class UIHelper
    {
        //有关界面的Grid边界线的绘制
        private static SolidColorBrush _BorderBrush = new SolidColorBrush(Colors.DarkGray);

        //有关界面的Grid边界线的绘制
        public static SolidColorBrush BorderBrush {
            get { return UIHelper._BorderBrush; }
            set { UIHelper._BorderBrush = value; }
        }

        //有关界面的Grid边界线的绘制
        private static double _BorderThickness = 1;

        //有关界面的Grid边界线的绘制
        public static double BorderThickness {
            get { return UIHelper._BorderThickness; }
            set { UIHelper._BorderThickness = value; }
        }

        //有关界面的Grid边界线的绘制
        public static bool GetShowBorder(DependencyObject obj) {
            return (bool)obj.GetValue(ShowBorderProperty);
        }

        //有关界面的Grid边界线的绘制
        public static void SetShowBorder(DependencyObject obj, bool value) {
            obj.SetValue(ShowBorderProperty, value);
        }

        //有关界面的Grid边界线的绘制
        public static readonly DependencyProperty ShowBorderProperty =
        DependencyProperty.RegisterAttached("ShowBorder", typeof(bool), typeof(UIHelper), new PropertyMetadata(OnShowBorderChanged));

        //有关界面的Grid边界线的绘制
        private static void OnShowBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var grid = d as Grid;
            if((bool)e.OldValue) {
                grid.Loaded -= (s, arg) => { };
            }

            if((bool)e.NewValue) {
                grid.Loaded += (s, arg) => {
                    var rows = grid.RowDefinitions.Count;
                    var columns = grid.ColumnDefinitions.Count;

                    var controls = grid.Children;
                    var count = controls.Count;


                    for(int i = 0; i < count; i++) {
                        var item = controls[i] as FrameworkElement;
                        Border border = new Border();
                        border.BorderBrush = BorderBrush;
                        border.BorderThickness = new Thickness(0, 0, BorderThickness, BorderThickness);

                        var row = Grid.GetRow(item);
                        var column = Grid.GetColumn(item);
                        var rowspan = Grid.GetRowSpan(item);
                        var columnspan = Grid.GetColumnSpan(item);

                        Grid.SetRow(border, row);
                        Grid.SetColumn(border, column);
                        Grid.SetRowSpan(border, rowspan);
                        Grid.SetColumnSpan(border, columnspan);

                        grid.Children.Add(border);
                    }


                    //画最外面的边框
                    Border bo = new Border();
                    bo.BorderBrush = BorderBrush;
                    bo.BorderThickness = new Thickness(BorderThickness, BorderThickness, 0, 0);
                    bo.SetValue(Grid.ColumnProperty, 0);
                    bo.SetValue(Grid.RowProperty, 0);

                    bo.SetValue(Grid.ColumnSpanProperty, grid.ColumnDefinitions.Count);
                    bo.SetValue(Grid.RowSpanProperty, grid.RowDefinitions.Count);

                    bo.Tag = "autoBorder";
                    grid.Children.Add(bo);
                };
            }
        }

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

        public void SetDevelopingTextBlock(int slot, out TextBlock textname, out TextBlock texttime, bool isEquip = false)
        {
            if (!isEquip)
            {
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
            }
            else
            {
                switch (slot)
                {
                    case 1:
                        textname = im.mainWindow.textFactoryEquipName1;
                        texttime = im.mainWindow.textFactoryEquipTime1;
                        break;
                    case 2:
                        textname = im.mainWindow.textFactoryEquipName2;
                        texttime = im.mainWindow.textFactoryEquipTime2;
                        break;
                    case 3:
                        textname = im.mainWindow.textFactoryEquipName3;
                        texttime = im.mainWindow.textFactoryEquipTime3;
                        break;
                    case 4:
                        textname = im.mainWindow.textFactoryEquipName4;
                        texttime = im.mainWindow.textFactoryEquipTime4;
                        break;
                    case 5:
                        textname = im.mainWindow.textFactoryEquipName5;
                        texttime = im.mainWindow.textFactoryEquipTime5;
                        break;
                    case 6:
                        textname = im.mainWindow.textFactoryEquipName6;
                        texttime = im.mainWindow.textFactoryEquipTime6;
                        break;
                    case 7:
                        textname = im.mainWindow.textFactoryEquipName7;
                        texttime = im.mainWindow.textFactoryEquipTime7;
                        break;
                    case 8:
                        textname = im.mainWindow.textFactoryEquipName8;
                        texttime = im.mainWindow.textFactoryEquipTime8;
                        break;

                    default:
                        textname = im.mainWindow.textFactoryEquipName1;
                        texttime = im.mainWindow.textFactoryEquipTime1;
                        break;
                }
            }
        }

        public void setDevelopingTimer(Timer timer, int slot, int id, int starttime, bool isEquip = false)
        {
            TextBlock texttime;
            TextBlock textname;
        
            slot = (slot + 1) / 2;
            this.SetDevelopingTextBlock(slot, out textname, out texttime, isEquip);

            string name;
            int duration;
            if (!isEquip)
            {
                name = Data.gunInfo[id].name;
                duration = Data.gunInfo[id].developDuration;
            }
            else
            {
                name = Data.equipInfo[id].name;
                duration = Data.equipInfo[id].developDuration;
            }
            this.setTextBlockText(textname, name);
            
            timer.AddTimerText(texttime, starttime, duration);
            timer.Start();
        }

        public void setFactoryTimerDefault(int slot, bool isEquip = false)
        {
            TextBlock textname, texttime;
            slot = (slot + 1) / 2;

            SetDevelopingTextBlock(slot, out textname, out texttime, isEquip);

            this.setTextBlockText(textname, "未使用");
            im.logger.Log(im.timer.GetType());
            im.timer.DeleteTimerWithTextBlock(texttime);
            this.setTextBlockText(texttime, CommonHelper.formatDuration(0));
        }

        public void setUserInfo()
        {
            try {
                UserInfo userInfo = Data.userInfo;
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
                im.logger.Log(e);
                throw;
            }
        }

    }
}
