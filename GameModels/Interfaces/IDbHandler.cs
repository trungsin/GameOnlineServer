using System.Collections.Generic;

namespace GameOnlineServer.GameModels.Interfaces
{
    public interface IDbHandler<T>
    {
        T Find(string id);
        List<T> FindAll();
        T Create(T item);
        T Update(string id, T item);
        void Remove(string id);
    }
}