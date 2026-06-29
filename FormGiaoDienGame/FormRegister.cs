namespace FormGiaoDienGame
{
    public partial class FormRegister : Form
    {
        private readonly string _serverIp;

        public FormRegister(string serverIp)
        {
            InitializeComponent();
            _serverIp = serverIp;

            // Ẩn mật khẩu
            txtMatKhau.PasswordChar = '●';

            // Hiển thị server đang kết nối
            lblServer.Text = $"Đang truy cập vào server: {_serverIp}";

            lblStatus.ForeColor = SystemColors.ControlDarkDark;
            lblStatus.Text = "Điền thông tin để đăng ký.";
        }

        // ── Nút Đăng kí ──────────────────────────────────────────
        private void btnDangKi_Click(object sender, EventArgs e)
        {
            string displayName = textBox1.Text.Trim();        // Họ và tên (textBox1)
            string username = txtTenDangNhap.Text.Trim();  // Tên đăng nhập
            string password = txtMatKhau.Text.Trim();      // Mật khẩu

            // ── Validation: không được để trống ──────────────────
            if (string.IsNullOrEmpty(displayName)
                || string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password))
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "✘ Vui lòng điền đầy đủ thông tin.";
                return;
            }

            // ── Gửi yêu cầu đăng ký lên server ──────────────────
            lblStatus.ForeColor = Color.Gray;
            lblStatus.Text = "Đang đăng ký...";
            btnDangKi.Enabled = false;

            Task.Run(() =>
            {
                try
                {
                    // Tạo raw socket — kết nối, gửi, nhận rồi đóng luôn
                    using var raw = new System.Net.Sockets.Socket(
                        System.Net.Sockets.AddressFamily.InterNetwork,
                        System.Net.Sockets.SocketType.Stream,
                        System.Net.Sockets.ProtocolType.Tcp);

                    raw.Connect(_serverIp, 12345);

                    string json = System.Text.Json.JsonSerializer.Serialize(
                        new SocketData((int)SocketCommand.REGISTER,
                                       $"{username}|{password}|{displayName}",
                                       new Point(0, 0)));

                    raw.Send(System.Text.Encoding.UTF8.GetBytes(json));

                    // Nhận phản hồi
                    byte[] buf = new byte[4096];
                    int n = raw.Receive(buf);
                    string responseJson = System.Text.Encoding.UTF8.GetString(buf, 0, n);
                    var response = System.Text.Json.JsonSerializer.Deserialize<SocketData>(responseJson);

                    this.Invoke(() =>
                    {
                        btnDangKi.Enabled = true;

                        if (response == null)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "✘ Không nhận được phản hồi từ server.";
                            return;
                        }

                        if (response.Command == (int)SocketCommand.REGISTER_OK)
                        {
                            MessageBox.Show($"Đăng ký thành công!\n" +
                                $"Tài khoản: {username}\n" +
                                $"Tên hiển thị: {displayName}\n\n" +
                                $"Bạn có thể đăng nhập ngay bây giờ.",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else if (response.Command == (int)SocketCommand.REGISTER_FAIL)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = response.Message.Contains("tồn tại", StringComparison.OrdinalIgnoreCase)
                                ? "✘ Tên tài khoản đã tồn tại."
                                : $"✘ {response.Message}";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "✘ Phản hồi không hợp lệ từ server.";
                        }
                    });
                }
                catch (Exception ex)
                {
                    this.Invoke(() =>
                    {
                        btnDangKi.Enabled = true;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = $"✘ Lỗi: {ex.Message}";
                    });
                }
            });
        }

        // ── Đóng FormRegister → FormLogin tự show lại ────────────

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Tìm FormLogin đang bị ẩn và hiện lại — giữ nguyên IP đã nhập
            var login = Application.OpenForms.OfType<FormLogin>().FirstOrDefault();
            if (login != null) login.Show();
            else new FormLogin().Show(); // không tìm thấy → tạo mới
        }
    }
}
