namespace CarLicensePlate
{
    partial class frm_main
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.letterA = new System.Windows.Forms.PictureBox();
            this.Letter_txt = new System.Windows.Forms.TextBox();
            this.letterB = new System.Windows.Forms.PictureBox();
            this.letterC = new System.Windows.Forms.PictureBox();
            this.btn_importIMG = new System.Windows.Forms.Button();
            this.letterD = new System.Windows.Forms.PictureBox();
            this.letterE = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterE)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Location = new System.Drawing.Point(153, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(176, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // letterA
            // 
            this.letterA.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.letterA.Location = new System.Drawing.Point(154, 86);
            this.letterA.Name = "letterA";
            this.letterA.Size = new System.Drawing.Size(35, 34);
            this.letterA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.letterA.TabIndex = 3;
            this.letterA.TabStop = false;
            this.letterA.Click += new System.EventHandler(this.letterA_Click);
            // 
            // Letter_txt
            // 
            this.Letter_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Letter_txt.Location = new System.Drawing.Point(347, 3);
            this.Letter_txt.Name = "Letter_txt";
            this.Letter_txt.Size = new System.Drawing.Size(46, 24);
            this.Letter_txt.TabIndex = 4;
            this.Letter_txt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // letterB
            // 
            this.letterB.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.letterB.Location = new System.Drawing.Point(189, 86);
            this.letterB.Name = "letterB";
            this.letterB.Size = new System.Drawing.Size(35, 34);
            this.letterB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.letterB.TabIndex = 3;
            this.letterB.TabStop = false;
            this.letterB.Click += new System.EventHandler(this.letterB_Click);
            // 
            // letterC
            // 
            this.letterC.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.letterC.Location = new System.Drawing.Point(224, 86);
            this.letterC.Name = "letterC";
            this.letterC.Size = new System.Drawing.Size(35, 34);
            this.letterC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.letterC.TabIndex = 6;
            this.letterC.TabStop = false;
            this.letterC.Click += new System.EventHandler(this.letterC_Click);
            // 
            // btn_importIMG
            // 
            this.btn_importIMG.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_importIMG.Location = new System.Drawing.Point(12, 20);
            this.btn_importIMG.Name = "btn_importIMG";
            this.btn_importIMG.Size = new System.Drawing.Size(126, 27);
            this.btn_importIMG.TabIndex = 7;
            this.btn_importIMG.Text = "Import and Recognition";
            this.btn_importIMG.UseVisualStyleBackColor = true;
            this.btn_importIMG.Click += new System.EventHandler(this.button3_Click);
            // 
            // letterD
            // 
            this.letterD.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.letterD.Location = new System.Drawing.Point(259, 86);
            this.letterD.Name = "letterD";
            this.letterD.Size = new System.Drawing.Size(35, 34);
            this.letterD.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.letterD.TabIndex = 3;
            this.letterD.TabStop = false;
            this.letterD.Click += new System.EventHandler(this.letterD_Click);
            // 
            // letterE
            // 
            this.letterE.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.letterE.Location = new System.Drawing.Point(294, 86);
            this.letterE.Name = "letterE";
            this.letterE.Size = new System.Drawing.Size(35, 34);
            this.letterE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.letterE.TabIndex = 6;
            this.letterE.TabStop = false;
            this.letterE.Click += new System.EventHandler(this.letterE_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 94);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(126, 26);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_close
            // 
            this.btn_close.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_close.Location = new System.Drawing.Point(12, 47);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(126, 29);
            this.btn_close.TabIndex = 8;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(9, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "DetectedLetters:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(154, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Letter Image";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(339, 128);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_importIMG);
            this.Controls.Add(this.letterE);
            this.Controls.Add(this.letterC);
            this.Controls.Add(this.letterD);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Letter_txt);
            this.Controls.Add(this.letterB);
            this.Controls.Add(this.letterA);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_main";
            this.Text = "Letter Recognition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_main_FormClosing);
            this.Load += new System.EventHandler(this.frm_main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.letterE)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox letterA;
        private System.Windows.Forms.TextBox Letter_txt;
        private System.Windows.Forms.PictureBox letterB;
        private System.Windows.Forms.PictureBox letterC;
        private System.Windows.Forms.Button btn_importIMG;
        private System.Windows.Forms.PictureBox letterD;
        private System.Windows.Forms.PictureBox letterE;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
    }
}

