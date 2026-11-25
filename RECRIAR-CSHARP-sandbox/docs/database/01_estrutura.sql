CREATE DATABASE recriar_prod;
USE recriar_prod;

CREATE TABLE ActivityLogs (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Action varchar(100) NOT NULL,
    Description varchar(255) NOT NULL,
    CreatedAt datetime NOT NULL
);

CREATE TABLE Children (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name varchar(100) NOT NULL,
    Age int NOT NULL,
    Cpf varchar(20) NOT NULL,
    Rg varchar(20) NOT NULL,
    Address varchar(100) NOT NULL,
    Neighborhood varchar(50) NOT NULL,
    City varchar(50) NOT NULL,
    School varchar(100) NOT NULL,
    MotherName varchar(100) NOT NULL,
    FatherName varchar(100) NOT NULL,
    Phone varchar(20) NOT NULL,
    RegistrationDate date NOT NULL,
    HealthCenter varchar(100) NOT NULL,
    FileNumber varchar(50) NOT NULL,
    Allergy varchar(100) NOT NULL
);

CREATE TABLE Programs (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name varchar(100) NOT NULL,
    Description varchar(255) NOT NULL
);

CREATE TABLE RelationshipTypes (
    Id int NOT NULL PRIMARY KEY,
    Name varchar(50) NOT NULL
);

CREATE TABLE EmergencyContacts (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name varchar(100) NOT NULL,
    Phone varchar(20) NOT NULL,
    ChildId int NOT NULL,
    FOREIGN KEY (ChildId) REFERENCES Children (Id) ON DELETE CASCADE
);

CREATE TABLE Guardians (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name varchar(100) NOT NULL,
    Rg varchar(20) NOT NULL,
    Cpf varchar(20) NOT NULL,
    ChildId int NOT NULL,
    FOREIGN KEY (ChildId) REFERENCES Children (Id) ON DELETE CASCADE
);

CREATE TABLE Activities (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ProgramId int NOT NULL,
    Name varchar(100) NOT NULL,
    FOREIGN KEY (ProgramId) REFERENCES Programs (Id) ON DELETE RESTRICT
);

CREATE TABLE FamilyMembers (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    RelationshipTypeId int NOT NULL,
    OtherRelationship varchar(50) NULL,
    Name varchar(100) NOT NULL,
    Age int NOT NULL,
    ChildId int NOT NULL,
    FOREIGN KEY (ChildId) REFERENCES Children (Id) ON DELETE CASCADE,
    FOREIGN KEY (RelationshipTypeId) REFERENCES RelationshipTypes (Id) ON DELETE RESTRICT
);

CREATE TABLE ActivitySessions (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ActivityId int NOT NULL,
    StartsAt datetime NOT NULL,
    EndsAt datetime NOT NULL,
    Location varchar(100) NOT NULL,
    FOREIGN KEY (ActivityId) REFERENCES Activities (Id) ON DELETE RESTRICT
);

CREATE TABLE SessionEnrollments (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ChildId int NOT NULL,
    SessionId int NOT NULL,
    RegisteredAt datetime NOT NULL,
    FOREIGN KEY (SessionId) REFERENCES ActivitySessions (Id) ON DELETE RESTRICT,
    FOREIGN KEY (ChildId) REFERENCES Children (Id) ON DELETE CASCADE
);

CREATE TABLE AttendanceRecords (
    Id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
    SessionEnrollmentId int NOT NULL,
    Attended tinyint(1) NOT NULL,
    RecordedAt datetime NOT NULL,
    FOREIGN KEY (SessionEnrollmentId) REFERENCES SessionEnrollments (Id) ON DELETE CASCADE
);



CREATE INDEX IX_Activities_ProgramId ON Activities (ProgramId);

CREATE INDEX IX_ActivityLogs_CreatedAt ON ActivityLogs (CreatedAt);

CREATE INDEX IX_ActivitySessions_ActivityId ON ActivitySessions (ActivityId);

CREATE INDEX IX_ActivitySessions_StartsAt ON ActivitySessions (StartsAt);

CREATE UNIQUE INDEX IX_AttendanceRecords_SessionEnrollmentId ON AttendanceRecords (SessionEnrollmentId);

CREATE INDEX IX_Children_Name ON Children (Name);

CREATE UNIQUE INDEX IX_EmergencyContacts_ChildId ON EmergencyContacts (ChildId);

CREATE INDEX IX_FamilyMembers_ChildId ON FamilyMembers (ChildId);

CREATE INDEX IX_FamilyMembers_RelationshipTypeId ON FamilyMembers (RelationshipTypeId);

CREATE UNIQUE INDEX IX_Guardians_ChildId ON Guardians (ChildId);

CREATE INDEX IX_SessionEnrollments_ChildId ON SessionEnrollments (ChildId);

CREATE UNIQUE INDEX IX_SessionEnrollments_SessionId_ChildId ON SessionEnrollments (SessionId, ChildId);
