using ServerGame.Data;
using ServerGame.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
namespace ServerGame.Core;

public class GameServer
{
    private const int PORT = 12345;
    private const int DISCOVERY_PORT = 12346; // cổng phụ chỉ dùng để tìm server

    private static readonly string CONN_STR = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\GameCaro\data\dataCaro.mdf;Integrated Security=True;";    // Danh sách tất cả client đang kết nối và tất cả phòng
    private readonly List<PlayerSession> _clients = new();
    private readonly List<LobbyRoom> _rooms = new();
    private readonly object _lock = new(); // Lock cho thread-safe

    public void Start()
    {
        Console.WriteLine("===== Caro Online =====");
        LoadRoomsFromDb();   // khởi tạo DB trước
        BatDauDiscovery();   // mở cổng discovery sau khi DB đã sẵn sàng

        var sckServer = TaoServerSocket(PORT);
        Console.WriteLine($"Đang lắng nghe cổng {PORT}...\n");

        while (true)
        {
            Socket client = sckServer.Accept();
            Console.WriteLine($"Kết nối mới: {client.RemoteEndPoint}");
            var t = new Thread(() => HandleClient(client));
            t.IsBackground = true;
            t.Start();
        }
    }
    private void BatDauDiscovery()
    {
        var t = new Thread(() =>
        {
            // Mở socket lắng nghe trên cổng phụ
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, DISCOVERY_PORT));
            listener.Listen(10);
            Console.WriteLine($"Discovery đang lắng nghe cổng {DISCOVERY_PORT}...");

            while (true)
            {
                try
                {
                    Socket client = listener.Accept(); // chờ client kết nối vào cổng phụ
                    byte[] buf = new byte[64];
                    int n = client.Receive(buf);
                    string msg = Encoding.UTF8.GetString(buf, 0, n);

                    // Chỉ phản hồi đúng mật khẩu nhận dạng
                    if (msg == "FIND_SERVER")
                        client.Send(Encoding.UTF8.GetBytes("CARO_SERVER"));

                    client.Close(); // đóng ngay sau khi trả lời, không giữ kết nối
                }
                catch { }
            }
        });
        t.IsBackground = true;
        t.Start();
    }
    private void XuLyDangKy(Socket socket, SocketData data)
    {
        // Format: "username|password|displayName"
        var parts = data.Message.Split('|');
        if (parts.Length != 3)
        {
            GuiJson(socket, SocketCommand.REGISTER_FAIL, "Dữ liệu không hợp lệ.");
            return;
        }

        string username = parts[0].Trim();
        string password = parts[1].Trim();
        string displayName = parts[2].Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)
            || string.IsNullOrEmpty(displayName))
        {
            GuiJson(socket, SocketCommand.REGISTER_FAIL, "Vui lòng điền đầy đủ thông tin.");
            return;
        }

        var (ok, message) = DatabaseHelper.CreateUser(CONN_STR, username, password, displayName);
        GuiJson(socket, ok ? SocketCommand.REGISTER_OK : SocketCommand.REGISTER_FAIL, message);
        Console.WriteLine($"[Register] {username} → {message}");
    }
    // Load phòng từ DB vào memory khi khởi động
    private void LoadRoomsFromDb()
    {
        DatabaseHelper.XoaPhongRac(CONN_STR); // dọn phòng rác trước khi load

        var rows = DatabaseHelper.LoadAllRooms(CONN_STR);
        foreach (var (id, name, isDefault) in rows)
            _rooms.Add(new LobbyRoom { Id = id, Name = name, IsDefault = isDefault });
        Console.WriteLine($"Đã load {_rooms.Count} phòng từ DB.");
    }

    // Xử lý toàn bộ vòng đời 1 client
    private void HandleClient(Socket socket)
    {
        // Đọc gói đầu tiên — có thể là LOGIN hoặc REGISTER
        SocketData? firstPacket = NhanGoi(socket);
        if (firstPacket == null) { socket.Close(); return; }

        // Nếu là REGISTER → xử lý rồi đóng kết nối (client sẽ reconnect để login)
        if (firstPacket.Command == (int)SocketCommand.REGISTER)
        {
            XuLyDangKy(socket, firstPacket);
            socket.Close();
            return;
        }

        // Nếu không phải LOGIN → từ chối
        if (firstPacket.Command != (int)SocketCommand.LOGIN)
        {
            GuiJson(socket, SocketCommand.LOGIN_FAIL, "Dữ liệu không hợp lệ.");
            socket.Close();
            return;
        }

        // Xác thực login như cũ
        PlayerSession? session = XacThucLogin(socket, firstPacket);
        if (session == null) return;

        lock (_lock) _clients.Add(session);
        Console.WriteLine($"[+] {session.DisplayName} vào lobby.");

        try
        {
            while (true)
            {
                SocketData? data = NhanGoi(socket);
                if (data == null) break;
                HandleCommand(session, data);
            }
        }
        catch { }
        finally { OnClientDisconnect(session); }
    }

    // Xử lý từng lệnh client gửi lên
    private void HandleCommand(PlayerSession session, SocketData data)
    {
        switch ((SocketCommand)data.Command)
        {
            // ── Lobby ───────────────────────────────────────────────
            case SocketCommand.GET_ROOMS:
                GuiDanhSachPhong(session);
                break;

            case SocketCommand.CREATE_ROOM:
                TaoPhong(session, data.Message);
                break;

            case SocketCommand.JOIN_ROOM:
                VaoPhong(session, int.Parse(data.Message));
                break;

            case SocketCommand.LEAVE_ROOM:
                RoiPhong(session);
                break;

            // ── Phòng chờ ───────────────────────────────────────────
            case SocketCommand.READY:
                XuLyReady(session);
                break;

            // ── Gameplay — relay thẳng sang đối thủ ─────────────────
            case SocketCommand.SEND_POINT:
            case SocketCommand.END:
            case SocketCommand.HET_GIO:
            case SocketCommand.THOAT_PHONG:
                RelayToOpponent(session, data);
                break;
            case SocketCommand.CAU_HOA:
            case SocketCommand.CHOI_LAI:
            case SocketCommand.DAU_HANG:
                RelayToOpponent(session, data);
                break;
        }
    }

    private void XuLyReady(PlayerSession session)
    {
        lock (_lock)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == session.CurrentRoomId);
            if (room == null) return;

            room.SetReady(session);
            Console.WriteLine($"[Ready] {session.DisplayName} đã sẵn sàng.");

            if (room.BothReady())
            {
                room.ResetReady();
                int firstMover = room.NextFirstMover();

                Console.WriteLine($"[Game] Bắt đầu! Người đi trước: index {firstMover}");

                foreach (var p in room.Players)
                    GuiJson(p.Socket, SocketCommand.START_GAME, firstMover.ToString());
            }
        }
    }
    private void GuiDanhSachPhong(PlayerSession session)
    {
        lock (_lock)
        {
            // Serialize toàn bộ danh sách phòng thành JSON array
            var list = _rooms.Select(r => new
            {
                r.Id,
                r.Name,
                r.Status,
                r.HostName,
                r.PlayerCount,
                r.IsFull
            });
            string json = JsonSerializer.Serialize(list);
            GuiJson(session.Socket, SocketCommand.ROOMS_LIST, json);
        }
    }

    private void TaoPhong(PlayerSession session, string roomName)
    {
        lock (_lock)
        {
            // Tạo trong DB
            int newId = DatabaseHelper.CreateRoom(CONN_STR, roomName, session.UserId);
            if (newId < 0)
            {
                GuiJson(session.Socket, SocketCommand.JOIN_FAIL, "Không thể tạo phòng.");
                return;
            }

            var room = new LobbyRoom { Id = newId, Name = roomName, IsDefault = false };
            room.Players.Add(session);
            session.CurrentRoomId = newId;
            session.Index = 0;

            _rooms.Add(room);
            DatabaseHelper.UpdateRoom(CONN_STR, newId, session.UserId, 1, "Waiting");

            // Báo người tạo: vào phòng thành công với index 0
            GuiJson(session.Socket, SocketCommand.JOIN_OK,
                $"{room.Name}|0|{session.DisplayName}|");

            // Broadcast cập nhật lên tất cả lobby
            BroadcastRoomUpdate(room);
            Console.WriteLine($"[Phòng] '{roomName}' được tạo bởi {session.DisplayName}");
        }
    }

    private void VaoPhong(PlayerSession session, int roomId)
    {
        lock (_lock)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == roomId);

            if (room == null)
            {
                GuiJson(session.Socket, SocketCommand.JOIN_FAIL, "Phòng không tồn tại.");
                return;
            }

            if (room.IsFull)
            {
                GuiJson(session.Socket, SocketCommand.JOIN_FAIL, "Phòng đã đầy.");
                return;
            }

            room.Players.Add(session);
            session.CurrentRoomId = roomId;
            session.Index = room.Players.Count - 1;

            DatabaseHelper.UpdateRoom(CONN_STR, roomId, room.HostId, room.PlayerCount, room.IsFull ? "Playing" : "Waiting");

            if (room.IsFull)
            {
                var p0 = room.Players[0];
                var p1 = room.Players[1];

                // Guest (p1) nhận JOIN_OK → FormLobby chuyển sang FormGame
                GuiJson(p1.Socket, SocketCommand.JOIN_OK,
                    $"{room.Name}|1|{p1.DisplayName}|{p0.DisplayName}");

                // Host (p0) đang ở FormGame → gửi OPPONENT_JOINED để mở btnSanSang
                GuiJson(p0.Socket, SocketCommand.OPPONENT_JOINED,
                    $"{room.Name}|0|{p0.DisplayName}|{p1.DisplayName}");

                Console.WriteLine($"[Phòng] '{room.Name}': {p0.DisplayName} vs {p1.DisplayName} — đủ người!");
            }
            else
            {
                // Chỉ mình host trong phòng, chờ đối thủ
                GuiJson(session.Socket, SocketCommand.JOIN_OK,
                    $"{room.Name}|0|{session.DisplayName}|");
            }

            BroadcastRoomUpdate(room);
        }
    }

    private void RoiPhong(PlayerSession session)
    {
        lock (_lock) { XuLyRoiPhong(session); }
    }

    private void XuLyRoiPhong(PlayerSession session)
    {
        if (session.CurrentRoomId < 0)
        {
            Console.WriteLine($"[RoiPhong] {session.DisplayName} không ở trong phòng nào.");
            return;
        }

        var room = _rooms.FirstOrDefault(r => r.Id == session.CurrentRoomId);
        if (room == null)
        {
            Console.WriteLine($"[RoiPhong] Không tìm thấy phòng {session.CurrentRoomId}.");
            return;
        }

        Console.WriteLine($"[RoiPhong] {session.DisplayName} rời phòng '{room.Name}' — trước: {room.PlayerCount} người.");

        room.RemovePlayer(session);
        session.CurrentRoomId = -1;

        Console.WriteLine($"[RoiPhong] Sau khi xóa: {room.PlayerCount} người. IsEmpty={room.IsEmpty} IsDefault={room.IsDefault}");

        if (room.IsEmpty && !room.IsDefault)
        {
            _rooms.Remove(room);
            DatabaseHelper.DeleteRoom(CONN_STR, room.Id);
            BroadcastToLobby(SocketCommand.ROOM_DELETED, room.Id.ToString());
            Console.WriteLine($"[RoiPhong] Phòng '{room.Name}' đã bị xóa.");
        }
        else
        {
            int newHostId = room.Players.Count > 0 ? room.Players[0].UserId : room.HostId;
            Console.WriteLine($"[RoiPhong] UpdateRoom id={room.Id} hostId={newHostId} playerCount={room.PlayerCount}");
            DatabaseHelper.UpdateRoom(CONN_STR, room.Id, newHostId, room.PlayerCount, "Waiting");

            if (room.Players.Count > 0)
            {
                room.Players[0].Index = 0;
                GuiJson(room.Players[0].Socket, SocketCommand.LEAVE_ROOM, "Đối thủ đã rời phòng. Bạn đã thắng.");
            }

            BroadcastRoomUpdate(room);
        }
    }

    private void OnClientDisconnect(PlayerSession session)
    {
        lock (_lock)
        {
            Console.WriteLine($"[-] {session.DisplayName} ngắt kết nối.");
            XuLyRoiPhong(session);
            _clients.Remove(session);
        }
        try { session.Socket.Close(); } catch { }
    }

    private void RelayToOpponent(PlayerSession session, SocketData data)
    {
        lock (_lock)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == session.CurrentRoomId);
            if (room == null) return;
            var opponent = room.Players.FirstOrDefault(p => p != session);
            if (opponent == null) return;

            string json = JsonSerializer.Serialize(data);
            try { opponent.Socket.Send(Encoding.UTF8.GetBytes(json)); }
            catch { }
        }
    }

    private void BroadcastRoomUpdate(LobbyRoom room)
    {
        string payload = room.ToJson();
        foreach (var client in _clients.Where(c => c.CurrentRoomId < 0))
            try { GuiJson(client.Socket, SocketCommand.ROOM_UPDATE, payload); }
            catch { }
    }

    private void BroadcastToLobby(SocketCommand cmd, string message)
    {
        foreach (var client in _clients.Where(c => c.CurrentRoomId < 0))
            try { GuiJson(client.Socket, cmd, message); }
            catch { }
    }

    private PlayerSession? XacThucLogin(Socket client, SocketData data)
    {
        var parts = data.Message.Split('|');
        if (parts.Length != 2)
        {
            GuiJson(client, SocketCommand.LOGIN_FAIL, "Sai định dạng.");
            client.Close(); return null;
        }

        string username = parts[0].Trim();
        string password = parts[1].Trim();

        // Kiểm tra tài khoản đã đăng nhập chưa — chống login trùng
        lock (_lock)
        {
            bool alreadyOnline = _clients.Any(c => c.DisplayName != null &&
                DatabaseHelper.GetUserId(CONN_STR, username) == c.UserId);
            if (alreadyOnline)
            {
                GuiJson(client, SocketCommand.LOGIN_FAIL, "Tài khoản đang được sử dụng ở nơi khác.");
                client.Close(); return null;
            }
        }

        string? displayName = DatabaseHelper.KiemTraLogin(CONN_STR, username, password);
        if (displayName == null)
        {
            GuiJson(client, SocketCommand.LOGIN_FAIL, "Sai tài khoản hoặc mật khẩu.");
            client.Close(); return null;
        }

        int userId = DatabaseHelper.GetUserId(CONN_STR, username);
        GuiJson(client, SocketCommand.LOGIN_OK, displayName);
        return new PlayerSession(client, displayName, -1, userId);
    }

    private static SocketData? NhanGoi(Socket client)
    {
        try
        {
            var sb = new StringBuilder();
            byte[] buf = new byte[65536];

            do
            {
                int n = client.Receive(buf);
                if (n == 0) return null; // client ngắt kết nối bình thường
                sb.Append(Encoding.UTF8.GetString(buf, 0, n));
            }
            while (!IsCompleteJson(sb.ToString()));

            return JsonSerializer.Deserialize<SocketData>(sb.ToString().Trim('\0'));
        }
        catch (SocketException) { return null; } // client tắt đột ngột → trả null để server xử lý disconnect
        catch (Exception) { return null; }       // các lỗi khác → tương tự
    }
    private static bool IsCompleteJson(string s)
    {
        int depth = 0;       // đếm độ sâu lồng nhau của { }
        bool inString = false; // đang trong chuỗi "" hay không

        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (c == '"' && (i == 0 || s[i - 1] != '\\')) inString = !inString; // đổi trạng thái khi gặp " không bị escape
            if (inString) continue; // bỏ qua { } bên trong chuỗi
            if (c == '{') depth++;  // mở ngoặc → tăng độ sâu
            else if (c == '}') depth--; // đóng ngoặc → giảm độ sâu
        }

        // depth == 0 nghĩa là mọi { đã có } tương ứng → JSON hoàn chỉnh
        return depth == 0 && s.Contains('{');
    }
    private static void GuiJson(Socket s, SocketCommand cmd, string message = "")
    {
        string json = JsonSerializer.Serialize(
            new SocketData((int)cmd, message, new Point(0, 0)));
        s.Send(Encoding.UTF8.GetBytes(json));
    }
    private static Socket TaoServerSocket(int port)
    {
        var sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sck.Bind(new IPEndPoint(IPAddress.Any, port));
        sck.Listen(100);
        return sck;
    }
}