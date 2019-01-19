using System.Threading.Tasks;
using Tuneage.Data.Repositories.Sql.EfCore;

namespace Tuneage.Data.TestData
{
    public class DataSeeder
    {
        private readonly ILabelRepository _labelRepository;

        public DataSeeder(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task Seed()
        {
            foreach (var label in TestDataGraph.Labels.LabelsRaw)
            {
                await _labelRepository.Create(label);
            }
        }
    }
}
