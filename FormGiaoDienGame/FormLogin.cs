using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

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

        // Tự động load danh sách IP local vào ComboBox
        private void LoadServerList()
        {
            txtServer.Items.Clear();

            string wifi = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            string ethernet = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);

            if (!string.IsNullOrEmpty(wifi)) txtServer.Items.Add(wifi);
            if (!string.IsNullOrEmpty(ethernet)) txtServer.Items.Add(ethernet);

            if (txtServer.Items.Count > 0)
                txtServer.SelectedIndex = 0;

            lblStatus.ForeColor = SystemColors.ControlDarkDark;
            lblStatus.Text = "Chọn server để kết nối.";
        }

        // Nút kính lúp — tìm kiếm server
        private void btnKetNoi_Click(object sender, EventArgs e)
        {
            string ip = txtServer.Text.Trim();

            if (string.IsNullOrEmpty(ip))
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Vui lòng nhập địa chỉ server.";
                return;
            }

            lblStatus.ForeColor = Color.Gray;
            lblStatus.Text = "Đang tìm server...";
            btnKetNoi.Enabled = false;

            Task.Run(() =>
            {
                socket = new SocketManager();
                socket.IP = ip;
                bool connected = socket.KetNoiServer();

                this.Invoke(() =>
                {
                    btnKetNoi.Enabled = true;

                    if (connected)
                    {
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = $"✔ Kết nối thành công: {ip}";

                        // Lưu IP vào ComboBox nếu chưa có
                        if (!txtServer.Items.Contains(ip))
                            txtServer.Items.Add(ip);
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = $"✘ Không tìm thấy server: {ip}";
                    }
                });
            });
        }

        // Nút Đăng nhập
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
                lblStatus.Text = "Chưa kết nối server. Nhấn 🔍 trước.";
                return;
            }

            lblStatus.ForeColor = Color.Gray;
            lblStatus.Text = "Đang đăng nhập...";
            btnDangNhap.Enabled = false;

            // Gửi gói LOGIN
            var loginData = new SocketData(
                (int)SocketCommand.LOGIN,
                $"{tenDangNhap}|{matKhau}",
                new Point(0, 0)
            );
            socket.Send(loginData);

            // Nhận phản hồi
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
                        string displayName = response.Message; // Server chỉ gửi displayName

                        SetStatus(Color.Green, $"✔ Xin chào {displayName}!");

                        var formLobby = new FormLobby(_socket, displayName);
                        formLobby.Show();
                        this.Hide();
                    }
                    else
                    {
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = $"✘ {response.Message}";
                    }
                });
            });
        }

        // Nút Đăng kí (chưa implement)
        private void btnDangKi_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = Color.Gray;
            lblStatus.Text = "Chức năng đăng ký chưa khả dụng.";
        }
    }
}