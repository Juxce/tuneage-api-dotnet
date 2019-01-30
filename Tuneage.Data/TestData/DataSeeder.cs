using System.Threading.Tasks;
using Tuneage.Data.Repositories.Sql.EfCore;

namespace Tuneage.Data.TestData
{
    public class DataSeeder
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IArtistRepository _artistRepository;

        public DataSeeder(ILabelRepository labelRepository, IArtistRepository artistRepository)
        {
            _labelRepository = labelRepository;
            _artistRepository = artistRepository;
        }

        public async Task Seed(bool isIntegrationTest)
        {
            bool removeIdsSoValuesCanBeGenerated = !isIntegrationTest;

            foreach (var label in TestDataGraph.Labels.LabelsRaw)
            {
                if (removeIdsSoValuesCanBeGenerated) label.LabelId = 0;
                await _labelRepository.Create(label);
            }

            foreach (var artist in TestDataGraph.Artists.ArtistsRaw)
            {
                if (removeIdsSoValuesCanBeGenerated) artist.ArtistId = 0;
                await _artistRepository.Create(artist);
            }
        }
    }
}
