namespace FormGiaoDienGame
{
    public partial class FormGame : Form
    {
        private QlyBanCo _banCo;
        private SocketManager _socket;
        private int _playerIndex;
        private bool _gameOver = false;
        private readonly Queue<SocketData> _pendingData = new();
        private bool _handleReady = false;


        public FormGame(SocketManager socket, string displayName, int playerIndex, string opponentName, string roomName)
        {
            InitializeComponent();

            _socket = socket;
            _playerIndex = playerIndex;

            string nameX = (playerIndex == 0) ? displayName : (string.IsNullOrEmpty(opponentName) ? "Người chơi 1" : opponentName);
            string nameO = (playerIndex == 1) ? displayName : (string.IsNullOrEmpty(opponentName) ? "Người chơi 2" : opponentName);

            _banCo = new QlyBanCo(pnlBanCo, lblPlayer1, lblPlayer2, lblStatus, nameX, nameO);
            _banCo.EndGame += BanCo_EndGame;
            _banCo.PlayerMark += BanCo_PlayerMark;
            _socket.OnDataReceived += OnSocketData;

            lblSoPhong.Text = roomName;
            prcbCoolDown.Step = Cons.coolDownStep;
            prcbCoolDown.Maximum = Cons.coolDownTime;
            prcbCoolDown.Value = 0;
            tmCoolDown.Interval = Cons.coolDownInterval;

            _banCo.VeBanCo();

            pnlBanCo.Enabled = false;
            btnSanSang.Enabled = false;
            btnSanSang.Visible = true;
            btnChoiLai.Enabled = false;
            btnDauHang.Enabled = false;
            btnCauHoa.Enabled = false;

            if (string.IsNullOrEmpty(opponentName))
                lblStatus.Text = "Đang chờ đối thủ vào phòng...";
            else
                OnOpponentJoined(opponentName); // vào phòng đã có đủ 2 người
        }
        private void OnSocketData(SocketData data)
        {
            if (this.IsDisposed) return;

            if (!_handleReady)
            {
                lock (_pendingData) _pendingData.Enqueue(data); // queue lại thay vì drop
                return;
            }

            this.Invoke(() => ProcessData(data));
        }
        private void StopListen()
        {
            _socket.OnDataReceived -= OnSocketData;
        }
        private void OnOpponentJoined(string opponentName)
        {
            // Cập nhật tên đối thủ
            if (_playerIndex == 0) lblPlayer2.Text = opponentName;
            else lblPlayer1.Text = opponentName;

            lblStatus.Text = "Đủ 2 người! Bấm Sẵn sàng để bắt đầu.";
            btnSanSang.Enabled = true;
        }
        private void BatDauGame(int firstMover)
        {
            _gameOver = false;
            _banCo.VeBanCo();
            prcbCoolDown.Value = 0;

            btnDauHang.Enabled = true;
            btnCauHoa.Enabled = true;

            bool myTurn = (_playerIndex == firstMover);
            pnlBanCo.Enabled = myTurn;
            lblStatus.Text = myTurn ? "Đến lượt bạn!" : "Chờ đối thủ đánh...";
            if (myTurn) tmCoolDown.Start();
        }

        private void BanCo_PlayerMark(object? sender, ButtonClickEvent e)
        {
            pnlBanCo.Enabled = false;
            prcbCoolDown.Value = 0;
            _socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", e.ClickPoint));
        }

