using System.Net.Sockets;

namespace ServerGame.Models;

// Thông tin 1 phiên chơi: gom socket + tên + chỉ số vào 1 object cho gọn
public class PlayerSession
{
    public Socket Socket { get; }       // Kết nối TCP của player
    public string DisplayName { get; }  // Tên hiển thị lấy từ DB
    public int Index { get; }           // 0 hoặc 1
    public string Label => $"Player{Index}({DisplayName})"; // Dùng để in log: "Player0(Luân)"

    public PlayerSession(Socket socket, string displayName, int index)
    {
        Socket = socket;
        DisplayName = displayName;
        Index = index;
    }
}