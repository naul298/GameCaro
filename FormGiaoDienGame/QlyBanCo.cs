using System;
using System.Collections.Generic;
using System.Text;

namespace FormGiaoDienGame
{
    public class QlyBanCo
    {
        #region Properties
        private Panel banCo; //Panel chứa bàn cờ
        private Label player1; //Người chơi 1
        private Label player2; //Người chơi 2
        private Label lblStatus; //Thanh trạng thái
        private List<Player> listPlayers; //List chứa danh sách người chơi
        private List<List<Button>> matrix;
        private int curPlayer; //Biến lưu trữ người chơi hiện tại (0 hoặc 1)

        public Panel BanCo { get => banCo; set => banCo = value; }
        public int CurPlayer { get => curPlayer; set => curPlayer = value; }
        public Label Player1 { get => player1; set => player1 = value; }
        public Label Player2 { get => player2; set => player2 = value; }
        public Label LblStatus { get => lblStatus; set => lblStatus = value; }
        public List<List<Button>> Matrix { get => matrix; set => matrix = value; }

        private event EventHandler<ButtonClickEvent> playerMark;
        public event EventHandler<ButtonClickEvent> PlayerMark
        {
            add
            {
                playerMark += value;
            }
            remove
            {
                playerMark -= value;
            }
        }
        private event EventHandler endGame;
        public event EventHandler EndGame
        {
            add
            {
                endGame += value;
            }
            remove
            {
                endGame -= value;
            }
        }
        #endregion

        #region Initialize
        public QlyBanCo(Panel banCo, Label player1, Label player2, Label lblStatus)
        {
            this.BanCo = banCo;
            this.Player1 = player1;
            this.Player2 = player2;
            this.LblStatus = lblStatus;
            this.listPlayers = new List<Player>()
            {
                //Thêm 2 người chơi với tên và hình ảnh quân cờ tương ứng
                new Player("Lê Hoàng Luân", Properties.Resources.x),
                new Player("Lê Nhật Hoà", Properties.Resources.o)
            };
            this.player1.Text = listPlayers[0].Name;
            this.player2.Text = listPlayers[1].Name;
            this.curPlayer = 0;
            CapNhatHeader();
        }
        #endregion

