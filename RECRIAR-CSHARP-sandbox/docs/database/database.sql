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


USE recriar_prod;

INSERT INTO RelationshipTypes (Id, Name)
VALUES (1, 'Mãe'),
(2, 'Pai'),
(3, 'Avó'),
(4, 'Avô'),
(5, 'Irmã'),
(6, 'Irmão'),
(7, 'Tia'),
(8, 'Tio'),
(9, 'Outro');
INSERT INTO Children (Name, Age, Cpf, Rg, Address, Neighborhood, City, School,
 MotherName, FatherName, Phone, RegistrationDate, HealthCenter, FileNumber, Allergy)
VALUES ('Crianca Exemplo', 8, '00000000000', '00123', '', 'Centro', 'Campinas', '',
 'Mae Exemplo', 'Pai Exemplo', '0012345678', '2026-10-31', '', '', '');
SET @child_id = LAST_INSERT_ID();
INSERT INTO Guardians (Name, Rg, Cpf, ChildId)
VALUES ('Responsavel Exemplo', '00123', '00000000000', @child_id);
INSERT INTO EmergencyContacts (Name, Phone, ChildId)
VALUES ('Contato Exemplo', '0012345678', @child_id);
INSERT INTO FamilyMembers (RelationshipTypeId, OtherRelationship, Name, Age, ChildId)
VALUES (9, 'Primo', 'Familiar Exemplo', 20, @child_id);
INSERT INTO ActivityLogs (Action, Description, CreatedAt)
VALUES ('Cadastro criado', 'Cadastro ficticio para demonstracao', '2026-11-25 10:00:00');
INSERT INTO Programs (Name, Description) VALUES ('Programa Exemplo', 'Programa ficticio para demonstracao');
SET @program_id = LAST_INSERT_ID();
INSERT INTO Activities (ProgramId, Name) VALUES (@program_id, 'Oficina Exemplo');
SET @activity_id = LAST_INSERT_ID();
INSERT INTO ActivitySessions (ActivityId, StartsAt, EndsAt, Location)
VALUES (@activity_id, '2026-11-01 09:00:00', '2026-11-01 10:00:00', 'Sala Exemplo');
SET @session_id = LAST_INSERT_ID();
INSERT INTO SessionEnrollments (ChildId, SessionId, RegisteredAt)
VALUES (@child_id, @session_id, '2026-11-25 10:00:00');
SET @enrollment_id = LAST_INSERT_ID();
INSERT INTO AttendanceRecords (SessionEnrollmentId, Attended, RecordedAt)
VALUES (@enrollment_id, TRUE, '2026-11-25 10:00:00');

USE recriar_prod;

SELECT Id, Name, Cpf, Phone FROM Children ORDER BY Id;

SET @name = '%exemplo%';
SELECT Id, Name, Cpf, Phone FROM Children WHERE Name LIKE @name ORDER BY Id;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT * FROM Children WHERE Id = @id;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT * FROM Guardians WHERE ChildId = @id;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT * FROM EmergencyContacts WHERE ChildId = @id;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT f.*, r.Name AS RelationshipName FROM FamilyMembers f
INNER JOIN RelationshipTypes r ON r.Id = f.RelationshipTypeId
WHERE f.ChildId = @id ORDER BY f.Id;

SELECT Id, Name FROM RelationshipTypes ORDER BY Id;
