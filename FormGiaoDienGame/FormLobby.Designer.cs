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
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            panel4 = new Panel();
            btnLamMoi = new Button();
            dgvRooms = new DataGridView();
            lblUserName = new Label();
            btnDangXuat = new Button();
            btnTaoPhong = new Button();
            btnVaoPhong = new Button();
            btnSearch = new Button();
            txtSearch = new TextBox();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRooms).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.InactiveCaption;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(534, 68);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.BackColor = SystemColors.GradientInactiveCaption;
            label1.Font = new Font("Segoe UI Semibold", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(510, 45);
            label1.TabIndex = 6;
            label1.Text = "Sảnh chính";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Location = new Point(3, 67);
            panel2.Name = "panel2";
            panel2.Size = new Size(542, 441);
            panel2.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(txtSearch);
            panel4.Controls.Add(btnSearch);
            panel4.Controls.Add(btnLamMoi);
            panel4.Controls.Add(dgvRooms);
            panel4.Controls.Add(lblUserName);
            panel4.Controls.Add(btnDangXuat);
            panel4.Controls.Add(btnTaoPhong);
            panel4.Controls.Add(btnVaoPhong);
            panel4.Location = new Point(0, 71);
            panel4.Name = "panel4";
            panel4.Size = new Size(534, 528);
            panel4.TabIndex = 2;
            // 
            // btnLamMoi
            // 
            btnLamMoi.BackColor = SystemColors.Control;
            btnLamMoi.BackgroundImageLayout = ImageLayout.Zoom;
            btnLamMoi.FlatStyle = FlatStyle.Popup;
            btnLamMoi.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLamMoi.Location = new Point(424, 34);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(88, 29);
            btnLamMoi.TabIndex = 9;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            // 
            // dgvRooms
            // 
            dgvRooms.AllowUserToAddRows = false;
            dgvRooms.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.WhiteSmoke;
            dgvRooms.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvRooms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRooms.BackgroundColor = SystemColors.Window;
            dgvRooms.BorderStyle = BorderStyle.None;
            dgvRooms.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRooms.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.SteelBlue;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvRooms.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvRooms.ColumnHeadersHeight = 40;
            dgvRooms.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRooms.EnableHeadersVisualStyles = false;
            dgvRooms.Location = new Point(12, 87);
            dgvRooms.MultiSelect = false;
            dgvRooms.Name = "dgvRooms";
            dgvRooms.ReadOnly = true;
            dgvRooms.RowHeadersVisible = false;
            dgvRooms.RowTemplate.Height = 35;
            dgvRooms.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRooms.Size = new Size(510, 367);
            dgvRooms.TabIndex = 5;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblUserName.Location = new Point(12, 0);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(70, 21);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "xin chào";
            // 
            // btnDangXuat
            // 
            btnDangXuat.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnDangXuat.Location = new Point(372, 476);
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Size = new Size(130, 40);
            btnDangXuat.TabIndex = 1;
            btnDangXuat.Text = "Đăng xuất";
            btnDangXuat.UseVisualStyleBackColor = true;
            // 
            // btnTaoPhong
            // 
            btnTaoPhong.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnTaoPhong.Location = new Point(202, 476);
            btnTaoPhong.Name = "btnTaoPhong";
            btnTaoPhong.Size = new Size(130, 40);
            btnTaoPhong.TabIndex = 4;
            btnTaoPhong.Text = "Tạo phòng";
            btnTaoPhong.UseVisualStyleBackColor = true;
            // 
            // btnVaoPhong
            // 
            btnVaoPhong.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnVaoPhong.Location = new Point(32, 476);
            btnVaoPhong.Name = "btnVaoPhong";
            btnVaoPhong.Size = new Size(130, 40);
            btnVaoPhong.TabIndex = 2;
            btnVaoPhong.Text = "Vào phòng";
            btnVaoPhong.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(257, 45);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 10;
            btnSearch.Text = "button1";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(176, 40);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(8, 23);
            txtSearch.TabIndex = 11;
            // 
            // FormLobby
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 611);
            Controls.Add(panel4);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormLobby";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Caro Online";
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRooms).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel4;
        private Button btnTaoPhong;
        private Button btnDangXuat;
        private Label lblUserName;
        private Button btnVaoPhong;
        private Label label1;
        private DataGridView dgvRooms;
        private Button btnLamMoi;
        private TextBox txtSearch;
        private Button btnSearch;
    }
}