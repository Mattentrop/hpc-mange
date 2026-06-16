using hpc_mange.Controllers;
using hpc_mange.Models;
using hpc_mange.DAO;
using System;
using System.Linq;

namespace hpc_mange
{
    public partial class EditClusterPage : ContentPage
    {
        private ClusterController _controller;
        private Cluster _clusterAtual;
        private NodoDAO _nodoDAO;
        private PesquisadorDAO _pesquisadorDAO;
        private int _clusterIdAtual;

        public EditClusterPage(Cluster clusterSelecionado)
        {
            InitializeComponent();
            _controller = new ClusterController();
            _nodoDAO = new NodoDAO();
            _pesquisadorDAO = new PesquisadorDAO();
            _clusterAtual = clusterSelecionado;
            _clusterIdAtual = clusterSelecionado.Id;

            EntryNome.Text = _clusterAtual.Nome;
            EntryLocalizacao.Text = _clusterAtual.Localizacao;
            EntryCapacidade.Text = _clusterAtual.CapacidadeRede;

            CarregarDropdownPesquisadores();
            CarregarNodos();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarNodos();
        }

        private void CarregarDropdownPesquisadores()
        {
            var pesquisadores = _pesquisadorDAO.BuscarTodos();
            PickerPesquisador.ItemsSource = pesquisadores;

            if (_clusterAtual.PesquisadorId.HasValue)
            {
                PickerPesquisador.SelectedItem = pesquisadores.FirstOrDefault(p => p.Id == _clusterAtual.PesquisadorId.Value);
            }
        }

        private void CarregarNodos()
        {
            try
            {
                var nodos = _nodoDAO.BuscarPorCluster(_clusterIdAtual);
                ListaNodos.ItemsSource = nodos;
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void OnAdicionarNodoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddNodoPage(_clusterIdAtual));
        }

        private async void OnDeletarNodoClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var nodo = button?.CommandParameter as Nodo;

            if (nodo != null)
            {
                bool confirma = await DisplayAlert("Atenção", $"Deseja remover o nodo '{nodo.Hostname}'?", "Sim", "Não");
                if (confirma)
                {
                    try
                    {
                        _nodoDAO.Excluir(nodo.Id);
                        hpc_mange.Services.AuditService.RegistrarLog("Exclusão", $"Nodo {nodo.Hostname} removido do cluster.", "Sistema");
                        CarregarNodos();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Erro", ex.Message, "OK");
                    }
                }
            }
        }

        private async void OnAtualizarClicked(object sender, EventArgs e)
        {
            try
            {
                _clusterAtual.Nome = EntryNome.Text;
                _clusterAtual.Localizacao = EntryLocalizacao.Text;
                _clusterAtual.CapacidadeRede = EntryCapacidade.Text;

                var pesquisadorSelecionado = PickerPesquisador.SelectedItem as Pesquisador;
                _clusterAtual.PesquisadorId = pesquisadorSelecionado?.Id;

                _controller.Atualizar(_clusterAtual);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void OnExcluirClicked(object sender, EventArgs e)
        {
            bool confirma = await DisplayAlert("Atenção Crítica", $"Deseja realmente EXCLUIR '{_clusterAtual.Nome}'?", "Sim", "Não");
            if (confirma)
            {
                try
                {
                    _controller.Excluir(_clusterAtual.Id);
                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", ex.Message, "OK");
                }
            }
        }
    }
}