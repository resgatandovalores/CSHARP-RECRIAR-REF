namespace Recriar.WinForms.Forms
{
    partial class FamilyMemberForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblParentesco;
        private System.Windows.Forms.ComboBox cmbParentesco;
        private System.Windows.Forms.Label lblOutro;
        private System.Windows.Forms.TextBox txtOutro;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblIdade;
        private System.Windows.Forms.TextBox txtIdade;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnCancelar;

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
            this.lblParentesco = new System.Windows.Forms.Label();
            this.lblParentesco.Location = new System.Drawing.Point(15, 20);
            this.lblParentesco.Size = new System.Drawing.Size(140, 25);
            this.lblParentesco.Name = "lblParentesco";
            this.lblParentesco.Text = "Parentesco";
            this.cmbParentesco = new System.Windows.Forms.ComboBox();
            this.cmbParentesco.Location = new System.Drawing.Point(165, 17);
            this.cmbParentesco.Size = new System.Drawing.Size(285, 25);
            this.cmbParentesco.Name = "cmbParentesco";
            this.cmbParentesco.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParentesco.AccessibleName = "Parentesco";
            this.lblOutro = new System.Windows.Forms.Label();
            this.lblOutro.Location = new System.Drawing.Point(15, 60);
            this.lblOutro.Size = new System.Drawing.Size(140, 25);
            this.lblOutro.Name = "lblOutro";
            this.lblOutro.Text = "Qual parentesco?";
            this.txtOutro = new System.Windows.Forms.TextBox();
            this.txtOutro.Location = new System.Drawing.Point(165, 57);
            this.txtOutro.Size = new System.Drawing.Size(285, 25);
            this.txtOutro.Name = "txtOutro";
            this.txtOutro.AccessibleName = "Qual parentesco?";
            this.lblNome = new System.Windows.Forms.Label();
            this.lblNome.Location = new System.Drawing.Point(15, 100);
            this.lblNome.Size = new System.Drawing.Size(140, 25);
            this.lblNome.Name = "lblNome";
            this.lblNome.Text = "Nome";
            this.txtNome = new System.Windows.Forms.TextBox();
            this.txtNome.Location = new System.Drawing.Point(165, 97);
            this.txtNome.Size = new System.Drawing.Size(285, 25);
            this.txtNome.Name = "txtNome";
            this.txtNome.AccessibleName = "Nome do familiar";
            this.lblIdade = new System.Windows.Forms.Label();
            this.lblIdade.Location = new System.Drawing.Point(15, 140);
            this.lblIdade.Size = new System.Drawing.Size(140, 25);
            this.lblIdade.Name = "lblIdade";
            this.lblIdade.Text = "Idade";
            this.txtIdade = new System.Windows.Forms.TextBox();
            this.txtIdade.Location = new System.Drawing.Point(165, 137);
            this.txtIdade.Size = new System.Drawing.Size(285, 25);
            this.txtIdade.Name = "txtIdade";
            this.txtIdade.AccessibleName = "Idade do familiar";
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnAplicar.Location = new System.Drawing.Point(335, 185);
            this.btnAplicar.Size = new System.Drawing.Size(115, 32);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Text = "Aplicar";
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnCancelar.Location = new System.Drawing.Point(210, 185);
            this.btnCancelar.Size = new System.Drawing.Size(115, 32);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmbParentesco.SelectedIndexChanged += new System.EventHandler(this.cmbParentesco_SelectedIndexChanged);
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            this.Controls.Add(this.lblParentesco);
            this.Controls.Add(this.cmbParentesco);
            this.Controls.Add(this.lblOutro);
            this.Controls.Add(this.txtOutro);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblIdade);
            this.Controls.Add(this.txtIdade);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.btnCancelar);
            this.AcceptButton = this.btnAplicar;
            this.CancelButton = this.btnCancelar;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 235);
            this.Name = "FamilyMemberForm";
            this.Text = "Familiar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
