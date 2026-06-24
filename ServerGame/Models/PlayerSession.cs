using System.Net.Sockets;

namespace ServerGame.Models;

public class PlayerSession
{
    public Socket Socket { get; }
    public string DisplayName { get; }
    public int Index { get; set; }  // 0 hoặc 1 (trong phòng)
    public int UserId { get; set; }
    public int CurrentRoomId { get; set; } = -1;  // -1 = ở lobby
    public string Label => $"Player{Index}({DisplayName})";

    public PlayerSession(Socket socket, string displayName, int index, int userId = 0)
    {
        Socket = socket;
        DisplayName = displayName;
        Index = index;
        UserId = userId;
    }
}