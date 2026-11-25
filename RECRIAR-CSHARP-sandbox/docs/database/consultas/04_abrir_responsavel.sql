USE recriar_prod;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT * FROM Guardians WHERE ChildId = @id;
