namespace FormGiaoDienGame
{
    partial class FormLobby
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLobby));
            panel4 = new Panel();
            btnLamMoi = new Button();
            btnTaoPhong = new Button();
            dgvLobby = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colName = new DataGridViewTextBoxColumn();
            colHost = new DataGridViewTextBoxColumn();
            colPlayers = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            lblUserName = new Label();
            btnDangXuat = new Button();
            btnVaoPhong = new Button();
            txtTenPhong = new TextBox();
            btnTao = new Button();
            btnHuy = new Button();
            pnlTaoPhong = new Panel();
            label1 = new Label();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLobby).BeginInit();
            pnlTaoPhong.SuspendLayout();
            SuspendLayout();
            // 
            // panel4
            // 
            panel4.Controls.Add(btnLamMoi);
            panel4.Controls.Add(btnTaoPhong);
            panel4.Controls.Add(dgvLobby);
            panel4.Controls.Add(lblUserName);
            panel4.Controls.Add(btnDangXuat);
            panel4.Controls.Add(btnVaoPhong);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(4, 5, 4, 5);
            panel4.Name = "panel4";
            panel4.Size = new Size(763, 818);
            panel4.TabIndex = 2;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = Color.FromArgb(0, 192, 0);
            btnLamMoi.BackgroundImageLayout = ImageLayout.Zoom;
            btnLamMoi.FlatStyle = FlatStyle.Popup;
            btnLamMoi.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLamMoi.ForeColor = Color.Navy;
            btnLamMoi.Location = new Point(111, 723);
            btnLamMoi.Margin = new Padding(4, 5, 4, 5);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(157, 67);
            btnLamMoi.TabIndex = 9;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnTaoPhong
            // 
            btnTaoPhong.BackColor = Color.Navy;
            btnTaoPhong.FlatStyle = FlatStyle.Popup;
            btnTaoPhong.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnTaoPhong.ForeColor = SystemColors.Window;
            btnTaoPhong.Location = new Point(350, 723);
            btnTaoPhong.Margin = new Padding(4, 5, 4, 5);
            btnTaoPhong.Name = "btnTaoPhong";
            btnTaoPhong.Size = new Size(157, 67);
            btnTaoPhong.TabIndex = 4;
            btnTaoPhong.Text = "Tạo phòng";
            btnTaoPhong.UseVisualStyleBackColor = false;
            btnTaoPhong.Click += btnTaoPhong_Click_1;
            // 
            // dgvLobby
            // 
            dgvLobby.AllowUserToAddRows = false;
            dgvLobby.AllowUserToDeleteRows = false;
            dgvLobby.AllowUserToResizeColumns = false;
            dgvLobby.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dgvLobby.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvLobby.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLobby.BackgroundColor = SystemColors.Window;
            dgvLobby.BorderStyle = BorderStyle.None;
            dgvLobby.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLobby.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.SteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvLobby.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvLobby.ColumnHeadersHeight = 40;
            dgvLobby.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvLobby.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colHost, colPlayers, colStatus });
            dgvLobby.EnableHeadersVisualStyles = false;
            dgvLobby.Location = new Point(17, 83);
            dgvLobby.Margin = new Padding(4, 5, 4, 5);
            dgvLobby.MultiSelect = false;
            dgvLobby.Name = "dgvLobby";
            dgvLobby.ReadOnly = true;
            dgvLobby.RowHeadersVisible = false;
            dgvLobby.RowHeadersWidth = 62;
            dgvLobby.RowTemplate.Height = 35;
            dgvLobby.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLobby.Size = new Size(729, 612);
            dgvLobby.TabIndex = 5;
            // 
            // colId
            // 
            colId.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colId.HeaderText = "ID";
            colId.MinimumWidth = 8;
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Visible = false;
            // 
            // colName
            // 
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.HeaderText = "Tên phòng";
            colName.MinimumWidth = 8;
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.Visible = false;
            // 
            // colHost
            // 
            colHost.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colHost.HeaderText = "Chủ phòng";
            colHost.MinimumWidth = 8;
            colHost.Name = "colHost";
            colHost.ReadOnly = true;
            colHost.Visible = false;
            // 
            // colPlayers
            // 
            colPlayers.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colPlayers.HeaderText = "Người chơi";
            colPlayers.MinimumWidth = 8;
            colPlayers.Name = "colPlayers";
            colPlayers.ReadOnly = true;
            colPlayers.Visible = false;
            // 
            // colStatus
            // 
            colStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colStatus.HeaderText = "Trạng thái";
            colStatus.MinimumWidth = 8;
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Visible = false;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblUserName.Location = new Point(17, 32);
            lblUserName.Margin = new Padding(4, 0, 4, 0);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(106, 32);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "xin chào";
            // 
            // btnDangXuat
            // 
            btnDangXuat.BackColor = Color.Maroon;
            btnDangXuat.FlatStyle = FlatStyle.Popup;
            btnDangXuat.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnDangXuat.ForeColor = Color.White;
            btnDangXuat.Location = new Point(609, 18);
            btnDangXuat.Margin = new Padding(4, 5, 4, 5);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(137, 48);
            btnDangXuat.TabIndex = 1;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = false;
            btnDangXuat.Click += btnDangXuat_Click_1;
            // 
            // btnVaoPhong
            // 
            btnVaoPhong.BackColor = Color.Teal;
            btnVaoPhong.FlatStyle = FlatStyle.Popup;
            btnVaoPhong.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnVaoPhong.ForeColor = Color.White;
            btnVaoPhong.Location = new Point(589, 723);
            btnVaoPhong.Margin = new Padding(4, 5, 4, 5);
            btnVaoPhong.Name = "btnVaoPhong";
            btnVaoPhong.Size = new Size(157, 67);
            btnVaoPhong.TabIndex = 2;
            btnVaoPhong.Text = "Vào phòng";
            btnVaoPhong.UseVisualStyleBackColor = false;
            btnVaoPhong.Click += btnVaoPhong_Click;
            // 
            // txtTenPhong
            // 
            txtTenPhong.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTenPhong.Location = new Point(17, 55);
            txtTenPhong.Margin = new Padding(4, 5, 4, 5);
            txtTenPhong.Name = "txtTenPhong";
            txtTenPhong.Size = new Size(263, 39);
            txtTenPhong.TabIndex = 3;
            // 
            // btnTao
            // 
            btnTao.BackColor = Color.Lime;
            btnTao.FlatStyle = FlatStyle.Popup;
            btnTao.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnTao.ForeColor = Color.Navy;
            btnTao.Location = new Point(401, 37);
            btnTao.Margin = new Padding(4, 5, 4, 5);
            btnTao.Name = "btnTao";
            btnTao.Size = new Size(157, 67);
            btnTao.TabIndex = 4;
            btnTao.Text = "Xác nhận";
            btnTao.UseVisualStyleBackColor = false;
            btnTao.Click += btnTao_Click;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Red;
            btnHuy.FlatStyle = FlatStyle.Popup;
            btnHuy.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(589, 37);
            btnHuy.Margin = new Padding(4, 5, 4, 5);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(157, 67);
            btnHuy.TabIndex = 5;
            btnHuy.Text = "Huỷ";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // pnlTaoPhong
            // 
            pnlTaoPhong.Controls.Add(label1);
            pnlTaoPhong.Controls.Add(btnHuy);
            pnlTaoPhong.Controls.Add(btnTao);
            pnlTaoPhong.Controls.Add(txtTenPhong);
            pnlTaoPhong.Dock = DockStyle.Bottom;
            pnlTaoPhong.Location = new Point(0, 850);
            pnlTaoPhong.Margin = new Padding(4, 5, 4, 5);
            pnlTaoPhong.Name = "pnlTaoPhong";
            pnlTaoPhong.Size = new Size(763, 142);
            pnlTaoPhong.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(17, 7);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(135, 32);
            label1.TabIndex = 6;
            label1.Text = "Tên phòng:";
            // 
            // FormLobby
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientInactiveCaption;
            ClientSize = new Size(763, 992);
            Controls.Add(pnlTaoPhong);
            Controls.Add(panel4);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "FormLobby";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Caro Online";
            FormClosing += FormLobby_FormClosing;
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLobby).EndInit();
            pnlTaoPhong.ResumeLayout(false);
            pnlTaoPhong.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel4;
        private Button btnTaoPhong;
        private Button btnDangXuat;
        private Label lblUserName;
        private Button btnVaoPhong;
        private DataGridView dgvLobby;
        private Button btnLamMoi;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colHost;
        private DataGridViewTextBoxColumn colPlayers;
        private DataGridViewTextBoxColumn colStatus;
        private TextBox txtTenPhong;
        private Button btnTao;
        private Button btnHuy;
        private Panel pnlTaoPhong;
        private Label label1;
    }
}