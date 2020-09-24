using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webcam_AForge_Edition
{
    public partial class Form3 : Form
    {
        Form1 form;
        private string color = "";
        private int[,] brightenValue = { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
        private int currentValue = 0;

        int counter = 500;

        public Form3(Form1 form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

 

        private void ChangeBrightness(string c)
        {
            // Check if the requirement exist
            if(form.imgCapture.Image != null)
            {
                
                // Get the image to change it's brightness
                Bitmap bm = new Bitmap(form.imgCapture.Image);

                form.imageStack.Push(bm);

                // Lock the bitmap
                BitmapData bData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, bm.PixelFormat);

                // Get the pixels colors information
                IntPtr ptr = bData.Scan0;

                // Get the pixels length and the colors layer
                int len = bData.Stride * bm.Height;
                int cLayers = bData.Stride / bm.Width;

                // Byte array that will hold the information of the pixels' colors
                byte[] bgrValues = new byte[len];

                // Copy the colors information to the byte array
                System.Runtime.InteropServices.Marshal.Copy(ptr, bgrValues, 0, len);

                int startAt = 0;
                bool checkSum = false;
                switch (c)
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
                for (int i = 0; i < len / cLayers; i++)
                {
                    if (bgrValues[i * cLayers + startAt] != 0 && !checkSum)
                    {
                        if (bgrValues[i * cLayers + startAt] + currentValue > 255)
                            bgrValues[i * cLayers + startAt] = 255;
                        else if(bgrValues[i * cLayers + startAt] + currentValue < 0)
                            bgrValues[i * cLayers + startAt] = 0;
                        else
                            bgrValues[i * cLayers + startAt] = (byte)(bgrValues[i * cLayers + startAt] + currentValue);
                    }
                    else if (checkSum)
                    {
                        for(int colorPos = 0; colorPos <= 2; colorPos++)
                        {
                            if (bgrValues[i * cLayers + startAt + colorPos] + currentValue > 255)
                                bgrValues[i * cLayers + startAt + colorPos] = 255;
                            else if (bgrValues[i * cLayers + startAt + colorPos] + currentValue < 0)
                                bgrValues[i * cLayers + startAt + colorPos] = 0;
                            else
                                bgrValues[i * cLayers + startAt + colorPos] = (byte)(bgrValues[i * cLayers + startAt + colorPos] + currentValue);
                        }
                    }
                }


                // Copy the edited information back to the original pic
                System.Runtime.InteropServices.Marshal.Copy(bgrValues, 0, ptr, len);

                // Unlock the image
                bm.UnlockBits(bData);

                form.imgCapture.Image = bm;

            }


        }

        private void Sum_ValueChanged(object sender, EventArgs e)
        {
            if(brightenValue[0,0] == 0)
                brightenValue[0,1] = Sum.Value;
            else
                brightenValue[0,1] = Sum.Value - brightenValue[0, 0];

            currentValue = brightenValue[0, 1];
            counter = 0;
            color = "Gray";
        }

        private void Redish_ValueChanged(object sender, EventArgs e)
        {
            if (brightenValue[3, 0] == 0)
                brightenValue[3, 1] = Redish.Value;
            else
                brightenValue[3, 1] = Redish.Value - brightenValue[3, 0];

            currentValue = brightenValue[3, 1];
            counter = 0;
            color = "Red";
        }

        private void Greenish_ValueChanged(object sender, EventArgs e)
        {
            if (brightenValue[2, 0] == 0)
                brightenValue[2, 1] = Greenish.Value;
            else
                brightenValue[2, 1] = Greenish.Value - brightenValue[2, 0];

            currentValue = brightenValue[2, 1];
            counter = 0;
            color = "Green";
        }

        private void Blueish_ValueChanged(object sender, EventArgs e)
        {
            if (brightenValue[1, 0] == 0)
                brightenValue[1, 1] = Blueish.Value;
            else
                brightenValue[1, 1] = Blueish.Value - brightenValue[1, 0];


            currentValue = brightenValue[1, 1];
            counter = 0;
            color = "Blue";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(counter != 500 & color != "")
            {
                if(counter == 2)
                {
                    switch (color)
                    {
                        case "Gray":
                            brightenValue[0, 0] = Sum.Value;
                            //MessageBox.Show(brightenValue[0, 0] + "");
                            break;
                        case "Red":
                            brightenValue[1, 0] = Blueish.Value;
                            //MessageBox.Show(brightenValue[1, 0] + "");
                            break;
                        case "Green":
                            brightenValue[2, 0] = Greenish.Value;
                            //MessageBox.Show(brightenValue[2, 0] + "");
                            break;
                        case "Blue":
                            brightenValue[3, 0] = Redish.Value;
                            //MessageBox.Show(brightenValue[3, 0] + "");
                            break;
                    }
                    ChangeBrightness(color);
                    counter = 500;
                    color = "";
                }
                else
                 counter++;
            }
        }
    }
}
