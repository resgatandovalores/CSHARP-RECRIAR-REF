using Recriar.Application.Dtos;
using Recriar.Application.Exceptions;
using Recriar.Application.Services;
using Recriar.Infrastructure.Logging;
using Recriar.Application.Validators;
using Recriar.Domain.Entities;

namespace Recriar.WinForms.Forms;

internal sealed class ChildForm : Form
{
    private readonly ChildService _service;
    private readonly int _childId;
    private readonly List<RelationshipType> _relationshipTypes;
    private readonly List<FamilyMemberDto> _familyMembers;
    private readonly TextBox _name = new();
    private readonly TextBox _age = new();
    private readonly TextBox _cpf = new();
    private readonly TextBox _rg = new();
    private readonly TextBox _address = new();
    private readonly TextBox _neighborhood = new();
    private readonly TextBox _city = new();
    private readonly TextBox _school = new();
    private readonly TextBox _mother = new();
    private readonly TextBox _father = new();
    private readonly TextBox _phone = new();
    private readonly TextBox _date = new() { PlaceholderText = "dd/mm/aaaa" };
    private readonly TextBox _guardianName = new();
    private readonly TextBox _guardianRg = new();
    private readonly TextBox _guardianCpf = new();
    private readonly TextBox _emergencyName = new();
    private readonly TextBox _emergencyPhone = new();
    private readonly TextBox _healthCenter = new();
    private readonly TextBox _fileNumber = new();
    private readonly TextBox _allergy = new();
    private readonly ListView _familyList = new()
    {
        Dock = DockStyle.Fill,
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false,
        HideSelection = false
    };
    private readonly Button _addFamily = new() { Text = "Adicionar familiar", AutoSize = true };
    private readonly Button _editFamily = new() { Text = "Editar", AutoSize = true };
    private readonly Button _removeFamily = new() { Text = "Remover", AutoSize = true };
    private readonly Label _familyCount = new() { AutoSize = true };

