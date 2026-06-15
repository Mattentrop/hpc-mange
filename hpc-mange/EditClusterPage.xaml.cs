using hpc_mange.Controllers;
using hpc_mange.Models;
using System;

namespace hpc_mange
{
    public partial class EditClusterPage : ContentPage
    {
        private ClusterController _controller;
        private Cluster _clusterAtual;

        // O construtor agora exige que um cluster seja passado para ele
        public EditClusterPage(Cluster clusterSelecionado)
        {
            InitializeComponent();
            _controller = new ClusterController();
            _clusterAtual = clusterSelecionado;

            // Preenche os campos com os dados atuais
            EntryNome.Text = _clusterAtual.Nome;
            EntryLocalizacao.Text = _clusterAtual.Localizacao;
            EntryCapacidade.Text = _clusterAtual.CapacidadeRede;
        }

        private async void OnAtualizarClicked(object sender, EventArgs e)
        {
            try
            {
                _clusterAtual.Nome = EntryNome.Text;
                _clusterAtual.Localizacao = EntryLocalizacao.Text;
                _clusterAtual.CapacidadeRede = EntryCapacidade.Text;

                _controller.Atualizar(_clusterAtual);
                await DisplayAlert("Sucesso", "Cluster atualizado!", "OK");
                await Navigation.PopAsync(); // Volta para a lista
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
                    await Navigation.PopAsync(); // Volta para a lista
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", $"Falha ao excluir: {ex.Message}", "OK");
                }
            }
        }
    }
}