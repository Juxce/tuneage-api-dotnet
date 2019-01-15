using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Orm.EF.DataContexts;
using Tuneage.Domain.Entities;

namespace Tuneage.Data.Repositories.Sql.EfCore
{
    public interface ILabelRepository : IEfCoreMsSqlRepository<Label>
    {
        Task<List<Label>> GetAllAlphabetical();
    }

    public class LabelRepository : EfCoreMsSqlRepository<Label>, ILabelRepository
    {
        public LabelRepository(TuneageDataContext dbContext) : base(dbContext)
        {
        }

        public virtual async Task<List<Label>> GetAllAlphabetical()
        {
            return await GetAll().OrderBy(l => l.Name).ToListAsync();
        }
    }
}
