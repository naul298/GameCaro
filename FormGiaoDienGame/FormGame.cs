namespace FormGiaoDienGame
{
    public partial class FormGame : Form
    {
        private QlyBanCo _banCo;
        private SocketManager _socket;
        private int _playerIndex;
        private bool _isWaiting;

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

            // Khóa tất cả, chờ đủ người
            pnlBanCo.Enabled = false;
            btnSanSang.Enabled = false;
            btnSanSang.Visible = true;

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

                case (int)SocketCommand.HET_GIO:
                    StopGame();
                    MessageBox.Show("Đối thủ hết giờ — Bạn thắng!", "Kết thúc");
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
    }
}