        private void BanCo_EndGame(object? sender, EventArgs e)
        {
            StopGame();


            string winner = _banCo.WinnerName;
            MessageBox.Show($"🏆 {winner} thắng!", "Kết thúc ván cờ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            lblStatus.Text = $"{winner} thắng!";

            // Gửi END để đối thủ biết mình thua
            _socket.Send(new SocketData((int)SocketCommand.END, winner, new Point()));
        }
        private void StopGame()
        {
            tmCoolDown.Stop();
            pnlBanCo.Enabled = false;
            _gameOver = true;
            btnChoiLai.Enabled = true;

            btnDauHang.Enabled = false;
            btnCauHoa.Enabled = false;
        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();
            if (prcbCoolDown.Value >= prcbCoolDown.Maximum)
            {
                StopGame();
                lblStatus.Text = "Hết giờ!";
                _socket.Send(new SocketData((int)SocketCommand.HET_GIO, "", new Point()));
            }
        }

        private void ProcessData(SocketData data)
        {
            switch (data.Command)
            {
                // ── Nước đi ─────────────────────────────────────────────
                case (int)SocketCommand.SEND_POINT:
                    prcbCoolDown.Value = 0;
                    tmCoolDown.Start();
                    pnlBanCo.Enabled = true;
                    lblStatus.Text = "Đến lượt bạn!";
                    _banCo.OtherPlayerMark(data.Point);
                    break;

                // ── Kết thúc ván ────────────────────────────────────────
                case (int)SocketCommand.END:
                    StopGame();
                    MessageBox.Show($"Bạn thua! {data.Message} đã thắng ván này.", "Kết thúc ván cờ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = $"Thua! {data.Message} thắng.";
                    break;

                case (int)SocketCommand.HET_GIO:
                    StopGame();
                    lblStatus.Text = "Đối thủ hết giờ — Bạn thắng!";
                    MessageBox.Show(
                        "Đối thủ hết giờ suy nghĩ.\nBạn thắng ván này!",
                        "Chiến thắng",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    break;

                case (int)SocketCommand.DAU_HANG:
                    StopGame();
                    lblStatus.Text = "Đối thủ đầu hàng — Bạn thắng!";
                    MessageBox.Show(
                        "Đối thủ đã đầu hàng.\nBạn thắng ván này!",
                        "Chiến thắng",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    break;

                // ── Thương lượng ────────────────────────────────────────
                case (int)SocketCommand.CAU_HOA:
                    if (data.Message == "OK")
                    {
                        StopGame();
                        lblStatus.Text = "Ván cờ kết thúc hòa.";
                        // Không cần popup — lblStatus đã đủ, tránh spam box
                        // Nếu muốn vẫn có thể giữ, nhưng bỏ btnCauHoa.Enabled = true vì game đã xong
                    }
                    else if (data.Message == "NO")
                    {
                        btnCauHoa.Enabled = true; // mở lại để có thể xin lần khác
                        lblStatus.Text = "Đối thủ từ chối cầu hòa. Tiếp tục chơi.";
                        // Không cần popup — lblStatus đủ rõ, không làm gián đoạn game
                    }
                    else
                    {
                        // Đối thủ xin hòa → hỏi mình, gộp thành 1 popup duy nhất
                        var result = MessageBox.Show(
                            "Đối thủ xin cầu hòa.\nBạn có đồng ý không?",
                            "Cầu hòa",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2); // mặc định "No"

                        if (result == DialogResult.Yes)
                        {
                            StopGame();
                            lblStatus.Text = "Ván cờ kết thúc hòa.";
                            _socket.Send(new SocketData((int)SocketCommand.CAU_HOA, "OK", new Point()));
                            // Không cần popup thứ 2 — lblStatus đủ
                        }
                        else
                        {
                            lblStatus.Text = "Bạn từ chối cầu hòa.";
                            _socket.Send(new SocketData((int)SocketCommand.CAU_HOA, "NO", new Point()));
                        }
                    }
                    break;

                case (int)SocketCommand.CHOI_LAI:
                    if (data.Message == "OK")
                    {
                        lblStatus.Text = "Đối thủ đồng ý chơi lại. Bấm Sẵn sàng!";
                        btnSanSang.Text = "Sẵn sàng";
                        btnSanSang.Visible = true;
                        btnSanSang.Enabled = true;
                        btnChoiLai.Enabled = true;
                        // Không cần popup — lblStatus đã hướng dẫn action tiếp theo rõ ràng
                    }
                    else if (data.Message == "NO")
                    {
                        btnChoiLai.Enabled = true;
                        lblStatus.Text = "Đối thủ từ chối chơi lại.";
                        // Không cần popup
                    }
                    else
                    {
                        // Đối thủ xin chơi lại
                        var result = MessageBox.Show(
                            "Đối thủ muốn chơi lại.\nBạn có đồng ý không?",
                            "Chơi lại",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1);

                        if (result == DialogResult.Yes)
                        {
                            _socket.Send(new SocketData((int)SocketCommand.CHOI_LAI, "OK", new Point()));
                            lblStatus.Text = "Bạn đồng ý chơi lại. Bấm Sẵn sàng!";
                            btnSanSang.Text = "Sẵn sàng";
                            btnSanSang.Visible = true;
                            btnSanSang.Enabled = true;
                        }
                        else
                        {
                            _socket.Send(new SocketData((int)SocketCommand.CHOI_LAI, "NO", new Point()));
                            lblStatus.Text = "Bạn từ chối chơi lại.";
                        }
                    }
                    break;

                // ── Phòng chờ ───────────────────────────────────────────
                case (int)SocketCommand.OPPONENT_JOINED:
                    // Tách tên đối thủ từ message format "tênPhòng|index|tênMình|tênĐốiThủ"
                    var parts = data.Message.Split('|');
                    string opponent = parts.Length > 3 ? parts[3] : "Đối thủ";
                    OnOpponentJoined(opponent);
                    break;
                case (int)SocketCommand.JOIN_OK:
                    // Host nhận lệnh này khi đối thủ vừa vào phòng → mở khóa btnSanSang
                    var joinParts = data.Message.Split('|');
                    string opponentJoined = joinParts.Length > 3 ? joinParts[3] : "";
                    if (!string.IsNullOrEmpty(opponentJoined))
                        OnOpponentJoined(opponentJoined);
                    break;
                case (int)SocketCommand.START_GAME:
                    // Message = "0" hoặc "1" — index người đi trước
                    int firstMover = int.Parse(data.Message);
                    btnSanSang.Visible = false;
                    _banCo.VeBanCo();
                    BatDauGame(firstMover);
                    break;
                case (int)SocketCommand.LEAVE_ROOM:
                    // Server báo đối thủ rời phòng — chỉ hiện 1 lần
                    StopGame();
                    MessageBox.Show("Đối thủ đã thoát phòng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "Đối thủ đã thoát phòng.";
                    break;
            }
        }
        private void FormGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Thoát khỏi trò chơi?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            try
            {
                _socket.Send(new SocketData((int)SocketCommand.THOAT_PHONG, "", new Point())); // báo đối thủ
                _socket.Send(new SocketData((int)SocketCommand.LEAVE_ROOM, "", new Point()));  // báo server rời phòng
            }
            catch { }

            var lobby = Application.OpenForms.OfType<FormLobby>().FirstOrDefault();
            if (lobby != null) lobby.Show();
        }
        private void btnSanSang_Click(object sender, EventArgs e)
        {
            btnSanSang.Enabled = false; // chống bấm 2 lần
            btnSanSang.Text = "Đã sẵn sàng";
            lblStatus.Text = "Đang chờ đối thủ sẵn sàng...";
            _socket.Send(new SocketData((int)SocketCommand.READY, "", new Point()));
        }

        private void btnDauHang_Click(object sender, EventArgs e)
        {
            if (_gameOver) return;
            if (MessageBox.Show(
             "Bạn sẽ thua ván này nếu đầu hàng. Xác nhận?",
             "Đầu hàng",
             MessageBoxButtons.YesNo,
             MessageBoxIcon.Warning,
             MessageBoxDefaultButton.Button2) // mặc định focus vào "No" — tránh bấm nhầm
         != DialogResult.Yes) return;

            StopGame();
            lblStatus.Text = "Bạn đã đầu hàng.";
            _socket.Send(new SocketData((int)SocketCommand.DAU_HANG, "", new Point()));
        }

        private void btnCauHoa_Click(object sender, EventArgs e)
        {
            if (_gameOver) return;
            btnCauHoa.Enabled = false;
            lblStatus.Text = "Đã gửi yêu cầu cầu hòa, chờ đối thủ...";
            _socket.Send(new SocketData((int)SocketCommand.CAU_HOA, "", new Point()));
        }

        private void btnChoiLai_Click(object sender, EventArgs e)
        {
            if (!_gameOver)
            {
                MessageBox.Show(
                    "Ván cờ chưa kết thúc.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            btnChoiLai.Enabled = false;
            lblStatus.Text = "Đã gửi yêu cầu chơi lại, chờ đối thủ...";
            _socket.Send(new SocketData((int)SocketCommand.CHOI_LAI, "", new Point()));
        }

        private void btnThoatPhong_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn thoát phòng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            StopListen(); // hủy đăng ký trước khi lobby nhận lại
            try
            {
                _socket.Send(new SocketData((int)SocketCommand.THOAT_PHONG, "", new Point()));
                _socket.Send(new SocketData((int)SocketCommand.LEAVE_ROOM, "", new Point()));
            }
            catch { }

            var lobby = Application.OpenForms.OfType<FormLobby>().FirstOrDefault();
            if (lobby != null) lobby.Show(); // VisibleChanged → lobby tự đăng ký lại

            this.FormClosing -= FormGame_FormClosing;
            this.Close();
        }

        private void FormGame_Load(object sender, EventArgs e)
        {
            _handleReady = true;
            // Xử lý các gói đã bị queue trong lúc chờ handle
            lock (_pendingData)
            {
                while (_pendingData.Count > 0)
                    ProcessData(_pendingData.Dequeue());
            }
        }
    }
}