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
            var releases = await GetAll().Include(r => r.Label).Include(r => r.Artist).OrderBy(r => r.Title).ToListAsync();
            foreach (var vaRelease in releases.Where(r => r.IsByVariousArtists))
            {
                vaRelease.Artist = new Artist { Name = DefaultValues.VariousArtistsDisplayName };
            }

            return releases;
        }

        public override async Task<Release> GetById(int id)
        {
            return await DbContext.Releases.Include(r => r.Label).Include(r => r.Artist).FirstOrDefaultAsync(r => r.ReleaseId == id);
        }
    }
}
