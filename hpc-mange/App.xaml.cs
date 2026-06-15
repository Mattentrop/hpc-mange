using hpc_mange.Services;

namespace hpc_mange
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Verifica no banco offline se já existe alguém logado
            SQLiteService sqlite = new SQLiteService();
            string usuario = sqlite.LerConfiguracao("UsuarioLogado");

            // Se encontrou o e-mail no SQLite, pula o login e vai pro Menu!
            if (usuario != "Nenhuma configuração encontrada")
            {
                MainPage = new AppShell();
            }
            else
            {
                // Se não tem ninguém logado, abre a tela de Login
                MainPage = new LoginPage();
            }
        }
    }
}