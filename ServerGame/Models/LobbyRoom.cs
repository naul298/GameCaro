namespace ServerGame.Models;

// Trạng thái 1 phòng trong bộ nhớ server
public class LobbyRoom
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsDefault { get; set; }      // 10 phòng mặc định
    public string Status { get; set; } = "Waiting";
    private bool _readyP0 = false;
    private bool _readyP1 = false;
    public int LastLoser = -1; // -1 = chưa có ván nào

    // Tối đa 2 người: [0] = host, [1] = guest
    public List<PlayerSession> Players { get; } = new();

    public int HostId => Players.Count > 0 ? Players[0].UserId : 0;
    public string HostName => Players.Count > 0 ? Players[0].DisplayName : "";
    public int PlayerCount => Players.Count;
    public bool IsFull => Players.Count >= 2;
    public bool IsEmpty => Players.Count == 0;

    // Xóa player, nhường host cho người còn lại nếu cần
    public void RemovePlayer(PlayerSession session)
    {
        Players.Remove(session);
        // Players[0] tự động thành host mới nếu host cũ rời đi
    }
    public void SetReady(PlayerSession session)
    {
        if (session == Players[0]) _readyP0 = true;
        else if (session == Players[1]) _readyP1 = true;
    }
    public bool BothReady() => _readyP0 && _readyP1;
    public void ResetReady()
    {
        _readyP0 = false;
        _readyP1 = false;
    }
    public int NextFirstMover()
    {
        // Ván đầu: host (index 0) đi trước
        // Ván sau: người thua đi trước
        return LastLoser == -1 ? 0 : LastLoser;
    }
    // Chuyển object sang string JSON gọn để gửi qua socket
    public string ToJson() =>
        System.Text.Json.JsonSerializer.Serialize(new
        {
            Id,
            Name,
            Status,
            HostName,
            PlayerCount,
            IsFull
        });
}