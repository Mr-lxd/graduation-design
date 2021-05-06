using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Input;

namespace data_processing_software
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class Window3 : Window
    {
        bool flag_havedata = true;


        //注意 与winform不同，在WPF的textbox无法实现多行文本输入，必须在xaml添加  TextWrapping="Wrap"和 AcceptsReturn="True" 在按回车键时插入新行，必要时会再次自动扩展 TextBox 以便为新行留出空间。
        public Window3()
        {
            InitializeComponent();

            textBox_Data.Text = "注意：此功能需联网\r\n输入格式如下：\r\n[标题]软件功能改进意见\r\n[内容]体验感不好\r\n请将上述内容删除后按格式反馈！";               //显示提示信息
            //字体颜色设为浅灰色
            textBox_Data.Foreground = System.Windows.Media.Brushes.Gray;
            //textBox_Data1.Text = "联系邮箱：XXXX@163.com";
            //textBox_Data1.Foreground = System.Windows.Media.Brushes.Gray;
        }


        //private void textBox_Data_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (flag_havedata == false)
        //    {

        //        textBox_Data.Text = " ";    //非空字符串
        //        textBox_Data.Foreground = System.Windows.Media.Brushes.Black;

        //        flag_havedata = true;
        //    }
        //}


        //限制发送空内容
        private void textBox_Data_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (flag_havedata)
            {
                String str = textBox_Data.Text.ToString();  /* 获取文本框里的数据 */
                if (str != "")
                {
                    button_Send.IsEnabled = true;
                }
                else
                {
                    button_Send.IsEnabled = false; //空内容时 button不可按
                }
            }
            else
            {
                /* no code */
            }
        }

        private void button_Send_Click(object sender, RoutedEventArgs e)
        {
            String str = textBox_Data.Text.ToString();  /* 获取文本框里的数据 */

            int str_index = str.IndexOf("\r\n"); //获取“回车键”在str中的索引值（位置）
            String str_title = str.Substring(0, str_index);//截取从0开始，长度为str_index的字符串
            String str_info = str.Substring(str_index + 2);//截取从str_index+2以后的所有字符串

            int str_title_check = str_title.IndexOf("[标题]");
            int str_info_check = str_info.IndexOf("[内容]");
            if (str_title_check == -1 || str_info_check == -1)
            {
                MessageBox.Show("检测到您输入的格式不正确，请重新输入", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information,MessageBoxResult.OK);
            }
            else
            {
                try
                {
                    String text_title = "text=" + str_title;
                    String text_info = "desp=" + str_info;
                    // Prepare web request
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://sc.ftqq.com/SCU171890T8e220f35ae2d3ada1e247e8c0906cf2b608c2f0bdc217.send?" + text_title + "&" + text_info);
                    myRequest.Method = "GET";
                    myRequest.ContentType = "text/html";
                    // Get response
                    HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                    Stream myResponseStream = myResponse.GetResponseStream();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);//提交的数据中含有中文 用UTF8对其编码
                    string retString = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();

                    MessageBox.Show("您已成功向作者吐槽", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
                    button_Send.IsEnabled = false;    //未修改不能再次发送
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发生异常,请查看网络连接", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }

        }



        //private void textBox_Data_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (flag_havedata == false)
        //    {
        //        button_Send.IsEnabled = false;

        //        textBox_Data.Text = "注意：此功能需联网\r\n输入格式如下：\r\n[标题]上位机功能改进意见\r\n[内容]体验感不好,\r\n[内容]没有进度条提示进度，不知何时可关闭上位机。";               //显示提示信息
        //        textBox_Data.Foreground = System.Windows.Media.Brushes.Gray;               //字体颜色设为浅灰色
        //    }
        //    else
        //    {
        //        String str = textBox_Data.Text.ToString();  /* 获取文本框里的数据 */
        //        if (str != "")
        //        {
        //            button_Send.IsEnabled = true;
        //        }
        //        else
        //        {
        //            button_Send.IsEnabled = false;

        //            flag_havedata = false;
        //            textBox_Data.Text = "注意：此功能需联网\r\n输入格式如下：\r\n[标题]上位机功能改进意见\r\n[内容]体验感不好,\r\n[内容]没有进度条提示进度，不知何时可关闭上位机。";               //显示提示信息
        //            textBox_Data.Foreground = System.Windows.Media.Brushes.Gray;                //字体颜色设为浅灰色
        //        }
        //    }
        //}




    }
}
