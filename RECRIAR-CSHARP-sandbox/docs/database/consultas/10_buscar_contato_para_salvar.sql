USE recriar_prod;
SET @id = (SELECT MIN(Id) FROM Children);
SET @childId = @id;
SELECT Id FROM EmergencyContacts WHERE ChildId = @childId;
