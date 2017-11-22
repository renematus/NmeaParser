namespace NmeaParser
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tbSourceFile = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnParseFile = new System.Windows.Forms.Button();
            this.tbGpxFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbGGA = new System.Windows.Forms.TextBox();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbRMC = new System.Windows.Forms.TextBox();
            this.tbGLL = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbFtime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbFdistance = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbFiltrCount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tbSourceFile
            // 
            this.tbSourceFile.Enabled = false;
            this.tbSourceFile.Location = new System.Drawing.Point(13, 25);
            this.tbSourceFile.Name = "tbSourceFile";
            this.tbSourceFile.Size = new System.Drawing.Size(463, 20);
            this.tbSourceFile.TabIndex = 0;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(501, 21);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 1;
            this.btnOpenFile.Text = "Source file";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnParseFile
            // 
            this.btnParseFile.Location = new System.Drawing.Point(13, 126);
            this.btnParseFile.Name = "btnParseFile";
            this.btnParseFile.Size = new System.Drawing.Size(563, 31);
            this.btnParseFile.TabIndex = 2;
            this.btnParseFile.Text = "Parse file";
            this.btnParseFile.UseVisualStyleBackColor = true;
            this.btnParseFile.Click += new System.EventHandler(this.btnParseFile_Click);
            // 
            // tbGpxFile
            // 
            this.tbGpxFile.Enabled = false;
            this.tbGpxFile.Location = new System.Drawing.Point(64, 73);
            this.tbGpxFile.Name = "tbGpxFile";
            this.tbGpxFile.Size = new System.Drawing.Size(512, 20);
            this.tbGpxFile.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "GPX file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Status";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "GGA message count:";
            // 
            // tbGGA
            // 
            this.tbGGA.Enabled = false;
            this.tbGGA.Location = new System.Drawing.Point(146, 218);
            this.tbGGA.Name = "tbGGA";
            this.tbGGA.Size = new System.Drawing.Size(94, 20);
            this.tbGGA.TabIndex = 7;
            // 
            // tbStatus
            // 
            this.tbStatus.Enabled = false;
            this.tbStatus.Location = new System.Drawing.Point(64, 182);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.Size = new System.Drawing.Size(512, 20);
            this.tbStatus.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 251);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "RMC message count:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 276);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "GLL message count:";
            // 
            // tbRMC
            // 
            this.tbRMC.Enabled = false;
            this.tbRMC.Location = new System.Drawing.Point(146, 247);
            this.tbRMC.Name = "tbRMC";
            this.tbRMC.Size = new System.Drawing.Size(94, 20);
            this.tbRMC.TabIndex = 11;
            // 
            // tbGLL
            // 
            this.tbGLL.Enabled = false;
            this.tbGLL.Location = new System.Drawing.Point(146, 273);
            this.tbGLL.Name = "tbGLL";
            this.tbGLL.Size = new System.Drawing.Size(94, 20);
            this.tbGLL.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Filtr zmena casu (s)";
            // 
            // tbFtime
            // 
            this.tbFtime.Location = new System.Drawing.Point(119, 100);
            this.tbFtime.Name = "tbFtime";
            this.tbFtime.Size = new System.Drawing.Size(85, 20);
            this.tbFtime.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(253, 103);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "filtr min vzdalenost (m)";
            // 
            // tbFdistance
            // 
            this.tbFdistance.Location = new System.Drawing.Point(363, 99);
            this.tbFdistance.Name = "tbFdistance";
            this.tbFdistance.Size = new System.Drawing.Size(100, 20);
            this.tbFdistance.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(280, 224);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "filtr count";
            // 
            // tbFiltrCount
            // 
            this.tbFiltrCount.Enabled = false;
            this.tbFiltrCount.Location = new System.Drawing.Point(345, 217);
            this.tbFiltrCount.Name = "tbFiltrCount";
            this.tbFiltrCount.Size = new System.Drawing.Size(94, 20);
            this.tbFiltrCount.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 348);
            this.Controls.Add(this.tbFiltrCount);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbFdistance);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbFtime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbGLL);
            this.Controls.Add(this.tbRMC);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.tbGGA);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbGpxFile);
            this.Controls.Add(this.btnParseFile);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.tbSourceFile);
            this.Name = "Form1";
            this.Text = "NMEA parser";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbSourceFile;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnParseFile;
        private System.Windows.Forms.TextBox tbGpxFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbGGA;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbRMC;
        private System.Windows.Forms.TextBox tbGLL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbFtime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbFdistance;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbFiltrCount;
    }
}

