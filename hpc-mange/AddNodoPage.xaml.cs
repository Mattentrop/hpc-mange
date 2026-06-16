using hpc_mange.DAO;
using hpc_mange.Models;
using System;
using System.Linq;

namespace hpc_mange
{
    public partial class AddNodoPage : ContentPage
    {
        private int _clusterId;
        private SoftwareDAO _softwareDAO;
        private NodoDAO _nodoDAO;

        public AddNodoPage(int clusterId)
        {
            InitializeComponent();
            _clusterId = clusterId;
            _softwareDAO = new SoftwareDAO();
            _nodoDAO = new NodoDAO();

            CarregarSoftwares();
        }

        private void CarregarSoftwares()
        {
            ListaSoftwares.ItemsSource = _softwareDAO.BuscarTodos();
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            try
            {
                int ram = 0;
                int.TryParse(EntryRAM.Text, out ram);

                var novoNodo = new Nodo
                {
                    Hostname = EntryHostname.Text,
                    ModeloCPU = EntryCPU.Text,
                    MemoriaRAM = ram,
                    ClusterId = _clusterId
                };

                int novoNodoId = _nodoDAO.InserirRetornandoId(novoNodo);

                var softwaresSelecionados = ListaSoftwares.SelectedItems.Cast<Software>().ToList();
                foreach (var soft in softwaresSelecionados)
                {
                    _nodoDAO.VincularSoftware(novoNodoId, soft.Id);
                }

                hpc_mange.Services.AuditService.RegistrarLog("Criação", $"Nodo {EntryHostname.Text} adicionado com softwares vinculados.", "Sistema");

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}