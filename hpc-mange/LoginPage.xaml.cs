using hpc_mange.DAO;
using hpc_mange.Services;
using System;

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

            UsuarioDAO dao = new UsuarioDAO();
            bool credenciaisValidas = dao.ValidarLogin(email, senha);

            if (credenciaisValidas)
            {
                SQLiteService sqlite = new SQLiteService();
                sqlite.SalvarConfiguracao("UsuarioLogado", email);

                // Vai para o Menu após o login dar certo
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await DisplayAlert("Acesso Negado", "E-mail ou senha incorretos.", "OK");
            }
        }
    }
}