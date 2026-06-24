namespace ServerGame.Models;

// Tọa độ ô cờ trên bàn — dùng record vì chỉ lưu dữ liệu thuần
public record Point(int X, int Y)
{
    public Point() : this(0, 0) { } // Constructor mặc định cho JSON deserialize
}