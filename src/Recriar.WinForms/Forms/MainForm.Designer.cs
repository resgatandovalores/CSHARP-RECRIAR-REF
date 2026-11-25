namespace Recriar.WinForms.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblPesquisa;
        private System.Windows.Forms.TextBox txtPesquisa;
        private System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.Button btnListar;
        private System.Windows.Forms.Button btnNovo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.DataGridView grdCriancas;
        private System.Windows.Forms.Label lblQuantidade;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Size = new System.Drawing.Size(600, 30);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Text = "Cadastro de crianças";
            this.lblPesquisa = new System.Windows.Forms.Label();
            this.lblPesquisa.Location = new System.Drawing.Point(20, 62);
            this.lblPesquisa.Size = new System.Drawing.Size(135, 25);
            this.lblPesquisa.Name = "lblPesquisa";
            this.lblPesquisa.Text = "Nome da criança";
            this.txtPesquisa = new System.Windows.Forms.TextBox();
            this.txtPesquisa.Location = new System.Drawing.Point(155, 58);
            this.txtPesquisa.Size = new System.Drawing.Size(300, 25);
            this.txtPesquisa.Name = "txtPesquisa";
            this.txtPesquisa.AccessibleName = "txtPesquisa";
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.btnPesquisar.Location = new System.Drawing.Point(470, 55);
            this.btnPesquisar.Size = new System.Drawing.Size(110, 32);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Text = "Pesquisar";
            this.btnListar = new System.Windows.Forms.Button();
            this.btnListar.Location = new System.Drawing.Point(590, 55);
            this.btnListar.Size = new System.Drawing.Size(110, 32);
            this.btnListar.Name = "btnListar";
            this.btnListar.Text = "Listar todos";
            this.btnNovo = new System.Windows.Forms.Button();
            this.btnNovo.Location = new System.Drawing.Point(20, 100);
            this.btnNovo.Size = new System.Drawing.Size(140, 34);
            this.btnNovo.Name = "btnNovo";
            this.btnNovo.Text = "Novo cadastro";
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEditar.Location = new System.Drawing.Point(170, 100);
            this.btnEditar.Size = new System.Drawing.Size(140, 34);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Text = "Editar ficha";
            this.btnExportar = new System.Windows.Forms.Button();
            this.btnExportar.Location = new System.Drawing.Point(320, 100);
            this.btnExportar.Size = new System.Drawing.Size(140, 34);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Text = "Exportar ficha";
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btnExcluir.Location = new System.Drawing.Point(470, 100);
            this.btnExcluir.Size = new System.Drawing.Size(140, 34);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Text = "Excluir ficha";
            this.grdCriancas = new System.Windows.Forms.DataGridView();
            this.grdCriancas.Location = new System.Drawing.Point(20, 150);
            this.grdCriancas.Size = new System.Drawing.Size(840, 300);
            this.grdCriancas.Name = "grdCriancas";
            this.grdCriancas.ReadOnly = true;
            this.grdCriancas.AllowUserToAddRows = false;
            this.grdCriancas.AllowUserToDeleteRows = false;
            this.grdCriancas.MultiSelect = false;
            this.grdCriancas.RowHeadersVisible = false;
            this.grdCriancas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdCriancas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.lblQuantidade = new System.Windows.Forms.Label();
            this.lblQuantidade.Location = new System.Drawing.Point(20, 467);
            this.lblQuantidade.Size = new System.Drawing.Size(800, 25);
            this.lblQuantidade.Name = "lblQuantidade";
            this.lblQuantidade.Text = "";
            this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
            this.btnListar.Click += new System.EventHandler(this.btnListar_Click);
            this.btnNovo.Click += new System.EventHandler(this.btnNovo_Click);
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            this.grdCriancas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdCriancas_CellDoubleClick);
            this.grdCriancas.SelectionChanged += new System.EventHandler(this.grdCriancas_SelectionChanged);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblPesquisa);
            this.Controls.Add(this.txtPesquisa);
            this.Controls.Add(this.btnPesquisar);
            this.Controls.Add(this.btnListar);
            this.Controls.Add(this.btnNovo);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.btnExcluir);
            this.Controls.Add(this.grdCriancas);
            this.Controls.Add(this.lblQuantidade);
            this.grdCriancas.Columns.Add("Id", "Ficha");
            this.grdCriancas.Columns.Add("Name", "Nome da criança");
            this.grdCriancas.Columns.Add("Cpf", "CPF");
            this.grdCriancas.Columns.Add("Phone", "Telefone");
            this.AcceptButton = this.btnPesquisar;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 510);
            this.Name = "MainForm";
            this.Text = "RECRIAR - Cadastro de crianças";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
