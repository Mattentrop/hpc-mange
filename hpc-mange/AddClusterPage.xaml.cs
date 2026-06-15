using hpc_mange.Controllers;
using hpc_mange.Models;
using System;

namespace hpc_mange
{
    public partial class AddClusterPage : ContentPage
    {
        private ClusterController _controller;

        public AddClusterPage()
        {
            InitializeComponent();
            _controller = new ClusterController();
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            try
            {
                Cluster novoCluster = new Cluster
                {
                    Nome = EntryNome.Text,
                    Localizacao = EntryLocalizacao.Text,
                    CapacidadeRede = EntryCapacidade.Text
                };

                _controller.Salvar(novoCluster);
                await DisplayAlert("Sucesso", "Cluster cadastrado!", "OK");

                // Fecha esta tela e volta para a lista
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}