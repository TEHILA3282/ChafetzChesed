CREATE TABLE Deposits (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ClientID VARCHAR(9) NOT NULL,      -- קישור לטבלת Registration (ת.ז)
    DepositDate DATE NOT NULL,
    DepositTypeID INT NOT NULL,        -- קישור לטבלת DepositTypes
    Amount DECIMAL(9,2) NOT NULL,      -- עד 9,999,999.99
    PurposeDetails NVARCHAR(MAX),      -- טקסט ארוך ללא הגבלה
    IsDirectDeposit BIT NOT NULL,      -- TRUE / FALSE
    DepositReceivedDate DATE,           -- חובה אם IsDirectDeposit = 1
    PaymentMethod NVARCHAR(50) 
        CHECK (
            PaymentMethod IN (
                N'הו"ק קיימת בקופת הגמ"ח',
                N'חתימה על הוראת קבע חדשה',
                N'גביה באשראי המופיע בקופת הגמ"ח',
                N'גביה מאשראי אחר'
            )
        ),
    FOREIGN KEY (ClientID) REFERENCES Registration(ID),
    FOREIGN KEY (DepositTypeID) REFERENCES DepositTypes(ID)
);