CREATE TABLE Loans (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ClientID VARCHAR(9) NOT NULL,  -- קישור ל־Registration(ID)
    LoanTypeID INT NOT NULL,       -- קישור ל־LoanTypes(ID)
    LoanDate DATE NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    InstallmentsCount INT NOT NULL,
    Purpose NVARCHAR(50) NOT NULL CHECK (Purpose IN (N'שיפוץ', N'קניית רכב', N'לימודים', N'אחר')),
    PurposeDetails NVARCHAR(255),
    FOREIGN KEY (ClientID) REFERENCES Registration(ID),
    FOREIGN KEY (LoanTypeID) REFERENCES LoanTypes(ID)
);