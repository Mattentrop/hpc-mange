using hpc_mange.DAO;
using hpc_mange.Models;
using System;

namespace hpc_mange
{
    public partial class GestaoAuxiliarPage : ContentPage
    {
        private PesquisadorDAO _pesquisadorDAO;
        private SoftwareDAO _softwareDAO;

        public GestaoAuxiliarPage()
        {
            InitializeComponent();
            _pesquisadorDAO = new PesquisadorDAO();
            _softwareDAO = new SoftwareDAO();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarListas();
        }

        private void CarregarListas()
        {
            ListaPesquisadores.ItemsSource = _pesquisadorDAO.BuscarTodos();
            ListaSoftwares.ItemsSource = _softwareDAO.BuscarTodos();
        }

        private async void OnAdicionarPesquisadorClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddPesquisadorPage());
        }

        private async void OnDeletarPesquisadorClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is Pesquisador pesq)
            {
                if (await DisplayAlert("Excluir", $"Remover {pesq.Nome}?", "Sim", "Não"))
                {
                    _pesquisadorDAO.Excluir(pesq.Id);
                    hpc_mange.Services.AuditService.RegistrarLog("Exclusão", $"Pesquisador {pesq.Nome} removido.", "Sistema");
                    CarregarListas();
                }
            }
        }

        private async void OnAdicionarSoftwareClicked(object sender, EventArgs e)
        {
            string nome = await DisplayPromptAsync("Software", "Nome do módulo:");
            if (!string.IsNullOrWhiteSpace(nome))
            {
                string versao = await DisplayPromptAsync("Software", "Versão:");
                string licenca = await DisplayPromptAsync("Software", "Tipo de Licença:");

                _softwareDAO.Inserir(new Software { NomeModulo = nome, Versao = versao ?? "1.0", Licenca = licenca ?? "Open-Source" });
                hpc_mange.Services.AuditService.RegistrarLog("Criação", $"Software {nome} adicionado ao catálogo.", "Sistema");
                CarregarListas();
            }
        }

        private async void OnDeletarSoftwareClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is Software soft)
            {
                if (await DisplayAlert("Excluir", $"Remover {soft.NomeModulo}?", "Sim", "Não"))
                {
                    _softwareDAO.Excluir(soft.Id);
                    hpc_mange.Services.AuditService.RegistrarLog("Exclusão", $"Software {soft.NomeModulo} removido do catálogo.", "Sistema");
                    CarregarListas();
                }
            }
        }
    }
}