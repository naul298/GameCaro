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
        public const int BUFFER = 65536;

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
        private static bool IsCompleteJson(string s) // Kiểm tra JSON đã đầy đủ chưa (đếm { và })
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

        public object? Receive()
        {
            if (_client == null) return null;

            // Đọc cho đến khi nhận đủ 1 JSON object hoàn chỉnh
            var sb = new System.Text.StringBuilder();
            byte[] buf = new byte[BUFFER];

            do
            {
                int n = _client.Receive(buf);
                if (n == 0) return null;
                sb.Append(System.Text.Encoding.UTF8.GetString(buf, 0, n));
            }
            // JSON object kết thúc bằng } — kiểm tra chuỗi đã cân bằng chưa
            while (!IsCompleteJson(sb.ToString()));

            return DeserializeData(sb.ToString());
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

        private object? DeserializeData(string json)
        {
            try
            {
                return System.Text.Json.JsonSerializer.Deserialize<SocketData>(
                    json.Trim('\0'));
            }
            catch { return null; }
        }
    }
}