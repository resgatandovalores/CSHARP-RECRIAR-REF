USE recriar_prod;
SET @id = (SELECT MIN(Id) FROM Children);
SET @childId = @id;
SELECT Id FROM Children WHERE Id = @id;
