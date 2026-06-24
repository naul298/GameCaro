namespace FormGiaoDienGame
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            btnKetNoi = new Button();
            txtServer = new ComboBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtMatKhau = new TextBox();
            txtTenDangNhap = new TextBox();
            btnDangNhap = new Button();
            btnDangKi = new Button();
            panel3 = new Panel();
            lblStatus = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.InactiveCaption;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(477, 102);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.BackColor = SystemColors.InactiveBorder;
            label1.Font = new Font("Segoe UI Semibold", 20.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(17, 13);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(443, 75);
            label1.TabIndex = 0;
            label1.Text = "Đăng nhập";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnKetNoi);
            panel2.Controls.Add(txtServer);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(txtMatKhau);
            panel2.Controls.Add(txtTenDangNhap);
            panel2.Location = new Point(0, 112);
            panel2.Margin = new Padding(4, 5, 4, 5);
            panel2.Name = "panel2";
            panel2.Size = new Size(477, 408);
            panel2.TabIndex = 1;
            // 
            // btnKetNoi
            // 
            btnKetNoi.BackColor = SystemColors.Control;
            btnKetNoi.BackgroundImage = Properties.Resources.kinhlup;
            btnKetNoi.BackgroundImageLayout = ImageLayout.Zoom;
            btnKetNoi.FlatStyle = FlatStyle.Popup;
            btnKetNoi.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnKetNoi.Location = new Point(311, 335);
            btnKetNoi.Margin = new Padding(4, 5, 4, 5);
            btnKetNoi.Name = "btnKetNoi";
            btnKetNoi.Size = new Size(60, 48);
            btnKetNoi.TabIndex = 7;
            btnKetNoi.UseVisualStyleBackColor = false;
            btnKetNoi.Click += btnKetNoi_Click;
            // 
            // txtServer
            // 
            txtServer.Font = new Font("Segoe UI", 12F);
            txtServer.FormattingEnabled = true;
            txtServer.Location = new Point(29, 335);
            txtServer.Margin = new Padding(4, 5, 4, 5);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(264, 40);
            txtServer.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic);
            label4.Location = new Point(17, 295);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(147, 32);
            label4.TabIndex = 3;
            label4.Text = "Chọn server:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic);
            label3.Location = new Point(17, 160);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(124, 32);
            label3.TabIndex = 2;
            label3.Text = "Mật khẩu:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic);
            label2.Location = new Point(17, 25);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(185, 32);
            label2.TabIndex = 1;
            label2.Text = "Tên đăng nhập:";
            // 
            // txtMatKhau
            // 
            txtMatKhau.Font = new Font("Segoe UI", 12F);
            txtMatKhau.Location = new Point(29, 200);
            txtMatKhau.Margin = new Padding(4, 5, 4, 5);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(418, 39);
            txtMatKhau.TabIndex = 1;
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Font = new Font("Segoe UI", 12F);
            txtTenDangNhap.Location = new Point(29, 65);
            txtTenDangNhap.Margin = new Padding(4, 5, 4, 5);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(418, 39);
            txtTenDangNhap.TabIndex = 0;
            // 
            // btnDangNhap
            // 
            btnDangNhap.BackColor = SystemColors.InactiveCaption;
            btnDangNhap.FlatStyle = FlatStyle.Popup;
            btnDangNhap.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnDangNhap.Location = new Point(29, 10);
            btnDangNhap.Margin = new Padding(4, 5, 4, 5);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(186, 67);
            btnDangNhap.TabIndex = 4;
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.UseVisualStyleBackColor = false;
            // 
            // btnDangKi
            // 
            btnDangKi.FlatStyle = FlatStyle.Popup;
            btnDangKi.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnDangKi.Location = new Point(263, 10);
            btnDangKi.Margin = new Padding(4, 5, 4, 5);
            btnDangKi.Name = "btnDangKi";
            btnDangKi.Size = new Size(186, 67);
            btnDangKi.TabIndex = 5;
            btnDangKi.Text = "Đăng kí";
            btnDangKi.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(lblStatus);
            panel3.Controls.Add(btnDangKi);
            panel3.Controls.Add(btnDangNhap);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 530);
            panel3.Margin = new Padding(4, 5, 4, 5);
            panel3.Name = "panel3";
            panel3.Size = new Size(477, 155);
            panel3.TabIndex = 6;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = SystemColors.ControlDarkDark;
            lblStatus.Location = new Point(173, 110);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(120, 32);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Loading...";
            lblStatus.TextAlign = ContentAlignment.BottomCenter;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(477, 685);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Caro Online";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private ComboBox txtServer;
        private Button btnDangKi;
        private Button btnDangNhap;
        private Label label4;
        private Label label3;
        private Label label2;
        private TextBox txtMatKhau;
        private TextBox txtTenDangNhap;
        private Button btnKetNoi;
        private Panel panel3;
        private Label lblStatus;
    }
}