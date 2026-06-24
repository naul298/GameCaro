using System;
using System.Collections.Generic;
using System.Text;

namespace FormGiaoDienGame
{
    public class RoomInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Status { get; set; } = "";
        public string HostName { get; set; } = "";
        public int PlayerCount { get; set; }
        public bool IsFull { get; set; }
    }
}
