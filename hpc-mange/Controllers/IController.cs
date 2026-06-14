using System.Collections.Generic;

namespace hpc_mange.Controllers
{
    public interface IController<T>
    {
        void Cadastrar(T obj);
        void Atualizar(T obj);
        void Deletar(int id);
        List<T> CarregarDados();
    }
}