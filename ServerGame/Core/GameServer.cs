using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ServerGame.Data;
using ServerGame.Models;

namespace ServerGame.Core;

// Server chính — lắng nghe, xác thực login, relay dữ liệu game
public class GameServer
{
    // ---- Hằng số cấu hình ----
    private const int PORT = 12345;
    private const string CONN_STR =
        "Server=(localdb)\\MSSQLLocalDB;Database=dbCaro;" +
        "Trusted_Connection=True;TrustServerCertificate=True;";

    // ---- Biến toàn cục của class ----
    private readonly List<PlayerSession> _players = new(); // Danh sách 2 player đã login

    // Kịch bản tổng: tạo socket → chờ 2 người login → relay dữ liệu
    public void Start()
    {
        Console.WriteLine("===== CoCaro Server =====");

        Socket sckServer = TaoServerSocket(PORT);
        Console.WriteLine($"Đang lắng nghe cổng {PORT}...\n");

        AcceptPlayers(sckServer);   // Chờ đủ 2 người login

        Console.WriteLine("\nCả 2 người chơi đã vào. Bắt đầu game!\n");

        StartRelay();               // Bắt đầu chuyển tiếp dữ liệu

        Console.WriteLine("Nhấn Enter để dừng server...");
        Console.ReadLine();
    }

    // Tạo socket server, gán cổng và bắt đầu lắng nghe kết nối
    private static Socket TaoServerSocket(int port)
    {
        var sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sck.Bind(new IPEndPoint(IPAddress.Any, port)); // Lắng nghe trên tất cả card mạng
        sck.Listen(10);                                // Tối đa 10 kết nối chờ trong hàng đợi
        return sck;
    }

    // Vòng lặp chờ đủ 2 player login hợp lệ mới thoát
    private void AcceptPlayers(Socket sckServer)
    {
        while (_players.Count < 2)
        {
            Socket client = sckServer.Accept(); // Chặn tại đây cho đến khi có người kết nối
            Console.WriteLine($"Có kết nối mới: {client.RemoteEndPoint}");

            // Xác thực login — trả về null nếu sai, tiếp tục chờ người khác
            PlayerSession? session = XacThucLogin(client);
            if (session == null) continue;

            _players.Add(session);
            Console.WriteLine($"[{_players.Count}/2] người chơi đã vào.");
        }
    }

    // Xác thực login theo từng bước, trả về PlayerSession nếu hợp lệ
    private PlayerSession? XacThucLogin(Socket client)
    {
        SocketData? data = NhanGoiDau(client);

        // Bước 1: kiểm tra gói có đúng là gói LOGIN không
        if (data == null || data.Command != (int)SocketCommand.LOGIN)
        {
            GuiJson(client, SocketCommand.LOGIN_FAIL, "Dữ liệu không hợp lệ");
            client.Close();
            return null;
        }

        // Bước 2: kiểm tra định dạng "username|password"
        var parts = data.Message.Split('|');
        if (parts.Length != 2)
        {
            GuiJson(client, SocketCommand.LOGIN_FAIL, "Sai định dạng");
            client.Close();
            return null;
        }

        // Bước 3: kiểm tra tài khoản trong DB
        string username = parts[0];
        string password = parts[1];
        string? displayName = DatabaseHelper.KiemTraLogin(CONN_STR, username, password);

        if (displayName == null)
        {
            GuiJson(client, SocketCommand.LOGIN_FAIL, "Sai tài khoản hoặc mật khẩu");
            client.Close();
            Console.WriteLine($"Login thất bại: {username}");
            return null;
        }

        // Bước 4: login OK — gửi về tên + chỉ số player (0 hoặc 1)
        int playerIndex = _players.Count;
        GuiJson(client, SocketCommand.LOGIN_OK, $"{displayName}|{playerIndex}");
        Console.WriteLine($"Login OK: {displayName} (Player {playerIndex})");

        return new PlayerSession(client, displayName, playerIndex);
    }

    // Nhận đúng 1 gói JSON đầu tiên từ client, chuyển thành SocketData
    private static SocketData? NhanGoiDau(Socket client)
    {
        byte[] buffer = new byte[1024];
        int n = client.Receive(buffer);
        string json = Encoding.UTF8.GetString(buffer, 0, n).Trim('\0');
        return JsonSerializer.Deserialize<SocketData>(json);
    }

    // Gửi gói JSON với command và message, Point mặc định (0,0)
    private static void GuiJson(Socket s, SocketCommand cmd, string message = "")
    {
        string json = JsonSerializer.Serialize(
            new SocketData((int)cmd, message, new Point(0, 0)));
        s.Send(Encoding.UTF8.GetBytes(json));
    }

    // Tạo RelayService và bắt đầu chuyển tiếp dữ liệu 2 chiều
    private void StartRelay()
    {
        var relay = new RelayService(_players[0], _players[1]);
        relay.Start();
    }
}