using System.Net;
using System.Net.Http.Json;
using CBTSWE2_Prova2.Core.DTOs;
using CBTSWE2_Prova2.Desktop.Services;

namespace CBTSWE2_Prova2.Desktop
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) ||
       string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show(
                    "Informe o usuário e a senha.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            btnEntrar.Enabled = false;

            try
            {
                ApiService.RemoverToken();

                var login = new LoginDTO
                {
                    Nome = txtUsuario.Text.Trim(),
                    Senha = txtSenha.Text
                };

                var response = await ApiService.Client.PostAsJsonAsync(
                    "api/Auth/login", login
                );

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Usuário ou senha inválidos.");
                    return;
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    MessageBox.Show("O usuário está inativo.");
                    return;
                }

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Não foi possível realizar o login.");
                    return;
                }

                var resultado = await response.Content
                    .ReadFromJsonAsync<LoginResponseDTO>();

                if (resultado == null ||
                    string.IsNullOrWhiteSpace(resultado.Token))
                {
                    MessageBox.Show("Resposta inválida da API.");
                    return;
                }

                ApiService.DefinirToken(resultado.Token);

                // Verifica se o usuário possui acesso administrativo.
                var acesso = await ApiService.Client.GetAsync(
                    "api/Usuarios"
                );

                if (!acesso.IsSuccessStatusCode)
                {
                    ApiService.RemoverToken();

                    if (acesso.StatusCode == HttpStatusCode.Forbidden)
                    {
                        MessageBox.Show(
                            "Acesso permitido somente ao administrador.",
                            "Acesso negado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            "Não foi possível validar o acesso administrativo."
                        );
                    }

                    return;
                }

                txtSenha.Clear();

                using var frmUsuarios =
                    new FrmUsuarios(resultado.Nome);

                Hide();

                try
                {
                    frmUsuarios.ShowDialog();
                }
                finally
                {
                    ApiService.RemoverToken();

                    txtUsuario.Clear();
                    txtSenha.Clear();

                    Show();

                    txtUsuario.Focus();
                }
            }
            catch (Exception ex)
            {
                ApiService.RemoverToken();

                MessageBox.Show(
                    $"Erro ao comunicar com a API: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                btnEntrar.Enabled = true;
            }
        }
    }
}
