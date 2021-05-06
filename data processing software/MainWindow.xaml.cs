using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
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
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using NSMatrix;
using System.Drawing;


namespace data_processing_software

{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Page1 p1;
        Page2 p2;
        Page3 p3;
        Page5 p5;
        Page6 p6;
        Window4 Win4;
        bool Done=false;//该布尔值用于拟合中的判断  拟合完成后置为true 切换到其他界面再返回时就不会弹出输入力的对话框
        public static double[,] array;
        public static double[] XAxis;//X轴的力值
        public static double[,] yu;//正行程数组
        public static double[,] yd;//负行程数组
        public static double[] Hys;//迟滞
        public static double r;//重复度
        public static double b;//截距
        public static double k;//斜率
        bool IsClick = false;//图像显示的判断条件 在线性拟合中true
        bool IsClick2 = false;//保存图片的判断条件 在图像显示中true

        public MainWindow()
        {
            InitializeComponent();
        }


        private void button8_Click(object sender, RoutedEventArgs e)//反馈窗口
        {
            Window3 Win = new Window3();
            Win.ShowDialog();       
        }

        private void button2_Click(object sender, RoutedEventArgs e)//关于窗口
        {

            Window2 Win = new Window2();
            Win.ShowDialog();
        }

        private void button3_Click(object sender, RoutedEventArgs e)//帮助窗口
        {

               Window1 Win = new Window1();
                Win.ShowDialog();//showdialog限制窗口重复打开

        }

        private void button1_Click(object sender, RoutedEventArgs e)//图像显示
        {
           
            if ( IsClick== false)
            {
                MessageBox.Show("请进行线性拟合！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                IsClick2 = true;
                if (p1 == null)
                {
                    p1 = new Page1();
                }
                content1.Content = new Frame()
                {
                    Content = null
                };
                content2.Content = new Frame()
                {
                    Content = null
                };
                content3.Content = new Frame()
                {
                    Content = null
                };
                content4.Content = new Frame()
                {
                    Content = p1
                };
            }
           
        }

        private void button5_Click(object sender, RoutedEventArgs e)//打开文件
        {

            int i = 0;
            int row = 0;
            int col=0;

            OpenFileDialog openFileDialog = new OpenFileDialog();//打开文件
            openFileDialog.Title = "选择文件";
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "(*.txt)|*.txt";

            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            DataTable tb = new DataTable();
            DataColumn c = tb.Columns.Add("Value", typeof(double));
            StreamReader sr = new StreamReader(openFileDialog.FileName);
            string line;

            //读取文件的行数和列数
            while ((line = sr.ReadLine()) != null)
            {
                col = 0;
                row++;
                string[] values = line.Split('\t');
                foreach (string s in values)
                {
                    col++;
                }
            }

            array = new double[row, col];

            StreamReader read = new StreamReader(openFileDialog.FileName);//定义一个新变量去读取文件数据
            while ((line = read.ReadLine()) != null)
            {
                // 拆分出一行的所有用空格分割的数据项
                string[] values = line.Split('\t');
                //values为每行数据切割后的数组
                // 将每个数据项转换成浮点数，并存入DataTable
                foreach (string s in values) //s为行内元素
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        // 转换成浮点数
                        double v = double.Parse(s);
                        // 存入DataTable
                        DataRow r = tb.NewRow();
                        r["Value"] = v;
                        tb.Rows.Add(v);
                    }
                }

                int j = 0; // 列数
                           //输出DataTable中保存的数组
                foreach (DataRow r in tb.Rows)
                {
                    var k = (double)r["Value"];//获取行内元素
                    if (!string.IsNullOrEmpty(k.ToString()))
                    {
                        array[i, j] = k;

                    }
                    else
                    {
                        array[i, j] = 1.23456;
                    }


                    j += 1;
                }

                tb.Rows.Clear();
                i += 1;
            }

