using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace GFHelper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private InstanceManager im;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                this.im = new InstanceManager(this);
                //加载配置文件

                if (!this.im.configManager.Load())
                {
                    MessageBox.Show("配置文件加载失败！");
                    Environment.Exit(0);
                }

                Models.SimpleInfo.platform = Models.Platform.Android;

                if (this.im.configManager.getConfigString("platform") == "ios")
                    Models.SimpleInfo.platform = Models.Platform.IOS;

                if (this.im.configManager.getConfigBool("debuglog"))
                    this.im.logger.SetLogState(true);

                if (this.im.configManager.getConfigBool("buildlog"))
                    this.im.logger.SetBuildLogState(true);

                if (this.im.configManager.getConfigBool("autoopt"))
                    this.TabItemOperation.Visibility = Visibility.Visible;

                im.logger.Log("GFHelper启动");

                //讲道理，挺危险
                Task.Run(() =>
                {
                    if (im.dataHelper.ReadCatchData())
                    {
                        im.autoOperation.SetOperationInfo();
                        im.autoOperation.StartRefresh();
                    }
                    else
                    {
                        im.uiHelper.setStatusBarText_InThread("catchdata读取失败！请检查相关文件！");
                        im.listener.Shutdown();
                    }
                    

                    Console.WriteLine("ok");
                });

                int port = im.configManager.getConfigInt("port");
                if (port <= 0) port = 8888;
                im.listener.startProxy(port);

                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show("GFHelper启动失败！错误原因: " + e.ToString());
            }

            
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            im.listener.Shutdown();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        { 
            if(comboBoxOperationTeam.SelectedIndex == -1 || comboBoxOperation.SelectedIndex == -1)
            {
                MessageBox.Show("添加失败！请检查你的选择！");
                //return;
            }
            Console.WriteLine("{0} {1}", comboBoxOperationTeam.SelectedIndex, comboBoxOperation.SelectedIndex);
            im.autoOperation.Start(comboBoxOperationTeam.SelectedIndex + 1, comboBoxOperation.SelectedIndex + 1);
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(checkBoxAutoOperation.IsChecked);
            im.autoOperation.autoOperation = (bool)checkBoxAutoOperation.IsChecked;
        }
    }
}
