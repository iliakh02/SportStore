using SportStore.Models;
using System.Collections.Generic;

namespace SportStore.Data.Abstract
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Commit();
    }
}
