CREATE TABLE Messages (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ClientID VARCHAR(9) NOT NULL,  -- קישור ל-Registration(ID)
    MessageType NVARCHAR(20) NOT NULL CHECK (MessageType IN (N'מערכת', N'גביה')),
    MessageText NVARCHAR(MAX) NOT NULL,
    DateSent DATETIME NOT NULL DEFAULT GETDATE(),
    IsRead BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (ClientID) REFERENCES Registration(ID)
);
