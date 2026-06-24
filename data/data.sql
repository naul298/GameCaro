CREATE TABLE Users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    display_name NVARCHAR(50) NOT NULL
);

CREATE TABLE Lobby (
    id INT IDENTITY(1,1) PRIMARY KEY,
    room_name NVARCHAR(50) NOT NULL,
    host_id INT NOT NULL,
    status VARCHAR(20) DEFAULT 'Waiting'
);