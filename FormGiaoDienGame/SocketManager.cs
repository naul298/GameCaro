using FormGiaoDienGame;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

public class SocketManager
{
    private Socket? _client;
    public string IP = "127.0.0.1";
    public int port = 12345;
    public const int BUFFER = 65536;

    // Event trung tâm — tất cả form đăng ký vào đây
    public event Action<SocketData>? OnDataReceived;

    private Thread? _receiveThread;
    private bool _isListening = false;

    public bool IsConnected =>
        _client != null && _client.Connected;

    public bool KetNoiServer()
    {
        try
        {
            if (IsConnected) return true;
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _client.Connect(new IPEndPoint(IPAddress.Parse(IP), port));
            StartReceiveLoop(); // bắt đầu nhận ngay sau khi kết nối
            return true;
        }
        catch { _client = null; return false; }
    }

    // Chỉ 1 thread nhận duy nhất — chạy suốt vòng đời kết nối
    private void StartReceiveLoop()
    {
        _isListening = true;
        _receiveThread = new Thread(() =>
        {
            while (_isListening)
            {
                try
                {
                    var data = ReceiveInternal();
                    if (data == null) break;
                    OnDataReceived?.Invoke(data); // gọi tất cả subscriber
                }
                catch { break; }
            }
        });
        _receiveThread.IsBackground = true;
        _receiveThread.Start();
    }

    public bool Send(object data)
    {
        if (_client == null) return false;
        byte[] bytes = SerializeData(data);
        return _client.Send(bytes) > 0;
    }

    public void Close()
    {
        _isListening = false;
        try { _client?.Close(); } catch { }
        _client = null;
    }

    // Internal receive — chỉ dùng trong StartReceiveLoop
    private SocketData? ReceiveInternal()
    {
        if (_client == null) return null;
        var sb = new StringBuilder();
        byte[] buf = new byte[BUFFER];
        do
        {
            int n = _client.Receive(buf);
            if (n == 0) return null;
            sb.Append(Encoding.UTF8.GetString(buf, 0, n));
        }
        while (!IsCompleteJson(sb.ToString()));
        return DeserializeData(sb.ToString());
    }

    private static bool IsCompleteJson(string s)
    {
        int depth = 0;
        bool inString = false;
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (c == '"' && (i == 0 || s[i - 1] != '\\')) inString = !inString;
            if (inString) continue;
            if (c == '{') depth++;
            else if (c == '}') depth--;
        }
        return depth == 0 && s.Contains('{');
    }

    public string GetLocalIPv4(NetworkInterfaceType type)
    {
        foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.NetworkInterfaceType != type) continue;
            if (nic.OperationalStatus != OperationalStatus.Up) continue;
            foreach (var addr in nic.GetIPProperties().UnicastAddresses)
                if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    return addr.Address.ToString();
        }
        return "";
    }

    private byte[] SerializeData(object o)
    {
        try { return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(o)); }
        catch { return Array.Empty<byte>(); }
    }

    private SocketData? DeserializeData(string json)
    {
        try { return JsonSerializer.Deserialize<SocketData>(json.Trim('\0')); }
        catch { return null; }
    }
}