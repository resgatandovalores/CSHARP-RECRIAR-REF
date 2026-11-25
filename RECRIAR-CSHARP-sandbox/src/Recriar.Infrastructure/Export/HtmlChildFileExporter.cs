using System.Text;
using Recriar.Domain.Entities;

namespace Recriar.Infrastructure.Export;
public class HtmlChildFileExporter
{

    private readonly string _folder;
    public HtmlChildFileExporter(string? folder = null)
    {
        _folder = folder ?? Path.Combine(AppContext.BaseDirectory, "fichas");
    }
    public string Export(Child child, List<RelationshipType> relationshipNames)
    {
        Directory.CreateDirectory(_folder);
        string path = Path.Combine(_folder, $"ficha_{child.getId()}.html");
        File.WriteAllText(path, BuildHtml(child, relationshipNames), Encoding.UTF8);

        return path;
    }
    private static string BuildHtml(Child child, List<RelationshipType> relationshipNames)
    {
        var html = new StringBuilder();

        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html lang=\"pt-BR\">");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset=\"utf-8\">");
        html.AppendLine($"<title>Ficha {child.getId()} - {Encode(child.getName())}</title>");
        html.AppendLine("<style>");
        html.AppendLine("  body { font-family: Arial, sans-serif; margin: 32px; color: #222; }");
        html.AppendLine("  h1 { font-size: 22px; border-bottom: 2px solid #333; padding-bottom: 6px; }");
        html.AppendLine("  h2 { font-size: 16px; background: #eee; padding: 6px 8px; margin-top: 24px; }");
        html.AppendLine("  table { border-collapse: collapse; width: 100%; }");
        html.AppendLine("  th, td { border: 1px solid #bbb; padding: 6px 8px; text-align: left; vertical-align: top; }");
        html.AppendLine("  th { width: 30%; background: #f7f7f7; }");
        html.AppendLine("  .family th { width: auto; }");
        html.AppendLine("  .footer { margin-top: 32px; font-size: 12px; color: #666; }");
        html.AppendLine("  @media print { body { margin: 0; } h2 { -webkit-print-color-adjust: exact; print-color-adjust: exact; } }");
        html.AppendLine("</style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");

        html.AppendLine("<h1>RECRIAR - Ficha de Cadastro</h1>");
        StartSection(html, "Dados da criança");
        Row(html, "Número da ficha", child.getId().ToString());
        Row(html, "Nome", child.getName());
        Row(html, "Idade", child.getAge().ToString());
        Row(html, "CPF", child.getCpf());
        Row(html, "RG", child.getRg());
        Row(html, "Endereço", child.getAddress());
        Row(html, "Bairro", child.getNeighborhood());
        Row(html, "Cidade", child.getCity());
        Row(html, "Escola", child.getSchool());
        Row(html, "Nome da mãe", child.getMotherName());
        Row(html, "Nome do pai", child.getFatherName());
        Row(html, "Telefone", child.getPhone());
        Row(html, "Data de cadastro", child.getRegistrationDate().ToString("dd/MM/yyyy"));
        EndSection(html);
        StartSection(html, "Responsável");
        Row(html, "Nome", child.getGuardian()?.getName());
        Row(html, "RG", child.getGuardian()?.getRg());
        Row(html, "CPF", child.getGuardian()?.getCpf());
        EndSection(html);
        StartSection(html, "Contato de emergência");
        Row(html, "Nome", child.getEmergencyContact()?.getName());
        Row(html, "Telefone", child.getEmergencyContact()?.getPhone());
        EndSection(html);
        html.AppendLine("<h2>Composição familiar</h2>");
        if (child.getFamilyMembers().Count == 0)
        {
            html.AppendLine("<p>Nenhum familiar cadastrado.</p>");
        }
        else
        {
            html.AppendLine("<table class=\"family\">");
            html.AppendLine("<tr><th>Parentesco</th><th>Nome</th><th>Idade</th></tr>");
            foreach (FamilyMember member in child.getFamilyMembers())
            {
                html.AppendLine(
                    $"<tr><td>{Encode(RelationshipText(member, relationshipNames))}</td>" +
                    $"<td>{Encode(member.getName())}</td>" +
                    $"<td>{member.getAge()}</td></tr>");
            }
            html.AppendLine("</table>");
        }
        StartSection(html, "Outras informações");
        Row(html, "Centro de saúde", child.getHealthCenter());
        Row(html, "Número da ficha (saúde)", child.getFileNumber());
        Row(html, "Alergia", child.getAllergy());
        EndSection(html);

        html.AppendLine($"<p class=\"footer\">Ficha gerada em {DateTime.Now:dd/MM/yyyy HH:mm}.</p>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        return html.ToString();
    }
    private static string RelationshipText(FamilyMember member, List<RelationshipType> relationshipNames)
    {
        if (member.getRelationshipTypeId() == new RelationshipType().getOtherId() && !string.IsNullOrWhiteSpace(member.getOtherRelationship()))
        {
            return member.getOtherRelationship() ?? "";
        }

        foreach (RelationshipType type in relationshipNames)
        {
            if (type.getId() == member.getRelationshipTypeId())
            {
                return type.getName();
            }
        }

        return member.getRelationshipType()?.getName() ?? string.Empty;
    }

    private static void StartSection(StringBuilder html, string title)
    {
        html.AppendLine($"<h2>{Encode(title)}</h2>");
        html.AppendLine("<table>");
    }

    private static void EndSection(StringBuilder html)
    {
        html.AppendLine("</table>");
    }
    private static void Row(StringBuilder html, string label, string? value)
    {
        string shown = string.IsNullOrWhiteSpace(value) ? "-" : value;
        html.AppendLine($"<tr><th>{Encode(label)}</th><td>{Encode(shown)}</td></tr>");
    }
    private static string Encode(string? text)
    {
        return System.Net.WebUtility.HtmlEncode(text ?? string.Empty);
    }
}
