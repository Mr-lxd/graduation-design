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

namespace data_processing_software
{
    /// <summary>
    /// Page5.xaml 的交互逻辑
    /// </summary>
    public partial class Page5 : Page
    {
        public static string text5;
        private double a = MainWindow.k;
        private double b = MainWindow.b;
        private string slope = "斜率：";
        private string offset = "截距：";
        public Page5()
        {
            InitializeComponent();

            TextRange text = new TextRange(RichTextBox5.Document.ContentStart, RichTextBox5.Document.ContentEnd);//使用richTextBox的doucument属性
            text.Text = "\n" + slope + a.ToString() + "\n" + offset + b.ToString() + "\n" + "拟合直线方程：" + "y=" + a + "x" + b;
            text5 = text.Text + "\n";
        }
    }
}
