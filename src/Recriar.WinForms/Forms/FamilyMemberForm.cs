using Recriar.Application.Dtos;
using Recriar.Domain.Entities;

namespace Recriar.WinForms.Forms;

internal sealed class FamilyMemberForm : Form
{
    private readonly ComboBox _relationship = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly Label _otherLabel = new() { Text = "Qual parentesco?", AutoSize = true };
    private readonly TextBox _other = new() { AccessibleName = "Qual parentesco?" };
    private readonly TextBox _name = new() { AccessibleName = "Nome do familiar" };
    private readonly TextBox _age = new() { AccessibleName = "Idade do familiar" };

    internal FamilyMemberDto Member { get; private set; }

    internal FamilyMemberForm(List<RelationshipType> relationships, FamilyMemberDto member)
    {
        Member = member;
        Text = "Familiar";
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(460, 210);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = true;

        _relationship.AccessibleName = "Parentesco";
        foreach (RelationshipType type in relationships)
        {
            _relationship.Items.Add(type.getName());
        }
        _name.Text = member.getName();
        _age.Text = member.getAge();
        _other.Text = member.getOtherRelationship();

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5, Padding = new Padding(12) };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        AddRow(layout, 0, new Label { Text = "Parentesco", AutoSize = true }, _relationship);
        AddRow(layout, 1, _otherLabel, _other);
        AddRow(layout, 2, new Label { Text = "Nome", AutoSize = true }, _name);
        AddRow(layout, 3, new Label { Text = "Idade", AutoSize = true }, _age);

        var apply = new Button { Text = "Aplicar", AutoSize = true };
        var cancel = new Button { Text = "Cancelar", AutoSize = true, DialogResult = DialogResult.Cancel };
        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft
        };
        actions.Controls.AddRange([apply, cancel]);
        layout.Controls.Add(actions, 0, 4);
        layout.SetColumnSpan(actions, 2);
        Controls.Add(layout);

        // Data binding can select the first item while the dialog is being created.
        Load += (_, _) =>
        {
            for (int i = 0; i < relationships.Count; i++)
            {
                if (relationships[i].getId() == member.getRelationshipTypeId())
                    _relationship.SelectedIndex = i;
            }
            UpdateOtherField();
        };
        _relationship.SelectedIndexChanged += (_, _) => UpdateOtherField();
        AcceptButton = apply;
        CancelButton = cancel;
        apply.Click += (_, _) =>
        {
            int relationshipId = 0;
            if (_relationship.SelectedIndex >= 0)
                relationshipId = relationships[_relationship.SelectedIndex].getId();
            Member = new FamilyMemberDto();
        Member.setId(member.getId());
        Member.setName(_name.Text);
        Member.setAge(_age.Text);
        Member.setRelationshipTypeId(relationshipId);
        Member.setOtherRelationship(relationshipId == new RelationshipType().getOtherId() ? _other.Text : string.Empty);
            DialogResult = DialogResult.OK;
        };
    }

    private void UpdateOtherField()
    {
        bool isOther = _relationship.SelectedItem as string == "Outro";
        _otherLabel.Visible = _other.Visible = isOther;
    }

    private static void AddRow(TableLayoutPanel layout, int row, Label label, Control input)
    {
        label.Anchor = AnchorStyles.Left;
        input.Dock = DockStyle.Fill;
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(label, 0, row);
        layout.Controls.Add(input, 1, row);
    }
}
