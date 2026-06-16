using hpc_mange.Controllers;
using hpc_mange.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hpc_mange
{
    public partial class MainPage : ContentPage
    {
        private ClusterController _controller;
        private const string OpcaoTodas = "Todas";

        public MainPage()
        {
            InitializeComponent();
            _controller = new ClusterController();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarFiltroCapacidades();
            AplicarFiltros();
        }

        private void CarregarFiltroCapacidades()
        {
            if (FiltroCapacidade.ItemsSource != null)
                return;

            var capacidades = new List<string> { OpcaoTodas };
            capacidades.AddRange(_controller.BuscarCapacidadesDistintas());

            FiltroCapacidade.ItemsSource = capacidades;
            FiltroCapacidade.SelectedIndex = 0;
        }

        private void AplicarFiltros()
        {
            string termo = BarraPesquisa?.Text;
            string capacidadeSelecionada = FiltroCapacidade.SelectedItem as string;

            string capacidade = (string.IsNullOrWhiteSpace(capacidadeSelecionada) || capacidadeSelecionada == OpcaoTodas)
                ? null
                : capacidadeSelecionada;

            ListaClusters.ItemsSource = _controller.BuscarComFiltro(termo, capacidade);
        }

        private void OnPesquisarClicked(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void OnFiltroAlterado(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private async void OnAdicionarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddClusterPage());
        }

        private async void OnGestaoAuxiliarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GestaoAuxiliarPage());
        }

        private async void OnClusterSelecionado(object sender, SelectionChangedEventArgs e)
        {
            var clusterClicado = e.CurrentSelection.FirstOrDefault() as Cluster;
            if (clusterClicado != null)
            {
                ListaClusters.SelectedItem = null;
                await Navigation.PushAsync(new EditClusterPage(clusterClicado));
            }
        }
    }
}