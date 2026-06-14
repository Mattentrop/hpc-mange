using System.Collections.Generic;

namespace hpc_mange.DAO
{
    public interface IDAO<T>
    {
        void Inserir(T obj);
        void Atualizar(T obj);
        void Excluir(int id);
        T BuscarPorId(int id);
        List<T> BuscarTodos();
    }
}