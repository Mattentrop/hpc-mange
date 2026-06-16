using hpc_mange.DAO;
using hpc_mange.Models;
using System;

namespace hpc_mange
{
    public partial class AddPesquisadorPage : ContentPage
    {
        private PesquisadorDAO _pesquisadorDAO;
        private UsuarioDAO _usuarioDAO;

        public AddPesquisadorPage()
        {
            InitializeComponent();
            _pesquisadorDAO = new PesquisadorDAO();
            _usuarioDAO = new UsuarioDAO();
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            try
            {
                var pesq = new Pesquisador
                {
                    Nome = EntryNome.Text,
                    Departamento = EntryDepto.Text,
                    Email = EntryEmail.Text
                };

                int pesquisadorId = _pesquisadorDAO.InserirRetornandoId(pesq);

                _usuarioDAO.InserirComPesquisador(EntryLogin.Text, EntrySenha.Text, pesquisadorId);

                hpc_mange.Services.AuditService.RegistrarLog("Criação", $"Pesquisador {EntryNome.Text} e Usuário {EntryLogin.Text} criados.", "Sistema");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}