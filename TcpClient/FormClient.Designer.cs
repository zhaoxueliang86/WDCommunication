namespace TcpClient
{
    partial class FormClient
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
            groupBox1 = new GroupBox();
            BtnStart = new Button();
            txtPort = new TextBox();
            txtIp = new TextBox();
            BtnStop = new Button();
            groupBox2 = new GroupBox();
            rSend = new RichTextBox();
            groupBox3 = new GroupBox();
            rReceive = new RichTextBox();
            BtnSend = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(BtnSend);
            groupBox1.Controls.Add(BtnStop);
            groupBox1.Controls.Add(BtnStart);
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(txtIp);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1503, 104);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // BtnStart
            // 
            BtnStart.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            BtnStart.Location = new Point(371, 29);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(112, 40);
            BtnStart.TabIndex = 5;
            BtnStart.Text = "连接";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;
            // 
            // txtPort
            // 
            txtPort.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            txtPort.Location = new Point(257, 29);
            txtPort.MaxLength = 15;
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(89, 40);
            txtPort.TabIndex = 4;
            txtPort.Text = "6000";
            // 
            // txtIp
            // 
            txtIp.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            txtIp.Location = new Point(12, 29);
            txtIp.MaxLength = 15;
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(239, 40);
            txtIp.TabIndex = 3;
            txtIp.Text = "127.0.0.1";
            // 
            // BtnStop
            // 
            BtnStop.Enabled = false;
            BtnStop.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            BtnStop.Location = new Point(489, 28);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(112, 40);
            BtnStop.TabIndex = 6;
            BtnStop.Text = "断开";
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rSend);
            groupBox2.Dock = DockStyle.Top;
            groupBox2.Location = new Point(0, 104);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1503, 228);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            // 
            // rSend
            // 
            rSend.Dock = DockStyle.Fill;
            rSend.Location = new Point(3, 26);
            rSend.Name = "rSend";
            rSend.Size = new Size(1497, 199);
            rSend.TabIndex = 0;
            rSend.Text = "";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(rReceive);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(0, 332);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1503, 709);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            // 
            // rReceive
            // 
            rReceive.Dock = DockStyle.Fill;
            rReceive.Location = new Point(3, 26);
            rReceive.Name = "rReceive";
            rReceive.Size = new Size(1497, 680);
            rReceive.TabIndex = 0;
            rReceive.Text = "";
            // 
            // BtnSend
            // 
            BtnSend.Enabled = false;
            BtnSend.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            BtnSend.Location = new Point(674, 28);
            BtnSend.Name = "BtnSend";
            BtnSend.Size = new Size(112, 40);
            BtnSend.TabIndex = 7;
            BtnSend.Text = "发送";
            BtnSend.UseVisualStyleBackColor = true;
            BtnSend.Click += BtnSend_Click;
            // 
            // FormClient
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1503, 1041);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "FormClient";
            Text = "FormClient";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button BtnStart;
        private TextBox txtPort;
        private TextBox txtIp;
        private Button BtnSend;
        private Button BtnStop;
        private GroupBox groupBox2;
        private RichTextBox rSend;
        private GroupBox groupBox3;
        private RichTextBox rReceive;
    }
}