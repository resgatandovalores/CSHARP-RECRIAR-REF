using MySqlConnector;
using Recriar.Application.Dtos;
using Recriar.Application.Services;
using Recriar.Infrastructure.Export;
using Recriar.Infrastructure.Logging;
using Recriar.Infrastructure.Persistence.Context;
using Recriar.Infrastructure.Persistence.Repositories;

class BasicTests
{
    private static int checks;
    private static string connectionString = "";

    static void Main(string[] args)
    {
        if (args.Length != 1) throw new ArgumentException("Informe um arquivo de conexao de teste.");
        connectionString = File.ReadAllText(args[0]).Trim();
        MySqlConnectionStringBuilder settings = new MySqlConnectionStringBuilder(connectionString);
        if (settings.Database != "recriar_ref_teste" || settings.Port != 33179)
            throw new ArgumentException("Use somente recriar_ref_teste na porta 33179.");
        AppDbContext context = new AppDbContext(connectionString);
        ChildRepository repository = new ChildRepository(context);
        RelationshipTypeRepository types = new RelationshipTypeRepository(context);
        string folder = Path.Combine(Path.GetTempPath(), "recriar-ref-teste");
        ChildService service = new ChildService(repository, types, new DbActivityLogger(context),
            new FileErrorLogger(folder), new HtmlChildFileExporter(folder));
        Check(Number("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema=DATABASE() AND table_type='BASE TABLE'") == 11, "11 tabelas");
        Check(Number("SELECT COUNT(*) FROM information_schema.views WHERE table_schema=DATABASE()") == 0, "sem views");
        Check(types.GetAll().Count == 9 && types.GetAll()[8].getName() == "Outro", "9 parentescos e Outro=9");
        int before = service.GetAll().Count;
        Check(before > 0, "dados iniciais");
        Check(service.SearchByName("exemplo").Count > 0, "pesquisa por parte do nome");
        Check(service.SearchByName("").Count == before, "pesquisa vazia");
        int id = service.Register(Example());
        Check(id > 0 && service.GetAll().Count == before + 1, "cadastro pelo servico");
        ChildDto saved = service.GetById(id);
        Check(saved.getGuardianName() == "Responsavel Teste", "leitura do responsavel");
        Check(saved.getEmergencyContactName() == "Contato Teste", "leitura do contato");
        Check(saved.getFamilyMembers().Count == 2, "leitura dos familiares");
        int familyId = saved.getFamilyMembers()[0].getId();
        saved.setAge("10");
        saved.getFamilyMembers()[0].setName("Familiar Alterado");
        saved.getFamilyMembers().RemoveAt(1);
        saved.getFamilyMembers().Add(Family("Novo Familiar", 9, "Primo"));
        service.Update(id, saved);
        saved = service.GetById(id);
        Check(saved.getAge() == "10", "edicao da crianca");
        Check(saved.getFamilyMembers().Count == 2, "remover e incluir familiar");
        Check(saved.getFamilyMembers()[0].getId() == familyId, "preservar ID do familiar");
        Check(saved.getFamilyMembers()[0].getName() == "Familiar Alterado", "editar familiar");
        Check(saved.getFamilyMembers()[1].getOtherRelationship() == "Primo", "parentesco Outro");
        string path = service.ExportFile(id);
        Check(File.Exists(path) && File.ReadAllText(path).Contains("Familiar Alterado"), "exportacao HTML");
        Check(Number("SELECT COUNT(*) FROM ActivityLogs") >= 4, "log de atividades");
        ChildDto invalid = Example(); invalid.setAge("121"); Reject(service, invalid, "idade invalida");
        invalid = Example(); invalid.setRegistrationDate("31/02/2026"); Reject(service, invalid, "data invalida");
        invalid = Example(); invalid.setGuardianName(""); Reject(service, invalid, "responsavel obrigatorio");
        invalid = Example(); invalid.setName("Teste123"); Reject(service, invalid, "nome com numeros");
        invalid = Example(); invalid.getFamilyMembers().Clear();
        for (int i = 0; i < 11; i++) invalid.getFamilyMembers().Add(Family("Familiar Teste", 1, ""));
        Reject(service, invalid, "limite de dez familiares");
        SqlError("INSERT INTO Guardians(Name,Rg,Cpf,ChildId) VALUES('Duplicado','',''," + id + ")", 1062, "UNIQUE do responsavel");
        SqlError("INSERT INTO Guardians(Name,Rg,Cpf,ChildId) VALUES('Inexistente','','',2147483647)", 1452, "FK da crianca");
        SqlError("DELETE FROM RelationshipTypes WHERE Id=9", 1451, "RESTRICT do parentesco");
        SqlError("INSERT INTO SessionEnrollments(ChildId,SessionId,RegisteredAt) SELECT ChildId,SessionId,RegisteredAt FROM SessionEnrollments LIMIT 1", 1062, "inscricao duplicada");
        service.Delete(id);
        Check(repository.GetById(id) == null, "exclusao da crianca");
        Check(Number("SELECT COUNT(*) FROM Guardians WHERE ChildId=" + id) == 0, "CASCADE do responsavel");
        Check(Number("SELECT COUNT(*) FROM EmergencyContacts WHERE ChildId=" + id) == 0, "CASCADE do contato");
        Check(Number("SELECT COUNT(*) FROM FamilyMembers WHERE ChildId=" + id) == 0, "CASCADE dos familiares");
        Check(!repository.Delete(id), "excluir ID inexistente");
        Check(service.GetAll().Count == before, "dados iniciais preservados");
        Console.WriteLine("PASSOU: " + checks + " verificacoes de integracao.");
        Console.WriteLine("Sem transacoes: varias gravacoes nao sao atomicas.");
    }

    static ChildDto Example()
    {
        ChildDto c = new ChildDto();
        c.setName("Crianca Teste"); c.setAge("9"); c.setCpf("00000000000"); c.setRg("00123");
        c.setNeighborhood("Centro"); c.setCity("Campinas"); c.setMotherName("Mae Teste");
        c.setFatherName("Pai Teste"); c.setPhone("0012345678"); c.setRegistrationDate("25/11/2026");
        c.setGuardianName("Responsavel Teste"); c.setGuardianCpf("00000000000"); c.setGuardianRg("00123");
        c.setEmergencyContactName("Contato Teste"); c.setEmergencyContactPhone("0012345678");
        c.getFamilyMembers().Add(Family("Familiar Teste", 1, ""));
        c.getFamilyMembers().Add(Family("Outro Familiar", 2, ""));
        return c;
    }
    static FamilyMemberDto Family(string name, int type, string other)
    {
        FamilyMemberDto m = new FamilyMemberDto();
        m.setName(name); m.setAge("20"); m.setRelationshipTypeId(type); m.setOtherRelationship(other);
        return m;
    }
    static int Number(string sql)
    {
        using (MySqlConnection c = new MySqlConnection(connectionString))
        {
            c.Open();
            using (MySqlCommand cmd = new MySqlCommand(sql, c)) return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
    static void Reject(ChildService service, ChildDto dto, string label)
    {
        int before = service.GetAll().Count; bool rejected = false;
        try { service.Register(dto); } catch (ArgumentException) { rejected = true; }
        Check(rejected && service.GetAll().Count == before, label);
    }
    static void SqlError(string sql, int number, string label)
    {
        bool rejected = false;
        try { Number(sql); } catch (MySqlException ex) { rejected = ex.Number == number; }
        Check(rejected, label);
    }
    static void Check(bool condition, string label)
    {
        if (!condition) throw new Exception("FALHOU: " + label);
        checks++; Console.WriteLine("OK: " + label);
    }
}
