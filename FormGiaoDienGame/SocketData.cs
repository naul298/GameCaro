namespace FormGiaoDienGame
{
    [Serializable]
    public class SocketData
    {
        public int Command { get; set; }
        public string Message { get; set; } = "";
        public Point Point { get; set; }

        public SocketData(int command, string message, Point point)
        {
            Command = command;
            Message = message;
            Point = point;
        }
    }

    public enum SocketCommand
    {
        SEND_POINT = 0,
        THONG_BAO = 1,
        CHOI_LAI = 2,
        CAU_HOA = 3,
        DAU_HANG = 4,
        END = 5,
        HET_GIO = 6,
        THOAT_PHONG = 7,
        LOGIN = 8,
        LOGIN_OK = 9,
        LOGIN_FAIL = 10,
        GET_ROOMS = 11,
        ROOMS_LIST = 12,
        CREATE_ROOM = 13,
        JOIN_ROOM = 14,
        LEAVE_ROOM = 15,
        ROOM_UPDATE = 16,
        JOIN_OK = 17,
        JOIN_FAIL = 18,
        ROOM_DELETED = 19,
        REGISTER = 20,       // Client gửi yêu cầu đăng ký
        REGISTER_OK = 21,    // Server báo thành công
        REGISTER_FAIL = 22,  // Server báo thất bại (trùng username, v.v.)
    }
}