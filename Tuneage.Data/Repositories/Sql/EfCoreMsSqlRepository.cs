using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Orm.EF.DataContexts;

namespace Tuneage.Data.Repositories.Sql
{
    public class EfCoreMsSqlRepository<T> : IEfCoreMsSqlRepository<T> where T : class
    {
        private readonly TuneageDataContext _context;

        public EfCoreMsSqlRepository(TuneageDataContext context)
        {
            _context = context;
        }

        public Task<List<T>> GetAll()
        {
            return _context.Set<T>().ToListAsync();
        }

        public Task<T> Get(int id)
        {
            return _context.Set<T>().FindAsync(id);
        }

        public void SetModified(T entity)
        {
            _context.SetModified(entity);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void SaveChangesAsync()
        {
            _context.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            var entity = _context.Set<T>().Find(id);

            return entity != null;
        }
    }
}