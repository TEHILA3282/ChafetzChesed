CREATE TABLE Registration (
    ID               VARCHAR(9)     NOT NULL PRIMARY KEY,
    FirstName        NVARCHAR(50)   NOT NULL,
    LastName         NVARCHAR(50)   NOT NULL,
    PhoneNumber      VARCHAR(15),
    LandlineNumber   VARCHAR(15),
    Email            VARCHAR(100),
    DateOfBirth      DATE,
    PersonalStatus   NVARCHAR(20),
    Street           NVARCHAR(100),
    City             NVARCHAR(50),
    HouseNumber      VARCHAR(10),
    Password         VARCHAR(100),
    RegistrationStatus NVARCHAR(10) CHECK (RegistrationStatus IN (N'ממתין', N'מאושר', N'דחוי')),
    StatusUpdatedAt    DATE
);