using hpc_mange.Controllers; // <- Mudamos isso!
using hpc_mange.Models;
using hpc_mange.Services;
using System;
using System.Linq;

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

                // 1. Salva no MySQL via MVC (Regra de Negócio principal)
                ClusterController controller = new ClusterController();
                controller.Cadastrar(novoCluster);

                // 2. Grava o log de auditoria no MongoDB (Registro de atividade)
                AuditService auditoria = new AuditService();
                auditoria.RegistrarLog("NOVO_CADASTRO", $"Cluster '{novoCluster.Nome}' adicionado na {novoCluster.Localizacao}.");

                await DisplayAlert("Sucesso", "Cluster salvo no MySQL e log gravado no MongoDB!", "OK");

                EntryNome.Text = "";
                EntryLocalizacao.Text = "";
                EntryCapacidade.Text = "";
            }
            catch (Exception ex)
            {
                // Se der erro no banco, gravamos o erro no MongoDB!
                AuditService auditoriaErro = new AuditService();
                auditoriaErro.RegistrarLog("ERRO_CADASTRO", $"Falha ao tentar cadastrar: {ex.Message}");

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