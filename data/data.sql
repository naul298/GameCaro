-- Bảng lưu trữ thông tin tài khoản người chơi
CREATE TABLE Users (
    id          INT IDENTITY(1,1) PRIMARY KEY,
    account     VARCHAR(50) NOT NULL UNIQUE,
    password    VARCHAR(255) NOT NULL,
    name        NVARCHAR(100) NOT NULL
);
GO

-- Bảng quản lý sảnh chờ (phòng game)
CREATE TABLE Lobby (
    id              INT IDENTITY(1,1) PRIMARY KEY,
    name            NVARCHAR(100) NOT NULL,
    host_id         INT NOT NULL,
    player_count    INT NOT NULL DEFAULT 1 CHECK (player_count BETWEEN 0 AND 2),
    status          NVARCHAR(20) NOT NULL DEFAULT N'Waiting',
    is_default      BIT NOT NULL DEFAULT 0,
    
    -- Khóa ngoại liên kết với người tạo phòng
    FOREIGN KEY (host_id) REFERENCES Users(id)
);
GO

