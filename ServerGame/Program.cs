// ServerGame/Program.cs
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.Data.SqlClient;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("===== CoCaro Server =====");

// ---- Chuỗi kết nối SQL Server ----
string connStr = "Server=.;Database=GameCaro;Trusted_Connection=True;TrustServerCertificate=True;";

Socket sckServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
sckServer.Bind(new IPEndPoint(IPAddress.Any, 12345));
sckServer.Listen(10);
Console.WriteLine("Đang lắng nghe cổng 12345...\n");

// Danh sách client đã login thành công
List<Socket> players = new List<Socket>();

// Chấp nhận và xác thực 2 người chơi
while (players.Count < 2)
{
    Socket client = sckServer.Accept();
    Console.WriteLine($"Có kết nối mới: {client.RemoteEndPoint}");

    byte[] buffer = new byte[1024];
    int n = client.Receive(buffer);
    string json = Encoding.UTF8.GetString(buffer, 0, n).Trim('\0');

    // Nhận gói login: { "Command": 0, "Message": "username|password", "Point": {"X":0,"Y":0} }
    var data = JsonSerializer.Deserialize<SocketData>(json);

    if (data == null || data.Command != (int)SocketCommand.LOGIN)
    {
        SendJson(client, new SocketData((int)SocketCommand.LOGIN_FAIL, "Dữ liệu không hợp lệ", new Point(0, 0)));
        client.Close();
        continue;
    }

    // Tách username|password
    var parts = data.Message.Split('|');
    if (parts.Length != 2)
    {
        SendJson(client, new SocketData((int)SocketCommand.LOGIN_FAIL, "Sai định dạng", new Point(0, 0)));
        client.Close();
        continue;
    }

    string username = parts[0];
    string password = parts[1];
    string? displayName = CheckLogin(connStr, username, password);

    if (displayName == null)
    {
        SendJson(client, new SocketData((int)SocketCommand.LOGIN_FAIL, "Sai tài khoản hoặc mật khẩu", new Point(0, 0)));
        client.Close();
        Console.WriteLine($"Login thất bại: {username}");
        continue;
    }

    // Login OK — gửi lại tên hiển thị và số thứ tự player
    int playerIndex = players.Count;
    SendJson(client, new SocketData((int)SocketCommand.LOGIN_OK, $"{displayName}|{playerIndex}", new Point(0, 0)));
    players.Add(client);
    Console.WriteLine($"Login OK: {displayName} (Player {playerIndex})");
}

Console.WriteLine("\nCả 2 người chơi đã vào. Bắt đầu game!\n");

// Relay 2 chiều
Thread t1 = new Thread(() => RelayLoop(players[0], players[1], "Player0"));
Thread t2 = new Thread(() => RelayLoop(players[1], players[0], "Player1"));
t1.IsBackground = true;
t2.IsBackground = true;
t1.Start();
t2.Start();

Console.WriteLine("Nhấn Enter để dừng server...");
Console.ReadLine();

// ---- Hàm kiểm tra login ----
static string? CheckLogin(string connStr, string username, string password)
{
    try
    {
        using SqlConnection conn = new SqlConnection(connStr);
        conn.Open();
        string sql = "SELECT display_name FROM Users WHERE username=@u AND password=@p";
        using SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@u", username);
        cmd.Parameters.AddWithValue("@p", password);
        var result = cmd.ExecuteScalar();
        return result?.ToString();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Lỗi DB: " + ex.Message);
        return null;
    }
}

static void RelayLoop(Socket source, Socket dest, string label)
{
    byte[] buffer = new byte[1024];
    try
    {
        while (true)
        {
            int n = source.Receive(buffer);
            if (n == 0) break;
            string msg = Encoding.UTF8.GetString(buffer, 0, n);
            Console.WriteLine($"[{label}] → {msg}");
            dest.Send(buffer, n, SocketFlags.None);
        }
    }
    catch
    {
        Console.WriteLine($"[{label}] mất kết nối.");
    }
}

static void SendJson(Socket s, object data)
{
    string json = JsonSerializer.Serialize(data);
    s.Send(Encoding.UTF8.GetBytes(json));
}

// ---- Model dùng chung ----
public class SocketData
{
    public int Command { get; set; }
    public string Message { get; set; } = "";
    public Point Point { get; set; }

    public SocketData(int command, string message, Point point)
    {
        Command = command; Message = message; Point = point;
    }
}

public record Point(int X, int Y) { public Point() : this(0, 0) { } }

public enum SocketCommand
{
    SEND_POINT = 0,
    THONG_BAO,
    CHOI_LAI,
    CAU_HOA,
    DAU_HANG,
    END,
    HET_GIO,
    THOAT_PHONG,
    LOGIN,       // = 8
    LOGIN_OK,    // = 9
    LOGIN_FAIL   // = 10
}