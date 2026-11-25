using System;
using System.IO;

namespace Recriar.Infrastructure.Logging
{
    public class FileErrorLogger
    {
        private string folder;

        public FileErrorLogger()
        {
            folder = Path.Combine(AppContext.BaseDirectory, "logs");
        }

        public FileErrorLogger(string folder)
        {
            if (folder == null) this.folder = Path.Combine(AppContext.BaseDirectory, "logs");
            else this.folder = folder;
        }

        public void LogError(Exception exception, string context)
        {
            try
            {
                Directory.CreateDirectory(folder);
                string path = Path.Combine(folder, "errors-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                string text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - " + context + Environment.NewLine;
                text = text + exception.ToString() + Environment.NewLine + Environment.NewLine;
                File.AppendAllText(path, text);
            }
            catch (Exception)
            {
                // Uma falha no arquivo de log nao deve esconder o erro original.
            }
        }
    }
}
