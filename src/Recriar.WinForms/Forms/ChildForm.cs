using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Recriar.Application.Dtos;
using Recriar.Application.Exceptions;
using Recriar.Application.Services;
using Recriar.Application.Validators;
using Recriar.Domain.Entities;

namespace Recriar.WinForms.Forms
{
    public partial class ChildForm : Form
    {
        private ChildService service;
        private int childId;
        private List<RelationshipType> relationships;
        private List<FamilyMemberDto> family;

        public ChildForm()
        {
            relationships = new List<RelationshipType>();
            family = new List<FamilyMemberDto>();
            InitializeComponent();
        }

        public ChildForm(ChildService service, ChildDto child, List<RelationshipType> relationships)
        {
            this.service = service;
            this.childId = child.getId();
            this.relationships = relationships;
            family = new List<FamilyMemberDto>();
            foreach (FamilyMemberDto member in child.getFamilyMembers()) family.Add(member);
            InitializeComponent();
            if (childId == 0) Text = "Novo cadastro";
            else Text = "Editar ficha " + childId;
            txtNome.Text = child.getName();
            txtIdade.Text = child.getAge();
            txtCpf.Text = child.getCpf();
            txtRg.Text = child.getRg();
            txtEndereco.Text = child.getAddress();
            txtBairro.Text = child.getNeighborhood();
            txtCidade.Text = child.getCity();
            txtEscola.Text = child.getSchool();
            txtTelefone.Text = child.getPhone();
            txtData.Text = child.getRegistrationDate();
            txtMae.Text = child.getMotherName();
            txtPai.Text = child.getFatherName();
            txtResponsavel.Text = child.getGuardianName();
            txtRgResponsavel.Text = child.getGuardianRg();
            txtCpfResponsavel.Text = child.getGuardianCpf();
            txtContato.Text = child.getEmergencyContactName();
            txtTelefoneContato.Text = child.getEmergencyContactPhone();
            txtSaude.Text = child.getHealthCenter();
            txtFichaSaude.Text = child.getFileNumber();
            txtAlergia.Text = child.getAllergy();
            RefreshFamily();
        }

        private void RefreshFamily()
        {
            lstFamilia.Items.Clear();
            foreach (FamilyMemberDto member in family)
            {
                string relationship = member.getOtherRelationship();
                if (member.getRelationshipTypeId() != new RelationshipType().getOtherId())
                {
                    foreach (RelationshipType type in relationships)
                    {
                        if (type.getId() == member.getRelationshipTypeId()) relationship = type.getName();
                    }
                }
                string[] values = new string[] { relationship, member.getName(), member.getAge() };
                lstFamilia.Items.Add(new ListViewItem(values));
            }
            lblFamilia.Text = "Familiares na ficha: " + family.Count + " de " + new ChildValidator().getMaxFamilyMembers();
            UpdateButtons();
        }
        private void UpdateButtons()
        {
            btnAdicionar.Enabled = family.Count < new ChildValidator().getMaxFamilyMembers();
            btnEditarFamiliar.Enabled = lstFamilia.SelectedIndices.Count == 1;
            btnRemover.Enabled = lstFamilia.SelectedIndices.Count == 1;
        }
        private void OpenFamily(int index)
        {
            FamilyMemberDto member = new FamilyMemberDto();
            if (index >= 0) member = family[index];
            using (FamilyMemberForm form = new FamilyMemberForm(relationships, member))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    if (index >= 0) family[index] = form.getMember();
                    else family.Add(form.getMember());
                    RefreshFamily();
                }
            }
        }
        private void btnAdicionar_Click(object sender, EventArgs e) { OpenFamily(-1); }
        private void btnEditarFamiliar_Click(object sender, EventArgs e)
        {
            if (lstFamilia.SelectedIndices.Count == 1) OpenFamily(lstFamilia.SelectedIndices[0]);
        }
        private void btnRemover_Click(object sender, EventArgs e)
        {
            if (lstFamilia.SelectedIndices.Count == 1)
            {
                family.RemoveAt(lstFamilia.SelectedIndices[0]);
                RefreshFamily();
            }
        }
        private void lstFamilia_SelectedIndexChanged(object sender, EventArgs e) { UpdateButtons(); }
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            ChildDto child = new ChildDto();
            child.setId(childId);
            child.setName(txtNome.Text);
            child.setAge(txtIdade.Text);
            child.setCpf(txtCpf.Text);
            child.setRg(txtRg.Text);
            child.setAddress(txtEndereco.Text);
            child.setNeighborhood(txtBairro.Text);
            child.setCity(txtCidade.Text);
            child.setSchool(txtEscola.Text);
            child.setPhone(txtTelefone.Text);
            child.setRegistrationDate(txtData.Text);
            child.setMotherName(txtMae.Text);
            child.setFatherName(txtPai.Text);
            child.setGuardianName(txtResponsavel.Text);
            child.setGuardianRg(txtRgResponsavel.Text);
            child.setGuardianCpf(txtCpfResponsavel.Text);
            child.setEmergencyContactName(txtContato.Text);
            child.setEmergencyContactPhone(txtTelefoneContato.Text);
            child.setHealthCenter(txtSaude.Text);
            child.setFileNumber(txtFichaSaude.Text);
            child.setAllergy(txtAlergia.Text);
            child.setFamilyMembers(family);
            try
            {
                if (childId == 0)
                {
                    int id = service.Register(child);
                    MessageBox.Show(this, "Cadastro realizado. Ficha número " + id, "RECRIAR");
                }
                else service.Update(childId, child);
                DialogResult = DialogResult.OK;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Confira os dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, new ErrorHandler().GetMessage(ex), "Atenção");
            }
        }
    }
}
