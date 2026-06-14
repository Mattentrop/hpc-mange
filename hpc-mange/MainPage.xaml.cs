using hpc_mange.Controllers; // <- Mudamos isso!
using hpc_mange.Models;
using System.Linq;
using System;

namespace hpc_mange
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryNome.Text) ||
                string.IsNullOrWhiteSpace(EntryLocalizacao.Text) ||
                string.IsNullOrWhiteSpace(EntryCapacidade.Text))
            {
                await DisplayAlert("Aviso", "Preencha todos os campos antes de salvar!", "OK");
                return;
            }

            try
            {
                Cluster novoCluster = new Cluster
                {
                    Nome = EntryNome.Text,
                    Localizacao = EntryLocalizacao.Text,
                    CapacidadeRede = EntryCapacidade.Text
                };

                // Agora chamamos o CONTROLLER, respeitando o MVC!
                ClusterController controller = new ClusterController();
                controller.Cadastrar(novoCluster);

                await DisplayAlert("Sucesso", "Cluster cadastrado com sucesso via MVC!", "OK");

                EntryNome.Text = "";
                EntryLocalizacao.Text = "";
                EntryCapacidade.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar:\n{ex.Message}", "OK");
            }
        }

        private async void OnListarClicked(object sender, EventArgs e)
        {
            try
            {
                // Aqui também chamamos o CONTROLLER!
                ClusterController controller = new ClusterController();
                var clusters = controller.CarregarDados();

                if (clusters.Count > 0)
                {
                    string nomes = string.Join("\n- ", clusters.Select(c => c.Nome));
                    await DisplayAlert($"Total: {clusters.Count} Clusters", $"\n- {nomes}", "OK");
                }
                else
                {
                    await DisplayAlert("Aviso", "Nenhum cluster encontrado no banco.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao buscar:\n{ex.Message}", "OK");
            }
        }
    }
}