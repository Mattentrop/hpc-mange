using System.Collections.Generic;

namespace hpc_mange.Services
{
    public interface IService<T>
    {
        void Salvar(T obj);
        void Remover(int id);
        T Obter(int id);
        List<T> Listar();
    }
}