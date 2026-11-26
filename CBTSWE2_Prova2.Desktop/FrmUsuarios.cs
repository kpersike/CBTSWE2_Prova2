
using System.Net;
using System.Net.Http.Json;
using CBTSWE2_Prova2.Core.DTOs;
using CBTSWE2_Prova2.Desktop.Services;
using System.Text.Json;

namespace CBTSWE2_Prova2.Desktop
{
    public partial class FrmUsuarios : Form
    {
        private readonly string _nomeAdministrador;
        private int? _idUsuarioSelecionado = null;

        private void LimparCampos()
        {
            _idUsuarioSelecionado = null;

            txtNome.Clear();
            txtSenha.Clear();

            chkStatus.Checked = true;

            dgvUsuarios.ClearSelection();

            btnCadastrar.Enabled = true;
            btnAtualizar.Enabled = false;
            btnExcluir.Enabled = false;

            txtNome.Focus();
        }

        private async Task ExibirErroApiAsync(
    HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                response.StatusCode == HttpStatusCode.Forbidden)
            {
                MessageBox.Show(
                    "Sessão expirada ou acesso não autorizado."
                );

                Close();
                return;
            }

            string mensagem =
                $"Não foi possível realizar a operação. " +
                $"Código HTTP: {(int)response.StatusCode}";

            var conteudo = await response.Content
                .ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(conteudo))
            {
                try
                {
                    using var json = JsonDocument.Parse(conteudo);

                    if (json.RootElement.ValueKind ==
                        JsonValueKind.Object)
                    {
                        if (json.RootElement.TryGetProperty(
                            "mensagem", out var propriedade))
                        {
                            mensagem = propriedade.ToString();
                        }
                        else if (json.RootElement.TryGetProperty(
                            "errors", out var erros))
                        {
                            mensagem = erros.ToString();
                        }
                        else if (json.RootElement.TryGetProperty(
                            "title", out var titulo))
                        {
                            mensagem = titulo.ToString();
                        }
                    }
                    else if (json.RootElement.ValueKind ==
                             JsonValueKind.String)
                    {
                        mensagem = json.RootElement.GetString()
                            ?? mensagem;
                    }
                }
                catch (JsonException)
                {
                    mensagem = conteudo;
                }
            }

            MessageBox.Show(
                mensagem,
                "Atenção",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        public FrmUsuarios(string nomeAdministrador)
        {
            InitializeComponent();

            _nomeAdministrador = nomeAdministrador;

            lblUsuarioLogado.Text =
                $"Usuário conectado: {_nomeAdministrador}";

            ConfigurarDataGridView();

            LimparCampos();

            Load += FrmUsuarios_Load;

        }

        private void ConfigurarDataGridView()
        {
            dgvUsuarios.AutoGenerateColumns = false;

            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colId",
                    HeaderText = "ID",
                    DataPropertyName = "Id",
                    FillWeight = 20
                }
            );

            dgvUsuarios.Columns.Add(
                new DataGridViewTextBoxColumn
                {
                    Name = "colNome",
                    HeaderText = "Nome",
                    DataPropertyName = "Nome",
                    FillWeight = 60
                }
            );

