namespace FormGiaoDienGame
{
    public class Player
    {
        //private int id;
        //private string account;
        //private string password;
        private string name;
        private Image chess;

        public string Name { get => name; set => name = value; }
        public Image Chess { get => chess; set => chess = value; }

        //public int Id { get => id; set => id = value; }
        //public string Account { get => account; set => account = value; }
        //public string Password { get => password; set => password = value; }

        public Player(string name, Image chess)
        {
            this.Name = name;
            this.Chess = chess;
        }
    }
}
