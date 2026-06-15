using hpc_mange.Controllers;
using hpc_mange.Models;
using System;
using System.Linq;

namespace hpc_mange
{
    public partial class MainPage : ContentPage
    {
        private ClusterController _controller;

        public MainPage()
        {
            InitializeComponent();
            _controller = new ClusterController();
        }

        // O SEGREDO ESTÁ AQUI: Isso garante que a lista atualize sempre que você voltar das outras telas
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarLista();
        }

        private void CarregarLista()
        {
            ListaClusters.ItemsSource = _controller.CarregarDados();
        }

        private void OnPesquisarClicked(object sender, EventArgs e)
        {
            string termo = BarraPesquisa.Text;
            if (string.IsNullOrWhiteSpace(termo))
                CarregarLista();
            else
                ListaClusters.ItemsSource = _controller.BuscarPorNome(termo);
        }

        private async void OnAdicionarClicked(object sender, EventArgs e)
        {
            // Abre a tela de adicionar por cima
            await Navigation.PushAsync(new AddClusterPage());
        }

        private async void OnClusterSelecionado(object sender, SelectionChangedEventArgs e)
        {
            var clusterClicado = e.CurrentSelection.FirstOrDefault() as Cluster;

            if (clusterClicado != null)
            {
                // Tira a seleção visual do item para poder clicar nele de novo depois
                ListaClusters.SelectedItem = null;

                // Passa o cluster clicado para a tela de edição
                await Navigation.PushAsync(new EditClusterPage(clusterClicado));
            }
        }
    }
}