namespace FormGiaoDienGame
{
    public partial class FormGame : Form
    {
        private QlyBanCo _banCo;
        private SocketManager _socket;
        private int _playerIndex;
        private bool _isWaiting;
        private bool _gameOver = false;


        public FormGame(SocketManager socket, string displayName, int playerIndex, string opponentName, string roomName)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            _socket = socket;
            _playerIndex = playerIndex;

            string nameX = (playerIndex == 0) ? displayName : opponentName;
            string nameO = (playerIndex == 1) ? displayName : opponentName;

            _banCo = new QlyBanCo(pnlBanCo, lblPlayer1, lblPlayer2, lblStatus, nameX, nameO);
            _banCo.EndGame += BanCo_EndGame;
            _banCo.PlayerMark += BanCo_PlayerMark;

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

            if (string.IsNullOrEmpty(opponentName))
                lblStatus.Text = "Đang chờ đối thủ vào phòng...";
            else
                OnOpponentJoined(opponentName); // vào phòng đã có đủ 2 người

            Listen();
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
            _banCo.VeBanCo(); // reset bàn cờ nếu chơi lại
            prcbCoolDown.Value = 0;

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
        private void Listen()
        {
            var t = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        var data = _socket.Receive() as SocketData;
                        if (data == null) break;
                        this.Invoke(() => ProcessData(data));
                    }
                }
                catch { }
            });
            t.IsBackground = true;
            t.Start();
        }

        private void ProcessData(SocketData data)
        {
            switch (data.Command)
            {
                case (int)SocketCommand.SEND_POINT:
                    prcbCoolDown.Value = 0;
                    tmCoolDown.Start();
                    pnlBanCo.Enabled = true;
                    lblStatus.Text = "Đến lượt bạn!";
                    _banCo.OtherPlayerMark(data.Point);
                    break;

                case (int)SocketCommand.END:
                    StopGame();
                    string winner = data.Message;
                    MessageBox.Show($"💀 Bạn thua! {winner} đã thắng ván này.", "Kết thúc ván cờ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = $"Thua! {winner} thắng.";
                    break;
                case (int)SocketCommand.CAU_HOA:
                    if (data.Message == "OK")
                    {
                        // Đối thủ đồng ý hòa
                        StopGame();
                        MessageBox.Show("Đối thủ đồng ý cầu hòa!\n🤝 Ván cờ kết thúc hòa.", "Hòa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblStatus.Text = "Ván cờ kết thúc hòa.";
                        btnCauHoa.Enabled = true;
                    }
                    else if (data.Message == "NO")
                    {
                        // Đối thủ từ chối
                        MessageBox.Show("Đối thủ từ chối cầu hòa!", "Bị từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lblStatus.Text = "Đối thủ từ chối cầu hòa.";
                        btnCauHoa.Enabled = true;
                    }
                    else
                    {
                        // Đối thủ xin cầu hòa → hỏi mình
                        var result = MessageBox.Show(
                            "Đối thủ xin cầu hòa!\nBạn có đồng ý không?",
                            "Cầu hòa",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            StopGame();
                            _socket.Send(new SocketData((int)SocketCommand.CAU_HOA, "OK", new Point()));
                            MessageBox.Show("Bạn đã đồng ý cầu hòa.\n🤝 Ván cờ kết thúc hòa.", "Hòa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            lblStatus.Text = "Ván cờ kết thúc hòa.";
                        }
                        else
                        {
                            _socket.Send(new SocketData((int)SocketCommand.CAU_HOA, "NO", new Point()));
                            lblStatus.Text = "Bạn từ chối cầu hòa.";
                        }
                    }
                    break;
                case (int)SocketCommand.HET_GIO:
                    StopGame();
                    MessageBox.Show("Đối thủ hết giờ — Bạn thắng!", "Kết thúc");
                    break;
                case (int)SocketCommand.CHOI_LAI:
                    if (data.Message == "OK")
                    {
                        // Đối thủ đồng ý chơi lại
                        btnChoiLai.Enabled = true;
                        _banCo.VeBanCo();
                        BatDauGame(_playerIndex == 0 ? 1 : 0); // người thua đi trước (đối thủ mình)
                        lblStatus.Text = "Ván mới bắt đầu!";
                    }
                    else if (data.Message == "NO")
                    {
                        MessageBox.Show("Đối thủ từ chối chơi lại!", "Bị từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lblStatus.Text = "Đối thủ từ chối chơi lại.";
                        btnChoiLai.Enabled = true;
                    }
                    else
                    {
                        // Đối thủ xin chơi lại → hỏi mình
                        var result = MessageBox.Show("Đối thủ muốn chơi lại! Bạn có đồng ý không?", "Chơi lại", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            _socket.Send(new SocketData((int)SocketCommand.CHOI_LAI, "OK", new Point()));
                            _banCo.VeBanCo();
                            BatDauGame(_playerIndex); // người gửi yêu cầu (thua) đi trước
                            lblStatus.Text = "Ván mới bắt đầu!";
                        }
                        else
                        {
                            _socket.Send(new SocketData((int)SocketCommand.CHOI_LAI, "NO", new Point()));
                            lblStatus.Text = "Bạn từ chối chơi lại.";
                        }
                    }
                    break;
                case (int)SocketCommand.OPPONENT_JOINED:
                    var parts = data.Message.Split('|');
                    string opponent = parts.Length > 3 ? parts[3] : "Đối thủ";
                    OnOpponentJoined(opponent);
                    break;

                case (int)SocketCommand.START_GAME:
                    int firstMover = int.Parse(data.Message); // 0 hoặc 1
                    btnSanSang.Visible = false;
                    BatDauGame(firstMover);
                    break;
                case (int)SocketCommand.THOAT_PHONG:
                    StopGame();
                    MessageBox.Show("Đối thủ đã thoát game.", "Thông báo");
                    break;
                case (int)SocketCommand.DAU_HANG:
                    StopGame();
                    MessageBox.Show("Đối thủ đã đầu hàng!\nBạn thắng!", "Chiến thắng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "Bạn thắng! Đối thủ đầu hàng.";
                    break;
            }
        }
        private void FormGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Thoát khỏi trò chơi?", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true; // người dùng bấm Cancel → không thoát
                return;
            }

            // Báo server biết người chơi đã thoát phòng
            try { _socket.Send(new SocketData((int)SocketCommand.THOAT_PHONG, "", new Point())); }
            catch { }

            // Tìm FormLobby đang bị ẩn và hiện lại
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
            var confirm = MessageBox.Show("Bạn có chắc muốn đầu hàng không?\nĐối thủ sẽ thắng ván này.", "Xác nhận đầu hàng", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            StopGame();
            lblStatus.Text = "Bạn đã đầu hàng.";
            _socket.Send(new SocketData((int)SocketCommand.DAU_HANG, "", new Point()));
        }

        private void btnCauHoa_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn xin cầu hòa không?", "Xác nhận cầu hòa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            btnCauHoa.Enabled = false; // chống bấm 2 lần
            lblStatus.Text = "Đang chờ đối thủ trả lời...";
            _socket.Send(new SocketData((int)SocketCommand.CAU_HOA, "", new Point()));
        }

        private void btnChoiLai_Click(object sender, EventArgs e)
        {
            if (!_gameOver)
            {
                MessageBox.Show("Chỉ có thể yêu cầu chơi lại sau khi ván cờ kết thúc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc muốn xin chơi lại không?", "Xác nhận chơi lại", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            btnChoiLai.Enabled = false;
            lblStatus.Text = "Đang chờ đối thủ trả lời...";
            _socket.Send(new SocketData((int)SocketCommand.CHOI_LAI, "", new Point()));
        }

        private void btnThoatPhong_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn thoát phòng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try { _socket.Send(new SocketData((int)SocketCommand.THOAT_PHONG, "", new Point())); }
            catch { }

            var lobby = Application.OpenForms.OfType<FormLobby>().FirstOrDefault();
            if (lobby != null) lobby.Show();

            this.FormClosing -= FormGame_FormClosing; // tránh hỏi 2 lần khi Close()
            this.Close();
        }
    }
}