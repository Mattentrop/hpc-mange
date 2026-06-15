using System.Collections.Generic;

namespace hpc_mange.Services
{
    public interface IService<T>
    {
        void Salvar(T obj);
        void Atualizar(T obj);
        void Excluir(int id);
        List<T> CarregarDados();
        List<T> BuscarPorNome(string termo);
    }
}