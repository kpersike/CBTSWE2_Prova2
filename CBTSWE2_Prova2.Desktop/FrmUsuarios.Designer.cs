/*
Nome: Kaik Persike Maiorquino
Prontuário: CB3029689

Nome: Luiz Gustavo Verissimo Monteiro
Prontuário: CB3030326
*/
namespace CBTSWE2_Prova2.Desktop
{
    partial class FrmUsuarios
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblUsuarioLogado = new Label();
            lblNome = new Label();
            lblSenha = new Label();
            txtNome = new TextBox();
            txtSenha = new TextBox();
            chkStatus = new CheckBox();
            btnCadastrar = new Button();
            btnAtualizar = new Button();
            btnExcluir = new Button();
            btnLimpar = new Button();
            btnRecarregar = new Button();
            btnSair = new Button();
            dgvUsuarios = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(25, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(327, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gerenciamento de usuários";
            // 
            // lblUsuarioLogado
            // 
            lblUsuarioLogado.AutoSize = true;
            lblUsuarioLogado.Location = new Point(25, 50);
            lblUsuarioLogado.Name = "lblUsuarioLogado";
            lblUsuarioLogado.Size = new Size(109, 15);
            lblUsuarioLogado.TabIndex = 1;
            lblUsuarioLogado.Text = "Usuário conectado:";
            // 
            // lblNome
            // 
            lblNome.AutoSize = true;
            lblNome.Location = new Point(25, 85);
            lblNome.Name = "lblNome";
            lblNome.Size = new Size(43, 15);
            lblNome.TabIndex = 2;
            lblNome.Text = "Nome:";
            // 
            // lblSenha
            // 
            lblSenha.AutoSize = true;
            lblSenha.Location = new Point(445, 85);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(42, 15);
            lblSenha.TabIndex = 3;
            lblSenha.Text = "Senha:";
            // 
            // txtNome
            // 
            txtNome.Location = new Point(25, 105);
            txtNome.MaxLength = 100;
            txtNome.Name = "txtNome";
            txtNome.Size = new Size(390, 23);
            txtNome.TabIndex = 4;
            // 
            // txtSenha
            // 
            txtSenha.Location = new Point(445, 105);
            txtSenha.MaxLength = 0;
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(390, 23);
            txtSenha.TabIndex = 5;
            txtSenha.UseSystemPasswordChar = true;
            // 
            // chkStatus
            // 
            chkStatus.AutoSize = true;
            chkStatus.Checked = true;
            chkStatus.CheckState = CheckState.Checked;
            chkStatus.Location = new Point(25, 140);
            chkStatus.Name = "chkStatus";
            chkStatus.Size = new Size(95, 19);
            chkStatus.TabIndex = 6;
            chkStatus.Text = "Usuário ativo";
            chkStatus.UseVisualStyleBackColor = true;
            // 
            // btnCadastrar
            // 
            btnCadastrar.Location = new Point(25, 175);
            btnCadastrar.Name = "btnCadastrar";
            btnCadastrar.Size = new Size(190, 35);
            btnCadastrar.TabIndex = 7;
            btnCadastrar.Text = "Cadastrar";
            btnCadastrar.UseVisualStyleBackColor = true;
            btnCadastrar.Click += btnCadastrar_Click;
            // 
            // btnAtualizar
            // 
            btnAtualizar.Location = new Point(230, 175);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.Size = new Size(190, 35);
            btnAtualizar.TabIndex = 8;
            btnAtualizar.Text = "Atualizar";
            btnAtualizar.UseVisualStyleBackColor = true;
            btnAtualizar.Click += btnAtualizar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Location = new Point(435, 175);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(190, 35);
            btnExcluir.TabIndex = 9;
            btnExcluir.Text = "Excluir";
            btnExcluir.UseVisualStyleBackColor = true;
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnLimpar
            // 
            btnLimpar.Location = new Point(640, 175);
            btnLimpar.Name = "btnLimpar";
            btnLimpar.Size = new Size(190, 35);
            btnLimpar.TabIndex = 10;
            btnLimpar.Text = "Limpar";
            btnLimpar.UseVisualStyleBackColor = true;
            btnLimpar.Click += btnLimpar_Click;
            // 
            // btnRecarregar
            // 
            btnRecarregar.Location = new Point(25, 455);
            btnRecarregar.Name = "btnRecarregar";
            btnRecarregar.Size = new Size(140, 35);
            btnRecarregar.TabIndex = 11;
            btnRecarregar.Text = "Recarregar";
            btnRecarregar.UseVisualStyleBackColor = true;
            btnRecarregar.Click += btnRecarregar_Click;
            // 
            // btnSair
            // 
            btnSair.Location = new Point(695, 455);
            btnSair.Name = "btnSair";
            btnSair.Size = new Size(140, 35);
            btnSair.TabIndex = 12;
            btnSair.Text = "Sair";
            btnSair.UseVisualStyleBackColor = true;
            btnSair.Click += btnSair_Click;
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsuarios.Location = new Point(25, 225);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.Size = new Size(810, 215);
            dgvUsuarios.TabIndex = 13;
            dgvUsuarios.CellClick += dgvUsuarios_CellClick;
            // 
            // FrmUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 511);
            Controls.Add(dgvUsuarios);
            Controls.Add(btnSair);
            Controls.Add(btnRecarregar);
            Controls.Add(btnLimpar);
            Controls.Add(btnExcluir);
            Controls.Add(btnAtualizar);
            Controls.Add(btnCadastrar);
            Controls.Add(chkStatus);
            Controls.Add(txtSenha);
            Controls.Add(txtNome);
            Controls.Add(lblSenha);
            Controls.Add(lblNome);
            Controls.Add(lblUsuarioLogado);
            Controls.Add(lblTitulo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmUsuarios";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gerenciamento de usuários";
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Label lblUsuarioLogado;
        private Label lblNome;
        private Label lblSenha;
        private TextBox txtNome;
        private TextBox txtSenha;
        private CheckBox chkStatus;
        private Button btnCadastrar;
        private Button btnAtualizar;
        private Button btnExcluir;
        private Button btnLimpar;
        private Button btnRecarregar;
        private Button btnSair;
        private DataGridView dgvUsuarios;
    }
}