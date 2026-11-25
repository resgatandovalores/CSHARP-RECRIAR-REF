USE recriar_prod;

SELECT Id, Name, Cpf, Phone FROM Children ORDER BY Id;

USE recriar_prod;

SET @name = '%exemplo%';
SELECT Id, Name, Cpf, Phone FROM Children WHERE Name LIKE @name ORDER BY Id;

USE recriar_prod;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT * FROM Children WHERE Id = @id;

USE recriar_prod;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT * FROM Guardians WHERE ChildId = @id;

USE recriar_prod;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT * FROM EmergencyContacts WHERE ChildId = @id;

USE recriar_prod;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT f.*, r.Name AS RelationshipName FROM FamilyMembers f
INNER JOIN RelationshipTypes r ON r.Id = f.RelationshipTypeId
WHERE f.ChildId = @id ORDER BY f.Id;

USE recriar_prod;

SELECT Id, Name FROM RelationshipTypes ORDER BY Id;

USE recriar_prod;
SET @id = (SELECT MIN(Id) FROM Children);
SET @childId = @id;
SELECT Id FROM Children WHERE Id = @id;

USE recriar_prod;
SET @id = (SELECT MIN(Id) FROM Children);
SET @childId = @id;
SELECT Id FROM Guardians WHERE ChildId = @childId;

USE recriar_prod;
SET @id = (SELECT MIN(Id) FROM Children);
SET @childId = @id;
SELECT Id FROM EmergencyContacts WHERE ChildId = @childId;

USE recriar_prod;
SET @id = (SELECT MIN(Id) FROM Children);
SET @childId = @id;
SELECT Id FROM FamilyMembers WHERE ChildId = @childId;

USE recriar_prod;
SET @id = (SELECT MIN(Id) FROM Children);
SET @childId = @id;
SELECT LAST_INSERT_ID();
