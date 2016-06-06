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

                //讲道理，挺危险
                Task.Run(() =>
                {
                    im.dataHelper.ReadCatchData();
                    im.autoOperation.SetOperationInfo();
                    im.autoOperation.StartRefresh();

                    Console.WriteLine("ok");
                });

                im.listener.startProxy(8888);

                
            }
            catch(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
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
