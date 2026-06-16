using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using hpc_mange.Models;
using System.Collections.Generic;

// --- Apelidos para resolver o conflito ---
using QColors = QuestPDF.Helpers.Colors;
using QIContainer = QuestPDF.Infrastructure.IContainer;

namespace hpc_mange.Relatorios
{
    public class PdfService
    {
        public static void GerarRelatorio(List<Cluster> clusters, string caminho)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    // Como adicionámos mais uma coluna, se o texto ficar muito apertado, 
                    // você pode mudar a orientação da página com: page.PageOrientation(PageOrientation.Landscape);
                    page.Margin(1, Unit.Centimetre);

                    page.Header().Text("Relatório de Infraestrutura HPC")
                        .FontSize(20).SemiBold().FontColor(QColors.Blue.Medium);

                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30); // Coluna ID
                            columns.RelativeColumn();   // Coluna Cluster
                            columns.RelativeColumn();   // Coluna Localização
                            columns.RelativeColumn();   // Coluna Rede
                            columns.RelativeColumn();   // NOVO: Coluna Pesquisador
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("#");
                            header.Cell().Element(CellStyle).Text("Cluster");
                            header.Cell().Element(CellStyle).Text("Localização");
                            header.Cell().Element(CellStyle).Text("Rede");
                            header.Cell().Element(CellStyle).Text("Pesquisador"); // NOVO Cabeçalho

                            static QIContainer CellStyle(QIContainer container) =>
                                container.DefaultTextStyle(x => x.SemiBold())
                                         .PaddingVertical(5)
                                         .BorderBottom(1)
                                         .BorderColor(QColors.Black);
                        });

                        foreach (var item in clusters)
                        {
                            table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(item.Id.ToString());
                            table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(item.Nome);
                            table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(item.Localizacao);
                            table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(item.CapacidadeRede);

                      
                            table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5)
                                 .Text(item.NomePesquisador ?? "Não Atribuído");
                        }
                    });

                    page.Footer().AlignCenter().Text(x => { x.Span("Página "); x.CurrentPageNumber(); });
                });
            }).GeneratePdf(caminho);
        }
    }
}