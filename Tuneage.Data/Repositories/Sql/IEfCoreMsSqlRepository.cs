using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tuneage.Data.Repositories.Sql
{
    public interface IEfCoreMsSqlRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> Get(int id);
        void SetModified(T entity);
        void Update(T entity);
        void Add(T entity);
        void Remove(T entity);
        Task<int> SaveChangesAsync();
        bool Any(int id);
    }
}
