using System.Collections.Generic;

namespace hpc_mange.Interfaces
{
    public interface IController<T>
    {
        void Salvar(T obj);
        void Atualizar(T obj);
        void Excluir(int id);
        List<T> CarregarDados();
        List<T> BuscarPorNome(string termo);
    }
}