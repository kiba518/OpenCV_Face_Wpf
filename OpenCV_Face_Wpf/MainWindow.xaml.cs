using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace OpenCV_Face_Wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            SelectImg();
        }
        public void SelectImg()
        {
            System.Windows.Forms.OpenFileDialog frm = new System.Windows.Forms.OpenFileDialog();
            frm.Filter = "(*.jpg,*.png,*.jpeg,*.bmp)|*.jgp;*.png;*.jpeg;*.bmp|All files(*.*)|*.*"; 
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CutFace(frm.FileName);
            } 
           
        }
        public void CutFace(string filename)
        {
            //CvInvoke.UseOpenCL = CvInvoke.HaveOpenCLCompatibleGpuDevice;//使用GPU运算
            var face = new CascadeClassifier("haarcascade_frontalface_default.xml"); 
            var mat = new Mat(filename, ImreadModes.Grayscale);//灰度导入图片 
            int minNeighbors = 7;//最小矩阵组，默认3
            var size = new System.Drawing.Size(10, 10);//最小头像大小
            var facesDetected = face.DetectMultiScale(mat, 1.1, minNeighbors, size);
            //循环把人脸部分切割出来并保存
            int index = 0; 
            var bitmap = Bitmap.FromFile(filename);
            foreach (var item in facesDetected)
            {
                index++;
                var bmpOut = new Bitmap(item.Width, item.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                var g = Graphics.FromImage(bmpOut);
                g.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, item.Width, item.Height),
                    new System.Drawing.Rectangle(item.X, item.Y, item.Width, item.Height), GraphicsUnit.Pixel);
                g.Dispose();
                bmpOut.Save($"Face_{index}.png", System.Drawing.Imaging.ImageFormat.Png);
                bmpOut.Dispose();
            } 
            bitmap.Dispose(); 
            mat.Dispose();
            face.Dispose();
        }

   
    }
}
