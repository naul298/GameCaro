using Microsoft.Data.SqlClient;

namespace ServerGame.Data;

public static class DatabaseHelper
{
    // Login (giữ nguyên)
    public static string? KiemTraLogin(string connStr, string username, string password)
    {
        try
        {
            using var conn = new SqlConnection(connStr);
            conn.Open();
            string sql = "SELECT display_name FROM Users WHERE username=@u AND password=@p";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);
            return cmd.ExecuteScalar()?.ToString();
        }
        catch (Exception ex) { Console.WriteLine("Lỗi DB: " + ex.Message); return null; }
    }

    // Lấy userId từ username
    public static int GetUserId(string connStr, string username)
    {
        try
        {
            using var conn = new SqlConnection(connStr);
            conn.Open();
            using var cmd = new SqlCommand(
                "SELECT id FROM Users WHERE username=@u", conn);
            cmd.Parameters.AddWithValue("@u", username);
            return Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
        }
        catch { return 0; }
    }

    // Load tất cả phòng từ DB lên memory khi server khởi động
    public static List<(int Id, string Name, bool IsDefault)> LoadAllRooms(string connStr)
    {
        var list = new List<(int, string, bool)>();
        try
        {
            using var conn = new SqlConnection(connStr);
            conn.Open();
            using var cmd = new SqlCommand(
                "SELECT id, room_name, is_default FROM Lobby ORDER BY id", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add((reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2)));
        }
        catch (Exception ex) { Console.WriteLine("Lỗi DB LoadRooms: " + ex.Message); }
        return list;
    }

    // Tạo phòng mới trong DB, trả về id mới
    public static int CreateRoom(string connStr, string roomName, int hostId)
    {
        try
        {
            using var conn = new SqlConnection(connStr);
            conn.Open();
            using var cmd = new SqlCommand(
                "INSERT INTO Lobby (room_name, host_id, status, player_count, is_default)" +
                " OUTPUT INSERTED.id VALUES (@n, @h, 'Waiting', 0, 0)", conn);
            cmd.Parameters.AddWithValue("@n", roomName);
            cmd.Parameters.AddWithValue("@h", hostId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex) { Console.WriteLine("Lỗi DB CreateRoom: " + ex.Message); return -1; }
    }

    // Xóa phòng không mặc định khỏi DB
    public static void DeleteRoom(string connStr, int roomId)
    {
        try
        {
            using var conn = new SqlConnection(connStr);
            conn.Open();
            using var cmd = new SqlCommand(
                "DELETE FROM Lobby WHERE id=@id AND is_default=0", conn);
            cmd.Parameters.AddWithValue("@id", roomId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine("Lỗi DB DeleteRoom: " + ex.Message); }
    }

    // Cập nhật host_id và player_count
    public static void UpdateRoom(string connStr, int roomId, int hostId,
                                   int playerCount, string status)
    {
        try
        {
            using var conn = new SqlConnection(connStr);
            conn.Open();
            using var cmd = new SqlCommand(
                "UPDATE Lobby SET host_id=@h, player_count=@p, status=@s WHERE id=@id", conn);
            cmd.Parameters.AddWithValue("@h", hostId);
            cmd.Parameters.AddWithValue("@p", playerCount);
            cmd.Parameters.AddWithValue("@s", status);
            cmd.Parameters.AddWithValue("@id", roomId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex) { Console.WriteLine("Lỗi DB UpdateRoom: " + ex.Message); }
    }
}