USE recriar_prod;
SET @id = (SELECT MIN(Id) FROM Children);
SET @childId = @id;
SELECT LAST_INSERT_ID();