            dgvUsuarios.Columns.Add(
                new DataGridViewCheckBoxColumn
                {
                    Name = "colStatus",
                    HeaderText = "Ativo",
                    DataPropertyName = "Status",
                    FillWeight = 20
                }
            );
        }

        private async void FrmUsuarios_Load(
            object? sender, EventArgs e)
        {
            await CarregarUsuariosAsync();
        }

        private async Task CarregarUsuariosAsync()
        {
            try
            {
                using var response =
                    await ApiService.Client.GetAsync(
                        "api/Usuarios"
                    );

                if (response.StatusCode ==
                    HttpStatusCode.Unauthorized ||
                    response.StatusCode ==
                    HttpStatusCode.Forbidden)
                {
                    MessageBox.Show(
                        "Sessão expirada ou acesso não autorizado."
                    );

                    Close();
                    return;
                }

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Não foi possível consultar os usuários."
                    );

                    return;
                }

                var usuarios = await response.Content
                    .ReadFromJsonAsync<List<UsuarioResponseDTO>>();

                dgvUsuarios.DataSource =
                    usuarios ?? new List<UsuarioResponseDTO>();

                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao consultar usuários: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async void btnRecarregar_Click(
            object sender, EventArgs e)
        {
            btnRecarregar.Enabled = false;

            try
            {
                await CarregarUsuariosAsync();
            }
            finally
            {
                btnRecarregar.Enabled = true;
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvUsuarios_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var usuario = dgvUsuarios.Rows[e.RowIndex]
                .DataBoundItem as UsuarioResponseDTO;

            if (usuario == null)
                return;

            _idUsuarioSelecionado = usuario.Id;

            txtNome.Text = usuario.Nome;

            txtSenha.Clear();

            chkStatus.Checked = usuario.Status;

            btnCadastrar.Enabled = false;
            btnAtualizar.Enabled = true;
            btnExcluir.Enabled = true;
        }

        private async void btnCadastrar_Click(
            object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Informe o nome e a senha.");
                return;
            }

            if (txtSenha.Text.Length < 6)
            {
                MessageBox.Show(
                    "A senha deve possuir pelo menos 6 caracteres."
                );

                return;
            }

            btnCadastrar.Enabled = false;

            try
            {
                var usuario = new UsuarioCadastroDTO
                {
                    Nome = txtNome.Text.Trim(),
                    Senha = txtSenha.Text,
                    Status = chkStatus.Checked
                };

                using var response =
                    await ApiService.Client.PostAsJsonAsync(
                        "api/Usuarios", usuario
                    );

                if (!response.IsSuccessStatusCode)
                {
                    await ExibirErroApiAsync(response);
                    return;
                }

                MessageBox.Show(
                    "Usuário cadastrado com sucesso!",
                    "Cadastro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LimparCampos();

                await CarregarUsuariosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao cadastrar usuário: {ex.Message}"
                );
            }
            finally
            {
                btnCadastrar.Enabled =
                    _idUsuarioSelecionado == null;
            }
        }

        private async void btnAtualizar_Click(
            object sender, EventArgs e)
        {
            if (_idUsuarioSelecionado == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome do usuário.");
                return;
            }

            if (!string.IsNullOrEmpty(txtSenha.Text) &&
                (string.IsNullOrWhiteSpace(txtSenha.Text) ||
                 txtSenha.Text.Length < 6))
            {
                MessageBox.Show(
                    "A nova senha deve possuir pelo menos 6 caracteres válidos."
                );

                return;
            }

            btnAtualizar.Enabled = false;

            try
            {
                var usuario = new UsuarioUpdateDTO
                {
                    Nome = txtNome.Text.Trim(),

                    Senha = string.IsNullOrEmpty(txtSenha.Text)
                        ? null
                        : txtSenha.Text,

                    Status = chkStatus.Checked
                };

                using var response =
                    await ApiService.Client.PutAsJsonAsync(
                        $"api/Usuarios/{_idUsuarioSelecionado}",
                        usuario
                    );

                if (!response.IsSuccessStatusCode)
                {
                    await ExibirErroApiAsync(response);
                    return;
                }

                MessageBox.Show(
                    "Usuário atualizado com sucesso!",
                    "Atualização",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LimparCampos();

                await CarregarUsuariosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao atualizar usuário: {ex.Message}"
                );
            }
            finally
            {
                btnAtualizar.Enabled =
                    _idUsuarioSelecionado != null;
            }
        }

        private async void btnExcluir_Click(
            object sender, EventArgs e)
        {
            if (_idUsuarioSelecionado == null)
            {
                MessageBox.Show("Selecione um usuário.");
                return;
            }

            var confirmacao = MessageBox.Show(
                $"Deseja realmente excluir o usuário {txtNome.Text}?",
                "Confirmar exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacao != DialogResult.Yes)
                return;

            btnExcluir.Enabled = false;

            try
            {
                using var response =
                    await ApiService.Client.DeleteAsync(
                        $"api/Usuarios/{_idUsuarioSelecionado}"
                    );

                if (!response.IsSuccessStatusCode)
                {
                    await ExibirErroApiAsync(response);
                    return;
                }

                MessageBox.Show(
                    "Usuário excluído com sucesso!",
                    "Exclusão",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LimparCampos();

                await CarregarUsuariosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao excluir usuário: {ex.Message}"
                );
            }
            finally
            {
                btnExcluir.Enabled =
                    _idUsuarioSelecionado != null;
            }
        }

        private void btnLimpar_Click(
            object sender, EventArgs e)
        {
            LimparCampos();
        }
    }
}
