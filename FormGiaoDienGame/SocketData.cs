using System;
using System.Collections.Generic;
using System.Text;

namespace FormGiaoDienGame
{
    [Serializable]
    public class SocketData
    {
        private int command;
        private Point point;
        private string message;

        public int Command { get => command; set => command = value; }
        public Point Point { get => point; set => point = value; }
        public string Message { get => message; set => message = value; }

        public SocketData(int command, string message, Point point)
        {
            this.Command = command;
            this.Message = message;
            this.Point = point;
        }
    }

    public enum SocketCommand
    {
        SEND_POINT,   // = 0
        THONG_BAO,    // = 1
        CHOI_LAI,     // = 2
        CAU_HOA,      // = 3
        DAU_HANG,     // = 4
        END,          // = 5
        HET_GIO,      // = 6
        THOAT_PHONG,  // = 7
        LOGIN,        // = 8
        LOGIN_OK,     // = 9
        LOGIN_FAIL    // = 10
    }
}