CREATE TABLE Messages (
    Id INT PRIMARY KEY IDENTITY,
    Zeout VARCHAR(9) NOT NULL,           -- ת"ז של המשתמש כמחרוזת
    InstitutionId INT NOT NULL,          -- קוד מוסד
    Seder INT NOT NULL,                  -- סדר להצגה
    Perut NVARCHAR(MAX) NOT NULL,        -- תוכן ההודעה
    Important INT NOT NULL,              -- סוג הודעה (0=רגילה, 1=מודגשת)
    CreatedAt DATETIME NOT NULL,         -- תאריך יצירה

    CONSTRAINT FK_Messages_Registration
        FOREIGN KEY (Zeout, InstitutionId)
        REFERENCES Registration(ID, InstitutionId)
);
