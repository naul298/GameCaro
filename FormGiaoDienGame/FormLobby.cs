using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FormGiaoDienGame
{
    public partial class FormLobby : Form
    {
        private readonly SocketManager _socket;
        private readonly string _displayName;

        // Cache danh sách phòng hiện tại
        private List<RoomInfo> _rooms = new();

        public FormLobby(SocketManager socket, string displayName)
        {
            InitializeComponent();
            _socket = socket;
            _displayName = displayName;

            lblUserName.Text = $"Xin chào: {displayName}";
            pnlTaoPhong.Visible = false;
            SetupGrid();

            // Đăng ký nhận gói từ SocketManager — chỉ 1 thread nhận duy nhất
            _socket.OnDataReceived += OnSocketData;
            XinDanhSachPhong();
        }
        // ── Nhận gói từ SocketManager ────────────────────────────────────
        // Chạy trên thread nhận của SocketManager → cần Invoke để cập nhật UI
        private void OnSocketData(SocketData data)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            this.Invoke(() => ProcessData(data));
        }
        // Hủy đăng ký — gọi trước khi chuyển sang FormGame hoặc đóng form
        private void StopListen()
        {
            _socket.OnDataReceived -= OnSocketData;
        }
        // ── Cấu hình cột DataGridView ────────────────────────────────────
        private void SetupGrid()
        {
            dgvLobby.AutoGenerateColumns = false;
            dgvLobby.AllowUserToAddRows = false;
            dgvLobby.ReadOnly = true;
            dgvLobby.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvLobby.Columns.Clear();
            dgvLobby.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", Width = 40 });
            dgvLobby.Columns.Add(new DataGridViewTextBoxColumn { Name = "colName", HeaderText = "Tên phòng", Width = 150 });
            dgvLobby.Columns.Add(new DataGridViewTextBoxColumn { Name = "colHost", HeaderText = "Chủ phòng", Width = 120 });
            dgvLobby.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPlayers", HeaderText = "Người", Width = 60 });
            dgvLobby.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Trạng thái", Width = 80 });
        }

        // ── Gửi yêu cầu lấy danh sách phòng ─────────────────────────────
        private void XinDanhSachPhong()
        {
            _socket.Send(new SocketData((int)SocketCommand.GET_ROOMS, "", new Point()));
        }

        // ── Hiển thị danh sách phòng lên grid ───────────────────────────
        private void HienThiPhong(List<RoomInfo> rooms)
        {
            if (dgvLobby.InvokeRequired)
            {
                dgvLobby.Invoke(() => HienThiPhong(rooms));
                return;
            }

            dgvLobby.Rows.Clear();

            foreach (var r in rooms)
            {
                int rowIndex = dgvLobby.Rows.Add();
                DataGridViewRow row = dgvLobby.Rows[rowIndex];

                row.Cells["colId"].Value = r.Id;
                row.Cells["colName"].Value = r.Name;
                row.Cells["colHost"].Value = string.IsNullOrEmpty(r.HostName) ? "(trống)" : r.HostName;
                row.Cells["colPlayers"].Value = $"{r.PlayerCount}/2";
                row.Cells["colStatus"].Value = r.IsFull ? "Đang chơi" : "Chờ";

                if (r.IsFull)
                    row.DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        // ── Xử lý gói tin nhận được ─────────────────────────────────────
        private void ProcessData(SocketData data)
        {
            switch ((SocketCommand)data.Command)
            {
                // ── Danh sách phòng ─────────────────────────────────────
                case SocketCommand.ROOMS_LIST:
                    // Nhận toàn bộ danh sách lần đầu khi vào lobby
                    _rooms = JsonSerializer.Deserialize<List<RoomInfo>>(data.Message) ?? new List<RoomInfo>();
                    HienThiPhong(_rooms);
                    break;

                case SocketCommand.ROOM_UPDATE:
                    // Cập nhật 1 phòng — thêm mới nếu chưa có, sửa nếu đã có
                    var updated = JsonSerializer.Deserialize<RoomInfo>(data.Message);
                    if (updated != null)
                    {
                        int idx = _rooms.FindIndex(r => r.Id == updated.Id);
                        if (idx >= 0) _rooms[idx] = updated;
                        else _rooms.Add(updated);
                        HienThiPhong(_rooms);
                    }
                    break;

                case SocketCommand.ROOM_DELETED:
                    // Xóa phòng khỏi danh sách khi phòng bị giải thể
                    int deletedId = int.Parse(data.Message);
                    _rooms.RemoveAll(r => r.Id == deletedId);
                    HienThiPhong(_rooms);
                    break;

                // ── Vào phòng ───────────────────────────────────────────
                case SocketCommand.JOIN_OK:
                    var parts = data.Message.Split('|');
                    string roomName = parts[0];
                    int playerIndex = int.Parse(parts[1]);
                    string myName = parts[2];
                    string opponentName = parts.Length > 3 ? parts[3] : "";

                    var formGame = new FormGame(_socket, myName, playerIndex, opponentName, roomName);
                    formGame.Show();      // Show TRƯỚC → IsHandleCreated = true
                    StopListen();         // Hủy Lobby SAU → không còn khoảng trống chết
                    this.Hide();
                    break;

                case SocketCommand.JOIN_FAIL:
                    MessageBox.Show(data.Message, "Không thể vào phòng");
                    break;
            }
        }
        // ── Button handlers ──────────────────────────────────────────────
        private void btnDangXuat_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Đăng xuất?", "Thông báo", MessageBoxButtons.OKCancel) != DialogResult.OK) return;
            StopListen();
            _socket.Close();
            this.Close();
        }
        private void btnTaoPhong_Click_1(object sender, EventArgs e)
        {
            txtTenPhong.Clear();
            pnlTaoPhong.Visible = true;
            txtTenPhong.Focus();
        }

        private void btnVaoPhong_Click(object sender, EventArgs e)
        {
            if (dgvLobby.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phòng trong danh sách trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow row = dgvLobby.SelectedRows[0];

            // Tên cột khớp với SetupGrid() đã khai báo: "colId"
            object? idValue = row.Cells["colId"].Value;
            if (idValue == null) return;

            int roomId = Convert.ToInt32(idValue);

            _socket.Send(new SocketData((int)SocketCommand.JOIN_ROOM, roomId.ToString(), new Point()));
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            XinDanhSachPhong();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            pnlTaoPhong.Visible = false;
            txtTenPhong.Clear();
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            string tenPhong = txtTenPhong.Text.Trim();

            if (string.IsNullOrEmpty(tenPhong))
            {
                MessageBox.Show("Tên phòng không được để trống", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenPhong.Focus();
                return;
            }

            // Ẩn panel trước khi gửi
            pnlTaoPhong.Visible = false;
            txtTenPhong.Clear();

            _socket.Send(new SocketData(
                (int)SocketCommand.CREATE_ROOM,
                tenPhong,
                new Point()));
        }

        private void FormLobby_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Chỉ đóng socket khi form thực sự đóng (đăng xuất)
            StopListen();
            try
            {
                if (_socket.IsConnected)
                    _socket.Send(new SocketData((int)SocketCommand.LEAVE_ROOM, "", new Point()));
                _socket.Close();
            }
            catch { }
        }
        private void FormLobby_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible) return;

            // Đăng ký lại khi quay về lobby từ FormGame
            _socket.OnDataReceived -= OnSocketData; // tránh đăng ký trùng
            _socket.OnDataReceived += OnSocketData;

            Task.Delay(200).ContinueWith(_ => this.Invoke(XinDanhSachPhong));
        }
    }
}