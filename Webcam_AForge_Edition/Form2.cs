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
    public partial class Form2 : Form
    {
        public Graphics g;
        public Pen p = new Pen(Color.Black);
        public int colorLengthToRemove = 0;
        public bool closed = false;
        private Form1 form;
        private int counter = 500;
        public Color filterC;
        public Form2(Form1 form)
        {
            InitializeComponent();
            g = Diagram.CreateGraphics();
            this.form = form;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void Diagram_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            closed = true;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            colorLengthToRemove = trackBar1.Value;
            counter = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(counter != 500)
            {
                if(counter == 1)
                {
                    if (form.imgCapture.Image != null)
                    {
                        // Get the captured picture and put it in the imagestack variable
                        form.imageStack.Push(new Bitmap(form.imgCapture.Image));
                        //  undoToolStripMenuItem.Enabled = true;
                        Bitmap bt = new Bitmap(form.imgCapture.Image);

                        // Lock the image
                        BitmapData bData = bt.LockBits(new Rectangle(0, 0, bt.Width, bt.Height), ImageLockMode.ReadWrite, bt.PixelFormat);

                        // Read the image pixels information
                        IntPtr ptr = bData.Scan0;

                        // Get the length of the image's pixels and the color's layers
                        int len = bData.Stride * bt.Height;
                        int cLayers = bData.Stride / bt.Width;

                        // Create a byte array that will hold the pixels informations
                        byte[] bgrValues = new byte[len];

                        // Copy the image information to the byte array
                        System.Runtime.InteropServices.Marshal.Copy(ptr, bgrValues, 0, len);

                        for (int i = 0; i < len / cLayers; i++)
                        {
                            int avg = 0;
                            int B = bgrValues[i*cLayers] < colorLengthToRemove ? 0 : bgrValues[i * cLayers];
                            int G = bgrValues[i * cLayers + 1] < colorLengthToRemove ? 0 : bgrValues[i * cLayers + 1];
                            int R = bgrValues[i * cLayers + 2] < colorLengthToRemove ? 0 : bgrValues[i * cLayers + 2];
                            if (filterC == Color.Gray)
                            {
                                avg = (R + G + B) / 3;
                                bgrValues[i * cLayers] = (byte)avg;
                                bgrValues[i * cLayers + 1] = (byte)avg;
                                bgrValues[i * cLayers + 2] = (byte)avg;
                            }
                            else if (filterC == Color.Red)
                            {
                                bgrValues[i * cLayers + 2] = (byte)R;
                            }
                            else if (filterC == Color.Green)
                            {
                                bgrValues[i * cLayers + 1] = (byte)G;
                            }
                            else if (filterC == Color.Blue)
                            {
                                bgrValues[i * cLayers] = (byte)B;
                            }

                        }

                        // Copy the edited information from the byte array back to the bitmapData
                        System.Runtime.InteropServices.Marshal.Copy(bgrValues, 0, ptr, len);

                        // Unlock the image
                        bt.UnlockBits(bData);

                        form.imgCapture.Image = bt;
                        counter = 500;
                    }
                }
                else
                {
                    counter++;
                }
            }
        }

        private void Diagram_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
