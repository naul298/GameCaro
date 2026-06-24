using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;

namespace FormGiaoDienGame
{
    internal class SocketManager
    {
        #region Client
        Socket client;
        public bool KetNoiServer()
        {
            //Tao socket
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Ket noi den server
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), port);
            try
            {
                client.Connect(ep);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Server
        Socket server;
        public void TaoServer()
        {
            //Tao socket
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Ket noi den server
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IP), port);
            server.Bind(ep);
            server.Listen(10);
            Thread acceptClient = new Thread(() => { 
                 client = server.Accept();
            });
            acceptClient.IsBackground = true;
            acceptClient.Start();
        }
        #endregion

        #region Common
        public string IP = "192.168.2.8";
        public int port = 12345;
        public const int BUFFER = 1024;
        public bool isServer = true;
        
        public bool Send(object data)
        {
            byte[] sendData = SerializeData(data);

            return SendData(client, sendData);
        }

        public object Receive()
        {
            byte[] receiveData = new byte[BUFFER];
            bool isOk = ReceiveData(client, receiveData);

            return DeserializeData(receiveData);
        }
        private bool SendData(Socket target, byte[] data)
        {
            return target.Send(data) > 0;
        }
        private bool ReceiveData(Socket target, byte[] data)
        {
            return target.Receive(data) > 0;
        }
        public string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
        public object DeserializeData(byte[] theByteArray)
        {
            try
            {
                //Dịch mảng byte thành chuỗi văn bản UTF-8
                string jsonString = Encoding.UTF8.GetString(theByteArray);

                // Nếu chuỗi JSON thực tế chỉ dài 100 byte, 924 byte còn lại sẽ là các ký tự rỗng ('\0').
                // Ta cần phải cắt bỏ các ký tự rỗng này trước khi Deserialize, nếu không JSON sẽ báo lỗi.
                jsonString = jsonString.Trim('\0');

                //Chuyển chuỗi JSON thành đối tượng GamePacket
                return JsonSerializer.Deserialize<object>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi Deserialize: " + ex.Message);
                return null; // Trả về null nếu mảng byte bị lỗi hoặc không đúng định dạng GamePacket
            }
        }
        public byte[] SerializeData(object o)
        {
            try
            {
                // Chuyển đối tượng (thường là GamePacket) thành chuỗi JSON
                string jsonString = JsonSerializer.Serialize(o);

                // Mã hóa chuỗi JSON thành mảng byte chuẩn UTF-8
                return Encoding.UTF8.GetBytes(jsonString);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có (ghi log, hiển thị console...)
                Console.WriteLine("Lỗi Serialize: " + ex.Message);
                return new byte[0];
            }
        }
        #endregion
    }
}
