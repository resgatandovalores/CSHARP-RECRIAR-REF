
namespace Recriar.Infrastructure.Logging;
public class FileErrorLogger
{

    private readonly string _folder;
    public FileErrorLogger(string? folder = null)
    {
        _folder = folder ?? Path.Combine(AppContext.BaseDirectory, "logs");
    }
    public void LogError(Exception exception, string context)
    {
        try
        {
            Directory.CreateDirectory(_folder);

            DateTime now = DateTime.Now;
            string path = Path.Combine(_folder, $"errors-{now:yyyy-MM-dd}.log");
            string text =
                $"[{now:yyyy-MM-dd HH:mm:ss}] {context}{Environment.NewLine}" +
                $"{exception}{Environment.NewLine}" +
                $"{new string('-', 60)}{Environment.NewLine}";

            File.AppendAllText(path, text);
        }
        catch
        {
        }
    }
}
