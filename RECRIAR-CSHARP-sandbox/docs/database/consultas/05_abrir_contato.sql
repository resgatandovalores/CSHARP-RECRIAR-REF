USE recriar_prod;

SET @id = (SELECT MIN(Id) FROM Children);
SELECT * FROM EmergencyContacts WHERE ChildId = @id;
