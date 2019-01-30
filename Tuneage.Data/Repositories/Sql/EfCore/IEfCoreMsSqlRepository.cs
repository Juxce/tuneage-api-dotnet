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
        protected readonly TuneageDataContext DbContext;

        public EfCoreMsSqlRepository(TuneageDataContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task Create(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Update(int id, TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(int id)
        {
            var entity = await DbContext.Set<TEntity>().FindAsync(id);
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual void SetModified(TEntity entity)
        {
            DbContext.SetModified(entity);
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        public bool Any(int id)
        {
            var entity = DbContext.Set<TEntity>().Find(id);

            return entity != null;
        }
    }
}
