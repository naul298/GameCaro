namespace ServerGame.Models;

// Danh sách lệnh giao tiếp giữa client và server
public enum SocketCommand
{
    // --- Game ---
    SEND_POINT = 0,
    THONG_BAO = 1,
    CHOI_LAI = 2,
    CAU_HOA = 3,
    DAU_HANG = 4,
    END = 5,
    HET_GIO = 6,
    THOAT_PHONG = 7,

    // --- Auth ---
    LOGIN = 8,
    LOGIN_OK = 9,
    LOGIN_FAIL = 10,

    // --- Lobby ---
    GET_ROOMS = 11,  // Client xin danh sách phòng
    ROOMS_LIST = 12,  // Server trả danh sách phòng (JSON)
    CREATE_ROOM = 13,  // Client tạo phòng mới
    JOIN_ROOM = 14,  // Client vào phòng
    LEAVE_ROOM = 15,  // Client rời phòng
    ROOM_UPDATE = 16,  // Server broadcast cập nhật 1 phòng
    JOIN_OK = 17,  // Vào phòng thành công, bắt đầu game
    JOIN_FAIL = 18,  // Phòng đầy hoặc không tồn tại
    ROOM_DELETED = 19,  // Phòng đã bị xóa
}