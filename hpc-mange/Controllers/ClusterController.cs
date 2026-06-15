using System.Collections.Generic;
using hpc_mange.Models;
using hpc_mange.Services;

namespace hpc_mange.Controllers
{
    public class ClusterController : IController<Cluster>
    {
        private ClusterService _service;

        public ClusterController()
        {
            _service = new ClusterService();
        }

        public void Salvar(Cluster obj)
        {
            _service.Salvar(obj);
        }

        public List<Cluster> CarregarDados()
        {
            return _service.CarregarDados();
        }

        public void Atualizar(Cluster obj)
        {
            _service.Atualizar(obj);
        }

        public void Excluir(int id)
        {
            _service.Excluir(id);
        }

        public List<Cluster> BuscarPorNome(string termo)
        {
            return _service.BuscarPorNome(termo);
        }
    }
}