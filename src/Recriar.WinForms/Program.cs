using Recriar.Application.Exceptions;
using Recriar.Application.Services;
using Recriar.Infrastructure.Logging;
using Recriar.Infrastructure.Export;
using Recriar.Infrastructure.Persistence.Context;
using Recriar.Infrastructure.Persistence.Repositories;
using Recriar.Domain.Entities;
using Recriar.WinForms.Forms;

namespace Recriar.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        FileErrorLogger errorLogger = new FileErrorLogger();
        System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        System.Windows.Forms.Application.ThreadException += (_, e) => ShowUnexpectedError(e.Exception, "Tela", errorLogger);

        try
        {
            ChildService service = CreateService(errorLogger);
            using var mainForm = new MainForm(service, errorLogger);
            System.Windows.Forms.Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            ShowUnexpectedError(ex, "Abertura do programa", errorLogger);
        }
    }

    private static ChildService CreateService(FileErrorLogger errorLogger)
    {
        AppDbContext context = new AppDbContextFactory().Create();

        // Check the write connection before opening the main window.
        using (var connection = context.CreateWriteConnection())
        {
            connection.Open();
        }

        ChildRepository children = new ChildRepository(context);
        RelationshipTypeRepository relationshipTypes = new RelationshipTypeRepository(context);
        DbActivityLogger activityLogger = new DbActivityLogger(context);

        bool hasOther = false;
        foreach (RelationshipType type in relationshipTypes.GetAll())
        {
            if (type.getId() == new RelationshipType().getOtherId() && type.getName() == "Outro")
            {
                hasOther = true;
            }
        }
        if (!hasOther)
        {
            throw new InvalidOperationException("Confira a carga inicial de parentescos: Outro deve ter Id 9.");
        }

        return CreateChildService(children, relationshipTypes, activityLogger, errorLogger);
    }

    private static ChildService CreateChildService(
        ChildRepository children,
        RelationshipTypeRepository relationshipTypes,
        DbActivityLogger activityLogger,
        FileErrorLogger errorLogger)
    {
        return new ChildService(children, relationshipTypes, activityLogger, errorLogger,
            new HtmlChildFileExporter());
    }

    private static void ShowUnexpectedError(Exception exception, string context, FileErrorLogger errorLogger)
    {
        errorLogger.LogError(exception, context);
        string message = new ErrorHandler().GetMessage(exception);
        if (context == "Abertura do programa")
        {
            message = "Não foi possível abrir o banco.\n\nConfira se o MySQL está ligado e se o arquivo conexao.txt está ao lado do programa.\n\n" + exception.Message;
        }
        MessageBox.Show(message, "Atenção",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
