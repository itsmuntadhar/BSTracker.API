using System.Collections.Generic;
using BSTracker.Entities;

namespace BSTracker.Interfaces
{
    public interface IService<T> where T : Entity
    {
        bool ExplicitSaveChanges { get; set; }
        IEnumerable<T> Get(int offset);
        T Get(string id);
        void Add(T t);
        void Add(IEnumerable<T> ts);
        void Update(T t);
        void Update(IEnumerable<T> ts);
        void Remove(T t);
        void Remove(IEnumerable<T> ts);
        int SaveChanges();
    }
}
