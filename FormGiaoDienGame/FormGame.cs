using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace FormGiaoDienGame
{
    public partial class FormGame : Form
    {
        #region Properties
        QlyBanCo BanCo;

        SocketManager socket;
        #endregion
        #region Methods
        public void EndGame()
        {
            tmCoolDown.Stop();
            pnlBanCo.Enabled = false;
            //MessageBox.Show("Kết thúc");
        }

        #endregion
        public FormGame(SocketManager socket, string displayName, int playerIndex)
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;

            BanCo = new QlyBanCo(pnlBanCo, lblPlayer1, lblPlayer2, lblStatus);
            BanCo.EndGame += BanCo_EndGame;
            BanCo.PlayerMark += BanCo_PlayerMark;

            prcbCoolDown.Step = Cons.coolDownStep;
            prcbCoolDown.Maximum = Cons.coolDownTime;
            prcbCoolDown.Value = 0;

            tmCoolDown.Interval = Cons.coolDownInterval;

            this.socket = socket;

            if (playerIndex == 0)
                lblPlayer1.Text = displayName;
            else
                lblPlayer2.Text = displayName;
            BanCo.VeBanCo();

            tmCoolDown.Start();
            Listen();
        }

        private void BanCo_PlayerMark(object? sender, ButtonClickEvent e)
        {
            tmCoolDown.Start();
            pnlBanCo.Enabled = false;
            prcbCoolDown.Value = 0;
            socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", e.ClickPoint));
            Listen();
        }

        private void BanCo_EndGame(object? sender, EventArgs e)
        {
            EndGame();
            socket.Send(new SocketData((int)SocketCommand.END, "", new Point()));
        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();
            if (prcbCoolDown.Value >= prcbCoolDown.Maximum)
            {
                EndGame();
            }
        }

        private void FormGame_Shown(object sender, EventArgs e)
        {
            txtIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            if (string.IsNullOrEmpty(txtIP.Text)) { txtIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet); }
        }

        private void btnLan_Click(object sender, EventArgs e)
        {
            socket.IP = txtIP.Text;

            if (socket.KetNoiServer())
            {
                lblConnect.Text = "✔ Kết nối thành công!";
                lblConnect.ForeColor = Color.Green;
                pnlBanCo.Enabled = true;
                Listen(); // bắt đầu lắng nghe từ server
            }
            else
            {
                lblConnect.Text = "✘ Không kết nối được!";
                lblConnect.ForeColor = Color.Red;
                pnlBanCo.Enabled = false;
            }
        }
        void Listen()
        {
            Thread listenThread = new Thread(() =>
            {
                try
                {
                    SocketData data = (SocketData)socket.Receive();
                    ProcessData(data);
                }
                catch (SocketException ex)
                {
                    this.Invoke((MethodInvoker)(() =>
                    {
                        tmCoolDown.Stop();
                        pnlBanCo.Enabled = false;
                        MessageBox.Show("Mất kết nối với server.", "Thông báo");
                    }));
                }
                catch (Exception ex)
                {
                    // Bỏ qua nếu form đang đóng
                    if (!this.IsDisposed)
                    {
                        this.Invoke((MethodInvoker)(() =>
                        {
                            MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo");
                        }));
                    }
                }
            });
            listenThread.IsBackground = true;
            listenThread.Start();
        }
        private void ProcessData(SocketData data)
        {
            switch (data.Command)
            {
                case (int)SocketCommand.THONG_BAO:
                    MessageBox.Show(data.Message);
                    break;
                case (int)SocketCommand.SEND_POINT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        prcbCoolDown.Value = 0;
                        pnlBanCo.Enabled = true;
                        tmCoolDown.Start();
                        BanCo.OtherPlayerMark(data.Point);
                    }));
                    break;
                case (int)SocketCommand.CAU_HOA:

                    break;
                case (int)SocketCommand.CHOI_LAI:

                    break;
                case (int)SocketCommand.DAU_HANG:

                    break;
                case (int)SocketCommand.END:
                    MessageBox.Show("Win!");
                    break;
                case (int)SocketCommand.HET_GIO:

                    break;
                case (int)SocketCommand.THOAT_PHONG:
                    tmCoolDown.Stop();
                    MessageBox.Show("Người chơi đã thoát game","Thông báo");
                    break;
                default:
                    break;
            }
            Listen();
        }

        private void FormGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Thoát khỏi trò chơi?","Thông báo",MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                //socket.Send(new SocketData((int)SocketCommand.THOAT_PHONG, "", new Point()));
            }
        }
    }
}
