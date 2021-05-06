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
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page3 : Page
    {
        private double[,] array3 = MainWindow.array;
        public Page3()
        {
            InitializeComponent();

            TextRange text = new TextRange(RichTextBox3.Document.ContentStart, RichTextBox3.Document.ContentEnd);//使用richTextBox的doucument属性

            //对读入的数据可在xaml设置字体属性
            for (int i = 0; i < array3.GetLength(0); i++)
            {
                for (int j = 0; j < array3.GetLength(1); j++)
                {
                    text.Text += array3[i, j].ToString() + " " + "\\" + " ";
                }
                text.Text += "\n";//换行处理
            }

            int isize = 20;
            RichTextBox3.Document.Blocks.Add(new Paragraph(new Run("显示成功!") { Foreground = Brushes.Blue,FontSize=isize}));
            RichTextBox3.Selection.Text = "读取成功！";
            RichTextBox3.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
            RichTextBox3.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Medium);
            RichTextBox3.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, isize.ToString());//注意这里文字大小是string类型
            
        }

        public void showRbutton_Click(object sender, RoutedEventArgs e)
        {
            TextRange text = new TextRange(RichTextBox3.Document.ContentStart, RichTextBox3.Document.ContentEnd);

            text.Text += "求得重复度为：   " + MainWindow.r;
        }
    }
}
