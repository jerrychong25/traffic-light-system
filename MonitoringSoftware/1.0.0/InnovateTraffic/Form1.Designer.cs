namespace InnovateTraffic
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.ledBulb12 = new InnovateTraffic.LedBulb();
            this.ledBulb11 = new InnovateTraffic.LedBulb();
            this.ledBulb10 = new InnovateTraffic.LedBulb();
            this.ledBulb9 = new InnovateTraffic.LedBulb();
            this.ledBulb8 = new InnovateTraffic.LedBulb();
            this.ledBulb7 = new InnovateTraffic.LedBulb();
            this.ledBulb6 = new InnovateTraffic.LedBulb();
            this.ledBulb5 = new InnovateTraffic.LedBulb();
            this.ledBulb4 = new InnovateTraffic.LedBulb();
            this.ledBulb3 = new InnovateTraffic.LedBulb();
            this.ledBulb2 = new InnovateTraffic.LedBulb();
            this.ledBulb1 = new InnovateTraffic.LedBulb();
            this.sevenSegment1 = new DmitryBrant.CustomControls.SevenSegment();
            this.sevenSegment2 = new DmitryBrant.CustomControls.SevenSegment();
            this.sevenSegment3 = new DmitryBrant.CustomControls.SevenSegment();
            this.sevenSegment4 = new DmitryBrant.CustomControls.SevenSegment();
            this.sevenSegment5 = new DmitryBrant.CustomControls.SevenSegment();
            this.sevenSegment6 = new DmitryBrant.CustomControls.SevenSegment();
            this.sevenSegment7 = new DmitryBrant.CustomControls.SevenSegment();
            this.sevenSegment8 = new DmitryBrant.CustomControls.SevenSegment();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(430, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(430, 94);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(339, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(427, 344);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label3.Location = new System.Drawing.Point(119, 437);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label4.Location = new System.Drawing.Point(22, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "label4";
            // 
            // ledBulb12
            // 
            this.ledBulb12.BackColor = System.Drawing.Color.Silver;
            this.ledBulb12.Location = new System.Drawing.Point(131, 211);
            this.ledBulb12.Name = "ledBulb12";
            this.ledBulb12.On = true;
            this.ledBulb12.Size = new System.Drawing.Size(23, 23);
            this.ledBulb12.TabIndex = 13;
            this.ledBulb12.Text = "ledBulb12";
            // 
            // ledBulb11
            // 
            this.ledBulb11.BackColor = System.Drawing.Color.Silver;
            this.ledBulb11.Color = System.Drawing.Color.Orange;
            this.ledBulb11.Location = new System.Drawing.Point(131, 188);
            this.ledBulb11.Name = "ledBulb11";
            this.ledBulb11.On = true;
            this.ledBulb11.Size = new System.Drawing.Size(24, 23);
            this.ledBulb11.TabIndex = 12;
            this.ledBulb11.Text = "ledBulb11";
            // 
            // ledBulb10
            // 
            this.ledBulb10.BackColor = System.Drawing.Color.Silver;
            this.ledBulb10.Color = System.Drawing.Color.Red;
            this.ledBulb10.Location = new System.Drawing.Point(131, 165);
            this.ledBulb10.Name = "ledBulb10";
            this.ledBulb10.On = true;
            this.ledBulb10.Size = new System.Drawing.Size(24, 23);
            this.ledBulb10.TabIndex = 11;
            this.ledBulb10.Text = "ledBulb10";
            // 
            // ledBulb9
            // 
            this.ledBulb9.BackColor = System.Drawing.Color.Silver;
            this.ledBulb9.Location = new System.Drawing.Point(215, 344);
            this.ledBulb9.Name = "ledBulb9";
            this.ledBulb9.On = true;
            this.ledBulb9.Size = new System.Drawing.Size(23, 23);
            this.ledBulb9.TabIndex = 10;
            this.ledBulb9.Text = "ledBulb9";
            // 
            // ledBulb8
            // 
            this.ledBulb8.BackColor = System.Drawing.Color.Silver;
            this.ledBulb8.Color = System.Drawing.Color.Orange;
            this.ledBulb8.Location = new System.Drawing.Point(191, 343);
            this.ledBulb8.Name = "ledBulb8";
            this.ledBulb8.On = true;
            this.ledBulb8.Size = new System.Drawing.Size(24, 23);
            this.ledBulb8.TabIndex = 9;
            this.ledBulb8.Text = "ledBulb8";
            // 
            // ledBulb7
            // 
            this.ledBulb7.BackColor = System.Drawing.Color.Silver;
            this.ledBulb7.Color = System.Drawing.Color.Red;
            this.ledBulb7.Location = new System.Drawing.Point(167, 343);
            this.ledBulb7.Name = "ledBulb7";
            this.ledBulb7.On = true;
            this.ledBulb7.Size = new System.Drawing.Size(24, 23);
            this.ledBulb7.TabIndex = 8;
            this.ledBulb7.Text = "ledBulb7";
            // 
            // ledBulb6
            // 
            this.ledBulb6.BackColor = System.Drawing.Color.Silver;
            this.ledBulb6.Location = new System.Drawing.Point(343, 254);
            this.ledBulb6.Name = "ledBulb6";
            this.ledBulb6.On = true;
            this.ledBulb6.Size = new System.Drawing.Size(23, 23);
            this.ledBulb6.TabIndex = 7;
            this.ledBulb6.Text = "ledBulb6";
            // 
            // ledBulb5
            // 
            this.ledBulb5.BackColor = System.Drawing.Color.Silver;
            this.ledBulb5.Color = System.Drawing.Color.Orange;
            this.ledBulb5.Location = new System.Drawing.Point(343, 278);
            this.ledBulb5.Name = "ledBulb5";
            this.ledBulb5.On = true;
            this.ledBulb5.Size = new System.Drawing.Size(24, 23);
            this.ledBulb5.TabIndex = 6;
            this.ledBulb5.Text = "ledBulb5";
            // 
            // ledBulb4
            // 
            this.ledBulb4.BackColor = System.Drawing.Color.Silver;
            this.ledBulb4.Color = System.Drawing.Color.Red;
            this.ledBulb4.Location = new System.Drawing.Point(343, 302);
            this.ledBulb4.Name = "ledBulb4";
            this.ledBulb4.On = true;
            this.ledBulb4.Size = new System.Drawing.Size(24, 23);
            this.ledBulb4.TabIndex = 5;
            this.ledBulb4.Text = "ledBulb4";
            // 
            // ledBulb3
            // 
            this.ledBulb3.BackColor = System.Drawing.Color.Silver;
            this.ledBulb3.Location = new System.Drawing.Point(257, 130);
            this.ledBulb3.Name = "ledBulb3";
            this.ledBulb3.On = true;
            this.ledBulb3.Size = new System.Drawing.Size(23, 23);
            this.ledBulb3.TabIndex = 2;
            this.ledBulb3.Text = "ledBulb3";
            // 
            // ledBulb2
            // 
            this.ledBulb2.BackColor = System.Drawing.Color.Silver;
            this.ledBulb2.Color = System.Drawing.Color.Orange;
            this.ledBulb2.Location = new System.Drawing.Point(281, 130);
            this.ledBulb2.Name = "ledBulb2";
            this.ledBulb2.On = true;
            this.ledBulb2.Size = new System.Drawing.Size(24, 23);
            this.ledBulb2.TabIndex = 1;
            this.ledBulb2.Text = "ledBulb2";
            // 
            // ledBulb1
            // 
            this.ledBulb1.BackColor = System.Drawing.Color.Silver;
            this.ledBulb1.Color = System.Drawing.Color.Red;
            this.ledBulb1.Location = new System.Drawing.Point(305, 130);
            this.ledBulb1.Name = "ledBulb1";
            this.ledBulb1.On = true;
            this.ledBulb1.Size = new System.Drawing.Size(24, 23);
            this.ledBulb1.TabIndex = 0;
            this.ledBulb1.Text = "ledBulb1";
            this.ledBulb1.Click += new System.EventHandler(this.ledBulb1_Click);
            // 
            // sevenSegment1
            // 
            this.sevenSegment1.ColorBackground = System.Drawing.Color.Silver;
            this.sevenSegment1.ColorDark = System.Drawing.Color.DimGray;
            this.sevenSegment1.ColorLight = System.Drawing.Color.Red;
            this.sevenSegment1.CustomPattern = 0;
            this.sevenSegment1.DecimalOn = false;
            this.sevenSegment1.DecimalShow = false;
            this.sevenSegment1.ElementWidth = 10;
            this.sevenSegment1.ItalicFactor = 0F;
            this.sevenSegment1.Location = new System.Drawing.Point(257, 60);
            this.sevenSegment1.Name = "sevenSegment1";
            this.sevenSegment1.Padding = new System.Windows.Forms.Padding(4);
            this.sevenSegment1.Size = new System.Drawing.Size(38, 64);
            this.sevenSegment1.TabIndex = 18;
            this.sevenSegment1.TabStop = false;
            this.sevenSegment1.Value = null;
            // 
            // sevenSegment2
            // 
            this.sevenSegment2.ColorBackground = System.Drawing.Color.Silver;
            this.sevenSegment2.ColorDark = System.Drawing.Color.DimGray;
            this.sevenSegment2.ColorLight = System.Drawing.Color.Red;
            this.sevenSegment2.CustomPattern = 0;
            this.sevenSegment2.DecimalOn = false;
            this.sevenSegment2.DecimalShow = false;
            this.sevenSegment2.ElementWidth = 10;
            this.sevenSegment2.ItalicFactor = 0F;
            this.sevenSegment2.Location = new System.Drawing.Point(291, 60);
            this.sevenSegment2.Name = "sevenSegment2";
            this.sevenSegment2.Padding = new System.Windows.Forms.Padding(4);
            this.sevenSegment2.Size = new System.Drawing.Size(38, 64);
            this.sevenSegment2.TabIndex = 19;
            this.sevenSegment2.TabStop = false;
            this.sevenSegment2.Value = null;
            // 
            // sevenSegment3
            // 
            this.sevenSegment3.ColorBackground = System.Drawing.Color.Silver;
            this.sevenSegment3.ColorDark = System.Drawing.Color.DimGray;
            this.sevenSegment3.ColorLight = System.Drawing.Color.Red;
            this.sevenSegment3.CustomPattern = 0;
            this.sevenSegment3.DecimalOn = false;
            this.sevenSegment3.DecimalShow = false;
            this.sevenSegment3.ElementWidth = 10;
            this.sevenSegment3.ItalicFactor = 0F;
            this.sevenSegment3.Location = new System.Drawing.Point(372, 254);
            this.sevenSegment3.Name = "sevenSegment3";
            this.sevenSegment3.Padding = new System.Windows.Forms.Padding(4);
            this.sevenSegment3.Size = new System.Drawing.Size(38, 64);
            this.sevenSegment3.TabIndex = 20;
            this.sevenSegment3.TabStop = false;
            this.sevenSegment3.Value = null;
            // 
            // sevenSegment4
            // 
            this.sevenSegment4.ColorBackground = System.Drawing.Color.Silver;
            this.sevenSegment4.ColorDark = System.Drawing.Color.DimGray;
            this.sevenSegment4.ColorLight = System.Drawing.Color.Red;
            this.sevenSegment4.CustomPattern = 0;
            this.sevenSegment4.DecimalOn = false;
            this.sevenSegment4.DecimalShow = false;
            this.sevenSegment4.ElementWidth = 10;
            this.sevenSegment4.ItalicFactor = 0F;
            this.sevenSegment4.Location = new System.Drawing.Point(407, 254);
            this.sevenSegment4.Name = "sevenSegment4";
            this.sevenSegment4.Padding = new System.Windows.Forms.Padding(4);
            this.sevenSegment4.Size = new System.Drawing.Size(38, 64);
            this.sevenSegment4.TabIndex = 21;
            this.sevenSegment4.TabStop = false;
            this.sevenSegment4.Value = null;
            // 
            // sevenSegment5
            // 
            this.sevenSegment5.ColorBackground = System.Drawing.Color.Silver;
            this.sevenSegment5.ColorDark = System.Drawing.Color.DimGray;
            this.sevenSegment5.ColorLight = System.Drawing.Color.Red;
            this.sevenSegment5.CustomPattern = 0;
            this.sevenSegment5.DecimalOn = false;
            this.sevenSegment5.DecimalShow = false;
            this.sevenSegment5.ElementWidth = 10;
            this.sevenSegment5.ItalicFactor = 0F;
            this.sevenSegment5.Location = new System.Drawing.Point(166, 372);
            this.sevenSegment5.Name = "sevenSegment5";
            this.sevenSegment5.Padding = new System.Windows.Forms.Padding(4);
            this.sevenSegment5.Size = new System.Drawing.Size(38, 64);
            this.sevenSegment5.TabIndex = 23;
            this.sevenSegment5.TabStop = false;
            this.sevenSegment5.Value = null;
            // 
            // sevenSegment6
            // 
            this.sevenSegment6.ColorBackground = System.Drawing.Color.Silver;
            this.sevenSegment6.ColorDark = System.Drawing.Color.DimGray;
            this.sevenSegment6.ColorLight = System.Drawing.Color.Red;
            this.sevenSegment6.CustomPattern = 0;
            this.sevenSegment6.DecimalOn = false;
            this.sevenSegment6.DecimalShow = false;
            this.sevenSegment6.ElementWidth = 10;
            this.sevenSegment6.ItalicFactor = 0F;
            this.sevenSegment6.Location = new System.Drawing.Point(200, 372);
            this.sevenSegment6.Name = "sevenSegment6";
            this.sevenSegment6.Padding = new System.Windows.Forms.Padding(4);
            this.sevenSegment6.Size = new System.Drawing.Size(38, 64);
            this.sevenSegment6.TabIndex = 22;
            this.sevenSegment6.TabStop = false;
            this.sevenSegment6.Value = null;
            // 
            // sevenSegment7
            // 
            this.sevenSegment7.ColorBackground = System.Drawing.Color.Silver;
            this.sevenSegment7.ColorDark = System.Drawing.Color.DimGray;
            this.sevenSegment7.ColorLight = System.Drawing.Color.Red;
            this.sevenSegment7.CustomPattern = 0;
            this.sevenSegment7.DecimalOn = false;
            this.sevenSegment7.DecimalShow = false;
            this.sevenSegment7.ElementWidth = 10;
            this.sevenSegment7.ItalicFactor = 0F;
            this.sevenSegment7.Location = new System.Drawing.Point(51, 168);
            this.sevenSegment7.Name = "sevenSegment7";
            this.sevenSegment7.Padding = new System.Windows.Forms.Padding(4);
            this.sevenSegment7.Size = new System.Drawing.Size(38, 64);
            this.sevenSegment7.TabIndex = 25;
            this.sevenSegment7.TabStop = false;
            this.sevenSegment7.Value = null;
            // 
            // sevenSegment8
            // 
            this.sevenSegment8.ColorBackground = System.Drawing.Color.Silver;
            this.sevenSegment8.ColorDark = System.Drawing.Color.DimGray;
            this.sevenSegment8.ColorLight = System.Drawing.Color.Red;
            this.sevenSegment8.CustomPattern = 0;
            this.sevenSegment8.DecimalOn = false;
            this.sevenSegment8.DecimalShow = false;
            this.sevenSegment8.ElementWidth = 10;
            this.sevenSegment8.ItalicFactor = 0F;
            this.sevenSegment8.Location = new System.Drawing.Point(85, 168);
            this.sevenSegment8.Name = "sevenSegment8";
            this.sevenSegment8.Padding = new System.Windows.Forms.Padding(4);
            this.sevenSegment8.Size = new System.Drawing.Size(38, 64);
            this.sevenSegment8.TabIndex = 24;
            this.sevenSegment8.TabStop = false;
            this.sevenSegment8.Value = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(984, 496);
            this.Controls.Add(this.sevenSegment7);
            this.Controls.Add(this.sevenSegment8);
            this.Controls.Add(this.sevenSegment5);
            this.Controls.Add(this.sevenSegment6);
            this.Controls.Add(this.sevenSegment4);
            this.Controls.Add(this.sevenSegment3);
            this.Controls.Add(this.sevenSegment2);
            this.Controls.Add(this.sevenSegment1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ledBulb12);
            this.Controls.Add(this.ledBulb11);
            this.Controls.Add(this.ledBulb10);
            this.Controls.Add(this.ledBulb9);
            this.Controls.Add(this.ledBulb8);
            this.Controls.Add(this.ledBulb7);
            this.Controls.Add(this.ledBulb6);
            this.Controls.Add(this.ledBulb5);
            this.Controls.Add(this.ledBulb4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ledBulb3);
            this.Controls.Add(this.ledBulb2);
            this.Controls.Add(this.ledBulb1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LedBulb ledBulb1;
        private LedBulb ledBulb2;
        private LedBulb ledBulb3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private LedBulb ledBulb4;
        private LedBulb ledBulb5;
        private LedBulb ledBulb6;
        private LedBulb ledBulb7;
        private LedBulb ledBulb8;
        private LedBulb ledBulb9;
        private LedBulb ledBulb10;
        private LedBulb ledBulb11;
        private LedBulb ledBulb12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private DmitryBrant.CustomControls.SevenSegment sevenSegment1;
        private DmitryBrant.CustomControls.SevenSegment sevenSegment2;
        private DmitryBrant.CustomControls.SevenSegment sevenSegment3;
        private DmitryBrant.CustomControls.SevenSegment sevenSegment4;
        private DmitryBrant.CustomControls.SevenSegment sevenSegment5;
        private DmitryBrant.CustomControls.SevenSegment sevenSegment6;
        private DmitryBrant.CustomControls.SevenSegment sevenSegment7;
        private DmitryBrant.CustomControls.SevenSegment sevenSegment8;
    }
}

