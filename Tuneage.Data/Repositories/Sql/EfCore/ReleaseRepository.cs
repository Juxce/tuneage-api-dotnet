using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Constants;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Domain.Entities;

namespace Tuneage.Data.Repositories.Sql.EfCore
{
    public interface IReleaseRepository : IEfCoreMsSqlRepository<Release>
    {
        Task<List<Release>> GetAllAlphabetical();
    }

    public class ReleaseRepository : EfCoreMsSqlRepository<Release>, IReleaseRepository
    {
        public ReleaseRepository(TuneageDataContext dbContext) : base(dbContext)
        {
        }

        public virtual async Task<List<Release>> GetAllAlphabetical()
        {
            return await GetAll().OrderBy(r => r.Title).ToListAsync();
        }

        public override async Task Create(Release entity)
        {
            switch (entity.GetType().ToString())
            {
                case ReleaseTypes.SingleArtistRelease:
                    entity.IsByVariousArtists = false;
                    break;
                case ReleaseTypes.VariousArtistsRelease:
                    entity.IsByVariousArtists = true;
                    entity.ArtistId = null;
                    break;
            }

            await DbContext.Set<Release>().AddAsync(entity);
            await SaveChangesAsync();
        }
    }
}
