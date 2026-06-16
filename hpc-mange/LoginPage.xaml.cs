using hpc_mange.DAO;
using hpc_mange.Services;
using System;
using Microsoft.Maui.Controls;

namespace hpc_mange
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = EntryLogin.Text;
            string senha = EntrySenha.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                await DisplayAlert("Aviso", "Por favor, preencha o e-mail e a senha.", "OK");
                return;
            }

            try
            {
                UsuarioDAO dao = new UsuarioDAO();
                bool credenciaisValidas = dao.ValidarLogin(email, senha);

                if (credenciaisValidas)
                {
                    SQLiteService sqlite = new SQLiteService();
                    sqlite.SalvarConfiguracao("UsuarioLogado", email);

                    AuditService.RegistrarLog("Login", "Acesso autenticado com sucesso.", email);

                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    AuditService.RegistrarLog("Acesso Negado", "Tentativa de login com credenciais inválidas.", email);

                    await DisplayAlert("Acesso Negado", "E-mail ou senha incorretos.", "OK");
                }
            }
            catch (Exception ex)
            {
                AuditService.RegistrarLog("Erro de Sistema", $"Falha no processo de login: {ex.Message}", email ?? "Desconhecido");

                await DisplayAlert("Erro", "Ocorreu um erro de conexão: " + ex.Message, "OK");
            }
        }
    }
}