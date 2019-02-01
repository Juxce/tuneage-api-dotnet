using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Constants;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Domain.Entities;

namespace Tuneage.Data.Repositories.Sql.EfCore
{
    public interface IArtistRepository : IEfCoreMsSqlRepository<Artist>
    {
        Task<List<Artist>> GetAllAlphabetical();
    }

    public class ArtistRepository : EfCoreMsSqlRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(TuneageDataContext dbContext) : base(dbContext)
        {
        }

        public virtual async Task<List<Artist>> GetAllAlphabetical()
        {
            return await GetAll().OrderBy(a => a.Name).ToListAsync();
        }

        public override async Task Create(Artist entity)
        {
            switch (entity.GetType().ToString())
            {
                case ArtistTypes.SoloArtist:
                    entity.IsBand = false;
                    entity.IsPrinciple = true;
                    break;
                case ArtistTypes.Band:
                    entity.IsBand = true;
                    entity.IsPrinciple = true;
                    break;
                //case ArtistTypes.AliasedArtist:
                //    entity.IsPrinciple = false;
                //    break;
            }

            await DbContext.Set<Artist>().AddAsync(entity);
            await SaveChangesAsync();
        }
    }
}
