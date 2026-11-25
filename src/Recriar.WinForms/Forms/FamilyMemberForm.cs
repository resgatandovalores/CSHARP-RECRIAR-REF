using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Recriar.Application.Dtos;
using Recriar.Domain.Entities;

namespace Recriar.WinForms.Forms
{
    public partial class FamilyMemberForm : Form
    {
        private List<RelationshipType> relationships;
        private FamilyMemberDto member;

        public FamilyMemberForm()
        {
            relationships = new List<RelationshipType>();
            member = new FamilyMemberDto();
            InitializeComponent();
        }
        public FamilyMemberForm(List<RelationshipType> relationships, FamilyMemberDto original)
        {
            this.relationships = relationships;
            member = original;
            InitializeComponent();
            foreach (RelationshipType type in relationships) cmbParentesco.Items.Add(type.getName());
            for (int i = 0; i < relationships.Count; i++)
            {
                if (relationships[i].getId() == original.getRelationshipTypeId()) cmbParentesco.SelectedIndex = i;
            }
            txtNome.Text = original.getName();
            txtIdade.Text = original.getAge();
            txtOutro.Text = original.getOtherRelationship();
            UpdateOtherField();
        }
        public FamilyMemberDto getMember() { return member; }
        private int SelectedRelationship()
        {
            if (cmbParentesco.SelectedIndex < 0) return 0;
            return relationships[cmbParentesco.SelectedIndex].getId();
        }
        private void UpdateOtherField()
        {
            bool other = SelectedRelationship() == new RelationshipType().getOtherId();
            lblOutro.Visible = other;
            txtOutro.Visible = other;
        }
        private void cmbParentesco_SelectedIndexChanged(object sender, EventArgs e) { UpdateOtherField(); }
        private void btnAplicar_Click(object sender, EventArgs e)
        {
            FamilyMemberDto edited = new FamilyMemberDto();
            edited.setId(member.getId());
            edited.setName(txtNome.Text);
            edited.setAge(txtIdade.Text);
            int typeId = SelectedRelationship();
            edited.setRelationshipTypeId(typeId);
            if (typeId == new RelationshipType().getOtherId()) edited.setOtherRelationship(txtOutro.Text);
            else edited.setOtherRelationship("");
            member = edited;
            DialogResult = DialogResult.OK;
        }
    }
}
