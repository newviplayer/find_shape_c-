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
        public string test(Vec3b aaa)
        {
            if (aaa.Item2 > aaa.Item1 && aaa.Item2 > aaa.Item0)
                return "Red";
            else if (aaa.Item1 > aaa.Item0 && aaa.Item1 > aaa.Item2)
                return "Green";
            else
                return "Blue";
        }
        public void img_process(double treshold_val)
        {
            Mat find_shape = img.Clone();
            Console.WriteLine(find_shape.Height);
            Console.WriteLine(find_shape.Width);
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
                    Console.WriteLine(shape_num);
                    Console.WriteLine(area);
                    new_contours.Add(contour);
                    Cv2.DrawContours(find_shape, new Point[][] { contour },-1, Scalar.DarkGray, 3);
                    int Cx = (int)(moments.M10 / moments.M00);
                    int Cy = (int)(moments.M01 / moments.M00);
                    //Console.WriteLine(Cx + " ," + Cy);
                    Vec3b pixelValue = img.At<Vec3b>(Cy, Cx);
                    //Console.WriteLine(pixelValue);
                    string what_color = test(pixelValue);
                    //int hue = GetHue(pixelValue.Item2, pixelValue.Item1, pixelValue.Item0);
                    //Console.WriteLine(hue);
                    Cv2.Circle(find_shape, Cx, Cy, 1, Scalar.Black, -1);
                    //Cv2.PutText(find_shape, shape_num.ToString(), new Point(Cx - 60, Cy), HersheyFonts.HersheySimplex, 1, Scalar.Black, 2);
                    Cv2.PutText(find_shape, what_color, new Point(Cx - 60, Cy + 25), HersheyFonts.HersheySimplex, 1, Scalar.Black, 2);
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
        public void open_image()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                img_src = dlg.FileName;
                img = new Mat(img_src);
                Console.WriteLine(img.Height + ",   " + img.Width);
                pictureBox1.Image = BitmapConverter.ToBitmap(img);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double threshval = trackBar1.Value * 2.55;
            Console.WriteLine(threshval);
            img_process(threshval);
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {         
            open_image();
            pictureBox2.Image = null;
            trackBar1.Enabled = false;
            label1.Visible = false;
        }

        private void detectShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(120 / 2.55);
            img_process(160.65);
            trackBar1.Enabled = true;
            label1.Visible = true;
        }
    }
}

//1
//50원
//17129.5
//2
//500원
//247352
//3
//500원
//234243.5
//4
//100원
//195466.5
//5
//신 10원
//104327.5
//6
//구 10원
//157666.5
//7
//100원
//188179.5
//8
//신 10원
//101443

