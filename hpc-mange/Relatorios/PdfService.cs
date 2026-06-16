using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using hpc_mange.Models;
using hpc_mange.DAO;
using System;
using System.Collections.Generic;
using System.Linq;

using QColors = QuestPDF.Helpers.Colors;
using QIContainer = QuestPDF.Infrastructure.IContainer;

namespace hpc_mange.Relatorios
{
    public class PdfService
    {
        public static void GerarRelatorio(List<Cluster> clusters, string caminho)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var nodoDAO = new NodoDAO();
            var softwareDAO = new SoftwareDAO();

            int totalClusters = clusters.Count;
            int totalNodosGeral = 0;
            var nodosParaSegundaPagina = new List<Nodo>();

            foreach (var c in clusters)
            {
                var nodosDoCluster = nodoDAO.BuscarPorCluster(c.Id);
                totalNodosGeral += nodosDoCluster.Count;
                nodosParaSegundaPagina.AddRange(nodosDoCluster);
            }

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);

                    page.Header().Text("Relatório de Infraestrutura HPC")
                        .FontSize(20).SemiBold().FontColor(QColors.Blue.Medium);

                    page.Content().Column(column =>
                    {
                        column.Item().PaddingTop(5).Text($"Data de Geração: {DateTime.Now:dd/MM/yyyy HH:mm}")
                              .FontSize(12).FontColor(QColors.Grey.Darken2);

                        column.Item().PaddingBottom(15).Text($"Estatísticas: {totalClusters} Clusters Cadastrados | {totalNodosGeral} Nodos Ativos")
                              .FontSize(12).SemiBold().FontColor(QColors.Black);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn(1.5f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("#");
                                header.Cell().Element(CellStyle).Text("Cluster");
                                header.Cell().Element(CellStyle).Text("Localização");
                                header.Cell().Element(CellStyle).Text("Rede");
                                header.Cell().Element(CellStyle).Text("Pesquisador");
                                header.Cell().Element(CellStyle).Text("Nodos Associados");

                                static QIContainer CellStyle(QIContainer container) =>
                                    container.DefaultTextStyle(x => x.SemiBold())
                                             .PaddingVertical(5)
                                             .BorderBottom(1)
                                             .BorderColor(QColors.Black);
                            });

                            foreach (var item in clusters)
                            {
                                var nodos = nodoDAO.BuscarPorCluster(item.Id);
                                string nomesNodos = nodos.Count > 0 ? string.Join(", ", nodos.Select(n => n.Hostname)) : "Nenhum Nodo";

                                table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(item.Id.ToString());
                                table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(item.Nome);
                                table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(item.Localizacao);
                                table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(item.CapacidadeRede);
                                table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(item.NomePesquisador ?? "Não Atribuído");
                                table.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(nomesNodos);
                            }
                        });

                        column.Item().PageBreak();

                        column.Item().PaddingBottom(15).Text("Relatório de Softwares por Nodo")
                              .FontSize(18).SemiBold().FontColor(QColors.Blue.Medium);

                        column.Item().Table(table2 =>
                        {
                            table2.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            table2.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Hostname do Nodo");
                                header.Cell().Element(CellStyle).Text("Especificações");
                                header.Cell().Element(CellStyle).Text("Softwares Instalados");

                                static QIContainer CellStyle(QIContainer container) =>
                                    container.DefaultTextStyle(x => x.SemiBold())
                                             .PaddingVertical(5)
                                             .BorderBottom(1)
                                             .BorderColor(QColors.Black);
                            });

                            foreach (var nodo in nodosParaSegundaPagina)
                            {
                                var softwares = softwareDAO.BuscarPorNodo(nodo.Id);
                                string listaSoftwares = softwares.Count > 0
                                    ? string.Join("\n", softwares.Select(s => $"- {s.NomeModulo} (v{s.Versao})"))
                                    : "Nenhum software vinculado";

                                table2.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(nodo.Hostname);
                                table2.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text($"{nodo.ModeloCPU}\n{nodo.MemoriaRAM}GB RAM");
                                table2.Cell().BorderBottom(1).BorderColor(QColors.Grey.Lighten2).Padding(5).Text(listaSoftwares);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(x => { x.Span("Página "); x.CurrentPageNumber(); });
                });
            }).GeneratePdf(caminho);
        }
    }
}