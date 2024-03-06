using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        string img_src;
        Mat img;
        int cnt;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void img_process(double treshold_val)
        {
            Mat find_shape = img.Clone();

            Mat bin = new Mat();
            Cv2.CvtColor(img, bin, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(bin, bin, treshold_val, 255, ThresholdTypes.BinaryInv);
            Point[][] contours;
            HierarchyIndex[] hierarchy;

            Cv2.FindContours(bin, out contours, out hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxNone);

            List<Point[]> new_contours = new List<Point[]>();

            int shape_num = 1;
            foreach (Point[] contour in contours)
            {

                Moments moments = Cv2.Moments(contour, false);
                double area = Math.Abs(Cv2.ContourArea(contour, true));

                if (area > 5000)
                {
                    Console.WriteLine(area);
                    new_contours.Add(contour);
                    Cv2.DrawContours(find_shape, new Point[][] { contour },-1, Scalar.Gray, 3);
                    int Cx = (int)(moments.M10 / moments.M00);
                    int Cy = (int)(moments.M01 / moments.M00);
                    Cv2.Circle(find_shape, Cx, Cy, 5, Scalar.Black, -1);
                    Cv2.PutText(find_shape, shape_num.ToString(), new Point(Cx - 60, Cy + 25), HersheyFonts.HersheySimplex,1,Scalar.Black,1);
                    shape_num++;
                }
            }
            cnt = new_contours.Count;

            pictureBox2.Image = BitmapConverter.ToBitmap(find_shape);
            if (cnt == 0)
            {
                label1.Text = "No shape";
            }
            else
            {
                label1.Text = "find " + cnt + " shape";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                img_src = dlg.FileName;
                img = new Mat(img_src);
                pictureBox1.Image = BitmapConverter.ToBitmap(img);
                trackBar1.Value = (int)(240 / 2.55);
                img_process(240);
            }
            label1.Visible = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double threshval = trackBar1.Value * 2.55;

            img_process(threshval);
        }

    }
}
