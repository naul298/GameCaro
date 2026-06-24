namespace FormGiaoDienGame
{
    partial class FormRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegister));
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            textBox1 = new TextBox();
            txtName = new Label();
            txtMauKhau = new Label();
            txtTenTaiKhoan = new Label();
            txtMatKhau = new TextBox();
            txtTenDangNhap = new TextBox();
            lblServer = new Label();
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
            panel1.BackColor = Color.Navy;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(334, 61);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // label1
            // 
            label1.BackColor = Color.Navy;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 8);
            label1.Name = "label1";
            label1.Size = new Size(310, 45);
            label1.TabIndex = 0;
            label1.Text = "Đăng kí";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(txtName);
            panel2.Controls.Add(txtMauKhau);
            panel2.Controls.Add(txtTenTaiKhoan);
            panel2.Controls.Add(txtMatKhau);
            panel2.Controls.Add(txtTenDangNhap);
            panel2.Location = new Point(0, 67);
            panel2.Name = "panel2";
            panel2.Size = new Size(334, 228);
            panel2.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            textBox1.Location = new Point(24, 47);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(294, 29);
            textBox1.TabIndex = 1;
            // 
            // txtName
            // 
            txtName.AutoSize = true;
            txtName.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic);
            txtName.Location = new Point(16, 23);
            txtName.Name = "txtName";
            txtName.Size = new Size(83, 21);
            txtName.TabIndex = 0;
            txtName.Text = "Họ và tên:";
            // 
            // txtMauKhau
            // 
            txtMauKhau.AutoSize = true;
            txtMauKhau.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic);
            txtMauKhau.Location = new Point(16, 153);
            txtMauKhau.Name = "txtMauKhau";
            txtMauKhau.Size = new Size(82, 21);
            txtMauKhau.TabIndex = 0;
            txtMauKhau.Text = "Mật khẩu:";
            // 
            // txtTenTaiKhoan
            // 
            txtTenTaiKhoan.AutoSize = true;
            txtTenTaiKhoan.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic);
            txtTenTaiKhoan.Location = new Point(16, 88);
            txtTenTaiKhoan.Name = "txtTenTaiKhoan";
            txtTenTaiKhoan.Size = new Size(120, 21);
            txtTenTaiKhoan.TabIndex = 0;
            txtTenTaiKhoan.Text = "Tên đăng nhập:";
            // 
            // txtMatKhau
            // 
            txtMatKhau.Font = new Font("Segoe UI", 12F);
            txtMatKhau.Location = new Point(24, 177);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.Size = new Size(294, 29);
            txtMatKhau.TabIndex = 3;
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Font = new Font("Segoe UI", 12F);
            txtTenDangNhap.Location = new Point(24, 112);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(294, 29);
            txtTenDangNhap.TabIndex = 2;
            // 
            // lblServer
            // 
            lblServer.Dock = DockStyle.Top;
            lblServer.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblServer.ForeColor = Color.Black;
            lblServer.Location = new Point(0, 0);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(334, 15);
            lblServer.TabIndex = 0;
            lblServer.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnDangKi
            // 
            btnDangKi.BackColor = Color.Navy;
            btnDangKi.FlatStyle = FlatStyle.Popup;
            btnDangKi.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnDangKi.ForeColor = SystemColors.Control;
            btnDangKi.Location = new Point(102, 24);
            btnDangKi.Name = "btnDangKi";
            btnDangKi.Size = new Size(130, 40);
            btnDangKi.TabIndex = 1;
            btnDangKi.Text = "Đăng kí";
            btnDangKi.UseVisualStyleBackColor = false;
            btnDangKi.Click += btnDangKi_Click;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.GradientInactiveCaption;
            panel3.Controls.Add(lblServer);
            panel3.Controls.Add(lblStatus);
            panel3.Controls.Add(btnDangKi);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 301);
            panel3.Name = "panel3";
            panel3.Size = new Size(334, 88);
            panel3.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Black;
            lblStatus.Location = new Point(0, 67);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(334, 21);
            lblStatus.TabIndex = 0;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormRegister
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(334, 389);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(panel3);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormRegister";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormRegister";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private Label txtMauKhau;
        private Label txtTenTaiKhoan;
        private TextBox txtMatKhau;
        private TextBox txtTenDangNhap;
        private Button btnDangKi;
        private Panel panel3;
        private Label lblStatus;
        private TextBox textBox1;
        private Label txtName;
        private Label lblServer;
    }
}