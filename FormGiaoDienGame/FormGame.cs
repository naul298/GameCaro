namespace FormGiaoDienGame
{
    public partial class FormGame : Form
    {
        private QlyBanCo _banCo;
        private SocketManager _socket;
        private int _playerIndex;
        private bool _isWaiting;

        public FormGame(SocketManager socket, string displayName, int playerIndex, string opponentName, string roomName) : this(socket, displayName, playerIndex, opponentName)
        {
            lblSoPhong.Text = roomName;
        }
        public FormGame(SocketManager socket, string displayName, int playerIndex, string opponentName = "Đối thủ")
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

            prcbCoolDown.Step = Cons.coolDownStep;
            prcbCoolDown.Maximum = Cons.coolDownTime;
            prcbCoolDown.Value = 0;
            tmCoolDown.Interval = Cons.coolDownInterval;

            _banCo.VeBanCo();

            _isWaiting = string.IsNullOrEmpty(opponentName);
            if (_isWaiting)
            {
                pnlBanCo.Enabled = false;
                lblStatus.Text = "Đang chờ đối thủ vào...";
                // timer không Start — ngưng
            }
            else
            {
                BatDauGame();
            }
            Listen();
        }
        private void BatDauGame()
        {
            _isWaiting = false;
            pnlBanCo.Enabled = (_playerIndex == 0);
            lblStatus.Text = (_playerIndex == 0) ? "Đến lượt bạn!" : "Chờ đối thủ đánh...";
            if (_playerIndex == 0) tmCoolDown.Start();
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
            _socket.Send(new SocketData((int)SocketCommand.END, "", new Point()));
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
                case (int)SocketCommand.OPPONENT_JOINED:
                    // Format message: "roomName|playerIndex|myName|opponentName"
                    var joinParts = data.Message.Split('|');
                    string opponentName = joinParts.Length > 3 ? joinParts[3] : "Đối thủ";
                    string roomName = joinParts.Length > 0 ? joinParts[0] : "";

                    lblSoPhong.Text = roomName;
                    lblPlayer2.Text = opponentName; // Cập nhật tên đối thủ lên UI
                    BatDauGame();                   // Mở khóa bàn cờ, bắt đầu timer
                    break;

                case (int)SocketCommand.SEND_POINT:
                    prcbCoolDown.Value = 0;
                    tmCoolDown.Start();
                    pnlBanCo.Enabled = true;
                    lblStatus.Text = "Đến lượt bạn!";
                    _banCo.OtherPlayerMark(data.Point);
                    break;

                case (int)SocketCommand.END:
                    StopGame();
                    MessageBox.Show("Đối thủ thắng!", "Kết thúc");
                    break;

                case (int)SocketCommand.HET_GIO:
                    StopGame();
                    MessageBox.Show("Đối thủ hết giờ — Bạn thắng!", "Kết thúc");
                    break;

                case (int)SocketCommand.THOAT_PHONG:
                    StopGame();
                    MessageBox.Show("Đối thủ đã thoát game.", "Thông báo");
                    break;
            }
        }
        private void FormGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Thoát khỏi trò chơi?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            try { _socket.Send(new SocketData((int)SocketCommand.THOAT_PHONG, "", new Point())); }
            catch { }
        }

        // Designer đã đăng ký 3 handler này — giữ empty để không báo lỗi
        private void FormGame_Shown(object sender, EventArgs e) { }
        private void btnLan_Click(object sender, EventArgs e) { }
    }
}