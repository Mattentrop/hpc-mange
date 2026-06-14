using System;
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

        public void Cadastrar(Cluster obj)
        {
            // O Controller apenas repassa o objeto para o Service trabalhar
            _service.Salvar(obj);
        }

        public List<Cluster> CarregarDados()
        {
            return _service.Listar();
        }

        // Os outros métodos ficam vazios por enquanto
        public void Atualizar(Cluster obj) { throw new NotImplementedException(); }
        public void Deletar(int id) { throw new NotImplementedException(); }
    }
}