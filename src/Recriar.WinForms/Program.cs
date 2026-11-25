using System;
using System.Threading;
using System.Windows.Forms;
using MySqlConnector;
using Recriar.Application.Services;
using Recriar.Infrastructure.Export;
using Recriar.Infrastructure.Logging;
using Recriar.Infrastructure.Persistence.Context;
using Recriar.Infrastructure.Persistence.Repositories;
using Recriar.Domain.Entities;
using Recriar.WinForms.Forms;

namespace Recriar.WinForms
{
    internal class Program
    {
        private static FileErrorLogger errorLogger;

        [STAThread]
        private static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            errorLogger = new FileErrorLogger();
            System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            try
            {
                AppDbContext context = new AppDbContextFactory().Create();
                using (MySqlConnection connection = context.CreateWriteConnection())
                {
                    connection.Open();
                }
                ChildRepository children = new ChildRepository(context);
                RelationshipTypeRepository types = new RelationshipTypeRepository(context);
                bool found = false;
                foreach (RelationshipType type in types.GetAll())
                {
                    if (type.getId() == 9 && type.getName() == "Outro") found = true;
                }
                if (!found) throw new ArgumentException("Confira os parentescos: Outro deve ter Id 9.");
                DbActivityLogger activityLogger = new DbActivityLogger(context);
                HtmlChildFileExporter exporter = new HtmlChildFileExporter();
                ChildService service = new ChildService(children, types, activityLogger, errorLogger, exporter);
                using (MainForm form = new MainForm(service, errorLogger))
                {
                    System.Windows.Forms.Application.Run(form);
                }
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex, "Abertura do programa");
                MessageBox.Show("Não foi possível abrir o banco. Confira o MySQL e o arquivo conexao.txt.\n\n" + ex.Message, "Atenção");
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            errorLogger.LogError(e.Exception, "Tela");
            MessageBox.Show("Ocorreu um erro ao usar a tela. Tente novamente.", "Atenção");
        }
    }
}
