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
            Listen();
            XinDanhSachPhong();
        }

        // Cấu hình cột cho DataGridView
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

        // Gửi yêu cầu lấy danh sách phòng
        private void XinDanhSachPhong()
        {
            _socket.Send(new SocketData((int)SocketCommand.GET_ROOMS, "", new Point()));
        }

        //Hiển thị danh sách phòng lên grid
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
        private void Listen()
        {
            var t = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        var data = _socket.Receive() as SocketData;
                        if (data == null)
                        {
                            break;
                        }
                        Console.WriteLine($"[DEBUG] Nhận lệnh: {(SocketCommand)data.Command}");
                        this.Invoke(() => ProcessData(data));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[DEBUG] Listen() lỗi: {ex.Message}");
                        break;
                    }
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
                    _rooms = JsonSerializer.Deserialize<List<RoomInfo>>(data.Message) ?? new List<RoomInfo>();
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
                    var parts = data.Message.Split('|');
                    string roomName = parts[0];
                    int playerIndex = int.Parse(parts[1]);
                    string myName = parts[2];
                    string opponentName = parts.Length > 3 ? parts[3] : "";

                    var formGame = new FormGame(_socket, myName, playerIndex, opponentName, roomName);
                    formGame.Show();
                    this.Hide();

                    break;

                case SocketCommand.JOIN_FAIL:
                    MessageBox.Show(data.Message, "Không thể vào phòng");
                    break;
            }
        }

        private void btnDangXuat_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Đăng xuất?", "Thông báo",
                MessageBoxButtons.OKCancel) != DialogResult.OK) return;

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

            _socket.Send(new SocketData(
                (int)SocketCommand.JOIN_ROOM,
                roomId.ToString(),
                new Point()));
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
    }
}