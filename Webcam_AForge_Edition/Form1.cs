using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Providers.Entities;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Webcam_AForge_Edition
{
    
    public partial class Form1 : Form
    {
        

        //variable declaration
        public Stack<Bitmap> imageStack;
        internal GlobalVars gv; // Instantiate Global var

        public int[] brightnessValues = { 0, 0, 0, 0 };


        private Form2[] _colorsFormArray = new Form2[4];

        private int maxNumber = 0;
        private int[] colorsToRemove = new int[3];


        public Form1()
        {
            InitializeComponent();
            buttonCamStart.Enabled = false;
            gv = new GlobalVars(); // Initilize variable
            //Filter.BackColor = Color.FromArgb(0, 150, 150, 150);
        }
        
        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void Form1_Load(object sender, EventArgs e)
        {
            imageStack = new Stack<Bitmap>();

            // Get all the caperas that are connected to this device
            gv.VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            // Run across all the caperas and show them in the cambobox
            foreach (FilterInfo VideoCaptureDevice in gv.VideoCaptureDevices)
            {
                comboBoxCameraList.Items.Add(VideoCaptureDevice.Name);
            }
            if (comboBoxCameraList.Items.Count > 0)
            {
                comboBoxCameraList.SelectedIndex = 0;
                buttonCamStart.Enabled = true;
            }
            buttonStop.Enabled = false;
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Show Yes/No dialog msg to check if the user really want to close the form
            DialogResult dr = MessageBox.Show("Sure you want to close?", "Are you sure?", MessageBoxButtons.YesNo);
            // Check if the user clicked on no
            if (DialogResult.No == dr)
            {
                e.Cancel = true;
            }
            else
            {
                // If the user didn't clicked on the no then close the form
                if (gv.FinalVideo != null)
                {
                    gv.FinalVideo.Stop();
                    gv.FinalVideo.WaitForStop();
                }
                gv.FinalVideo = null;
            }
        }
        
        /**************************************************************************************/
        //
        /**************************************************************************************/
        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Copy the sended image and save it in bitmap
            Bitmap video = (Bitmap)eventArgs.Frame.Clone(); // Type cast
            // Show the image in the video
            imgVideo.Image = video;

        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void buttonCapture_Click(object sender, EventArgs e)
        {
            if (gv.FinalVideo != null)
            {
                imgCapture.Image = (Image)imgVideo.Image.Clone();
                this.BackgroundImage = (Image)imgVideo.Image.Clone();
            }


        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void buttonCamStart_Click(object sender, EventArgs e)
        {
            gv.FinalVideo = new VideoCaptureDevice(gv.VideoCaptureDevices[comboBoxCameraList.SelectedIndex].MonikerString);

            CameraSettings cs = new CameraSettings(gv);
            DialogResult dr = cs.ShowDialog();

            if (DialogResult.OK == dr)
            {
                // Get video resolution possibilites
                VideoCapabilities[] vc = gv.FinalVideo.VideoCapabilities;
                // Get selected resolution
                int resolutionSelection = int.Parse(cs.tabControl1.SelectedTab.Text) - 1;  // Minus 1 due to 0 offset
                // Set camera resolution
                gv.FinalVideo.VideoResolution = vc[resolutionSelection];
                // Enable eventhandler
                gv.FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                gv.FinalVideo.Start();

                buttonCamStart.Enabled = false;
                buttonStop.Enabled = true;
            }
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Only if there is still something left on ehe stack
            if (imageStack.Count > 0)
                imgCapture.Image = imageStack.Pop();
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void resolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (gv.FinalVideo == null)
                return;

            gv.FinalVideo.SignalToStop();
            
            gv.FinalVideo.Stop();
            gv.FinalVideo.WaitForStop();
            gv.FinalVideo.NewFrame -= new NewFrameEventHandler(FinalVideo_NewFrame);
            
            CameraSettings cs = new CameraSettings(gv);
            DialogResult dr = cs.ShowDialog();

            if (DialogResult.OK == dr)
            {
                VideoCapabilities[] vc = gv.FinalVideo.VideoCapabilities;
                int resolutionSelection = int.Parse(cs.tabControl1.SelectedTab.Text) - 1;  // Minus 1 due to 0 offset

                gv.FinalVideo.VideoResolution = vc[resolutionSelection];

                gv.FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
                gv.FinalVideo.Start();

                buttonCamStart.Enabled = false;
                buttonStop.Enabled = true;
            }
        }

        /**************************************************************************************/
        //
        /**************************************************************************************/
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (gv.FinalVideo != null)
            {
                gv.FinalVideo.Stop();
                gv.FinalVideo.WaitForStop();
            }
            gv.FinalVideo = null;
            buttonCamStart.Enabled = true;
            buttonStop.Enabled = false;
        }

        private void colorScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawDiagram("Gray", Color.Gray, 0);
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawDiagram("Red", Color.Red, 1);
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawDiagram("Green", Color.Green, 2);
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawDiagram("Blue", Color.Blue, 3);
        }

        /// <summary>
        /// Draw a diagram
        /// </summary>
        /// <param name="color">The color that the diagram will be made of</param>
        /// <param name="drawC">The color of the pen that will draw the diagram</param>
        /// <param name="index">The index of the opened form2</param>
        private void DrawDiagram(string color, Color drawC, int index)
        {

            bool checkIsOpen = false; // Will be sued to decide whether to open a new form2 or no

            // Check if there is already a diagram opened for this color
            if (_colorsFormArray[index] != null)
                checkIsOpen = _colorsFormArray[index].closed;
            else
                checkIsOpen = true;

            if (imgCapture.Image != null & checkIsOpen)
            {

                // Show the form 2
                Form2 newDiagram = new Form2(this);
                int[] colorScale = new int[256];

                Bitmap bt = new Bitmap(imgCapture.Image);

                // Lock the image
                BitmapData bData = bt.LockBits(new Rectangle(0, 0, bt.Width, bt.Height), ImageLockMode.ReadWrite, bt.PixelFormat);

                // Get the information from the image
                IntPtr info = bData.Scan0;

                // Get the length of all pixel width there format
                int len = bData.Stride * bt.Height;

                // Get the color layers
                int cLayers = bData.Stride / bt.Width;

                // Create byte array to hold the colors value
                byte[] bgrValues = new byte[len];

                // Copy the colors information from the bitmapData to the arrray
                System.Runtime.InteropServices.Marshal.Copy(info, bgrValues, 0, len);

                //Bitmap graybt = new Bitmap(256, 100);
                int startAt = 0;
                bool checkSum = false;
                switch (color)
                {
                    case "Gray":
                        checkSum = true;
                        break;
                    case "Red":
                        startAt = 2;
                        break;
                    case "Green":
                        startAt = 1;
                        break;
                    case "Blue":
                        startAt = 0;
                        break;
                }

                // Run across all the required colors to make a diagraam
                for(int i = 0; i < len/cLayers; i++)
                {
                    if (bgrValues[i * cLayers + startAt] != 0 && !checkSum)
                        colorScale[bgrValues[i * cLayers + startAt]]++;
                    else if(checkSum)
                        colorScale[(bgrValues[i * cLayers] + bgrValues[i * cLayers + 1] + bgrValues[i * cLayers + 2])/3]++;
                }

               
                if(maxNumber == 0)
                    maxNumber = colorScale.Max() == 0? 1 : colorScale.Max();

                newDiagram.Show();

                

                newDiagram.g = newDiagram.Diagram.CreateGraphics();
                newDiagram.p = new Pen(drawC);
                newDiagram.p.Width = 1;
                newDiagram.filterC = drawC;

                for (int x = 0; x <= 255; x++)
                {

                    int yRun = 499 - (int)(colorScale[x] * 499 / maxNumber);
                    Point p1 = new Point(x,499);
                    Point p2 = new Point(x,yRun);

                    newDiagram.g.DrawLine(newDiagram.p, p1, p2);

                }

                _colorsFormArray[index] = newDiagram;

                
            }
        }


        public void setFilter(Color filterC, int removeColor)
        {
            if (imgCapture.Image != null)
            {
                /*
                // Get the captured picture and put it in the imagestack variable
                imageStack.Push(new Bitmap(imgCapture.Image));
                //  undoToolStripMenuItem.Enabled = true;
                Bitmap bt = new Bitmap(imgCapture.Image);
                //MessageBox.Show(filterC + "");
                for (int y = 0; y < bt.Height; y++)
                {
                    for (int x = 0; x < bt.Width; x++)
                    {
                        Color c = bt.GetPixel(x, y);
                        int avg = 0;
                        int R = c.R < removeColor ? 0 : c.R;
                        int G = c.G < removeColor ? 0 : c.G;
                        int B = c.B < removeColor ? 0 : c.B;

                        if (filterC == Color.Gray)
                        {
                            avg = (R + G + B) / 3;
                            bt.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                        }
                        else if(filterC == Color.Red)
                        {
                            bt.SetPixel(x, y, Color.FromArgb(R, 0, 0));
                        }
                        else if (filterC == Color.Green)
                        {
                            bt.SetPixel(x, y, Color.FromArgb(0, G, 0));
                        }
                        else if (filterC == Color.Blue)
                        {
                            bt.SetPixel(x, y, Color.FromArgb(0, 0, B));
                        }
                    }
                }
                imgCapture.Image = bt;*/





                // This is the lockbit method that is used for fast image processing
                // Get the captured picture and put it in the imagestack var, that will allow us to active the undo button
                imageStack.Push(new Bitmap(imgCapture.Image));

                // Get the picture and save it to a bitmap var
                Bitmap pic = new Bitmap(imgCapture.Image);

                // Get the data of the pciture
                BitmapData picData = pic.LockBits(new Rectangle(0, 0, pic.Width, pic.Height), ImageLockMode.ReadWrite, pic.PixelFormat);

                unsafe
                {
                    // Get the address of the first line.
                    // ptr will hold all the pixels and their colors
                    IntPtr ptr = picData.Scan0;


                    // Declare an array to hold the bytes of the bitmap.
                    int bytes = Math.Abs(picData.Stride) * pic.Height;
                    byte[] rgbValues = new byte[bytes];

                    // Copy the RGB values into the array.
                    // All the pixel colors will be hold in the rgbValues array
                    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                    /*for (int i = 0; i < rgbValues.Length/4; i++)
                    {
                        MessageBox.Show(rgbValues[i*4] + " : " + rgbValues[i*4+1] + " : " + rgbValues[i*4+2] + " : " + rgbValues[i*4+3]);
                    }*/


                    for (int i = 0; i < rgbValues.Length / 4; i++)
                    {
                        

                        if (filterC == Color.Gray)
                        {
                            int avg = (rgbValues[i * 4] + rgbValues[i * 4 + 1] + rgbValues[i * 4 + 2]) / 3;
                            rgbValues[i * 4] = (byte)avg;
                            rgbValues[i * 4 + 1] = (byte)avg;
                            rgbValues[i * 4 + 2] = (byte)avg;
                        }
                        else if (filterC == Color.Red)
                        {
                            rgbValues[i * 4] = 0;
                            rgbValues[i * 4 + 1] = 0;
                        }
                        else if (filterC == Color.Green)
                        {
                            rgbValues[i * 4] = 0;
                            rgbValues[i * 4 + 2] = 0;
                        }
                        else if (filterC == Color.Blue)
                        {
                            rgbValues[i * 4 + 2] = 0;
                            rgbValues[i * 4 + 1] = 0;
                        }

                    }


                    // Copy the RGB values back to the bitmap, Thsi function will copy the new color to the old image
                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

                    // Unlock the image
                    pic.UnlockBits(picData);

                    // Show the image
                    imgCapture.Image = pic;

                }



            }
        }

        private void redToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setFilter(Color.Red, 0);
        }

        private void greenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setFilter(Color.Green, 0);
        }

        private void blueToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setFilter(Color.Blue, 0);
        }

        private void grayToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setFilter(Color.Gray, 0);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.KeyCode + "");
        }

        private void buttonCapture_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.KeyCode + "");
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 brightness = new Form3(this);
            brightness.Show();
        }

        private void dimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an array that will have 9 color values
            int[,] colorValues = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            // Put the image to the image stack
            imageStack.Push((Bitmap)imgCapture.Image);

            // Create an bitmap with the image
            Bitmap bm = new Bitmap(imgCapture.Image);

            // Lock the image
            BitmapData bData = bm.LockBits(new Rectangle( 0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, bm.PixelFormat);

            // Get the required information
            int len = bData.Stride * bm.Height;
            int cLayers = bData.Stride / bm.Width;

            // Read the colors information from the picture
            IntPtr ptr = bData.Scan0;

            // Create byte array that will hold the colors information of the image
            byte[] bgrValues = new byte[len];

            // Copy the image information from ptr to the byte array
            System.Runtime.InteropServices.Marshal.Copy(ptr, bgrValues, 0, len);

            int theRestColors = 0;

            /* for (int repeat = 0; repeat < 1; repeat++)
             {


                 for (int i = 0; i < len; i++)
                 {
                     if (cLayers == 4)
                         if ((i % 3) == 0 && i != 0)
                             continue;

                     try
                     {

                         if ((i - theRestColors) < bData.Stride)
                         {
                             if ((i - theRestColors) == 0)
                             {
                                 bgrValues[i] = (byte)((bgrValues[i] + bgrValues[i + cLayers] + bgrValues[i + bData.Stride] + bgrValues[i + cLayers + bData.Stride]) / 4);
                             }
                             else if ((i - theRestColors) < bData.Stride)
                             {
                                 bgrValues[i] = (byte)((bgrValues[i] + bgrValues[i - cLayers] + bgrValues[i + bData.Stride] + bgrValues[i - cLayers + bData.Stride]) / 4);
                             }
                             else
                             {
                                 bgrValues[i] = (byte)((bgrValues[i] + bgrValues[i - cLayers] + bgrValues[i + bData.Stride]
                                     + bgrValues[i - cLayers + bData.Stride] + bgrValues[i + cLayers]
                                     + bgrValues[i + cLayers + bData.Stride]) / 6);
                             }
                         }
                         else if ((i - theRestColors) > len - bData.Stride)
                         {
                             if ((i - theRestColors) == 0)
                             {
                                 bgrValues[i] = (byte)((bgrValues[i] + bgrValues[i + cLayers] + bgrValues[i - bData.Stride] + bgrValues[i + cLayers - bData.Stride]) / 4);
                             }
                             else if ((i - theRestColors) < bData.Stride)
                             {
                                 bgrValues[i] = (byte)((bgrValues[i] + bgrValues[i - cLayers] + bgrValues[i - bData.Stride] + bgrValues[i - cLayers - bData.Stride]) / 4);
                             }
                             else
                             {
                                 bgrValues[i] = (byte)((bgrValues[i] + bgrValues[i - cLayers] + bgrValues[i - bData.Stride]
                                     + bgrValues[i - cLayers - bData.Stride] + bgrValues[i + cLayers]
                                     + bgrValues[i + cLayers - bData.Stride]) / 6);
                             }
                         }
                         else if (((i - theRestColors) % bData.Stride) == bData.Stride - 1)
                         {
                             bgrValues[i] = (byte)((bgrValues[i] + bgrValues[i - cLayers] + bgrValues[i - bData.Stride]
                                     + bgrValues[i - cLayers - bData.Stride] + bgrValues[i + bData.Stride]
                                     + bgrValues[i - cLayers + bData.Stride]) / 6);
                         }
                         else if ((i % bData.Stride) == 0 || (i % bData.Stride) == 1 || (i % bData.Stride) == 2)
                         {
                             bgrValues[i] = (byte)((bgrValues[i] + bgrValues[i + cLayers] + bgrValues[i - bData.Stride]
                                     + bgrValues[i + cLayers - bData.Stride] + bgrValues[i + bData.Stride]
                                     + bgrValues[i + cLayers + bData.Stride]) / 6);
                         }
                         else
                         {

                             bgrValues[i] = (byte)((bgrValues[i] + bgrValues[i + cLayers] + bgrValues[i - bData.Stride]
                                 + bgrValues[i + cLayers - bData.Stride] + bgrValues[i + bData.Stride]
                                 + bgrValues[i + cLayers + bData.Stride] + bgrValues[i - cLayers]
                                 + bgrValues[i - cLayers - bData.Stride] + bgrValues[i - cLayers + bData.Stride]) / 9);


                         }

                     } catch (Exception ee)
                     {
                         continue;
                     }

                     if (theRestColors == 2)
                         theRestColors = 0;
                     else
                         theRestColors++;
                 }
             }*/

            byte[,][] colors = new byte[9,9][];
            int center = (int)(Math.Round(colors.Length / 2f) - Math.Floor(colors.Length / 2f));

            for (int i = 0; i < len / cLayers; i++)
            {
                for (int ii = -4; ii < 5; ii++)
                {
                    for (int j = -4; j < 5; j++)
                    {
                        colors[ii + 4, j + 4] = new byte[3];
                        colors[ii + 4, j + 4][0] = (1 + i + (ii * bm.Width) < len) ||
                            ((i + j + 1) % bm.Width > (0 + 4 + j)) ? (byte)120 : bgrValues[i];
                        colors[ii + 4, j + 4][1] = (1 + i + (ii * bm.Width) < len) ||
                            ((i + j + 1) % bm.Width > (0 + 4 + j)) ? (byte)120 : bgrValues[i + 1];
                        colors[ii + 4, j + 4][2] = (1 + i + (ii * bm.Width) < len) ||
                            ((i + j + 1) % bm.Width > (0 + 4 + j)) ? (byte)120 : bgrValues[i + 2];
                    }
                }

                int countAvaliable = 81;
                int[] avg = { 0, 0, 0 };
                for (int ii = 0; ii < 9; ii++)
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if (colors[ii, j][0] == 0 && colors[ii, j][1] == 0 && colors[ii, j][2] == 0)
                        {
                            countAvaliable--;
                        }
                        else
                        {
                            avg[0] = colors[ii, j][0];
                            avg[1] = colors[ii, j][1];
                            avg[2] = colors[ii, j][2];
                        }
                    }
                }
                avg[0] /= countAvaliable;
                avg[1] /= countAvaliable;
                avg[2] /= countAvaliable;


                bgrValues[i * cLayers] = (byte)avg[0];
                bgrValues[i * cLayers] = (byte)avg[1];
                bgrValues[i * cLayers] = (byte)avg[2];




            }

            // Copy the information back to the image
            System.Runtime.InteropServices.Marshal.Copy(bgrValues, 0, ptr, len);

            // Unlock the image
            bm.UnlockBits(bData);

            // Show the image
            imgCapture.Image = bm;
        }
    }
}
