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
            textBox1.PasswordChar = '●'; // ô xác nhận mật khẩu

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
                var regSocket = new SocketManager { IP = _serverIp };
                bool connected = regSocket.KetNoiServer();

                if (!connected)
                {
                    this.Invoke(() =>
                    {
                        btnDangKi.Enabled = true;
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "✘ Không kết nối được server.";
                    });
                    return;
                }

                // Gửi gói REGISTER — format: "username|password|displayName"
                regSocket.Send(new SocketData(
                    (int)SocketCommand.REGISTER,
                    $"{username}|{password}|{displayName}",
                    new Point(0, 0)));

                var response = regSocket.Receive() as SocketData;

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
                        MessageBox.Show(
                            $"Đăng ký thành công!\n" +
                            $"Tài khoản: {username}\n" +
                            $"Tên hiển thị: {displayName}\n\n" +
                            $"Bạn có thể đăng nhập ngay bây giờ.",
                            "Thành công",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        // Đóng FormRegister → FormLogin tự hiện lại
                        this.Close();
                    }
                    else if (response.Command == (int)SocketCommand.REGISTER_FAIL)
                    {
                        // Server báo tên tài khoản đã tồn tại hoặc lỗi khác
                        if (response.Message.Contains("tồn tại", StringComparison.OrdinalIgnoreCase)
                            || response.Message.Contains("exist", StringComparison.OrdinalIgnoreCase))
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "✘ Tên tài khoản đã tồn tại.";
                        }
                        else
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = $"✘ {response.Message}";
                        }
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "✘ Phản hồi không hợp lệ từ server.";
                    }
                });
            });
        }

        // ── Đóng FormRegister → FormLogin tự show lại ────────────
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            var login = Application.OpenForms.OfType<FormLogin>().FirstOrDefault();
            if (login != null)
                login.Show();
            else
                new FormLogin().Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
