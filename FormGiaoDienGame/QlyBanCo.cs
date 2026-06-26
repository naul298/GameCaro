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
        public QlyBanCo(Panel banCo, Label player1, Label player2, Label lblStatus,
                string nameX, string nameO)
        {
            this.BanCo = banCo;
            this.Player1 = player1;
            this.Player2 = player2;
            this.LblStatus = lblStatus;
            this.listPlayers = new List<Player>()
            {
                new Player(nameX, Properties.Resources.x),
                new Player(nameO, Properties.Resources.o)
            };
        }
        #endregion

        #region Methods
        public void VeBanCo()
        {
            // Xóa toàn bộ ô cờ cũ trên panel và bật panel
            banCo.Controls.Clear();
            banCo.Enabled = true;

            // Ma trận 2 chiều: Matrix[hàng][cột] → truy cập từng ô bàn cờ theo tọa độ
            Matrix = new List<List<Button>>();

            // Vòng lặp hàng (i = 0 → 14, tức 15 hàng)
            for (int i = 0; i < Cons.chieuCaoBanCo; i++)
            {
                // Thêm 1 hàng mới vào ma trận — mỗi hàng là 1 List<Button>
                Matrix.Add(new List<Button>());

                // Vòng lặp cột (j = 0 → 19, tức 20 cột)
                for (int j = 0; j < Cons.chieuRongBanCo; j++)
                {
                    // Tạo ô cờ tại vị trí (hàng i, cột j)
                    Button btn = new Button()
                    {
                        Width = Cons.chessWidth,                      // chiều rộng ô (px)
                        Height = Cons.chessHeight,                     // chiều cao ô (px)
                        Location = new Point(j * Cons.chessWidth,    // x = cột × rộng
                                             i * Cons.chessHeight),    // y = hàng × cao
                        BackgroundImageLayout = ImageLayout.Stretch,   // ảnh quân cờ co giãn vừa ô
                        Tag = i.ToString()                             // lưu chỉ số hàng để tra tọa độ sau
                    };

                    // Sự kiện click: xử lý khi người chơi đánh vào ô này
                    btn.Click += (sender, e) =>
                    {
                        Button clicked = sender as Button; // ô vừa được bấm

                        // Ô đã có quân → bỏ qua
                        if (clicked.BackgroundImage != null) return;

                        // Đặt quân cờ của người chơi hiện tại lên ô
                        clicked.BackgroundImage = listPlayers[curPlayer].Chess;

                        // Kiểm tra thắng TRƯỚC khi đổi lượt
                        // → curPlayer lúc này vẫn là người vừa đánh
                        if (IsEndGame(clicked))
                        {
                            // Thông báo tọa độ vừa đánh lên server (để relay cho đối thủ)
                            playerMark?.Invoke(this, new ButtonClickEvent(LayToaDoChess(clicked)));
                            isEndGame(); // kết thúc game, hiện thông báo thắng
                            return;      // dừng, không đổi lượt nữa
                        }

                        // Chưa thắng → đổi lượt sang người kia
                        DoiNguoiChoi();

                        // Thông báo tọa độ vừa đánh lên server
                        playerMark?.Invoke(this, new ButtonClickEvent(LayToaDoChess(clicked)));
                    };

                    // Thêm ô vào panel để hiển thị
                    banCo.Controls.Add(btn);

                    // Lưu ô vào ma trận tại đúng vị trí Matrix[hàng i][cột j]
                    Matrix[i].Add(btn);
                }
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
            // Kiểm tra 5 quân liên tiếp theo đường CHÉO PHỤ (↗ đến ↙) tính từ ô vừa đánh
            Point point = LayToaDoChess(btn); // tọa độ (x=cột, y=hàng) của ô vừa đánh

            int countTop = 0; // số quân liên tiếp cùng loại theo hướng ↗ (lên phải)
            for (int i = 0; i < Cons.chieuRongBanCo; i++)
            {
                if (point.X + i >= Cons.chieuRongBanCo || point.Y - i < 0) break; // ra khỏi biên → dừng
                if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage) countTop++;
                else break; // gặp ô trống hoặc quân đối thủ → dừng đếm
            }

            int countBottom = 0; // số quân liên tiếp cùng loại theo hướng ↙ (xuống trái)
            for (int i = 1; i < Cons.chieuRongBanCo; i++)
            {
                if (point.Y + i >= Cons.chieuCaoBanCo || point.X - i < 0) break; // ra khỏi biên → dừng
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage) countBottom++;
                else break; // gặp ô trống hoặc quân đối thủ → dừng đếm
            }

            // Tổng 2 hướng >= 5 → thắng (countTop đã bao gồm ô hiện tại nên chỉ cần >= 5)
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
