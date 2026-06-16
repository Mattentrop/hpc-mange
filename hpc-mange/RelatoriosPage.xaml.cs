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
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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

        // --- NOVO MÉTODO: EXPORTAR JSON ---
        private async void OnExportarJsonClicked(object sender, EventArgs e)
        {
            try
            {
                var listaClusters = _clusterController.CarregarDados();

                if (listaClusters == null || listaClusters.Count == 0)
                {
                    await DisplayAlert("Aviso", "Não há clusters cadastrados para exportar.", "OK");
                    return;
                }

                var opcoes = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(listaClusters, opcoes);

                string arquivoDestino = Path.Combine(FileSystem.CacheDirectory, "hpc_backup.json");
                File.WriteAllText(arquivoDestino, jsonString);

                // Auditoria
                var sqlite = new hpc_mange.Services.SQLiteService();
                string emailLogado = sqlite.LerConfiguracao("UsuarioLogado");
                string usuarioLog = (!string.IsNullOrWhiteSpace(emailLogado) && emailLogado != "Nenhuma configuração encontrada") ? emailLogado : "Sistema";

                hpc_mange.Services.AuditService.RegistrarLog("Exportação JSON", "Backup dos clusters gerado pelo utilizador.", usuarioLog);

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    Title = "Exportação JSON HPC",
                    File = new ReadOnlyFile(arquivoDestino)
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro na Exportação", ex.Message, "OK");
            }
        }

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
                            new XElement("Id", log.Id?.ToString() ?? "N/A"),
                            new XElement("Operacao", log.Operacao ?? "Desconhecida"),
                            new XElement("DataHora", log.DataHora != DateTime.MinValue ? log.DataHora.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                            new XElement("Detalhes", log.Detalhes ?? "Sem detalhes"),
                            new XElement("UsuarioResponsavel", log.UsuarioResponsavel ?? "Sistema")
                        ))
                    )
                );

                string caminhoTemporario = Path.Combine(FileSystem.CacheDirectory, "audit_report.xml");
                xmlEstrutura.Save(caminhoTemporario);

                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(caminhoTemporario)
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro na Exportação", ex.Message, "OK");
            }
        }

        private async void OnExportarPdfClicked(object sender, EventArgs e)
        {
            try
            {
                var clusters = _clusterController.CarregarDados();
                string caminho = Path.Combine(FileSystem.CacheDirectory, "RelatorioHPC.pdf");

                PdfService.GerarRelatorio(clusters, caminho);

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

    // CLASSE DE LOG CORRIGIDA PARA LER E GRAVAR OS IDS CORRETAMENTE NO MONGO
    public class LogAuditoria
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Operacao { get; set; }
        public DateTime DataHora { get; set; }
        public string Detalhes { get; set; }
        public string UsuarioResponsavel { get; set; }
    }
}