using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerGame
{
    internal class Program
    {
        static int SeverPort = 9999;

        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                SeverPort = int.Parse(args[0]);
            }

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("===== Double chat TCP Server =====");
            Console.WriteLine($"Server listening on port {SeverPort}...\n");
            // Khởi tạo socket làm nhiệm vụ tiếp nhận kết nối
            Socket sckServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Liên kết socket với cổng dịch vụ trên máy hiện tại
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, SeverPort);
            sckServer.Bind(ipEndPoint);
            // Đưa socket vào trạng thái lắng nghe
            sckServer.Listen(10);
            // Đợi người dùng thứ nhất tham gia
            Console.WriteLine("Waiting for Client 1...");
            Socket sckClient1 = sckServer.Accept();
            Console.WriteLine("Client 1 connected!\n");
            // Đợi người dùng thứ hai tham gia
            Console.WriteLine("Waiting for Client 2...");
            Socket sckClient2 = sckServer.Accept();
            Console.WriteLine("Client 2 connected!\n");
            Console.WriteLine("Both clients connected. Starting chat...\n");
            Console.WriteLine("========================================\n");
            // Vùng nhớ tạm để chứa dữ liệu trao đổi
            byte[] data = new byte[1024];
            int soByteNhan;
            while (true)
            {
                // Nhận nội dung từ client thứ nhất
                Console.Write("Client 1 → Server: ");
                soByteNhan = sckClient1.Receive(data);
                if (soByteNhan == 0)
                    break;
                string noiDungClient1 = Encoding.UTF8.GetString(data, 0, soByteNhan);
                Console.WriteLine(noiDungClient1);
                // Chuyển tiếp tin nhắn sang client thứ hai
                sckClient2.Send(data, soByteNhan, SocketFlags.None);
                Console.WriteLine($"Server → Client 2: {noiDungClient1}\n");
                // Nhận nội dung từ client thứ hai
                Console.Write("Client 2 → Server: ");
                soByteNhan = sckClient2.Receive(data);
                if (soByteNhan == 0)
                    break;
                string noiDungClient2 = Encoding.UTF8.GetString(data, 0, soByteNhan);
                Console.WriteLine(noiDungClient2);
                // Chuyển tiếp tin nhắn sang client thứ nhất
                sckClient1.Send(data, soByteNhan, SocketFlags.None);
                Console.WriteLine($"Server → Client 1: {noiDungClient2}\n");
            }
            sckClient1.Close();
            sckClient2.Close();
            sckServer.Close();
            Console.WriteLine("Ket thuc thanh cong!");
            Console.ReadLine();
        }
    }
}