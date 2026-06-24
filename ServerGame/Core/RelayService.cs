using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ServerGame.Models;

namespace ServerGame.Core;

// Chuyển tiếp dữ liệu 2 chiều giữa 2 player
public class RelayService
{
    // ---- Biến toàn cục của class ----
    private readonly PlayerSession _p0; // Player 0
    private readonly PlayerSession _p1; // Player 1

    public RelayService(PlayerSession p0, PlayerSession p1)
    {
        _p0 = p0;
        _p1 = p1;
    }

    // Tạo 2 thread relay ngược chiều nhau: p0→p1 và p1→p0
    public void Start()
    {
        StartThread(_p0.Socket, _p1.Socket, _p0.Label);
        StartThread(_p1.Socket, _p0.Socket, _p1.Label);
    }

    // Tạo 1 thread chạy nền, relay từ source → dest
    private static void StartThread(Socket source, Socket dest, string label)
    {
        var t = new Thread(() => RelayLoop(source, dest, label));
        t.IsBackground = true; // Thread tự tắt khi app tắt
        t.Start();
    }

    // Vòng lặp nhận từ source rồi gửi sang dest liên tục
    private static void RelayLoop(Socket source, Socket dest, string label)
    {
        byte[] buffer = new byte[1024];
        try
        {
            while (true)
            {
                int n = source.Receive(buffer); // Chặn tại đây cho đến khi có dữ liệu
                if (n == 0) break;              // Client ngắt kết nối bình thường

                string msg = Encoding.UTF8.GetString(buffer, 0, n);
                Console.WriteLine($"[{label}] → {msg}");
                dest.Send(buffer, n, SocketFlags.None); // Chuyển tiếp nguyên gói sang đối thủ
            }
        }
        catch
        {
            Console.WriteLine($"[{label}] mất kết nối đột ngột.");
        }
        finally
        {
            // Dù ngắt kiểu nào (bình thường hay đột ngột) cũng báo cho đối thủ
            ThongBaoThoat(dest, label);
        }
    }

    // Gửi lệnh THOAT_PHONG cho đối thủ khi 1 bên mất kết nối
    private static void ThongBaoThoat(Socket dest, string label)
    {
        try
        {
            string json = JsonSerializer.Serialize(
                new SocketData((int)SocketCommand.THOAT_PHONG, "", new Point(0, 0)));
            dest.Send(Encoding.UTF8.GetBytes(json));
        }
        catch
        {
            Console.WriteLine($"[{label}] không thể thông báo thoát cho đối thủ.");
        }
    }
}