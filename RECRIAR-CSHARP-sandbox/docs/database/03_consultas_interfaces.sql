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
