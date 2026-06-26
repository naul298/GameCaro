using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace FormGiaoDienGame
{
    public partial class FormLogin : Form
    {
        private SocketManager socket = new SocketManager();

        public FormLogin()
        {
            InitializeComponent();
            LoadServerList();
        }

        // Load IP máy mình vào cboServer
        private void LoadServerList()
        {
            cboServer.Items.Clear();

            string wifi = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            string ethernet = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);

            if (!string.IsNullOrEmpty(wifi)) cboServer.Items.Add(wifi);
            if (!string.IsNullOrEmpty(ethernet)) cboServer.Items.Add(ethernet);

            if (cboServer.Items.Count > 0)
                cboServer.SelectedIndex = 0;
        }
        // Quét LAN dựa trên subnet của các IP đã load
        private void TimLan(string subnet)
        {
            Task.Run(() =>
            {
                var found = new List<string>();

                var tasks = Enumerable.Range(1, 254).Select(i => Task.Run(() =>
                {
                    string ip = subnet + i;
                    try
                    {
                        using var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        bool connected = s.ConnectAsync(new IPEndPoint(IPAddress.Parse(ip), 12346)).Wait(500);
                        if (!connected) return;

                        s.Send(Encoding.UTF8.GetBytes("FIND_SERVER"));

                        byte[] buf = new byte[64];
                        int n = s.Receive(buf);

                        if (Encoding.UTF8.GetString(buf, 0, n) == "CARO_SERVER")
                            lock (found) { found.Add(ip); }
                    }
                    catch { }
                })).ToArray();

                Task.WaitAll(tasks, 3000);
                CapNhatUISauKhiQuet(found);
            });
        }
        // Cập nhật UI sau khi quét xong
        private void CapNhatUISauKhiQuet(List<string> found)
        {
            this.Invoke(() =>
            {
                if (found.Count > 0)
                {
                    foreach (var ip in found)
                        if (!cboServer.Items.Contains(ip))
                            cboServer.Items.Add(ip);

                    string serverIp = found[0];
                    cboServer.Text = serverIp;
                    socket = new SocketManager { IP = serverIp };
                    socket.KetNoiServer();

                    lblStatus.ForeColor = Color.Green;
                    lblStatus.Text = $"Kết nối thành công: {serverIp}";
                }
                else
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = $"Không tìm thấy server: {cboServer.Text.Trim()}";
                }
            });
        }
        // Nút Truy cập — quét LAN theo subnet của IP đang chọn rồi kết nối
        private void btnKetNoi_Click(object sender, EventArgs e)
        {
            string selectedIp = cboServer.Text.Trim();
            if (string.IsNullOrEmpty(selectedIp)) return;

            // Lấy subnet từ IP đang chọn rồi quét LAN
            string subnet = selectedIp.Substring(0, selectedIp.LastIndexOf('.') + 1);
            TimLan(subnet);
        }
        private void btnDangKi_Click(object sender, EventArgs e)
        {
            var formRegister = new FormRegister(cboServer.Text.Trim());
            formRegister.Show();
            this.Hide();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Vui lòng nhập đủ thông tin.";
                return;
            }

            if (socket == null || !socket.KetNoiServer())
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Chưa kết nối server. Vui lòng kết nối đến Server!";
                return;
            }

            lblStatus.ForeColor = Color.Gray;
            lblStatus.Text = "Đang đăng nhập...";
            btnDangNhap.Enabled = false;

            // Gửi gói LOGIN
            var loginData = new SocketData((int)SocketCommand.LOGIN, $"{tenDangNhap}|{matKhau}", new Point(0, 0));
            socket.Send(loginData);
            Task.Run(() =>
            {
                var response = socket.Receive() as SocketData;

                this.Invoke(() =>
                {
                    btnDangNhap.Enabled = true;

                    if (response == null)
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = "Lỗi phản hồi từ server.";
                        return;
                    }

                    if (response.Command == (int)SocketCommand.LOGIN_OK)
                    {
                        string displayName = response.Message;

                        lblStatus.ForeColor = Color.Green; lblStatus.Text = "Xin chao " + displayName;
                        var formLobby = new FormLobby(socket, displayName);
                        formLobby.Show();
                        this.Hide();
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = $"{response.Message}";
                    }
                });
            });
        }
    }
}