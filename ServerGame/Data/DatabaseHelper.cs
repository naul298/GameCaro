using Microsoft.Data.SqlClient;

namespace ServerGame.Data;

// Xử lý toàn bộ truy vấn liên quan đến DB
public static class DatabaseHelper
{
    // Truy vấn DB: trả về display_name nếu đúng tài khoản, null nếu sai hoặc lỗi
    public static string? KiemTraLogin(string connStr, string username, string password)
    {
        try
        {
            using SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            // Dùng tham số @u, @p thay vì ghép string — tránh SQL Injection
            string sql = "SELECT display_name FROM Users WHERE username=@u AND password=@p";
            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);

            return cmd.ExecuteScalar()?.ToString(); // null nếu không tìm thấy
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi DB: " + ex.Message);
            return null;
        }
    }
}