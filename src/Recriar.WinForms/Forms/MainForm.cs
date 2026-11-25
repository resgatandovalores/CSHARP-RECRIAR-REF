using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Recriar.Application.Dtos;
using Recriar.Application.Exceptions;
using Recriar.Application.Services;
using Recriar.Domain.Entities;
using Recriar.Infrastructure.Logging;

namespace Recriar.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private ChildService service;
        private FileErrorLogger errorLogger;
        private List<ChildSummaryDto> children;

        public MainForm()
        {
            children = new List<ChildSummaryDto>();
            InitializeComponent();
        }

        public MainForm(ChildService service, FileErrorLogger errorLogger)
        {
            this.service = service;
            this.errorLogger = errorLogger;
            children = new List<ChildSummaryDto>();
            InitializeComponent();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            LoadChildren("");
        }

        private void LoadChildren(string search)
        {
            try
            {
                if (search.Trim() == "") children = service.GetAll();
                else children = service.SearchByName(search);
                grdCriancas.Rows.Clear();
                foreach (ChildSummaryDto child in children)
                {
                    grdCriancas.Rows.Add(child.getId(), child.getName(), child.getCpf(), child.getPhone());
                }
                grdCriancas.ClearSelection();
                lblQuantidade.Text = "Cadastros exibidos: " + children.Count;
            }
            catch (Exception ex)
            {
                children.Clear();
                grdCriancas.Rows.Clear();
                lblQuantidade.Text = "Não foi possível carregar os cadastros.";
                ShowError(ex);
            }
            UpdateButtons();
        }

        private ChildSummaryDto SelectedChild()
        {
            if (grdCriancas.SelectedRows.Count == 0) return null;
            int id = Convert.ToInt32(grdCriancas.SelectedRows[0].Cells[0].Value);
            foreach (ChildSummaryDto child in children)
            {
                if (child.getId() == id) return child;
            }
            return null;
        }

        private void OpenChild(int id)
        {
            try
            {
                ChildDto child = new ChildDto();
                if (id > 0) child = service.GetById(id);
                List<RelationshipType> relationships = service.GetRelationshipTypes();
                using (ChildForm form = new ChildForm(service, child, relationships))
                {
                    if (form.ShowDialog(this) == DialogResult.OK) LoadChildren(txtPesquisa.Text);
                }
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private void btnPesquisar_Click(object sender, EventArgs e) { LoadChildren(txtPesquisa.Text); }
        private void btnListar_Click(object sender, EventArgs e)
        {
            txtPesquisa.Clear();
            LoadChildren("");
        }
        private void btnNovo_Click(object sender, EventArgs e) { OpenChild(0); }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            ChildSummaryDto child = SelectedChild();
            if (child != null) OpenChild(child.getId());
        }
        private void grdCriancas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnEditar_Click(sender, e);
        }
        private void grdCriancas_SelectionChanged(object sender, EventArgs e) { UpdateButtons(); }
        private void UpdateButtons()
        {
            bool selected = SelectedChild() != null;
            btnEditar.Enabled = selected;
            btnExcluir.Enabled = selected;
            btnExportar.Enabled = selected;
        }
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            ChildSummaryDto child = SelectedChild();
            if (child == null) return;
            DialogResult answer = MessageBox.Show(this, "Excluir a ficha de " + child.getName() + "?",
                "Confirmar exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes) return;
            try
            {
                service.Delete(child.getId());
                LoadChildren(txtPesquisa.Text);
            }
            catch (Exception ex) { ShowError(ex); }
        }
        private void btnExportar_Click(object sender, EventArgs e)
        {
            ChildSummaryDto child = SelectedChild();
            if (child == null) return;
            string path;
            try { path = service.ExportFile(child.getId()); }
            catch (Exception ex) { ShowError(ex); return; }
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(path);
                info.UseShellExecute = true;
                Process.Start(info);
            }
            catch (Exception ex)
            {
                errorLogger.LogError(ex, "Abertura da ficha");
                MessageBox.Show(this, "A ficha foi salva em: " + path, "Ficha exportada");
            }
        }
        private void ShowError(Exception ex)
        {
            MessageBox.Show(this, new ErrorHandler().GetMessage(ex), "Atenção",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
