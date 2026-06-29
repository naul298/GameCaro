namespace FormGiaoDienGame
{
    partial class FormGame
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGame));
            pnlBanCo = new Panel();
            btnChoiLai = new Button();
            btnDauHang = new Button();
            btnCauHoa = new Button();
            btnSanSang = new Button();
            btnThoatPhong = new Button();
            lblStatus = new Label();
            lblPlayer2 = new Label();
            lblPlayer1 = new Label();
            lblSoPhong = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            prcbCoolDown = new ProgressBar();
            tmCoolDown = new System.Windows.Forms.Timer(components);
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBanCo
            // 
            pnlBanCo.AutoSize = true;
            pnlBanCo.BackColor = SystemColors.GradientInactiveCaption;
            pnlBanCo.BorderStyle = BorderStyle.FixedSingle;
            pnlBanCo.Cursor = Cursors.Hand;
            pnlBanCo.ForeColor = SystemColors.ControlText;
            pnlBanCo.Location = new Point(0, 141);
            pnlBanCo.Name = "pnlBanCo";
            pnlBanCo.Size = new Size(572, 449);
            pnlBanCo.TabIndex = 0;
            // 
            // btnChoiLai
            // 
            btnChoiLai.BackColor = Color.Lime;
            btnChoiLai.Cursor = Cursors.Hand;
            btnChoiLai.FlatStyle = FlatStyle.Popup;
            btnChoiLai.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnChoiLai.ForeColor = Color.FromArgb(0, 0, 192);
            btnChoiLai.ImageAlign = ContentAlignment.MiddleLeft;
            btnChoiLai.Location = new Point(12, 91);
            btnChoiLai.Name = "btnChoiLai";
            btnChoiLai.Size = new Size(120, 44);
            btnChoiLai.TabIndex = 1;
            btnChoiLai.Text = "Chơi lại";
            btnChoiLai.UseVisualStyleBackColor = false;
            btnChoiLai.Click += btnChoiLai_Click;
            // 
            // btnDauHang
            // 
            btnDauHang.BackColor = Color.FromArgb(0, 0, 192);
            btnDauHang.Cursor = Cursors.Hand;
            btnDauHang.FlatStyle = FlatStyle.Popup;
            btnDauHang.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnDauHang.ForeColor = Color.White;
            btnDauHang.ImageAlign = ContentAlignment.MiddleLeft;
            btnDauHang.Location = new Point(296, 91);
            btnDauHang.Name = "btnDauHang";
            btnDauHang.Size = new Size(120, 44);
            btnDauHang.TabIndex = 2;
            btnDauHang.Text = "Đầu hàng";
            btnDauHang.UseVisualStyleBackColor = false;
            btnDauHang.Click += btnDauHang_Click;
            // 
            // btnCauHoa
            // 
            btnCauHoa.BackColor = Color.Yellow;
            btnCauHoa.Cursor = Cursors.Hand;
            btnCauHoa.FlatStyle = FlatStyle.Popup;
            btnCauHoa.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnCauHoa.ForeColor = Color.FromArgb(192, 0, 0);
            btnCauHoa.ImageAlign = ContentAlignment.MiddleLeft;
            btnCauHoa.Location = new Point(154, 91);
            btnCauHoa.Name = "btnCauHoa";
            btnCauHoa.Size = new Size(120, 44);
            btnCauHoa.TabIndex = 4;
            btnCauHoa.Text = "Cầu hoà";
            btnCauHoa.UseVisualStyleBackColor = false;
            btnCauHoa.Click += btnCauHoa_Click;
            // 
            // btnSanSang
            // 
            btnSanSang.BackColor = Color.Lime;
            btnSanSang.Cursor = Cursors.Hand;
            btnSanSang.Dock = DockStyle.Bottom;
            btnSanSang.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnSanSang.ForeColor = Color.Navy;
            btnSanSang.ImageAlign = ContentAlignment.MiddleLeft;
            btnSanSang.Location = new Point(0, 599);
            btnSanSang.Name = "btnSanSang";
            btnSanSang.Size = new Size(572, 54);
            btnSanSang.TabIndex = 8;
            btnSanSang.Text = "Sẵn sàng";
            btnSanSang.UseVisualStyleBackColor = false;
            btnSanSang.Click += btnSanSang_Click;
            // 
            // btnThoatPhong
            // 
            btnThoatPhong.BackColor = Color.FromArgb(192, 0, 0);
            btnThoatPhong.Cursor = Cursors.Hand;
            btnThoatPhong.FlatStyle = FlatStyle.Popup;
            btnThoatPhong.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnThoatPhong.ForeColor = Color.Yellow;
            btnThoatPhong.ImageAlign = ContentAlignment.MiddleLeft;
            btnThoatPhong.Location = new Point(438, 91);
            btnThoatPhong.Name = "btnThoatPhong";
            btnThoatPhong.Size = new Size(120, 44);
            btnThoatPhong.TabIndex = 7;
            btnThoatPhong.Text = "Thoát phòng";
            btnThoatPhong.UseVisualStyleBackColor = false;
            btnThoatPhong.Click += btnThoatPhong_Click;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.None;
            lblStatus.AutoSize = true;
            lblStatus.BackColor = Color.Navy;
            lblStatus.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Lime;
            lblStatus.Location = new Point(260, 62);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(49, 19);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "status";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPlayer2
            // 
            lblPlayer2.Anchor = AnchorStyles.Left;
            lblPlayer2.AutoSize = true;
            lblPlayer2.BackColor = Color.Navy;
            lblPlayer2.Font = new Font("Segoe UI", 12F);
            lblPlayer2.ForeColor = Color.White;
            lblPlayer2.ImageAlign = ContentAlignment.MiddleLeft;
            lblPlayer2.Location = new Point(402, 33);
            lblPlayer2.Name = "lblPlayer2";
            lblPlayer2.Size = new Size(101, 21);
            lblPlayer2.TabIndex = 2;
            lblPlayer2.Text = "Người chơi 2";
            lblPlayer2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPlayer1
            // 
            lblPlayer1.Anchor = AnchorStyles.Right;
            lblPlayer1.AutoSize = true;
            lblPlayer1.BackColor = Color.Navy;
            lblPlayer1.Font = new Font("Times New Roman", 14.25F, FontStyle.Bold);
            lblPlayer1.ForeColor = Color.White;
            lblPlayer1.ImageAlign = ContentAlignment.MiddleLeft;
            lblPlayer1.Location = new Point(52, 32);
            lblPlayer1.Name = "lblPlayer1";
            lblPlayer1.Size = new Size(116, 22);
            lblPlayer1.TabIndex = 3;
            lblPlayer1.Text = "Người chơi 1";
            lblPlayer1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSoPhong
            // 
            lblSoPhong.Anchor = AnchorStyles.None;
            lblSoPhong.AutoSize = true;
            lblSoPhong.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblSoPhong.ForeColor = Color.Yellow;
            lblSoPhong.ImageAlign = ContentAlignment.MiddleLeft;
            lblSoPhong.Location = new Point(247, 4);
            lblSoPhong.Name = "lblSoPhong";
            lblSoPhong.Size = new Size(76, 21);
            lblSoPhong.TabIndex = 4;
            lblSoPhong.Text = "Số phòng";
            lblSoPhong.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Navy;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Controls.Add(lblSoPhong, 1, 0);
            tableLayoutPanel1.Controls.Add(lblPlayer2, 2, 1);
            tableLayoutPanel1.Controls.Add(lblPlayer1, 0, 1);
            tableLayoutPanel1.Controls.Add(lblStatus, 1, 2);
            tableLayoutPanel1.Controls.Add(prcbCoolDown, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Top;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel1.Size = new Size(572, 85);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // prcbCoolDown
            // 
            prcbCoolDown.Anchor = AnchorStyles.None;
            prcbCoolDown.BackColor = SystemColors.Control;
            prcbCoolDown.ForeColor = Color.FromArgb(102, 187, 106);
            prcbCoolDown.Location = new Point(193, 32);
            prcbCoolDown.Name = "prcbCoolDown";
            prcbCoolDown.Size = new Size(183, 23);
            prcbCoolDown.Style = ProgressBarStyle.Continuous;
            prcbCoolDown.TabIndex = 8;
            // 
            // tmCoolDown
            // 
            tmCoolDown.Tick += tmCoolDown_Tick;
            // 
            // FormGame
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(572, 653);
            Controls.Add(btnSanSang);
            Controls.Add(btnThoatPhong);
            Controls.Add(btnDauHang);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(pnlBanCo);
            Controls.Add(btnCauHoa);
            Controls.Add(btnChoiLai);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormGame";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Caro Online";
            FormClosing += FormGame_FormClosing;
            Load += FormGame_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlBanCo;
        private Button btnChoiLai;
        private Button btnDauHang;
        private Button btnCauHoa;
        private Label lblPlayer2;
        private Label lblPlayer1;
        private Label lblSoPhong;
        private Label lblStatus;
        private TableLayoutPanel tableLayoutPanel1;
        private Button btnThoatPhong;
        private ProgressBar prcbCoolDown;
        private System.Windows.Forms.Timer tmCoolDown;
        private Button btnSanSang;
    }
}
