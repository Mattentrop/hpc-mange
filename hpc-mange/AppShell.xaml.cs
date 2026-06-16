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

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool aceitou = await DisplayAlert("Sair", "Deseja encerrar a sessão atual?", "Sim", "Não");

            if (aceitou)
            {
                try
                {
                    var sqlite = new hpc_mange.Services.SQLiteService();
                    string emailLogado = sqlite.LerConfiguracao("UsuarioLogado");

                    string usuarioLog = (!string.IsNullOrWhiteSpace(emailLogado) && emailLogado != "Nenhuma configuração encontrada")
                                        ? emailLogado
                                        : "Sistema";

                    // 1. Registra no Mongo antes de apagar a sessão local
                    hpc_mange.Services.AuditService.RegistrarLog("Logout", "Sessão encerrada pelo utilizador.", usuarioLog);

                    // 2. O Pulo do Gato: Salva o texto EXATO que o seu App.cs espera para barrar o acesso!
                    sqlite.SalvarConfiguracao("UsuarioLogado", "Nenhuma configuração encontrada");

                    // 3. Joga o usuário de volta para a tela de Login
                    Application.Current.MainPage = new LoginPage();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro no processo de logout: " + ex.Message);
                    Application.Current.MainPage = new LoginPage();
                }
            }
        }
    }
}