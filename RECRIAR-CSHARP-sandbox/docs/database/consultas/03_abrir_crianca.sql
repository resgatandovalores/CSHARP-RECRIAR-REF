USE recriar_prod;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT * FROM Children WHERE Id = @id;
