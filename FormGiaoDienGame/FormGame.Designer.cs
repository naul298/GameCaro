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
            button1 = new Button();
            button2 = new Button();
            button4 = new Button();
            btnSanSang = new Button();
            lblThoat = new Button();
            lblStatus = new Label();
            lblPlayer2 = new Label();
            lblPlayer1 = new Label();
            lblTysoP1 = new Label();
            lblTysoP2 = new Label();
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
            // button1
            // 
            button1.BackColor = Color.Lime;
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            button1.ForeColor = Color.FromArgb(0, 0, 192);
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(12, 91);
            button1.Name = "button1";
            button1.Size = new Size(120, 44);
            button1.TabIndex = 1;
            button1.Text = "Chơi lại";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(0, 0, 192);
            button2.Cursor = Cursors.Hand;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            button2.ForeColor = Color.White;
            button2.ImageAlign = ContentAlignment.MiddleLeft;
            button2.Location = new Point(296, 91);
            button2.Name = "button2";
            button2.Size = new Size(120, 44);
            button2.TabIndex = 2;
            button2.Text = "Đầu hàng";
            button2.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            button4.BackColor = Color.Yellow;
            button4.Cursor = Cursors.Hand;
            button4.FlatStyle = FlatStyle.Popup;
            button4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            button4.ForeColor = Color.FromArgb(192, 0, 0);
            button4.ImageAlign = ContentAlignment.MiddleLeft;
            button4.Location = new Point(154, 91);
            button4.Name = "button4";
            button4.Size = new Size(120, 44);
            button4.TabIndex = 4;
            button4.Text = "Cầu hoà";
            button4.UseVisualStyleBackColor = false;
            // 
            // btnSanSang
            // 
            btnSanSang.BackColor = Color.Lime;
            btnSanSang.Cursor = Cursors.Hand;
            btnSanSang.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnSanSang.ForeColor = Color.Navy;
            btnSanSang.ImageAlign = ContentAlignment.MiddleLeft;
            btnSanSang.Location = new Point(218, 596);
            btnSanSang.Name = "btnSanSang";
            btnSanSang.Size = new Size(137, 54);
            btnSanSang.TabIndex = 8;
            btnSanSang.Text = "Sẵn sàng";
            btnSanSang.UseVisualStyleBackColor = false;
            btnSanSang.Click += btnSanSang_Click;
            // 
            // lblThoat
            // 
            lblThoat.BackColor = Color.FromArgb(192, 0, 0);
            lblThoat.Cursor = Cursors.Hand;
            lblThoat.FlatStyle = FlatStyle.Popup;
            lblThoat.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblThoat.ForeColor = Color.Yellow;
            lblThoat.ImageAlign = ContentAlignment.MiddleLeft;
            lblThoat.Location = new Point(438, 91);
            lblThoat.Name = "lblThoat";
            lblThoat.Size = new Size(120, 44);
            lblThoat.TabIndex = 7;
            lblThoat.Text = "Thoát phòng";
            lblThoat.UseVisualStyleBackColor = false;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.None;
            lblStatus.AutoSize = true;
            lblStatus.BackColor = SystemColors.Control;
            lblStatus.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Gray;
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
            // lblTysoP1
            // 
            lblTysoP1.Anchor = AnchorStyles.Right;
            lblTysoP1.Font = new Font("Segoe UI", 12F);
            lblTysoP1.ForeColor = Color.FromArgb(192, 0, 0);
            lblTysoP1.ImageAlign = ContentAlignment.MiddleLeft;
            lblTysoP1.Location = new Point(119, 62);
            lblTysoP1.Name = "lblTysoP1";
            lblTysoP1.Size = new Size(49, 19);
            lblTysoP1.TabIndex = 7;
            lblTysoP1.Text = "label2";
            lblTysoP1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTysoP2
            // 
            lblTysoP2.Anchor = AnchorStyles.Left;
            lblTysoP2.Font = new Font("Segoe UI", 12F);
            lblTysoP2.ForeColor = Color.FromArgb(0, 192, 0);
            lblTysoP2.ImageAlign = ContentAlignment.MiddleLeft;
            lblTysoP2.Location = new Point(402, 62);
            lblTysoP2.Name = "lblTysoP2";
            lblTysoP2.Size = new Size(49, 19);
            lblTysoP2.TabIndex = 6;
            lblTysoP2.Text = "label2";
            lblTysoP2.TextAlign = ContentAlignment.MiddleCenter;
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
            tableLayoutPanel1.Controls.Add(lblTysoP2, 2, 2);
            tableLayoutPanel1.Controls.Add(lblTysoP1, 0, 2);
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
            ClientSize = new Size(572, 657);
            Controls.Add(btnSanSang);
            Controls.Add(lblThoat);
            Controls.Add(button2);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(pnlBanCo);
            Controls.Add(button4);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormGame";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Caro Online";
            FormClosing += FormGame_FormClosing;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlBanCo;
        private Button button1;
        private Button button2;
        private Button button4;
        private Label lblPlayer2;
        private Label lblPlayer1;
        private Label lblSoPhong;
        private Label lblStatus;
        private Label lblTysoP1;
        private Label lblTysoP2;
        private TableLayoutPanel tableLayoutPanel1;
        private Button lblThoat;
        private ProgressBar prcbCoolDown;
        private System.Windows.Forms.Timer tmCoolDown;
        private Button btnSanSang;
    }
}
