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
            cboServer = new ComboBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtMatKhau = new TextBox();
            txtTenDangNhap = new TextBox();
            btnDangNhap = new Button();
            btnDangKi = new Button();
            panel3 = new Panel();
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
            panel1.Name = "panel1";
            panel1.Size = new Size(334, 61);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 24.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(75, 6);
            label1.Name = "label1";
            label1.Size = new Size(186, 45);
            label1.TabIndex = 0;
            label1.Text = "Đăng nhập";
            // 
            // panel2
            // 
            panel2.Controls.Add(btnKetNoi);
            panel2.Controls.Add(cboServer);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(txtMatKhau);
            panel2.Controls.Add(txtTenDangNhap);
            panel2.Location = new Point(0, 67);
            panel2.Name = "panel2";
            panel2.Size = new Size(334, 245);
            panel2.TabIndex = 1;
            // 
            // btnKetNoi
            // 
            btnKetNoi.FlatStyle = FlatStyle.Popup;
            btnKetNoi.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnKetNoi.Location = new Point(231, 190);
            btnKetNoi.Name = "btnKetNoi";
            btnKetNoi.Size = new Size(83, 48);
            btnKetNoi.TabIndex = 7;
            btnKetNoi.Text = "Kết nối";
            btnKetNoi.UseVisualStyleBackColor = true;
            // 
            // cboServer
            // 
            cboServer.Font = new Font("Segoe UI", 12F);
            cboServer.FormattingEnabled = true;
            cboServer.Location = new Point(20, 201);
            cboServer.Name = "cboServer";
            cboServer.Size = new Size(186, 29);
            cboServer.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic);
            label4.Location = new Point(12, 177);
            label4.Name = "label4";
            label4.Size = new Size(98, 21);
            label4.TabIndex = 3;
            label4.Text = "Chọn server:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic);
            label3.Location = new Point(12, 96);
            label3.Name = "label3";
            label3.Size = new Size(82, 21);
            label3.TabIndex = 2;
            label3.Text = "Mật khẩu:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic);
            label2.Location = new Point(12, 15);
            label2.Name = "label2";
            label2.Size = new Size(120, 21);
            label2.TabIndex = 1;
            label2.Text = "Tên đăng nhập:";
            // 
            // txtMatKhau
            // 
            txtMatKhau.Font = new Font("Segoe UI", 12F);
            txtMatKhau.Location = new Point(20, 120);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(294, 29);
            txtMatKhau.TabIndex = 1;
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Font = new Font("Segoe UI", 12F);
            txtTenDangNhap.Location = new Point(20, 39);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(294, 29);
            txtTenDangNhap.TabIndex = 0;
            // 
            // btnDangNhap
            // 
            btnDangNhap.BackColor = SystemColors.InactiveCaption;
            btnDangNhap.FlatStyle = FlatStyle.Popup;
            btnDangNhap.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnDangNhap.Location = new Point(20, 20);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(130, 43);
            btnDangNhap.TabIndex = 4;
            btnDangNhap.Text = "Đăng nhập";
            btnDangNhap.UseVisualStyleBackColor = false;
            // 
            // btnDangKi
            // 
            btnDangKi.FlatStyle = FlatStyle.Popup;
            btnDangKi.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnDangKi.Location = new Point(184, 20);
            btnDangKi.Name = "btnDangKi";
            btnDangKi.Size = new Size(130, 43);
            btnDangKi.TabIndex = 5;
            btnDangKi.Text = "Đăng kí";
            btnDangKi.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnDangKi);
            panel3.Controls.Add(btnDangNhap);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 312);
            panel3.Name = "panel3";
            panel3.Size = new Size(334, 82);
            panel3.TabIndex = 6;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 394);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private ComboBox cboServer;
        private Button btnDangKi;
        private Button btnDangNhap;
        private Label label4;
        private Label label3;
        private Label label2;
        private TextBox txtMatKhau;
        private TextBox txtTenDangNhap;
        private Button btnKetNoi;
        private Panel panel3;
    }
}