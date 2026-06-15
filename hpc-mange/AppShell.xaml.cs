using hpc_mange.Services;
using System;

namespace hpc_mange
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            // Apaga a sessão do banco offline
            SQLiteService sqlite = new SQLiteService();
            sqlite.SalvarConfiguracao("UsuarioLogado", "Nenhuma configuração encontrada");

            // Joga o usuário de volta para a tela de Login
            Application.Current.MainPage = new LoginPage();
        }
    }
}