﻿namespace MCaptureDemo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel_camera = new System.Windows.Forms.Panel();
            this.textBox_height = new System.Windows.Forms.TextBox();
            this.textBox_width = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel_microphone = new System.Windows.Forms.Panel();
            this.label_audioDataTotal = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.frame = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_camera.SuspendLayout();
            this.panel_microphone.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(203, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "摄像头",
            "桌面",
            "麦克风",
            "声卡"});
            this.comboBox1.Location = new System.Drawing.Point(76, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "采集类型：";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(257, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "停止";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(11, 69);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(650, 486);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "分辨率宽：";
            // 
            // panel_camera
            // 
            this.panel_camera.Controls.Add(this.textBox_height);
            this.panel_camera.Controls.Add(this.textBox_width);
            this.panel_camera.Controls.Add(this.label3);
            this.panel_camera.Controls.Add(this.label2);
            this.panel_camera.Location = new System.Drawing.Point(7, 34);
            this.panel_camera.Name = "panel_camera";
            this.panel_camera.Size = new System.Drawing.Size(190, 31);
            this.panel_camera.TabIndex = 5;
            // 
            // textBox_height
            // 
            this.textBox_height.Location = new System.Drawing.Point(141, 7);
            this.textBox_height.Name = "textBox_height";
            this.textBox_height.Size = new System.Drawing.Size(40, 21);
            this.textBox_height.TabIndex = 5;
            this.textBox_height.Text = "480";
            // 
            // textBox_width
            // 
            this.textBox_width.Location = new System.Drawing.Point(71, 7);
            this.textBox_width.Name = "textBox_width";
            this.textBox_width.Size = new System.Drawing.Size(40, 21);
            this.textBox_width.TabIndex = 5;
            this.textBox_width.Text = "640";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "高：";
            // 
            // panel_microphone
            // 
            this.panel_microphone.Controls.Add(this.label_audioDataTotal);
            this.panel_microphone.Controls.Add(this.label4);
            this.panel_microphone.Location = new System.Drawing.Point(321, 5);
            this.panel_microphone.Name = "panel_microphone";
            this.panel_microphone.Size = new System.Drawing.Size(258, 31);
            this.panel_microphone.TabIndex = 6;
            this.panel_microphone.Visible = false;
            // 
            // label_audioDataTotal
            // 
            this.label_audioDataTotal.AutoSize = true;
            this.label_audioDataTotal.Location = new System.Drawing.Point(147, 11);
            this.label_audioDataTotal.Name = "label_audioDataTotal";
            this.label_audioDataTotal.Size = new System.Drawing.Size(11, 12);
            this.label_audioDataTotal.TabIndex = 1;
            this.label_audioDataTotal.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "采集到声音数据（byte）：";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DarkRed;
            this.label5.Location = new System.Drawing.Point(11, 563);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(191, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "试用版的MCapture只能工作5分钟！";
            this.label5.Visible = false;
            // 
            // frame
            // 
            this.frame.AutoSize = true;
            this.frame.Location = new System.Drawing.Point(311, 51);
            this.frame.Name = "frame";
            this.frame.Size = new System.Drawing.Size(41, 12);
            this.frame.TabIndex = 7;
            this.frame.Text = "label6";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 579);
            this.Controls.Add(this.frame);
            this.Controls.Add(this.panel_microphone);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel_camera);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "语音视频采集组件 MCapture Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_camera.ResumeLayout(false);
            this.panel_camera.PerformLayout();
            this.panel_microphone.ResumeLayout(false);
            this.panel_microphone.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel_camera;
        private System.Windows.Forms.TextBox textBox_height;
        private System.Windows.Forms.TextBox textBox_width;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel_microphone;
        private System.Windows.Forms.Label label_audioDataTotal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label frame;
    }
}
