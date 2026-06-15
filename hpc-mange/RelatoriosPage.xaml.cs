using hpc_mange.Controllers;
using hpc_mange.Models;
using hpc_mange.Relatorios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using MongoDB.Driver;

namespace hpc_mange
{
    public partial class RelatoriosPage : ContentPage
    {
        private ClusterController _clusterController;

        public RelatoriosPage()
        {
            InitializeComponent();
            _clusterController = new ClusterController();
        }

        // ---------------- 1. IMPORTAÇÃO JSON -> MYSQL ---------------- //
        private async void OnImportarJsonClicked(object sender, EventArgs e)
        {
            try
            {
                var opcoes = new PickOptions
                {
                    PickerTitle = "Selecione o ficheiro de clusters (.json)",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".json" } },
                        { DevicePlatform.Android, new[] { "application/json" } }
                    })
                };

                var ficheiro = await FilePicker.Default.PickAsync(opcoes);
                if (ficheiro == null) return;

                using (var stream = await ficheiro.OpenReadAsync())
                using (var reader = new StreamReader(stream))
                {
                    string conteudoJson = await reader.ReadToEndAsync();
                    var listaClusters = JsonSerializer.Deserialize<List<Cluster>>(conteudoJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (listaClusters != null && listaClusters.Count > 0)
                    {
                        int importados = 0;
                        foreach (var cluster in listaClusters)
                        {
                            _clusterController.Salvar(cluster);
                            importados++;
                        }
                        await DisplayAlert("Importação Concluída", $"{importados} clusters foram registados com sucesso!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro na Importação", ex.Message, "OK");
            }
        }

        // ---------------- 2. EXPORTAÇÃO MONGODB -> XML ---------------- //
        private async void OnExportarXmlClicked(object sender, EventArgs e)
        {
            try
            {
                string mongoConnectionString = "mongodb://admin_mongo:senha_mongo@192.168.122.1:27017/?authSource=admin";
                var client = new MongoClient(mongoConnectionString);
                var db = client.GetDatabase("hpc_audit");
                var collection = db.GetCollection<LogAuditoria>("logs");

                List<LogAuditoria> listaLogs = await collection.Find(_ => true).ToListAsync();

                if (listaLogs == null || listaLogs.Count == 0)
                {
                    await DisplayAlert("Aviso", "Nenhum log encontrado no MongoDB.", "OK");
                    return;
                }

                XElement xmlEstrutura = new XElement("Auditoria",
                    new XElement("Registos",
                        listaLogs.ConvertAll(log => new XElement("Log",
                            new XElement("Id", log.Id.ToString()),
                            new XElement("Operacao", log.Operacao),
                            new XElement("DataHora", log.DataHora.ToString("yyyy-MM-dd HH:mm:ss")),
                            new XElement("Detalhes", log.Detalhes)
                        ))
                    )
                );

                string caminhoTemporario = Path.Combine(FileSystem.CacheDirectory, "audit_report.xml");
                xmlEstrutura.Save(caminhoTemporario);

                await Share.Default.RequestAsync(new ShareFileRequest { Title = "Exportar XML", File = new ShareFile(caminhoTemporario) });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro na Exportação", ex.Message, "OK");
            }
        }

        // ---------------- 3. EXPORTAÇÃO PDF ---------------- //
        private async void OnExportarPdfClicked(object sender, EventArgs e)
        {
            try
            {
                var clusters = _clusterController.CarregarDados();
                string caminho = Path.Combine(FileSystem.CacheDirectory, "RelatorioHPC.pdf");

                // Gera o PDF usando o serviço
                PdfService.GerarRelatorio(clusters, caminho);

                // Abre o PDF diretamente no leitor padrão (sem partilhar)
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(caminho)
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível abrir o PDF: {ex.Message}", "OK");
            }
        }
    }

    public class LogAuditoria
    {
        public object Id { get; set; }
        public string Operacao { get; set; }
        public DateTime DataHora { get; set; }
        public string Detalhes { get; set; }
    }
}