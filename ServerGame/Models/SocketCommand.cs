namespace ServerGame.Models;

public enum SocketCommand
{
    // ── Gameplay ────────────────────────────────
    SEND_POINT = 0,  // Gửi tọa độ nước đi
    THONG_BAO = 1,  // [Chưa dùng] Thông báo chung
    CHOI_LAI = 2,  // Yêu cầu / phản hồi chơi lại (Message: "" | "OK" | "NO")
    CAU_HOA = 3,  // Yêu cầu / phản hồi cầu hòa (Message: "" | "OK" | "NO")
    DAU_HANG = 4,  // Người gửi đầu hàng, đối thủ thắng
    END = 5,  // Kết thúc ván, Message = tên người thắng
    HET_GIO = 6,  // Hết giờ, người gửi thua
    THOAT_PHONG = 7,  // Thoát khỏi phòng đang chơi

    // ── Đăng nhập ───────────────────────────────
    LOGIN = 8,  // Client gửi tài khoản|mật khẩu
    LOGIN_OK = 9,  // Server xác nhận đăng nhập, Message = displayName
    LOGIN_FAIL = 10, // Server từ chối đăng nhập

    // ── Lobby ───────────────────────────────────
    GET_ROOMS = 11, // Client xin danh sách phòng
    ROOMS_LIST = 12, // Server trả toàn bộ danh sách phòng (JSON array)
    CREATE_ROOM = 13, // Client tạo phòng mới, Message = tên phòng
    JOIN_ROOM = 14, // Client vào phòng, Message = roomId
    LEAVE_ROOM = 15, // Client rời phòng (về lobby)
    ROOM_UPDATE = 16, // Server broadcast cập nhật 1 phòng (JSON)
    JOIN_OK = 17, // Vào phòng thành công, Message = "tênPhòng|index|tênMình|tênĐốiThủ"
    JOIN_FAIL = 18, // Vào phòng thất bại, Message = lý do
    ROOM_DELETED = 19, // Server thông báo phòng đã bị xóa, Message = roomId

    // ── Đăng ký ─────────────────────────────────
    REGISTER = 20, // Client gửi username|password|displayName
    REGISTER_OK = 21, // Server xác nhận đăng ký thành công
    REGISTER_FAIL = 22, // Server báo đăng ký thất bại, Message = lý do

    // ── Phòng chờ ───────────────────────────────
    OPPONENT_JOINED = 23, // [Chưa dùng] Đối thủ vào phòng (thay bằng JOIN_OK)
    READY,                // Client báo sẵn sàng
    START_GAME,           // Server báo bắt đầu, Message = index người đi trước
}