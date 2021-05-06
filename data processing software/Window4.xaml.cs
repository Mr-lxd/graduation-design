using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace data_processing_software
{
    /// <summary>
    /// Window4.xaml 的交互逻辑
    /// </summary>
    public partial class Window4 : Window
    {
        public double startPoint;
        public double gap;
        public double endPoint;
        public Window4()
        {
            InitializeComponent();

        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            
            string pattern = "^[0-9]*$";
            Regex regex = new Regex(pattern);

            if (textbox1.Text != "" && textbox2.Text!=""&& textbox3.Text!="")//判断是否为空内容
            {
                if (regex.IsMatch(textbox1.Text) && regex.IsMatch(textbox2.Text) && regex.IsMatch(textbox3.Text))//判断是否输入的纯数字
                {

                    startPoint = double.Parse(textbox1.Text);
                    gap = double.Parse(textbox2.Text);
                    endPoint = double.Parse(textbox3.Text);

                    double count = (endPoint - startPoint) / gap;

                    if (MainWindow.array.GetLength(1) != count + 1)//判断力的个数是否与文件的数据匹配
                    {
                        MessageBox.Show("请输入与文件数据匹配的力的范围！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
                        return;//返回输入页面重新输入
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("请输入数字！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }
            else
            {
                MessageBox.Show("请不要输入空内容！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
            }

        }
    }
}
