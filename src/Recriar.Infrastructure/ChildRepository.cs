using MySqlConnector;
using Recriar.Domain.Entities;
using Recriar.Infrastructure.Persistence.Context;

namespace Recriar.Infrastructure.Persistence.Repositories;

public class ChildRepository
{
    private readonly AppDbContext _context;

    public ChildRepository(AppDbContext context)
    {
        _context = context;
    }

    public Child? GetById(int id)
    {
        Child? child = null;
        using (MySqlConnection connection = _context.CreateReadConnection())
        {
            connection.Open();
            string sql = "SELECT * FROM Children WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        child = ReadChild(reader);
                    }
                }
            }
            if (child != null)
            {
                LoadRelatedData(child, connection);
            }
        }
        return child;
    }

    public List<Child> GetAll()
    {
        return ReadList(null);
    }

    public List<Child> SearchByName(string name)
    {
        return ReadList(name);
    }

    private List<Child> ReadList(string? name)
    {
        List<Child> children = new List<Child>();
        using (MySqlConnection connection = _context.CreateReadConnection())
        {
            connection.Open();
            string sql = "SELECT Id, Name, Cpf, Phone FROM Children";
            if (name != null)
            {
                sql = sql + " WHERE Name LIKE @name";
            }
            sql = sql + " ORDER BY Id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                if (name != null)
                {
                    command.Parameters.AddWithValue("@name", "%" + name + "%");
                }
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Child child = new Child();
                        child.setId(reader.GetInt32("Id"));
                        child.setName(reader.GetString("Name"));
                        child.setCpf(reader.GetString("Cpf"));
                        child.setPhone(reader.GetString("Phone"));
                        children.Add(child);
                    }
                }
            }
        }
        return children;
    }

    public void Add(Child child)
    {
        CheckRelatedData(child);
        using (MySqlConnection connection = _context.CreateWriteConnection())
        {
            connection.Open();
            string sql = "INSERT INTO Children (Name, Age, Cpf, Rg, Address, Neighborhood, City, School, MotherName, FatherName, Phone, RegistrationDate, HealthCenter, FileNumber, Allergy) VALUES (@Name, @Age, @Cpf, @Rg, @Address, @Neighborhood, @City, @School, @MotherName, @FatherName, @Phone, @RegistrationDate, @HealthCenter, @FileNumber, @Allergy)";
            int id;
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                AddChildParameters(command, child);
                command.ExecuteNonQuery();
                id = GetNewId(connection);
            }
            int guardianId = SaveGuardian(child.getGuardian()!, id, connection);
            int contactId = SaveEmergencyContact(child.getEmergencyContact()!, id, connection);
            List<int> familyIds = SaveFamilyMembers(child, id, connection, false);
            ApplyIds(child, id, guardianId, contactId, familyIds);
        }
    }

    public void Update(Child child)
    {
        CheckRelatedData(child);
        using (MySqlConnection connection = _context.CreateWriteConnection())
        {
            connection.Open();
            using (MySqlCommand find = new MySqlCommand("SELECT Id FROM Children WHERE Id = @id", connection))
            {
                find.Parameters.AddWithValue("@id", child.getId());
                if (find.ExecuteScalar() == null)
                {
                    throw new InvalidOperationException("Cadastro não encontrado para edição.");
                }
            }
            string sql = "UPDATE Children SET Name = @Name, Age = @Age, Cpf = @Cpf, Rg = @Rg, Address = @Address, Neighborhood = @Neighborhood, City = @City, School = @School, MotherName = @MotherName, FatherName = @FatherName, Phone = @Phone, RegistrationDate = @RegistrationDate, HealthCenter = @HealthCenter, FileNumber = @FileNumber, Allergy = @Allergy WHERE Id = @id";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                AddChildParameters(command, child);
                command.Parameters.AddWithValue("@id", child.getId());
                command.ExecuteNonQuery();
            }
            int guardianId = SaveGuardian(child.getGuardian()!, child.getId(), connection);
            int contactId = SaveEmergencyContact(child.getEmergencyContact()!, child.getId(), connection);
            List<int> familyIds = SaveFamilyMembers(child, child.getId(), connection, true);
            ApplyIds(child, child.getId(), guardianId, contactId, familyIds);
        }
    }

    public bool Delete(int id)
    {
        using (MySqlConnection connection = _context.CreateWriteConnection())
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand("DELETE FROM Children WHERE Id = @id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                int rows = command.ExecuteNonQuery();
                return rows > 0;
            }
        }
    }

    private static Child ReadChild(MySqlDataReader reader)
    {
        Child child = new Child();
        child.setId(reader.GetInt32("Id"));
        child.setName(reader.GetString("Name"));
        child.setAge(reader.GetInt32("Age"));
        child.setCpf(reader.GetString("Cpf"));
        child.setRg(reader.GetString("Rg"));
        child.setAddress(reader.GetString("Address"));
        child.setNeighborhood(reader.GetString("Neighborhood"));
        child.setCity(reader.GetString("City"));
        child.setSchool(reader.GetString("School"));
        child.setMotherName(reader.GetString("MotherName"));
        child.setFatherName(reader.GetString("FatherName"));
        child.setPhone(reader.GetString("Phone"));
        child.setRegistrationDate(reader.GetDateTime("RegistrationDate"));
        child.setHealthCenter(reader.GetString("HealthCenter"));
        child.setFileNumber(reader.GetString("FileNumber"));
        child.setAllergy(reader.GetString("Allergy"));
        return child;
    }

    private static void AddChildParameters(MySqlCommand command, Child child)
    {
        command.Parameters.AddWithValue("@Name", child.getName());
        command.Parameters.AddWithValue("@Age", child.getAge());
        command.Parameters.AddWithValue("@Cpf", child.getCpf());
        command.Parameters.AddWithValue("@Rg", child.getRg());
        command.Parameters.AddWithValue("@Address", child.getAddress());
        command.Parameters.AddWithValue("@Neighborhood", child.getNeighborhood());
        command.Parameters.AddWithValue("@City", child.getCity());
        command.Parameters.AddWithValue("@School", child.getSchool());
        command.Parameters.AddWithValue("@MotherName", child.getMotherName());
        command.Parameters.AddWithValue("@FatherName", child.getFatherName());
        command.Parameters.AddWithValue("@Phone", child.getPhone());
        command.Parameters.AddWithValue("@RegistrationDate", child.getRegistrationDate().Date);
        command.Parameters.AddWithValue("@HealthCenter", child.getHealthCenter());
        command.Parameters.AddWithValue("@FileNumber", child.getFileNumber());
        command.Parameters.AddWithValue("@Allergy", child.getAllergy());
    }

    private static void LoadRelatedData(Child child, MySqlConnection connection)
    {
        using (MySqlCommand command = new MySqlCommand("SELECT * FROM Guardians WHERE ChildId = @id", connection))
        {
            command.Parameters.AddWithValue("@id", child.getId());
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    Guardian guardian = new Guardian();
                    guardian.setId(reader.GetInt32("Id"));
                    guardian.setChildId(child.getId());
                    guardian.setChild(child);
                    guardian.setName(reader.GetString("Name"));
                    guardian.setCpf(reader.GetString("Cpf"));
                    guardian.setRg(reader.GetString("Rg"));
                    child.setGuardian(guardian);
                }
            }
        }
        using (MySqlCommand command = new MySqlCommand("SELECT * FROM EmergencyContacts WHERE ChildId = @id", connection))
        {
            command.Parameters.AddWithValue("@id", child.getId());
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    EmergencyContact contact = new EmergencyContact();
                    contact.setId(reader.GetInt32("Id"));
                    contact.setChildId(child.getId());
                    contact.setChild(child);
                    contact.setName(reader.GetString("Name"));
                    contact.setPhone(reader.GetString("Phone"));
                    child.setEmergencyContact(contact);
                }
            }
        }
        string sql = "SELECT f.*, r.Name AS RelationshipName FROM FamilyMembers f " +
            "INNER JOIN RelationshipTypes r ON r.Id = f.RelationshipTypeId WHERE f.ChildId = @id ORDER BY f.Id";
        using (MySqlCommand command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@id", child.getId());
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    FamilyMember member = new FamilyMember();
                    member.setId(reader.GetInt32("Id"));
                    member.setChildId(child.getId());
                    member.setChild(child);
                    member.setName(reader.GetString("Name"));
                    member.setAge(reader.GetInt32("Age"));
                    member.setRelationshipTypeId(reader.GetInt32("RelationshipTypeId"));
                    if (!reader.IsDBNull(reader.GetOrdinal("OtherRelationship")))
                    {
                        member.setOtherRelationship(reader.GetString("OtherRelationship"));
                    }
                    RelationshipType type = new RelationshipType();
                    type.setId(member.getRelationshipTypeId());
                    type.setName(reader.GetString("RelationshipName"));
                    member.setRelationshipType(type);
                    child.getFamilyMembers().Add(member);
                }
            }
        }
    }

    private static int GetNewId(MySqlConnection connection)
    {
        using (MySqlCommand command = new MySqlCommand("SELECT LAST_INSERT_ID()", connection))
        {
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }

    private static int FindRelatedId(string sql, int childId, MySqlConnection connection)
    {
        using (MySqlCommand command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@childId", childId);
            object? result = command.ExecuteScalar();
            if (result == null)
            {
                return 0;
            }
            return Convert.ToInt32(result);
        }
    }


    private static int SaveGuardian(Guardian person, int childId, MySqlConnection connection)
    {
        int id = FindRelatedId("SELECT Id FROM Guardians WHERE ChildId = @childId", childId, connection);
        string sql;
        if (id == 0)
        {
            sql = "INSERT INTO Guardians (Name, Cpf, Rg, ChildId) VALUES (@Name, @Cpf, @Rg, @childId)";
        }
        else
        {
            sql = "UPDATE Guardians SET Name = @Name, Cpf = @Cpf, Rg = @Rg WHERE ChildId = @childId";
        }
        using (MySqlCommand command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@childId", childId);
            command.Parameters.AddWithValue("@Name", person.getName());
            command.Parameters.AddWithValue("@Cpf", person.getCpf());
            command.Parameters.AddWithValue("@Rg", person.getRg());
            command.ExecuteNonQuery();
            if (id == 0)
            {
                id = GetNewId(connection);
            }
        }
        return id;
    }


    private static int SaveEmergencyContact(EmergencyContact person, int childId, MySqlConnection connection)
    {
        int id = FindRelatedId("SELECT Id FROM EmergencyContacts WHERE ChildId = @childId", childId, connection);
        string sql;
        if (id == 0)
        {
            sql = "INSERT INTO EmergencyContacts (Name, Phone, ChildId) VALUES (@Name, @Phone, @childId)";
        }
        else
        {
            sql = "UPDATE EmergencyContacts SET Name = @Name, Phone = @Phone WHERE ChildId = @childId";
        }
        using (MySqlCommand command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@childId", childId);
            command.Parameters.AddWithValue("@Name", person.getName());
            command.Parameters.AddWithValue("@Phone", person.getPhone());
            command.ExecuteNonQuery();
            if (id == 0)
            {
                id = GetNewId(connection);
            }
        }
        return id;
    }


    private static List<int> SaveFamilyMembers(Child child, int childId, MySqlConnection connection, bool isUpdate)
    {
        List<int> existingIds = new List<int>();
        if (isUpdate)
        {
            using (MySqlCommand command = new MySqlCommand("SELECT Id FROM FamilyMembers WHERE ChildId = @childId", connection))
            {
                command.Parameters.AddWithValue("@childId", childId);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existingIds.Add(reader.GetInt32("Id"));
                    }
                }
            }
        }
        List<int> savedIds = new List<int>();
        foreach (FamilyMember member in child.getFamilyMembers())
        {
            int id = member.getId();
            string sql;
            if (isUpdate && id > 0)
            {
                if (!existingIds.Contains(id) || savedIds.Contains(id))
                {
                    throw new InvalidOperationException("Confira os familiares enviados para edição.");
                }
                sql = "UPDATE FamilyMembers SET RelationshipTypeId = @typeId, OtherRelationship = @other, Name = @name, Age = @age WHERE Id = @id AND ChildId = @childId";
            }
            else
            {
                sql = "INSERT INTO FamilyMembers (RelationshipTypeId, OtherRelationship, Name, Age, ChildId) VALUES (@typeId, @other, @name, @age, @childId)";
                id = 0;
            }
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@typeId", member.getRelationshipTypeId());
                if (member.getOtherRelationship() == null)
                {
                    command.Parameters.AddWithValue("@other", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@other", member.getOtherRelationship());
                }
                command.Parameters.AddWithValue("@name", member.getName());
                command.Parameters.AddWithValue("@age", member.getAge());
                command.Parameters.AddWithValue("@childId", childId);
                if (id > 0)
                {
                    command.Parameters.AddWithValue("@id", id);
                }
                command.ExecuteNonQuery();
                if (id == 0)
                {
                    id = GetNewId(connection);
                }
                savedIds.Add(id);
            }
        }
        foreach (int id in existingIds)
        {
            if (!savedIds.Contains(id))
            {
                using (MySqlCommand command = new MySqlCommand("DELETE FROM FamilyMembers WHERE Id = @id AND ChildId = @childId", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@childId", childId);
                    command.ExecuteNonQuery();
                }
            }
        }
        return savedIds;
    }

    private static void ApplyIds(Child child, int id, int guardianId, int contactId, List<int> familyIds)
    {
        child.setId(id);
        Guardian? guardian = child.getGuardian();
        EmergencyContact? contact = child.getEmergencyContact();
        if (guardian != null)
        {
            guardian.setId(guardianId);
            guardian.setChildId(id);
            guardian.setChild(child);
        }
        if (contact != null)
        {
            contact.setId(contactId);
            contact.setChildId(id);
            contact.setChild(child);
        }
        for (int i = 0; i < child.getFamilyMembers().Count; i++)
        {
            FamilyMember member = child.getFamilyMembers()[i];
            member.setId(familyIds[i]);
            member.setChildId(id);
            member.setChild(child);
        }
    }

    private static void CheckRelatedData(Child child)
    {
        if (child.getGuardian() == null || child.getEmergencyContact() == null)
        {
            throw new InvalidOperationException("Informe o responsável e o contato de emergência.");
        }
    }
}