        #region Methods
        public void VeBanCo()
        {
            banCo.Enabled = true;

            Matrix = new List<List<Button>>();

            Button oldBtn = new Button() { Width = 0, Height = 0, Location = new Point(0, 0) };

            for (int i = 0; i < Cons.chieuCaoBanCo; i++)
            {
                for (int j = 0; j < Cons.chieuRongBanCo; j++)
                {
                    Matrix.Add(new List<Button>());
                    Button btn = new Button()
                    {
                        Width = Cons.chessWidth, //Đặt chiều rộng của ô bàn cờ
                        Height = Cons.chessHeight, //Đặt chiều cao của ô bàn cờ

                        //Đặt vị trí của ô bàn cờ dựa trên vị trí và kích thước của ô bàn cờ cũ
                        Location = new Point(oldBtn.Location.X + oldBtn.Width, oldBtn.Location.Y),

                        //Đặt chế độ hiển thị hình ảnh nền của ô bàn cờ là Stretch để hình ảnh vừa với kích thước của ô bàn cờ
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };

                    //Tạo 1 sự kiện: đánh cờ
                    btn.Click += (sender, e) => //sender: đối tượng gửi sự kiện, e: thông tin sự kiện
                    {
                        Button btn = sender as Button; //Ép kiểu sender về Button để lấy thông tin về ô bàn cờ được click

                        if (btn.BackgroundImage != null) { return; }

                        btn.BackgroundImage = listPlayers[curPlayer].Chess; //Đặt hình ảnh quân cờ của người chơi hiện tại

                        DoiNguoiChoi();

                        if (playerMark != null)
                        {
                            playerMark(this, new ButtonClickEvent(LayToaDoChess(btn)));
                        }

                        if (IsEndGame(btn))
                        {
                            isEndGame();
                        }
                    };

                    BanCo.Controls.Add(btn); //Thêm ô bàn cờ vào Panel chứa bàn cờ
                    Matrix[i].Add(btn);
                    oldBtn = btn; //Gán ô bàn cờ vừa tạo thành ô bàn cờ cũ
                }
                oldBtn.Location = new Point(0, oldBtn.Location.Y + Cons.chessHeight);
                oldBtn.Height = 0;
                oldBtn.Width = 0;
            }
        }
        public void OtherPlayerMark(Point point)
        {

            Button btn = Matrix[point.Y][point.X]; //Ép kiểu sender về Button để lấy thông tin về ô bàn cờ được click

            if (btn.BackgroundImage != null) { return; }

            //banCo.Enabled = true;

            btn.BackgroundImage = listPlayers[curPlayer].Chess; //Đặt hình ảnh quân cờ của người chơi hiện tại

            DoiNguoiChoi();

            if (IsEndGame(btn))
            {
                isEndGame();
            }
        }
        private void isEndGame()
        {
            if (endGame != null)
            {
                endGame(this, new EventArgs());
            }
            MessageBox.Show("Kết thúc game");
        }
        private bool IsEndGame(Button btn)
        {
            return isSecondaryDiagonal(btn) || isVertical(btn) || isHorizontal(btn) || isMainDiagonal(btn);
        }
        private Point LayToaDoChess(Button btn)
        {
            int y = Convert.ToInt32(btn.Tag);
            int x = matrix[y].IndexOf(btn);
            Point point = new Point(x, y);
            return point;
        }
        private bool isHorizontal(Button btn)
        {
            // Kiểm tra 5 quân liên tiếp theo hàng NGANG tính từ ô vừa đánh.
            // Đếm sang trái + sang phải, tổng đủ 5 thì thắng.
            Point point = LayToaDoChess(btn);
            int countLeft = 0;
            // Đếm số quân liên tiếp cùng loại về phía TRÁI (bao gồm ô hiện tại)
            for (int i = point.X; i >= 0; i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countLeft++;
                }
                else { break; }
            }
            int countRight = 0;
            // Đếm số quân liên tiếp cùng loại về phía PHẢI (bắt đầu từ ô kế tiếp)
            for (int i = point.X + 1; i < Cons.chieuRongBanCo; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                {
                    countRight++;
                }
                else { break; }
            }
            return countLeft + countRight >= 5;
        }
        private bool isVertical(Button btn)
        {
            // Kiểm tra 5 quân liên tiếp theo cột DỌC tính từ ô vừa đánh.
            // Đếm lên trên + xuống dưới, tổng đủ 5 thì thắng.
            Point point = LayToaDoChess(btn);
            int countTop = 0;
            // Đếm số quân liên tiếp cùng loại lên phía TRÊN (bao gồm ô hiện tại)
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else { break; }
            }
            int countBottom = 0;
            // Đếm số quân liên tiếp cùng loại xuống phía DƯỚI (bắt đầu từ ô kế tiếp)
            for (int i = point.Y + 1; i < Cons.chieuCaoBanCo; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else { break; }
            }
            return countTop + countBottom >= 5;
        }
        private bool isMainDiagonal(Button btn)
        {
            // Kiểm tra 5 quân liên tiếp theo đường CHÉO CHÍNH (↖ đến ↘) tính từ ô vừa đánh.
            // Đếm lên trái + xuống phải, tổng đủ 5 thì thắng.
            Point point = LayToaDoChess(btn);
            int countTop = 0;
            // Đếm quân liên tiếp cùng loại theo hướng ↖ (bao gồm ô hiện tại)
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0) { break; }

                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else { break; }
            }
            int countBottom = 0;
            // Đếm quân liên tiếp cùng loại theo hướng ↘ (bắt đầu từ ô kế tiếp)
            for (int i = 1; i <= Cons.chieuRongBanCo - point.X; i++)
            {
                if (point.Y + i >= Cons.chieuCaoBanCo || point.X + i >= Cons.chieuRongBanCo) { break; }
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else { break; }
            }
            return countTop + countBottom >= 5;
        }
        private bool isSecondaryDiagonal(Button btn)
        {
            // Kiểm tra 5 quân liên tiếp theo đường CHÉO PHỤ (↗ đến ↙) tính từ ô vừa đánh.
            // Đếm lên phải + xuống trái, tổng đủ 5 thì thắng.
            Point point = LayToaDoChess(btn);
            int countTop = 0;
            // Đếm quân liên tiếp cùng loại theo hướng ↗ (bao gồm ô hiện tại)
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Cons.chieuRongBanCo || point.Y - i < 0) { break; }
                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else { break; }
            }
            int countBottom = 0;
            // Đếm quân liên tiếp cùng loại theo hướng ↙ (bắt đầu từ ô kế tiếp)
            for (int i = 1; i <= Cons.chieuRongBanCo - point.X; i++)
            {
                if (point.Y + i >= Cons.chieuCaoBanCo || point.X - i < 0) { break; }
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else { break; }
            }
            return countTop + countBottom >= 5;
        }
        private void DoiNguoiChoi()
        {
            curPlayer = curPlayer == 0 ? 1 : 0;
            CapNhatHeader();
        }
        private void CapNhatHeader()
        {
            string tenNguoiChoi = listPlayers[curPlayer].Name;

            // Cập nhật label status
            lblStatus.Text = $"Chờ đối thủ {tenNguoiChoi}...";

            // In đậm tên người đang đến lượt, bình thường người còn lại
            player1.Font = new Font(player1.Font, curPlayer == 0 ? FontStyle.Bold : FontStyle.Regular);
            player2.Font = new Font(player2.Font, curPlayer == 1 ? FontStyle.Bold : FontStyle.Regular);
        }
        #endregion
    }
    public class ButtonClickEvent : EventArgs
    {
        private Point clickPoint;

        public Point ClickPoint { get => clickPoint; set => clickPoint = value; }
        public ButtonClickEvent(Point point)
        {
            this.ClickPoint = point;
        }
    }
}
