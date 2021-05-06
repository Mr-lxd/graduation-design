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
    /// Page6.xaml 的交互逻辑
    /// </summary>
    public partial class Page6 : Page
    {
        public static string text6;
        private double[] array1 = MainWindow.Hys;


        public Page6()
        {
            InitializeComponent();

            TextRange text = new TextRange(RichTextBox6.Document.ContentStart, RichTextBox6.Document.ContentEnd);//使用richTextBox的doucument属性

            text.Text = "\n" + "系统迟滞：";
            for (int j = 0; j < array1.GetLength(0); j++)
            {
                text.Text += "\n" + "Hys" + "[" + j + "]" + ":" + array1[j].ToString() + "\n";
                text6 = text.Text + "\n";//保存数据
            }

        }

    }
}