            yu = new double[array.GetLength(0) / 2, array.GetLength(1)];
            //取出yu
            for (int i1 = 0; i1 < array.GetLength(0) / 2; i1++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    yu[i1, j] = array[i1, j];
                }
            }

            yd = new double[array.GetLength(0) / 2, array.GetLength(1)];
            //取出yd
            for (int i1 = array.GetLength(0)/2; i1 < array.GetLength(0); i1++)//i1从数组中 array.GetLength(0)/2开始
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    yd[i1 - array.GetLength(0) / 2, j] = array[i1, j];//注意yd数组不能直接从i1开始，必须从0开始 即0，0开始标记第一个值
                }
            }

        }

        private void button6_Click(object sender, RoutedEventArgs e)//迟滞计算
        {
            if (array == null)
            {   
                MessageBox.Show("请打开文件！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
               double[,] ydk = new double[yd.GetLength(0), yd.GetLength(1)];

                //反转yd数组得到反转数组ydk
                double temp;//中间暂存变量
                int count = yd.GetLength(1);//取列数
                for (int i = 0; i < yd.GetLength(0); i++)
                {
                    for (int j = 0; j <= count / 2; j++)
                    {
                        temp = yd[i, count - 1 - j];
                        ydk[i, count - 1 - j] = yd[i, j];
                        ydk[i, j] = temp;
                    }
                }


                double[] yuRow = new double[yu.GetLength(1)];
                double[] AbsRow = new double[yu.GetLength(1)];
                Hys = new double[yu.GetLength(0)];
                double[,] Abs = new double[yu.GetLength(0), yu.GetLength(1)];


                for (int i = 0; i < yu.GetLength(0); i++)
                {
                    for (int j = 0; j < yu.GetLength(1); j++)
                    {
                        yuRow[j] = yu[i, j];//将yu每行赋值给yuRow
                        Abs[i, j] = Math.Abs(yu[i, j] - ydk[i, j]);//yu每行与ydk相减并取绝对值  赋值给Abs
                        AbsRow[j] = Abs[i, j];//取出Abs中的每行的值
                    }
                    Hys[i] = AbsRow.Max() / (Math.Abs(yuRow.Max() - Math.Abs(yuRow.Min())));//算出每行迟滞
                }


                if (p6 == null)
                {
                    p6 = new Page6();
                }
                content4.Content = new Frame()//将 content4.Content内容清除
                {
                    Content = null
                };
                content1.Content = new Frame()
                {
                    Content = p6
                };
            }
           
        }

        private void button4_Click(object sender, RoutedEventArgs e)//显示数据
        {
            if (array == null)
            {
                MessageBox.Show("请打开文件！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                if (p3 == null)
                {
                    p3 = new Page3();
                }
                content4.Content = new Frame()
                {
                    Content = p3
                };
            }
           
        }

        private void button7_Click(object sender, RoutedEventArgs e)//线性拟合
        {
           
            if (array == null)
            {
   
                MessageBox.Show("请打开文件！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {

                if(Done==false)//见全局变量Done的注释
                {
                    Win4 = new Window4();
                    Win4.ShowDialog();
                }
               
                IsClick = true;

                Tuple<double, double> s = new Tuple<double, double>(0, 0);
                XAxis = new double[array.GetLength(1)];
                double[] YAxis = new double[array.GetLength(1)];

                //将输入的力的范围内的值取出
                for(int i=0;i<array.GetLength(1);i++)
                {
                    XAxis[i] = Win4.startPoint + i * Win4.gap;
                }

                Random rd = new Random();
                int RandomNum = rd.Next(0, array.GetLength(0)/2);

                for (int i = 0; i < array.GetLength(1); i++)
                {
                    YAxis[i] = array[RandomNum, i];
                }

                s = MathNet.Numerics.Fit.Line(XAxis, YAxis);
                b = s.Item1;
                k = s.Item2;

                Done = true;

                if (p5 == null)
                {
                    p5 = new Page5();
                }
                content4.Content = new Frame()//将 content4.Content内容清除
                {
                    Content = null
                };
                content3.Content = new Frame()
                {
                    Content = p5
                };
            }
           
        }

        private void button11_Click(object sender, RoutedEventArgs e)//贝塞尔求重复性
        {
            if (array == null)
            {
                MessageBox.Show("请打开文件！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                int row = yu.GetLength(0);
                int col = yu.GetLength(1);
                double[,] yudlt = new double[row, col];
                double[,] yudlt2 = new double[row, col];
                double[,] yddlt = new double[row, col];
                double[,] yddlt2 = new double[row, col];
                double[] dlt2RowSum2 = new double[row];
                double[] dlt2RowSum1 = new double[row];
                double[] yuColAvg = new double[col];
                double[] ydColAvg = new double[col];
                double[] arrayRow = new double[array.GetLength(1)];
                double[] maxInput = new double[array.GetLength(0)];
                double Num = 0;
                double yuSi = 0;
                double ydSi = 0;
                double s;


                //求yu每列的平均值
                for (int j = 0; j < col; j++)
                {
                    for (int i = 0; i < row; i++)
                    {
                        Num += yu[i, j];
                    }
                    yuColAvg[j] = Num / row;//如果把这个式子放到第二个for循环外面  那么计算第二个for循环就不用计算这个式子  速度可能会快点
                    Num = 0;//循环后归零 
                }
                //求yd每列平均值
                for (int j = 0; j < col; j++)
                {
                    for (int i = 0; i < row; i++)
                    {
                        Num += yd[i, j];
                        //ydColAvg[j] = Num / row;
                    }
                    ydColAvg[j] = Num / row;
                    Num = 0;
                }

                //yu按行相减去每列均值 再平方 求每行总和 除列数减一 将得到的矩阵求和除以2*row
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        yudlt[i, j] = yu[i, j] - yuColAvg[j];
                        yudlt2[i, j] = yudlt[i, j] * yudlt[i, j];
                        dlt2RowSum1[i] += yudlt2[i, j] / (col - 1);
                    }
                    yuSi += dlt2RowSum1[i] / (2 * row);
                }

                //yd
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        yddlt[i, j] = yd[i, j] - ydColAvg[j];
                        yddlt2[i, j] = yddlt[i, j] * yddlt[i, j];
                        dlt2RowSum2[i] += yddlt2[i, j] / (col - 1);
                    }
                    ydSi += dlt2RowSum2[i] / (2 * row);
                }

                s = Math.Max(Math.Sqrt(yuSi), Math.Sqrt(ydSi));//按照贝塞尔公式求得的值

                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        arrayRow[j] = array[i, j];//将array每行赋值给arrayRow

                    }

                    maxInput[i] = Math.Abs(arrayRow.Max() - Math.Abs(arrayRow.Min()));//满量程输出
                }

                r = 3 * s / maxInput.Max();//重复度指标

                if (p2 == null)
                {
                    p2 = new Page2();

                }
                content4.Content = new Frame()//将 content4.Content内容清除
                {
                    Content = null
                };
                content2.Content = new Frame()
                {
                    Content = p2
                };
            }

        }

        private void button9_Click(object sender, RoutedEventArgs e)//数据保存
        {
            if (Page5.text5 == null && Page6.text6== null && Page2.text2==null)
            {
                MessageBox.Show("请先进行数据处理！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                // 利用SaveFileDialog，让用户指定文件的路径名
                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.Filter = "(*.txt)|*.txt|(*.xls)|*.xls|(*.doc)|*.doc";
                if (saveDlg.ShowDialog() == true)
                {
                    // 创建文件，将处理得到的数据保存到文件中
                    // saveDlg.FileName 是用户指定的文件路径
                    FileStream fs = File.Open(saveDlg.FileName, FileMode.Create, FileAccess.Write);

                    // 保存所有内容
                    StreamWriter sw = new StreamWriter(fs);

                    if(Page5.text5 != null)
                    {
                        sw.WriteLine(Page5.text5);
                    }
                    if (Page6.text6 != null)
                    {
                        sw.WriteLine(Page6.text6);
                    }
                    if (Page2.text2 != null)
                    {
                        sw.WriteLine(Page2.text2);
                    }


                    //关闭文件
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    // 提示用户：文件保存的位置和文件名
                    MessageBox.Show("文件已成功保存到" + saveDlg.FileName);
                }
            }
            
        }

        private void button10_Click(object sender, RoutedEventArgs e)//保存图片
        {
            if (IsClick2 == false)
            {
                MessageBox.Show("请进行图像显示！", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                //将参数传递给page1.SavePicButton_Click，触发该事件
                p1.SavePicButton_Click(sender, e);
            }
          

        }


    }


}
