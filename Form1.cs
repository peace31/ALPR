using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;
using System.IO;

namespace CarLicensePlate
{
    public partial class frm_main : Form
    {
        List<Image<Gray, Byte>> Letter_imgs = new List<Image<Gray, Byte>>();
        Matrix<float> LettersPattern;
        Matrix<float> response;
        List<string> res_array;
        bool train_flag = false;
        int m_time = 0;
        // ////////////////////////////////////////////////////////////////////////////////////////////////////

        public frm_main()
        {
            InitializeComponent();
        }
        // ////////////////////////////////////////////////////////////////////////////////////////////////////

        Image<Gray, Byte> bwareaopen(Image<Gray, Byte> binimg, int size)
        {

            Image<Gray, Byte> input = binimg.Clone();
            MemStorage storage = new MemStorage();
            IntPtr contour1 = new IntPtr();

            CvInvoke.cvFindContours(input.Ptr, storage, ref contour1, StructSize.MCvContour, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, new Point(0, 0));
            Seq<Point> contour = new Seq<Point>(contour1, null);

            double area;
            while (contour != null && contour.Ptr.ToInt32() != 0)
            {
                area = CvInvoke.cvContourArea(contour, Emgu.CV.Structure.MCvSlice.WholeSeq, 1);

                if (-size <= area && area <= 0)
                {
                    // removes white dots
                    CvInvoke.cvDrawContours(binimg.Ptr, contour.Ptr, new MCvScalar(0, 0, 0), new MCvScalar(0, 0, 0), -1, -1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
                }
                else if (0 < area && area <= size)
                {
                    // fills in black holes
                    CvInvoke.cvDrawContours(binimg.Ptr, contour.Ptr, new MCvScalar(0xff, 0xff, 0xff), new MCvScalar(0xff, 0xff, 0xff), -1, -1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
                }
                contour = contour.HNext;
            }
            return binimg;
        }
        // ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog of = new OpenFileDialog();
                of.Filter = "Image File(*.bmp,*.jpg,*.png,*.gif,*.tif)|*.bmp;*.jpg;*.png;*.gif;*.tif";
                DialogResult DR = of.ShowDialog();
                if (DR == DialogResult.Cancel) return;

                Emgu.CV.Image<Bgr, Byte> Origin_IMG = new Emgu.CV.Image<Bgr, byte>(of.FileName);
                int wid = Origin_IMG.Width, hei = Origin_IMG.Height;

                Emgu.CV.Image<Gray, Byte> Gray_IMG = Origin_IMG.Convert<Gray, Byte>();
                Emgu.CV.Image<Gray, Byte> BW_IMG = new Emgu.CV.Image<Gray, Byte>(wid, hei);

                CvInvoke.cvThreshold(Gray_IMG, BW_IMG, 0, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_OTSU);
                pictureBox1.Image = Origin_IMG.ToBitmap();

                bool flag = false;
                int start_pos = 0, end_pos = 0;
                //======================== set the start and end point by whether [0,17] point is black or white color===============================
                if (BW_IMG.Data[0, 20, 0] == 0)
                {
                    flag = true;
                    start_pos = 0; end_pos = hei - 1;
                }
                else
                {
                    start_pos = hei - 1; end_pos = 0;
                }

                //============ find the start point for boundary line seaching in 152 colume===========================================
                int[] bound_pos = new int[wid];
                int[] s_point = new int[wid];
                int m_s_value = -1;

                if (flag)
                {
                    for (int i = start_pos; i <= end_pos; i++)
                    {
                        if (m_s_value == -1) m_s_value = BW_IMG.Data[i, 152, 0];
                        else if (m_s_value != BW_IMG.Data[i, 152, 0])
                        {
                            s_point[152] = i;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = start_pos; i >= end_pos; i--)
                    {
                        if (m_s_value == -1) m_s_value = BW_IMG.Data[i, 152, 0];
                        else if (m_s_value != BW_IMG.Data[i, 152, 0])
                        {
                            s_point[152] = i;
                            break;
                        }
                    }
                }

                for (int j = 0; j < wid; j++)
                {
                    if (s_point[j] < 0 ) s_point[j]=0;
                    else if (s_point[j] >= hei) s_point[j] = hei-1;
                }

                //******************* search the boundary line from start point ***********************************
                for (int j = 151; j >= 17; j--)
                {
                    int top_bound_p = -1, end_pp = -1;
                    if (s_point[j + 1] - 1 <= 0) end_pp = 0;
                    else end_pp = s_point[j + 1] - 2;

                    for (int i = s_point[j + 1]; i >= end_pp; i--)
                    {
                        if (i - 1 <= 0 || i >= hei) break;
                        if (BW_IMG.Data[i, j, 0] == 255 && BW_IMG.Data[i - 1, j, 0] == 0)
                        {
                            top_bound_p = i;
                            break;
                        }
                        else if (BW_IMG.Data[i, j, 0] == 0 && BW_IMG.Data[i - 1, j, 0] == 255)
                        {
                            top_bound_p = i - 1;
                            break;
                        }
                    }

                    //=========================================================================
                    int bottom_bound_p = -1;
                    if (s_point[j + 1] + 1 >= hei) end_pp = hei;
                    else end_pp = s_point[j + 1] + 2;

                    for (int i = s_point[j + 1]; i <= end_pp; i++)
                    {
                        if (i + 1 >= hei || i < 0 ) break;
                        if (BW_IMG.Data[i, j, 0] == 255 && BW_IMG.Data[i + 1, j, 0] == 0)
                        {
                            bottom_bound_p = i; break;
                        }
                        else if (BW_IMG.Data[i, j, 0] == 0 && BW_IMG.Data[i + 1, j, 0] == 255)
                        {
                            bottom_bound_p = i + 1;
                            break;
                        }
                    }

                    //==========================================================================
                    if (top_bound_p != -1 && top_bound_p == bottom_bound_p)
                    {
                        s_point[j] = top_bound_p;
                        continue;
                    }

                    if (top_bound_p == -1 && top_bound_p == bottom_bound_p)
                    {
                        s_point[j] = s_point[j + 1];
                        continue;
                    }

                    if (bottom_bound_p - s_point[j + 1] == 1 && bottom_bound_p - top_bound_p == 2 && !flag)
                    {
                        s_point[j] = bottom_bound_p;
                        continue;
                    }
                    else if (bottom_bound_p - s_point[j + 1] == 1 && bottom_bound_p - top_bound_p == 2 && flag)
                    {
                        s_point[j] = top_bound_p;
                        continue;
                    }

                    if (s_point[j + 1] - top_bound_p == bottom_bound_p - s_point[j + 1] && !flag)
                    {
                        s_point[j] = bottom_bound_p;
                        continue;
                    }
                    else if (s_point[j + 1] - top_bound_p == bottom_bound_p - s_point[j + 1] && flag)
                    {
                        s_point[j] = top_bound_p;
                        continue;
                    }

                    //===============================================================================
                    if (top_bound_p == -1 && bottom_bound_p != -1) s_point[j] = bottom_bound_p;
                    else if (top_bound_p != -1 && bottom_bound_p == -1) s_point[j] = top_bound_p;
                    else if (s_point[j + 1] - top_bound_p >= bottom_bound_p - s_point[j + 1]) s_point[j] = bottom_bound_p;
                    else if (s_point[j + 1] - top_bound_p < bottom_bound_p - s_point[j + 1]) s_point[j] = top_bound_p;
                }

                //************************************************************************
                Emgu.CV.Image<Gray, Byte> plate_IMG = new Emgu.CV.Image<Gray, Byte>(wid, hei);
                Emgu.CV.Image<Gray, Byte> Letter_IMG = new Emgu.CV.Image<Gray, Byte>(wid, hei);

                for (int j = 17; j <= 152; j++)
                {
                    int top_p = 0;
                    int botoom_p = 0;
                    if (s_point[j] > start_pos)
                    {
                        top_p = start_pos;
                        botoom_p = s_point[j];
                    }
                    else
                    {
                        top_p = s_point[j] + 1;
                        botoom_p = start_pos + 1;
                    }

                    for (int i = top_p; i < botoom_p; i++)
                        plate_IMG.Data[i, j, 0] = 255;
                }
                //=========================================================================
                for (int i = 0; i < hei; i++)
                {
                    for (int j = 17; j <= 152; j++)
                    {
                        if (plate_IMG.Data[i, j, 0] == 0 && BW_IMG.Data[i, j, 0] == 0 || plate_IMG.Data[i, j, 0] == 255 && BW_IMG.Data[i, j, 0] == 255)
                            Letter_IMG.Data[i, j, 0] = 255;
                    }
                    for (int j = 0; j <= 17; j++)
                    {
                        if (BW_IMG.Data[i, j, 0] == 255) Letter_IMG.Data[i, j, 0] = 255;
                        if (BW_IMG.Data[i, 16, 0] == 0) Letter_IMG.Data[i, 17, 0] = 0;
                    }
                }

                letterA.Image = Letter_IMG.ToBitmap();
                //*******************************************************************************
                Letter_splite(Letter_IMG);
                //pictureBox1.Image = Letter_IMG.ToBitmap();
                letterA.Image = BW_IMG.ToBitmap();

                Application.DoEvents();
                Emgu.CV.Image<Gray, Byte> Letter_IMG1 = new Emgu.CV.Image<Gray, Byte>(wid, hei);
                if (Letter_imgs.Count <= 3)
                {
                    int[] top_bound = new int[BW_IMG.Width];
                    int[] bottom_bound = new int[BW_IMG.Width];
                    int[] equal_bound = new int[BW_IMG.Width];

                    if (flag)
                    {
                        top_bound = get_top_bound(BW_IMG, start_pos, end_pos);
                        bottom_bound = get_bottom_bound(BW_IMG, start_pos, end_pos);
                    }
                    else
                    {
                        top_bound = get_top_bound1(BW_IMG, end_pos, start_pos);
                        bottom_bound = get_bottom_bound1(BW_IMG, end_pos, start_pos);
                    }

                    //------------------------------------------------------------------
                    int origin_p = 152;
                    double h = 0;
                    int s_p = 152;

                    for (int j = 152; j >= 17; j--)
                    {
                        if (j == 18)
                        {
                            if (Math.Abs(equal_bound[s_p] - top_bound[17]) >= Math.Abs(equal_bound[s_p] - bottom_bound[17])) top_bound[17] = bottom_bound[17];
                            else bottom_bound[17] = top_bound[17];
                        }

                        if (j == 17)
                        {
                            int middle_point = 17;
                            if (!flag)
                            {
                                int n = 0;
                                for (int k1 = 17; k1 < s_p; k1++)
                                {
                                    if (top_bound[k1] >= equal_bound[17] && top_bound[k1] <= equal_bound[s_p])
                                        n++;
                                }
                                if (n == s_p - 17)
                                {
                                    middle_point = Convert.ToInt16((s_p + 17) / 2);
                                    h = Convert.ToDouble(equal_bound[s_p] - top_bound[middle_point]) / (s_p - middle_point);
                                    origin_p = middle_point;
                                    for (int k = s_p - middle_point; k >= 0; k--)
                                        equal_bound[s_p - k] = Convert.ToInt16(equal_bound[s_p] - h * k);

                                    s_p = middle_point;
                                }

                                for (int k1 = 0; k1 < hei; k1++)
                                {
                                    if (BW_IMG.Data[k1, 17, 0] == 255) top_bound[17] = k1;
                                    else if (BW_IMG.Data[k1, 17, 0] == 0 && BW_IMG.Data[k1, 16, 0] == 255) top_bound[17] = k1;
                                    else if (BW_IMG.Data[k1, 17, 0] == 0 && BW_IMG.Data[k1, 16, 0] == 0 && (BW_IMG.Data[k1 + 1, 16, 0] == 255 || BW_IMG.Data[k1 + 1, 17, 0] == 255)) top_bound[17] = k1;
                                    else break;
                                }
                            }
                            else
                            {
                                for (int k1 = hei - 2; k1 >= 0; k1--)
                                {
                                    if (BW_IMG.Data[k1, 17, 0] == 255) top_bound[17] = k1;
                                    else if (BW_IMG.Data[k1, 17, 0] == 0 && BW_IMG.Data[k1, 16, 0] == 255) top_bound[17] = k1;
                                    else if (BW_IMG.Data[k1, 17, 0] == 0 && BW_IMG.Data[k1, 16, 0] == 0 && (BW_IMG.Data[k1 - 1, 16, 0] == 255 || BW_IMG.Data[k1 - 1, 17, 0] == 255)) top_bound[17] = k1;
                                    else break;
                                }
                                
                            }
                            
                            h = Convert.ToDouble(equal_bound[s_p] - top_bound[j]) / (s_p - j);
                            origin_p = j;
                            for (int k = s_p - j; k >= 0; k--)
                                equal_bound[s_p - k] = Convert.ToInt16(equal_bound[s_p] - h * k);

                            continue;
                        }

                        //******************************************************************************************
                        if (top_bound[j] == bottom_bound[j] && origin_p - j <= 1)
                        {
                            if (Math.Abs(top_bound[s_p] - top_bound[j]) > 5)
                            {
                                if (Math.Abs(equal_bound[s_p] - top_bound[j]) >= Math.Abs(equal_bound[s_p] - bottom_bound[j]))       equal_bound[j] = bottom_bound[j];
                                else equal_bound[j] = top_bound[j];
                                continue;
                            }
                            equal_bound[j] = top_bound[j];
                            s_p = j; origin_p = j;
                        }
                        else if (top_bound[j] == bottom_bound[j] && origin_p - j > 15)
                        {
                            if (Math.Abs(s_p - j) <= 17 ) continue;
                            h = Convert.ToDouble(equal_bound[s_p] - top_bound[j]) / (s_p - j);
                            for (int k = s_p - j; k >= 0; k--)
                                equal_bound[s_p - k] = Convert.ToInt16(equal_bound[s_p] - h * k);

                            s_p = j;  origin_p = j;
                        }
                    }
                    //--------------------------------------------------------------------------------------------------
                    Emgu.CV.Image<Gray, Byte> plate_IMG1 = new Emgu.CV.Image<Gray, Byte>(wid, hei);
                    Letter_IMG1 = new Emgu.CV.Image<Gray, Byte>(wid, hei);
                    for (int i = 17; i <= 152; i++)
                    {
                        if (flag)
                        {
                            for (int j = 0; j <= equal_bound[i] - 1; j++)
                                plate_IMG1.Data[j, i, 0] = 255;
                        }
                        else
                        {
                            for (int j = equal_bound[i] + 1; j < hei; j++)
                                plate_IMG1.Data[j, i, 0] = 255;
                        }
                    }

                    //=========================================================================
                    for (int i = 0; i < hei; i++)
                    {
                        for (int j = 17; j <= 152; j++)
                        {
                            if (plate_IMG1.Data[i, j, 0] == 0 && BW_IMG.Data[i, j, 0] == 0 || plate_IMG1.Data[i, j, 0] == 255 && BW_IMG.Data[i, j, 0] == 255)
                                Letter_IMG1.Data[i, j, 0] = 255;
                        }
                        for (int j = 0; j <= 17; j++)
                        {
                            if (BW_IMG.Data[i, j, 0] == 255) Letter_IMG1.Data[i, j, 0] = 255;
                            if (BW_IMG.Data[i, 16, 0] == 0) Letter_IMG1.Data[i, 17, 0] = 0;
                        }
                    }

                    Letter_splite(Letter_IMG1);
                }

                //******************************** to split the letter into 2 letters*******************************************************
                if (Letter_imgs.Count == 4)
                {
                    int tmp_wid = 0, img_index = 0;

                    for (int i = 0; i < Letter_imgs.Count; i++)
                    {
                        if (Letter_imgs[i].Width > tmp_wid)
                        {
                            tmp_wid = Letter_imgs[i].Width;
                            img_index = i;
                        }
                    }
                   Image<Gray, Byte> split_img1;
                   Image<Gray, Byte> split_img2;
                   if (tmp_wid < 30)
                   {
                       if (img_index == 0 && Letter_imgs[1].Width == Letter_imgs[0].Width) img_index = 1;
                       split_img1 = Letter_imgs[img_index].Copy(new Rectangle(0, 0, Convert.ToInt32(tmp_wid / 2 + 5), Letter_imgs[img_index].Height));
                       split_img2 = Letter_imgs[img_index].Copy(new Rectangle(Convert.ToInt32(tmp_wid / 2) + 5, 0, tmp_wid- Convert.ToInt32(tmp_wid / 2) - 5, Letter_imgs[img_index].Height));
                   }
                   else
                   {
                       split_img1 = Letter_imgs[img_index].Copy(new Rectangle(0, 0, Convert.ToInt32(tmp_wid / 2 )+1, Letter_imgs[img_index].Height));
                       split_img2 = Letter_imgs[img_index].Copy(new Rectangle(Convert.ToInt32(tmp_wid / 2) + 2, 0, tmp_wid-Convert.ToInt32(tmp_wid / 2) - 2, Letter_imgs[img_index].Height));
                   }                   
                    

                    int[] v_hist = get_VertHist1(split_img1, 0, split_img1.Width, split_img1.Height);
                    int[] h_hist = get_horiHist1(split_img1);
                    split_img1 = split_img1.Copy(new Rectangle(h_hist[0], v_hist[0], h_hist[1] - h_hist[0], v_hist[1] - v_hist[0]));
                    v_hist = get_VertHist1(split_img2, 0, split_img2.Width, split_img2.Height);

                    h_hist = get_horiHist1(split_img2);
                    split_img2 = split_img2.Copy(new Rectangle(h_hist[0], v_hist[0], h_hist[1] - h_hist[0], v_hist[1] - v_hist[0]));


                    Letter_imgs[img_index] = bwareaopen(split_img1, 5);
                    Letter_imgs.Insert(img_index + 1, bwareaopen(split_img2, 5));

                    for (int i = 0; i < Letter_imgs[img_index].Height; i++)
                        Letter_IMG1.Data[i, Convert.ToInt32(tmp_wid / 2), 0] = 0;
                }

                //******************************** to display the letters*******************************************************
                if (Letter_imgs.Count >= 5)
                {
                    letterA.Image = Letter_imgs[0].ToBitmap();
                    letterB.Image = Letter_imgs[1].ToBitmap();
                    letterC.Image = Letter_imgs[2].ToBitmap();
                    letterD.Image = Letter_imgs[3].ToBitmap();
                    letterE.Image = Letter_imgs[4].ToBitmap();
                }
                else
                {
                    letterA.Image = null;
                    letterB.Image = null;
                    letterC.Image = null;
                    letterD.Image = null;
                    letterE.Image = null;
                    textBox1.Text = "No recognition";
                    return;
                }
                

                //*************** to detect the letter by using machine learning *******************************************************************************
                Matrix<float> testPattern;
                testPattern = new Matrix<float>(1, 400);

                string detectedLetters = "";
                for (int i = 0; i < Letter_imgs.Count; i++)
                {
                    Image<Gray, Byte> resize_IMG = Letter_imgs[i].Resize(20, 20, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                    int pixel_count = -1;
                    for (int i1 = 0; i1 < resize_IMG.Width; i1++)
                    {
                        for (int j1 = 0; j1 < resize_IMG.Height; j1++)
                        {
                            pixel_count++;
                            testPattern[0, pixel_count] = Convert.ToSingle(resize_IMG.Data[i1, j1, 0]);
                        }
                    }
                    //======================== to make the k-nearst model and train it by train data========================================================
                    int K = response.Rows;
                    Matrix<float> neighborResponses = new Matrix<float>(testPattern.Rows, K);
                    Matrix<float> results = new Matrix<float>(testPattern.Rows, 1);

                    Emgu.CV.ML.KNearest model = new Emgu.CV.ML.KNearest();
                    model.Train(LettersPattern, response, null, false, K, false);
                    float response1 = model.FindNearest(testPattern, K, results, null, neighborResponses, null);
                    detectedLetters += res_array[Convert.ToInt32(neighborResponses[0, 0])];
                }
                textBox1.Text = detectedLetters;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }


        // ///////////////////////////// to split the letter regions from inputed image  //////////////////////////////////////////////////////////////////
        private void Letter_splite(Image<Gray, Byte> input_Img)
        {
            int wid = input_Img.Width, hei = input_Img.Height;
            int[] vertical_histogram = new int[wid];
            for (int i = 0; i < wid; i++)
                for (int j = 0; j < hei; j++)
                    if (input_Img.Data[j, i, 0] == 255)
                        vertical_histogram[i]++;

            //====================================================================================
            Letter_imgs = new List<Image<Gray, Byte>>();

            for (int i = 0; i < wid; i++)
            {
                if (vertical_histogram[i] > 1)
                {
                    for (int j = i + 1; j < wid; j++)
                    {
                        if (vertical_histogram[j] <= 1 && j - i > 12)
                        {
                            int[] Top_bot_pos = get_VertHist(input_Img, i, j, hei);
                            Letter_imgs.Add(input_Img.Copy(new Rectangle(i, Top_bot_pos[0], j - i, Top_bot_pos[1] - Top_bot_pos[0])));
                            i = j + 1;
                            break;
                        }
                    }
                }
            }
        }

        // ////////////////////////// to get the top and bottom point with white color from inputed image /////////////////////////////////////////////////
        private int[] get_VertHist(Image<Gray, Byte> input_Img, int left_p, int right_p, int hei)
        {
            int[] returnVal = new int[2];
            int wid1 = right_p - left_p;
            int[] horizontal_histogram = new int[hei];
            for (int j1 = 0; j1 < hei; j1++)
            {
                for (int i1 = 0; i1 < wid1; i1++)
                {
                    if (input_Img.Data[j1, left_p + i1, 0] == 255)
                        horizontal_histogram[j1]++;
                }
            }

            int top_pos = -1, bottom_pos = hei;
            for (int j1 = 0; j1 < hei; j1++)
            {
                if (horizontal_histogram[j1] >= 2)
                { top_pos = j1; break; }
            }

            for (int j1 = hei - 1; j1 >= 0; j1--)
            {
                if (horizontal_histogram[j1] >= 2)
                { bottom_pos = j1 + 1; break; }
            }
            returnVal[0] = top_pos;
            returnVal[1] = bottom_pos;
            return returnVal;
        }


        // ////////////////////////// to get the top and bottom point with white color from inputed image /////////////////////////////////////////////////
        private int[] get_VertHist1(Image<Gray, Byte> input_Img, int left_p, int right_p, int hei)
        {
            int[] returnVal = new int[2];
            int wid1 = right_p - left_p;
            int[] horizontal_histogram = new int[hei];
            for (int j1 = 0; j1 < hei; j1++)
            {
                for (int i1 = 0; i1 < wid1; i1++)
                {
                    if (input_Img.Data[j1, left_p + i1, 0] == 255)
                        horizontal_histogram[j1]++;
                }
            }

            int top_pos = hei / 2, bottom_pos = hei / 2;
            for (int j1 = hei / 2; j1 < hei; j1++)
            {
                if (horizontal_histogram[j1] >= 1)
                {
                    top_pos = j1;
                }
                else
                {
                    if (j1 + 2 < hei && horizontal_histogram[j1 + 2] < 1) break;
                }
            }

            for (int j1 = hei / 2; j1 >= 0; j1--)
            {
                if (horizontal_histogram[j1] >= 1)
                {
                    bottom_pos = j1 + 1;
                }
                else
                {
                    if (j1 - 2 >= 0 && horizontal_histogram[j1 - 2] < 1) break;
                }
            }
            returnVal[0] = bottom_pos -1;
            returnVal[1] = top_pos + 1;
            return returnVal;
        }

        private int[] get_horiHist1(Image<Gray, Byte> input_Img)
        {
            int[] returnVal = new int[2];
            int wid = input_Img.Width;
            int hei = input_Img.Height;
            int[] horizontal_histogram = new int[wid];
            for (int j1 = 0; j1 < wid; j1++)
            {
                for (int i1 = 0; i1 < hei; i1++)
                {
                    if (input_Img.Data[i1, j1, 0] == 255)
                        horizontal_histogram[j1]++;
                }
            }

            int top_pos = wid / 2+1, bottom_pos = wid / 2-1;

            for (int j1 = wid/2; j1 < wid; j1++)
            {
                if (horizontal_histogram[j1] < 1 )
                { 
                    if (j1 + 2 < wid && horizontal_histogram[j1 + 2] < 1)  break; 
                }
                else top_pos = j1;
            }

            for (int j1 = wid/2 - 1; j1 >= 0; j1--)
            {
                if (horizontal_histogram[j1] < 1)
                { 
                    if (j1 - 2 >= 0 && horizontal_histogram[j1 - 2] < 1)  break; 
                }
                else bottom_pos = j1 ;
            }
            returnVal[0] = bottom_pos;
            returnVal[1] = top_pos;
            return returnVal;
        }
        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void letterA_Click(object sender, EventArgs e)
        {
            if (train_flag) SaveLetterPattern(Letter_imgs[0]);
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void letterB_Click(object sender, EventArgs e)
        {
            if (train_flag) SaveLetterPattern(Letter_imgs[1]);
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void letterC_Click(object sender, EventArgs e)
        {
            if (train_flag) SaveLetterPattern(Letter_imgs[2]);
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void letterD_Click(object sender, EventArgs e)
        {
            if (train_flag) SaveLetterPattern(Letter_imgs[3]);
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void letterE_Click(object sender, EventArgs e)
        {
            if (train_flag) SaveLetterPattern(Letter_imgs[4]);
        }
        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SaveLetterPattern(Image<Gray, Byte> input_Img)
        {
            StreamWriter sw = new StreamWriter("LetterPattern.ini", true, Encoding.UTF7);
            Image<Gray, Byte> resize_IMG = input_Img.Resize(20, 20, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            for (int i1 = 0; i1 < resize_IMG.Width; i1++)
            {
                for (int j1 = 0; j1 < resize_IMG.Height; j1++)
                {
                    if (i1 == 0 && j1 == 0) sw.Write(resize_IMG.Data[i1, j1, 0]);
                    else sw.Write("," + resize_IMG.Data[i1, j1, 0]);
                }
            }
            sw.WriteLine("");
            sw.Close();

            StreamWriter sw1 = new StreamWriter("Letter.ini", true, Encoding.UTF7);
            sw1.WriteLine(Letter_txt.Text);
            sw1.Close();
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void frm_main_Load(object sender, EventArgs e)
        {
            StreamReader sR = new StreamReader("Letter.ini", Encoding.UTF7, true);
            string[][] pattern_data = new string[10000][];
            res_array = new List<string>();

            while (!sR.EndOfStream)
            {
                res_array.Add(sR.ReadLine());
            }
            sR.Close();

            response = new Matrix<float>(res_array.Count, 1);
            for (int i = 0; i < res_array.Count; i++)
                response[i, 0] = i;

            LettersPattern = new Matrix<float>(res_array.Count, 400);
            StreamReader sR1 = new StreamReader("LetterPattern.ini", Encoding.UTF7, true);
            int line_count = -1;
            while (!sR1.EndOfStream)
            {
                line_count++;
                string tmp_str = sR1.ReadLine();
                string[] tmp_strArray = tmp_str.Split(',');
                for (int i = 0; i < tmp_strArray.Length; i++)
                    LettersPattern[line_count, i] = Convert.ToSingle(tmp_strArray[i]);
            }
            sR1.Close();


            sR1 = new StreamReader("OCl", Encoding.UTF7, true);
            while (!sR1.EndOfStream)
            {
                m_time = Convert.ToInt32(sR1.ReadLine());
            }
            sR1.Close();

            if (m_time > 3600) Application.Exit();
        }
        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void btn_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private int[] get_top_bound(Image<Gray, Byte> BW_IMG, int t_p, int bot_p)
        {
            int[] Return_bound = new int[BW_IMG.Width];
            for (int j = 17; j < 153; j++)
            {
                for (int i = t_p; i < bot_p; i++)
                {
                    if (BW_IMG.Data[i, j, 0] == 255)
                    {
                        Return_bound[j] = i;
                        break;
                    }
                }
            }
            return Return_bound;
        }
        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private int[] get_bottom_bound(Image<Gray, Byte> BW_IMG, int t_p, int bot_p)
        {
            int[] Return_bound = new int[BW_IMG.Width];
            for (int j = 17; j < 153; j++)
            {
                for (int i = bot_p; i >= t_p; i--)
                {
                    if (BW_IMG.Data[i, j, 0] == 0)  break;
                    else Return_bound[j] = i;
                }
            }
            return Return_bound;
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private int[] get_bottom_bound1(Image<Gray, Byte> BW_IMG, int t_p, int bot_p)
        {
            int[] Return_bound = new int[BW_IMG.Width];
            for (int j = 17; j < 153; j++)
            {
                for (int i = bot_p; i >= t_p; i--)
                {
                    if (BW_IMG.Data[i, j, 0] == 255)
                    {
                        Return_bound[j] = i;
                        break;
                    }
                }
            }
            return Return_bound;
        }
        // ////////////////////////////////////////////////////////////////////////////////////////////////////
        private int[] get_top_bound1(Image<Gray, Byte> BW_IMG, int t_p, int bot_p)
        {
            int[] Return_bound = new int[BW_IMG.Width];
            for (int j = 17; j < 153; j++)
            {
                for (int i = t_p; i < bot_p; i++)
                {
                    if (BW_IMG.Data[i, j, 0] == 0) break;
                    else Return_bound[j] = i;
                }
            }
            return Return_bound;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //m_time++;
        }

        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //StreamWriter sw1 = new StreamWriter("OCl", false, Encoding.UTF7);
            //sw1.WriteLine(m_time);
            //sw1.Close();
        }



    }
}
