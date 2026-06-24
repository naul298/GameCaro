using ServerGame.Core;
using System.Text;
<<<<<<< Updated upstream
using System.Text.Json;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("===== CoCaro Server =====");
Console.WriteLine("Đang lắng nghe cổng 12345...\n");

Socket sckServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
sckServer.Bind(new IPEndPoint(IPAddress.Any, 12345));
sckServer.Listen(10);

// Chờ đúng 2 client
Console.WriteLine("Chờ Client 1 kết nối...");
Socket client1 = sckServer.Accept();
Console.WriteLine($"Client 1 đã kết nối: {client1.RemoteEndPoint}");

Console.WriteLine("Chờ Client 2 kết nối...");
Socket client2 = sckServer.Accept();
Console.WriteLine($"Client 2 đã kết nối: {client2.RemoteEndPoint}\n");

Console.WriteLine("Cả 2 client đã vào. Bắt đầu game!\n");

// Thông báo cho mỗi client biết mình là player mấy
// Dùng đúng format SocketData của FormGiaoDienGame
SendJson(client1, new { Command = 99, Message = "PLAYER:0", Point = new { X = 0, Y = 0 } });
SendJson(client2, new { Command = 99, Message = "PLAYER:1", Point = new { X = 0, Y = 0 } });

// Relay 2 chiều: client1 <-> client2
Thread t1 = new Thread(() => RelayLoop(client1, client2, "Client1"));
Thread t2 = new Thread(() => RelayLoop(client2, client1, "Client2"));
t1.IsBackground = true;
t2.IsBackground = true;
t1.Start();
t2.Start();

Console.WriteLine("Server đang chạy. Nhấn Enter để dừng...");
Console.ReadLine();

// ---- Hàm tiện ích ----
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
    catch (Exception ex)
    {
        Console.WriteLine($"[{label}] mất kết nối: {ex.Message}");
    }
}

static void SendJson(Socket s, object data)
{
    string json = JsonSerializer.Serialize(data);
    s.Send(Encoding.UTF8.GetBytes(json));
}
=======


Console.OutputEncoding = Encoding.UTF8;
new GameServer().Start();
>>>>>>> Stashed changes
