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
using System.Windows.Shapes;

namespace data_processing_software
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            help.Text = "1.在进行任意数据处理之前请先打开相关文件"+"\n"+"2.为了确保处理的数据无误，建议在打开文件之后显示数据进行检查"+ "\n" + "3.输出图像之前请先拟合" + "\n" + "4.保存数据或图像之前请确保已有处理结果";
        }


    }
}
