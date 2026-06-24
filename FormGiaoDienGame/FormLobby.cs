using System.Text.Json;

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

            lblUserName.Text = $"Xin chào, {displayName}";

            // Gắn events
            btnTaoPhong.Click += BtnTaoPhong_Click;
            btnVaoPhong.Click += BtnVaoPhong_Click;
            btnDangXuat.Click += BtnDangXuat_Click;
            btnSearch.Click += BtnSearch_Click;
            txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnSearch_Click(s, e); };
            FormClosing += FormLobby_FormClosing;

            SetupGrid();
            XinDanhSachPhong();
            Listen();
        }

        // Cấu hình cột cho DataGridView
        private void SetupGrid()
        {
            dgvRooms.Columns.Clear();
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colId", HeaderText = "ID", Width = 50, ReadOnly = true });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colName", HeaderText = "Tên phòng", FillWeight = 50, ReadOnly = true });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colHost", HeaderText = "Chủ phòng", FillWeight = 30, ReadOnly = true });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colPlayers", HeaderText = "Người chơi", Width = 90, ReadOnly = true });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn
            { Name = "colStatus", HeaderText = "Trạng thái", Width = 90, ReadOnly = true });
        }

        // Gửi yêu cầu lấy danh sách phòng
        private void XinDanhSachPhong()
        {
            _socket.Send(new SocketData((int)SocketCommand.GET_ROOMS, "", new Point()));
        }

        // Hiển thị danh sách phòng lên grid
        private void HienThiPhong(List<RoomInfo> rooms)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            var filtered = string.IsNullOrEmpty(keyword)
                ? rooms
                : rooms.Where(r => r.Name.ToLower().Contains(keyword)).ToList();

            dgvRooms.Rows.Clear();
            foreach (var r in filtered)
            {
                dgvRooms.Rows.Add(
                    r.Id,
                    r.Name,
                    string.IsNullOrEmpty(r.HostName) ? "(trống)" : r.HostName,
                    $"{r.PlayerCount}/2",
                    r.IsFull ? "Đang chơi" : "Chờ"
                );
                // Tô màu dòng đang chơi
                var row = dgvRooms.Rows[dgvRooms.Rows.Count - 1];
                if (r.IsFull)
                    row.DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        // ── BUTTON HANDLERS ─────────────────────────────────────

        private void BtnTaoPhong_Click(object? sender, EventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập tên phòng:", "Tạo phòng mới", $"Phòng của {_displayName}");
            if (string.IsNullOrWhiteSpace(name)) return;

            _socket.Send(new SocketData((int)SocketCommand.CREATE_ROOM, name, new Point()));
        }

        private void BtnVaoPhong_Click(object? sender, EventArgs e)
        {
            if (dgvRooms.CurrentRow == null) return;

            int roomId = Convert.ToInt32(dgvRooms.CurrentRow.Cells["colId"].Value);
            bool isFull = dgvRooms.CurrentRow.Cells["colStatus"].Value?.ToString() == "Đang chơi";

            if (isFull)
            {
                MessageBox.Show("Phòng đã đầy!", "Thông báo");
                return;
            }

            _socket.Send(new SocketData((int)SocketCommand.JOIN_ROOM,
                roomId.ToString(), new Point()));
        }

        private void BtnDangXuat_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Đăng xuất?", "Thông báo",
                MessageBoxButtons.OKCancel) != DialogResult.OK) return;

            this.Close();
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            HienThiPhong(_rooms);
        }

        // ── LISTEN ──────────────────────────────────────────────

        private void Listen()
        {
            var t = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var data = _socket.Receive() as SocketData;
                        if (data == null) break;
                        this.Invoke(() => ProcessData(data));
                    }
                    catch { break; }
                }
            });
            t.IsBackground = true;
            t.Start();
        }

        private void ProcessData(SocketData data)
        {
            switch ((SocketCommand)data.Command)
            {
                case SocketCommand.ROOMS_LIST:
                    // Nhận toàn bộ danh sách lần đầu
                    _rooms = JsonSerializer.Deserialize<List<RoomInfo>>(data.Message)
                             ?? new List<RoomInfo>();
                    HienThiPhong(_rooms);
                    break;

                case SocketCommand.ROOM_UPDATE:
                    // Cập nhật 1 phòng
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
                    // Xóa phòng khỏi danh sách
                    int deletedId = int.Parse(data.Message);
                    _rooms.RemoveAll(r => r.Id == deletedId);
                    HienThiPhong(_rooms);
                    break;

                case SocketCommand.JOIN_OK:
                    // Vào phòng thành công → mở FormGame
                    // Format: "roomName|playerIndex|myName|opponentName"
                    var parts = data.Message.Split('|');
                    string roomName = parts[0];
                    int playerIndex = int.Parse(parts[1]);
                    string myName = parts[2];
                    string opponentName = parts.Length > 3 ? parts[3] : "";

                    if (string.IsNullOrEmpty(opponentName))
                    {
                        // Chỉ 1 mình → hiện thông báo chờ, không mở game
                        MessageBox.Show($"Đã vào '{roomName}'. Chờ người chơi thứ 2...",
                            "Đang chờ");
                        // Tiếp tục lắng nghe — khi đủ 2 sẽ nhận JOIN_OK lần 2
                    }
                    else
                    {
                        // Đủ 2 người → mở FormGame
                        var formGame = new FormGame(_socket, myName, playerIndex, opponentName);
                        formGame.Show();
                        this.Hide();
                    }
                    break;

                case SocketCommand.JOIN_FAIL:
                    MessageBox.Show(data.Message, "Không thể vào phòng");
                    break;
            }
        }

        private void FormLobby_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _socket.Send(new SocketData((int)SocketCommand.LEAVE_ROOM, "", new Point()));
            Application.Exit();
        }
    }

    // DTO để deserialize JSON danh sách phòng
    public class RoomInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Status { get; set; } = "";
        public string HostName { get; set; } = "";
        public int PlayerCount { get; set; }
        public bool IsFull { get; set; }
    }
}