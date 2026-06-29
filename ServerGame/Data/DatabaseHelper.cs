using Microsoft.Data.SqlClient;

namespace ServerGame.Data
{
    public static class DatabaseHelper
    {
        /// <summary>
        /// Tạo tài khoản mới. Trả về true nếu thành công, false nếu account đã tồn tại.
        /// </summary>
        public static (bool ok, string message) CreateUser(string connStr, string username, string password, string displayName)
        {
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();

                // 1. Kiểm tra tài khoản (account) đã tồn tại chưa
                using var checkCmd = new SqlCommand("SELECT COUNT(1) FROM Users WHERE account = @u", conn);
                checkCmd.Parameters.AddWithValue("@u", username);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                    return (false, "Tên đăng nhập đã tồn tại.");

                // 2. Insert user mới (Khớp cột: account, password, name)
                using var insertCmd = new SqlCommand(
                    "INSERT INTO Users (account, password, name) VALUES (@u, @p, @d)", conn);
                insertCmd.Parameters.AddWithValue("@u", username);
                insertCmd.Parameters.AddWithValue("@p", password);
                insertCmd.Parameters.AddWithValue("@d", displayName);
                insertCmd.ExecuteNonQuery();

                return (true, "Đăng ký thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DB CreateUser: " + ex.Message);
                return (false, "Lỗi server.");
            }
        }

        /// <summary>
        /// Kiểm tra đăng nhập. Trả về Display Name nếu đúng, trả về null nếu sai.
        /// </summary>
        public static string? KiemTraLogin(string connStr, string username, string password)
        {
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();
                // Khớp cột: name, account, password
                string sql = "SELECT name FROM Users WHERE account=@u AND password=@p";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                return cmd.ExecuteScalar()?.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DB Login: " + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Xóa toàn bộ phòng không mặc định và không có người chơi (player_count = 0)
        /// </summary>
        public static void XoaPhongRac(string connStr)
        {
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();
                using var cmd = new SqlCommand(
                    "DELETE FROM Lobby WHERE is_default = 0 AND player_count = 0", conn);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine($"Đã xóa {rows} phòng rác.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DB XoaPhongRac: " + ex.Message);
            }
        }
        /// <summary>
        /// Lấy số ID của User dựa vào tên tài khoản (account)
        /// </summary>
        public static int GetUserId(string connStr, string username)
        {
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();
                // Khớp cột: id, account
                using var cmd = new SqlCommand("SELECT id FROM Users WHERE account=@u", conn);
                cmd.Parameters.AddWithValue("@u", username);
                return Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DB GetUserId: " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Load tất cả phòng từ DB lên bộ nhớ khi Server khởi động
        /// </summary>
        public static List<(int Id, string Name, bool IsDefault)> LoadAllRooms(string connStr)
        {
            var list = new List<(int, string, bool)>();
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();
                // Khớp cột: id, name, is_default của bảng Lobby
                using var cmd = new SqlCommand("SELECT id, name, is_default FROM Lobby ORDER BY id", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add((reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DB LoadRooms: " + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Tạo phòng mới trong DB, trả về ID tự động tăng vừa tạo
        /// </summary>
        public static int CreateRoom(string connStr, string roomName, int hostId)
        {
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();
                // Khớp cột: name, host_id, status, player_count, is_default
                using var cmd = new SqlCommand(
                    "INSERT INTO Lobby (name, host_id, status, player_count, is_default)" +
                    " OUTPUT INSERTED.id VALUES (@n, @h, 'Waiting', 0, 0)", conn);
                cmd.Parameters.AddWithValue("@n", roomName);
                cmd.Parameters.AddWithValue("@h", hostId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DB CreateRoom: " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Xóa phòng không phải mặc định khỏi DB
        /// </summary>
        public static void DeleteRoom(string connStr, int roomId)
        {
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();
                // Khớp cột: id, is_default
                using var cmd = new SqlCommand("DELETE FROM Lobby WHERE id=@id AND is_default=0", conn);
                cmd.Parameters.AddWithValue("@id", roomId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DB DeleteRoom: " + ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật thông tin chủ phòng, số người và trạng thái phòng
        /// </summary>
        public static void UpdateRoom(string connStr, int roomId, int hostId, int playerCount, string status)
        {
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();
                // Khớp cột: host_id, player_count, status, id
                using var cmd = new SqlCommand(
                    "UPDATE Lobby SET host_id=@h, player_count=@p, status=@s WHERE id=@id", conn);
                cmd.Parameters.AddWithValue("@h", hostId);
                cmd.Parameters.AddWithValue("@p", playerCount);
                cmd.Parameters.AddWithValue("@s", status);
                cmd.Parameters.AddWithValue("@id", roomId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi DB UpdateRoom: " + ex.Message);
            }
        }
    }
}