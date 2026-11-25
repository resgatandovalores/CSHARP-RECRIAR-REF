USE recriar_prod;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT f.*, r.Name AS RelationshipName FROM FamilyMembers f
INNER JOIN RelationshipTypes r ON r.Id = f.RelationshipTypeId
WHERE f.ChildId = @id ORDER BY f.Id;
