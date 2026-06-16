using System;
using System.Collections.Generic;
using hpc_mange.Models;
using hpc_mange.DAO;
using hpc_mange.Interfaces;
using hpc_mange.Services;
namespace hpc_mange.Controllers
{
    public class ClusterController
    {
        private ClusterDAO _clusterDAO;
        private string _usuarioAtual = "Sistema";
        public ClusterController()
        {
            _clusterDAO = new ClusterDAO();
        }
        private void IdentificarUsuario(Cluster cluster = null)
        {
            try
            {
                var sqliteService = new SQLiteService();
                string emailLogado = sqliteService.LerConfiguracao("UsuarioLogado");
                if (!string.IsNullOrWhiteSpace(emailLogado) && emailLogado != "Nenhuma configuração encontrada")
                {
                    _usuarioAtual = emailLogado;
                    if (cluster != null)
                    {
                        var usuarioDAO = new UsuarioDAO();
                        var usuarioDb = usuarioDAO.BuscarPorEmail(emailLogado);
                        if (usuarioDb != null)
                        {
                            cluster.CreatedBy = usuarioDb.Id;
                            cluster.PesquisadorId = usuarioDb.PesquisadorId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Aviso: Falha ao identificar usuário local. " + ex.Message);
            }
        }
        public void Salvar(Cluster cluster)
        {
            if (cluster.Id == 0)
                Inserir(cluster);
            else
                Atualizar(cluster);
        }
        public void Inserir(Cluster cluster)
        {
            try
            {
                IdentificarUsuario(cluster);
                _clusterDAO.Inserir(cluster);
                AuditService.RegistrarLog("Inclusão de Registro", $"Novo cluster '{cluster.Nome}' cadastrado.", _usuarioAtual);
            }
            catch (Exception ex)
            {
                AuditService.RegistrarLog("Erro de Sistema", $"Falha ao inserir cluster: {ex.Message}", _usuarioAtual);
                throw;
            }
        }
        public void Update(Cluster cluster)
        {
            Atualizar(cluster);
        }
        public void Atualizar(Cluster cluster)
        {
            try
            {
                IdentificarUsuario(cluster);
                _clusterDAO.Atualizar(cluster);
                AuditService.RegistrarLog("Alteração de Dados", $"Cluster '{cluster.Nome}' (ID: {cluster.Id}) foi atualizado.", _usuarioAtual);
            }
            catch (Exception ex)
            {
                AuditService.RegistrarLog("Erro de Sistema", $"Falha ao atualizar cluster: {ex.Message}", _usuarioAtual);
                throw;
            }
        }
        public void Excluir(int id)
        {
            try
            {
                IdentificarUsuario();
                _clusterDAO.Excluir(id);
                AuditService.RegistrarLog("Exclusão de Dados", $"Cluster (ID: {id}) foi movido para a lixeira (Soft Delete).", _usuarioAtual);
            }
            catch (Exception ex)
            {
                AuditService.RegistrarLog("Erro de Sistema", $"Falha ao excluir cluster: {ex.Message}", _usuarioAtual);
                throw;
            }
        }
        public List<Cluster> CarregarDados()
        {
            return _clusterDAO.BuscarTodos();
        }
        public List<Cluster> BuscarPorNome(string termo)
        {
            return _clusterDAO.BuscarPorNome(termo);
        }
        public List<Cluster> BuscarComFiltro(string termoNome, string capacidade)
        {
            return _clusterDAO.BuscarComFiltro(termoNome, capacidade);
        }
        public List<string> BuscarCapacidadesDistintas()
        {
            return _clusterDAO.BuscarCapacidadesDistintas();
        }
    }
}