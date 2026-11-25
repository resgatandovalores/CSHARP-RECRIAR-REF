USE recriar_prod;

SET @name = '%exemplo%';
SELECT Id, Name, Cpf, Phone FROM Children WHERE Name LIKE @name ORDER BY Id;
