using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace FormGiaoDienGame
{
    public class SocketManager
    {
        private Socket? _client;
        public string IP = "127.0.0.1";
        public int port = 12345;
        public const int BUFFER = 1024;

        // Kiểm tra đã kết nối chưa (không tạo socket mới)
        public bool IsConnected =>
            _client != null && _client.Connected;

        // Kết nối đến server — chỉ gọi 1 lần
        public bool KetNoiServer()
        {
            try
            {
                // Nếu đang kết nối rồi thì không làm gì
                if (IsConnected) return true;

                _client = new Socket(AddressFamily.InterNetwork,
                                     SocketType.Stream, ProtocolType.Tcp);
                _client.Connect(new IPEndPoint(IPAddress.Parse(IP), port));
                return true;
            }
            catch
            {
                _client = null;
                return false;
            }
        }

        public bool Send(object data)
        {
            if (_client == null) return false;
            byte[] bytes = SerializeData(data);
            return _client.Send(bytes) > 0;
        }

        public object? Receive()
        {
            if (_client == null) return null;
            byte[] buf = new byte[BUFFER];
            int n = _client.Receive(buf);
            return DeserializeData(buf, n);
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

        private object? DeserializeData(byte[] buf, int length)
        {
            try
            {
                string json = Encoding.UTF8.GetString(buf, 0, length).Trim('\0');
                return JsonSerializer.Deserialize<SocketData>(json);
            }
            catch { return null; }
        }
    }
}