namespace TcpServer
{
    partial class FormTouchSocket
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
            cbEncoding = new ComboBox();
            BtnStop = new Button();
            BtnStart = new Button();
            txtPort = new TextBox();
            txtIp = new TextBox();
            groupBox2 = new GroupBox();
            lbClient = new ListBox();
            groupBox3 = new GroupBox();
            rtxt = new RichTextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbEncoding);
            groupBox1.Controls.Add(BtnStop);
            groupBox1.Controls.Add(BtnStart);
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(txtIp);
            groupBox1.Dock = DockStyle.Top;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1541, 95);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            // 
            // cbEncoding
            // 
            cbEncoding.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEncoding.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            cbEncoding.FormattingEnabled = true;
            cbEncoding.Items.AddRange(new object[] { "ASCII", "UTF-8", "HEX" });
            cbEncoding.Location = new Point(854, 31);
            cbEncoding.Name = "cbEncoding";
            cbEncoding.Size = new Size(182, 40);
            cbEncoding.TabIndex = 5;
            // 
            // BtnStop
            // 
            BtnStop.Enabled = false;
            BtnStop.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            BtnStop.Location = new Point(504, 28);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(112, 40);
            BtnStop.TabIndex = 4;
            BtnStop.Text = "停止";
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // BtnStart
            // 
            BtnStart.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            BtnStart.Location = new Point(386, 29);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(112, 40);
            BtnStart.TabIndex = 2;
            BtnStart.Text = "启动";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;
            // 
            // txtPort
            // 
            txtPort.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            txtPort.Location = new Point(272, 29);
            txtPort.MaxLength = 15;
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(89, 40);
            txtPort.TabIndex = 1;
            txtPort.Text = "6000";
            // 
            // txtIp
            // 
            txtIp.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            txtIp.Location = new Point(27, 29);
            txtIp.MaxLength = 15;
            txtIp.Name = "txtIp";
            txtIp.Size = new Size(239, 40);
            txtIp.TabIndex = 0;
            txtIp.Text = "127.0.0.1";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lbClient);
            groupBox2.Dock = DockStyle.Left;
            groupBox2.Location = new Point(0, 95);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(487, 845);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            // 
            // lbClient
            // 
            lbClient.Dock = DockStyle.Fill;
            lbClient.Font = new Font("Arial", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lbClient.FormattingEnabled = true;
            lbClient.ItemHeight = 32;
            lbClient.Location = new Point(3, 26);
            lbClient.Name = "lbClient";
            lbClient.Size = new Size(481, 816);
            lbClient.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(rtxt);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(487, 95);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1054, 845);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            // 
            // rtxt
            // 
            rtxt.Dock = DockStyle.Fill;
            rtxt.Font = new Font("Arial", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
            rtxt.Location = new Point(3, 26);
            rtxt.Name = "rtxt";
            rtxt.Size = new Size(1048, 816);
            rtxt.TabIndex = 0;
            rtxt.Text = "";
            // 
            // FormTouchSocket
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1541, 940);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "FormTouchSocket";
            Text = "FormTouchSocket";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cbEncoding;
        private Button BtnStop;
        private Button BtnStart;
        private TextBox txtPort;
        private TextBox txtIp;
        private GroupBox groupBox2;
        private ListBox lbClient;
        private GroupBox groupBox3;
        private RichTextBox rtxt;
    }
}