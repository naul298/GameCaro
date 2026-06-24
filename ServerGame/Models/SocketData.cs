namespace ServerGame.Models;

// Gói dữ liệu truyền qua socket — mọi lệnh đều dùng chung cấu trúc này
public class SocketData
{
    public int Command { get; set; }          // Loại lệnh (xem enum SocketCommand)
    public string Message { get; set; } = ""; // Nội dung kèm theo (tên, thông báo...)
    public Point Point { get; set; }          // Tọa độ ô cờ (nếu có)

    public SocketData(int command, string message, Point point)
    {
        Command = command;
        Message = message;
        Point = point;
    }
}