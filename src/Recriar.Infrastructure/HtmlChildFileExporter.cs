using System;
using System.Collections.Generic;
using System.IO;
using Recriar.Domain.Entities;

namespace Recriar.Infrastructure.Export
{
    public class HtmlChildFileExporter
    {
        private string folder;

        public HtmlChildFileExporter()
        {
            folder = Path.Combine(AppContext.BaseDirectory, "fichas");
        }

        public HtmlChildFileExporter(string folder)
        {
            if (folder == null) this.folder = Path.Combine(AppContext.BaseDirectory, "fichas");
            else this.folder = folder;
        }

        public string Export(Child child, List<RelationshipType> relationships)
        {
            Directory.CreateDirectory(folder);
            string path = Path.Combine(folder, "ficha_" + child.getId() + ".html");
            string html = "<!DOCTYPE html><html lang=\"pt-BR\"><head><meta charset=\"utf-8\">";
            html = html + "<title>Ficha " + child.getId() + "</title></head><body>";
            html = html + "<h1>RECRIAR - Ficha de cadastro</h1><h2>Dados da criança</h2><table border=\"1\">";
            html = html + Row("Ficha", child.getId().ToString());
            html = html + Row("Nome", child.getName());
            html = html + Row("Idade", child.getAge().ToString());
            html = html + Row("CPF", child.getCpf());
            html = html + Row("RG", child.getRg());
            html = html + Row("Endereço", child.getAddress());
            html = html + Row("Bairro", child.getNeighborhood());
            html = html + Row("Cidade", child.getCity());
            html = html + Row("Escola", child.getSchool());
            html = html + Row("Mãe", child.getMotherName());
            html = html + Row("Pai", child.getFatherName());
            html = html + Row("Telefone", child.getPhone());
            html = html + Row("Data do cadastro", child.getRegistrationDate().ToString("dd/MM/yyyy"));
            html = html + "</table><h2>Responsável</h2><table border=\"1\">";
            Guardian guardian = child.getGuardian();
            if (guardian != null)
            {
                html = html + Row("Nome", guardian.getName());
                html = html + Row("RG", guardian.getRg());
                html = html + Row("CPF", guardian.getCpf());
            }
            html = html + "</table><h2>Contato de emergência</h2><table border=\"1\">";
            EmergencyContact contact = child.getEmergencyContact();
            if (contact != null)
            {
                html = html + Row("Nome", contact.getName());
                html = html + Row("Telefone", contact.getPhone());
            }
            html = html + "</table><h2>Composição familiar</h2>";
            if (child.getFamilyMembers().Count == 0) html = html + "<p>Nenhum familiar cadastrado.</p>";
            else
            {
                html = html + "<table border=\"1\"><tr><th>Parentesco</th><th>Nome</th><th>Idade</th></tr>";
                foreach (FamilyMember member in child.getFamilyMembers())
                {
                    html = html + "<tr><td>" + Encode(RelationshipText(member, relationships)) + "</td>";
                    html = html + "<td>" + Encode(member.getName()) + "</td><td>" + member.getAge() + "</td></tr>";
                }
                html = html + "</table>";
            }
            html = html + "<h2>Outras informações</h2><table border=\"1\">";
            html = html + Row("Centro de saúde", child.getHealthCenter());
            html = html + Row("Ficha de saúde", child.getFileNumber());
            html = html + Row("Alergia", child.getAllergy());
            html = html + "</table><p>Ficha gerada em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".</p></body></html>";
            File.WriteAllText(path, html);
            return path;
        }

        private string RelationshipText(FamilyMember member, List<RelationshipType> relationships)
        {
            if (member.getRelationshipTypeId() == new RelationshipType().getOtherId())
            {
                string other = member.getOtherRelationship();
                if (!string.IsNullOrWhiteSpace(other)) return other;
            }
            foreach (RelationshipType type in relationships)
            {
                if (type.getId() == member.getRelationshipTypeId()) return type.getName();
            }
            RelationshipType relationship = member.getRelationshipType();
            if (relationship != null) return relationship.getName();
            return "";
        }

        private string Row(string label, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) value = "-";
            return "<tr><th>" + Encode(label) + "</th><td>" + Encode(value) + "</td></tr>";
        }

        private string Encode(string text)
        {
            if (text == null) return "";
            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&#39;");
        }
    }
}
