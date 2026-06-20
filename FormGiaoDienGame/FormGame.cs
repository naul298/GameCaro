using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace FormGiaoDienGame
{
    public partial class FormGame : Form
    {
        #region Properties
        QlyBanCo BanCo;

        SocketManager socket = new SocketManager();
        #endregion
        #region Methods
        public void EndGame()
        {
            tmCoolDown.Stop();
            pndlBanCo.Enabled = false;
            MessageBox.Show("Kết thúc");
        }

        #endregion
        public FormGame()
        {
            InitializeComponent();

            BanCo = new QlyBanCo(pndlBanCo, lblPlayer1, lblPlayer2, lblStatus);
            BanCo.EndGame += BanCo_EndGame;
            BanCo.PlayerMark += BanCo_PlayerMark;

            prcbCoolDown.Step = Cons.coolDownStep;
            prcbCoolDown.Maximum = Cons.coolDownTime;
            prcbCoolDown.Value = 0;

            tmCoolDown.Interval = Cons.coolDownInterval;

            BanCo.VeBanCo();

            tmCoolDown.Start();
        }

        private void BanCo_PlayerMark(object? sender, ButtonClickEvent e)
        {
            tmCoolDown.Start();
            prcbCoolDown.Value = 0;

            socket.Send(new SocketData((int)SocketCommand.SEND_POINT,"", e.ClickPoint));
        }

        private void BanCo_EndGame(object? sender, EventArgs e)
        {
            EndGame();
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

            if (!socket.KetNoiServer())
            {
                socket.TaoServer();
              
            }
            else
            {
                Listen();
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
                catch { }
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
                    BanCo.OtherPlayerMark(data.Point);
                    break;
                case (int)SocketCommand.CAU_HOA:

                    break;
                case (int)SocketCommand.CHOI_LAI:

                    break;
                case (int)SocketCommand.DAU_HANG:

                    break;
                case (int)SocketCommand.THOAT_PHONG:

                    break;
                default:
                    break;
            }
        }
    }
}
