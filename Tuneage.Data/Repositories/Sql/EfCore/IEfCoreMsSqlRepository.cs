using System.Linq;
using System.Threading.Tasks;
using Tuneage.Data.Orm.EF.DataContexts;

namespace Tuneage.Data.Repositories.Sql.EfCore
{
    public interface IEfCoreMsSqlRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetById(int id);
        Task Create(TEntity entity);
        Task Update(int id, TEntity entity);
        Task Delete(int id);
        void SetModified(TEntity entity);
        Task<int> SaveChangesAsync();
        bool Any(int id);
    }

    public class EfCoreMsSqlRepository<TEntity> : IEfCoreMsSqlRepository<TEntity> where TEntity : class
    {
        private readonly TuneageDataContext _dbContext;

        public EfCoreMsSqlRepository(TuneageDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task Create(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Update(int id, TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual void SetModified(TEntity entity)
        {
            _dbContext.SetModified(entity);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            var entity = _dbContext.Set<TEntity>().Find(id);

            return entity != null;
        }
    }
}
