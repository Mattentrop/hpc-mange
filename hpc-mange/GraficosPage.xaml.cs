using hpc_mange.Controllers;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace hpc_mange
{
    public partial class GraficosPage : ContentPage
    {
        private ClusterController _clusterController;

        public GraficosPage()
        {
            InitializeComponent();
            _clusterController = new ClusterController();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarGrafico();
        }

        private void OnAtualizarGraficoClicked(object sender, EventArgs e)
        {
            CarregarGrafico();
        }

        private void CarregarGrafico()
        {
            try
            {
                var clusters = _clusterController.CarregarDados();

                if (clusters == null || clusters.Count == 0)
                {
                    // Se não houver dados, não tenta desenhar para não quebrar
                    return;
                }

                // Nova Métrica: Agrupa os clusters pela Capacidade de Rede (ex: 10G, 100G)
                var clustersAgrupados = clusters
                    .GroupBy(c => string.IsNullOrWhiteSpace(c.CapacidadeRede) ? "Não Definida" : c.CapacidadeRede)
                    .Select(g => new { Categoria = g.Key, Quantidade = g.Count() })
                    .ToList();

                var entradasGrafico = new List<ChartEntry>();
                string[] coresHex = { "#E63946", "#4D82FF", "#2A9D8F", "#F4A261", "#E76F51" };
                int corIndex = 0;

                foreach (var item in clustersAgrupados)
                {
                    entradasGrafico.Add(new ChartEntry(item.Quantidade)
                    {
                        Label = item.Categoria,
                        ValueLabel = item.Quantidade.ToString(),
                        Color = SKColor.Parse(coresHex[corIndex % coresHex.Length]),
                        ValueLabelColor = SKColor.Parse(coresHex[corIndex % coresHex.Length])
                    });
                    corIndex++;
                }

                // Novo Visual: Gráfico de Rosca (Donut) fica muito mais moderno
                GraficoClusters.Chart = new DonutChart
                {
                    Entries = entradasGrafico,
                    BackgroundColor = SKColor.Parse("#111A42"),
                    LabelTextSize = 15f,
                    LabelMode = LabelMode.RightOnly, // Fica com uma legenda lateral bonita
                    HoleRadius = 0.5f // O tamanho do buraco no meio
                };
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", $"Falha ao gerar o gráfico: {ex.Message}", "OK");
            }
        }
    }
}