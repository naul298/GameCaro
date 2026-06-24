namespace ServerGame.Models;

// Danh sách lệnh giao tiếp giữa client và server
public enum SocketCommand
{
    SEND_POINT = 0,  // Gửi nước đi
    THONG_BAO = 1,  // Thông báo chung
    CHOI_LAI = 2,  // Yêu cầu chơi lại
    CAU_HOA = 3,  // Yêu cầu cầu hoà
    DAU_HANG = 4,  // Đầu hàng
    END = 5,  // Kết thúc game (có người thắng)
    HET_GIO = 6,  // Hết giờ
    THOAT_PHONG = 7,  // Thoát phòng
    LOGIN = 8,  // Gửi thông tin đăng nhập
    LOGIN_OK = 9,  // Đăng nhập thành công
    LOGIN_FAIL = 10, // Đăng nhập thất bại
}