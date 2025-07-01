CREATE TABLE Registration (
    ID             VARCHAR(9)     NOT NULL PRIMARY KEY,
    FirstName      NVARCHAR(50)   NOT NULL,
    LastName       NVARCHAR(50)   NOT NULL,
    PhoneNumber    VARCHAR(15),
    LandlineNumber VARCHAR(15),           -- טלפון ביתי (שונה מ-HomeNumber)
    Email          VARCHAR(100),
    DateOfBirth    DATE,
    PersonalStatus NVARCHAR(20),
    Street         NVARCHAR(100),
    City           NVARCHAR(50),
    HouseNumber    VARCHAR(10),           -- מספר בית (שם שונה מ-HomeNumber)
    Password       VARCHAR(100)
);
