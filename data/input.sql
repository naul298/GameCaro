INSERT INTO Users (account, password, name)
VALUES
('admin', '123456', N'Quản trị viên'),
('hoa123', '123456', N'Hoà Lê'),
('luan123', '123456', N'Hoàng Luân');
GO

INSERT INTO Lobby (name, host_id, player_count, status, is_default)
VALUES
(N'Lobby 1', 1, 0, N'Waiting', 1),
(N'Lobby 2', 1, 0, N'Waiting', 1),
(N'Lobby 3', 1, 0, N'Waiting', 1),
(N'Lobby 4', 1, 0, N'Waiting', 1),
(N'Lobby 5', 1, 0, N'Waiting', 1),
(N'Lobby 6', 1, 0, N'Waiting', 1),
(N'Lobby 7', 1, 0, N'Waiting', 1),
(N'Lobby 8', 1, 0, N'Waiting', 1),
(N'Lobby 9', 1, 0, N'Waiting', 1),
(N'Lobby 10', 1, 0, N'Waiting', 1);
GO