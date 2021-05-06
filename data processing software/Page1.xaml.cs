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
using System.Data;
using System.Data.OleDb;
using Microsoft.Win32;
using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.IO;

namespace data_processing_software
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {

        //设置全局变量 在后台代码取值   方便xaml页面调用后台取到的值
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }



        private double k = MainWindow.k;
        private double b = MainWindow.b;

        double[] y = new double[MainWindow.array.GetLength(1)];
        public Page1()
        {
            InitializeComponent();

            for (int i = 0; i <MainWindow.array.GetLength(1); i++)
            {

                y[i] = k * MainWindow.XAxis[i] + b;

            }


            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "yu[0]",
                    
                   
                    Values = new ChartValues<double> {y[0],y[1],y[2],y[3],y[4],y[5],y[6],y[7],y[8],y[9],y[10]},
                },
                //new LineSeries
                //{
                //    Title = "yu[1]",
                //    Values = new ChartValues<double> { },
                //    PointGeometry = null
                //},
                //new LineSeries
                //{
                //    Title = "yu[2]",
                //    Values = new ChartValues<double> { },
                //    PointGeometry = DefaultGeometries.Square,
                //    PointGeometrySize = 15
                //}
            };

            Labels = new[] { "0", "2", "4", "6", "8", "10", "12", "14", "16", "18", "20" };
            ////YFormatter = value => value.ToString("C");

            ////modifying the series collection will animate and update the chart
            //SeriesCollection.Add(new LineSeries
            //{
            //    Title = "yu[3]",
            //    Values = new ChartValues<double> { },
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    //PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 15,
            //    PointForeground = Brushes.Gray
            //});

            ////modifying any series values will also animate and update the chart
            //SeriesCollection[3].Values.Add(5d);

            DataContext = this;

        }



        //mainWindow保存图像
        public void SavePicButton_Click(object sender, RoutedEventArgs e)
        {
            myChart.Background = Brushes.White;//设置背景颜色

            //此处一样要有，否则导出图片为空白
            grid.Measure(myChart.RenderSize);
            grid.Arrange(new Rect(new Point(0, 0), myChart.RenderSize));
            //myChart.Update(true, true); //force chart redraw（重画图像）
            grid.UpdateLayout();

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "(*.png)|*.png|(*.jpg)|*.jpg";
            if(saveDlg.ShowDialog() == true)
            {

                FileStream fs = File.Open(saveDlg.FileName, FileMode.Create, FileAccess.Write);

                RenderTargetBitmap bmp = new RenderTargetBitmap((int)myChart.ActualWidth, (int)myChart.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(myChart);
                //对象的集合编码转成图像流
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                //保存到路径中
                encoder.Save(fs);
                //释放资源
                fs.Close();
                fs.Dispose();
                MessageBox.Show("文件已成功保存到" + saveDlg.FileName);
            }
            
        }




    }

   

}
