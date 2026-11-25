namespace Recriar.Infrastructure.Persistence.Context;

public class AppDbContextFactory
{
    public AppDbContext Create()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "conexao.txt");
        if (!File.Exists(path))
        {
            throw new InvalidOperationException("Crie o arquivo conexao.txt ao lado do programa, seguindo conexao.exemplo.txt.");
        }
        string connectionString = File.ReadAllText(path).Trim();
        return new AppDbContext(connectionString);
    }
}
