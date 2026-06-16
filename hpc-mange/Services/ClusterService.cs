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
            // Validações e Regras de Negócio
            if (string.IsNullOrWhiteSpace(obj.Nome) || string.IsNullOrWhiteSpace(obj.CapacidadeRede) || string.IsNullOrWhiteSpace(obj.Localizacao))
                throw new Exception("Todos os campos devem estar preenchidos.");
            if (obj.Nome.Contains(" "))
                throw new Exception("Regra de Negócio: O nome do cluster deve seguir o padrão de hostname (sem espaços em branco).");
            if (obj.Localizacao.Contains("Sala Cofre") && !obj.CapacidadeRede.Contains("Gbps"))
                throw new Exception("Regra de Negócio: Ambientes de 'Sala Cofre' não suportam conexões abaixo de Gbps.");
            if (obj.CapacidadeRede.Contains("400 Gbps") && obj.CapacidadeRede.Contains("Ethernet"))
                throw new Exception("Regra de Negócio: Para 400 Gbps ou superiores, é obrigatória a utilização de arquitetura Infiniband.");
            _dao.Inserir(obj);
        }
        public List<Cluster> CarregarDados()
        {
            return _dao.BuscarTodos();
        }
        public void Atualizar(Cluster obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Nome) || string.IsNullOrWhiteSpace(obj.CapacidadeRede) || string.IsNullOrWhiteSpace(obj.Localizacao))
                throw new Exception("Todos os campos devem estar preenchidos para atualizar.");
            _dao.Atualizar(obj);
        }
        public void Excluir(int id)
        {
            _dao.Excluir(id);
        }
        public List<Cluster> BuscarPorNome(string termo)
        {
            return _dao.BuscarPorNome(termo);
        }
        public List<Cluster> BuscarComFiltro(string termoNome, string capacidade)
        {
            return _dao.BuscarComFiltro(termoNome, capacidade);
        }
        public List<string> BuscarCapacidadesDistintas()
        {
            return _dao.BuscarCapacidadesDistintas();
        }
    }
}