using System;
using System.Collections.Generic;
using hpc_mange.Models;
using hpc_mange.DAO;

namespace hpc_mange.Services
{
    public class ClusterService : IService<Cluster>
    {
        private ClusterDAO _dao;

        public ClusterService()
        {
            _dao = new ClusterDAO();
        }

        public void Salvar(Cluster obj)
        {
            // O Service faz a ponte e pode aplicar regras (ex: validar se o nome não é só espaço)
            if (string.IsNullOrWhiteSpace(obj.Nome))
            {
                throw new Exception("O nome do cluster não pode estar vazio.");
            }

            _dao.Inserir(obj);
        }

        public List<Cluster> Listar()
        {
            return _dao.BuscarTodos();
        }

        // Os outros métodos ficam vazios por enquanto
        public void Remover(int id) { throw new NotImplementedException(); }
        public Cluster Obter(int id) { throw new NotImplementedException(); }
    }
}