    internal ChildForm(ChildService service, ChildDto child, List<RelationshipType> relationshipTypes)
    {
        _service = service;
        _childId = child.getId();
        _relationshipTypes = relationshipTypes;
        _familyMembers = new List<FamilyMemberDto>(child.getFamilyMembers());
        Text = child.getId() == 0 ? "Novo cadastro" : $"Editar ficha {child.getId()}";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(740, 590);
        MinimumSize = new Size(580, 450);
        ShowInTaskbar = true;

        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(CreateFieldsTab("Criança",
            ("Nome *", _name, child.getName()),
            ("Idade *", _age, child.getAge()),
            ("CPF *", _cpf, child.getCpf()),
            ("RG *", _rg, child.getRg()),
            ("Endereço", _address, child.getAddress()),
            ("Bairro *", _neighborhood, child.getNeighborhood()),
            ("Cidade *", _city, child.getCity()),
            ("Escola", _school, child.getSchool()),
            ("Telefone *", _phone, child.getPhone()),
            ("Data de cadastro *", _date, child.getRegistrationDate())));
        tabs.TabPages.Add(CreateFieldsTab("Responsáveis e emergência",
            ("Nome da mãe *", _mother, child.getMotherName()),
            ("Nome do pai *", _father, child.getFatherName()),
            ("Nome do responsável *", _guardianName, child.getGuardianName()),
            ("RG do responsável *", _guardianRg, child.getGuardianRg()),
            ("CPF do responsável *", _guardianCpf, child.getGuardianCpf()),
            ("Contato de emergência *", _emergencyName, child.getEmergencyContactName()),
            ("Telefone de emergência *", _emergencyPhone, child.getEmergencyContactPhone())));
        tabs.TabPages.Add(CreateFieldsTab("Saúde",
            ("Centro de saúde", _healthCenter, child.getHealthCenter()),
            ("Número da ficha de saúde", _fileNumber, child.getFileNumber()),
            ("Alergia", _allergy, child.getAllergy())));
        tabs.TabPages.Add(CreateFamilyTab());

        var save = new Button { Text = "Salvar ficha", AutoSize = true };
        var cancel = new Button { Text = "Cancelar", AutoSize = true, DialogResult = DialogResult.Cancel };
        var actions = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.RightToLeft
        };
        actions.Controls.AddRange([save, cancel]);

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(12) };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(new Label
        {
            AutoSize = true,
            Text = "* Campo obrigatório. Use apenas números em CPF, RG e telefone."
        }, 0, 0);
        layout.Controls.Add(tabs, 0, 1);
        layout.Controls.Add(actions, 0, 2);
        Controls.Add(layout);

        AcceptButton = save;
        CancelButton = cancel;
        save.Click += (_, _) => SaveChild();
    }

    private static TabPage CreateFieldsTab(string title, params (string Label, TextBox Input, string Value)[] fields)
    {
        var tab = new TabPage(title) { AutoScroll = true };
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2,
            RowCount = fields.Length, Padding = new Padding(12)
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 205));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        for (int index = 0; index < fields.Length; index++)
        {
            var (label, input, value) = fields[index];
            input.Text = value;
            input.AccessibleName = label.TrimEnd(' ', '*');
            input.Dock = DockStyle.Fill;
            input.Margin = new Padding(3, 5, 3, 5);
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, index);
            layout.Controls.Add(input, 1, index);
        }

        tab.Controls.Add(layout);
        return tab;
    }

    private TabPage CreateFamilyTab()
    {
        var tab = new TabPage("Composição familiar");
        _familyList.Columns.Add("Parentesco", 155);
        _familyList.Columns.Add("Nome", 260);
        _familyList.Columns.Add("Idade", 70);

        var actions = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        actions.Controls.AddRange([_addFamily, _editFamily, _removeFamily]);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(10) };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(_familyCount, 0, 0);
        layout.Controls.Add(_familyList, 0, 1);
        layout.Controls.Add(actions, 0, 2);
        tab.Controls.Add(layout);

        _addFamily.Click += (_, _) => OpenFamilyMember();
        _editFamily.Click += (_, _) => EditSelectedFamilyMember();
        _familyList.DoubleClick += (_, _) => EditSelectedFamilyMember();
        _removeFamily.Click += (_, _) =>
        {
            if (_familyList.SelectedIndices.Count == 1)
            {
                _familyMembers.RemoveAt(_familyList.SelectedIndices[0]);
                RefreshFamily();
            }
        };
        _familyList.SelectedIndexChanged += (_, _) => UpdateFamilyActions();

        RefreshFamily();
        return tab;
    }

    private void EditSelectedFamilyMember()
    {
        if (_familyList.SelectedIndices.Count == 1)
            OpenFamilyMember(_familyList.SelectedIndices[0]);
    }

    private void OpenFamilyMember(int? index = null)
    {
        var member = index.HasValue ? _familyMembers[index.Value] : new FamilyMemberDto();
        using var editor = new FamilyMemberForm(_relationshipTypes, member);
        if (editor.ShowDialog(this) != DialogResult.OK)
            return;

        if (index.HasValue)
            _familyMembers[index.Value] = editor.Member;
        else
            _familyMembers.Add(editor.Member);

        RefreshFamily(index ?? (_familyMembers.Count - 1));
    }

    private void RefreshFamily(int? selectedIndex = null)
    {
        _familyList.Items.Clear();
        foreach (var member in _familyMembers)
        {
            string relationship = member.getOtherRelationship();
            if (member.getRelationshipTypeId() != new RelationshipType().getOtherId())
            {
                foreach (RelationshipType type in _relationshipTypes)
                {
                    if (type.getId() == member.getRelationshipTypeId())
                        relationship = type.getName();
                }
            }
            _familyList.Items.Add(new ListViewItem([relationship, member.getName(), member.getAge()]));
        }

        if (selectedIndex.HasValue)
            _familyList.Items[selectedIndex.Value].Selected = true;

        _familyCount.Text = $"Familiares na ficha: {_familyMembers.Count} de {new ChildValidator().getMaxFamilyMembers()}";
        UpdateFamilyActions();
    }

    private void UpdateFamilyActions()
    {
        _addFamily.Enabled = _familyMembers.Count < new ChildValidator().getMaxFamilyMembers();
        _editFamily.Enabled = _removeFamily.Enabled = _familyList.SelectedIndices.Count == 1;
    }

    private void SaveChild()
    {
        var child = new ChildDto();
        child.setId(_childId);
        child.setName(_name.Text);
        child.setAge(_age.Text);
        child.setCpf(_cpf.Text);
        child.setRg(_rg.Text);
        child.setAddress(_address.Text);
        child.setNeighborhood(_neighborhood.Text);
        child.setCity(_city.Text);
        child.setSchool(_school.Text);
        child.setMotherName(_mother.Text);
        child.setFatherName(_father.Text);
        child.setPhone(_phone.Text);
        child.setRegistrationDate(_date.Text);
        child.setGuardianName(_guardianName.Text);
        child.setGuardianRg(_guardianRg.Text);
        child.setGuardianCpf(_guardianCpf.Text);
        child.setEmergencyContactName(_emergencyName.Text);
        child.setEmergencyContactPhone(_emergencyPhone.Text);
        child.setHealthCenter(_healthCenter.Text);
        child.setFileNumber(_fileNumber.Text);
        child.setAllergy(_allergy.Text);
        child.setFamilyMembers(_familyMembers);

        try
        {
            if (_childId == 0)
            {
                int id = _service.Register(child);
                MessageBox.Show(this, $"Cadastro realizado. Ficha número {id}.", "RECRIAR",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _service.Update(_childId, child);
            }

            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, new ErrorHandler().GetMessage(ex), "Atenção",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
