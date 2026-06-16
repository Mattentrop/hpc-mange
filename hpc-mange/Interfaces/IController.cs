using System.Collections.Generic;

namespace hpc_mange.Interfaces // ou o namespace que estiver a usar
{
    public interface IController<T>
    {
        void Salvar(T obj);
        void Inserir(T obj);
        void Atualizar(T obj);
        void Excluir(int id);
        List<T> CarregarDados();
    }
}