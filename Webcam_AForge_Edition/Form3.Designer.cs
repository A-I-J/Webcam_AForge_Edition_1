namespace Webcam_AForge_Edition
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Sum = new System.Windows.Forms.TrackBar();
            this.Redish = new System.Windows.Forms.TrackBar();
            this.Greenish = new System.Windows.Forms.TrackBar();
            this.Blueish = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Sum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Redish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Greenish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Blueish)).BeginInit();
            this.SuspendLayout();
            // 
            // Sum
            // 
            this.Sum.LargeChange = 10;
            this.Sum.Location = new System.Drawing.Point(24, 23);
            this.Sum.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Sum.Maximum = 50;
            this.Sum.Minimum = -50;
            this.Sum.Name = "Sum";
            this.Sum.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Sum.Size = new System.Drawing.Size(90, 288);
            this.Sum.TabIndex = 0;
            this.Sum.ValueChanged += new System.EventHandler(this.Sum_ValueChanged);
            // 
            // Redish
            // 
            this.Redish.LargeChange = 10;
            this.Redish.Location = new System.Drawing.Point(126, 23);
            this.Redish.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Redish.Maximum = 50;
            this.Redish.Minimum = -50;
            this.Redish.Name = "Redish";
            this.Redish.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Redish.Size = new System.Drawing.Size(90, 288);
            this.Redish.TabIndex = 1;
            this.Redish.ValueChanged += new System.EventHandler(this.Redish_ValueChanged);
            // 
            // Greenish
            // 
            this.Greenish.LargeChange = 10;
            this.Greenish.Location = new System.Drawing.Point(228, 23);
            this.Greenish.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Greenish.Maximum = 50;
            this.Greenish.Minimum = -50;
            this.Greenish.Name = "Greenish";
            this.Greenish.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Greenish.Size = new System.Drawing.Size(90, 288);
            this.Greenish.TabIndex = 2;
            this.Greenish.ValueChanged += new System.EventHandler(this.Greenish_ValueChanged);
            // 
            // Blueish
            // 
            this.Blueish.LargeChange = 10;
            this.Blueish.Location = new System.Drawing.Point(330, 23);
            this.Blueish.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Blueish.Maximum = 50;
            this.Blueish.Minimum = -50;
            this.Blueish.Name = "Blueish";
            this.Blueish.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Blueish.Size = new System.Drawing.Size(90, 288);
            this.Blueish.TabIndex = 3;
            this.Blueish.ValueChanged += new System.EventHandler(this.Blueish_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 317);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Sum";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 317);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Red";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 317);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Green";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(324, 317);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "Blue";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 335);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Blueish);
            this.Controls.Add(this.Greenish);
            this.Controls.Add(this.Redish);
            this.Controls.Add(this.Sum);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximumSize = new System.Drawing.Size(450, 406);
            this.MinimumSize = new System.Drawing.Size(450, 406);
            this.Name = "Form3";
            this.Text = "Brightness";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Sum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Redish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Greenish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Blueish)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar Sum;
        private System.Windows.Forms.TrackBar Redish;
        private System.Windows.Forms.TrackBar Greenish;
        private System.Windows.Forms.TrackBar Blueish;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
    }
}